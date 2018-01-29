import { RouterModule } from '@angular/router';

import { GenerateInvoicesComponent } from './generate-invoices.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const generateInvoicesRouting = RouterModule.forChild([
    { path: 'generate-invoices', component: GenerateInvoicesComponent }
]);
