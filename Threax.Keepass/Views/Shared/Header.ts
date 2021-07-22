import * as controller from 'htmlrapier/src/controller';
import * as startup from 'Client/Libs/startup';
import * as client from 'Client/Libs/ServiceClient';
import * as tm from 'htmlrapier.accesstoken/src/manager';
import * as loginPopup from 'htmlrapier.relogin/src/LoginPopup';
import * as safepost from 'htmlrapier/src/safepostmessage';

const AccessTokenClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

class AppMenuController {
    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [controller.BindingCollection, client.EntryPointInjector, tm.TokenManager, safepost.PostMessageValidator];
    }

    private userInfoView: controller.IView<any>;
    private loggedInAreaToggle: controller.OnOffToggle;
    private adminToggle: controller.OnOffToggle;
    private users: controller.OnOffToggle;

    constructor(bindings: controller.BindingCollection,
        private entryPointInjector: client.EntryPointInjector,
        private tokenManger: tm.TokenManager,
        private messageValidator: safepost.PostMessageValidator) {

        this.userInfoView = bindings.getView("userInfo");
        this.loggedInAreaToggle = bindings.getToggle("loggedInArea");
        this.adminToggle = bindings.getToggle("admin");
        this.users = bindings.getToggle("users");

        //Listen for relogin events
        window.addEventListener("message", e => { this.handleMessage(e); });

        this.setup();
    }

    public async setup(): Promise<void> {
        const entry = await this.entryPointInjector.load();

        let accessToken = await this.tokenManger.getAccessToken();
        if (accessToken) {
            this.userInfoView.setData({
                userName: accessToken[AccessTokenClaim]
            });
        }
        this.loggedInAreaToggle.mode = accessToken !== null;

        let showAdmin = false;
        showAdmin = this.users.mode = entry.canListUsers() || showAdmin;
        this.adminToggle.mode = showAdmin;
    }

    private handleMessage(e: MessageEvent): void {
        if (this.messageValidator.isValid(e)) {
            const message: loginPopup.ILoginMessage = e.data;
            if (message.type === loginPopup.MessageType && message.success) {
                this.setup();
            }
        }
    }
}

const builder = startup.createBuilder();
builder.Services.addShared(AppMenuController, AppMenuController);
builder.create("appMenu", AppMenuController);