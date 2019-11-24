using KeePassWeb.InputModels;
using KeePassWeb.Services;
using KeePassWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace KeePassWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Bearer)]
    public class KeepassDatabaseController : Controller
    {
        private IKeePassService keepass;

        public KeepassDatabaseController(IKeePassService keepass)
        {
            this.keepass = keepass;
        }

        [HttpGet]
        [HalRel("Status")]
        public async Task<DbStatus> Status()
        {
            return new DbStatus()
            {
                DbClosed = !await keepass.IsOpen()
            };
        }

        [Route("Open")]
        [HttpPost]
        [HalRel("OpenDb")]
        [AutoValidate("Cannot open database")]
        public async Task<DbStatus> Open([FromBody]OpenDbInput input)
        {
            await keepass.Open(input.DatabasePassword);
            return new DbStatus()
            {
                DbClosed = !await keepass.IsOpen()
            };
        }

        [Route("Close")]
        [HttpPost]
        [HalRel("CloseDb")]
        [AutoValidate("Cannot close database")]
        public async Task<DbStatus> Close()
        {
            await keepass.Close();
            return new DbStatus()
            {
                DbClosed = !await keepass.IsOpen()
            };
        }
    }
}
