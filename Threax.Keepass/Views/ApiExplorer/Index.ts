import * as explorer from 'htmlrapier.halcyon-explorer/src/HalcyonBrowserController';
import * as startup from 'Client/Libs/startup';
import * as deepLink from 'htmlrapier/src/deeplink';

var builder = startup.createBuilder();
deepLink.addServices(builder.Services);
explorer.addServices(builder.Services);
explorer.createBrowser(builder, new explorer.BrowserOptions);