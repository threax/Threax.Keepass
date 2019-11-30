import * as hal from 'hr.halcyon.EndpointClient';

export class RoleAssignmentsResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: RoleAssignments = undefined;
    public get data(): RoleAssignments {
        this.strongData = this.strongData || this.client.GetData<RoleAssignments>();
        return this.strongData;
    }

    public refresh(): Promise<RoleAssignmentsResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public setUser(data: RoleAssignments): Promise<RoleAssignmentsResult> {
        return this.client.LoadLinkWithData("SetUser", data)
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canSetUser(): boolean {
        return this.client.HasLink("SetUser");
    }

    public linkForSetUser(): hal.HalLink {
        return this.client.GetLink("SetUser");
    }

    public getSetUserDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("SetUser", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasSetUserDocs(): boolean {
        return this.client.HasLinkDoc("SetUser");
    }

    public update(data: RoleAssignments): Promise<RoleAssignmentsResult> {
        return this.client.LoadLinkWithData("Update", data)
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canUpdate(): boolean {
        return this.client.HasLink("Update");
    }

    public linkForUpdate(): hal.HalLink {
        return this.client.GetLink("Update");
    }

    public getUpdateDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Update", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasUpdateDocs(): boolean {
        return this.client.HasLinkDoc("Update");
    }

    public delete(): Promise<void> {
        return this.client.LoadLink("Delete").then(hal.makeVoid);
    }

    public canDelete(): boolean {
        return this.client.HasLink("Delete");
    }

    public linkForDelete(): hal.HalLink {
        return this.client.GetLink("Delete");
    }
}

export class DbStatusInjector {
    private url: string;
    private fetcher: hal.Fetcher;
    private instancePromise: Promise<DbStatusResult>;

    constructor(url: string, fetcher: hal.Fetcher) {
        this.url = url;
        this.fetcher = fetcher;
    }

    public load(): Promise<DbStatusResult> {
        if (!this.instancePromise) {
            this.instancePromise = DbStatusResult.Load(this.url, this.fetcher);
        }

        return this.instancePromise;
    }
}

export class DbStatusResult {
    private client: hal.HalEndpointClient;

    public static Load(url: string, fetcher: hal.Fetcher): Promise<DbStatusResult> {
        return hal.HalEndpointClient.Load({
            href: url,
            method: "GET"
        }, fetcher)
            .then(c => {
                 return new DbStatusResult(c);
             });
            }

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: DbStatus = undefined;
    public get data(): DbStatus {
        this.strongData = this.strongData || this.client.GetData<DbStatus>();
        return this.strongData;
    }

    public refresh(): Promise<EntryPointResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new EntryPointResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getUser(): Promise<RoleAssignmentsResult> {
        return this.client.LoadLink("GetUser")
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canGetUser(): boolean {
        return this.client.HasLink("GetUser");
    }

    public linkForGetUser(): hal.HalLink {
        return this.client.GetLink("GetUser");
    }

    public getGetUserDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetUser", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetUserDocs(): boolean {
        return this.client.HasLinkDoc("GetUser");
    }

    public listUsers(data: RolesQuery): Promise<UserCollectionResult> {
        return this.client.LoadLinkWithData("ListUsers", data)
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canListUsers(): boolean {
        return this.client.HasLink("ListUsers");
    }

    public linkForListUsers(): hal.HalLink {
        return this.client.GetLink("ListUsers");
    }

    public getListUsersDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("ListUsers", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListUsersDocs(): boolean {
        return this.client.HasLinkDoc("ListUsers");
    }

    public setUser(data: RoleAssignments): Promise<RoleAssignmentsResult> {
        return this.client.LoadLinkWithData("SetUser", data)
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canSetUser(): boolean {
        return this.client.HasLink("SetUser");
    }

    public linkForSetUser(): hal.HalLink {
        return this.client.GetLink("SetUser");
    }

    public getSetUserDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("SetUser", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasSetUserDocs(): boolean {
        return this.client.HasLinkDoc("SetUser");
    }

    public listAppUsers(data: UserSearchQuery): Promise<UserSearchCollectionResult> {
        return this.client.LoadLinkWithData("ListAppUsers", data)
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canListAppUsers(): boolean {
        return this.client.HasLink("ListAppUsers");
    }

    public linkForListAppUsers(): hal.HalLink {
        return this.client.GetLink("ListAppUsers");
    }

    public getListAppUsersDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("ListAppUsers", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListAppUsersDocs(): boolean {
        return this.client.HasLinkDoc("ListAppUsers");
    }

    public openDb(data: OpenDbInput): Promise<DbStatusResult> {
        return this.client.LoadLinkWithData("OpenDb", data)
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canOpenDb(): boolean {
        return this.client.HasLink("OpenDb");
    }

    public linkForOpenDb(): hal.HalLink {
        return this.client.GetLink("OpenDb");
    }

    public getOpenDbDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("OpenDb", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasOpenDbDocs(): boolean {
        return this.client.HasLinkDoc("OpenDb");
    }

    public closeDb(): Promise<DbStatusResult> {
        return this.client.LoadLink("CloseDb")
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canCloseDb(): boolean {
        return this.client.HasLink("CloseDb");
    }

    public linkForCloseDb(): hal.HalLink {
        return this.client.GetLink("CloseDb");
    }

    public getCloseDbDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("CloseDb", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasCloseDbDocs(): boolean {
        return this.client.HasLinkDoc("CloseDb");
    }
}

export class EntryResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: Entry = undefined;
    public get data(): Entry {
        this.strongData = this.strongData || this.client.GetData<Entry>();
        return this.strongData;
    }

    public refresh(): Promise<EntryResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public update(data: EntryInput): Promise<EntryResult> {
        return this.client.LoadLinkWithData("Update", data)
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canUpdate(): boolean {
        return this.client.HasLink("Update");
    }

    public linkForUpdate(): hal.HalLink {
        return this.client.GetLink("Update");
    }

    public getUpdateDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Update", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasUpdateDocs(): boolean {
        return this.client.HasLinkDoc("Update");
    }

    public delete(): Promise<void> {
        return this.client.LoadLink("Delete").then(hal.makeVoid);
    }

    public canDelete(): boolean {
        return this.client.HasLink("Delete");
    }

    public linkForDelete(): hal.HalLink {
        return this.client.GetLink("Delete");
    }
}

export class EntryCollectionResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: EntryCollection = undefined;
    public get data(): EntryCollection {
        this.strongData = this.strongData || this.client.GetData<EntryCollection>();
        return this.strongData;
    }

    private itemsStrong: EntryResult[];
    public get items(): EntryResult[] {
        if (this.itemsStrong === undefined) {
            var embeds = this.client.GetEmbed("values");
            var clients = embeds.GetAllClients();
            this.itemsStrong = [];
            for (var i = 0; i < clients.length; ++i) {
                this.itemsStrong.push(new EntryResult(clients[i]));
            }
        }
        return this.itemsStrong;
    }

    public refresh(): Promise<EntryCollectionResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new EntryCollectionResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getGetDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Get", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetDocs(): boolean {
        return this.client.HasLinkDoc("Get");
    }

    public getListDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("List", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListDocs(): boolean {
        return this.client.HasLinkDoc("List");
    }

    public getUpdateDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Update", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasUpdateDocs(): boolean {
        return this.client.HasLinkDoc("Update");
    }

    public add(data: EntryInput): Promise<EntryResult> {
        return this.client.LoadLinkWithData("Add", data)
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canAdd(): boolean {
        return this.client.HasLink("Add");
    }

    public linkForAdd(): hal.HalLink {
        return this.client.GetLink("Add");
    }

    public getAddDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Add", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasAddDocs(): boolean {
        return this.client.HasLinkDoc("Add");
    }

    public next(): Promise<EntryCollectionResult> {
        return this.client.LoadLink("next")
               .then(r => {
                    return new EntryCollectionResult(r);
                });

    }

    public canNext(): boolean {
        return this.client.HasLink("next");
    }

    public linkForNext(): hal.HalLink {
        return this.client.GetLink("next");
    }

    public getNextDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("next", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasNextDocs(): boolean {
        return this.client.HasLinkDoc("next");
    }

    public previous(): Promise<EntryCollectionResult> {
        return this.client.LoadLink("previous")
               .then(r => {
                    return new EntryCollectionResult(r);
                });

    }

    public canPrevious(): boolean {
        return this.client.HasLink("previous");
    }

    public linkForPrevious(): hal.HalLink {
        return this.client.GetLink("previous");
    }

    public getPreviousDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("previous", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasPreviousDocs(): boolean {
        return this.client.HasLinkDoc("previous");
    }

    public first(): Promise<EntryCollectionResult> {
        return this.client.LoadLink("first")
               .then(r => {
                    return new EntryCollectionResult(r);
                });

    }

    public canFirst(): boolean {
        return this.client.HasLink("first");
    }

    public linkForFirst(): hal.HalLink {
        return this.client.GetLink("first");
    }

    public getFirstDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("first", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasFirstDocs(): boolean {
        return this.client.HasLinkDoc("first");
    }

    public last(): Promise<EntryCollectionResult> {
        return this.client.LoadLink("last")
               .then(r => {
                    return new EntryCollectionResult(r);
                });

    }

    public canLast(): boolean {
        return this.client.HasLink("last");
    }

    public linkForLast(): hal.HalLink {
        return this.client.GetLink("last");
    }

    public getLastDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("last", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasLastDocs(): boolean {
        return this.client.HasLinkDoc("last");
    }
}

export class EntryPointInjector {
    private url: string;
    private fetcher: hal.Fetcher;
    private instancePromise: Promise<EntryPointResult>;

    constructor(url: string, fetcher: hal.Fetcher) {
        this.url = url;
        this.fetcher = fetcher;
    }

    public load(): Promise<EntryPointResult> {
        if (!this.instancePromise) {
            this.instancePromise = EntryPointResult.Load(this.url, this.fetcher);
        }

        return this.instancePromise;
    }
}

export class EntryPointResult {
    private client: hal.HalEndpointClient;

    public static Load(url: string, fetcher: hal.Fetcher): Promise<EntryPointResult> {
        return hal.HalEndpointClient.Load({
            href: url,
            method: "GET"
        }, fetcher)
            .then(c => {
                 return new EntryPointResult(c);
             });
            }

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: EntryPoint = undefined;
    public get data(): EntryPoint {
        this.strongData = this.strongData || this.client.GetData<EntryPoint>();
        return this.strongData;
    }

    public refresh(): Promise<EntryPointResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new EntryPointResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getUser(): Promise<RoleAssignmentsResult> {
        return this.client.LoadLink("GetUser")
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canGetUser(): boolean {
        return this.client.HasLink("GetUser");
    }

    public linkForGetUser(): hal.HalLink {
        return this.client.GetLink("GetUser");
    }

    public getGetUserDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetUser", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetUserDocs(): boolean {
        return this.client.HasLinkDoc("GetUser");
    }

    public listUsers(data: RolesQuery): Promise<UserCollectionResult> {
        return this.client.LoadLinkWithData("ListUsers", data)
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canListUsers(): boolean {
        return this.client.HasLink("ListUsers");
    }

    public linkForListUsers(): hal.HalLink {
        return this.client.GetLink("ListUsers");
    }

    public getListUsersDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("ListUsers", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListUsersDocs(): boolean {
        return this.client.HasLinkDoc("ListUsers");
    }

    public setUser(data: RoleAssignments): Promise<RoleAssignmentsResult> {
        return this.client.LoadLinkWithData("SetUser", data)
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canSetUser(): boolean {
        return this.client.HasLink("SetUser");
    }

    public linkForSetUser(): hal.HalLink {
        return this.client.GetLink("SetUser");
    }

    public getSetUserDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("SetUser", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasSetUserDocs(): boolean {
        return this.client.HasLinkDoc("SetUser");
    }

    public listAppUsers(data: UserSearchQuery): Promise<UserSearchCollectionResult> {
        return this.client.LoadLinkWithData("ListAppUsers", data)
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canListAppUsers(): boolean {
        return this.client.HasLink("ListAppUsers");
    }

    public linkForListAppUsers(): hal.HalLink {
        return this.client.GetLink("ListAppUsers");
    }

    public getListAppUsersDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("ListAppUsers", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListAppUsersDocs(): boolean {
        return this.client.HasLinkDoc("ListAppUsers");
    }

    public getDbStatus(): Promise<DbStatusResult> {
        return this.client.LoadLink("GetDbStatus")
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canGetDbStatus(): boolean {
        return this.client.HasLink("GetDbStatus");
    }

    public linkForGetDbStatus(): hal.HalLink {
        return this.client.GetLink("GetDbStatus");
    }

    public getGetDbStatusDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetDbStatus", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetDbStatusDocs(): boolean {
        return this.client.HasLinkDoc("GetDbStatus");
    }

    public openDb(data: OpenDbInput): Promise<DbStatusResult> {
        return this.client.LoadLinkWithData("OpenDb", data)
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canOpenDb(): boolean {
        return this.client.HasLink("OpenDb");
    }

    public linkForOpenDb(): hal.HalLink {
        return this.client.GetLink("OpenDb");
    }

    public getOpenDbDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("OpenDb", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasOpenDbDocs(): boolean {
        return this.client.HasLinkDoc("OpenDb");
    }

    public closeDb(): Promise<DbStatusResult> {
        return this.client.LoadLink("CloseDb")
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canCloseDb(): boolean {
        return this.client.HasLink("CloseDb");
    }

    public linkForCloseDb(): hal.HalLink {
        return this.client.GetLink("CloseDb");
    }

    public getCloseDbDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("CloseDb", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasCloseDbDocs(): boolean {
        return this.client.HasLinkDoc("CloseDb");
    }

    public listEntries(data: EntryQuery): Promise<EntryCollectionResult> {
        return this.client.LoadLinkWithData("ListEntries", data)
               .then(r => {
                    return new EntryCollectionResult(r);
                });

    }

    public canListEntries(): boolean {
        return this.client.HasLink("ListEntries");
    }

    public linkForListEntries(): hal.HalLink {
        return this.client.GetLink("ListEntries");
    }

    public getListEntriesDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("ListEntries", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListEntriesDocs(): boolean {
        return this.client.HasLinkDoc("ListEntries");
    }

    public addEntry(data: EntryInput): Promise<EntryResult> {
        return this.client.LoadLinkWithData("AddEntry", data)
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canAddEntry(): boolean {
        return this.client.HasLink("AddEntry");
    }

    public linkForAddEntry(): hal.HalLink {
        return this.client.GetLink("AddEntry");
    }

    public getAddEntryDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("AddEntry", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasAddEntryDocs(): boolean {
        return this.client.HasLinkDoc("AddEntry");
    }

    public listItems(data: ItemQuery): Promise<ItemCollectionResult> {
        return this.client.LoadLinkWithData("ListItems", data)
               .then(r => {
                    return new ItemCollectionResult(r);
                });

    }

    public canListItems(): boolean {
        return this.client.HasLink("ListItems");
    }

    public linkForListItems(): hal.HalLink {
        return this.client.GetLink("ListItems");
    }

    public getListItemsDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("ListItems", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListItemsDocs(): boolean {
        return this.client.HasLinkDoc("ListItems");
    }
}

export class ItemResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: Item = undefined;
    public get data(): Item {
        this.strongData = this.strongData || this.client.GetData<Item>();
        return this.strongData;
    }

    public refresh(): Promise<ItemResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new ItemResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getEntry(): Promise<EntryResult> {
        return this.client.LoadLink("GetEntry")
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canGetEntry(): boolean {
        return this.client.HasLink("GetEntry");
    }

    public linkForGetEntry(): hal.HalLink {
        return this.client.GetLink("GetEntry");
    }

    public getGetEntryDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetEntry", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetEntryDocs(): boolean {
        return this.client.HasLinkDoc("GetEntry");
    }

    public update(data: EntryInput): Promise<EntryResult> {
        return this.client.LoadLinkWithData("Update", data)
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canUpdate(): boolean {
        return this.client.HasLink("Update");
    }

    public linkForUpdate(): hal.HalLink {
        return this.client.GetLink("Update");
    }

    public getUpdateDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Update", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasUpdateDocs(): boolean {
        return this.client.HasLinkDoc("Update");
    }

    public delete(): Promise<void> {
        return this.client.LoadLink("Delete").then(hal.makeVoid);
    }

    public canDelete(): boolean {
        return this.client.HasLink("Delete");
    }

    public linkForDelete(): hal.HalLink {
        return this.client.GetLink("Delete");
    }

    public getPassword(): Promise<PasswordInfoResult> {
        return this.client.LoadLink("GetPassword")
               .then(r => {
                    return new PasswordInfoResult(r);
                });

    }

    public canGetPassword(): boolean {
        return this.client.HasLink("GetPassword");
    }

    public linkForGetPassword(): hal.HalLink {
        return this.client.GetLink("GetPassword");
    }

    public getGetPasswordDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetPassword", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetPasswordDocs(): boolean {
        return this.client.HasLinkDoc("GetPassword");
    }
}

export class ItemCollectionResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: ItemCollection = undefined;
    public get data(): ItemCollection {
        this.strongData = this.strongData || this.client.GetData<ItemCollection>();
        return this.strongData;
    }

    private itemsStrong: ItemResult[];
    public get items(): ItemResult[] {
        if (this.itemsStrong === undefined) {
            var embeds = this.client.GetEmbed("values");
            var clients = embeds.GetAllClients();
            this.itemsStrong = [];
            for (var i = 0; i < clients.length; ++i) {
                this.itemsStrong.push(new ItemResult(clients[i]));
            }
        }
        return this.itemsStrong;
    }

    public refresh(): Promise<ItemCollectionResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new ItemCollectionResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getGetDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Get", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetDocs(): boolean {
        return this.client.HasLinkDoc("Get");
    }

    public getListDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("List", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListDocs(): boolean {
        return this.client.HasLinkDoc("List");
    }

    public getUpdateDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Update", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasUpdateDocs(): boolean {
        return this.client.HasLinkDoc("Update");
    }

    public add(data: EntryInput): Promise<EntryResult> {
        return this.client.LoadLinkWithData("Add", data)
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canAdd(): boolean {
        return this.client.HasLink("Add");
    }

    public linkForAdd(): hal.HalLink {
        return this.client.GetLink("Add");
    }

    public getAddDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Add", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasAddDocs(): boolean {
        return this.client.HasLinkDoc("Add");
    }

    public addChild(data: EntryInput): Promise<EntryResult> {
        return this.client.LoadLinkWithData("AddChild", data)
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canAddChild(): boolean {
        return this.client.HasLink("AddChild");
    }

    public linkForAddChild(): hal.HalLink {
        return this.client.GetLink("AddChild");
    }

    public getAddChildDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("AddChild", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasAddChildDocs(): boolean {
        return this.client.HasLinkDoc("AddChild");
    }

    public next(): Promise<ItemCollectionResult> {
        return this.client.LoadLink("next")
               .then(r => {
                    return new ItemCollectionResult(r);
                });

    }

    public canNext(): boolean {
        return this.client.HasLink("next");
    }

    public linkForNext(): hal.HalLink {
        return this.client.GetLink("next");
    }

    public getNextDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("next", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasNextDocs(): boolean {
        return this.client.HasLinkDoc("next");
    }

    public previous(): Promise<ItemCollectionResult> {
        return this.client.LoadLink("previous")
               .then(r => {
                    return new ItemCollectionResult(r);
                });

    }

    public canPrevious(): boolean {
        return this.client.HasLink("previous");
    }

    public linkForPrevious(): hal.HalLink {
        return this.client.GetLink("previous");
    }

    public getPreviousDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("previous", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasPreviousDocs(): boolean {
        return this.client.HasLinkDoc("previous");
    }

    public first(): Promise<ItemCollectionResult> {
        return this.client.LoadLink("first")
               .then(r => {
                    return new ItemCollectionResult(r);
                });

    }

    public canFirst(): boolean {
        return this.client.HasLink("first");
    }

    public linkForFirst(): hal.HalLink {
        return this.client.GetLink("first");
    }

    public getFirstDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("first", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasFirstDocs(): boolean {
        return this.client.HasLinkDoc("first");
    }

    public last(): Promise<ItemCollectionResult> {
        return this.client.LoadLink("last")
               .then(r => {
                    return new ItemCollectionResult(r);
                });

    }

    public canLast(): boolean {
        return this.client.HasLink("last");
    }

    public linkForLast(): hal.HalLink {
        return this.client.GetLink("last");
    }

    public getLastDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("last", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasLastDocs(): boolean {
        return this.client.HasLinkDoc("last");
    }

    public openDb(data: OpenDbInput): Promise<DbStatusResult> {
        return this.client.LoadLinkWithData("OpenDb", data)
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canOpenDb(): boolean {
        return this.client.HasLink("OpenDb");
    }

    public linkForOpenDb(): hal.HalLink {
        return this.client.GetLink("OpenDb");
    }

    public getOpenDbDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("OpenDb", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasOpenDbDocs(): boolean {
        return this.client.HasLinkDoc("OpenDb");
    }

    public closeDb(): Promise<DbStatusResult> {
        return this.client.LoadLink("CloseDb")
               .then(r => {
                    return new DbStatusResult(r);
                });

    }

    public canCloseDb(): boolean {
        return this.client.HasLink("CloseDb");
    }

    public linkForCloseDb(): hal.HalLink {
        return this.client.GetLink("CloseDb");
    }

    public getCloseDbDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("CloseDb", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasCloseDbDocs(): boolean {
        return this.client.HasLinkDoc("CloseDb");
    }
}

export class PasswordInfoResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: PasswordInfo = undefined;
    public get data(): PasswordInfo {
        this.strongData = this.strongData || this.client.GetData<PasswordInfo>();
        return this.strongData;
    }

    public refresh(): Promise<PasswordInfoResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new PasswordInfoResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getItem(): Promise<ItemResult> {
        return this.client.LoadLink("GetItem")
               .then(r => {
                    return new ItemResult(r);
                });

    }

    public canGetItem(): boolean {
        return this.client.HasLink("GetItem");
    }

    public linkForGetItem(): hal.HalLink {
        return this.client.GetLink("GetItem");
    }

    public getGetItemDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetItem", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetItemDocs(): boolean {
        return this.client.HasLinkDoc("GetItem");
    }

    public getEntry(): Promise<EntryResult> {
        return this.client.LoadLink("GetEntry")
               .then(r => {
                    return new EntryResult(r);
                });

    }

    public canGetEntry(): boolean {
        return this.client.HasLink("GetEntry");
    }

    public linkForGetEntry(): hal.HalLink {
        return this.client.GetLink("GetEntry");
    }

    public getGetEntryDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("GetEntry", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetEntryDocs(): boolean {
        return this.client.HasLinkDoc("GetEntry");
    }
}

export class UserCollectionResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: UserCollection = undefined;
    public get data(): UserCollection {
        this.strongData = this.strongData || this.client.GetData<UserCollection>();
        return this.strongData;
    }

    private itemsStrong: RoleAssignmentsResult[];
    public get items(): RoleAssignmentsResult[] {
        if (this.itemsStrong === undefined) {
            var embeds = this.client.GetEmbed("values");
            var clients = embeds.GetAllClients();
            this.itemsStrong = [];
            for (var i = 0; i < clients.length; ++i) {
                this.itemsStrong.push(new RoleAssignmentsResult(clients[i]));
            }
        }
        return this.itemsStrong;
    }

    public refresh(): Promise<UserCollectionResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getGetDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Get", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetDocs(): boolean {
        return this.client.HasLinkDoc("Get");
    }

    public getListDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("List", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListDocs(): boolean {
        return this.client.HasLinkDoc("List");
    }

    public getUpdateDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Update", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasUpdateDocs(): boolean {
        return this.client.HasLinkDoc("Update");
    }

    public add(data: RoleAssignments): Promise<RoleAssignmentsResult> {
        return this.client.LoadLinkWithData("Add", data)
               .then(r => {
                    return new RoleAssignmentsResult(r);
                });

    }

    public canAdd(): boolean {
        return this.client.HasLink("Add");
    }

    public linkForAdd(): hal.HalLink {
        return this.client.GetLink("Add");
    }

    public getAddDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Add", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasAddDocs(): boolean {
        return this.client.HasLinkDoc("Add");
    }

    public next(): Promise<UserCollectionResult> {
        return this.client.LoadLink("next")
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canNext(): boolean {
        return this.client.HasLink("next");
    }

    public linkForNext(): hal.HalLink {
        return this.client.GetLink("next");
    }

    public getNextDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("next", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasNextDocs(): boolean {
        return this.client.HasLinkDoc("next");
    }

    public previous(): Promise<UserCollectionResult> {
        return this.client.LoadLink("previous")
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canPrevious(): boolean {
        return this.client.HasLink("previous");
    }

    public linkForPrevious(): hal.HalLink {
        return this.client.GetLink("previous");
    }

    public getPreviousDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("previous", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasPreviousDocs(): boolean {
        return this.client.HasLinkDoc("previous");
    }

    public first(): Promise<UserCollectionResult> {
        return this.client.LoadLink("first")
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canFirst(): boolean {
        return this.client.HasLink("first");
    }

    public linkForFirst(): hal.HalLink {
        return this.client.GetLink("first");
    }

    public getFirstDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("first", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasFirstDocs(): boolean {
        return this.client.HasLinkDoc("first");
    }

    public last(): Promise<UserCollectionResult> {
        return this.client.LoadLink("last")
               .then(r => {
                    return new UserCollectionResult(r);
                });

    }

    public canLast(): boolean {
        return this.client.HasLink("last");
    }

    public linkForLast(): hal.HalLink {
        return this.client.GetLink("last");
    }

    public getLastDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("last", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasLastDocs(): boolean {
        return this.client.HasLinkDoc("last");
    }
}

export class UserSearchResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: UserSearch = undefined;
    public get data(): UserSearch {
        this.strongData = this.strongData || this.client.GetData<UserSearch>();
        return this.strongData;
    }

    public refresh(): Promise<UserSearchResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new UserSearchResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }
}

export class UserSearchCollectionResult {
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {
        this.client = client;
    }

    private strongData: UserSearchCollection = undefined;
    public get data(): UserSearchCollection {
        this.strongData = this.strongData || this.client.GetData<UserSearchCollection>();
        return this.strongData;
    }

    private itemsStrong: UserSearchResult[];
    public get items(): UserSearchResult[] {
        if (this.itemsStrong === undefined) {
            var embeds = this.client.GetEmbed("values");
            var clients = embeds.GetAllClients();
            this.itemsStrong = [];
            for (var i = 0; i < clients.length; ++i) {
                this.itemsStrong.push(new UserSearchResult(clients[i]));
            }
        }
        return this.itemsStrong;
    }

    public refresh(): Promise<UserSearchCollectionResult> {
        return this.client.LoadLink("self")
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canRefresh(): boolean {
        return this.client.HasLink("self");
    }

    public linkForRefresh(): hal.HalLink {
        return this.client.GetLink("self");
    }

    public getRefreshDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("self", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasRefreshDocs(): boolean {
        return this.client.HasLinkDoc("self");
    }

    public getGetDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("Get", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasGetDocs(): boolean {
        return this.client.HasLinkDoc("Get");
    }

    public getListDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("List", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasListDocs(): boolean {
        return this.client.HasLinkDoc("List");
    }

    public next(): Promise<UserSearchCollectionResult> {
        return this.client.LoadLink("next")
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canNext(): boolean {
        return this.client.HasLink("next");
    }

    public linkForNext(): hal.HalLink {
        return this.client.GetLink("next");
    }

    public getNextDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("next", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasNextDocs(): boolean {
        return this.client.HasLinkDoc("next");
    }

    public previous(): Promise<UserSearchCollectionResult> {
        return this.client.LoadLink("previous")
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canPrevious(): boolean {
        return this.client.HasLink("previous");
    }

    public linkForPrevious(): hal.HalLink {
        return this.client.GetLink("previous");
    }

    public getPreviousDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("previous", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasPreviousDocs(): boolean {
        return this.client.HasLinkDoc("previous");
    }

    public first(): Promise<UserSearchCollectionResult> {
        return this.client.LoadLink("first")
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canFirst(): boolean {
        return this.client.HasLink("first");
    }

    public linkForFirst(): hal.HalLink {
        return this.client.GetLink("first");
    }

    public getFirstDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("first", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasFirstDocs(): boolean {
        return this.client.HasLinkDoc("first");
    }

    public last(): Promise<UserSearchCollectionResult> {
        return this.client.LoadLink("last")
               .then(r => {
                    return new UserSearchCollectionResult(r);
                });

    }

    public canLast(): boolean {
        return this.client.HasLink("last");
    }

    public linkForLast(): hal.HalLink {
        return this.client.GetLink("last");
    }

    public getLastDocs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {
        return this.client.LoadLinkDoc("last", query)
            .then(r => {
                return r.GetData<hal.HalEndpointDoc>();
            });
    }

    public hasLastDocs(): boolean {
        return this.client.HasLinkDoc("last");
    }
}
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v9.10.49.0 (Newtonsoft.Json v12.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------





export interface RoleAssignments {
    userId?: string;
    name?: string;
    editRoles?: boolean;
    superAdmin?: boolean;
}

export interface DbStatus {
    dbClosed?: boolean;
}

export interface EntryPoint {
}

export interface RolesQuery {
    userId?: string[];
    name?: string;
    editRoles?: boolean;
    superAdmin?: boolean;
    offset?: number;
    limit?: number;
}

export interface UserCollection {
    name?: string;
    userId?: string[];
    total?: number;
    editRoles?: boolean;
    superAdmin?: boolean;
    offset?: number;
    limit?: number;
}

export interface UserSearchQuery {
    userId?: string;
    userName?: string;
    offset?: number;
    limit?: number;
}

export interface UserSearchCollection {
    userName?: string;
    userId?: string;
    total?: number;
    offset?: number;
    limit?: number;
}

export interface OpenDbInput {
    databasePassword?: string;
}

export interface Entry {
    itemId?: string;
    name?: string;
    userName?: string;
    url?: string;
    notes?: string;
    created?: string;
    modified?: string;
}

export interface EntryInput {
    name?: string;
    userName?: string;
    url?: string;
    notes?: string;
}

export interface EntryCollection {
    offset?: number;
    /** Lookup a entry by id. */
    itemId?: string;
    total?: number;
    limit?: number;
}

export interface EntryQuery {
    /** Lookup a entry by id. */
    itemId?: string;
    offset?: number;
    limit?: number;
}

export interface ItemQuery {
    /** Lookup a item by id. */
    itemId?: string;
    /** Lookup the items contained in a given parent id. */
    parentItemId?: string;
    search?: string;
    offset?: number;
    limit?: number;
}

export interface ItemCollection {
    dbClosed?: boolean;
    /** Lookup the items contained in a given parent id. */
    parentItemId?: string;
    /** Lookup a item by id. */
    itemId?: string;
    total?: number;
    search?: string;
    offset?: number;
    limit?: number;
}

export interface Item {
    itemId?: string;
    name?: string;
    isGroup?: boolean;
    created?: string;
    modified?: string;
}

export interface PasswordInfo {
    itemId?: string;
    password?: string;
}

export interface UserSearch {
    userId?: string;
    userName?: string;
}

export interface HalEndpointDocQuery {
    includeRequest?: boolean;
    includeResponse?: boolean;
}
