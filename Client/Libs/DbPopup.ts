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

    private input: controller.IForm<any>;
    private dialog: controller.OnOffToggle;
    private currentPromise: ep.ExternalPromise<boolean>;

    constructor(bindings: controller.BindingCollection, private options: DbPopupOptions, fetcher: Fetcher) {
        this.input = bindings.getForm("input");
        this.dialog = bindings.getToggle("dialog");
        this.dialog.offEvent.add(t => {
            this.closed();
        });

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
    }

    public showLogin(): Promise<boolean> {
        this.dialog.on();
        this.currentPromise = new ep.ExternalPromise<boolean>();
        return this.currentPromise.Promise;
    }

    private handleMessage(e: MessageEvent): void {
        var message: ILoginMessage = JSON.parse(e.data);
        if (message.type === MessageType && message.success) {
            this.dialog.off();
        }
    }

    private async closed(): Promise<void> {
        if (this.currentPromise) {
            var promise = this.currentPromise;

            this.currentPromise = null;

            //Reset contents
            this.input.clear();
            this.input.clearError();

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
    services.tryAddShared(DbPopupOptions, (s) => new DbPopupOptions("/Account/Relogin"));
    services.tryAddShared(DbPopup, DbPopup);
}