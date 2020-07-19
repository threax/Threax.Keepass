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
        private readonly AppConfig appConfig;

        public EndpointDocController(IEndpointDocBuilder descriptionProvider, IHttpContextAccessor httpContextAccessor, AppConfig appConfig)
        {
            this.descriptionProvider = descriptionProvider;
            this.httpContextAccessor = httpContextAccessor;
            this.appConfig = appConfig;
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
                    httpContextAccessor.HttpContext.Response.Headers["Cache-Control"] = appConfig.CacheControlHeaderString;
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