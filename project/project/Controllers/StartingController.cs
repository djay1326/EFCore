using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project.Data;
using project.Models;
using project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace project.Controllers
{
    public class StartingController : Controller
    {
        private readonly HelperlandContext _DbContext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<StartingController> logger;
        //private readonly RoleManager<IdentityRole> roleManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public StartingController(UserManager<User> userManager,
                                  SignInManager<User> signInManager,
                                  ILogger<StartingController> logger,
                                  RoleManager<IdentityRole<int>> roleManager,
                                  HelperlandContext DbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.roleManager = roleManager;
            _DbContext = DbContext;
        }

        //public StartingController(HelperlandContext DbContext)
        //{
        //    _DbContext = DbContext;
        //}

        [HttpGet]
        public IActionResult newaccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> newaccount(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, FirstName=model.FirstName };
                var result = await userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {
                    //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var confirmationLink = Url.Action("confirmEmail", "Starting",
                    //                    new { userId = user.Id, token = token}, Request.Scheme);
                    //logger.Log(LogLevel.Warning,confirmationLink);

                    //ViewBag.ErrorTitle = "Registration Successful!";
                    //ViewBag.ErrorMessage = "Before you Login, please confirm your email by clicking on the link we have emailed you";
                    //return View("smallDisplay");
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                    await signInManager.SignInAsync(user, isPersistent: false);
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("confirmemail", "Starting",
                                                                         new { UserId = user.Id, code = code });
                    string baseUrl ="Please confirm your account by clicking here:  " +
                        string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host) + callbackUrl;

                    MailMessage ms = new MailMessage();
                    ms.To.Add(model.Email);
                    ms.From = new MailAddress("ravi.smith.1326@gmail.com");
                    ms.Subject = "Account Confirmation link";
                    ms.Body = baseUrl;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.Port = 587;


                    NetworkCredential NetworkCred = new NetworkCredential("ravi.smith.1326@gmail.com", "Sandwich#");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(ms);
                    ViewBag.Message = "mail has been sent successfully ";
                    //return RedirectToAction("ForgotPasswordConfirmation");

                    return RedirectToAction("AccountConfirmation");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult AccountConfirmation()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>confirmemail(string code, string email, int UserId)
        {
            var x = UserId.ToString();
            var user = await userManager.FindByIdAsync(x);
            if(user == null)
            {
                return View("smallDisplay");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded?"confirmemail":"smallDisplay");
        }

        //[HttpGet][HttpPost]
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        public IActionResult about()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Starting");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user!=null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user,model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet!");
                    return View(model);
                }
                //var user = new IdentityUser { UserName = (model.FirstName), Email = model.Email };
                var result = await signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);


                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("homepage");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Credentials");
                
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult forgotpwd()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            //string resetCode = Guid.NewGuid().ToString();
            //var verifyUrl = "/Account/ResetPassword/" + resetCode;
            // var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            //var X = _DbContext.Userr.FirstOrDefault((System.Linq.Expressions.Expression<Func<Userr, bool>>)((Userr p) => (bool)p.Email.Equals((string)model.Email))).UserId;
            //string baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
            //var activationUrl = $"{baseUrl}/Starting/ForgotPwd?UserId={X}";

            //var get_user = _DbContext.Userr.FirstOrDefault((System.Linq.Expressions.Expression<Func<Userr, bool>>)(p => (bool)p.Email.Equals((string)model.Email)));

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null /*|| !(await UserManager.IsEmailConfirmedAsync(user.Id))*/)
                {
                    return View("smallDisplay");
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                //    var encodedToken = Encoding.UTF8.GetBytes(code);
                //    var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var callbackUrl = Url.Action("resetpwd", "Starting",
            new { UserId = user.Id, code = code });
            //    await userManager.SendEmailAsync(user.Id, "Reset Password",
            //"Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                //string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";
                string baseUrl = "Please reset your password by clicking here: " + string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host) 
                    + callbackUrl 
                    + "\n Your Reset Password link will expire in 1 minute" ;
                

                //    return View("ForgotPasswordConfirmation");
                MailMessage ms = new MailMessage();
                ms.To.Add(model.Email);
                ms.From = new MailAddress("ravi.smith.1326@gmail.com");
                ms.Subject = "Forgot Password reset link";
                ms.Body = baseUrl;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Port = 587;


                NetworkCredential NetworkCred = new NetworkCredential("ravi.smith.1326@gmail.com", "Sandwich#");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Send(ms);
                ViewBag.Message = "mail has been sent successfully ";
                return RedirectToAction("ForgotPasswordConfirmation");
           
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult resetpwd(string code, string email, int UserId)
        {
            var model = new ResetPasswordModel { Token = code, Email = email, Id= UserId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> resetpwd(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordModel);
            }
            var x = (resetPasswordModel.Id).ToString();
            var user = await userManager.FindByIdAsync(x);
            if (user == null)
            {
              return RedirectToAction("ResetPasswordConfirmation");
            }
            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction("ResetPasswordConfirmation");
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        //}
        //[HttpPost]
        //public async Task<IActionResult> forgotpwd(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //        }
        //    }
        //    return View(model);
        //}

        public IActionResult homepage()
        {
            int userId = int.Parse(userManager.GetUserId(User));
            //DateTime zz = DateTime.Now;
            if (User.IsInRole("Manager"))
            {
                var query1 = (from s in _DbContext.leave
                              join y in _DbContext.Users
                              on s.userid equals y.Id
                              //let fromDate = DateTime.ParseExact(s.fromDate,"yyyyMMdd",null)
                              where
                              (y.managerid == userId) &&
                              //(DateTime.Parse(s.fromDate.ToString()).ToString("yyyy-MM-dd") == DateTime.Today.ToString("yyyy-MM-dd")) 
                              //DateTime.Parse(s.fromDate.Value.ToShortDateString()).ToString("yyyy-MM-dd") == "2022-05-18"
                              (s.fromDate.Value.Date == DateTime.Now.Date)
                              //(s.fromDate == DateTime.Now)
                              && (s.statusid == 2)
                              select s
                              ).ToList();
                ViewBag.onleave = query1.Count();
                return View(query1);
            }
            else if (User.IsInRole("Employee"))
            {
                var query2 = (from s in _DbContext.leave
                              where (s.userid == userId) && (s.statusid == 2) && (s.fromDate.Value.Date.Month == DateTime.Now.Date.Month)
                              select s
                              ).ToList();
                
                ViewBag.onleave2 = query2.Count();
                return View(query2);
            }
            else if (User.IsInRole("Admin"))
            {
                var query3 = (from s in _DbContext.leave
                              where (s.roleid == 2) && (s.statusid == 2) && (s.fromDate.Value.Date == DateTime.Now.Date)
                              select s).ToList();
                var query4 = (from s in _DbContext.leave
                              where (s.roleid == 3) && (s.statusid == 2) && (s.fromDate.Value.Date == DateTime.Now.Date)
                              select s).ToList();
                var query5 = (from h in _DbContext.holidays
                              where h.onDate.Date.Month == DateTime.Now.Date.Month
                              select h).ToList();
                ViewBag.onleave3 = query3.Count();
                ViewBag.onleave4 = query4.Count();
                ViewBag.onleave5 = query5.Count();
                return View(query4);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult listroles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> editrole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
                return View("notfound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        public IActionResult addleaverequest()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> leavepage(string currentFilter, string searchString, int? pageNumber)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (User.IsInRole("Manager"))
            {
                var students = ((from s in _DbContext.Users
                                join u in _DbContext.leave
                                on s.Id  equals u.userid
                                where s.managerid == userId
                                 select new leave
                                 {
                                     leaveid = u.leaveid,
                                     fromDate = u.fromDate,
                                     toDate = u.toDate,
                                     statusid = u.statusid,
                                     userid = u.userid,
                                     roleid = u.roleid
                                 }).Union(
                                    from s in _DbContext.Users
                                    join u in _DbContext.leave
                                    on s.Id equals u.userid
                                    where s.Id == userId
                                  select new leave { 
                                    leaveid = u.leaveid,
                                    fromDate = u.fromDate,
                                    toDate = u.toDate,
                                    statusid = u.statusid,
                                      userid = u.userid,
                                      roleid = u.roleid
                                  })).ToList();

                
                return View(students);
            }
            else if (User.IsInRole("Admin"))
            {
                //List<UserAddress> u = _DbContext.UserAddress.Where(x => x.UserId == ty).ToList();
                List<leave> l = _DbContext.leave.ToList();
                return View(l);
            }
            else
            {

                var students = (from s in _DbContext.leave
                           where s.userid == userId 
                            select s);
                return View(students);
            }
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    students = students.Where(s => s.reason.Contains(searchString)
            //                           /*|| s.lastname.Contains(searchString)*/);
            //}
            //List<leave> x = _DbContext.leave.Where(z => z.userid == userId).ToList();
            //return View(x);
            //int pageSize = 2;
            //return View(await PaginatedList<leave>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public IActionResult salary(DateTime monthyear)
        {
            
            int userId = int.Parse(userManager.GetUserId(User));
            if (User.IsInRole("Manager"))
            {
                var data = (from u in _DbContext.Users
                            join s in _DbContext.salary
                            on u.Id equals s.userid
                            where u.managerid == userId
                            select s).ToList();
                var result1 = data.Where(x => x.createddate == monthyear).ToList();
                ViewBag.msg1 = monthyear.ToString("yyyy-MM");
                return View(result1);
            }
            var query = (from s in _DbContext.salary
                         where (s.createddate == monthyear)
                         select s).ToList();
            ViewBag.msg1 = monthyear.ToString("yyyy-MM");
            return View(query);
        }

        [HttpPost]
        public IActionResult leavepage(DateTime startdate, DateTime enddate,int? statusids,int? id)
        {
            var abc = id;
            DateTime xy = DateTime.Parse("01-01-0001 12:00:00 AM");
            DateTime uy = DateTime.Parse("01-01-9999 12:00:00 AM");
            if (enddate == xy)
            {
                enddate = DateTime.Parse("01-01-9999 12:00:00 AM");
            }
            int userId = int.Parse(userManager.GetUserId(User));

            if (User.IsInRole("Manager"))
            {
                var students = ((from s in _DbContext.Users
                                 join u in _DbContext.leave
                                 on s.Id equals u.userid
                                 where s.managerid == userId
                                 select new leave
                                 {
                                     leaveid = u.leaveid,
                                     fromDate = u.fromDate,
                                     toDate = u.toDate,
                                     statusid = u.statusid,
                                     userid = u.userid,
                                     roleid = u.roleid
                                 }).Union(
                                    from s in _DbContext.Users
                                    join u in _DbContext.leave
                                    on s.Id equals u.userid
                                    where s.Id == userId
                                    select new leave
                                    {
                                        leaveid = u.leaveid,
                                        fromDate = u.fromDate,
                                        toDate = u.toDate,
                                        statusid = u.statusid,
                                        userid = u.userid,
                                        roleid = u.roleid
                                    })).ToList();
                if (statusids == null)
                {
                    var result1 = students.Where(x => (x.fromDate >= startdate) && (x.toDate <= enddate)).ToList();
                    ViewBag.startdate = startdate.ToString("yyyy-MM-dd");
                    ViewBag.enddate = enddate.ToString("yyyy-MM-dd");
                    return View(result1);
                }
                else if (startdate == xy && enddate==xy)
                {
                    var result2 = students.Where(x => (x.statusid == statusids)).ToList();
                    //ViewBag.startdate = startdate.ToString("yyyy-MM-dd");
                    //ViewBag.enddate = enddate.ToString("yyyy-MM-dd");
                    return View(result2);
                }
                var result = students.Where(x => (x.fromDate >= startdate) && (x.toDate <= enddate) && (x.statusid == statusids)).ToList();
                ViewBag.startdate = startdate.ToString("yyyy-MM-dd");
                ViewBag.enddate = enddate.ToString("yyyy-MM-dd");
                return View(result);
            }

            else if (User.IsInRole("Admin"))
            {
                List<leave> leavedates = _DbContext.leave.ToList();
                if (statusids == null)
                {
                    var result1 = leavedates.Where(x => (x.fromDate >= startdate) && (x.toDate <= enddate)).ToList();
                    ViewBag.startdate = startdate.ToString("yyyy-MM-dd");
                    ViewBag.enddate = enddate.ToString("yyyy-MM-dd");
                    return View(result1);
                }
                else if (startdate == xy && enddate == uy)
                {
                    var result2 = leavedates.Where(x => (x.statusid == statusids)).ToList();
                    if(statusids == 1)
                    {
                        ViewBag.status = "Pending";
                    }
                    else if(statusids == 2)
                    {
                        ViewBag.status = "Accepted";
                    }
                    else if(statusids == 3)
                    {
                        ViewBag.status = "Rejected";
                    }
                    return View(result2);
                }
                var result = leavedates.Where(x => (x.fromDate >= startdate) && (x.toDate <= enddate) && (x.statusid == statusids)).ToList();
                ViewBag.startdate = startdate.ToString("yyyy-MM-dd");
                ViewBag.enddate = enddate.ToString("yyyy-MM-dd");
                return View(result);
            }
            
            if  (statusids == null)
            {
                //var query1 = (from x in _DbContext.leave
                //              where (x.userid == userId)
                //              select x
                //         ).ToList();
                var query1 = (from x in _DbContext.leave
                              where ((x.fromDate >= startdate) && (x.toDate <= enddate) && (x.userid == userId))                              
                              select x
                         ).ToList();
                return View(query1);
            }
            var query = (from x in _DbContext.leave
                         where ((x.fromDate >= startdate) && (x.toDate <= enddate) && (x.userid == userId) && (x.statusid == statusids))  
                         //((x.statusid == searchstatus) && (x.userid == userId)) ||
                         //((x.fromDate >= startdate) && (x.toDate <= enddate) && (x.userid == userId))
                         select x
                         ).ToList();
            return View(query);
        }

                
        public IActionResult salaryclear()
        {
            int userId = int.Parse(userManager.GetUserId(User));

            if (User.IsInRole("Manager"))
            {
                //var data = (from u in _DbContext.Users
                //            join s in _DbContext.salary
                //            on u.Id equals s.userid
                //            where u.managerid == userId
                //            select s).ToList();
                var data = _DbContext.salary.Where(x => x.createddate.Value.Date.Month == DateTime.Now.Date.Month && x.createddate.Value.Date.Year == DateTime.Now.Date.Year).ToList();

                return View("salary",data);
            }
            else if (User.IsInRole("Admin"))
            {
                var data2 = _DbContext.salary.ToList();
                return View("salary", data2);
            }
            var query = (from s in _DbContext.salary
                         where (s.userid == userId)
                         select s).ToList();
            return View("salary", query);
        }
        public IActionResult display()
        {
            
            int userId = int.Parse(userManager.GetUserId(User));

            if (User.IsInRole("Manager"))
            {
                var students = ((from s in _DbContext.Users
                                 join u in _DbContext.leave
                                 on s.Id equals u.userid
                                 where s.managerid == userId
                                 select new leave
                                 {
                                     leaveid = u.leaveid,
                                     fromDate = u.fromDate,
                                     toDate = u.toDate,
                                     statusid = u.statusid,
                                     userid = u.userid,
                                     roleid = u.roleid
                                 }).Union(
                                    from s in _DbContext.Users
                                    join u in _DbContext.leave
                                    on s.Id equals u.userid
                                    where s.Id == userId
                                    select new leave
                                    {
                                        leaveid = u.leaveid,
                                        fromDate = u.fromDate,
                                        toDate = u.toDate,
                                        statusid = u.statusid,
                                        userid = u.userid,
                                        roleid = u.roleid
                                    })).ToList();
                return View("leavepage", students);
            }
            else if (User.IsInRole("Admin"))
            {
                List<leave> leavedates = _DbContext.leave.ToList();
                return View("leavepage", leavedates);
            }
            var query = (from x in _DbContext.leave
                         where (x.userid == userId)
                         select x
                         ).ToList();
            return View("leavepage",query);

        }

        [HttpPost]
        public IActionResult leavesave(leave saveleavedb)
        {
            var y = User.Identity.Name;
            var abc = _DbContext.Users.Where(x => x.Email == y).FirstOrDefault();
            saveleavedb.userid = abc.Id;
            var z = saveleavedb.userid;
            var def = _DbContext.UserRoles.Where(x => x.UserId == z).FirstOrDefault();
            saveleavedb.roleid = def.RoleId;
            saveleavedb.statusid = 1;
            _DbContext.leave.Add(saveleavedb);
            _DbContext.SaveChanges();
            return RedirectToAction("leavepage");
        }

        public string deleteTab(int i)
        {
            leave x = _DbContext.leave.Where(z => z.leaveid == i).FirstOrDefault();
            _DbContext.leave.Remove(x);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult getleavedata(int id)
        {
            leave z = _DbContext.leave.Where(x => x.leaveid == id).FirstOrDefault();
            return View(z);
        }

        public IActionResult getholidaydata(int id)
        {
            List<holidays> holidayinfo = _DbContext.holidays.ToList();
            return View(holidayinfo);
        }

        public bool updateleave([FromBody] leave change)
        {
            if (User.IsInRole("Manager"))
            {
                leave z = _DbContext.leave.Where(x => x.leaveid == change.leaveid).FirstOrDefault();
                z.fromDate = change.fromDate;
                z.toDate = change.toDate;
                z.reason = change.reason;
                _DbContext.leave.Update(z);
                _DbContext.SaveChanges();
                //return RedirectToAction("homepage");
                //int userId = int.Parse(userManager.GetUserId(User));
                return true;
                //var students = ((from s in _DbContext.Users
                //                 join t in _DbContext.leave
                //                 on s.Id equals t.userid
                //                 where s.managerid == userId
                //                 select new leave
                //                 {
                //                     leaveid = t.leaveid,
                //                     fromDate = t.fromDate,
                //                     toDate = t.toDate,
                //                     statusid = t.statusid,
                //                     userid = t.userid,
                //                     roleid = t.roleid
                //                 }).Union(
                //                    from s in _DbContext.Users
                //                    join t in _DbContext.leave
                //                    on s.Id equals t.userid
                //                    where s.Id == userId
                //                    select new leave
                //                    {
                //                        leaveid = t.leaveid,
                //                        fromDate = t.fromDate,
                //                        toDate = t.toDate,
                //                        statusid = t.statusid,
                //                        userid = t.userid,
                //                        roleid = t.roleid
                //                    })).ToList();
                //return View("leavepage",students);
            }
            leave u = _DbContext.leave.Where(x => x.leaveid == change.leaveid).FirstOrDefault();
            u.fromDate = change.fromDate;
            u.toDate = change.toDate;
            u.reason = change.reason;
            _DbContext.leave.Update(u);
            _DbContext.SaveChanges();
            //int userIdd = int.Parse(userManager.GetUserId(User));
            //List<leave> p = _DbContext.leave.Where(z => z.userid == userIdd).ToList();
            //return RedirectToAction("leavepage");
            return true;
        }

        public IActionResult activate(int id)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            leave l = _DbContext.leave.Where(x => x.leaveid == id).FirstOrDefault();
            l.statusid = 2;
            _DbContext.leave.Update(l);
            _DbContext.SaveChanges();
            return RedirectToAction("leavepage");
        }

        public IActionResult deactivate(int id)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            leave l = _DbContext.leave.Where(x => x.userid == id).FirstOrDefault();
            l.statusid = 3;
            _DbContext.leave.Update(l);
            _DbContext.SaveChanges();
            return RedirectToAction("leavepage");
        }

        [HttpGet]
        public async Task<IActionResult> salary(string currentFilter, string searchString, int? pageNumber)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            if (User.IsInRole("Manager"))
            {
                var emp = (from u in _DbContext.Users
                           join s in _DbContext.salary
                           on u.Id equals s.userid                           
                           where (u.managerid == userId) && (s.createddate.Value.Date.Month == DateTime.Now.Date.Month) & (s.createddate.Value.Date.Year == DateTime.Now.Date.Year)
                           select s).ToList();
                //&& (s.fromDate.Value.Date.Month == DateTime.Now.Date.Month) 
                return View(emp);

            }
            else if (User.IsInRole("Admin"))
            {
                List<salary> salaryinfo = _DbContext.salary.Where(s=> s.createddate.Value.Date.Month == DateTime.Now.Date.Month && s.createddate.Value.Date.Year == DateTime.Now.Date.Year).ToList();
                return View(salaryinfo);
            }
            else
            {
                var students = (from s in _DbContext.salary
                                where s.userid == userId
                                select s);
                return View(students);
            }
            
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    students = students.Where(s => s.basic.ToString().Contains(searchString)
            //                           /*|| s.lastname.Contains(searchString)*/);
            //}
            //List<salary> x = _DbContext.salary.Where(z => z.userid == userId).ToList();
            //return View(x);
            //int pageSize = 1;
            //return View(await PaginatedList<salary>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        //[HttpPost]
        //public IActionResult salary(DateTime startdate, DateTime enddate)
        //{
        //    DateTime xy = DateTime.Parse("01-01-0001 12:00:00 AM");
        //    int userId = int.Parse(userManager.GetUserId(User));
        //    if((startdate == xy)&& (enddate == xy))
        //    {
        //        var query1 = (from s in _DbContext.salary
        //                      where s.userid == userId
        //                      select s).ToList();
        //        return View(query1);
        //    }
        //    else if (enddate == xy)
        //    {
        //        var query2 = (from s in _DbContext.salary
        //                     where (s.createddate >= startdate) && (s.userid == userId)
        //                     select s
        //                 ).ToList();
        //        return View(query2);
        //    }
        //    else if (startdate == xy)
        //    {
        //        var query3 = (from s in _DbContext.salary
        //                      where (s.createddate <= enddate) && (s.userid == userId)
        //                      select s
        //                 ).ToList();
        //        return View(query3);
        //    }
        //    var query = (from s in _DbContext.salary
        //                 where (s.createddate>=startdate) && (s.createddate <= enddate) && (s.userid == userId)
        //                 select s
        //                 ).ToList();
        //    return View(query);
        //}

        public IActionResult displayclear()
        {

            int userId = int.Parse(userManager.GetUserId(User));

            var query = (from x in _DbContext.salary
                         where (x.userid == userId)
                         select x
                         ).ToList();
            return View("salary", query);
        }

        public IActionResult unassignedroles()
        {
            var users = userManager.Users;
            return View(users);
        }

        public IActionResult assignmanager(int id)
        {
            var y = id;
            List<IdentityUserRole<int>> u = new List<IdentityUserRole<int>>();
            //u = (from x in _DbContext.UserRoles
            //     where x.RoleId == 2

            //     select x
            //     //select new IdentityUserRole<int>
            //     //{
            //     //    UserId = x.UserId
            //     //}              
            //     ).ToList();
            var query = (from t in _DbContext.Users
                 join r in _DbContext.UserRoles
                 on t.Id equals r.UserId
                 where r.RoleId == 2
                 select new mixed
                 {
                     username = t.UserName,
                     userid = t.Id
                 }
                 ).ToList();
            //u.Insert(0,new IdentityUserRole<int> {UserId=0,RoleId=0 });
            ViewBag.message = query;
            ViewBag.id = y;
            return View();
        }

        //[HttpPost]
        //public IActionResult assignmanager(IdentityUserRole<int> u)
        //{
        //    _DbContext.Add(u);
        //    _DbContext.SaveChanges();
        //    ViewBag.msg = "The selected manager" + "is assigned successfully ";
        //    return View();
        //}

        public string updatemanager([FromBody] User x)
        {
            var user = _DbContext.Users.Where(y => y.Id == x.Id).FirstOrDefault();
            user.managerid = x.managerid;
            user.manager = x.manager;
            _DbContext.Users.Update(user);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult addsalary()
        {
            int userId = int.Parse(userManager.GetUserId(User));
            var y = User.Identity.Name;

            if (User.IsInRole("Manager"))
            {

            //var query = (from x in _DbContext.Users
            //             where x.manager == y
            //             select x).ToList();
            //(s.fromDate.Value.Date.Month == DateTime.Now.Date.Month)
            var query1 = (from x in _DbContext.Users
                         where x.managerid == userId
                          select new User
                          {
                              Id = x.Id,
                              UserName = x.UserName
                          }).Distinct().ToList();
            var query = (from x in _DbContext.Users
                         join s in _DbContext.salary
                         on x.Id equals s.userid
                         where (x.managerid == userId) && (s.createddate.Value.Date.Month == DateTime.Now.Date.Month) && (s.createddate.Value.Date.Year == DateTime.Now.Date.Year)
                         select new User
                         {
                             Id = x.Id,
                             UserName = x.UserName
                         }
                         ).ToList();
            if(query.Count == 0)
            {
                var query2 = _DbContext.Users.Where(x => x.managerid == userId).ToList();
                ViewBag.msg = query2;
                return View();
            }
            else if(query.Count != 0)
            {
                // var result = query1.Except(query).ToList();
                //var result = query1.RemoveAll(x => query.Contains(x));
                foreach(var item in query1.ToList())
                {
                    if (query.Any(x=>x.Id == item.Id))
                    {
                        query1.Remove(item);
                    }
                }
                ViewBag.msg = query1;
                return View();
            }
            }
            else if (User.IsInRole("Admin"))
            {
                var query11 = (from x in _DbContext.Users
                              where x.managerid == userId
                              select new User
                              {
                                  Id = x.Id,
                                  UserName = x.UserName
                              }).Distinct().ToList();
                var queryy = (from x in _DbContext.Users
                             join s in _DbContext.salary
                             on x.Id equals s.userid
                             where (x.managerid == userId) && (s.createddate.Value.Date.Month == DateTime.Now.Date.Month) && (s.createddate.Value.Date.Year == DateTime.Now.Date.Year)
                             select new User
                             {
                                 Id = x.Id,
                                 UserName = x.UserName
                             }
                         ).ToList();
                if (queryy.Count == 0)
                {
                    var query2 = _DbContext.Users.Where(x => x.managerid == userId).ToList();
                    ViewBag.msg = query2;
                    return View();
                }
                else if (queryy.Count != 0)
                {
                    // var result = query1.Except(query).ToList();
                    //var result = query1.RemoveAll(x => query.Contains(x));
                    foreach (var item in query11.ToList())
                    {
                        if (queryy.Any(x => x.Id == item.Id))
                        {
                            query11.Remove(item);
                        }
                    }
                    ViewBag.msg = query11;
                    return View();
                }
            }
            //ViewBag.msg = query;
            return View();
        }

        
        public string savesalary2([FromBody] salary savesalarydb)
        {
            var y = User.Identity.Name;
            //savesalarydb.username = 
            //var abc = savesalarydb.basic;
            //var def = savesalarydb.tax;
            //savesalarydb.final = abc - def;
            //savesalarydb.final = savesalarydb.basic - savesalarydb.tax;
            //_DbContext.salary.Where(x => x.Email == y).FirstOrDefault();
            salary s = new salary();
            s.createddate = DateTime.Parse(savesalarydb.date);
            s.basic = savesalarydb.basic;
            s.tax = savesalarydb.tax;
            s.final = savesalarydb.final;
            s.username = savesalarydb.username;
            s.userid = savesalarydb.userid;
            _DbContext.salary.Add(s);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult addemp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addemp(User user)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(userManager.GetUserId(User));
                var yy = User.Identity.Name;
                //int i = 1;
                //bool b = Convert.ToBoolean(i);
                //var user = new User { UserName = u.UserName, EmailConfirmed = b, managerid = userId, manager = yy,Email = u.UserName,
                //                    NormalizedEmail = u.UserName.ToUpper(), NormalizedUserName = u.UserName.ToUpper(), PasswordHash = u.PasswordHash
                //};
                //var result = await userManager.CreateAsync(user, u.PasswordHash);
                var users = new User { UserName = user.UserName, Email = user.UserName , EmailConfirmed = true, managerid = userId, manager = yy };
                var result = await userManager.CreateAsync(users, user.PasswordHash);
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(users, "Employee").Wait();
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //int i = 1;
                    //bool b = Convert.ToBoolean(i);
                    
                    //User userdata = new User();
                    
                    //uu.UserName = u.UserName;
                    //uu.PasswordHash = u.PasswordHash;
                    //uu.EmailConfirmed = b;
                    //uu.managerid = userId;
                    //uu.manager = yy;
                    //uu.Email = u.UserName;
                    //uu.NormalizedEmail = u.UserName.ToUpper();
                    //uu.NormalizedUserName = u.UserName.ToUpper();
                    //_DbContext.Users.Add(users);  I commented this bcoz CreateAsync at "result" is already adding/creating user in db so no need to write again.
                    //_DbContext.SaveChanges();      and no need to save it.
                    return RedirectToAction("homepage");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);


        }

        public IActionResult adminaddemp()
        {
            var query = (from t in _DbContext.Users
                         join r in _DbContext.UserRoles
                         on t.Id equals r.UserId
                         where r.RoleId == 2
                         select new mixed
                         {
                             username = t.UserName,
                             userid = t.Id
                         }
                 ).ToList();
            //u.Insert(0,new IdentityUserRole<int> {UserId=0,RoleId=0 });
            ViewBag.message = query;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> adminaddemp([FromBody] User x)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(userManager.GetUserId(User));
                var yy = User.Identity.Name;
                var users = new User { UserName = x.UserName, Email = x.UserName, EmailConfirmed = true, managerid = userId, manager = yy };
                var result = await userManager.CreateAsync(users, x.PasswordHash);
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(users, "Employee").Wait();
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //int i = 1;
                    //bool b = Convert.ToBoolean(i);

                    //User userdata = new User();

                    //uu.UserName = u.UserName;
                    //uu.PasswordHash = u.PasswordHash;
                    //uu.EmailConfirmed = b;
                    //uu.managerid = userId;
                    //uu.manager = yy;
                    //uu.Email = u.UserName;
                    //uu.NormalizedEmail = u.UserName.ToUpper();
                    //uu.NormalizedUserName = u.UserName.ToUpper();
                    //_DbContext.Users.Add(users);  I commented this bcoz CreateAsync at "result" is already adding/creating user in db so no need to write again.
                    //_DbContext.SaveChanges();      and no need to save it.
                    return RedirectToAction("homepage");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(x);            
        }


        public IActionResult adminaddman()
        {
            List<IdentityUserRole<int>> u = new List<IdentityUserRole<int>>();
            //u = (from x in _DbContext.UserRoles
            //     where x.RoleId == 2

            //     select x
            //     //select new IdentityUserRole<int>
            //     //{
            //     //    UserId = x.UserId
            //     //}              
            //     ).ToList();
            var query = (from t in _DbContext.Users
                         join r in _DbContext.UserRoles
                         on t.Id equals r.UserId
                         where r.RoleId == 3
                         select new mixed
                         {
                             username = t.UserName,
                             userid = t.Id
                         }
                 ).ToList();
            
            //u.Insert(0,new IdentityUserRole<int> {UserId=0,RoleId=0 });
            ViewBag.message = query;
            return View();
        }

        public IActionResult giveincrement(int id)
        {
            salary z = _DbContext.salary.Where(x => x.salaryid == id).FirstOrDefault();
            return View(z);
        }


        public string updatebyincrement([FromBody] salary change)
        {
            salary sal = _DbContext.salary.Where(x => x.salaryid == change.salaryid).FirstOrDefault();
            sal.basic = change.basic + change.increment;
            sal.tax = change.tax;
            sal.final = change.basic + change.increment - change.tax;
            _DbContext.salary.Update(sal);
            _DbContext.SaveChanges();
            return "true";
        }


        public string updateemptomanager([FromBody] User userdata)
        {            
            int userId = int.Parse(userManager.GetUserId(User));
            var managerName = User.Identity.Name;
            var user = userManager.FindByIdAsync(userdata.Id.ToString()).Result;
            user.managerid = userId;
            user.manager = managerName;
            userManager.RemoveFromRoleAsync(user, "Employee").Wait();
            userManager.AddToRoleAsync(user, "Manager").Wait();
            userManager.UpdateAsync(user);            
            return "true";
        }


        public IActionResult manuallycreatemanager()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> manuallycreatemanager(User user)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(userManager.GetUserId(User));
                var yy = User.Identity.Name;
                var users = new User { UserName = user.UserName, Email = user.UserName, EmailConfirmed = true, managerid = userId, manager = yy };
                var result = await userManager.CreateAsync(users, user.PasswordHash);
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(users, "Manager").Wait();
                    return RedirectToAction("unassignedroles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
    }

}
