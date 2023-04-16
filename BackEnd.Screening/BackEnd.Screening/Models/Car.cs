namespace BackEnd.Screening.Models
{
    public class Car
    {
        public string StockType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }        
        public int Mile { get; set; }
        public double Price { get; set; }
        public double? MonthlyPayment { get; set; }
        public string DealType { get; set; }
        public string Seller { get; set; }
        public decimal Ratings { get; set; }
        public double Reviews { get; set; }
        public double Distance { get; set; }
        public string Zip { get; set; }
               
        public string[] carThumbnails { get; set; }
    }
}
