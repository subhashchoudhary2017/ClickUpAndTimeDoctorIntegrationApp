using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.ApiModels.Folders;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Controllers
{
    public class FolderController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _accessToken;

        public FolderController(IOptions<ClickUpApiSettings> clickUpApiSettings)
        {
            _baseUrl = clickUpApiSettings.Value.EndPoint;
            _accessToken = clickUpApiSettings.Value.PersonalAccessToken;
        }

        public async Task<IActionResult> GetFolders(string spaceId)
        {
            var route = $"space/{spaceId}/folder";
            Response<Folders> response = await DataHelper<Folders>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }
        public async Task<IActionResult> GetFolder(string folderId)
        {
            var route = $"folder/{folderId}";
            Response<Folder> response = await DataHelper<Folder>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }
    }
}
