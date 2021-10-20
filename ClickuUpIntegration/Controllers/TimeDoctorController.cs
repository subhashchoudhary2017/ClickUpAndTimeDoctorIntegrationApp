﻿using ClickUpIntegration.Helpers;
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
        private readonly IHttpContextAccessor _httpAccessor;
        public TimeDoctorController(IOptions<TimeDoctorApiSettings> timeDoctorApiSettings,
                                    IHttpContextAccessor httpAccessor)
        {
            _baseUrl = timeDoctorApiSettings.Value.EndPoint;
            _httpAccessor = httpAccessor;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Authenticate(string username = "", string password = "")
        {
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
                    Expires = DateTime.Now.AddMonths(6)
                });

                _httpAccessor.HttpContext.Response.Cookies.Append("UserId", response.Result.Data.UserId, new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(6)
                });

                var company = response.Result.Data.Companies.FirstOrDefault(s => s.Name == "BladePorts");
                if (company != null)
                {
                    _httpAccessor.HttpContext.Response.Cookies.Append("CompanyId", company.Id, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMonths(6)
                    });
                }
            }
            return Json(response);
        }

        public async Task<IActionResult> Users()
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];
            var companyId = _httpAccessor.HttpContext.Request.Cookies["CompanyId"];

            var route = $"users?company={companyId}&token={token}";
            Response<TimeDoctorUsers> response = await DataHelper<TimeDoctorUsers>.Execute(_baseUrl, route, OperationType.GET);

            return Json(response);
        }

        public async Task<IActionResult> WorkLog(string from, string to)
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];
            var companyId = _httpAccessor.HttpContext.Request.Cookies["CompanyId"];

            var route = $"activity/worklog";
            route += "?company=" + companyId;
            route += "&from=" + from;
            route += "&to=" + to;
            route += "&task-project-names=true";
            route += $"&token={token}";

            Response<TimeDoctorWorkLog> response = await DataHelper<TimeDoctorWorkLog>.Execute(_baseUrl, route, OperationType.GET);

            var data = response.Result.WorkLog.FirstOrDefault().ToList();

            List<ProjectTask> result = new List<ProjectTask>();

            if (data != null)
            {
                var groupByProject = (from d in data
                                      group d by (d.ProjectId, d.ProjectName) into g
                                      select new ProjectTask
                                      {
                                          ProjectName = g.Key.ProjectName,
                                          ProjectId = g.Key.ProjectId,
                                          TaskName = "",
                                          TaskId="",
                                          Order = 0,
                                          TotalHour = ConvertToTime(g.Sum(x => x.Time)),
                                      }).ToList();

                var groupByProjectAndTask = (from d in data 
                                             group d by (d.ProjectId, d.ProjectName, d.TaskId, d.TaskName) into g
                                             select new ProjectTask
                                             {
                                                 ProjectName = g.Key.ProjectName,
                                                 ProjectId = g.Key.ProjectId,
                                                 TaskName = g.Key.TaskName,
                                                 TaskId = g.Key.TaskId,
                                                 Order = 1,
                                                 TotalHour = ConvertToTime(g.Sum(x => x.Time)),
                                             }).ToList();

                result = groupByProject.Union(groupByProjectAndTask).OrderBy(x => x.ProjectId).ThenBy(x=>x.Order).ToList();


            }
           

            return View(result);
        }
        public string ConvertToTime(double timeSeconds)

        {
            int mySeconds = System.Convert.ToInt32(timeSeconds);

            int myHours = mySeconds / 3600; //3600 Seconds in 1 hour

            mySeconds %= 3600;


            int myMinutes = mySeconds / 60; //60 Seconds in a minute

            mySeconds %= 60;


            string mySec = mySeconds.ToString(),

            myMin = myMinutes.ToString(),

            myHou = myHours.ToString();


            if (myHours < 10) { myHou = myHou.Insert(0, "0"); }

            if (myMinutes < 10) { myMin = myMin.Insert(0, "0"); }

            if (mySeconds < 10) { mySec = mySec.Insert(0, "0"); }

            return myHou + "hh" + ":" + myMin + "mm" + ":" + mySec + "ss";

        }

        public IActionResult Logout()
        {
            _httpAccessor.HttpContext.Response.Cookies.Delete("timedoctor_accesstoken", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });

            _httpAccessor.HttpContext.Response.Cookies.Delete("UserId", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });

            _httpAccessor.HttpContext.Response.Cookies.Delete("CompanyId", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });

            return RedirectToAction("Login", "Home");
        }
    }
}
