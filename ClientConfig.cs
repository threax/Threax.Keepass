using System;
using HtmlRapier.TagHelpers;
using Newtonsoft.Json;

namespace KeePassWeb
{
    /// <summary>
    /// Client side configuration, copied onto pages returned to client, so don't include secrets.
    /// </summary>
    public class ClientConfig : ClientConfigBase
    {
        /// <summary>
        /// The url of the app's service, defaults to ~/api. You can
        /// specify an absolute url here if you want.
        /// </summary>
        [ExpandHostPath]
        public string ServiceUrl { get; set; } = "~/api";

        [ExpandHostPath]
        public string DbStatusUrl { get; set; } = "~/api/KeepassDatabase";
    }
}