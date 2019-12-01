using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Threax.ProgressiveWebApp;

namespace Threax.Keepass.Controllers
{
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Cookies)]
    public partial class HomeController : Controller
    {
        public IActionResult Item()
        {
            return View();
        }
        public IActionResult Index()
        {
            return Redirect("~/Item");
        }

        [AllowAnonymous]
        public IActionResult AppStart()
        {
            return View();
        }

        [Route("webmanifest.json")]
        [AllowAnonymous]
        public IActionResult Manifest([FromServices] IWebManifestProvider webManifestProvider)
        {
            return Json(webManifestProvider.CreateManifest());
        }
    }
}
