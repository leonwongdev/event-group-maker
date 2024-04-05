﻿using MyPassionProject.Models;//should add this to use EventDto 
using MyPassionProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;//should add this to utilize the HttpClient
using System.Web.Mvc;
using System.Web.Script.Serialization;



namespace MyPassionProject.Controllers
{
    public class EventController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EventController()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:44317/api/");
        }


        // GET: Event/List
        public ActionResult List()//should be the same as viewbag.title 
        {
            //use HTTP client to access infomation
            //objective: communicate with our event data api to retrieve a list of event
            //curl https://localhost:44317/api/EventData/ListEvents

            string url = "EventData/ListEvents";// In order to work , need a router like:" https://localhost:44317/api/"before string
            HttpResponseMessage response = client.GetAsync(url).Result;//According to your method, use GetAsync,PostAsync,or ReadAsAsync.
            List<EventDto> Events = response.Content.ReadAsAsync<List<EventDto>>().Result;

            //Views/Event/List.cshtml
            return View(Events);
        }


        [Authorize]
        //Get:Event/Find/2
        public ActionResult Find(int id)
        {
            //use HTTP client to access infomation
            //objective: communicate with our event data api to retrieve a specific event by ID
            //curl https://localhost:44317/api/EventData/FindEvent/2

            FindEvent ViewModel = new FindEvent();

            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/FindEvent/" + convertedId;//In order to work , need a router like:"https://localhost:44317/api/"before string

            HttpResponseMessage response = client.GetAsync(url).Result;

            // Try block to handle errors
            EventDto SelectedEvent;
            try
            {
                SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
                ViewModel.SelectedEvent = SelectedEvent;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not find the event with id: " + id);
                Debug.WriteLine(e);
                return RedirectToAction("Error");
            }
            //EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            Debug.WriteLine("event received: ");
            Debug.WriteLine($"EventController.Find: Event title:{SelectedEvent.Title}, Group:{SelectedEvent.Groups}");
            foreach (var group in SelectedEvent.Groups)
            {
                Debug.WriteLine($"Requirements: {group.Requirements}");
            }
            ViewModel.SelectedEvent = SelectedEvent;

            //show associated AppUsers with this Event

            url = "AppUserData/ListAppUsersForEvent/" + convertedId;
            response = client.GetAsync(url).Result;

            IEnumerable<AppUserDto> ParticipatingUsers = response.Content.ReadAsAsync<List<AppUserDto>>().Result;

            ViewModel.ParticipatingUsers = ParticipatingUsers;


            url = "AppUserData/ListAppUserNotForEvent/" + convertedId;

            response = client.GetAsync(url).Result;

            IEnumerable<AppUserDto> NotPaticipatingUsers = response.Content.ReadAsAsync<IEnumerable<AppUserDto>>().Result;

            ViewModel.NotPaticipatingUsers = NotPaticipatingUsers;

            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }



        //POST: Event/Associate/{EventId}/{UserId}
        [HttpPost]
        public ActionResult Associate(int EventId, int UserId)
        {

            Debug.WriteLine("Attempting to associate event:" + EventId + " with AppUser " + UserId);

            //call our api to associate event with AppUser
            string convertedEventId = EventId.ToString();
            string convertedUserId = UserId.ToString();

            string url = "EventData/AssociateEventWithAppUser/" + convertedEventId + "/" + convertedUserId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Find/" + EventId);
        }


        //Get: Event/UnAssociate/{EventId}?UserId={UserId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int UserId)
        {
            Debug.WriteLine("Unassociate " + id + "with" + UserId);
            string convertedEventId = id.ToString();
            string convertedUserId = UserId.ToString();

            string url = "EventData/UnAssociateEventWithAppUser/" + convertedEventId + "/" + convertedUserId;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            return RedirectToAction("Find/" + id);
        }



        // GET: Event/New
        public ActionResult New()
        {
            //infomation about all categories in the system
            //GET api/CategoryData/ListCategories
            FindCategory ViewModel = new FindCategory();
            string url = "CategoryData/ListCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> CategoryOptions = response.Content.ReadAsAsync<List<CategoryDto>>().Result;
            ViewModel.CategoryOptions = CategoryOptions;
            return View(ViewModel);
        }


        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(Event newEvent)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Event.Title);
            //objective: add a new event into our system using the API
            //curl -H "Content-Type:application/json" -d @newEvent.json https://localhost:44317/api/EventData/AddEvent 

            string url = "EventData/AddEvent";//In order to work , need a router like:"https://localhost:44317/api/"before string

            string jsonpayload = jss.Serialize(newEvent);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }
        // GET: Event/Edit/9
        public ActionResult Edit(int id)
        {
            //grab the event information
            UpdateEvent ViewModel = new UpdateEvent();

            //objective: communicate with our event data api to retrieve a specific event by ID
            //curl https://localhost:44317/api/EventData/FindEvent/9

            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/FindEvent/" + convertedId; //In order to work, need a router like: "https://localhost:44317/api/"before string
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            ViewModel.SelectedEvent = SelectedEvent;

            // all categorys to choose from when updating this event
            //the existing event information
            url = "CategoryData/ListCategories/";
            response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> CategoryOptions = response.Content.ReadAsAsync<List<CategoryDto>>().Result;

            ViewModel.CategoryOptions = CategoryOptions;

            return View(ViewModel);
        }

        // POST: Event/UpdateEvent/9
        [HttpPost]
        public ActionResult Update(int id, Event newEvent)
        {

            Debug.WriteLine("event info : ");
            Debug.WriteLine(newEvent.Title);
            Debug.WriteLine(newEvent.CategoryId);

            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/UpdateEvent/" + convertedId;//In order to work , need a router like:"https://localhost:44317/api/"before string
                                                                //serialize into JSON
                                                                //Send the request to the API

            string jsonpayload = jss.Serialize(newEvent);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // POST: api/EventData/UpdateEvent/9
            //Header : Content-Type: application/json

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Find/" + id);
            }
            else
            {
                return View();
            }
        }


        //GET : /Event/DeleteConfirm/{id}
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/FindEvent/" + convertedId;//In order to work , need a router like:"https://localhost:44317/api/"before string
            HttpResponseMessage response = client.GetAsync(url).Result;

            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            if (SelectedEvent == null)
            {
                // If the event is not found, return HttpNotFound
                return HttpNotFound();
            }

            return View(SelectedEvent);
        }




        //Post: Event/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("The Event is ");
            Debug.WriteLine(id);

            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/DeleteEvent/" + convertedId;// In order to work, need a router like: "https://localhost:44317/api/"before string

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {

                return RedirectToAction("Error");
            }


        }



    }
}