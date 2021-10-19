using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.ApiModels.Spaces;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Controllers
{
    public class SpaceController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _accessToken;
        public SpaceController(IOptions<ClickUpApiSettings> clickUpApiSettings)
        {
            _baseUrl = clickUpApiSettings.Value.EndPoint;
            _accessToken = clickUpApiSettings.Value.PersonalAccessToken;
        }
        public async Task<IActionResult> GetSpaces(string teamId)
        {
            var route = $"team/{teamId}/space";
            Response<Spaces> response = await DataHelper<Spaces>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }

        public async Task<IActionResult> GetSpace(string spaceId)
        {
            var route = $"space/{spaceId}";
            Response<Space> response = await DataHelper<Space>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return Json(response);
        }
    }
}
