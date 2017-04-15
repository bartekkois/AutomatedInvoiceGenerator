import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { OneTimeServiceItem } from './one-time-service-item';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        //OneTimeServiceItemsComponent,
        //OneTimeServiceItemFormComponent
    ],
    exports: [
        //OneTimeServiceItemsComponent,
        //OneTimeServiceItemFormComponent
    ],
    providers: [
        //OneTimeServiceItemsService
    ]
})
export class OneTimeServiceItemsModule {
}
