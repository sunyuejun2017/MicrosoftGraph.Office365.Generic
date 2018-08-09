using OfficeDevPnP.MSGraphAPI.Infrastructure;
using OfficeDevPnP.MSGraphAPI.Models;
using OfficeDevPnP.MSGraphAPIDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OfficeDevPnP.MSGraphAPIDemo.Controllers
{
    [Authorize]
    public class MailCalendarContactsController : Controller
    {
        #region Actions to be implemented

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListFolders()
        {
            var folders = MailHelper.ListFolders();

          /*  String jsonResponse = MicrosoftGraphHelper.MakeGetRequestForString(
                String.Format("{0}me/mailFolders?$skip={1}",
                    MicrosoftGraphHelper.MicrosoftGraphV1BaseUri,
                    0));

            var folders = JsonConvert.DeserializeObject<MailFolderList>(jsonResponse);
            */
            return View(folders.ToList());
        }

        public ActionResult ListMessages()
        {
            return View();
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        public ActionResult ListCalendarEvents()
        {
            return View();
        }

        public ActionResult SendMeetingRequest()
        {
            return View();
        }

        public ActionResult ListContacts()
        {
            return View();
        }

        public ActionResult AddContact()
        {
            return View();
        }

        #endregion

        [HttpPost]
        public ActionResult PlayWithMail(PlayWithMailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            AntiForgery.Validate();

            var folders = MailHelper.ListFolders();
            
            // Here you can use whatever mailbox name that you like, instead of Inbox f.Name == "发件箱" || 
            var messages = MailHelper.ListMessages(folders.FirstOrDefault(f => f.Name == "收件箱").Id);
            if (messages != null && messages.Count > 0)
            {
                var message = MailHelper.GetMessage(messages[0].Id, true);

                foreach (var attachment in message.Attachments)
                {
                    // Download content only for attachments smaller than 100K
                    if (attachment.Size < 100 * 1024)
                    {
                        attachment.EnsureContent();
                    }
                }
            }
            
            MailHelper.SendMessage(new MailMessageToSend
            {
                Message = new MailMessage
                {
                    Subject = "Test message",
                    Body = new ItemBody
                    {
                        Content = "<html><body><h1>Hello from ASP.NET MVC calling Microsoft Graph API!</h1></body></html>",
                        Type = BodyType.Html,
                    },
                    To = new List<UserInfoContainer>(
                        new UserInfoContainer[] {
                            new UserInfoContainer
                            {
                                Recipient = new UserInfo
                                {
                                    Name = model.MailSendToDescription,
                                    Address = model.MailSendTo
                                }
                            }
                    }),
                },
                SaveToSentItems = true,
            });

            if (messages != null && messages.Count > 0)
            {
                MailHelper.Reply(messages[0].Id, "This a direct reply!");
                MailHelper.ReplyAll(messages[0].Id, "This a reply all!");
               /* MailHelper.Forward(messages[0].Id,
                    new List<UserInfoContainer>(
                        new UserInfoContainer[]
                        {
                        new UserInfoContainer
                        {
                            Recipient = new UserInfo
                            {
                                Name = model.MailSendToDescription,
                                Address = model.MailSendTo,
                            }
                        },
                        new UserInfoContainer
                        {
                            Recipient = new UserInfo
                            {
                                Address = "o365admin@21v365.win",
                                Name = "Tenant Admin",
                            }
                        },
                        }),
                    "Hey! Look at this!");
                    */
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult PlayWithCalendars(PlayWithMailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            AntiForgery.Validate();

            var calendars = CalendarHelper.ListCalendars();
            var calendar = CalendarHelper.GetCalendar(calendars[0].Id);
            var events = CalendarHelper.ListEvents(calendar.Id, 0);
            var eventsCalendarView = CalendarHelper.ListEvents(calendar.Id, DateTime.Now, DateTime.Now.AddDays(10), 0);

            if (events[0].ResponseStatus != null && events[0].ResponseStatus.Response == ResponseType.NotResponded)
            {
                CalendarHelper.SendFeedbackForMeetingRequest(
                    calendar.Id, events[0].Id, MeetingRequestFeedback.Accept,
                    "I'm looking forward to meet you!");
            }

            var singleEvent = CalendarHelper.CreateEvent(calendars[0].Id,
                new Event
                {
                    Attendees = new List<UserInfoContainer>(
                        new UserInfoContainer[]
                        {
                            new UserInfoContainer
                            {
                                Recipient = new UserInfo
                                {
                                    Name = model.MailSendToDescription,
                                    Address = model.MailSendTo,
                                }
                            },
                            new UserInfoContainer
                            {
                                Recipient = new UserInfo
                                {
                                    Address = "someone@company.com",
                                    Name = "Someone Else",
                                }
                            },
                        }),
                    Start = new TimeInfo
                    {
                        DateTime = DateTime.Now.AddDays(2).ToUniversalTime(),
                        TimeZone = "UTC"
                    },
                    OriginalStartTimeZone = "UTC",
                    End = new TimeInfo
                    {
                        DateTime = DateTime.Now.AddDays(2).AddHours(1).ToUniversalTime(),
                        TimeZone = "UTC"
                    },
                    OriginalEndTimeZone = "UTC",
                    Importance = ItemImportance.High,
                    Subject = "Introducing the Microsoft Graph API",
                    Body = new ItemBody
                    {
                        Content = "<html><body><h2>Let's talk about the Microsoft Graph API!</h2></body></html>",
                        Type = BodyType.Html,
                    },
                    Location = new EventLocation
                    {
                        Name = "PiaSys.com Head Quarters",
                    },
                    IsAllDay = false,
                    IsOrganizer = true,
                    ShowAs = EventStatus.WorkingElsewhere,
                    Type = EventType.SingleInstance,
                });

            var nextMonday = DateTime.Now.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek + 7) % 7);
            var nextMonday9AM = new DateTime(nextMonday.Year, nextMonday.Month, nextMonday.Day, 9, 0, 0);
            var lastDayOfMonth = new DateTime(nextMonday.AddMonths(1).Year, nextMonday.AddMonths(1).Month, 1).AddDays(-1);
            var eventSeries = CalendarHelper.CreateEvent(calendars[0].Id,
                new Event
                {
                    Start = new TimeInfo
                    {
                        DateTime = nextMonday9AM.ToUniversalTime(),
                        TimeZone = "UTC"
                    },
                    OriginalStartTimeZone = "UTC",
                    End = new TimeInfo
                    {
                        DateTime = nextMonday9AM.AddHours(1).ToUniversalTime(),
                        TimeZone = "UTC"
                    },
                    OriginalEndTimeZone = "UTC",
                    Importance = ItemImportance.Normal,
                    Subject = "Recurring Event about Microsoft Graph API",
                    Body = new ItemBody
                    {
                        Content = "<html><body><h2>Let's talk about the Microsoft Graph API!</h2></body></html>",
                        Type = BodyType.Html,
                    },
                    Location = new EventLocation
                    {
                        Name = "Paolo's Office",
                    },
                    IsAllDay = false,
                    IsOrganizer = true,
                    ShowAs = EventStatus.Busy,
                    Type = EventType.SeriesMaster,
                    Recurrence = new EventRecurrence
                    {
                        Pattern = new EventRecurrencePattern
                        {
                            Type = RecurrenceType.Weekly,
                            DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday },
                            Interval = 1,
                        },
                        Range = new EventRecurrenceRange
                        {
                            StartDate = nextMonday9AM.ToUniversalTime(),
                            Type = RecurrenceRangeType.EndDate,
                            EndDate = lastDayOfMonth.ToUniversalTime(),
                        }
                    }
                });

            var seriesInstances = CalendarHelper.ListSeriesInstances(
                calendar.Id, eventSeries.Id, DateTime.Now, DateTime.Now.AddMonths(2));

            var singleEventToUpdate = CalendarHelper.GetEvent(calendar.Id, events[0].Id);

            singleEventToUpdate.Attendees = new List<UserInfoContainer>(
                        new UserInfoContainer[]
                        {
                            new UserInfoContainer
                            {
                                Recipient = new UserInfo
                                {
                                    Name = model.MailSendToDescription,
                                    Address = model.MailSendTo,
                                }
                            },
                        });
            var updatedEvent = CalendarHelper.UpdateEvent(calendar.Id, singleEventToUpdate);

            CalendarHelper.DeleteEvent(calendar.Id, singleEvent.Id);
            CalendarHelper.DeleteEvent(calendar.Id, eventSeries.Id);

            return View("Index");
        }

        public ActionResult PlayWithContacts()
        {
            var contacts = ContactsHelper.ListContacts();
            try
            {
                var photo = ContactsHelper.GetContactPhoto(contacts[0].Id);
            }
            catch (Exception)
            {
                // Something wrong while getting the thumbnail,
                // We will have to handle it properly ...
            }

            contacts[0].PersonalNotes += String.Format("Modified on {0}", DateTime.Now);
            var updatedContact = ContactsHelper.UpdateContact(contacts[0]);

            var addedContact = ContactsHelper.AddContact(new Contact
            {
                GivenName = "Michael",
                DisplayName = "Michael Red",
                EmailAddresses = new List<UserInfo>(
                    new UserInfo[]
                    {
                        new UserInfo
                        {
                            Address = "michael@company.com",
                            Name  = "Michael Red",
                        }
                    }),
                CompanyName = "Sample Company",
            });

            ContactsHelper.DeleteContact(addedContact.Id);

            return View("Index");
        }
    }
}
