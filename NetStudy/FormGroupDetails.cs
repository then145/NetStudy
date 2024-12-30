using FontAwesome.Sharp;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using NetStudy.Models;
using NetStudy.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class FormGroupDetails : Form
    {
        private HubConnection connection;
        private string accessToken;
        private readonly JObject UserInfo;
        private readonly JObject GroupInfo;
        private string groupId;
        private readonly GroupService groupService;
        private readonly RsaService rsaService;
        private readonly AesService aesService;
        private readonly List<MemberRole> members;
        private List<string> memReq;
        private string roleUser;
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private string _key;
        private string aesKey;  
        public FormGroupDetails(string token, JObject info, JObject groupInfo, string key)
        {
            InitializeComponent();
            btnSend.Enabled = false;
            accessToken = token;
            UserInfo = info;
            GroupInfo = groupInfo;
            groupId = groupInfo["id"].ToString();
            rsaService = new RsaService();
            aesService = new AesService();
            _key = key;
            string EnAesKey = GroupInfo["keys"][UserInfo["username"].ToString()].ToString();
            aesKey = rsaService.Decrypt(EnAesKey, _key);
            members = GroupInfo["members"].ToObject<List<MemberRole>>();
            var user = members.FirstOrDefault(u => u.Username == UserInfo["username"].ToString());
            roleUser = user.Role;
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7070/groupChatHub", opts =>
                {
                    opts.AccessTokenProvider = async () => await Task.FromResult(accessToken);
                })
                .Build();
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Invoke(async () =>
                {
                    DateTime timeStamp = DateTime.Now;
                    var content = aesService.DecryptMessage(message, aesKey);
                    listMsg.Items.Add(" ");
                    listMsg.Items.Add(timeStamp.ToString("dd/MM/yyyy hh:mm tt"));
                    listMsg.Items.Add($"{user}: {content}");
                });
            });

            lblTitle.Text = GroupInfo["name"].ToString();
            groupService = new GroupService(accessToken);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            DateTime timeStamp = DateTime.UtcNow;
            var content = aesService.EncryptMessage(txtMessage.Text, aesKey);
            await connection.InvokeAsync("SendMessageGroup", groupId, UserInfo["name"].ToString(), content);
            
            await groupService.SendMessage(groupId, UserInfo["name"].ToString(), content, timeStamp);
            txtMessage.Clear();
        }

        private async void FormGroupDetails_Load(object sender, EventArgs e)
        {
            
           
            int total;
            (memReq, total) = await groupService.GetJoinListByGroupId(groupId);
            if (roleUser == "001" || roleUser == "003")
            {
                IconButton btnUserRequest = new IconButton
                {

                    FlatStyle = FlatStyle.Flat,
                    Text = $"User Requests ({total})",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Bahnschrift", 9, FontStyle.Bold),
                    BackColor = Color.FromArgb(192, 0, 192),
                    IconChar = IconChar.PersonWalkingArrowRight,
                    IconSize = 35,
                    IconColor = Color.Gainsboro,
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Location = new Point(729, 12),
                    Size = new Size(203, 44)
                };
                btnUserRequest.Click += async (sender, e) =>
                {
                    btnUserReq_Click(sender, e);
                };
                panelTop.Controls.Add(btnUserRequest);
            }
             
            await connection.StartAsync();
            await connection.SendAsync("JoinGroup", groupId);
            var data = await groupService.LoadMessageByGroupId(groupId);
            listMsg.Items.Clear();
            foreach (var message in data)
            {
                var content = aesService.DecryptMessage(message.Content, aesKey);
                var time = message.TimeStamp;
                TimeZoneInfo gmtPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gmt7Time = TimeZoneInfo.ConvertTimeFromUtc(time, gmtPlus7);
                listMsg.Items.Add(" ");
                listMsg.Items.Add($"{gmt7Time.ToString("dd/MM/yyyy hh:mm tt")}");
                listMsg.Items.Add($"{message.Sender}: {content}");
            }
        }
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = !string.IsNullOrWhiteSpace(txtMessage.Text);
        }
        private async void btnLeave_Click(object sender, EventArgs e)
        {
            string username = UserInfo["username"].ToString();
            string name = UserInfo["name"].ToString();

            if (roleUser == "003")
            {
                var res = MessageBox.Show("Bạn có chắc muốn rời khỏi nhóm này? (Bạn là chủ nhóm và khi bạn rời khỏi thì nhóm sẽ giải tán)", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    await groupService.DeleteGroup(groupId, username);
                    await groupService.DeleteAllMessage(groupId);
                    await connection.InvokeAsync("LeaveGroup", groupId);
                    await groupService.LeaveGroup(groupId, username);
                    this.Hide();
                    this.Close();
                }
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn rời khỏi nhóm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                await connection.InvokeAsync("LeaveGroup", groupId);
                await connection.InvokeAsync("SendMessageGroup", groupId, "Thông báo", $"{name} đã rời khỏi nhóm");
                await groupService.LeaveGroup(groupId, username);
                this.Hide();
                this.Close();
            }
        }

        private void btnUserReq_Click(object sender, EventArgs e)
        {
            FormUserRequests formReq = new FormUserRequests(accessToken, groupId, memReq, UserInfo["name"].ToString());
            formReq.ShowDialog();
        }

        private void linkMemList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            FormAddUser formAdd = new FormAddUser(
                accessToken,
                GroupInfo["name"].ToString(),
                GroupInfo["id"].ToString(),
                UserInfo["name"].ToString(),
                roleUser,
                aesKey
            );
            formAdd.ShowDialog();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnMemberList_Click(object sender, EventArgs e)
        {
            FormMemberList memList = new FormMemberList(accessToken, groupId, roleUser, UserInfo["username"].ToString(), aesKey);
            memList.ShowDialog();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            FormGroupInfo formInfo = new FormGroupInfo(accessToken, GroupInfo, roleUser, UserInfo["username"].ToString());
            formInfo.ShowDialog();
        }
    }
}
