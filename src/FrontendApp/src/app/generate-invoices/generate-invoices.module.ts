import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { GenerateInvoicesComponent } from './generate-invoices.component';
import { GenerateInvoicesService } from './generate-invoices.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        GenerateInvoicesComponent,
    ],
    exports: [
        GenerateInvoicesComponent
    ],
    providers: [
        GenerateInvoicesService
    ]
})
export class GenerateInvoicesModule {
}
