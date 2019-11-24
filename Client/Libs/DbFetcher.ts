//import * as http from 'hr.http';
//import { Fetcher, RequestInfo, RequestInit, Response, Request } from 'hr.fetcher';
//import * as events from 'hr.eventdispatcher';
//import * as ep from 'hr.externalpromise';
//import { IWhitelist } from 'hr.whitelist';

//interface IServerTokenResult {
//    nbf: string;
//    exp: string;
//}

//class TokenManager {
//    private currentToken: string;
//    private startTime: number;
//    private currentSub: string;
//    private expirationTick: number;
//    private needLoginEvent: events.PromiseEventDispatcher<boolean, TokenManager> = new events.PromiseEventDispatcher<boolean, TokenManager>();
//    private queuePromise: ep.ExternalPromise<string> = null;

//    constructor(private tokenPath: string, private fetcher: Fetcher) {

//    }

//    public getToken(): Promise<string> {
//        //First check if we should queue the request
//        if (this.queuePromise !== null) {
//            return this.queuePromise.Promise;
//        }

//        //Do we need to refresh?
//        if (this.startTime === undefined || Date.now() / 1000 - this.startTime > this.expirationTick) {
//            //If we need to refresh, create the queue and fire the refresh
//            this.queuePromise = new ep.ExternalPromise<string>();
//            this.doRefreshToken(); //Do NOT await this, we want execution to continue.
//            return this.queuePromise.Promise; //Here we return the queued promise that will resolve when doRefreshToken is done.
//        }

//        //Didn't need refresh, return current token.
//        return Promise.resolve(this.currentToken);
//    }

//    private async doRefreshToken(): Promise<void> {
//        try {
//            await this.readServerToken();
//            this.resolveQueue();
//        }
//        catch (err) {
//            //This error happens only if we can't get the access token
//            //If we did not yet have a token, allow the request to finish, the user is not logged in
//            //Otherwise try to get the login
//            if (this.currentToken === undefined) {
//                this.resolveQueue();
//            }
//            else if (await this.fireNeedLogin()) {
//                //After login read the server token again and resolve the queue
//                await this.readServerToken();
//                this.resolveQueue();
//            }
//            else { //Got false from fireNeedLogin, which means no login was performed, return an error
//                this.startTime = undefined;
//                this.rejectQueue("Could not refresh access token or log back in.");
//            }
//        }
//    }

//    private async readServerToken(): Promise<void> {
//        var data = await http.post<IServerTokenResult>(this.tokenPath, undefined, this.fetcher);
//        this.currentToken = data;

//        var tokenObj = data;

//        this.startTime = tokenObj.nbf;
//        this.expirationTick = (tokenObj.exp - this.startTime) / 2; //After half the token time has expired we will turn it in for another one.
//    }

//    private clearToken(): void {
//        this.currentToken = undefined;
//        this.startTime = undefined;
//        this.currentSub = undefined;
//    }

//    /**
//     * Get an event listener for the given status code. Since this fires as part of the
//     * fetch request the events can return promises to delay sending the event again
//     * until the promise resolves.
//     * @param status The status code for the event.
//     */
//    public get onNeedLogin(): events.EventModifier<events.FuncEventListener<Promise<boolean>, TokenManager>> {
//        return this.needLoginEvent.modifier;
//    }

//    public get headerName() {
//        return this._headerName;
//    }

//    private async fireNeedLogin(): Promise<boolean> {
//        var retryResults = await this.needLoginEvent.fire(this);

//        if (retryResults) {
//            //Take first result that is actually defined
//            for (var i = 0; i < retryResults.length; ++i) {
//                if (retryResults[i]) {
//                    return retryResults[i];
//                }
//            }
//        }

//        return false;
//    }

//    private resolveQueue() {
//        var promise = this.queuePromise;
//        this.queuePromise = null;
//        promise.resolve(this.currentToken);
//    }

//    private rejectQueue(err: any) {
//        var promise = this.queuePromise;
//        this.queuePromise = null;
//        promise.reject(this.currentToken);
//    }
//}

//export class DbFetcher extends Fetcher {
//    public static isInstance(t: any): t is DbFetcher {
//        return (<DbFetcher>t).onNeedLogin !== undefined
//            && (<DbFetcher>t).fetch !== undefined;
//    }

//    private next: Fetcher;
//    private accessWhitelist: IWhitelist;
//    private tokenManager: TokenManager;
//    private needLoginEvent: events.PromiseEventDispatcher<boolean, DbFetcher> = new events.PromiseEventDispatcher<boolean, DbFetcher>();
//    private _alwaysRefreshToken: boolean = false;
//    private _useToken: boolean = true;
//    private _disableOnNoToken: boolean = true;

//    constructor(tokenPath: string, accessWhitelist: IWhitelist, next: Fetcher) {
//        super();
//        this.tokenManager = new TokenManager(tokenPath, next);
//        this.tokenManager.onNeedLogin.add((t) => this.fireNeedLogin());
//        this.next = next;
//        this.accessWhitelist = accessWhitelist;
//    }

//    public async fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
//        if (this._useToken) {
//            //Make sure the request is allowed to send an access token
//            var whitelisted: boolean = this.accessWhitelist.isWhitelisted(url);

//            //Sometimes we always refresh the token even if the item is not on the whitelist
//            //This is configured by the user
//            if (whitelisted || this._alwaysRefreshToken) {
//                var token: string = await this.tokenManager.getToken();
//                if (token) {
//                    var headerName: string = this.tokenManager.headerName;
//                    if (whitelisted && headerName) {
//                        init.headers[headerName] = token;
//                    }
//                }
//                else {
//                    //No token, stop trying to use it
//                    this._useToken = !this._disableOnNoToken;
//                }
//            }
//        }

//        return this.next.fetch(url, init);
//    }

//    /**
//     * This event will fire if the token manager tried to get an access token and failed. You can try
//     * to log the user back in at this point.
//     */
//    public get onNeedLogin(): events.EventModifier<events.FuncEventListener<Promise<boolean>, DbFetcher>> {
//        return this.needLoginEvent;
//    }

//    public get alwaysRefreshToken(): boolean {
//        return this._alwaysRefreshToken;
//    }

//    public set alwaysRefreshToken(value: boolean) {
//        this._alwaysRefreshToken = value;
//    }

//    public get useToken(): boolean {
//        return this._useToken;
//    }

//    public set useToken(value: boolean) {
//        this._useToken = value;
//    }

//    public get disableOnNoToken(): boolean {
//        return this._disableOnNoToken;
//    }

//    public set disableOnNoToken(value: boolean) {
//        this._disableOnNoToken = value;
//    }

//    private async fireNeedLogin(): Promise<boolean> {
//        var retryResults = await this.needLoginEvent.fire(this);

//        if (retryResults) {
//            for (var i = 0; i < retryResults.length; ++i) {
//                if (retryResults[i]) {
//                    return retryResults[i];
//                }
//            }
//        }

//        return false;
//    }
//}