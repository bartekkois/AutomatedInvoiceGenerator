import { NgModule }            from '@angular/core';
import { CommonModule }        from '@angular/common';
import { FormsModule,
    ReactiveFormsModule } from '@angular/forms';
import { RouterModule }        from '@angular/router';
import { HttpModule }          from '@angular/http';

import { Customer } from './customer';
import { CustomersComponent } from './customers.component';
import { CustomersService } from './customers.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule
    ],
    declarations: [
        CustomersComponent
    ],
    exports: [
        CustomersComponent
    ],
    providers: [
        CustomersService
    ]
})
export class CustomersModule {
}
