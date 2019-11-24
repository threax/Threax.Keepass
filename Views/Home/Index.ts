import * as standardCrudPage from 'hr.widgets.StandardCrudPage';
import * as startup from 'clientlibs.startup';
import * as deepLink from 'hr.deeplink';
import { ItemCrudInjector } from 'clientlibs.ItemCrudInjector';
import { CrudItemEditorControllerExtensions, CrudItemEditorController } from 'hr.widgets.CrudItemEditor';
import * as controller from 'hr.controller';
import * as client from 'clientlibs.ServiceClient';
import { CrudTableRowControllerExtensions, CrudTableRowController } from 'hr.widgets.CrudTableRow';

class EditExtensions extends CrudItemEditorControllerExtensions {
    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [client.EntryPointInjector];
    }

    private copyToggle: controller.OnOffToggle;
    private copyElement: HTMLInputElement;
    private passwordCopyElement: HTMLInputElement;
    private data: client.Item;

    constructor(private injector: client.EntryPointInjector) {
        super();
    }

    constructed(editor: CrudItemEditorController, bindings: controller.BindingCollection): void {
        bindings.setListener(this);
        this.copyElement = <HTMLInputElement>bindings.getHandle("copyElement");
        this.passwordCopyElement = <HTMLInputElement>bindings.getHandle("passwordCopyElement");
        this.copyToggle = bindings.getToggle("copy");
    }

    setup(editor: CrudItemEditorController): Promise<void> {
        return Promise.resolve(undefined);
    }

    formDataSet(data: client.Item): void {
        this.data = data;
        this.copyElement.value = '';
        this.passwordCopyElement.value = '';
        this.copyToggle.off();
    }

    public async loadPassword(evt: Event): Promise<void> {
        evt.preventDefault();

        var entry = await this.injector.load();
        var item = await entry.listItems({
            itemId: this.data.itemId
        });
        var password = await item.items[0].getPassword();

        this.passwordCopyElement.value = password.data.password;
        this.copyToggle.on();
    }

    public async copyPassword(evt: Event): Promise<void> {
        evt.preventDefault();

        this.copyElement.value = this.passwordCopyElement.value;
        this.copyElement.select();
        this.copyElement.setSelectionRange(0, 99999); /*For mobile devices*/
        document.execCommand("copy");
        this.copyElement.value = "";
        this.passwordCopyElement.value = "";
        this.copyToggle.off();
    }
}

class CrudRow extends CrudTableRowControllerExtensions {
    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [];
    }

    public onEdit(row: CrudTableRowController, data: client.ItemResult): boolean {
        return !data.data.isGroup;
    }
}

var injector = ItemCrudInjector;

var builder = startup.createBuilder();
builder.Services.tryAddShared(CrudItemEditorControllerExtensions, EditExtensions);
builder.Services.tryAddTransient(CrudTableRowControllerExtensions, CrudRow);
deepLink.addServices(builder.Services);
standardCrudPage.addServices(builder, injector);
standardCrudPage.createControllers(builder, new standardCrudPage.Settings());