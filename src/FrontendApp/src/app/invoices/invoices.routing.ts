import { RouterModule } from '@angular/router';

import { InvoicesComponent } from './invoices.component';
import { InvoiceFormComponent } from './invoice-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const invoicesRouting = RouterModule.forChild([
    { path: 'invoice/:invoiceId/customer/:customerId', component: InvoiceFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'invoice/new', component: InvoiceFormComponent, canDeactivate: [CanDeactivateGuard] },
    { path: 'invoices/:id', component: InvoicesComponent },
    { path: 'invoices', component: InvoicesComponent }
]);
