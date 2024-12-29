using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetStudy.Models;
using Newtonsoft.Json;
using NetStudy.Models;

namespace NetStudy.Forms
{
    public partial class DocumentEdit : Form
    {
        private readonly HttpClient _httpClient;
        private string _documentId;
        private string _accessToken;
        private string _currentTag;
        private string _currentUploaderName;

        public DocumentEdit(string documentId, string accessToken)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7070/") };
            _documentId = documentId;
            _accessToken = accessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            LoadDocumentDetails();
        }

        private async void LoadDocumentDetails()
        {
            var document = await GetDocumentDetailsAsync(_documentId);
            if (document != null)
            {
                textBox_oldname.Text = document.Title;
                textBox_newname.Text = document.Title;
                textBox_olddescription.Text = document.Description;
                textBox_newdescription.Text = document.Description;
                checkBox_friend.Checked = document.ShareWithFriends;
                checkBox_group.Checked = document.ShareWithGroups;
                checkBox_all.Checked = document.ShareWithAll;

                _currentTag = document.Tag;
                _currentUploaderName = document.UploaderName;
            }
        }

        private async Task<UploadResponse> GetDocumentDetailsAsync(string documentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/documents/{documentId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<UploadResponse>(jsonResponse);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Không tìm thấy tài liệu.", "Get Document Details Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}", "Get Document Details Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}\nDocument ID: {documentId}", "Request Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}\nDocument ID: {documentId}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async void button_edit_Click(object sender, EventArgs e)
        {
            var updatedDocument = new Document
            {
                Id = _documentId,
                Title = textBox_newname.Text,
                Description = textBox_newdescription.Text,
                Tag = _currentTag,
                UploaderName = _currentUploaderName,
                ShareWithFriends = checkBox_friend.Checked,
                ShareWithGroups = checkBox_group.Checked,
                ShareWithAll = checkBox_all.Checked
            };

            var response = await UpdateDocumentAsync(updatedDocument);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Chỉnh sửa tài liệu thành công.");
                this.Close();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Chỉnh sửa tài liệu thất bại. Lỗi: {errorMessage}");
            }
        }

        private async Task<HttpResponseMessage> UpdateDocumentAsync(Document updatedDocument)
        {
            var jsonContent = JsonConvert.SerializeObject(updatedDocument);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"api/documents/update/{updatedDocument.Id}", content);
        }

        private void DocumentEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
