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
import { InvoicesComponent } from './invoices.component';
import { InvoiceFormComponent } from './invoice-form.component';
import { InvoicesService } from './invoices.service';
import { InvoiceItemsService } from '../invoice-items/invoice-items.service';
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
        InvoicesComponent,
        InvoiceFormComponent
    ],
    exports: [
        InvoicesComponent,
        InvoiceFormComponent
    ],
    providers: [
        CustomersService,
        GroupsService,
        SubscriptionServiceItemsService,
        OneTimeServiceItemsService,
        InvoicesService,
        InvoiceItemsService
    ]
})
export class InvoicesModule {
}
