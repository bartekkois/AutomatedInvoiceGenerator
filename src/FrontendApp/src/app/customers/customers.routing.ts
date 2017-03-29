import { RouterModule  }    		  from '@angular/router';

import { CustomersComponent }         from './customers.component';

export const customersRouting = RouterModule.forChild([
    { path: 'customers/:id', component: CustomersComponent },
    { path: 'customersbygroup/:groupid', component: CustomersComponent },
    { path: 'customers', component: CustomersComponent },
]);
