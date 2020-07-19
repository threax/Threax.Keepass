using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Threax.ProgressiveWebApp;
using Threax.AspNetCore.Mvc.CacheUi;

namespace Threax.Keepass.Controllers
{
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Cookies)]
    public partial class HomeController : CacheUiController
    {
        public HomeController(ICacheUiBuilder builder)
            : base(builder)
        {

        }

        public Task<IActionResult> Item()
        {
            return CacheUiView();
        }

        public IActionResult Index()
        {
            return Redirect("~/Item");
        }

        public Task<IActionResult> Header()
        {
            return CacheUiView();
        }

        public Task<IActionResult> Footer()
        {
            return CacheUiView();
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
            return Json(webManifestProvider.CreateManifest(Url));
        }
    }
}
