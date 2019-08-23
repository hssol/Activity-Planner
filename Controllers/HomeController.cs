using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult LoginUser()
        {
            return View("Login");
        }
        [HttpPost("userlogin")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u=>u.Email == userSubmission.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                var Hasher = new PasswordHasher<LoginUser>();
                var result = Hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                else
                {
                    HttpContext.Session.SetString("CurrentUser", userInDb.Email);
                    HttpContext.Session.SetInt32("CurrentUserID", userInDb.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Login");
            }
        }        
/////////////////////////////////////////////////////////
        [HttpGet("register")]
        public IActionResult RegisterUser()
        {
            return View("Register");
        }
        [HttpPost("userregister")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u=>u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already taken");
                    return View("Register");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    dbContext.Users.Add(user);
                    HttpContext.Session.SetString("CurrentUser", user.Email);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("CurrentUserID", user.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Register");
            }
        }
////////////////////////////////////////////////////////////
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                int? ID =  HttpContext.Session.GetInt32("CurrentUserID");
                int realId = (int) ID;
                User currentuser = dbContext.Users.FirstOrDefault(u=>u.UserId == realId);
                ViewBag.user = currentuser;
                ViewBag.userID = realId;
                List<Occurance> AllOccurances = dbContext.Occurances.Include(w=>w.Attendees).ThenInclude(w=>w.User).OrderBy(w=>w.Date).ToList();
                List<Occurance> creators = dbContext.Occurances.Include(w=>w.Creator).ToList();
                ViewBag.creators = creators;
                ViewBag.current = DateTime.Now;
                ViewData["CurrentTime"] = DateTime.Now.ToString();
                return View("Dashboard", AllOccurances);
            }
            else
            {
                return Redirect("/");
            }
        }
/////////////////////////////////////////////////////////////
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
/////////////////////////////////////////////////////////////
        [HttpGet("newoccurance")]
        public IActionResult NewOccurance()
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                int? ID =  HttpContext.Session.GetInt32("CurrentUserID");
                int realId = (int) ID;
                User currentuser = dbContext.Users.FirstOrDefault(u=>u.UserId == realId);
                int creatorid = currentuser.UserId;
                ViewBag.creatorId = creatorid;
                return View("NewOccurance");
            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpPost("addoccurance")]
        public IActionResult AddOccurance(Occurance newOccurance)
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                if(ModelState.IsValid)
                {
                    dbContext.Add(newOccurance);
                    dbContext.SaveChanges();
                    return Redirect("/dashboard");
                }
                else
                {
                int? ID =  HttpContext.Session.GetInt32("CurrentUserID");
                int realId = (int) ID;
                User currentuser = dbContext.Users.FirstOrDefault(u=>u.UserId == realId);
                int creatorid = currentuser.UserId;
                ViewBag.creatorId = creatorid;
                return View("NewOccurance");
                }
            }
            else
            {
                return RedirectToAction("LoginUser");
            }
        }   
            
//////////////////////////////////////////////////////////////
        [HttpGet("occurance/{occuranceID}")]
        public IActionResult ViewOccurance(int occuranceID)
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                Occurance currentOccurance = dbContext.Occurances.Include(w=>w.Attendees).ThenInclude(w=>w.User).Where(w=>w.OccuranceId == occuranceID).FirstOrDefault();
                ViewBag.occurance = currentOccurance;
                int? ID =  HttpContext.Session.GetInt32("CurrentUserID");
                int realId = (int) ID;
                User currentuser = dbContext.Users.FirstOrDefault(u=>u.UserId == realId);
                
                int creatorID = currentOccurance.CreatorId;
                User Creator = dbContext.Users.FirstOrDefault(u=>u.UserId == creatorID);
                ViewBag.creator = Creator;
                return View("ViewOccurance");
            }
            else
            {
                return Redirect("/");
            }
        }

        [HttpGet("delete/{occuranceID}")]
        public IActionResult DeleteOccurance(int occuranceID)
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                Occurance currentOccurance = dbContext.Occurances.Where(w=>w.OccuranceId == occuranceID).FirstOrDefault();
                dbContext.Remove(currentOccurance);
                dbContext.SaveChanges();
                return Redirect("/dashboard");
            }
            else
            {
                return Redirect("/");
            }
        }

        [HttpGet("join/{occuranceID}")]
        public IActionResult Join(int occuranceID)
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                int? ID =  HttpContext.Session.GetInt32("CurrentUserID");
                int realId = (int) ID;
                Association newAssociation = new Association {OccuranceId = occuranceID, UserId = realId};
                dbContext.Associations.Add(newAssociation);
                dbContext.SaveChanges();
                return Redirect("/dashboard");
            }
            else
            {
                return Redirect("/");
            }
        }

        [HttpGet("leave/{occuranceID}")]
        public IActionResult Leave(int occuranceID)
        {
            if(HttpContext.Session.GetString("CurrentUser") != null)
            {
                int? ID =  HttpContext.Session.GetInt32("CurrentUserID");
                int realId = (int) ID;
                Occurance currentOccurance = dbContext.Occurances.Include(w=>w.Attendees).Where(w=>w.OccuranceId == occuranceID).FirstOrDefault();
                Association UserToLeave = currentOccurance.Attendees.FirstOrDefault(u=>u.UserId == realId);
                dbContext.Associations.Remove(UserToLeave);
                dbContext.SaveChanges();
                return Redirect("/dashboard");
            }
            else
            {
                return Redirect("/");
            } 
        }
    }
}
