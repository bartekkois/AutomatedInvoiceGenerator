import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { ServiceItemInvoiceHistoryComponent } from '../service-item-invoice-history/service-item-invoice-history.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        ServiceItemInvoiceHistoryComponent
    ],
    exports: [
        ServiceItemInvoiceHistoryComponent
    ],
    providers: [
    ]
})
export class ServiceItemInvoiceHistoryModule {
}
