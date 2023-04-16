namespace BackEnd.Screening.Models
{
    public class CarDetail
    {
        public string AdditionalFeatures { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public DealStatus DealStatus { get; set; }
        public FeatureInfo FeatureInfo { get; set; }
        public List<HomeDelivey> HomeDelivey { get; set; }
        public string Make { get; set; }
        public int Miles { get; set; }
        public string Model { get; set; }
        public int MonthlyPrice { get; set; }
        public int Price { get; set; }
        public int PriceDrop { get; set; }
        public List<PriceHistoryList> PriceHistoryList { get; set; }
        public SellerInfo SellerInfo { get; set; }
        public string StockType { get; set; }
        public VehicleHistory VehicleHistory { get; set; }
        public int Year { get; set; }
    }

    public class BasicInfo
    {
        public string Drivetrain { get; set; }
        public string Engine { get; set; }
        public string ExteriorColor { get; set; }
        public string FuelType { get; set; }
        public string InteriorColor { get; set; }
        public string Mileage { get; set; }
        public string Stock { get; set; }
        public string Transmission { get; set; }
        public string VIN { get; set; }
    }

    public class DealStatus
    {
        public string DealType { get; set; }
        public string PriceSavings { get; set; }
    }

    public class FeatureInfo
    {
        public List<string> Convenience { get; set; }
        public List<string> Entertainment { get; set; }
        public List<string> Exterior { get; set; }
        public List<string> Safety { get; set; }
        public List<string> Seating { get; set; }
    }

    public class HomeDelivey
    {
        public string Badge { get; set; }
        public string Text { get; set; }
    }

    public class PriceHistoryList
    {
        public string Date { get; set; }
        public string Price { get; set; }
        public string PriceChange { get; set; }
    }
    

    public class SellerInfo
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Rating { get; set; }
        public string ReviewCount { get; set; }
    }

    public class VehicleHistory
    {
        public string AccidentsOrDamage { get; set; }
        public string CleanTitle { get; set; }
        public string FirstOwnerVehicle { get; set; }
        public string PersonalUseOnly { get; set; }
        public string ReportedBy { get; set; }
    }
}
