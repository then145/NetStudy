using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetStudy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetStudy.Services
{
    
    public class UserService
    {
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private string accessToken;
        public UserService()
        {

        }
        public UserService(string token) {
            accessToken = token;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }    

        public async Task<string> Login(dynamic loginModel)
        {
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/user/login", content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> LogOut()
        {
            var response = await httpClient.PostAsync("api/user/logout", null);
            var res = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        private async void LoadImage(string imgUrl, PictureBox avaPic)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(imgUrl) ||
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

        public async Task SetUserInfo(string userName, string avatar, Label lbl, PictureBox avaPic)
        {
            lbl.Text = userName;
            LoadImage(avatar, avaPic);
        }
        public async Task<JObject> UpdateUserInfo(string username, string name, string email, string file)
        {
            try
            {
                using(var form = new MultipartFormDataContent())
                {
                    if(!string.IsNullOrEmpty(username))
                    {
                        form.Add(new StringContent(username), "username");
                    }    
                    if(!string.IsNullOrEmpty(name))
                    {
                        form.Add(new StringContent(name), "name");
                    }
                    if (!string.IsNullOrEmpty(email))
                    {
                        form.Add(new StringContent(email), "email");
                    }
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                        var content = new StreamContent(stream);
                        content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                        form.Add(content, "avatar", Path.GetFileName(file));
                    }
                    else
                    {
                        form.Add(new StreamContent(Stream.Null), "avatar", "current image");
                    }
                    var response = await httpClient.PatchAsync("api/user/update-info", form);
                    var res = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JObject info = JObject.Parse(res);
                        return info;
                    }
                    else
                    {
                        throw new Exception($"{response.StatusCode} - {res}");
                    }
                }
            }
            
            catch (JsonReaderException)
            {
                throw new Exception("Phản hồi từ server không phải JSON hợp lệ.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi: {ex.Message}");
            }
        }

        public async Task<bool> SendFriendRequest(string username,string targetUsername,Button btnFriend, string token)
        {
            
            try
            {
                var response = await httpClient.PostAsync($"api/user/{username}/add-friend/{targetUsername}", null);
                if(response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Gửi yêu cầu kết bạn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnFriend.Text = "Đã gửi";
                    btnFriend.Enabled = false;
                    btnFriend.BackColor = Color.LightYellow;
                    return true;
                }    
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi: {error}", "Thông báo 123", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi yêu cầu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/user/{username}");
                var res = await response.Content.ReadAsStringAsync();
                JObject userInfo = JObject.Parse(res);

                if (response.IsSuccessStatusCode)
                {
                    
                    var user = new User
                    {
                        Name = userInfo["userFound"]["name"].ToString(),
                        Username = userInfo["userFound"]["username"].ToString(),
                        Email = userInfo["userFound"]["email"].ToString(),
                        Avatar = userInfo["userFound"]["avatar"].ToString(),
                        PublicKey = userInfo["userFound"]["publicKey"].ToString()
                    };
                    return user;
                }
                else
                {
                    var error = userInfo["message"].ToString();
                    MessageBox.Show(error, "Error...123", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error...GetUser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<(List<string>,int)> GetReqList(string username, string token)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/user/get-request-list/{username}");
                var res = await response.Content.ReadAsStringAsync();
                var info = JObject.Parse(res);
                if (response.IsSuccessStatusCode)
                {
                    
                    List<string> reqList = info["data"].ToObject<List<string>>();
                    int total = int.Parse(info["total"].ToString());
                    return (reqList,total);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (new List<string>(), 0);
                }    
                else
                {
                    var error = info["message"].ToString();
                    MessageBox.Show(error, $"Thông báo: {response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (new List<string>(), 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error...GetReqList", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (new List<string>(), 0);
            }
        }

        public async Task AcceptRequest(string username, string requestUsername, Button btnAcptFriend, string accessToken)
        {
            try
            {
                var response = await httpClient.PostAsync($"api/user/{username}/accept-request/{requestUsername}", null);
                
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Chấp nhận kết bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAcptFriend.Text = "Đã chấp nhận";
                    btnAcptFriend.ForeColor = Color.Gainsboro;
                    return;
                }
                else
                {
                    MessageBox.Show("Không được rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"Lỗi khi gửi yêu cầu: {e.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
        }

        

        public async Task RemoveRequest(string username, string reqUsername, Button btn, string accessToken)
        {
            try
            {
                var res = await httpClient.DeleteAsync($"api/user/{username}/remove-request/{reqUsername}");
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đã xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btn.Text = "Đã xóa";
                    btn.ForeColor = Color.Gainsboro;
                    return;
                }
                else if(res.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                {
                    MessageBox.Show("Không xác thực được rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show($"Lỗi: {res.StatusCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Lỗi khi gửi yêu cầu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        } 
        
        public async Task DeleteFriend( string friendName)
        {
            try
            {
                var res = await httpClient.DeleteAsync($"api/user/delete-friend/{friendName}");
                var resp = await res.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(resp);
                var msg = obj["message"].ToString();
                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đã xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    
                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi yêu cầu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public async Task<bool> SendRequestChangePassword(string username)
        {
            try
            {
                var response = await httpClient.PostAsync($"api/user/{username}/request-change-password", null);
                var res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Mã xác nhận đã được gửi đến email của bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Email không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> ChangePasswordWithOtp(string username, string otp, string currentPassword, string newPassword, string confirmPassword, string publickey, string privatekey, string salt)
        {
            try
            {
                var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                var model = new
                {
                    Username = username,
                    Otp = otp,
                    CurrentPassword = currentPassword,
                    NewPassword = hashedNewPassword,
                    PublicKey = publickey,
                    Privatekey = privatekey,
                    Salt = salt
                };
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/user/change-password-with-otp", content);
                var res = await response.Content.ReadAsStringAsync();
                var msg = JObject.Parse(res)["message"].ToString();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!");
                    return true;
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



    }
}
