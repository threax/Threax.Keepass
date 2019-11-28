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
    public partial class ItemsController : Controller
    {
        private IItemRepository repo;

        public ItemsController(IItemRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<ItemCollection> List([FromQuery] ItemQuery query)
        {
            return await repo.List(query);
        }

        [HttpGet("{ItemId}")]
        [HalRel(CrudRels.Get)]
        public async Task<Item> Get(Guid itemId)
        {
            return await repo.Get(itemId);
        }

        [HttpGet("Password/{ItemId}")]
        [HalRel("GetPassword")]
        public async Task<PasswordInfo> GetPassword(Guid itemId)
        {
            return await repo.GetPassword(itemId);
        }

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate("Cannot add new item")]
        public async Task<Item> Add([FromBody]ItemInput item)
        {
            return await repo.Add(item);
        }

        [HttpPut("{ItemId}")]
        [HalRel(CrudRels.Update)]
        [AutoValidate("Cannot update item")]
        public async Task<Item> Update(Guid itemId, [FromBody]ItemInput item)
        {
            return await repo.Update(itemId, item);
        }

        [HttpDelete("{ItemId}")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Guid itemId)
        {
            await repo.Delete(itemId);
        }
    }
}