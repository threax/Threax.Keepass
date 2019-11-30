using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.Keepass.Controllers.Api;

namespace Threax.Keepass.ViewModels
{
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.List), "ListItems")]
    public partial class EntryPoint
    {
        
    }
}