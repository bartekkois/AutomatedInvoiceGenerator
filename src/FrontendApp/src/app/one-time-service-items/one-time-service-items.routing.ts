import { RouterModule } from '@angular/router';

import { OneTimeServiceItemFormComponent } from './one-time-service-item-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const oneTimeServiceItemsRouting = RouterModule.forChild([
    { path: 'customer/:customerId/serviceItemsSet/:serviceItemsSetId/oneTimeServiceItem/:oneTimeServiceItemId', component: OneTimeServiceItemFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'customer/:customerId/oneTimeServiceItem/new', component: OneTimeServiceItemFormComponent, canDeactivate: [CanDeactivateGuard] }
]);
