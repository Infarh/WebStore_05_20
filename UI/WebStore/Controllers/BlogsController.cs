using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult BlogSingle() => View();
    }
}
