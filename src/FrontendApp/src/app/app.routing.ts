import { RouterModule  }      from '@angular/router';

import { AppComponent }       from './app.component';
import { CustomersComponent } from './customers/customers.component';

export const routing = RouterModule.forRoot([
    { path: '', component: CustomersComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' }
]);
