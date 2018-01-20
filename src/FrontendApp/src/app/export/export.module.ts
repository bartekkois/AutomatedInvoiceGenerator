import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { ExportComponent } from './export.component';
import { ExportService } from './export.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        ExportComponent,
    ],
    exports: [
        ExportComponent
    ],
    providers: [
        ExportService
    ]
})
export class ExportModule {
}
