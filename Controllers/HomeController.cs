using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
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
            string UserId = Request.Cookies["UserId"];
            indexViewModel.UserId = UserId;

            //If User is unidentified, creates a new one with a random ID and saves it in cookies. Expires in one year
            if (UserId == null)
            {
                Random rnd = new Random();
                string NewUserId;
                byte[] buffer = new byte[20];
                rnd.NextBytes(buffer);
                NewUserId = Encoding.UTF8.GetString(buffer);
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Append("UserId", NewUserId, cookieOptions);
                indexViewModel.UserId = NewUserId;
            }

            indexViewModel.reminders = _context.Reminders.Where(r => r.UserId == Request.Cookies["UserId"]).ToList();
            indexViewModel.UserId = Request.Cookies["UserId"];
            return View(indexViewModel);
        }

        //Creates a new reminder and adds it to the DB
        public IActionResult AddReminder(string deadline, string body, string title)
        {
            IndexViewModel indexViewModel = new IndexViewModel();
            Reminder newReminder = new Reminder()
            {
                Body = body,
                Title = title,
                ShowTime = Convert.ToDateTime(deadline),
                UserId = Request.Cookies["UserId"],
                Elapsed = false
            };
            _context.Add(newReminder);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Removes a reminder from the DB
        public IActionResult DeleteReminder(string reminderId)
        {
            IndexViewModel indexViewModel = new IndexViewModel();

            Reminder remToDelete = _context.Reminders.FirstOrDefault(rm => rm.Id == Convert.ToInt32(reminderId));
            if (remToDelete != null)
            {
                _context.Reminders.Remove(remToDelete);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
