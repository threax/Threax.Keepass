import * as startup from 'Client/Libs/startup';
import * as loggedInPage from 'htmlrapier.relogin/src/LoggedInPage';

const builder = startup.createBuilder();
loggedInPage.addServices(builder.Services);
const loggedIn = builder.createUnbound(loggedInPage.LoggedInPage);

setTimeout(() => loggedIn.alertLoggedIn(), 250);