import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { ServiceItemSet } from './service-items-set';
import { OneTimeServiceItem } from '../one-time-service-items/one-time-service-item';
import { SubscriptionServiceItem } from '../subscription-service-items/subscription-service-item';
import { ServiceItemsSetsService } from './service-items-sets.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        //ServiceItemSetsComponent,
        //ServiceItemSetFormComponent
    ],
    exports: [
        //ServiceItemSetsComponent,
        //ServiceItemSetFormComponent
    ],
    providers: [
        ServiceItemsSetsService
    ]
})
export class ServiceItemsSetsModule {
}
