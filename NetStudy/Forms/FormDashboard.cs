using NetStudy.Services;
using Newtonsoft.Json.Linq;
using NetStudy.Models;
using System.Globalization;

namespace NetStudy.Forms
{
    public partial class FormDashboard : Form
    {
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private JObject UserInfo;
        private string accessToken;
        private TaskService taskService;
        private bool isEditMode = false;
        private Dictionary<int, string> descriptions = new Dictionary<int, string>();
        public FormDashboard(JObject info, string token)
        {
            InitializeComponent();
            accessToken = token;
            UserInfo = info;
            lbl_username.Text = UserInfo["username"].ToString();
            taskService = new TaskService(accessToken);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        private async void FormDashboard_Load(object sender, EventArgs e)
        {
            await LoadAllTaskAsync();
            SaveDescriptions();
        }

        private void SaveDescriptions()
        {
            descriptions.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                var description = row.Cells["Description"]?.Value?.ToString();
                if (!string.IsNullOrEmpty(description))
                {
                    descriptions[row.Index] = description;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            try
            {
                DateTime selectedDate = e.Start;

                var tasks = await taskService.GetTaskByDate(UserInfo["username"].ToString(), selectedDate);

                if (tasks != null && tasks.Count > 0)
                {
                    DisplayTasks(tasks);
                }
                if (tasks.Count == 0 || tasks == null)
                {
                    MessageBox.Show("Chưa có sự kiện được tạo!.");
                    dataGridView1.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async Task LoadAllTaskAsync()
        {
            try
            {
                var tasks = await taskService.GetTasks(UserInfo["username"].ToString());
                if (tasks != null)
                {
                    DisplayTasks(tasks);
                }
                else
                {
                    MessageBox.Show("No tasks found", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayTasks(List<TaskDTO> tasks)
        {
            dataGridView1.Rows.Clear();

            foreach (var task in tasks)
            {
                int rowIndex = dataGridView1.Rows.Add(
                    "",
                    task.Description,
                    task.Category,
                    task.StartDate.ToString("dd/MM/yyyy"),
                    task.EndDate.ToString("dd/MM/yyyy"),
                    task.IsCompleted
                );

                dataGridView1.Rows[rowIndex].Cells["Index"].Value = rowIndex + 1;
            }
        }

        private async void btn_all_Click(object sender, EventArgs e)
        {
            await LoadAllTaskAsync();
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                isEditMode = false;
                MessageBox.Show("Đã tắt chế độ chỉnh sửa hoặc tạo mới");
            }
            else
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                isEditMode = true;
                MessageBox.Show("Đã bật chế độ chỉnh sửa hoặc tạo mới");
            }
        }

        private async void btn_pending_Click(object sender, EventArgs e)
        {
            try
            {
                var tasks = await taskService.GetTaskIsPending(UserInfo["username"].ToString());
                if (tasks != null && tasks.Count > 0)
                {
                    DisplayTasks(tasks);
                }
                else
                {
                    MessageBox.Show("No tasks found", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveChanges()
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    var description = row.Cells["Description"]?.Value?.ToString();
                    var category = row.Cells["Category"]?.Value?.ToString();
                    var isCompleted = row.Cells["IsCompleted"]?.Value != null && Convert.ToBoolean(row.Cells["IsCompleted"].Value);

                    var startDateStr = row.Cells["StartDate"]?.Value?.ToString();
                    var endDateStr = row.Cells["EndDate"]?.Value?.ToString();

                    DateTime startDate;
                    DateTime endDate;

                    string dateFormat = "dd/MM/yyyy";

                    if (!DateTime.TryParseExact(startDateStr, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        MessageBox.Show($"Ngày bắt đầu không hợp lệ ở dòng {row.Index + 1}. Vui lòng sử dụng định dạng dd/MM/yyyy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.CurrentCell = row.Cells["StartDate"];
                        dataGridView1.BeginEdit(true);
                        return;
                    }

                    if (!DateTime.TryParseExact(endDateStr, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    {
                        MessageBox.Show($"Ngày kết thúc không hợp lệ ở dòng {row.Index + 1}. Vui lòng sử dụng định dạng dd/MM/yyyy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.CurrentCell = row.Cells["EndDate"];
                        dataGridView1.BeginEdit(true);
                        return;
                    }

                    //Ngày kết thúc không thể nhỏ hơn ngày bắt đầu
                    if (endDate < startDate)
                    {
                        MessageBox.Show($"Ngày kết thúc không thể nhỏ hơn ngày bắt đầu ở dòng {row.Index + 1}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.CurrentCell = row.Cells["EndDate"];
                        dataGridView1.BeginEdit(true);
                        return;
                    }

                    var originalDescription = descriptions.ContainsKey(row.Index) ? descriptions[row.Index] : null;

                    var existingTask = await taskService.IsTaskExist(UserInfo["username"].ToString(), originalDescription);

                    if (existingTask != null)
                    {
                        var newTask = new TaskDTO
                        {
                            Description = description,
                            Category = category,
                            StartDate = startDate,
                            EndDate = endDate,
                            IsCompleted = isCompleted
                        };

                        var result = await taskService.UpdateTask(UserInfo["username"].ToString(), existingTask, newTask);
                        if (result)
                        {
                            MessageBox.Show($"Cập nhật thành công: {description}");
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        var newTask = new UserTask
                        {
                            Description = description,
                            Category = category,
                            StartDate = startDate,
                            EndDate = endDate,
                            IsCompleted = isCompleted,
                            Owner = UserInfo["username"].ToString()
                        };

                        var result = await taskService.CreateTask(newTask, UserInfo["username"].ToString());
                        if (result == null)
                        {
                            MessageBox.Show($"Tạo mới thất bại: {description}");
                        }
                        else
                        {
                            MessageBox.Show($"Tạo mới thành công: {description}");
                        }
                    }
                }

                await LoadAllTaskAsync();
                SaveDescriptions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            await SaveChanges();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Index"].Index)
            {
                e.Value = e.RowIndex + 1;
            }
        }

        private async void btn_done_Click(object sender, EventArgs e)
        {
            try
            {
                var tasks = await taskService.GetTaskIsCompleted(UserInfo["username"].ToString());
                if (tasks != null && tasks.Count > 0)
                {
                    DisplayTasks(tasks);
                }
                else
                {
                    MessageBox.Show("No tasks found", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
