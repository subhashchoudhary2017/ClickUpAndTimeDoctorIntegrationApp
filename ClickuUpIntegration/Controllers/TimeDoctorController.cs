using ClickUpIntegration.Helpers;
using ClickUpIntegration.Models.TimeDoctor;
using ClickUpIntegration.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClickUpIntegration.Models.TimeDoctor.TimeDoctorEnums;

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


        public IActionResult Index()
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
                    Expires = DateTime.Parse(response.Result.Data.ExpireAt)
                });

                _httpAccessor.HttpContext.Response.Cookies.Append("UserId", response.Result.Data.UserId, new CookieOptions
                {
                    Expires = DateTime.Parse(response.Result.Data.ExpireAt)
                });

                var companiesAsString = JsonConvert.SerializeObject(response.Result.Data.Companies);

                _httpAccessor.HttpContext.Response.Cookies.Append("Companies", companiesAsString, new CookieOptions
                {
                    Expires = DateTime.Parse(response.Result.Data.ExpireAt)
                });

                var company = response.Result.Data.Companies.FirstOrDefault(s => s.Name == "BladePorts");
                if (company != null)
                {
                    _httpAccessor.HttpContext.Response.Cookies.Append("CompanyId", company.Id, new CookieOptions
                    {
                        Expires = DateTime.Parse(response.Result.Data.ExpireAt)
                    });
                }
            }
            return Json(response);
        }

        public async Task<List<Users>> GetUserListByCompany(string companyId)
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];

            var route = $"users?company={companyId}&token={token}";
            Response<TimeDoctorUsers> response = await DataHelper<TimeDoctorUsers>.Execute(_baseUrl, route, OperationType.GET);

            return response.Result.Users;
        }

        public async Task<List<Project>> GetProjectListByCompany(string companyId)
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];
            var route = $"projects?company={companyId}&token={token}&all=true&show-integration=true";
            Response<TimeDoctorProjects> response = await DataHelper<TimeDoctorProjects>.Execute(_baseUrl, route, OperationType.GET);
            return response.Result.Projects;
        }

        public async Task<IActionResult> Users()
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];
            var companyId = _httpAccessor.HttpContext.Request.Cookies["CompanyId"];

            var route = $"users?company={companyId}&token={token}";
            Response<TimeDoctorUsers> response = await DataHelper<TimeDoctorUsers>.Execute(_baseUrl, route, OperationType.GET);

            return Json(response);
        }

        public IActionResult WorkLog()
        {
            ViewBag.Companies = JsonConvert.DeserializeObject<List<Company>>(_httpAccessor.HttpContext.Request.Cookies["Companies"]);
            ViewBag.GroupByTypes = GetGroupByType();
            return View();
        }

        private List<SelectListItem> GetGroupByType()
        {
            var groupByType = Enum.GetValues(typeof(GroupByTypeEnum)).Cast<GroupByTypeEnum>()
                     .Select(x => new SelectListItem { Text = TimeDoctorEnums.GetEnumDescription(x), Value = ((int)x).ToString() }).ToList();

            return groupByType;
        }

        public async Task<IActionResult> GetDropdowns(string companyId)
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];
            if (string.IsNullOrEmpty(token))
                return Json(new { Success = false, Result = "", Message = "Token is expired!!!" });

            var result = new
            {
                Projects = await GetProjectListByCompany(companyId),
                Users = await GetUserListByCompany(companyId),
            };
            return Json(new { Result = result, Success = true, Message = "" });
        }

        [HttpPost]
        public async Task<IActionResult> GetTimeDoctorData(WorkLogFilterDto input)
        {
            var token = _httpAccessor.HttpContext.Request.Cookies["timedoctor_accesstoken"];

            List<string> dateRanges = new List<string>();

            List<WorkLogByDates> data = new List<WorkLogByDates>();
            var day = 15;

            if (!string.IsNullOrEmpty(input.From) && !string.IsNullOrEmpty(input.To))
            {
                var newfDate = DateTime.Parse(input.From);
                var newtDate = DateTime.Parse(input.To);

                List<Tuple<DateTime, DateTime>> r = new List<Tuple<DateTime, DateTime>>();

                var startlop = true;
                while (startlop)
                {
                    var daysInMonth = DateTime.DaysInMonth(newfDate.Year, newfDate.Month);
                    DateTime endDat = DateTime.Now;

                    if (newfDate.Day <= 15)
                    {
                        endDat = new DateTime(newfDate.Year, newfDate.Month, 15, 23, 59, 59);
                    }
                    else if(newfDate.Day > 15 && daysInMonth == 31)
                    {
                        endDat = new DateTime(newfDate.Year, newfDate.Month, 31, 23, 59, 59);
                    }
                    else if(newfDate.Day > 15 && daysInMonth == 30)
                    {
                        endDat = new DateTime(newfDate.Year, newfDate.Month, 30, 23, 59, 59);
                    }

                    if (newtDate.Date == endDat.Date || newtDate.Date < endDat.Date)
                    {
                        r.Add(new Tuple<DateTime, DateTime>(newfDate, newtDate.AddDays(1).AddSeconds(-1)));
                        startlop = false;
                    }
                    else
                    {
                        r.Add(new Tuple<DateTime, DateTime>(newfDate, endDat));
                        newfDate = endDat.AddDays(1).Date;
                    }
                }

                for (int i = 0; i < r.Count; i++)
                {
                    var stDate = r[i].Item1;
                    var eDate = r[i].Item2;

                    input.From = stDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    input.To = eDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    data.Add(new WorkLogByDates
                    {
                        DateRange = stDate.ToString("MM/dd/yyyy") + " - " + eDate.ToString("MM/dd/yyyy"),
                        WorkLogs = await GetData(input, token)
                    });
                }


                var loopCount = Math.Ceiling((newtDate - newfDate).TotalDays / day);

                //for (int i = 0; i < loopCount; i++)
                //{


                //    if ((newtDate - newfDate).TotalDays > day)
                //    {
                //        var sDate = newfDate;
                //        var eDate = sDate.AddDays(day);

                //        input.From = newfDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                //        input.To = eDate.AddSeconds(-1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                //        data.Add(new WorkLogByDates
                //        {
                //            DateRange = newfDate.ToString("MM/dd/yyyy") + " - " + eDate.AddSeconds(-1).ToString("MM/dd/yyyy"),
                //            WorkLogs = await GetData(input, token)
                //        });

                //        newfDate = eDate;
                //    }
                //    else if ((newtDate - newfDate).TotalDays <= day)
                //    {
                //        input.From = newfDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                //        input.To = newtDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                //        data.Add(new WorkLogByDates
                //        {
                //            DateRange = newfDate.ToString("MM/dd/yyyy") + " - " + newtDate.AddDays(1).AddSeconds(-1).ToString("MM/dd/yyyy"),
                //            WorkLogs = await GetData(input, token)
                //        });
                //    }
                //}
            }

            data = data.Where(s => s.WorkLogs.Any()).ToList();

            return PartialView("~/Views/Shared/_WorkLogByUser.cshtml", data);
        }

        private async Task<List<WorkLogByUser>> GetData(WorkLogFilterDto input, string token)
        {
            var route = $"activity/worklog";
            route += "?company=" + input.CompanyId;
            if (!string.IsNullOrEmpty(input.UserId))
                route += "&user=" + input.UserId;

            if (!string.IsNullOrEmpty(input.From))
            {
                route += "&from=" + input.From;
            }

            if (!string.IsNullOrEmpty(input.To))
            {
                route += "&to=" + input.To;
            }

            route += "&task-project-names=true";
            route += $"&token={token}";

            Response<TimeDoctorWorkLog> response = await DataHelper<TimeDoctorWorkLog>.Execute(_baseUrl, route, OperationType.GET);

            var users = await GetUserListByCompany(input.CompanyId);


            List<WorkLogByUser> workLogByUsers = new List<WorkLogByUser>();

            if (input.GroupByType == (int)GroupByTypeEnum.GroupByUser)
            {
                foreach (var item in response.Result.WorkLog.Where(s => s.Count > 0))
                {
                    var dataWithUserName = (from i in item
                                            join u in users on i.UserId equals u.Id into xyz
                                            from s in xyz.DefaultIfEmpty()
                                            select new WorkLog
                                            {
                                                Date = i.Date,
                                                DeviceId = i.DeviceId,
                                                Mode = i.Mode,
                                                ProjectId = i.ProjectId,
                                                ProjectName = i.ProjectName,
                                                Start = i.Start,
                                                TaskId = i.TaskId,
                                                TaskName = i.TaskName,
                                                Time = i.Time,
                                                UserId = i.UserId,
                                                UserName = s == null ? "" : s.Name,
                                            }).ToList();

                    if (!string.IsNullOrEmpty(input.ProjectName) && input.ProjectName.Split(',').Length == 1)
                    {
                        dataWithUserName = dataWithUserName.Where(x => (input.ProjectName.ToLower().IndexOf(x.ProjectName.ToLower()) > -1)).ToList();
                    }
                    if (!string.IsNullOrEmpty(input.UserId) && input.UserId.Split(',').Length == 1)
                    {
                        dataWithUserName = dataWithUserName.Where(x => input.UserId == x.UserId).ToList();
                    }

                    if (dataWithUserName.Count > 0)
                    {
                        var userName = dataWithUserName.FirstOrDefault()?.UserName;

                        var groupByProject = (from d in dataWithUserName
                                              group d by (d.ProjectId, d.ProjectName) into g
                                              select new ProjectTask
                                              {
                                                  ProjectName = g.Key.ProjectName,
                                                  ProjectId = g.Key.ProjectId,
                                                  TaskName = "",
                                                  TaskId = "",
                                                  Order = 0,
                                                  TotalTimeSpentInProject = g.Sum(x => x.Time),
                                                  TotalHour = ConvertToTime(g.Sum(x => x.Time))
                                              }).ToList();

                        var totalTimeSpent = ConvertToTime(groupByProject.Select(s => s.TotalTimeSpentInProject).Sum());

                        var groupByProjectAndTask = (from d in dataWithUserName
                                                     group d by (d.ProjectId, d.ProjectName, d.TaskId, d.TaskName) into g
                                                     select new ProjectTask
                                                     {
                                                         ProjectName = "",
                                                         ProjectId = g.Key.ProjectId,
                                                         TaskName = g.Key.TaskName,
                                                         TaskId = g.Key.TaskId,
                                                         Order = 1,
                                                         TotalHour = ConvertToTime(g.Sum(x => x.Time)),
                                                     }).ToList();

                        var result = groupByProject.Union(groupByProjectAndTask).OrderBy(x => x.ProjectId).ThenBy(x => x.Order).ToList();

                        if (!string.IsNullOrEmpty(userName))
                        {
                            workLogByUsers.Add(new WorkLogByUser
                            {
                                UserName = userName,
                                TotalUserTime = totalTimeSpent,
                                GroupByType = input.GroupByType,
                                UserData = result
                            });
                        }
                    }
                }
            }
            else if (input.GroupByType == (int)GroupByTypeEnum.GroupByTask)
            {
                List<WorkLog> dataWithUserName = new List<WorkLog>();
                foreach (var item in response.Result.WorkLog.Where(s => s.Count > 0))
                {
                    var data = (from i in item
                                join u in users on i.UserId equals u.Id into xyz
                                from s in xyz.DefaultIfEmpty()
                                select new WorkLog
                                {
                                    Date = i.Date,
                                    DeviceId = i.DeviceId,
                                    Mode = i.Mode,
                                    ProjectId = i.ProjectId,
                                    ProjectName = i.ProjectName,
                                    Start = i.Start,
                                    TaskId = i.TaskId,
                                    TaskName = i.TaskName,
                                    Time = i.Time,
                                    UserId = i.UserId,
                                    UserName = s == null ? "" : s.Name,
                                }).ToList();

                    dataWithUserName.AddRange(data);

                }

                if (!string.IsNullOrEmpty(input.ProjectName) && input.ProjectName.Split(',').Length == 1)
                {
                    dataWithUserName = dataWithUserName.Where(x => (input.ProjectName.ToLower().IndexOf(x.ProjectName.ToLower()) > -1)).ToList();
                }
                if (!string.IsNullOrEmpty(input.UserId) && input.UserId.Split(',').Length == 1)
                {
                    dataWithUserName = dataWithUserName.Where(x => input.UserId == x.UserId).ToList();
                }

                if (dataWithUserName.Count > 0)
                {
                    var userName = dataWithUserName.FirstOrDefault()?.UserName;

                    var groupByProject = (from d in dataWithUserName
                                          group d by (d.ProjectId, d.ProjectName) into g
                                          select new ProjectTask
                                          {
                                              ProjectName = g.Key.ProjectName,
                                              ProjectId = g.Key.ProjectId,
                                              TaskName = "",
                                              TaskId = "",
                                              Order = 0,
                                              TotalTimeSpentInProject = g.Sum(x => x.Time),
                                              TotalHour = ConvertToTime(g.Sum(x => x.Time))
                                          }).ToList();

                    var totalTimeSpent = ConvertToTime(groupByProject.Select(s => s.TotalTimeSpentInProject).Sum());

                    var groupByProjectAndTask = (from d in dataWithUserName
                                                 group d by (d.ProjectId, d.ProjectName, d.TaskId, d.TaskName) into g
                                                 select new ProjectTask
                                                 {
                                                     ProjectName = "",
                                                     ProjectId = g.Key.ProjectId,
                                                     TaskName = g.Key.TaskName,
                                                     TaskId = g.Key.TaskId,
                                                     Order = 1,
                                                     TotalHour = ConvertToTime(g.Sum(x => x.Time)),
                                                 }).ToList();

                    var result = groupByProject.Union(groupByProjectAndTask).OrderBy(x => x.ProjectId).ThenBy(x => x.Order).ToList();

                    if (!string.IsNullOrEmpty(userName))
                    {
                        workLogByUsers.Add(new WorkLogByUser
                        {
                            UserName = userName,
                            TotalUserTime = totalTimeSpent,
                            GroupByType = input.GroupByType,
                            UserData = result
                        });
                    }
                }
            }
            else
            {
                List<WorkLog> workLogs = new List<WorkLog>();
                foreach (var item in response.Result.WorkLog.Where(s => s.Count > 0))
                {
                    var data = (from i in item
                                join u in users on i.UserId equals u.Id into xyz
                                from s in xyz.DefaultIfEmpty()
                                select new WorkLog
                                {
                                    Date = i.Date,
                                    DeviceId = i.DeviceId,
                                    Mode = i.Mode,
                                    ProjectId = i.ProjectId,
                                    ProjectName = i.ProjectName,
                                    Start = i.Start,
                                    TaskId = i.TaskId,
                                    TaskName = i.TaskName,
                                    Time = i.Time,
                                    UserId = i.UserId,
                                    UserName = s == null ? "" : s.Name,
                                }).ToList();

                    workLogs.AddRange(data);

                }

                if (!string.IsNullOrEmpty(input.ProjectName) && input.ProjectName.Split(',').Length == 1)
                {
                    workLogs = workLogs.Where(x => (input.ProjectName.ToLower().IndexOf(x.ProjectName.ToLower()) > -1)).ToList();
                }
                if (!string.IsNullOrEmpty(input.UserId) && input.UserId.Split(',').Length == 1)
                {
                    workLogs = workLogs.Where(x => input.UserId == x.UserId).ToList();
                }

                var groupByTask = (from d in workLogs
                                   group d by (d.ProjectId, d.ProjectName) into g
                                   select new ProjectTask
                                   {
                                       ProjectName = g.Key.ProjectName,
                                       ProjectId = g.Key.ProjectId,
                                       TaskName = "",
                                       TaskId = "",
                                       Order = 0,
                                       TotalHour = ConvertToTime(g.Sum(x => x.Time))
                                   }).ToList();

                var groupByTaskAndUser = (from d in workLogs
                                          group d by (d.ProjectId, d.ProjectName, d.TaskId, d.TaskName, d.UserId, d.UserName) into g
                                          select new ProjectTask
                                          {
                                              ProjectName = "",
                                              ProjectId = g.Key.ProjectId,
                                              TaskName = g.Key.TaskName,
                                              TaskId = g.Key.TaskId,
                                              UserId = g.Key.UserId,
                                              UserName = g.Key.UserName,
                                              Order = 1,
                                              TotalHour = ConvertToTime(g.Sum(x => x.Time)),
                                          });

                var taskData = groupByTaskAndUser.ToList();

                var result = groupByTask.Union(taskData).OrderBy(x => x.ProjectId).ThenBy(x => x.Order).ThenBy(s => s.TaskName).ToList();

                workLogByUsers.Add(new WorkLogByUser
                {
                    GroupByType = input.GroupByType,
                    UserData = result
                });
            }

            return workLogByUsers;
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

            _httpAccessor.HttpContext.Response.Cookies.Delete("Companies", new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(-1)
            });

            return RedirectToAction("Login", "Home");
        }
    }
}
public class WorkLogByDates
{
    public WorkLogByDates()
    {
        WorkLogs = new List<WorkLogByUser>();
    }
    public string DateRange { get; set; }
    public List<WorkLogByUser> WorkLogs { get; set; }
}