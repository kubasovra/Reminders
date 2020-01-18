using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RemindersApp.DBModels;

namespace RemindersApp.Models
{
    public class IndexViewModel
    {
        public List<Reminder> reminders;
        public string UserId;
    }
}
