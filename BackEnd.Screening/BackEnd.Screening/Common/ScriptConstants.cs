namespace BackEnd.Screening.Common
{
    public class ScriptConstants
    {
        #region login
        public static string menuClick = "document.querySelector(\"body > div.global-header-container > cars-global-header > header > section > nav > button\").click();";
        public static string signInClick = "document.querySelector(\"body > div.global-header-container > cars-global-header\").shadowRoot.querySelector(\"spark-modal > div:nth-child(3) > div > spark-button:nth-child(1)\").shadowRoot.querySelector(\"button\").click();";
        public static string email = "document.querySelector('#auth-modal-email').value=\"{0}\";";
        public static string password = "document.querySelector(\"#auth-modal-current-password\").value=\"{0}\";";
        public static string loginClick = "document.querySelector(\"body > div.global-header-container > cars-global-header > cars-auth-modal\").shadowRoot.querySelector(\"spark-modal > form > spark-button\").shadowRoot.querySelector(\"button\").click();";
        public static string resultScript = "var result = document.querySelector(\"body > div.global-header-container > cars-global-header > header > section > nav > button > span.nav-user-name\").innerText;return result;";
        #endregion
        #region filter
        public static string stockTypeValueChange = "document.querySelector(\"#make-model-search-stocktype\").value='{0}';document.querySelector(\"#make-model-search-stocktype\").dispatchEvent(new Event('change'));";
        public static string makeTypeValueChange = "document.querySelector(\"#makes\").value='{0}';document.querySelector(\"#makes\").dispatchEvent(new Event('change'));";
        public static string modelTypeValueChange = "document.querySelector(\"#models\").value=\"{0}\";document.querySelector(\"#models\").dispatchEvent(new Event('change'));";
        public static string maxPriceValueChange = "document.querySelector(\"#make-model-max-price\").value=\"{0}\";document.querySelector(\"#make-model-max-price\").dispatchEvent(new Event('change'));";
        public static string maxDistanceValueChange = "document.querySelector('#make-model-maximum-distance').value=\"{0}\";document.querySelector(\"#make-model-maximum-distance\").dispatchEvent(new Event('change'));";
        public static string zipChange = "document.querySelector('#make-model-zip').value={0};";
        public static string searchButtonClick = "document.querySelector('#by-make-tab > div > div.sds-field.sds-home-search__submit > button').click();";
        #endregion
        #region searchResults
        public static string goToNPage = "document.querySelector('#\\\\31  > li:nth-child({0}) > a').click();";
        public static string selectModelX = "document.querySelector('#model > div > div:nth-child(4) > label').click()";
        #endregion
        #region select-a-car
        public static string selectModelYCar = "document.querySelector(\".vehicle-card > div > div.vehicle-details > a > h2\").click();";
        public static string clickHomeDelivery = "document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.vehicle-badging > div.sds-badge.sds-badge--home-delivery > span').click();";
        #endregion
        #region helperScripts
        public static string waitNSecond = "await new Promise(resolve => setTimeout(resolve, {0}));";
        #endregion
        #region large-scripts
        //They can read from .js file with FileReader.
        public static string GatherSearchResultScript = @"getData();
                    function getData(){
                        var result = gatherCarData(); 
                        return result;                     
                    };
                    function gatherCarData(){
                        var carElements = [...document.getElementsByClassName('vehicle-card')];    
                        let result=[];
                        for(let i = 0; i < carElements.length; i++){
                            var carImageElements = [...document.getElementsByClassName('vehicle-card')[i].getElementsByClassName('vehicle-image')];
                            let carImages =[];
                            carImageElements.forEach(x=>carImages.push(x.getAttribute('data-src')));
                            const carItem ={
                                carInfo: carElements[i].innerText,
                                carImages     
                            };
                            result.push(carItem);
                        }   
                        return result;  
                    }";
        public static string GatherHomeDeliveryData = @" 
(function(){
const homeDeliveryPopup = [...document.querySelectorAll('#sds-modal > div > div.sds-modal__content-body > ul > li > div')];
    let result = [];
    homeDeliveryPopup.forEach(x=>{
        const highlightNoteItem = {
            badge: x.getElementsByTagName('div')[0].innerText,
            text:  x.getElementsByTagName('p')[0].innerText
        };
        result.push(highlightNoteItem);
    });
return result
;})();";
        public static string GatherCarDetailScript = @"(async function(){
    await new Promise(resolve => setTimeout(resolve, 5000));
    const stockType = document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.title-section > p')?.innerText;
    const yearMakeModel = document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.title-section > h1')?.innerText;
    const make = yearMakeModel?.substring(5,10);
    const model = yearMakeModel?.substring(10);
    const year = Number(yearMakeModel?.substring(0,4));
    const miles = Number(document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.title-section > div.listing-mileage')?.innerText.replace(',','').match(/\d+/g)[0]);
    const price = Number(document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.price-section > span.primary-price')?.innerText.replace(',','').match(/\d+/g)[0]);
    const priceDrop = Number(document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.price-section > span.secondary-price.price-drop')?.innerText.match(/\d+/g)[0]);
    const monthlyPrice = Number(document.querySelector('#emp-tooltip-1 > span > a > span.js-estimated-monthly-payment-formatted-value-with-abr')?.innerText.replace(',','').match(/\d+/g)[0]);
    const deal = document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > section > header > div.vehicle-badging > div.sds-badge.sds-badge--icon > span')?.innerText.split(' | ');
    const dealStatus = {
        DealType: deal ? deal[0] : '',
        PriceSavings: deal?.length > 1 ? deal[1] : ''
    };
    const basicInfoList = [ ...document.querySelectorAll('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.sds-page-section.basics-section > dl > dd')];
    const basicInfo = {
        ExteriorColor: basicInfoList[0].innerText,
        InteriorColor:basicInfoList[1].innerText,
        Drivetrain:basicInfoList[2].innerText,        
        FuelType:basicInfoList[3].innerText,        
        Transmission:basicInfoList[4].innerText,        
        Engine:basicInfoList[5].innerText,        
        VIN:basicInfoList[6].innerText,        
        Stock:basicInfoList[7].innerText,        
        Mileage:basicInfoList[8].innerText
    };
    const featureList = [... document.querySelectorAll('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.sds-page-section.features-section > dl > dd')];
    const featureInfo = {
        Convenience: featureList[0].innerText.split('\n'),
        Entertainment: featureList[1].innerText.split('\n'),
        Exterior: featureList[2].innerText.split('\n'),
        Safety: featureList[3].innerText.split('\n'),
        Seating: featureList[4].innerText.split('\n'),
    };
    const additionalFeatures = document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.sds-page-section.features-section > div.auto-corrected-feature-wrapper > div > div.auto-corrected-feature-list')?.innerText;
    var vehichleHistoryList = [...document.querySelectorAll('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.sds-page-section.vehicle-history-section > dl > dd')];
    const vehicleHistory = {
        ReportedBy: document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.sds-page-section.vehicle-history-section > div.vehicle-history-header > h2').innerText.split(' ').pop(),
        AccidentsOrDamage: vehichleHistoryList[0]?.innerText,
        CleanTitle: vehichleHistoryList[1]?.innerText,
        FirstOwnerVehicle: vehichleHistoryList[2]?.innerText,
        PersonalUseOnly: vehichleHistoryList[3]?.innerText
    };
    const sellerInfo = {
        Name: document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.seller-info > h3')?.innerText,
        Rating: document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.seller-info > div.sds-rating > span')?.innerText,
        ReviewCount: document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.seller-info > div.sds-rating > a')?.innerText.replace(',','').match(/\d+/g)[0],
        Address:document.querySelector('#ae-skip-to-content > div.vdp-content-wrapper > div.basics-content-wrapper > section.seller-info > div:nth-child(4) > div')?.innerText
    };
    const priceHistoryList = [...document.querySelectorAll('#ae-skip-to-content > div.vdp-content-wrapper > div.price-history-container > cars-price-history > div.price-history > div > table > tbody> tr')];
    let priceHistories = [];
    priceHistoryList.forEach(h=>{
        const historyItem =
        {
            Date: h.getElementsByTagName('td')[0].innerText,
            PriceChange: h.getElementsByTagName('td')[1].innerText.trim(),
            Price: h.getElementsByTagName('td')[2].innerText
        };
        priceHistories.push(historyItem);
    });
    const result = {
        StockType: stockType,
        Make: make,
        Model: model,
        Year: year,
        Miles: miles,
        Price: price,
        PriceDrop: priceDrop ? priceDrop : 0,
        MonthlyPrice: monthlyPrice,
        DealStatus: dealStatus,
        BasicInfo: basicInfo,
        FeatureInfo: featureInfo,
        AdditionalFeatures: additionalFeatures ? additionalFeatures : '',
        VehicleHistory: vehicleHistory,
        SellerInfo: sellerInfo,
        PriceHistoryList: priceHistories       
    };
    return result;
})();";
        #endregion
    }
}
