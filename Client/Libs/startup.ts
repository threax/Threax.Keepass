import * as controller from 'hr.controller';
import * as WindowFetch from 'hr.windowfetch';
import * as AccessTokens from 'hr.accesstokens';
import * as whitelist from 'hr.whitelist';
import * as fetcher from 'hr.fetcher';
import * as bootstrap from 'hr.bootstrap.all';
import * as client from 'clientlibs.ServiceClient';
import * as userSearch from 'clientlibs.UserSearchClientEntryPointInjector';
import * as loginPopup from 'hr.relogin.LoginPopup';
import * as deepLink from 'hr.deeplink';
import * as xsrf from 'hr.xsrftoken';
import * as pageConfig from 'hr.pageconfig';
import * as dbfetcher from 'clientlibs.DbFetcher';
import * as dbpopup from 'clientlibs.DbPopup';

export interface Config {
    client: {
        ServiceUrl: string,
        PageBasePath: string,
        DbStatusUrl: string
    };
    tokens: {
        AccessTokenPath?: string;
        XsrfCookie?: string;
        XsrfPaths?: string[];
    };
}

export interface Options {
    EnableDbPopup?: boolean;
}

var builder: controller.InjectedControllerBuilder = null;

export function createBuilder(options?: Options) {
    if (options === undefined) {
        options = {
            EnableDbPopup: true
        };
    }

    if (builder === null) {
        builder = new controller.InjectedControllerBuilder();

        //Keep this bootstrap activator line, it will ensure that bootstrap is loaded and configured before continuing.
        bootstrap.activate();

        //Set up the access token fetcher
        var config = pageConfig.read<Config>();
        builder.Services.tryAddShared(fetcher.Fetcher, s => createFetcher(config, options));
        builder.Services.tryAddShared(client.EntryPointInjector, s => new client.EntryPointInjector(config.client.ServiceUrl, s.getRequiredService(fetcher.Fetcher)));
        
        userSearch.addServices(builder);

        //Setup Deep Links
        deepLink.setPageUrl(builder.Services, config.client.PageBasePath);

        //Setup relogin
        loginPopup.addServices(builder.Services);
        builder.create("hr-relogin", loginPopup.LoginPopup);

        //Set Db Popup
        if (options.EnableDbPopup) {
            dbpopup.addServices(builder.Services);
            builder.create("hr-opendb", dbpopup.DbPopup);
            builder.create("hr-opendbbutton", dbpopup.DbPopupButton)
        }
    }
    return builder;
}

function createFetcher(config: Config, options: Options): fetcher.Fetcher {
    var fetcher = new WindowFetch.WindowFetch();

    if (config.tokens !== undefined) {
        fetcher = new xsrf.XsrfTokenFetcher(
            new xsrf.CookieTokenAccessor(config.tokens.XsrfCookie),
            new whitelist.Whitelist(config.tokens.XsrfPaths),
            fetcher);
    }

    if (config.tokens.AccessTokenPath !== undefined) {
        fetcher = new AccessTokens.AccessTokenFetcher(
            config.tokens.AccessTokenPath,
            new whitelist.Whitelist([config.client.ServiceUrl]),
            fetcher);
    }

    if (options.EnableDbPopup && config.client.DbStatusUrl !== undefined) {
        fetcher = new dbfetcher.DbFetcher(
            config.client.DbStatusUrl,
            new whitelist.Whitelist([config.client.ServiceUrl]),
            fetcher);
    }

    return fetcher;
}