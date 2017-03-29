import { Component, OnInit, ViewChild } from '@angular/core';

import { GroupsService } from './groups.service';
import { GroupsManagerComponent } from './groups-manager/groups-manager.component';
import { RefreshGroupsNavigationService } from '../shared/refresh-groups-navigation.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {
    groups: any[];

    constructor(private _groupsService: GroupsService, private _refreshGroupsNavigationService: RefreshGroupsNavigationService) { }

    ngOnInit() {
        this.renderGroupsNavigation();

        this._refreshGroupsNavigationService.refreshGroupsNavigation
            .subscribe(
            success => {
                this.renderGroupsNavigation();
            });
    }

    renderGroupsNavigation() {
        this._groupsService.getGroups()
            .subscribe(groups => this.groups = groups);
    }
}
