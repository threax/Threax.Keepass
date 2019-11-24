import * as controller from 'hr.controller';
import { Fetcher, RequestInfo, Response } from 'hr.fetcher';
import * as ep from 'hr.externalpromise';
import { DbFetcher } from 'clientlibs.DbFetcher';

export class DbPopupOptions {
    private _relogPage;

    constructor(relogPage: string) {
        this._relogPage = relogPage;
    }

    public get relogPage() {
        return this._relogPage;
    }
}

export class DbPopup {
    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [controller.BindingCollection, DbPopupOptions, Fetcher];
    }

    private dialog: controller.OnOffToggle;
    private currentPromise: ep.ExternalPromise<boolean>;
    private iframe: HTMLIFrameElement;
    private resizeEvent;

    constructor(bindings: controller.BindingCollection, private options: DbPopupOptions, fetcher: Fetcher) {
        this.dialog = bindings.getToggle("dialog");
        this.dialog.offEvent.add(t => {
            this.closed();
        });

        this.iframe = <HTMLIFrameElement>bindings.getHandle("iframe");

        var currentFetcher = fetcher;
        while (currentFetcher) {
            if (DbFetcher.isInstance(currentFetcher)) {
                currentFetcher.onNeedDbPassword.add(f => this.showLogin());
            }
            currentFetcher = (<any>currentFetcher).next;
        }

        window.addEventListener("message", e => {
            this.handleMessage(e);
        });

        this.resizeEvent = e => {
            this.setIframeHeight();
        };
    }

    public showLogin(): Promise<boolean> {
        this.dialog.on();
        this.currentPromise = new ep.ExternalPromise<boolean>();
        this.setIframeHeight();
        this.iframe.src = this.options.relogPage;
        window.addEventListener("resize", this.resizeEvent);
        return this.currentPromise.Promise;
    }

    private handleMessage(e: MessageEvent): void {
        var message: ILoginMessage = JSON.parse(e.data);
        if (message.type === MessageType && message.success) {
            this.dialog.off();
        }
    }

    private setIframeHeight(): void {
        this.iframe.style.height = (window.innerHeight - 240) + "px";
    }

    private async closed(): Promise<void> {
        if (this.currentPromise) {
            var promise = this.currentPromise;

            this.currentPromise = null;

            //Reset iframe contents
            this.iframe.contentWindow.document.open();
            this.iframe.contentWindow.document.close();

            window.removeEventListener("resize", this.resizeEvent);

            promise.resolve(true); //Try to determine true or false, true to try again, false to error
        }
    }
}

export const MessageType: string = "LoginPageMessage";

export interface ILoginMessage {
    type: string;
    success: boolean;
}

export function addServices(services: controller.ServiceCollection): void {
    services.tryAddShared(DbPopupOptions, (s) => new DbPopupOptions("/ManageDb"));
    services.tryAddShared(DbPopup, DbPopup);
}