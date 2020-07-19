using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Threax.AspNetCore.ExceptionFilter;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.Keepass.Controllers.Api
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Bearer)]
    public class EndpointDocController : Controller
    {
        private readonly IEndpointDocBuilder descriptionProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EndpointDocController(IEndpointDocBuilder descriptionProvider, IHttpContextAccessor httpContextAccessor)
        {
            this.descriptionProvider = descriptionProvider;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{version}/{groupName}/{method}/{*relativePath}")]
        [HalRel(HalDocEndpointInfo.DefaultRels.Get)]
        [AllowAnonymous]
        public async Task<EndpointDoc> Get(String version, String groupName, String method, String relativePath, EndpointDocQuery docQuery)
        {
            try
            {
                var doc = await descriptionProvider.GetDoc(groupName, method, relativePath, new EndpointDocBuilderOptions()
                {
                    User = User,
                    IncludeRequest = docQuery.IncludeRequest,
                    IncludeResponse = docQuery.IncludeResponse
                });

                if (doc.Cacheable && version != "nocache")
                {
                    httpContextAccessor.HttpContext.Response.Headers["Cache-Control"] = "private, max-age=2592000, stale-while-revalidate=86400, immutable";
                }

                return doc;
            }
            catch (UnauthorizedAccessException)
            {
                throw new ErrorResultException("Unauthorized", HttpStatusCode.Unauthorized);
            }
        }
    }
}