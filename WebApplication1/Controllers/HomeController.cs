using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private DBContext dbContext;
        public HomeController(DBContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserHomePage()
        {
            return View(dbContext.Users.ToList());
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (CheckRegister(model))
            {
                dbContext.Users.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["RegisterError"] = "Email is already exist";
                return RedirectToAction("Index");
            }
        }
        public Boolean CheckRegister(User model)
        {
            if(!dbContext.Users.Any(x => x.Email.Equals(model.Email)))
            {
                return true;
            }
            else { 

                return false; 
            }
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            
            if (CheckLoginEmail(model))
            {
                if (CheckLoginPassword(model))
                {
                    TempData["User"] = model.Email;
                    return RedirectToAction("UserHomePage");
                }
                else
                {
                    TempData["LoginError"] = "Email or password is incorrect";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Something was happened wrong, please try again";
                return RedirectToAction("Index");
            }
        }

        public Boolean CheckLoginEmail(User model)
        {
            var user = dbContext.Users.Where(x => x.Email == model.Email);
            if (user.Any())
            {
                CheckLoginPassword(model);
                return true;
            }
            else return false;

        }
        public Boolean CheckLoginPassword(User model)
        {
            var user = dbContext.Users.Where(x => x.Password == model.Password);
            if (user.Any())
            {
                return true;
            }
            else
            {
                
                return false;
            }

        }

        //public IActionResult DeleteUser(int id)
        //{
        //    var model=dbContext.Users.Find(id);
        //    dbContext.Remove(model);
        //    dbContext.SaveChanges();
        //    return RedirectToAction("UserHomePage");
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}