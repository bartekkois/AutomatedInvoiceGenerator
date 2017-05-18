import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { OneTimeServiceItem } from './one-time-service-item';
import { OneTimeServiceItemFormComponent } from './one-time-service-item-form.component';
import { OneTimeServiceItemsService } from './one-time-service-items.service';

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
        //OneTimeServiceItemsComponent,
        OneTimeServiceItemFormComponent
    ],
    exports: [
        //OneTimeServiceItemsComponent,
        OneTimeServiceItemFormComponent
    ],
    providers: [
        //OneTimeServiceItemsService
    ]
})
export class OneTimeServiceItemsModule {
}
