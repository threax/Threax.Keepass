using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using KeePassWeb.Controllers.Api;

namespace KeePassWeb.ViewModels
{
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.List), "ListItems")]
    [HalActionLink(typeof(ItemsController), nameof(ItemsController.Add), "AddItem")]
    public partial class EntryPoint
    {
        
    }
}