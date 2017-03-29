import { RouterModule } from '@angular/router';

import { GroupsComponent } from './groups.component';
import { GroupsManagerComponent } from './groups-manager/groups-manager.component';
import { GroupsManagerGroupFormComponent } from './groups-manager/groups-manager-group-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const groupsRouting = RouterModule.forChild([
    { path: 'groups-manager/:id', component: GroupsManagerGroupFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'groups-manager/new', component: GroupsManagerGroupFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'groups-manager', component: GroupsManagerComponent }
]);
