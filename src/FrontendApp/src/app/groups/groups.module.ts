import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { Group } from './group';
import { GroupsComponent } from './groups.component';
import { GroupsService } from './groups.service';
import { GroupsManagerComponent } from './groups-manager/groups-manager.component';
import { GroupsManagerGroupFormComponent } from './groups-manager/groups-manager-group-form.component';
import { SharedModule } from '../shared/shared.module';
import { CanDeactivateGuard } from '../shared/can-deactivate-guard.service'

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule,
        SharedModule
    ],
    declarations: [
        GroupsComponent,
        GroupsManagerComponent,
        GroupsManagerGroupFormComponent
    ],
    exports: [
        GroupsComponent,
        GroupsManagerComponent,
        GroupsManagerGroupFormComponent
    ],
    providers: [
        GroupsService,
        CanDeactivateGuard
    ]
})
export class GroupsModule {
}
