using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemindersApp.Models;
using RemindersApp.DBModels;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace RemindersApp.Controllers
{
    public class HomeController : Controller
    {
        private DBModels.AppContext _context;

        public HomeController(DBModels.AppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel();
            string UserId = HttpContext.Session.GetString("UserId");
            indexViewModel.UserId = UserId;
            if (UserId == null)
            {
                Random rnd = new Random();
                string NewUserId;
                byte[] buffer = new byte[20];
                rnd.NextBytes(buffer);
                NewUserId = Encoding.UTF8.GetString(buffer);
                HttpContext.Session.SetString("UserId", NewUserId);
                indexViewModel.UserId = NewUserId;
            }
            return View(indexViewModel);
        }

        public IActionResult AddReminder(string image_url, string body, string title, int deadline_hours, int deadline_minutes, int deadline_day, int deadline_month, int deadline_year)
        {
            IndexViewModel indexViewModel = new IndexViewModel();
            Reminder newReminder = new Reminder()
            {
                Body = body,
                Image = image_url,
                Title = title,
                ShowTime = new DateTime(deadline_year, deadline_month, deadline_day, deadline_hours, deadline_minutes, 0),
                UserId = HttpContext.Session.GetString("UserId"),
                Elapsed = false
            };
            _context.Add(newReminder);
            _context.SaveChanges();
            indexViewModel.reminders = _context.Reminders.Where(r => r.UserId == HttpContext.Session.GetString("UserId")).ToList();
            indexViewModel.UserId = HttpContext.Session.GetString("UserId");
            return View("Index", indexViewModel);
        }

        public IActionResult DeleteReminder(string reminderId)
        {
            IndexViewModel indexViewModel = new IndexViewModel();


            indexViewModel.reminders = _context.Reminders.Where(r => r.UserId == HttpContext.Session.GetString("UserId")).ToList();
            indexViewModel.UserId = HttpContext.Session.GetString("UserId");
            return View("Index", indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
