import * as startup from 'Client/Libs/startup';
import * as crudService from 'htmlrapier.roleclient/src/UserCrudService';
import * as deepLink from 'htmlrapier/src/deeplink';

var builder = startup.createBuilder({
    EnableDbPopup: false
});
deepLink.addServices(builder.Services);
crudService.addServices(builder);

crudService.createControllers(builder, new crudService.UserCrudSettings());