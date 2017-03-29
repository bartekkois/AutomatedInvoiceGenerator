import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContenteditableComponent } from './contenteditable.component';
import { RefreshGroupsNavigationService } from './refresh-groups-navigation.service';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        ContenteditableComponent
    ],
    exports: [
        ContenteditableComponent
    ],
    providers: [
        RefreshGroupsNavigationService
    ]
})
export class SharedModule {
}