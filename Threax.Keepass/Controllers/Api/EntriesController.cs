using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Threax.Keepass.Repository;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.Keepass.ViewModels;
using Threax.Keepass.InputModels;
using Microsoft.AspNetCore.Authorization;

namespace Threax.Keepass.Controllers.Api
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Bearer)]
    public partial class EntriesController : Controller
    {
        private IEntryRepository repo;

        public EntriesController(IEntryRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<EntryCollection> List([FromQuery] EntryQuery query)
        {
            return await repo.List(query);
        }

        [HttpGet("{ItemId}")]
        [HalRel(CrudRels.Get)]
        public async Task<Entry> Get(Guid itemId)
        {
            return await repo.Get(itemId);
        }

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate("Cannot add new entry")]
        public async Task<Entry> Add([FromBody]EntryInput entry)
        {
            return await repo.Add(entry);
        }

        [HttpPut("{ItemId}")]
        [HalRel(CrudRels.Update)]
        [AutoValidate("Cannot update entry")]
        public async Task<Entry> Update(Guid itemId, [FromBody]EntryInput entry)
        {
            return await repo.Update(itemId, entry);
        }

        [HttpDelete("{ItemId}")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Guid itemId)
        {
            await repo.Delete(itemId);
        }
    }
}