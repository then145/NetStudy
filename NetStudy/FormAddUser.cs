using iTextSharp.text.pdf;
using Microsoft.AspNetCore.SignalR.Client;
using NetStudy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetStudy
{
    public partial class FormAddUser : Form
    {

        private readonly GroupService groupService;
        private string accessToken;
        private string groupName;
        private string groupId;
        private HubConnection connection;
        private string name;
        private DateTime timeStamp = DateTime.Now;
        private string roleUser;
        private readonly UserService userService;
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private readonly AesService aesService;
        private string aesKey;
        public FormAddUser(string token, string grName, string id, string username, string role, string key)
        {
            InitializeComponent();
            accessToken = token;
            groupName = grName;
            groupId = id;
            lblName.Text = groupName;
            groupService = new GroupService(accessToken);
            userService = new UserService(accessToken);
            aesService = new AesService();
            name = username;
            comboRole.Items.Add("User");
            if (role == "001" || role == "003")
            {
                comboRole.Items.Add("Admin");
            }
            aesKey = key;
            
            comboRole.SelectedIndex = 0;
            roleUser = role;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7070/groupChatHub", opts =>
                {
                    opts.AccessTokenProvider = async () => await Task.FromResult(accessToken);
                })
                .Build();

        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var roleSelected = comboRole.SelectedItem.ToString();
            var user = await userService.GetUserByUsername(username);
            if(user == null)
            {
                MessageBox.Show("Người dùng không tồn tại!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }    

            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrEmpty(roleSelected))
            {
                MessageBox.Show("Vui lòng điền tên người dùng và vai trò trong nhóm!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (roleSelected != "Admin" && roleSelected != "User")
            {
                MessageBox.Show("Vai trò không tồn tại!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(roleUser == "002")
            {
                var checkUser = await groupService.AddUserToGroupRequest(groupId, username, user.Name);
                return;
            }    
           
            var check = await groupService.AddUserToGroup(groupId, username, roleSelected, user.Name);
            if(check)
            {
                var content = aesService.EncryptMessage($"{name} đã thêm {username} là {roleSelected}", aesKey);
                await connection.InvokeAsync("SendMessageGroup", groupId, "Thông báo", content);
              
            }    
            
        }

        private async void FormAddUser_Load(object sender, EventArgs e)
        {
            await connection.StartAsync();
        }
    }
}
