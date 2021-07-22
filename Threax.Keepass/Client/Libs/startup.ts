import * as hr from 'htmlrapier/src/main';
import * as datetime from 'htmlrapier.bootstrap.datetime/src/main';
import * as bootstrap from 'htmlrapier.bootstrap/src/main';
import * as bootstrap4form from 'htmlrapier.form.bootstrap4/src/main';
import * as controller from 'htmlrapier/src/controller';
import * as WindowFetch from 'htmlrapier/src/windowfetch';
import * as tokenmanager from 'htmlrapier.accesstoken/src/manager';
import * as tokenfetcher from 'htmlrapier.accesstoken/src/fetcher';
import * as whitelist from 'htmlrapier/src/whitelist';
import * as fetcher from 'htmlrapier/src/fetcher';
import * as client from 'Client/Libs/ServiceClient';
import * as userSearch from 'Client/Libs/UserSearchClientEntryPointInjector';
import * as loginPopup from 'htmlrapier.relogin/src/LoginPopup';
import * as deepLink from 'htmlrapier/src/deeplink';
import * as pageConfig from 'htmlrapier/src/pageconfig';
import * as dbfetcher from 'Client/Libs/DbFetcher';
import * as dbpopup from 'Client/Libs/DbPopup';
import * as safepost from 'htmlrapier/src/safepostmessage';
import * as di from 'htmlrapier/src/di';

export interface Config {
    client: {
        ServiceUrl: string;
        PageBasePath: string;
        BearerCookieName?: string;
        AccessTokenPath?: string;
    };
    entry: any;
}

export interface Options {
    EnableDbPopup?: boolean;
}

let builder: controller.InjectedControllerBuilder = null;

export function createBuilder(options?: Options) {
    if (options === undefined) {
        options = {
            EnableDbPopup: true
        };
    }

    if (builder === null) {
        //Activate htmlrapier
        hr.setup();
        datetime.setup();
        bootstrap.setup();
        bootstrap4form.setup();

        //Create builder
        builder = new controller.InjectedControllerBuilder();

        //Set up the access token fetcher
        //Set up the fetcher and entry point
        const config = pageConfig.read<Config>();
        const entryPointData = config.entry || null;
        builder.Services.tryAddShared(fetcher.Fetcher, s => createFetcher(s, config, options));
        builder.Services.tryAddShared(client.EntryPointInjector, s => new client.EntryPointInjector(config.client.ServiceUrl, s.getRequiredService(fetcher.Fetcher), entryPointData));
        builder.Services.tryAddShared(safepost.MessagePoster, s => new safepost.MessagePoster(window.location.href));
        builder.Services.tryAddShared(safepost.PostMessageValidator, s => new safepost.PostMessageValidator(window.location.href));
        tokenmanager.addServices(builder.Services, config.client.AccessTokenPath, config.client.BearerCookieName);
        
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

function createFetcher(scope: di.Scope, config: Config, options: Options): fetcher.Fetcher {
    let fetcher = new WindowFetch.WindowFetch();

    if (config.client.AccessTokenPath) {
        const accessFetcher = new tokenfetcher.AccessTokenFetcher(
            scope.getRequiredService(tokenmanager.TokenManager),
            new whitelist.Whitelist([config.client.ServiceUrl]),
            fetcher);
        fetcher = accessFetcher;
    }


    if (options.EnableDbPopup) {
        fetcher = new dbfetcher.DbFetcher(
            config.client.ServiceUrl,
            new whitelist.Whitelist([config.client.ServiceUrl]),
            fetcher);
    }

    return fetcher;
}