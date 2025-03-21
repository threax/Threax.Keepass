import * as standardCrudPage from 'htmlrapier.widgets/src/StandardCrudPage';
import * as startup from 'Client/Libs/startup';
import * as deepLink from 'htmlrapier/src/deeplink';
import { ItemCrudInjector } from 'Client/Libs/ItemCrudInjector';
import { CrudItemEditorControllerExtensions, CrudItemEditorController } from 'htmlrapier.widgets/src/CrudItemEditor';
import * as controller from 'htmlrapier/src/controller';
import * as client from 'Client/Libs/ServiceClient';
import { CrudTableRowControllerExtensions, CrudTableRowController } from 'htmlrapier.widgets/src/CrudTableRow';
import * as crudpage from 'htmlrapier.widgets/src/CrudPage';

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
        
        this.copyToggle.off();

        await navigator.clipboard.writeText(this.passwordCopyElement.value);
    }

    public async clearPassword(evt: Event): Promise<void> {
        evt.preventDefault();

        this.copyElement.value = "clear";
        this.copyElement.select();
        this.copyElement.setSelectionRange(0, 99999); /*For mobile devices*/
        document.execCommand("copy");
        this.copyElement.value = "";
        this.passwordCopyElement.value = "";
        this.copyToggle.off();
    }
}

class CrudRow extends CrudTableRowControllerExtensions {
    private data: client.ItemResult;

    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [crudpage.ICrudService, crudpage.CrudQueryManager];
    }

    constructor(private crud: crudpage.ICrudService, private queryManager: crudpage.CrudQueryManager) {
        super();
    }

    public rowConstructed(row: CrudTableRowController, bindings: controller.BindingCollection, data: any): void {
        bindings.setListener(this);
        this.data = data;
    }

    public async visit(evt: Event): Promise<void> {
        evt.preventDefault();

        var loadEditor = !this.data.data.isGroup;

        if (this.data.data.isGroup) {
            var query = this.queryManager.setupQuery();
            query.offset = 0;
            query.parentItemId = this.data.data.itemId;
            this.crud.getPage(query);
        }
        else {
            var entry = await this.data.getEntry();
            this.crud.edit(entry);
        }
    }
}

var injector = ItemCrudInjector;

var builder = startup.createBuilder();
builder.Services.tryAddShared(CrudItemEditorControllerExtensions, EditExtensions);
builder.Services.tryAddTransient(CrudTableRowControllerExtensions, CrudRow);
deepLink.addServices(builder.Services);
standardCrudPage.addServices(builder, injector);
standardCrudPage.createControllers(builder, new standardCrudPage.Settings());