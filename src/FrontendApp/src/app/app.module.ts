import { BrowserModule }    from '@angular/platform-browser';
import { NgModule }         from '@angular/core';
import { FormsModule }      from '@angular/forms';
import { HttpModule }       from '@angular/http';
import { CookieService }    from 'ngx-cookie-service';

import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { GroupsModule } from './groups/groups.module';
import { groupsRouting } from './groups/groups.routing';
import { serviceItemsSetRouting } from './service-items-sets/service-items-sets.routing';
import { subscriptionServiceItemsRouting } from './subscription-service-items/subscription-service-items.routing';
import { oneTimeServiceItemsRouting } from './one-time-service-items/one-time-service-items.routing';
import { generateInvoicesRouting } from './generate-invoices/generate-invoices.routing';
import { exportRouting } from './export/export.routing';
import { routing } from './app.routing';
import { CustomersModule }  from './customers/customers.module';
import { customersRouting } from './customers/customers.routing';
import { InvoicesModule } from './invoices/invoices.module';
import { invoicesRouting } from './invoices/invoices.routing';
import { InvoiceItemsModule } from './invoice-items/invoice-items.module';
import { invoiceItemsRouting } from './invoice-items/invoice-items.routing';
import { ServiceItemsSetsModule } from './service-items-sets/service-items-sets.module';
import { OneTimeServiceItemsModule } from './one-time-service-items/one-time-service-items.module';
import { SubscriptionServiceItemsModule } from './subscription-service-items/subscription-service-items.module';
import { GenerateInvoicesModule } from './generate-invoices/generate-invoices.module';
import { ExportModule } from './export/export.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    SharedModule,
    GroupsModule,
    CustomersModule,
    ServiceItemsSetsModule,
    OneTimeServiceItemsModule,
    SubscriptionServiceItemsModule,
    OneTimeServiceItemsModule,
    InvoicesModule,
    InvoiceItemsModule,
    GenerateInvoicesModule,
    ExportModule,
    groupsRouting,
    customersRouting,
    serviceItemsSetRouting,
    subscriptionServiceItemsRouting,
    oneTimeServiceItemsRouting,
    invoicesRouting,
    invoiceItemsRouting,
    generateInvoicesRouting,
    exportRouting,
    routing
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
