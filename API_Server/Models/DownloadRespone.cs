namespace API_Server.Models
{
    public class DownloadResponse
    {
        public string DocumentId { get; set; }
        public string Username { get; set; }
        public DateTime DownloadedAt { get; set; }
    }
}
