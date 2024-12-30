using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetStudy.Models;
using NetStudy.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Generators;

namespace NetStudy
{
    public partial class ResetPassword : Form
    {
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private RsaService rsaService;
        private AesService aesService;
        public ResetPassword(string email)
        {
            InitializeComponent();
            tB_email.Text = email;
            tB_email.Enabled = false;
            rsaService = new RsaService();
            aesService = new AesService();
        }

        private void ResetPassword_Load(object sender, EventArgs e)
        {

        }

        private async void btn_confirm_Click(object sender, EventArgs e)
        {
            string email = tB_email.Text.Trim();
            string otp = tB_otp.Text.Trim();
            string newPassword = tB_pass.Text;
            string confirmPassword = tB_conPass.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(otp) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                var (publicKey, privateKey) = rsaService.GenerateKeys();
                string salt;
                var enKey = aesService.EncryptPrivateKey(privateKey, newPassword, out salt);

                var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                var request = new ResetPasswordRequest
                {
                    Email = email,
                    Otp = otp,
                    NewPassword = hashedNewPassword,
                    PublicKey = publicKey,
                    PrivateKey = enKey,
                    Salt = salt
                };
                
                string resultMessage = await ResetUserPassword(request);

                if(resultMessage.Contains("thành công"))
                {
                    MessageBox.Show(resultMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(resultMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> ResetUserPassword(ResetPasswordRequest request)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/user/reset-password", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var msg = JsonObject.Parse(responseContent)["message"].ToString();

                if (response.IsSuccessStatusCode)
                {
                    return "Đổi mật khẩu thành công!";
                }
                else
                {
                    return $"Lỗi: {msg}";
                }
            }
            catch (Exception ex)
            {
                return $"Lỗi khi kết nối đến API: {ex.Message}";
            }
        }

    }
}
