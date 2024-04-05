using MyPassionProject.Models;
using MyPassionProject.Models.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace MyPassionProject.Controllers
{
    public class TeamMemberController : Controller
    {
        HttpClient client;
        JavaScriptSerializer jss;

        public TeamMemberController()
        {
            client = new HttpClient();
            jss = new JavaScriptSerializer();
            client.BaseAddress = new Uri(Constant.BASE_URL);
        }


        /// <summary>
        /// CRUD - CREATE:
        /// Create a record in the ApplicationUserTeams table, meaning a user is joining a team
        /// </summary>
        /// <param name="userGroup">a record to represent the team and hackathon user is joining to</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JoinTeam(ApplicationUserGroup userGroup)
        {
            var jsonPayload = jss.Serialize(userGroup);
            var content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            var response = client.PostAsync("teammemberdata/jointeam", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { Id = userGroup.GroupId });
            }
            else
            {
                ViewBag.ErrorMessage = "Fail to join this team. You are already in another team for this same hackathon";
                return View("Error");
            }
        }

        /// <summary>
        /// display all member by team id
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List(int groupId, int eventId)
        {
            var response = client.GetAsync($"teammemberdata/list?groupId={groupId}&eventId={eventId}").Result;
            var groupListViewModel = response.Content.ReadAsAsync<GroupListViewModel>().Result;
            Debug.WriteLine($"TeamMemberController.List: {groupListViewModel.ApplicationUserGroups.Count}");
            if (groupListViewModel.ApplicationUserGroups.Count < 0)
            {
                ViewBag.ErrorMessage = "Unable to find any team member.";
                return View("Error");
            }

            return View(groupListViewModel);
        }



        /// <summary>
        /// CRUD - DELETE:
        /// Deleting a record in the ApplicationUserGroup table meaning the a user is quiting a team.
        /// </summary>
        /// <param name="userGroup">a record to represent the team and hackathon user is quiting from</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuitTeam(ApplicationUserGroup userGroup)
        {
            var jsonPayload = jss.Serialize(userGroup);
            var content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            var response = client.PostAsync("teammemberdata/quitteam", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { Id = userGroup.Group });
            }
            else
            {
                ViewBag.ErrorMessage = "Fail to quit this team.";
                return View("Error");
            }
        }
    }
}