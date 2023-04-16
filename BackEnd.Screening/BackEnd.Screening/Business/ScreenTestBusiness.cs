using BackEnd.Screening.Converters;
using BackEnd.Screening.Models;
using BackEnd.Screening.State;
using BackEnd.Screening.Utilities;
using CefSharp;
using CefSharp.OffScreen;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BackEnd.Screening.Business
{
    public class ScreenTestBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly ScriptBuilder _scriptBuilder;
        public ScreenTestBusiness(IConfiguration configuration, ScriptBuilder scriptBuilder)
        {
            _configuration = configuration;
            _scriptBuilder = scriptBuilder;
            Init();
        }
        private static ChromiumWebBrowser browser;



        #region Login
        private string Username { get; set; }
        private string Password { get; set; }
        #endregion
        #region Filters
        public string StockType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int MaxPrice { get; set; }
        public string Distance { get; set; }
        public int ZIP { get; set; }
        #endregion

        #region gatherin results
        List<SearchResult> Page1Results;
        List<SearchResult> Page2Results;
        List<SearchResult> AllResults;
        CarDetail CarDetailData;
        #endregion
        private string _searchResultURL = "https://www.cars.com/shopping/results/?stock_type=used&makes%5B%5D=tesla&models%5B%5D=tesla-model_s&list_price_max=100000&maximum_distance=all&zip=94596";

        private string Url { get; set; }
        private string CefCachePath { get; set; }
        public void StartTest()
        {
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            RemoveCacheFolder(); // remove the cache folder befor init CEF
            var settings = new CefSettings()
            {
                CachePath = CefCachePath
            };
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            browser = new ChromiumWebBrowser(Url);

            browser.LoadingStateChanged += BrowserLoadingStateChanged;
            browser.AddressChanged += BrowserAddressChanged;

            Console.ReadKey();
            Cef.Shutdown();
        }


        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                #region Login
                if (!ScreenTestStateManagement.IsLoggedIn && !ScreenTestStateManagement.IsLoginSuccessfully)
                {
                    Login();
                }
                if (ScreenTestStateManagement.RedirectToLogin && !ScreenTestStateManagement.IsLoginSuccessfully)
                {
                    CheckLoginStatus();
                }
                #endregion

                if (!ScreenTestStateManagement.IsModelSProcessCompleted)
                {
                    #region Filter  
                    if (!ScreenTestStateManagement.FilteredSearch)
                    {
                        FilterSearch();
                    }
                    #endregion
                    #region GetAllCars
                    else
                    {
                        GatherAllCarData();
                    }
                    #endregion
                    #region GetCarDetails
                    if (ScreenTestStateManagement.IsSecondPageGathered)
                    {
                        if (!ScreenTestStateManagement.IsCarDetailOpen)
                        {
                            SelectCar();
                        }
                        if (ScreenTestStateManagement.IsCarDetailOpen && !ScreenTestStateManagement.IsCarDetailGathered && !ScreenTestStateManagement.IsHomeDeliveryGathered)
                        {
                            GatherCarDetailData();
                        }
                    }
                    #endregion
                }
                if (ScreenTestStateManagement.IsModelSProcessCompleted && !ScreenTestStateManagement.IsModelXProcessCompleted) // Model X
                {
                    if (ScreenTestStateManagement.IsReadyToFilterModelX && !ScreenTestStateManagement.IsFilteredForModelX)
                    {
                        SelectModelX();
                    }
                    if (ScreenTestStateManagement.InModelXSearchResult)
                    {
                        #region GetAllCars                      
                        GatherAllCarData();
                        #endregion
                        #region GetCarDetails
                        if (ScreenTestStateManagement.IsSecondPageGathered)
                        {
                            if (!ScreenTestStateManagement.IsCarDetailOpen)
                            {
                                SelectCar();
                            }
                            if (ScreenTestStateManagement.IsCarDetailOpen && !ScreenTestStateManagement.IsCarDetailGathered)
                            {
                                GatherCarDetailData();
                            }
                        }
                        #endregion
                    }
                }

            }
            else
            {
                Console.WriteLine($"page {Url} is loading!");
            }
        }
        #region business flow.
        private void Login()
        {
            string openLoginScreenScript = _scriptBuilder.GenerateLoginScript(Username, Password);
            browser.EvaluateScriptAsync(openLoginScreenScript).ContinueWith(t =>
            {
                if (t.Result != null && t.Result.Success)
                {
                    ScreenTestStateManagement.IsLoggedIn = true;
                    ScreenTestStateManagement.RedirectToLogin = true;
                }
            });
        }
        private void CheckLoginStatus()
        {
            string checkLogin = _scriptBuilder.CheckLogin();
            browser.EvaluateScriptAsync(checkLogin).ContinueWith(x =>
            {
                if (x.Result != null && x.Result.Success)
                {
                    string expectedResult = "Hi, john g";
                    ScreenTestStateManagement.IsLoginSuccessfully = x.Result.Result.ToString() == expectedResult;
                }
            });
        }
        private void FilterSearch()
        {
            string filterString = _scriptBuilder.GenerateFilterString(StockType, Make, Model, MaxPrice, Distance, ZIP);
            browser.EvaluateScriptAsync(filterString).ContinueWith(x =>
            {
                if (x.Result != null && x.Result.Success)
                {
                    ScreenTestStateManagement.FilteredSearch = true;
                }
            });
        }
        private void GatherAllCarData()
        {
            string searchResultScript = _scriptBuilder.GenerateSearchResultsScript();
            if (!ScreenTestStateManagement.IsFirstPageGathered)
            {
                browser.EvaluateScriptAsync(searchResultScript).ContinueWith(x =>
                {
                    if (x.Result != null && x.Result.Success)
                    {
                        Page1Results = JsonConvert.DeserializeObject<List<SearchResult>>(JsonConvert.SerializeObject(x.Result.Result));
                        ScreenTestStateManagement.IsFirstPageGathered = true;
                        string goToSecondPage = _scriptBuilder.GenerateGoToSecondPageScript();
                        browser.EvaluateScriptAsync(goToSecondPage).ContinueWith(x =>
                        {
                            if (x.Result != null && x.Result.Success)
                            {
                                ScreenTestStateManagement.IsSecondPageWorked = true;
                            }
                        });
                    }
                });
            }
            if (ScreenTestStateManagement.IsPassedSecondPage && !ScreenTestStateManagement.IsSecondPageGathered)
            {
                browser.EvaluateScriptAsync(searchResultScript).ContinueWith(x =>
                {
                    if (x.Result != null && x.Result.Success)
                    {
                        Page2Results = JsonConvert.DeserializeObject<List<SearchResult>>(JsonConvert.SerializeObject(x.Result.Result));
                        AllResults = Page1Results.Concat(Page2Results).ToList();
                        var CarList = AllResults.ConvertToCarList();
                        string filename = !ScreenTestStateManagement.IsModelSProcessCompleted ? "model-s-search-results" : "model-x-search-results";
                        ExportHelper.ExportCarList(CarList, filename);
                        Console.WriteLine("Search results exported as JSON for first two page");
                        ScreenTestStateManagement.IsSecondPageGathered = true;
                    }
                });
            }

        }
        private void SelectCar()
        {
            string selectCarScript = _scriptBuilder.GenerateSelectCarScript();
            browser.EvaluateScriptAsync(selectCarScript).ContinueWith(x =>
            {
                if (x.Result != null && x.Result.Success)
                {
                    Console.WriteLine(x);
                }
            });
        }
        private void GatherCarDetailData()
        {
            string gatherCarDetailScript = _scriptBuilder.GenerateGatherCarDetailScript();
            browser.EvaluateScriptAsync(gatherCarDetailScript).ContinueWith(x =>
            {
                if (x.Result != null && x.Result.Success)
                {
                    CarDetailData = JsonConvert.DeserializeObject<CarDetail>(JsonConvert.SerializeObject(x.Result.Result));
                    ScreenTestStateManagement.IsCarDetailGathered = true;
                    string clickHomeDelivery = _scriptBuilder.ClickHomeDeliveryButton();
                    var result = browser.EvaluateScriptAsync(clickHomeDelivery).ContinueWith(x =>
                    {
                        if (x.Result != null && x.Result.Success)
                        {
                            string gatherHomeDeliveyData = _scriptBuilder.GatherHomeToDeliveryData();
                            browser.EvaluateScriptAsync(gatherHomeDeliveyData).ContinueWith(x =>
                            {
                                if (x.Result != null && x.Result.Success)
                                {
                                    CarDetailData.HomeDelivey = JsonConvert.DeserializeObject<List<HomeDelivey>>(JsonConvert.SerializeObject(x.Result.Result));
                                    string filename = !ScreenTestStateManagement.IsModelSProcessCompleted ? "model-s-car-detail" : "model-x-car-detail";
                                    ExportHelper.ExportCarDetail(CarDetailData, filename);
                                    ScreenTestStateManagement.IsHomeDeliveryGathered = true;
                                    if (ScreenTestStateManagement.SearcResultLoadCount == 2)
                                    {
                                        ScreenTestStateManagement.IsModelXProcessCompleted = true;
                                    }
                                    else
                                    {
                                        ScreenTestStateManagement.IsModelSProcessCompleted = true;
                                        RedirecToSearchResult();
                                    }
                                }
                            });
                        }
                    });
                }
            });
        }
        private void RedirecToSearchResult()
        {
            browser.LoadUrl(_searchResultURL);
        }
        private void SelectModelX()
        {
            string selectModelX = _scriptBuilder.GenerateSelectModelXScript();
            var filterModelXTask = browser.EvaluateScriptAsync(selectModelX).ContinueWith(x =>
            {
                if (x.Result != null && x.Result.Success)
                {
                    ScreenTestStateManagement.ResetGatherinStates();
                }
            });
        }
        #endregion

        private void BrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            string expectedPage2Url = "https://www.cars.com/shopping/results/?list_price_max=100000&makes[]=tesla&maximum_distance=all&models[]=tesla-model_s&page=2&stock_type=used&zip=94596";
            string expectedSearchResultUrl = "https://www.cars.com/shopping/results/?stock_type=used&makes%5B%5D=tesla&models%5B%5D=tesla-model_s&list_price_max=100000&maximum_distance=all&zip=94596";
            string modelXSearchResultUrl = "https://www.cars.com/shopping/results/?dealer_id=&keyword=&list_price_max=100000&list_price_min=&makes[]=tesla&maximum_distance=all&mileage_max=&models[]=tesla-model_s&models[]=tesla-model_x&page_size=20&sort=best_match_desc&stock_type=used&year_max=&year_min=&zip=94596";
            if (e.Address == expectedSearchResultUrl)
            {
                ScreenTestStateManagement.FilteredSearch = true;
                ScreenTestStateManagement.SearcResultLoadCount++;
                if (ScreenTestStateManagement.SearcResultLoadCount == 2) // search for model x
                {
                    ScreenTestStateManagement.IsReadyToFilterModelX = true;
                }
            }
            if (e.Address.Contains("page=2"))
            {
                ScreenTestStateManagement.IsPassedSecondPage = true;
            }
            if (e.Address.Contains("vehicledetail"))
            {
                ScreenTestStateManagement.IsCarDetailOpen = true;
            }
            if (e.Address == modelXSearchResultUrl)
            {
                ScreenTestStateManagement.InModelXSearchResult = true;
            }
        }
        #region init.
        private void Init()
        {
            Username = _configuration.GetValue<string>("UserCredential:Username");
            Password = _configuration.GetValue<string>("UserCredential:Password");
            StockType = _configuration.GetValue<string>("SearchCriteria:StockType");
            Make = _configuration.GetValue<string>("SearchCriteria:Make");
            Model = _configuration.GetValue<string>("SearchCriteria:Model");
            MaxPrice = _configuration.GetValue<int>("SearchCriteria:MaxPrice");
            Distance = _configuration.GetValue<string>("SearchCriteria:Distance");
            ZIP = _configuration.GetValue<int>("SearchCriteria:ZIP");
            Url = _configuration.GetValue<string>("Url");
            CefCachePath = Path.Combine(Environment.GetFolderPath(
                                         Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache");
        }
        private void RemoveCacheFolder()
        {
            if (Directory.Exists(CefCachePath))
            {
                Directory.Delete(CefCachePath, true);
            }
        }
        #endregion
    }
}
