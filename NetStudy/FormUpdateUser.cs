using NetStudy.Models;
using NetStudy.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NetStudy
{
    public partial class FormUpdateUser : Form
    {
        private UpdateUserDTO updateUserDTO = new UpdateUserDTO();
        private readonly UserService userService;
        private readonly RsaService rsaService;
        private readonly AesService aesService;
        private string accessToken;
        private string avatarPath = "";
        private readonly JObject UserInfo;
        public static HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
        };
        public FormUpdateUser(string token, JObject info)
        {
            InitializeComponent();
            accessToken = token;
            userService = new UserService(accessToken);
            rsaService = new RsaService();
            aesService = new AesService();
            UserInfo = info;
        }

        private byte[] ReadFileAsByteArray(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        private void FormUpdateUser_Load(object sender, EventArgs e)
        {
            SetInfo(UserInfo["username"].ToString());
            DrawCircularImage(avaPic);
        }

        private async void LoadImage(string imgUrl)
        {
            try
            {

                if (string.IsNullOrEmpty(imgUrl) ||
                !Uri.TryCreate(imgUrl, UriKind.Absolute, out var uriRes) ||
                !(uriRes.Scheme == Uri.UriSchemeHttp || uriRes.Scheme == Uri.UriSchemeHttps))
                {
                    avaPic.Image = LoadDefaultImg();
                    return;
                }
                var imageBytes = await httpClient.GetByteArrayAsync(imgUrl);
                using (var ms = new MemoryStream(imageBytes))
                {
                    if (ms != null && ms.CanRead)
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        Image image = Image.FromStream(ms);
                        avaPic.Image = image;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image LoadDefaultImg()
        {
            string defaultUrl = "https://i.pinimg.com/736x/62/ee/b3/62eeb37155f0df95a708586aed9165c5.jpg";
            using (var client = new HttpClient())
            {
                var bytes = client.GetByteArrayAsync(defaultUrl).Result;
                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
        }
        private async void SetInfo(string username)
        {
            try
            {
                var user = await userService.GetUserByUsername(username);
                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy người dùng");
                }
                txtFullName.Text = user.Name;
                txtUsername.Text = user.Username;
                txtEmail.Text = user.Email;
                if (!string.IsNullOrEmpty(user.Avatar))
                {

                    LoadImage(user.Avatar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DrawCircularImage(PictureBox pictureBox)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Region = new Region(path);
        }
        private void btnSelectImg_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    avatarPath = ofd.FileName;

                    avaPic.Image = Image.FromFile(avatarPath);
                }
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc muốn cập nhật các thông tin này ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var response = await userService.UpdateUserInfo(txtUsername.Text, txtFullName.Text, txtEmail.Text, avatarPath);
                    var message = response["message"].ToString();

                    if (message.Contains("thành công", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!response.ContainsKey("userUpdated"))
                        {
                            MessageBox.Show($"Lỗi: Không tìm thấy người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        var currUser = response["userUpdated"].ToObject<User>();
                        if (currUser != null)
                        {
                            SetInfo(currUser.Username);
                        }

                    }
                    MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void linkLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var res = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                var check = await userService.LogOut();
                if (check)
                {
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Đăng xuất thất bại!");
                }
            }
        }

        private async void linkChangePass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string username = UserInfo["username"].ToString();
            var result = await userService.SendRequestChangePassword(username);

            try
            {
                if (result)
                {
                    var (publicKey, privateKey) = rsaService.GenerateKeys();

                    
                    ChangePassword changePassword = new ChangePassword(username, accessToken, userService, publicKey , privateKey);
                    changePassword.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Gửi mã OTP thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormUpdateUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
