import { RouterModule } from '@angular/router';

import { InvoiceItemFormComponent } from './invoice-item-form.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const invoiceItemsRouting = RouterModule.forChild([
  { path: 'invoice/:invoiceId/invoiceItem/:invoiceItemId', component: InvoiceItemFormComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'invoice/:invoiceId/invoiceItem/new', component: InvoiceItemFormComponent, canDeactivate: [CanDeactivateGuard] },
]);
