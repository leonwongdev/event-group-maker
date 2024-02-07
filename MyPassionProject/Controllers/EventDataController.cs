using MyPassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection;


namespace MyPassionProject.Controllers
{

    public class EventDataController : ApiController
    {
        //Utlizing the database connection 
        private ApplicationDbContext db = new ApplicationDbContext();
        private object modelBuilder;

        //ListEvents
        //GET:"api/EventData/ListEvents
        [HttpGet]
        [Route("api/EventData/ListEvents")]
        public List<EventDto> ListEvents()
        {
            List<Event> Events = db.Events.ToList();
            List<EventDto> EventDtos = new List<EventDto>();
            Events.ForEach(b => EventDtos.Add(new EventDto()
            {
                EventId = b.EventId,
                Title = b.Title,
                CategoryName = b.Category.CategoryName
            }));
            return EventDtos;
        }

        //FindEvent
        // GET: api/EventData/FindEvent/2
        [ResponseType(typeof(Event))]
        [HttpGet]
        public IHttpActionResult FindEvent(int id)
        {
            Event Event = db.Events.Find(id);
            EventDto EventDto = new EventDto()
            {
                EventId = Event.EventId,
                Title = Event.Title,
                Location =Event.Location,
                EventDateTime =Event.EventDateTime,
                Capacity = Event.Capacity,
                Details = Event.Details,
                CategoryName = Event.Category.CategoryName
            };
            if (Event == null)
            {
                return NotFound();
            }

            return Ok(EventDto);
        }
        //AddEvent
        // POST: api/EventData/AddEvent
        [ResponseType(typeof(Event))]
        [HttpPost]
        public IHttpActionResult AddEvent(Event newEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Events.Add(newEvent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = newEvent.EventId }, newEvent);
        }


        // UpdateEvent
        // POST: api/EventData/UpdateEvent/9
        //CLI command:curl -H "Content-Type:application/json" -d @newEvent.json https://localhost:44317/api/EventData/UpdateEvent/9
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateEvent(int id, Event updatedEvent)
        {
            Debug.WriteLine("I have reached the update event method!");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            Event existingEvent = db.Events.Find(id);

            if (existingEvent == null)
            {
                Debug.WriteLine("Event not found");
                return NotFound();
            }

            // Only update properties user want to update
            existingEvent.UpdateDate = updatedEvent.UpdateDate != default
            ? updatedEvent.UpdateDate   
            : existingEvent.UpdateDate;//I leart that this patten is a short version of if statement. The patten A?B:C means if A then B , else C;
            // The update datetime should be current datetime, however, since it is a MVP ,I just leave it as original one for now
            existingEvent.Title = updatedEvent.Title ?? existingEvent.Title;//I learnt that this patten is a short version of another if statement. The patten A=B??C means If B is not null, let A=B, otherwise use A=C
            existingEvent.Location = updatedEvent.Location ?? existingEvent.Location;
            existingEvent.EventDateTime = updatedEvent.EventDateTime != default
            ? updatedEvent.EventDateTime
            : existingEvent.EventDateTime;
            existingEvent.Capacity = updatedEvent.Capacity ?? existingEvent.Capacity;
            existingEvent.Details = updatedEvent.Details ?? existingEvent.Details;
            

            try
            {
                db.SaveChanges();
                Debug.WriteLine("Event updated successfully");
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    Debug.WriteLine("Event not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        //DeleteEvent
        // POST: api/EventData/DeleteEvent/14
        [ResponseType(typeof(Event))]
        [HttpPost]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event existingEvent = db.Events.Find(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            db.Events.Remove(existingEvent);
            db.SaveChanges();

            return Ok();
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventId == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}




//Releted methods include:
//ListEventForCategory
//ListEventForUser
//AddEventToUser
//RemoveEventFromUser



