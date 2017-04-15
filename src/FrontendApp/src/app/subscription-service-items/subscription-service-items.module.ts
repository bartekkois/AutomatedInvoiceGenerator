import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { SubscriptionServiceItem } from './subscription-service-item';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        //SubscriptionServiceItemsComponent,
        //SubscriptionServiceItemFormComponent
    ],
    exports: [
        //SubscriptionServiceItemsComponent,
        //SubscriptionServiceItemFormComponent
    ],
    providers: [
        //SubscriptionServiceItemsService
    ]
})
export class SubscriptionServiceItemsModule {
}
