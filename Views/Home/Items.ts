import * as standardCrudPage from 'hr.widgets.StandardCrudPage';
import * as startup from 'clientlibs.startup';
import * as deepLink from 'hr.deeplink';
import { ItemCrudInjector } from 'clientlibs.ItemCrudInjector';
import { CrudItemEditorControllerExtensions, CrudItemEditorController } from 'hr.widgets.CrudItemEditor';
import * as controller from 'hr.controller';

class EditExtensions extends CrudItemEditorControllerExtensions {
    private copyElement: HTMLInputElement;

    constructor() {
        super();
    }

    constructed(editor: CrudItemEditorController, bindings: controller.BindingCollection): void {
        bindings.setListener(this);
        this.copyElement = <HTMLInputElement>bindings.getHandle("copyElement");
    }

    setup(editor: CrudItemEditorController): Promise<void> {
        return Promise.resolve(undefined);
    }

    formDataSet(data: any): void {

    }

    public async copyPassword(evt: Event): Promise<void> {
        evt.preventDefault();
        this.copyElement.value = "yo! dawg";
        this.copyElement.select();
        this.copyElement.setSelectionRange(0, 99999); /*For mobile devices*/
        document.execCommand("copy");
        this.copyElement.value = "";
    }
}

var injector = ItemCrudInjector;

var builder = startup.createBuilder();
builder.Services.tryAddSharedInstance(CrudItemEditorControllerExtensions, new EditExtensions());
deepLink.addServices(builder.Services);
standardCrudPage.addServices(builder, injector);
standardCrudPage.createControllers(builder, new standardCrudPage.Settings());