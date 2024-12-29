using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetStudy.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PdfiumViewer;

namespace NetStudy.Forms
{
    public partial class FormDocument : Form
    {
        private readonly HttpClient _httpClient;
        private string _currentUser;
        private string _selectedDocumentId;
        private string _accessToken;
        private Panel _previousSelectedPanel;
        private const int PanelsPerPage = 5;
        private int _currentPage = 1;
        private bool isSearchMode = false;

        public FormDocument(string accessToken, string username)
        {
            InitializeComponent();
            _currentUser = username;
            _accessToken = accessToken;
            textBox_myusrname.Text = _currentUser;

            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7070/") };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        private void FormDocument_Load(object sender, EventArgs e)
        {

        }

        private async void button_mydcm_Click(object sender, EventArgs e)
        {
            isSearchMode = false;
            comboBox_page.Items.Clear();
            await FetchAndUpdateMyDocumentsAsync();
        }

        private async Task FetchAndUpdateMyDocumentsAsync()
        {
            var uploadedDocuments = await GetMyDocumentsAsync();
            var downloadedDocuments = await GetMyDownloadsAsync();
            var allDocuments = uploadedDocuments.Concat(downloadedDocuments).ToList();

            if (allDocuments.Count == 0)
            {
                MessageBox.Show("Bạn chưa đăng hoặc tải tài liệu nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int totalPages = (int)Math.Ceiling((double)allDocuments.Count / PanelsPerPage);
            UpdatePage(totalPages);
            CreateDocumentPanel(allDocuments);
        }

        private async Task<List<Document>> GetMyDocumentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/documents/mydocuments?username={_currentUser}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Document>>(jsonResponse);
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new List<Document>();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}", "Request Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Document>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Document>();
            }
        }

