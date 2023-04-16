using BackEnd.Screening.Models;
using System.Text.RegularExpressions;

namespace BackEnd.Screening.Converters
{
    public static class ModelExtension
    {
        public static List<Car> ConvertToCarList(this List<SearchResult> searchResults)
        {
            var carList = new List<Car>();
            foreach (var item in searchResults)
            {
                Car car = new Car();
                string[] rawCarInfo = RemoveUnusedItems(item.CarInfo.Trim().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList()).ToArray();
            
                car.StockType = rawCarInfo.GetStockType();
                car.Make = rawCarInfo.GetMake();
                car.Model = rawCarInfo.GetModel();                
                car.Year = rawCarInfo.GetYear();
                car.Mile = rawCarInfo.GetMile();
                car.Price=rawCarInfo.GetPrice();
                car.MonthlyPayment = rawCarInfo.GetMonthly();
                car.DealType = rawCarInfo.GetDealType();
                car.Seller = rawCarInfo.GetSeller();
                car.Ratings = rawCarInfo.GetRatings();
                car.Reviews = rawCarInfo.GetRewiewCount();
                if (rawCarInfo.Length == 10)
                {
                    car.Distance = rawCarInfo.GetDistance();
                    car.Zip =rawCarInfo.GetZip();
                }                
                
                car.carThumbnails = item.CarImages;
                carList.Add(car);
            }
            return carList;
        }
        #region ExtensionHelpers.
        private static double GetPrice(this string[] info)
        {
            return double.Parse(info[3].Split()[0].Substring(1));
        }
        private static string GetStockType(this string[] info)
        {
            return info[0];
        }
        private static int GetYear(this string[] info)
        {
            return Int32.Parse(info[1].Substring(0, 4));
        }

        private static string GetMake(this string[] info)
        {
            return info[1].Substring(4, 6).Trim();
        }
        private static string GetModel(this string[] info)
        {
            return info[1].Substring(10).Trim();
        }
        private static string GetSeller(this string[] info)
        {
            int index = info.Length > 8 ? 6 : 5;
            return info[index];
        }
        private static double? GetMonthly(this string[] info)
        {
            if (info[4].Contains("/mo*"))
            {
                return double.Parse(info[4].Trim().Replace("/mo*", "").Substring(1));
            }
            return null;
        }
        private static decimal GetRatings(this string[] info)
        {
            int index = info.Length > 8 ? 7 : 6;
            decimal ratings = 0;
            if (info[index] == "Not rated")
            {
                return ratings;
            }
            else
            {
                decimal.TryParse(info[index].Replace(".", ","), out ratings);
                return ratings;
            }
        }
        private static int GetRewiewCount(this string[] info)
        {
            int index = info.Length > 8 ? 8 : 7;
            return Int32.Parse(Regex.Match(info[index], @"\d+").Value);
        }
        private static int GetMile(this string[] info)
        {
            return Int32.Parse(Regex.Match(info[2].Trim().Replace(",", ""), @"\d+").Value);
        }
        private static int GetDistance(this string[] info)
        {
            return Int32.Parse(info.Last().Replace(" mi. from ", " ").Split()[0].Replace(",", ""));
        }
        private static string GetZip(this string[] info)
        {
            return info.Last().Replace(" mi. from ", " ").Split()[1];
        }
        private static string GetDealType(this string[] info)
        {
            int index = info.Length > 8 ? 5 : 4;
            return info[index].Split(" | ")[0];
        }
        private static List<string> RemoveUnusedItems(List<string> infoList)
        {
            string[] items = new string[] { "Save", "Free CARFAX Report", "Hot Car", "Free CARFAX 1-Owner Report", "Free AutoCheck Report", "Get the AutoCheck Report", "Check availability", "View on dealer's site", "Online seller" };
            infoList.RemoveAll(x => x.StartsWith("/vehicledetail") || x.StartsWith("View all "));
            foreach (string item in items)
            {
                infoList.Remove(item);
            }
            return infoList;
        }
        #endregion

    }
}
