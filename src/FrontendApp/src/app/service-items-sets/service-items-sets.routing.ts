import { RouterModule } from '@angular/router';

import { ServiceItemsSetsComponent } from './service-items-sets.component';
import { ServiceItemsSetFormComponent } from './service-items-set-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const serviceItemsSetRouting = RouterModule.forChild([
    { path: 'customer/:customerId/serviceItemsSet/:serviceItemsSetId', component: ServiceItemsSetFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'customer/:customerId/serviceItemsSet/new', component: ServiceItemsSetFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'customer/:customerId/serviceItemsSet', component: ServiceItemsSetsComponent }
]);
