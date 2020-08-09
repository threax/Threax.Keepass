using Microsoft.Extensions.DependencyInjection;

namespace Threax.Keepass
{
    /// <summary>
    /// The primary configuration for the app.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// The base url of the application, by default automatically discovered.
        /// </summary>
        public string BaseUrl { get; set; } = HalcyonConventionOptions.HostVariable;

        /// <summary>
        /// The connection string for the app's primary database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// True to turn on detailed error messages.
        /// </summary>
        public bool DetailedErrors { get; set; }

        /// <summary>
        /// The base path the app lives on. Used for cookie paths and to enforce the url spelling.
        /// Can be null to live on the root path.
        /// </summary>
        public string PathBase { get; set; }

        /// <summary>
        /// Should the api explorer be accessible, false by default. The api explorer is pure client side
        /// and will not expose methods the user cannot access on the service, so if I hit the explorer page
        /// logged in vs logged out it will behave differently. That said it is probably best to leave this
        /// off in production.
        /// </summary>
        public bool AllowApiExplorer { get; set; }

        /// <summary>
        /// Set this to true to use asset bundles instead of individual client side files.
        /// </summary>
        public bool UseAssetBundles { get; set; } = false;

        /// <summary>
        /// Set this to false to disable value provider resolution. This should be true for produciton or your value providers
        /// won't work, but you can set it to false in tools mode, which will keep the value providers from resolving when
        /// doing stuff like regenerating clients. The default is true.
        /// </summary>
        public bool EnableValueProviders { get; set; } = true;

        /// <summary>
        /// The token to use as a cache token. This is a unique id for a particular website build. If this is null a hash
        /// of the main library that contains Startup.cs will be used. You can set this to something unique like your
        /// build number and that will ensure the cache always invalidates with each new release. Do not use the same
        /// value for multiple releases, or clients will never update their caches. You can set this to 'nocache' to totally
        /// disable the cache. Default: null (use assembly hash)
        /// </summary>
        public string CacheToken { get; set; } = null; //(use assembly hash)

        /// <summary>
        /// Set this to the string to use for the cache-control header on anything that is cached.
        /// Default: 'private, max-age=2592000, stale-while-revalidate=86400, immutable'
        /// </summary>
        public string CacheControlHeaderString { get; set; } = "private, max-age=2592000, stale-while-revalidate=86400, immutable";

        /// <summary>
        /// Set this to true to cache static assets like javascript and css files.
        /// </summary>
        public bool CacheStaticAssets { get; set; } = true;

        /// <summary>
        /// Set this to true to enable response compression from inside this application directly.
        /// If this is enabled it really only applies to static assets. The razor views are not
        /// compressed and neither are the api results due to issues with https and compression.
        /// Default: false (no compression)
        /// </summary>
        public bool EnableResponseCompression { get; set; } = false;

        /// <summary>
        /// Set this to true to enable the metadata for the id server. This can be disabled after
        /// initial setup. Default: true
        /// </summary>
        public bool EnableIdServerMetadata { get; set; } = true;
    }
}