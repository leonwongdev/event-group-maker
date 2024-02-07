using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;//should add this to utilize the HttpClient
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using MyPassionProject.Models;//should add this to use EventDto 
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;



namespace MyPassionProject.Controllers
{
    public class EventController : Controller
    {
        // GET: Event/List
        public ActionResult List()//should be the same as viewbag.title 
        {
            //use HTTP client to access infomation
            //objective: communicate with our event data api to retrieve a list of event
            //curl https://localhost:44317/api/EventData/ListEvents

            HttpClient client = new HttpClient();
            string url = "EventData/ListEvents";// In order to work , need a router like:" https://localhost:44317/api/"before string
            HttpResponseMessage response = client.GetAsync(url).Result;//According to your method, use GetAsync,PostAsync,or ReadAsAsync.
            List<EventDto> Events = response.Content.ReadAsAsync<List<EventDto>>().Result;

            //Views/Event/List.cshtml
            return View(Events);
        }
        //Get:Event/Find/2
        public ActionResult Find(int id)
        {
            //use HTTP client to access infomation
            //objective: communicate with our event data api to retrieve a specific event by ID
            //curl https://localhost:44317/api/EventData/FindEvent/2

            HttpClient client = new HttpClient();
            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/FindEvent/" + convertedId;//In order to work , need a router like:"https://localhost:44317/api/"before string
            HttpResponseMessage response = client.GetAsync(url).Result;

            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            Debug.WriteLine("event received : ");
            Debug.WriteLine(SelectedEvent.Title);
            //Views/Event/Find.cshtml
            return View(SelectedEvent);
        }
        public ActionResult Error()
        {

            return View();
        }

        // GET: Event/New
        public ActionResult New()
        {
            return View();
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

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(newEvent);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpClient client = new HttpClient();
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

            //objective: communicate with our event data api to retrieve a specific event by ID
            //curl https://localhost:44317/api/EventData/FindEvent/9

            HttpClient client = new HttpClient();
            string convertedId = id.ToString();//super important!!!!
            string url = "EventData/FindEvent/" + convertedId; //In order to work, need a router like: "https://localhost:44317/api/"before string
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            return View(SelectedEvent);
        }

        // POST: Event/UpdateEvent/9
        [HttpPost]
        public ActionResult Update(int id, Event newEvent)
        {
            try
            {
                Debug.WriteLine("event info : ");
                Debug.WriteLine(newEvent.Title);
                Debug.WriteLine(newEvent.CategoryId);

                string convertedId = id.ToString();//super important!!!!
                string url = "EventData/UpdateEvent/" + convertedId;//In order to work , need a router like:"https://localhost:44317/api/"before string
                //serialize into JSON
                //Send the request to the API

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string jsonpayload = jss.Serialize(newEvent);

                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                // POST: api/EventData/UpdateEvent/9
                //Header : Content-Type: application/json

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                Debug.WriteLine(response);


                return RedirectToAction("Find/" + id);
            }
            catch
            {
                return View();
            }
        }
        //GET : /Event/DeleteConfirm/{id}
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            HttpClient client = new HttpClient();
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
           
            try
            {
                string convertedId = id.ToString();//super important!!!!
                string url = "EventData/DeleteEvent/" + convertedId;// In order to work, need a router like: "https://localhost:44317/api/"before string
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent("");
                HttpResponseMessage response = client.PostAsync(url,content).Result;
                return RedirectToAction("List");

                /* if (response.IsSuccessStatusCode)
                 {
                     return RedirectToAction("List");
                 }
                 else
                 {

                     return RedirectToAction("Error");
                 }*/
            }
            catch
            {
                return View();
            }
        }



    }
}