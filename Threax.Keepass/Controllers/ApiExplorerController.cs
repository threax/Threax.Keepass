using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Threax.Keepass.Controllers
{
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Cookies)]
    public class ApiExplorerController : Controller
    {
        /// <summary>
        /// Not the best way to configure this, but this is a really one off thing, set this to true to enable the api explorer.
        /// </summary>
        public static bool Allow { get; set; } = false;

        public IActionResult Index()
        {
            if (Allow)
            {
                return View();
            }
            return StatusCode((int)HttpStatusCode.NotFound);
        }
    }
}
