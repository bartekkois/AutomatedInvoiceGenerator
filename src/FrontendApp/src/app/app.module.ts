import { BrowserModule }    from '@angular/platform-browser';
import { NgModule }         from '@angular/core';
import { FormsModule }      from '@angular/forms';
import { HttpModule }       from '@angular/http';

import { AppComponent }     from './app.component';
import { GroupsModule }     from './groups/groups.module';
import { groupsRouting }    from './groups/groups.routing';
import { routing }          from './app.routing';
import { CustomersModule }  from './customers/customers.module';
import { customersRouting } from './customers/customers.routing';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    GroupsModule,
    CustomersModule,
    groupsRouting,
    customersRouting,
    routing
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
