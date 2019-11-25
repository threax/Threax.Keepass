using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Threax.AspNetCore.ExceptionFilter;
using Threax.AspNetCore.Halcyon.Ext;

namespace KeePassWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Bearer)]
    public class EndpointDocController : Controller
    {
        IEndpointDocBuilder descriptionProvider;

        public EndpointDocController(IEndpointDocBuilder descriptionProvider)
        {
            this.descriptionProvider = descriptionProvider;
        }

        [HttpGet("{groupName}/{method}/{*relativePath}")]
        [HalRel(HalDocEndpointInfo.DefaultRels.Get)]
        [AllowAnonymous]
        public Task<EndpointDoc> Get(String groupName, String method, String relativePath, EndpointDocQuery docQuery)
        {
            try
            {
                return descriptionProvider.GetDoc(groupName, method, relativePath, new EndpointDocBuilderOptions()
                {
                    User = User,
                    IncludeRequest = docQuery.IncludeRequest,
                    IncludeResponse = docQuery.IncludeResponse
                });
            }
            catch (UnauthorizedAccessException)
            {
                throw new ErrorResultException("Unauthorized", HttpStatusCode.Unauthorized);
            }
        }
    }
}
