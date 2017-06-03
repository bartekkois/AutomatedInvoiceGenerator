import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { Customer } from './customer';
import { ServiceItemsSet } from '../service-items-sets/service-items-set';
import { OneTimeServiceItem } from '../one-time-service-items/one-time-service-item';
import { SubscriptionServiceItem } from '../subscription-service-items/subscription-service-item';
import { CustomersComponent } from './customers.component';
import { CustomerFormComponent } from './customer-form.component';
import { CustomersService } from './customers.service';
import { GroupsService } from '../groups/groups.service';
import { SubscriptionServiceItemsService } from './../subscription-service-items/subscription-service-items.service';
import { OneTimeServiceItemsService } from './../one-time-service-items/one-time-service-items.service';

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
        GroupsService,
        SubscriptionServiceItemsService,
        OneTimeServiceItemsService
    ]
})
export class CustomersModule {
}
