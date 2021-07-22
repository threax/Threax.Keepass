import * as controller from 'htmlrapier/src/controller';
import * as client from 'Client/Libs/ServiceClient';
import * as userDirectoryClient from 'htmlrapier.roleclient/src/UserDirectoryClient';
import * as roleClient from 'htmlrapier.roleclient/src/RoleClient';

export class UserSearchClientEntryPointInjector extends userDirectoryClient.UserSearchEntryPointInjector {
    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [client.EntryPointInjector];
    }

    private instance: Promise<userDirectoryClient.EntryPointResult>;

    constructor(private injector: client.EntryPointInjector) {
        super();
    }

    public load(): Promise<userDirectoryClient.EntryPointResult> {
        if (!this.instance) {
            this.instance = this.injector.load();
        }

        return this.instance;
    }
}


export function addServices(builder: controller.InjectedControllerBuilder) {
    //Map the role entry point to the service entry point and add the user directory
    builder.Services.addShared(roleClient.IRoleEntryInjector, s => s.getRequiredService(client.EntryPointInjector));
    builder.Services.addShared(userDirectoryClient.UserSearchEntryPointInjector, UserSearchClientEntryPointInjector);
}