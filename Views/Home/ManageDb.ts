import * as standardCrudPage from 'hr.widgets.StandardCrudPage';
import * as startup from 'clientlibs.startup';
import * as deepLink from 'hr.deeplink';
import { ItemCrudInjector } from 'clientlibs.ItemCrudInjector';
import { CrudItemEditorControllerExtensions, CrudItemEditorController } from 'hr.widgets.CrudItemEditor';
import * as controller from 'hr.controller';
import * as client from 'clientlibs.ServiceClient';
import { MainLoadErrorLifecycle } from 'hr.widgets.MainLoadErrorLifecycle';

class ManageDbController {
    private input: controller.IForm<client.OpenDbInput>;
    private injector: client.EntryPointInjector;
    private lifecycle: MainLoadErrorLifecycle;
    private mainErrorToggle: controller.OnOffToggle;
    private mainErrorView: controller.IView<Error>;

    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [controller.BindingCollection, client.EntryPointInjector];
    }

    constructor(bindings: controller.BindingCollection, injector: client.EntryPointInjector) {
        bindings.setListener(this);
        this.lifecycle = new MainLoadErrorLifecycle(bindings.getToggle("main"), bindings.getToggle("load"), bindings.getToggle("error"), true);
        this.mainErrorToggle = bindings.getToggle("mainError");
        this.mainErrorView = bindings.getView<Error>("mainError");
        this.input = bindings.getForm("input");
        this.injector = injector;
        this.setup();
    }

    private async setup(): Promise<void> {
        try {
            var entry = await this.injector.load();
            var openDbDocs = await entry.getOpenDbDocs();
            this.input.setSchema(openDbDocs.requestSchema);
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