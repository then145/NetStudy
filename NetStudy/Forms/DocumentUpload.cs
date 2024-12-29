using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetStudy.Models;
using Newtonsoft.Json;

namespace NetStudy.Forms
{
    public partial class DocumentUpload : Form
    {
        private readonly HttpClient _httpClient;
        private string _filePath;
        private string _currentUserName;
        private string _accessToken;

        public DocumentUpload(string currentUserName, string accessToken)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7070/") };
            _currentUserName = currentUserName;
            _accessToken = accessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = openFileDialog.FileName;
                    textBox_src.Text = _filePath;
                    textBox_filename.Text = Path.GetFileName(_filePath);
                }
            }
        }

        private async void button_upload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                MessageBox.Show("Please select a file to upload.");
                return;
            }

            var document = new Document
            {
                Id = Guid.NewGuid().ToString(),
                Title = textBox_filename.Text,
                Description = textBox_description.Text,
                Tag = textBox_filetag.Text,
                ShareWithFriends = checkBox_friend.Checked,
                ShareWithGroups = checkBox_group.Checked,
                ShareWithAll = checkBox_all.Checked,
                UploaderName = _currentUserName
            };

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(document.Id), "Id");
                content.Add(new StringContent(document.Title), "Title");
                content.Add(new StringContent(document.Description), "Description");
                content.Add(new StringContent(document.Tag), "Tag");
                content.Add(new StringContent(document.ShareWithFriends.ToString()), "ShareWithFriends");
                content.Add(new StringContent(document.ShareWithGroups.ToString()), "ShareWithGroups");
                content.Add(new StringContent(document.ShareWithAll.ToString()), "ShareWithAll");
                content.Add(new StringContent(document.UploaderName), "UploaderName");

                var fileContent = new StreamContent(File.OpenRead(_filePath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent, "file", Path.GetFileName(_filePath));

                string requestInfo = $"Id: {document.Id}\n" +
                                     $"Title: {document.Title}\n" +
                                     $"Description: {document.Description}\n" +
                                     $"Tag: {document.Tag}\n" +
                                     $"ShareWithFriends: {document.ShareWithFriends}\n" +
                                     $"ShareWithGroups: {document.ShareWithGroups}\n" +
                                     $"ShareWithAll: {document.ShareWithAll}\n" +
                                     $"UploaderName: {document.UploaderName}\n" +
                                     $"FileName: {Path.GetFileName(_filePath)}\n" +
                                     $"ContentType: {fileContent.Headers.ContentType}\n";
                MessageBox.Show($"Sending to server:\n{requestInfo}");

                var response = await _httpClient.PostAsync("api/documents/upload", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                string responseInfo = $"Status Code: {response.StatusCode}\n" +
                                      $"Response Content: {responseContent}\n";
                MessageBox.Show($"Received from server:\n{responseInfo}");

                if (response.IsSuccessStatusCode)
                {
                    //var responseData = JsonConvert.DeserializeObject<UploadResponse>(responseContent);
                    //string documentId = responseData.DocumentId;
                    //string uploaderName = responseData.UploaderName;
                    //MessageBox.Show($"Document uploaded successfully.\nDocument ID: {documentId}\nUploader: {uploaderName}");
                    MessageBox.Show($"Document uploaded successfully.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Failed to upload document. Error: {responseContent}");
                }
            }
        }

        private void DocumentUpload_Load(object sender, EventArgs e)
        {

        }
    }
}
