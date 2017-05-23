import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RefreshGroupsNavigationService } from './refresh-groups-navigation.service';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
    ],
    exports: [
    ],
    providers: [
        RefreshGroupsNavigationService
    ]
})
export class SharedModule {
}