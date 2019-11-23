import * as startup from 'clientlibs.startup';
import * as controller from 'hr.controller';
import * as crudService from 'hr.roleclient.UserCrudService';
import * as deepLink from 'hr.deeplink';
import * as userSearch from 'hr.roleclient.UserSearchController';

var builder = startup.createBuilder();
deepLink.addServices(builder.Services);
crudService.addServices(builder);

crudService.createControllers(builder, new crudService.UserCrudSettings());