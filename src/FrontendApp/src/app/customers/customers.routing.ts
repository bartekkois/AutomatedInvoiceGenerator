import { RouterModule } from '@angular/router';

import { CustomersComponent } from './customers.component';
import { CustomerFormComponent } from './customer-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const customersRouting = RouterModule.forChild([
    { path: 'customer/:id', component: CustomerFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'customer/new', component: CustomerFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'customers/:id', component: CustomersComponent },
    { path: 'customers', component: CustomersComponent }
]);
