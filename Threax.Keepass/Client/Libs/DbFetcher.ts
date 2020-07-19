import { Fetcher } from 'hr.fetcher';
import * as events from 'hr.eventdispatcher';
import * as ep from 'hr.externalpromise';
import { IWhitelist } from 'hr.whitelist';
import * as client from 'clientlibs.ServiceClient';

class TokenManager {
    private currentToken: client.DbStatus;
    private needLoginEvent: events.PromiseEventDispatcher<boolean, TokenManager> = new events.PromiseEventDispatcher<boolean, TokenManager>();
    private queuePromise: ep.ExternalPromise<client.DbStatus> = null;
    private entryPointInjector: client.EntryPointInjector;

    constructor(url: string, private fetcher: Fetcher) {
        this.entryPointInjector = new client.EntryPointInjector(url, fetcher);
    }

    public getToken(): Promise<client.DbStatus> {
        //First check if we should queue the request
        if (this.queuePromise !== null) {
            return this.queuePromise.Promise;
        }

        this.queuePromise = new ep.ExternalPromise<client.DbStatus>();
        this.doRefreshToken(); //Do NOT await this, we want execution to continue.
        return this.queuePromise.Promise; //Here we return the queued promise that will resolve when doRefreshToken is done.
    }

    private async doRefreshToken(): Promise<void> {
        try {
            await this.readServerToken();

            if (this.currentToken.dbClosed) {
                if (await this.fireNeedDbPassword()) {
                    //After login read the server token again and resolve the queue
                    await this.readServerToken();
                    if (this.currentToken.dbClosed) {
                        this.rejectQueue("Db still closed.");
                    }
                    else {
                        this.resolveQueue();
                    }
                }
                else {
                    this.rejectQueue("Db Password Entry Error.");
                }
            }
            else {
                this.resolveQueue();
            }
        }
        catch (err) {
            //This error happens only if we can't get the access token
            //If we did not yet have a token, allow the request to finish, the user is not logged in
            //Otherwise try to get the login
            if (this.currentToken === undefined) {
                this.resolveQueue();
            }
            else { //Got false from fireNeedLogin, which means no login was performed, return an error
                this.rejectQueue("Could not refresh access token or log back in.");
            }
        }
    }

    private async readServerToken(): Promise<void> {
        const entry = await this.entryPointInjector.load();
        if (!entry.canGetDbStatus()) {
            throw new Error("Cannot get db status.");
        }

        const result = await entry.getDbStatus();

        this.currentToken = result.data;
    }

    private clearToken(): void {
        this.currentToken = undefined;
    }

    /**
     * Get an event listener for the given status code. Since this fires as part of the
     * fetch request the events can return promises to delay sending the event again
     * until the promise resolves.
     * @param status The status code for the event.
     */
    public get onNeedDbPassword(): events.EventModifier<events.FuncEventListener<Promise<boolean>, TokenManager>> {
        return this.needLoginEvent.modifier;
    }

    private async fireNeedDbPassword(): Promise<boolean> {
        var retryResults = await this.needLoginEvent.fire(this);

        if (retryResults) {
            //Take first result that is actually defined
            for (var i = 0; i < retryResults.length; ++i) {
                if (retryResults[i]) {
                    return retryResults[i];
                }
            }
        }

        return false;
    }

    private resolveQueue() {
        var promise = this.queuePromise;
        this.queuePromise = null;
        promise.resolve(this.currentToken);
    }

    private rejectQueue(err: any) {
        var promise = this.queuePromise;
        this.queuePromise = null;
        promise.reject(this.currentToken);
    }
}

export class DbFetcher extends Fetcher {
    public static isInstance(t: any): t is DbFetcher {
        return (<DbFetcher>t).onNeedDbPassword !== undefined
            && (<DbFetcher>t).fetch !== undefined;
    }

    private next: Fetcher;
    private accessWhitelist: IWhitelist;
    private tokenManager: TokenManager;
    private needDbPasswordEvent: events.PromiseEventDispatcher<boolean, DbFetcher> = new events.PromiseEventDispatcher<boolean, DbFetcher>();
    private _alwaysRefreshToken: boolean = false;
    private _useToken: boolean = true;
    private _disableOnNoToken: boolean = true;

    constructor(tokenPath: string, accessWhitelist: IWhitelist, next: Fetcher) {
        super();
        this.tokenManager = new TokenManager(tokenPath, next);
        this.tokenManager.onNeedDbPassword.add((t) => this.fireNeedDbPassword());
        this.next = next;
        this.accessWhitelist = accessWhitelist;
    }

    public async fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
        if (this._useToken) {
            //Make sure the request is allowed to send an access token
            var whitelisted: boolean = this.accessWhitelist.isWhitelisted(url);

            //Sometimes we always refresh the token even if the item is not on the whitelist
            //This is configured by the user
            if (whitelisted || this._alwaysRefreshToken) {
                await this.tokenManager.getToken();
            }
        }

        return this.next.fetch(url, init);
    }

    /**
     * This event will fire if the token manager tried to get an access token and failed. You can try
     * to log the user back in at this point.
     */
    public get onNeedDbPassword(): events.EventModifier<events.FuncEventListener<Promise<boolean>, DbFetcher>> {
        return this.needDbPasswordEvent;
    }

    public get alwaysRefreshToken(): boolean {
        return this._alwaysRefreshToken;
    }

    public set alwaysRefreshToken(value: boolean) {
        this._alwaysRefreshToken = value;
    }

    public get useToken(): boolean {
        return this._useToken;
    }

    public set useToken(value: boolean) {
        this._useToken = value;
    }

    public get disableOnNoToken(): boolean {
        return this._disableOnNoToken;
    }

    public set disableOnNoToken(value: boolean) {
        this._disableOnNoToken = value;
    }

    private async fireNeedDbPassword(): Promise<boolean> {
        var retryResults = await this.needDbPasswordEvent.fire(this);

        if (retryResults) {
            for (var i = 0; i < retryResults.length; ++i) {
                if (retryResults[i]) {
                    return retryResults[i];
                }
            }
        }

        return false;
    }
}