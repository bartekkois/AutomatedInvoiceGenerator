import { RouterModule  }      from '@angular/router';

import { AppComponent }       from './app.component';
import { CustomersComponent } from './customers/customers.component';
import { UnauthotizedComponent } from './shared/unauthorized.component';

export const routing = RouterModule.forRoot([
    { path: '', component: CustomersComponent },
    { path: 'unauthorized', component: UnauthotizedComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' }
]);
