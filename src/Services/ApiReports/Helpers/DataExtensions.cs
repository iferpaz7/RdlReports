using Newtonsoft.Json;
using System.Data;

namespace ApiReports.Helpers
{
    public static class DataExtensions
    {
        public static DataTable JsonToDataTable(string json)
        {
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
            return dt;
        }
    }
}