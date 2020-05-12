using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class WebAPITestController : Controller
    {
        private readonly IValueServices _ValueServices;

        public WebAPITestController(IValueServices ValueServices) => _ValueServices = ValueServices;

        public IActionResult Index() => View(_ValueServices.Get());
    }
}