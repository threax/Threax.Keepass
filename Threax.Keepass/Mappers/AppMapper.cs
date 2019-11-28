using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.Keepass.Mappers
{
    /// <summary>
    /// The app mapper defines all the object mappings that this application can perform.
    /// Usually this is just a thin wrapper over automapper, but it establishes what mappings
    /// are supported and enables more advanced mappings between multiple objects.
    /// </summary>
    public partial class AppMapper
    {
        private IMapper mapper;

        public AppMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
