using System;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Throw(string id) => throw new ApplicationException(id);

        public IActionResult SomeAction() => View();

        public IActionResult Error404() => View();

        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult ContactUs() => View();

        public IActionResult ErrorStatus(string Code)
        {
            switch (Code)
            {
                default:
                    return Content($"Error code:{Code}");
                case "404":
                    return RedirectToAction("Error404", "Home");
            }
        }
    }
}