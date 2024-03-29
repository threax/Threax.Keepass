import * as startup from 'Client/Libs/startup';
import * as controller from 'htmlrapier/src/controller';
import * as client from 'Client/Libs/ServiceClient';
import { MainLoadErrorLifecycle } from 'htmlrapier.widgets/src/MainLoadErrorLifecycle';
import * as dbpopup from 'Client/Libs/DbPopup';
import * as safepost from 'htmlrapier/src/safepostmessage';

class ManageDbController {
    private input: controller.IForm<client.OpenDbInput>;
    private injector: client.EntryPointInjector;
    private lifecycle: MainLoadErrorLifecycle;
    private mainErrorToggle: controller.OnOffToggle;
    private mainErrorView: controller.IView<Error>;
    private lockUnlockToggle: controller.OnOffToggle;

    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [controller.BindingCollection, client.EntryPointInjector, safepost.MessagePoster];
    }

    constructor(bindings: controller.BindingCollection, injector: client.EntryPointInjector, private poster: safepost.MessagePoster) {
        bindings.setListener(this);
        this.lifecycle = new MainLoadErrorLifecycle(bindings.getToggle("main"), bindings.getToggle("load"), bindings.getToggle("error"), true);
        this.mainErrorToggle = bindings.getToggle("mainError");
        this.mainErrorView = bindings.getView<Error>("mainError");
        this.input = bindings.getForm("input");
        this.lockUnlockToggle = bindings.getToggle("lockUnlock");
        this.injector = injector;
        this.setup();
    }

    private async setup(): Promise<void> {
        try {
            var entry = await this.injector.load();
            var openDbDocs = await entry.getOpenDbDocs();
            this.input.setSchema(openDbDocs.requestSchema);
            var status = await entry.getDbStatus();
            this.lockUnlockToggle.mode = status.data.dbClosed;
            this.lifecycle.showMain();
        }
        catch (err) {
            this.lifecycle.showError(err);
        }
    }

    public async unlock(evt: Event): Promise<void> {
        evt.preventDefault();

        this.lifecycle.showLoad();
        this.mainErrorToggle.off();
        try {
            var data = this.input.getData();
            var entry = await this.injector.load();
            var result = await entry.openDb(data);
            this.lockUnlockToggle.mode = result.data.dbClosed;

            var message: dbpopup.ILoginMessage = {
                type: dbpopup.MessageType,
                unlocked: true
            };

            this.poster.postWindowMessage(parent, message);
        }
        catch (err) {
            console.error(err);
            this.input.setError(err);
            this.mainErrorView.setData(err);
            this.mainErrorToggle.on();
        }
        this.input.clear();
        this.lifecycle.showMain();
    }

    public async lock(evt: Event): Promise<void> {
        evt.preventDefault();

        this.lifecycle.showLoad();
        this.mainErrorToggle.off();
        try {
            var entry = await this.injector.load();
            var result = await entry.closeDb();
            this.lockUnlockToggle.mode = result.data.dbClosed;

            var message: dbpopup.ILoginMessage = {
                type: dbpopup.MessageType,
                unlocked: false
            };

            this.poster.postWindowMessage(parent, message);
        }
        catch (err) {
            console.error(err);
            this.input.setError(err);
            this.mainErrorView.setData(err);
            this.mainErrorToggle.on();
        }
        this.lifecycle.showMain();
    }
}

var builder = startup.createBuilder({
    EnableDbPopup: false
});
builder.Services.addTransient(ManageDbController, ManageDbController);

builder.create("manageDb", ManageDbController);