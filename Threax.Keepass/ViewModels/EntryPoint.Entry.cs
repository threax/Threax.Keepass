using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.Keepass.Controllers.Api;

namespace Threax.Keepass.ViewModels
{
    [HalActionLink(typeof(EntriesController), nameof(EntriesController.List), "ListEntries")]
    [HalActionLink(typeof(EntriesController), nameof(EntriesController.Add), "AddEntry")]
    public partial class EntryPoint
    {
        
    }
}