using KeePassWeb.Services;
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
    public class KeepassDatabaseController
    {
        private IKeePassService keepass;

        public KeepassDatabaseController(IKeePassService keepass)
        {
            this.keepass = keepass;
        }

        [HttpGet]
        [HalRel("Status")]
        public async Task Get()
        {
            await keepass.IsOpen();
        }

        [HttpPut]
        [HalRel("OpenDb")]
        [AutoValidate("Cannot open database")]
        public async Task Open()
        {
            await keepass.Open();
        }

        [HttpPut]
        [HalRel("CloseDb")]
        [AutoValidate("Cannot close database")]
        public async Task Close()
        {
            await keepass.Close();
        }
    }
}
