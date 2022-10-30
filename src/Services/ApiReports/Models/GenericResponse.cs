namespace ApiReports.Models
{
    public class GenericResponse
    {
        public string message { get; set; }
        public string mimeType { get; set; }
        public string fileName { get; set; }
        public byte[] payload { get; set; }
    }
}