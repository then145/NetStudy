using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using NetStudy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStudy.Services
{
    public class TaskService
    {
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://localhost:7070/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        private string accessToken;

        public TaskService(string accessToken)
        {
            this.accessToken = accessToken;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<TaskDTO> CreateTask(UserTask task, string username)
        {
            try
            {
                var payload = new
                {
                    description = task.Description,
                    category = task.Category,
                    isCompleted = task.IsCompleted,
                    startDate = task.StartDate.ToLocalTime().ToUniversalTime(),
                    endDate = task.EndDate.ToLocalTime().ToUniversalTime(),     
                    owner = username
                };

                var response = await httpClient.PostAsync(
                    $"api/user-task/{username}/create-task",
                    new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
                );

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TaskDTO>(content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<List<TaskDTO>> GetTasks(string username)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/user-task/{username}/get-all-task");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                    var tasks = jsonObject["tasks"].ToObject<List<TaskDTO>>();
                    return tasks;
                }
                else
                {
                    throw new Exception(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TaskDTO>> GetTaskIsPending(string username)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/user-task/{username}/get-task-ispending");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                    var tasks = jsonObject["tasks"].ToObject<List<TaskDTO>>();
                    return tasks;
                }
                else
                {
                    throw new Exception(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task <List<TaskDTO>> GetTaskIsCompleted(string username)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/user-task/{username}/get-task-iscompleted");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                    var tasks = jsonObject["tasks"].ToObject<List<TaskDTO>>();
                    return tasks;
                }
                else
                {
                    throw new Exception(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TaskDTO>> GetTaskByDate(string username, DateTime date)
        {
            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");

                var response = await httpClient.GetAsync($"api/user-task/{username}/get-task-day?date={formattedDate}");
                var content = await response.Content.ReadAsStringAsync();

                if (response == null)
                {
                    throw new Exception("No response received from server.");
                }

                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                    var tasks = jsonObject["tasks"].ToObject<List<TaskDTO>>();
                    return tasks;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<TaskDTO>();
                }
                else
                {
                    throw new Exception(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TaskDTO> IsTaskExist(string username, string description)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/user-task/{username}/get-task-description/{description}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                    var task = jsonObject["tasks"].ToObject<TaskDTO>();
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> UpdateTask(string username, TaskDTO oldTask, TaskDTO newTask)
        {
            if (oldTask == null || newTask == null)
            {
                throw new ArgumentException("Dữ liệu task không hợp lệ!");
            }

            var payload = new
            {
                oldTask = new
                {
                    description = oldTask.Description,
                    category = oldTask.Category,
                    isCompleted = oldTask.IsCompleted,
                    startDate = oldTask.StartDate.ToLocalTime().ToUniversalTime(),
                    endDate = oldTask.EndDate.ToLocalTime().ToUniversalTime(),
                    owner = username
                },

                newTask = new
                {
                    description = newTask.Description,
                    category = newTask.Category,
                    isCompleted = newTask.IsCompleted,
                    startDate = newTask.StartDate.ToLocalTime().ToUniversalTime(),
                    endDate = newTask.EndDate.ToLocalTime().ToUniversalTime(),
                    owner = username
                }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri($"api/user-task/{username}/update-task", UriKind.Relative),
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
