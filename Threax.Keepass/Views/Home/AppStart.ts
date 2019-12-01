import * as client from 'clientlibs.ServiceClient';
import * as controller from 'hr.controller';
import * as startup from 'clientlibs.startup';
import { Uri } from 'hr.uri';
import * as loginPopup from 'hr.relogin.LoginPopup';

//Written this way for injection, but not really used.
class AppStart {
    public static get InjectorArgs(): controller.DiFunction<any>[] {
        return [client.EntryPointInjector, loginPopup.LoginPopup];
    }

    constructor(private entry: client.EntryPointInjector, private login: loginPopup.LoginPopup) {
        this.setup();
    }

    private async setup(): Promise<void> {
        //Check for login
        var entry = await this.entry.load();
        if (!entry.canListItems()) {
            var loginResult = await this.login.showLogin();
        }

        //Redirect to main page, remove AppStart from url
        var uri = new Uri(window.location.href);
        var path = "";
        var i = 0;
        var part: string;
        var pathEnd = uri.getPathPart(--i) || uri.getPathPart(--i);
        if (pathEnd !== null) {
            if (pathEnd.toUpperCase() === "APPSTART") {
                while ((part = uri.getPathPart(--i)) !== null) {
                    path = '/' + part + path;
                }
                if (!path) {
                    path = '/';
                }
                window.location.href = path;
            }
        }
    }
}

var builder = startup.createBuilder({
    EnableDbPopup: false
});
builder.Services.addTransient(AppStart, AppStart);
builder.createUnbound(AppStart);