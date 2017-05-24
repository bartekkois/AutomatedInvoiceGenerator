import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { SubscriptionServiceItemFormComponent } from './subscription-service-item-form.component';

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
        SubscriptionServiceItemFormComponent
    ],
    exports: [
        SubscriptionServiceItemFormComponent
    ],
    providers: [
    ]
})
export class SubscriptionServiceItemsModule {
}
