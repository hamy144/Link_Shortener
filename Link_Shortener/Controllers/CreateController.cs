using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Link_Shortener.Controllers
{
    //This isn't ideal for a frontend but is quick and dirt,
    //would prefer a seperate project for a react app
    [Route("Create")]
    [Controller]
    public class CreateController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View("index");
        }
    }
}