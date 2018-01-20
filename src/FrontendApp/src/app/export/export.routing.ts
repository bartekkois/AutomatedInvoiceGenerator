import { RouterModule } from '@angular/router';

import { ExportComponent } from './export.component';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

export const exportRouting = RouterModule.forChild([
    { path: 'export', component: ExportComponent }
]);
