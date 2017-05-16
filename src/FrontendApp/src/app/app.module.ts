import { BrowserModule }    from '@angular/platform-browser';
import { NgModule }         from '@angular/core';
import { FormsModule }      from '@angular/forms';
import { HttpModule }       from '@angular/http';

import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { GroupsModule } from './groups/groups.module';
import { groupsRouting } from './groups/groups.routing';
import { subscriptionServiceItemsRouting } from './subscription-service-items/subscription-service-items.routing';
import { routing } from './app.routing';
import { CustomersModule }  from './customers/customers.module';
import { customersRouting } from './customers/customers.routing';
import { ServiceItemsSetsModule } from './service-items-sets/service-items-sets.module';
import { OneTimeServiceItemsModule } from './one-time-service-items/one-time-service-items.module';
import { SubscriptionServiceItemsModule } from './subscription-service-items/subscription-service-items.module';

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
    groupsRouting,
    customersRouting,
    subscriptionServiceItemsRouting,
    routing
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
