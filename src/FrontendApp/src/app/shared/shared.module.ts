import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RefreshGroupsNavigationService } from './refresh-groups-navigation.service';
import { UnauthotizedComponent } from './unauthorized.component';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        UnauthotizedComponent
    ],
    exports: [
        UnauthotizedComponent
    ],
    providers: [
        RefreshGroupsNavigationService
    ]
})
export class SharedModule {
}