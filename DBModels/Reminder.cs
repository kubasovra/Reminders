using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RemindersApp.DBModels
{
    public class Reminder
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public DateTime ShowTime { get; set; }
        public bool Elapsed { get; set; }
    }
}
