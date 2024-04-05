using MyPassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace MyPassionProject.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/teamdata/list
        [HttpGet]
        public IHttpActionResult List()
        {
            try
            {
                List<Group> teams = db.Groups.ToList();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/teamdata/find/{id}
        [HttpGet]
        public IHttpActionResult Find(int id)
        {
            try
            {
                Group team = db.Groups.Find(id);

                if (team == null)
                {
                    return NotFound();
                }

                return Ok(team);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }


        [HttpPost]
        public IHttpActionResult CreateTeamWithLeader([FromBody] Group group)
        {
            Debug.WriteLine("TeamDataController.CreateTeamWithLeader() called ");
            // Using transaction here to make sure both team and the relationship can be created successfully togather.
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    // Step 1: Create the Team
                    db.Groups.Add(group);
                    db.SaveChanges(); // This will generate the TeamId
                    Debug.WriteLine("TeamDataController.CreateTeamWithLeader() Group Added");
                    // Step 2: Create the ApplicationUserTeam record
                    ApplicationUserGroup userTeam = new ApplicationUserGroup
                    {
                        UserId = group.TeamLeaderId,
                        GroupId = group.Id,
                        EventId = group.EventId
                    };

                    db.ApplicationUserGroups.Add(userTeam);
                    db.SaveChanges();
                    Debug.WriteLine("TeamDataController.CreateTeamWithLeader: User and Group has linked");
                    transaction.Commit();
                    Debug.WriteLine("TeamDataController.CreateTeamWithLeader: Transacton commited");
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine($"TeamDataController.CreateTeamWithLeader Exception= {ex}");
                    return InternalServerError();
                }
            }
        }
        // POST: api/teamdata/create
        [HttpPost]
        public IHttpActionResult Create(Group group)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Groups.Add(group);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = group.Id }, group);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/teamdata/update
        [HttpPost]
        public IHttpActionResult Update([FromBody] Group updatedGroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Group existingTeam = db.Groups.Find(updatedGroup.Id);

                if (existingTeam == null)
                {
                    return NotFound();
                }

                // For MVP, Only allowing User to change requirments
                existingTeam.Requirements = updatedGroup.Requirements;

                db.SaveChanges();

                return Ok(existingTeam);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/teamdata/delete/{id}
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Remove all records of ApplicationUserTeam by TeamId to maintain referential integrity
                    var userTeamRecords = db.ApplicationUserGroups.Where(userAndGroup => userAndGroup.GroupId == id);
                    db.ApplicationUserGroups.RemoveRange(userTeamRecords);

                    // Delete the Team by Id
                    var teamToDelete = db.Groups.Find(id);
                    if (teamToDelete == null)
                    {
                        return NotFound(); // Team not found
                    }

                    db.Groups.Remove(teamToDelete);
                    db.SaveChanges();

                    transaction.Commit(); // If everything is successful, commit the transaction
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return InternalServerError(ex);
                }
            }
        }
    }
}
