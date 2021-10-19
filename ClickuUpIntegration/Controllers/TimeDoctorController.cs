using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.TimeDoctor;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickUpIntegration.Controllers
{
    public class TimeDoctorController : Controller
    {
        private readonly string _baseUrl;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpAccessor;
        public TimeDoctorController(IOptions<TimeDoctorApiSettings> timeDoctorApiSettings,
                                    IWebHostEnvironment env,
                                    IHttpContextAccessor httpAccessor)
        {
            _baseUrl = timeDoctorApiSettings.Value.EndPoint;
            _env = env;
            _httpAccessor = httpAccessor;
        }

        public async Task<IActionResult> Authenticate(string username = "", string password = "")
        {
            //if (_env.IsDevelopment())
            //{
            //    username = "alpeshkalena123@gmail.com";
            //    password = "P@ssword1";
            //}


            var route = "authorization/login";
            Response<AuthenticateResultModel> response = await DataHelper<AuthenticateResultModel>.Execute(_baseUrl, route, OperationType.POST, new
            {
                email = username,
                password = password
            });

            if (response.Success)
            {
                _httpAccessor.HttpContext.Response.Cookies.Append("timedoctor_accesstoken", response.Result.Data.Token, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                });

                _httpAccessor.HttpContext.Response.Cookies.Append("UserId", response.Result.Data.UserId, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                });

                var company = response.Result.Data.Companies.FirstOrDefault(s => s.Name == "BladePorts");
                if (company != null)
                {
                    _httpAccessor.HttpContext.Response.Cookies.Append("CompanyId", company.Id, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(1)
                    });
                }
            }
            return Json(response);
        }

        public async Task<IActionResult> WorkLog(DateTime? from = null, DateTime? to = null)
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];
            var companyId = _httpAccessor.HttpContext.Request.Cookies["CompanyId"];
            var userId = _httpAccessor.HttpContext.Request.Cookies["UserId"];

            var fromDate = "";
            var toDate = "";
            if (from.HasValue)
                fromDate = from.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
            if (to.HasValue)
                toDate = to.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");

            var route = $"activity/worklog?token={token}";
            route += "&company=" + companyId;
            route += "&from=" + fromDate;
            route += "&to=" + toDate;
            route += "&user=" + userId;
            route += "&task-project-names=true";

            Response<TimeDoctorWorkLog> response = await DataHelper<TimeDoctorWorkLog>.Execute(_baseUrl, route, OperationType.GET);

            return Json(response);
        }
    }
}
