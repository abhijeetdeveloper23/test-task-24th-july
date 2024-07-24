// Controllers/ReminderController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuleImplementation.Data;
using ModuleImplementation.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using ModuleImplementation.Services;

namespace ModuleImplementation.Controllers
{
    public class RemindersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailSender _emailSender;

        public RemindersController(AppDbContext context, EmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Reminder/Set
        public IActionResult Set()
        {
            return View();
        }

        // POST: Reminder/Set
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Set([Bind("Title,DateTime,Email")] Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                // Process and save reminder
                _context.Add(reminder);
                await _context.SaveChangesAsync();

                string subject = "Set Reminder";
                string body = $"Dear User,<br /><br />You have set a reminder for {reminder.DateTime}.<br /><br />Thank you.";

                // Example: Schedule to send the email after 10 minutes
                var sendEmailTask = SendEmailWithDelay(reminder.Email, subject, body, TimeSpan.FromMinutes(1));

               // Redirect to action after successful save
                return RedirectToAction(nameof(Index));
            }
            // If ModelState is not valid, return to the view with the model
            return View(reminder);
        }

        private async Task SendEmailWithDelay(string recipientEmail, string subject, string body, TimeSpan delay)
        {
            await Task.Delay(delay);
            await _emailSender.SendEmailNotification(recipientEmail, subject, body);
        }

        // GET: Reminder/Index
        public async Task<IActionResult> Index()
        {
            var reminders = await _context.Reminder.ToListAsync();
            return View(reminders);
        }
    }
}
