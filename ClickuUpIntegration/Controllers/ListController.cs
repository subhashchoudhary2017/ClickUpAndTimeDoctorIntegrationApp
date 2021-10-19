using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.ApiModels.Lists;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Controllers
{
    public class ListController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _accessToken;
        public ListController(IOptions<ClickUpApiSettings> clickUpApiSettings)
        {
            _baseUrl = clickUpApiSettings.Value.EndPoint;
            _accessToken = clickUpApiSettings.Value.PersonalAccessToken;
        }
        public async Task<IActionResult> GetLists(string folderId)
        {
            var route = $"folder/{folderId}/list";
            Response<Lists> response = await DataHelper<Lists>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }
        public async Task<IActionResult> GetList(string listId)
        {
            var route = $"list/{listId}";
            Response<List> response = await DataHelper<List>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }

    }
}
