import * as explorer from 'hr.halcyon-explorer.HalcyonBrowserController';
import * as controller from 'hr.controller';
import * as startup from 'clientlibs.startup';
import * as deepLink from 'hr.deeplink';

var builder = startup.createBuilder();
deepLink.addServices(builder.Services);
explorer.addServices(builder.Services);
explorer.createBrowser(builder, new explorer.BrowserOptions);