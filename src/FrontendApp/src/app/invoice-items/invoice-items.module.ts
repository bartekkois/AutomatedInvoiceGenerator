import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { Customer } from '../customers/customer';
import { ServiceItemsSet } from '../service-items-sets/service-items-set';
import { OneTimeServiceItem } from '../one-time-service-items/one-time-service-item';
import { SubscriptionServiceItem } from '../subscription-service-items/subscription-service-item';
import { CustomersComponent } from '../customers/customers.component';
import { CustomersService } from '../customers/customers.service';
import { InvoiceItemFormComponent } from './invoice-item-form.component';
import { InvoiceItemsService } from './invoice-items.service';
import { GroupsService } from '../groups/groups.service';
import { SubscriptionServiceItemsService } from './../subscription-service-items/subscription-service-items.service';
import { OneTimeServiceItemsService } from './../one-time-service-items/one-time-service-items.service';
import { CurrencyMaskModule } from "ng2-currency-mask";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule,
        CurrencyMaskModule
    ],
    declarations: [
        InvoiceItemFormComponent
    ],
    exports: [
        InvoiceItemFormComponent
    ],
    providers: [
        CustomersService,
        GroupsService,
        SubscriptionServiceItemsService,
        OneTimeServiceItemsService,
        InvoiceItemsService
    ]
})
export class InvoiceItemsModule {
}