        private async Task<List<Document>> GetMyDownloadsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/documents/mydownloads?username={_currentUser}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Document>>(jsonResponse);
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new List<Document>();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}", "Request Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Document>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Document>();
            }
        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button_search_Click(object sender, EventArgs e)
        {
            isSearchMode = true;
            comboBox_page.Items.Clear();
            await SearchAndUpdateDocumentsAsync();
        }

        private async Task SearchAndUpdateDocumentsAsync()
        {
            string keyword = textBox_search.Text;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Hãy nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (documents, totalPages) = await SearchDocumentsAsync(keyword);
            UpdatePage(totalPages);
            CreateDocumentPanel(documents);
        }

        private async Task<(List<Document>, int)> SearchDocumentsAsync(string keyword)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/documents/search?keyword={keyword}&username={_currentUser}&pageNumber={_currentPage}&pageSize={PanelsPerPage}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var documents = JsonConvert.DeserializeObject<List<Document>>(jsonResponse);
                    int totalPages = int.Parse(response.Headers.GetValues("X-Total-Pages").FirstOrDefault() ?? "1");
                    return (documents, totalPages);
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (new List<Document>(), 1);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}", "Request Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (new List<Document>(), 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (new List<Document>(), 1);
            }
        }

        private void UpdatePage(int newTotalPages)
        {
            comboBox_page.Items.Clear();
            for (int i = 1; i <= newTotalPages; i++)
            {
                comboBox_page.Items.Add(i);
            }
            comboBox_page.Enabled = newTotalPages > 1;
            comboBox_page.SelectedIndex = _currentPage - 1;
        }

        private async void comboBox_page_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_page.SelectedIndex >= 0)
            {
                _currentPage = comboBox_page.SelectedIndex + 1;
                if (isSearchMode)
                {
                    await SearchAndUpdateDocumentsAsync();
                }
                else
                {
                    await FetchAndUpdateMyDocumentsAsync();
                }
            }
        }

        private async void button_previous_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                comboBox_page.SelectedIndex = _currentPage - 1;
                if (isSearchMode)
                {
                    await SearchAndUpdateDocumentsAsync();
                }
                else
                {
                    await FetchAndUpdateMyDocumentsAsync();
                }
            }
            else
            {
                MessageBox.Show("Đây là trang đầu tiên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void button_next_Click(object sender, EventArgs e)
        {
            if (_currentPage < comboBox_page.Items.Count)
            {
                _currentPage++;
                comboBox_page.SelectedIndex = _currentPage - 1;
                if (isSearchMode)
                {
                    await SearchAndUpdateDocumentsAsync();
                }
                else
                {
                    await FetchAndUpdateMyDocumentsAsync();
                }
            }
            else
            {
                MessageBox.Show("Đã là trang cuối cùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CreateDocumentPanel(List<Document> documents)
        {
            panel_body.SuspendLayout();
            panel_body.Controls.Clear();
            int yOffset = 0;
            int panelHeight = (panel_body.Height - (PanelsPerPage - 1) * 10) / PanelsPerPage;

            int startIndex = (_currentPage - 1) * PanelsPerPage;
            int endIndex = Math.Min(startIndex + PanelsPerPage, documents.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var document = documents[i];

                Panel documentPanel = new Panel
                {
                    Width = panel_body.Width - 20,
                    Height = panelHeight,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10),
                    Margin = new Padding(5),
                    BackColor = Color.Indigo,
                    Location = new Point(10, yOffset),
                    Tag = document.Id
                };

                documentPanel.Click += Panel_Click;

                Label lblTitle = new Label
                {
                    Text = $"Tiêu đề: {document.Title}",
                    AutoSize = true,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.Gainsboro
                };

                Label lblTag = new Label
                {
                    Text = $"Mục: {document.Tag}",
                    AutoSize = true,
                    Font = new Font("Arial", 10),
                    ForeColor = Color.Gainsboro
                };

                Label lblUploader = new Label
                {
                    Text = $"Người đăng tài: {document.UploaderName}",
                    AutoSize = true,
                    Font = new Font("Arial", 10),
                    ForeColor = Color.Gainsboro
                };

                if (!document.Title.EndsWith("mp4") && !document.Title.EndsWith("mp3"))
                {
                    Button btnDetails = new Button
                    {
                        Text = "...",
                        Size = new Size(50, 30),
                        Location = new Point(documentPanel.Width - 60, 35),
                        BackColor = Color.FromArgb(0, 117, 214),
                        ForeColor = Color.Gainsboro,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    btnDetails.Click += (sender, e) => ShowDocumentDetails(document.Id);
                    documentPanel.Controls.Add(btnDetails);
                }

                documentPanel.Controls.Add(lblTitle);
                lblTitle.Location = new Point(10, 10);
                documentPanel.Controls.Add(lblTag);
                lblTag.Location = new Point(10, 40);
                documentPanel.Controls.Add(lblUploader);
                lblUploader.Location = new Point(10, 70);

                panel_body.Controls.Add(documentPanel);
                yOffset += documentPanel.Height + 10;
            }

            panel_body.ResumeLayout();
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            if (panel != null)
            {
                if (_previousSelectedPanel != null)
                {
                    _previousSelectedPanel.BackColor = Color.Indigo;
                    foreach (Control control in _previousSelectedPanel.Controls)
                    {
                        if (control is Label)
                        {
                            control.ForeColor = Color.Gainsboro;
                        }
                    }
                }

                _selectedDocumentId = panel.Tag.ToString();

                panel.BackColor = Color.FromArgb(50, 255, 255);
                foreach (Control control in panel.Controls)
                {
                    if (control is Label)
                    {
                        control.ForeColor = Color.Indigo;
                    }
                }

                _previousSelectedPanel = panel;

                // MessageBox.Show($"Selected Document ID: {_selectedDocumentId}");
            }
        }

        private async void ShowDocumentDetails(string documentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/documents/content/{documentId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var tempFilePath = Path.GetTempFileName();

                    File.WriteAllBytes(tempFilePath, content);

                    var document = await GetDocumentDetailsAsync(documentId);
                    if (document.Title.EndsWith(".pdf"))
                    {
                        var pdfViewer = new PdfViewer();
                        pdfViewer.Document = PdfDocument.Load(tempFilePath);
                        var pdfForm = new Form
                        {
                            Text = "PDF Viewer - NetStudy",
                            Width = 800,
                            Height = 600,
                            StartPosition = FormStartPosition.CenterScreen
                        };
                        pdfViewer.Dock = DockStyle.Fill;
                        pdfForm.Controls.Add(pdfViewer);
                        pdfForm.Show();
                    }
                    else if (document.Title.EndsWith(".docx"))
                    {
                        var doc = new Aspose.Words.Document(tempFilePath);
                        var pdfPath = Path.ChangeExtension(tempFilePath, ".pdf");
                        doc.Save(pdfPath);

                        var pdfViewer = new PdfViewer();
                        pdfViewer.Document = PdfDocument.Load(pdfPath);
                        var pdfForm = new Form
                        {
                            Text = "PDF Viewer - NetStudy",
                            Width = 800,
                            Height = 600,
                            StartPosition = FormStartPosition.CenterScreen
                        };
                        pdfViewer.Dock = DockStyle.Fill;
                        pdfForm.Controls.Add(pdfViewer);
                        pdfForm.Show();
                    }
                    else if (document.Title.EndsWith(".jpg") || document.Title.EndsWith(".png"))
                    {
                        var imageForm = new Form
                        {
                            Text = "Image Viewer - NetStudy",
                            Width = 800,
                            Height = 600,
                            StartPosition = FormStartPosition.CenterScreen
                        };
                        var pictureBox = new PictureBox
                        {
                            Image = Image.FromFile(tempFilePath),
                            Dock = DockStyle.Fill,
                            SizeMode = PictureBoxSizeMode.Zoom
                        };
                        imageForm.Controls.Add(pictureBox);
                        imageForm.Show();
                    }
                    else if (document.Title.EndsWith(".txt"))
                    {
                        var textForm = new Form
                        {
                            Text = "Text Viewer - NetStudy",
                            Width = 800,
                            Height = 600,
                            StartPosition = FormStartPosition.CenterScreen
                        };
                        var textBox = new TextBox
                        {
                            Multiline = true,
                            ReadOnly = true,
                            Dock = DockStyle.Fill,
                            ScrollBars = ScrollBars.Both,
                            Text = File.ReadAllText(tempFilePath)
                        };
                        textForm.Controls.Add(textBox);
                        textForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Không hỗ trợ định dạng tệp này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            var uploadForm = new DocumentUpload(_currentUser, _accessToken);
            uploadForm.ShowDialog();
        }

        private async void button_download_Click(object sender, EventArgs e)
        {
            string documentId = GetSelectedDocumentId();
            if (!string.IsNullOrEmpty(documentId))
            {
                DownloadDocument(documentId);
            }
        }

        private async void DownloadDocument(string documentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/documents/download/{documentId}?username={_currentUser}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|Word documents (*.docx)|*.docx|Text files (*.txt)|*.txt|PowerPoint files (*.pptx)|*.pptx|Excel files (*.xlsx)|*.xlsx|Image files (*.png;*.jpg)|*.png;*.jpg|Audio files (*.mp3)|*.mp3|Video files (*.mp4)|*.mp4|RAR files (*.rar)|*.rar|ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
                        saveFileDialog.Title = "Lưu tài liệu";
                        MessageBox.Show("Hãy chọn đúng định dạng tệp cần lưu!");

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                            MessageBox.Show("Tải xuống thành công.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}\nUsername: {_currentUser}", "Request Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}\nUsername: {_currentUser}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button_edit_Click(object sender, EventArgs e)
        {
            string documentId = GetSelectedDocumentId();
            if (!string.IsNullOrEmpty(documentId))
            {
                var document = await GetDocumentDetailsAsync(documentId);
                if (document != null && document.UploaderName == _currentUser)
                {
                    var editForm = new DocumentEdit(documentId, _accessToken);
                    editForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Bạn chỉ có thể chỉnh sửa tài liệu mà mình đăng tải.");
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một tài liệu trước khi chinh sửa.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Tài liệu không tồn tại.", "Get Document Details Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private async void button_delete_Click(object sender, EventArgs e)
        {
            string documentId = GetSelectedDocumentId();
            if (!string.IsNullOrEmpty(documentId))
            {
                var document = await GetDocumentDetailsAsync(documentId);
                if (document != null && document.UploaderName == _currentUser)
                {
                    await DeleteDocumentAsync(documentId);
                }
                else if (document == null)
                {
                    MessageBox.Show($"Tài liệu không tồn tại.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Bạn chỉ có thể xóa tài liệu mà mình đăng tải.\nDocument ID: {documentId}\nUploader: {document.UploaderName}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một tài liệu trước khi xóa.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteDocumentAsync(string documentId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/documents/delete/{documentId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Xóa tài liệu thành công.\nDocument ID: {documentId}");
                    await SearchAndUpdateDocumentsAsync();
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}\nDocument ID: {documentId}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}\nDocument ID: {documentId}", "Request Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}\nDocument ID: {documentId}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSelectedDocumentId()
        {
            return _selectedDocumentId;
        }
    }
}