﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetStudy.Models;

namespace NetStudy.Forms
{
    public partial class FormChat : Form
    {
        private HubConnection _hubConnection;
        private string _currentUser;
        private string _currentChatUser;
        private string _accessToken;

        public FormChat(string accessToken, string username)
        {
            InitializeComponent();
            CustomizeGroupBox();
            _accessToken = accessToken;
            _currentUser = username;
            textBox_myusrname.Text = _currentUser;
            InitializeSignalR();
            LoadFriends();
            comboBox_mystatus.SelectedItem = "Đang hoạt động";
        }

        private async void FormChat_Load(object sender, EventArgs e)
        {
            await LoadChatHistory();
        }

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7070/chatHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_accessToken);
                })
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Invoke((Action)(() =>
                {
                    var timestamp = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
                    textBox_showmsg.AppendText($"{timestamp}: {user}: {message}{Environment.NewLine}");
                }));
            });

            await _hubConnection.StartAsync();
        }

        private async void button_send_Click(object sender, EventArgs e)
        {
            var message = textBox_msg.Text;
            var localTime = DateTime.UtcNow.AddHours(7);
            var timestamp = localTime.ToString("HH:mm:ss - dd/MM/yyyy");
            textBox_showmsg.AppendText($"{timestamp}: {_currentUser}: {message}{Environment.NewLine}");
            await _hubConnection.InvokeAsync("SendMessage", _currentUser, _currentChatUser, message);
            textBox_msg.Clear();
        }

        private async Task LoadChatHistory()
        {
            if (string.IsNullOrEmpty(_currentChatUser))
            {
                return;
            }

            var messages = await _hubConnection.InvokeAsync<List<SingleChat>>("GetChatHistory", _currentUser, _currentChatUser);

            if (messages == null || messages.Count == 0)
            {
                MessageBox.Show("Đây là đoạn chat mới chưa có tin nhắn nào.");
                return;
            }

            foreach (var message in messages)
            {
                var localTime = message.Timestamp.AddHours(7);
                var timestamp = localTime.ToString("HH:mm:ss - dd/MM/yyyy");
                textBox_showmsg.AppendText($"{timestamp}: {message.Sender}: {message.Content}{Environment.NewLine}");
            }
        }

        private async void LoadFriends()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                var url = $"https://localhost:7070/api/user/get-friend-list/{_currentUser}";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return;
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                //MessageBox.Show(responseBody);

                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
                var friends = jsonResponse.GetProperty("data").EnumerateArray().Select(f => f.GetString()).ToList();
                var totalFriends = jsonResponse.GetProperty("total").GetInt32();

                if (friends == null || friends.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy người nào trong danh sách bạn bè của bạn.");
                    return;
                }

                int yOffset = 30;
                int labelHeight = 45;

                var totalLabel = new Label
                {
                    Text = $"Số lượng bạn bè: {totalFriends}",
                    ForeColor = Color.FromArgb(255, 255, 255),
                    AutoSize = true,
                    Location = new Point(10, yOffset),
                    Font = new Font("Arial", 12)
                };
                groupBox_doanchat.Controls.Add(totalLabel);

                yOffset += labelHeight;

                foreach (var friend in friends)
                {
                    var friendStatus = await GetFriendStatus(friend);
                    var labelColor = friendStatus ? Color.FromArgb(0, 255, 0) : Color.FromArgb(255, 255, 255);

                    var label = new Label
                    {
                        Text = friend,
                        ForeColor = labelColor,
                        AutoSize = true,
                        Location = new Point(10, yOffset),
                        Font = new Font("Arial", 12)
                    };
                    label.Click += (s, e) => LoadChatWithUser(_currentUser, friend);
                    groupBox_doanchat.Controls.Add(label);

                    yOffset += labelHeight;
                }
            }
        }

        private async Task<bool> GetFriendStatus(string username)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                var url = $"https://localhost:7070/api/user/get-status/{username}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
                    return jsonResponse.GetProperty("opStatus").GetBoolean();
                }
                return false;
            }
        }

        private async void LoadChatWithUser(string username, string friend)
        {
            _currentChatUser = friend;
            textBox_otherusrname.Text = friend;
            textBox_showmsg.Clear();
            await LoadChatHistory();

            var friendStatus = await GetFriendStatus(friend);
            textBox_otherstatus.Text = friendStatus ? "Đang hoạt động" : "Không hoạt động";
        }

        private async void comboBox_mystatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var statusString = comboBox_mystatus.SelectedItem.ToString();
            bool status = statusString == "Đang hoạt động";
            await UpdateUserStatus(status);
        }

        private async Task UpdateUserStatus(bool opstatus)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                var url = $"https://localhost:7070/api/user/updateStatus";
                var content = new StringContent(JsonSerializer.Serialize(new { Username = _currentUser, OpStatus = opstatus }), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        private void CustomizeGroupBox()
        {
            groupBox_doanchat.FlatStyle = FlatStyle.Flat;
            groupBox_doanchat.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, groupBox_doanchat.ClientRectangle, System.Drawing.Color.Black, ButtonBorderStyle.Solid);
            };
        }

        private void textBox_msg_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox_doanchat_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}