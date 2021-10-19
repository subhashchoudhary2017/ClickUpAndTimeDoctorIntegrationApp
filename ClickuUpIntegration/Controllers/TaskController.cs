using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.ApiModels.Tasks;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Controllers
{
    public class TaskController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _accessToken;
        public TaskController(IOptions<ClickUpApiSettings> clickUpApiSettings)
        {
            _baseUrl = clickUpApiSettings.Value.EndPoint;
            _accessToken = clickUpApiSettings.Value.PersonalAccessToken;
        }

        public async Task<IActionResult> GetTasks(string listId)
        {
            try
            {
                var route = $"list/{listId}/task";
                Response<ListTasks> response = await DataHelper<ListTasks>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
                var data = response.Result.MyTasks;
                var res = (from d in data
                           group d by d.Status.Type into g
                           select new
                           {
                               Type = g.Key,
                               Tasks = g.ToList()
                           }).ToList();
                return Json(res);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IActionResult> GetTask(string taskId)
        {
            var route = $"task/{taskId}";
            Response<ListTask> response = await DataHelper<ListTask>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }
    }
}
