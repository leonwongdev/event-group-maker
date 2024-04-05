using MyPassionProject.Models;
using MyPassionProject.Models.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace MyPassionProject.Controllers
{
    public class TeamMemberDataController : ApiController
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet]
        public IHttpActionResult Create()
        {
            return Ok();

        }

        /// <summary>
        /// CRUD - READ
        /// List all team member by team id.
        /// </summary>
        /// <param name="id">Team Id</param>
        /// <returns></returns>
        /// GET /api/teammemberdata/list?groupId={groupId}&eventId={eventId}
        [HttpGet]
        public IHttpActionResult List(int groupId, int eventId)
        {
            try
            {
                // Query ApplicationUserTeam to get all records where TeamId equals the provided id
                var teamMembers = context.ApplicationUserTeams
                    .Where(aut => aut.GroupId == groupId)
                    .ToList();

                var currEvent = context.Events.Find(eventId);
                var groupListViewModel = new GroupListViewModel
                {
                    ApplicationUserGroups = teamMembers,
                    Event = currEvent
                };
                return Ok(groupListViewModel);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// CRUD - CREATE:
        /// Create a record in the ApplicationUserGroup table, meaning a user is joining a team
        /// </summary>
        /// <param name="userGroup">a record to represent the team and hackathon user is joining to</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult JoinTeam(ApplicationUserGroup userGroup)
        {

            try
            {
                Debug.Write($"TeamMemberDataController.JoinTeam Event id={userGroup.EventId}, Group Id={userGroup.GroupId}, UserId={userGroup.UserId}");
                context.ApplicationUserTeams.Add(userGroup);
                context.SaveChanges();
                return Ok(userGroup);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TeamMemberDataController.JoinTeam Exception:{ex}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// CRUD - DELETE:
        /// Create a record in the ApplicationUserTeams table, meaning a user is joining a team
        /// </summary>
        /// <param name="userGroup">a record to represent the team and hackathon user is joining to</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QuitTeam(ApplicationUserGroup userGroup)
        {

            try
            {
                // Attach the team to the context first to fix the issue of
                // "The object cannot be deleted because it was not found in the ObjectStateManager"
                context.ApplicationUserTeams.Attach(userGroup);
                context.ApplicationUserTeams.Remove(userGroup);
                context.SaveChanges();
                return Ok(userGroup);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
