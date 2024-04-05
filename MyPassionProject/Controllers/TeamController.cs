using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyPassionProject;
using MyPassionProject.Controllers;
using MyPassionProject.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HackathonTeamBuilder.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {

        HttpClient client;
        JavaScriptSerializer jss;
        EventDataController eventDataController;
        public TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri(Constant.BASE_URL);
            jss = new JavaScriptSerializer();
            eventDataController = new EventDataController();
        }
        /// <summary>
        /// Display a list of teams for a hackthon
        /// </summary>
        /// <param name="id">hackthon id</param>
        /// <returns></returns>
        public ActionResult List(int Id)
        {
            HttpResponseMessage response = client.GetAsync($"teamdata/ListTeamsByHackathon/{Id}").Result;
            var teamViewModels = response.Content.ReadAsAsync<TeamViewModel>().Result;

            /*get current user from owin context*/
            var user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.CurrentUser = user;

            return View(teamViewModels);
        }


        /// <summary>
        /// For displaying team creation form.
        /// </summary>
        /// <param name="eventId">Id of a event</param>
        /// <returns></returns>
        public ActionResult Create(int eventId)
        {
            //var response = client.GetAsync($"hackathondata/findbyid/{hackathonId}").Result;
            // Get event by id using the EventDataController

            var response = client.GetAsync($"eventdata/FindEvent/{eventId}").Result;
            var eventDto = response.Content.ReadAsAsync<EventDto>().Result;
            var UserId = User.Identity.GetUserId();

            var tempTeam = new Group
            {
                EventId = eventId,
                TeamLeaderId = UserId,
                Event = new Event
                {
                    EventId = eventDto.EventId,
                    Title = eventDto.Title,
                }
            };

            return View(tempTeam);
        }

        /// <summary>
        /// For sending form data of new teamto the web api layer.
        /// </summary>
        /// <param name="group">New Team data</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Group group)
        {
            // Build payload
            string jsonpayload = jss.Serialize(group);
            HttpContent content = new StringContent(jsonpayload);
            Debug.WriteLine("Team Controller.Create jsonpayload");
            Debug.WriteLine(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            Debug.WriteLine("Team Controller.Create sending http request");
            var response = client.PostAsync($"teamdata/CreateTeamWithLeader", content).Result;
            Debug.WriteLine($"Team Controller.Create response code: {response.IsSuccessStatusCode}");
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("List", new { Id = team.HackathonId });
                Debug.WriteLine("Team Controller.Create: successfully created group");
                // Redirect to EventController.List
                return RedirectToAction("List", "Event");
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to create team, you might have already created a team for this hackathon.";
                return View("Error");
            }
        }

        /// <summary>
        /// For displaying the form to update team requirements.
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Update(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var response = client.GetAsync($"teamdata/find/{id}").Result;
            var team = response.Content.ReadAsAsync<Group>().Result;
            if (team == null)
            {
                ViewBag.ErrorMessage = "Unable to render form for updating team info: Team not found.";
                return View("Error");
            }
            else if (team.TeamLeaderId != currentUserId)
            {
                ViewBag.ErrorMessage = "Unable to update team info as you are not the team leader of this team.";
                return View("Error");
            }
            return View(team);
        }


        /// <summary>
        /// For sending data to the web api layer for updating team data in database
        /// </summary>
        /// <param name="team">New team data</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(Group team)
        {
            string payload = jss.Serialize(team);
            HttpContent content = new StringContent(payload);
            content.Headers.ContentType.MediaType = "application/json";
            var response = client.PostAsync($"teamdata/update/", content).Result;

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("List", new { Id = team.HackathonId });
                Debug.WriteLine("Team Controller.Update: successfully updated group");
                return View();
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to update team info. Contact Administrator please.";
                return View("Error");
            }
        }


        /// <summary>
        /// Display delete confirm page
        /// </summary>
        /// <param name="id">Id of Team</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var response = client.GetAsync($"teamdata/find/{id}").Result;
            var team = response.Content.ReadAsAsync<Group>().Result;

            /*get current user*/
            var user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (team == null)
            {
                ViewBag.ErrorMessage = "Unable to display delete page. Reason: Team not found.";
                return View("Error");
            }
            //else if (team.TeamLeaderId != currentUserId && (user.isAdministrator == null || user.isAdministrator != true))
            //{
            //    ViewBag.ErrorMessage = "Unable to display delete page. Reason: You are not the team leader, only team leader can delete this team .";
            //    return View("Error");
            //}
            return View(team);
        }

        /// <summary>
        /// For sending neccessary data to web api layer to delete a team in database
        /// </summary>
        /// <param name="id">team id</param>
        /// <param name="hackathonId">hackathon id, for redirect to team listing page of a ceratin hackathon.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id, int hackathonId)
        {

            string url = "teamdata/delete/" + id;
            HttpResponseMessage response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { Id = hackathonId });
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to delete the team.";
                return View("Error");
            }
        }
    }
}