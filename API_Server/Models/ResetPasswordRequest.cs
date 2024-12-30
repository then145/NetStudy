namespace API_Server.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Salt { get; set; }
    }

    public class ForgetPasswordRequest
    {
        public string Email { get; set; }
    }
}
