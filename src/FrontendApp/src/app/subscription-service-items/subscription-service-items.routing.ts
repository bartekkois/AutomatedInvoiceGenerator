import { RouterModule } from '@angular/router';

import { SubscriptionServiceItemFormComponent } from './subscription-service-item-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const subscriptionServiceItemsRouting = RouterModule.forChild([
    { path: 'customer/:customerId/serviceItemsSet/:serviceItemsSetId/subscriptionServiceItem/:subscriptionServiceItemId', component: SubscriptionServiceItemFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'customer/:customerId/subscriptionServiceItem/new', component: SubscriptionServiceItemFormComponent, canDeactivate: [CanDeactivateGuard] }
]);
