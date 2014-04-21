using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Geoculture.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public string RandomNumber()
        {
            try
            {
                int a = int.Parse(Request.Form["a"]);
                int b = int.Parse(Request.Form["b"]);
                int r = new Random().Next(a, b);
                return r.ToString();
            }
            catch (Exception e)
            {
                return "Bad-bad-bad request!";
            }
        }

    }
}
