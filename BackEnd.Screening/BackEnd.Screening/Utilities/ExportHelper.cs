using BackEnd.Screening.Models;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace BackEnd.Screening.Utilities
{
    public class ExportHelper
    {        
        private static readonly string _exportPath = Path.Combine(SpecialDirectories.Desktop, "ExportedJsonFiles");
        public static void ExportCarList(List<Car> data, string fileName)
        {
            string path = Path.Combine(_exportPath, $"{fileName}.json");
            string json = JsonConvert.SerializeObject(data.ToArray(), Formatting.Indented);    
            if (!File.Exists(_exportPath))
            {
                Directory.CreateDirectory(_exportPath);
            }
            File.WriteAllText(path, json);

        }
        public static void ExportCarDetail(object carDetailData, string fileName)
        {
            string path = Path.Combine(_exportPath, $"{fileName}.json");
            string json = JsonConvert.SerializeObject(carDetailData, Formatting.Indented);
            if (!File.Exists(_exportPath))
            {
                Directory.CreateDirectory(_exportPath);
            }
            File.WriteAllText(path, json);
        }
    }
}
