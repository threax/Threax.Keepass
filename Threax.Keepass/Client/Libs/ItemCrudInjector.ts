import * as client from 'clientlibs.ServiceClient';
import * as hyperCrud from 'hr.widgets.HypermediaCrudService';
import * as di from 'hr.di';

export class ItemCrudInjector extends hyperCrud.AbstractHypermediaPageInjector {
    public static get InjectorArgs(): di.DiFunction<any>[] {
        return [client.EntryPointInjector];
    }

    constructor(private injector: client.EntryPointInjector) {
        super();
    }

    async list(query: any): Promise<hyperCrud.HypermediaCrudCollection> {
        var entry = await this.injector.load();
        return entry.listItems(query);
    }

    async canList(): Promise<boolean> {
        var entry = await this.injector.load();
        return entry.canListItems();
    }

    public getDeletePrompt(item: client.ItemResult): string {
        return "Are you sure you want to delete the item?";
    }

    public getItemId(item: client.ItemResult): string | null {
        return String(item.data.itemId);
    }

    public createIdQuery(id: string): client.ItemQuery | null {
        return {
            itemId: id
        };
    }
}