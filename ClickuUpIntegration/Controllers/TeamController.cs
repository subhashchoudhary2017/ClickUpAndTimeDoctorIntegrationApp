using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.ApiModels;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Controllers
{
    public class TeamController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _accessToken;
        public TeamController(IOptions<ClickUpApiSettings> clickUpApiSettings)
        {
            _baseUrl = clickUpApiSettings.Value.EndPoint;
            _accessToken = clickUpApiSettings.Value.PersonalAccessToken;
        }
        public async Task<IActionResult> GetTeams()
        {
            var route = "team";
            Response<Teams> response = await DataHelper<Teams>.ExecuteWithToken(_baseUrl, route, OperationType.GET, _accessToken);
            return View(response.Result.MyTeams);
        }
    }
}
