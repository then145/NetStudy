namespace API_Server.Models
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Otp { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Salt { get; set; }

    }
}
