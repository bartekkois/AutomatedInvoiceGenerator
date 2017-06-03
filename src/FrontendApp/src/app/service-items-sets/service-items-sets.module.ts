import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { ServiceItemsSetsComponent } from './service-items-sets.component';
import { ServiceItemsSetFormComponent } from './service-items-set-form.component';
import { ServiceItemsSetsService } from '../service-items-sets/service-items-sets.service';
import { CustomersService } from '../customers/customers.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        ServiceItemsSetsComponent,
        ServiceItemsSetFormComponent
    ],
    exports: [
        ServiceItemsSetsComponent,
        ServiceItemsSetFormComponent
    ],
    providers: [
        ServiceItemsSetsService,
        CustomersService
    ]
})
export class ServiceItemsSetsModule {
}
