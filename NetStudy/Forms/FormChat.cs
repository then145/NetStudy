using Microsoft.AspNetCore.SignalR.Client;
using NetStudy.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace NetStudy.Forms
{
    public partial class FormChat : Form
    {
        private HubConnection _connection;
        private string _accessToken;
        private string _username;
        private HttpClient _httpClient;
        private Panel _selectedPanel;
        private Timer _timer;

        public FormChat(string accessToken, string username)
        {
            InitializeComponent();
            _accessToken = accessToken;
            _username = username;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7070/") };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            ConnectToServer();
            CustomizeGroupBox();
            LoadFriends();

            textBox_msg.Text = "Nhập tin nhắn...";
            textBox_msg.ForeColor = Color.Gray;
            textBox_msg.Enabled = false;

            textBox_msg.GotFocus += RemovePlaceholderText;
            textBox_msg.LostFocus += SetPlaceholderText;

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        private async void FormChat_Load(object sender, EventArgs e)
        {
        }

        private async void ConnectToServer()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7070/chatHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_accessToken);
                })
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Invoke((Action)(() =>
                {
                    textBox_showmsg.AppendText($"{user}: {message}{Environment.NewLine}");
                }));
            });

            await _connection.StartAsync();
        }

        private async void LoadFriends()
        {
            var response = await _httpClient.GetAsync($"api/singlechat/get-friend-list/{_username}");
            var jsonString = await response.Content.ReadAsStringAsync();
            //MessageBox.Show(jsonString);

            var jsonObject = JsonDocument.Parse(jsonString);
            var friends = jsonObject.RootElement.GetProperty("data").EnumerateArray().Select(x => x.GetString()).ToList();
            DisplayFriends(friends);
        }

        private void DisplayFriends(List<string> friends)
        {
            if (friends == null || friends.Count == 0)
            {
                MessageBox.Show("Không tìm thấy người nào trong danh sách bạn bè của bạn.");
                return;
            }

            int yOffset = 30;
            int panelHeight = 50;

            var totalLabel = new Label
            {
                Text = $"Số lượng bạn bè: {friends.Count}",
                ForeColor = Color.FromArgb(255, 255, 255),
                AutoSize = true,
                Location = new Point(10, yOffset),
                Font = new Font("Arial", 12)
            };
            groupBox_doanchat.Controls.Add(totalLabel);

            yOffset += panelHeight;

            foreach (var friend in friends)
            {
                var panel = new Panel
                {
                    Size = new Size(350, panelHeight),
                    Location = new Point(10, yOffset),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.Indigo,
                    Tag = friend
                };

                var label = new Label
                {
                    Text = friend,
                    ForeColor = Color.FromArgb(255, 255, 255),
                    AutoSize = true,
                    Location = new Point(10, 15),
                    Font = new Font("Arial", 12)
                };
                panel.Controls.Add(label);

                panel.Click += async (s, e) => await SelectFriendPanel(panel);

                groupBox_doanchat.Controls.Add(panel);

                yOffset += panelHeight + 5;
            }
        }

        private async Task SelectFriendPanel(Panel panel)
        {
            if (_selectedPanel != null)
            {
                _selectedPanel.BackColor = Color.Indigo;
                foreach (Control control in _selectedPanel.Controls)
                {
                    if (control is Label label)
                    {
                        label.ForeColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }

            _selectedPanel = panel;
            _selectedPanel.BackColor = Color.FromArgb(50, 255, 255);
            foreach (Control control in _selectedPanel.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = Color.Indigo;
                }
            }

            var friend = panel.Tag.ToString();
            await LoadChatHistory(friend);
            textBox_msg.Enabled = true;

            _timer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _timer.Stop();
            base.OnFormClosing(e);
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (_selectedPanel != null && !this.IsDisposed)
            {
                var friend = _selectedPanel.Tag.ToString();
                await LoadChatHistory(friend);
            }
        }

        private async Task LoadChatHistory(string friend)
        {
            var response = await _httpClient.GetAsync($"api/singlechat/history/{_username}/{friend}");
            var chatHistory = await response.Content.ReadFromJsonAsync<List<SingleChat>>();

            if (!this.IsDisposed && textBox_showmsg != null && !textBox_showmsg.IsDisposed)
            {
                textBox_showmsg.Clear();
                foreach (var chat in chatHistory)
                {
                    textBox_showmsg.AppendText($"{chat.Sender}: {chat.Message}{Environment.NewLine}");
                }
            }
        }

        private async void button_send_Click(object sender, EventArgs e)
        {
            if (_selectedPanel == null)
            {
                MessageBox.Show("Vui lòng chọn một bạn bè để chat.");
                return;
            }

            var message = textBox_msg.Text;
            var receiver = _selectedPanel.Tag.ToString();
            await _connection.InvokeAsync("SendMessage", _username, receiver, message);
            textBox_msg.Clear();
        }

        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            if (textBox_msg.Text == "Nhập tin nhắn...")
            {
                textBox_msg.Text = "";
                textBox_msg.ForeColor = Color.White;
            }
        }

        private void SetPlaceholderText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_msg.Text))
            {
                textBox_msg.Text = "Nhập tin nhắn...";
                textBox_msg.ForeColor = Color.Gray;
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