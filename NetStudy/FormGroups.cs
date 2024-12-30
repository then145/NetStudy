using FontAwesome.Sharp;
using Microsoft.VisualBasic.ApplicationServices;
using NetStudy.Models;
using NetStudy.Services;
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
    public partial class FormGroups : Form
    {
        private int currPage = 1;
        private int pageSize = 5;
        private string accessToken;
        private readonly JObject UserInfo;
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private GroupService groupService;
        private string _key;
        public FormGroups(string token, JObject info, string key)
        {
            InitializeComponent();
            accessToken = token;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            UserInfo = info;

            groupService = new GroupService(accessToken);
            _key = key;
        }

        private void linkCreateGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCreateGroup formCreate = new FormCreateGroup(UserInfo, accessToken);
            formCreate.ShowDialog();
        }

        private async Task LoadGroups()
        {
            var groups = await groupService.LoadGroups(UserInfo["username"].ToString());
            if (groups != null && groups.Any())
            {
                PopulateGroupPanel(flowLayoutPanel1, groups);
            }

        }
        private async Task LoadGroupSearching(string searchString, int page)
        {

            var (groups, total) = await groupService.SearchGroup(searchString, page, pageSize);
            if (groups != null && groups.Any())
            {
                PopulateGroupPanel(flowLayoutPanel1, groups);
                UpdatePage(total);
            }
        }
        private void PopulateGroupPanel(FlowLayoutPanel flpanel, List<Group> groups)
        {
            flpanel.Controls.Clear();
            foreach (var group in groups)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanel1.Width - 20,
                    Tag = group.Id.ToString(),
                    Height = 100,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10),
                    Margin = new Padding(5),
                    BackColor = Color.Indigo,
                    Cursor = Cursors.Hand
                };
                Label lblName = new Label
                {
                    Text = group.Name,
                    Location = new Point(10, 10),
                    AutoSize = true,
                    Font = new Font("Cambria", 12, FontStyle.Bold),
                    ForeColor = Color.Gainsboro
                };

                Label lblId = new Label
                {
                    Text = $"ID: {group.Id.ToString()}",
                    AutoSize = true,
                    Location = new Point(10, 40),
                    Font = new Font("Cambria", 10),
                    ForeColor = Color.Gainsboro
                };
                IconButton btnInfo = new IconButton
                {
                    AutoSize = true,
                    Width = 35,
                    Height = 35,
                    BackColor = Color.FromArgb(34, 33, 74),
                    FlatStyle = FlatStyle.Flat,

                    Location = new Point(1450, 5),
                    IconChar = IconChar.Info,
                    IconColor = Color.Gainsboro,
                    IconSize = 30,

                };
                btnInfo.Click += async (sender, e) =>
                {
                    var info = await groupService.GetGroupInfo(group.Id.ToString());

                    FormGroupInfo grInfo = new FormGroupInfo(accessToken, info, "000", UserInfo["username"].ToString());
                    grInfo.Show();
                };
                panel.Click += Group_Panel_Click;
                panel.Controls.Add(lblName);
                panel.Controls.Add(btnInfo);
                panel.Controls.Add(lblId);
                flpanel.Controls.Add(panel);
            }

        }

        private async void Group_Panel_Click(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            var groupId = panel.Tag;
            
            var response = await httpClient.GetAsync($"api/groups/get-group/{groupId}");
            var res = await response.Content.ReadAsStringAsync();
            var msg = JObject.Parse(res)["message"].ToString();
            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {

                MessageBox.Show($"{msg}", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var result = MessageBox.Show($"{msg} Bạn có muốn tham gia không?", "Error...", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    await groupService.JoinGroupReq(groupId.ToString(), UserInfo["username"].ToString(), UserInfo["name"].ToString());

                }
                return;
            }
           
        
            var info = JObject.Parse(res);
            var groupDetails = new FormGroupDetails(accessToken, UserInfo, info, _key);
            groupDetails.Show();
        }

        private async void FormGroups_Load(object sender, EventArgs e)
        {
            lblName.Text = UserInfo["name"].ToString();
            await LoadGroups();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchGroup.Text;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Hãy nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadGroupSearching(keyword, currPage);
        }

        private void txtSearchGroup_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = !string.IsNullOrWhiteSpace(txtSearchGroup.Text);
        }

        private void UpdatePage(int newTotalPages)
        {
            comboBox1.Items.Clear();
            for (int i = 1; i <= newTotalPages; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.Enabled = newTotalPages > 1;
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                currPage = comboBox1.SelectedIndex + 1;
                await LoadGroupSearching(txtSearchGroup.Text, currPage);
            }
        }
    }
}
