import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { SubscriptionServiceItem } from './subscription-service-item';
import { SubscriptionServiceItemFormComponent } from './subscription-service-item-form.component';
import { SubscriptionServiceItemsService } from './subscription-service-items.service';

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
        //SubscriptionServiceItemsComponent,
        SubscriptionServiceItemFormComponent
    ],
    exports: [
        //SubscriptionServiceItemsComponent,
        SubscriptionServiceItemFormComponent
    ],
    providers: [
        SubscriptionServiceItemsService
    ]
})
export class SubscriptionServiceItemsModule {
}
