import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { Customer } from './customer';
import { ServiceItemSet } from './service-item-set';
import { OneTimeServiceItem } from './one-time-service-item';
import { SubscriptionServiceItem } from './subscription-service-item';
import { CustomersComponent } from './customers.component';
import { CustomerFormComponent } from './customer-form.component';
import { CustomersService } from './customers.service';
import { GroupsService } from '../groups/groups.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        CustomersComponent,
        CustomerFormComponent
    ],
    exports: [
        CustomersComponent,
        CustomerFormComponent
    ],
    providers: [
        CustomersService,
        GroupsService
    ]
})
export class CustomersModule {
}
