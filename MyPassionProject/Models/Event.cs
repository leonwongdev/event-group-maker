using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPassionProject.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Column(TypeName = "datetime2")]

        public DateTime UpdateDate { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EventDateTime { get; set; }

        public string Capacity { get; set; }
        public string Details { get; set; }

        //Many AppUsers involved in an event
        public ICollection<AppUser> AppUsers { get; set; }

        //A event belongs to one category
        //A category has many events

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Ignore the Category object when serializing to prevent self referencing loop
        [JsonIgnore]
        public virtual Category Category { get; set; }

        // Associate Group with Event
        public ICollection<Group> Groups { get; set; }

    }
    public class EventDto
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EventDateTime { get; set; }

        public string Capacity { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}