import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { Group } from './group';
import { GroupsService } from './groups.service';
import { GroupsManagerComponent } from './groups-manager/groups-manager.component';
import { RefreshGroupsNavigationService } from '../shared/refresh-groups-navigation.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {
    groups: [Group];

    constructor(private _groupsService: GroupsService,
                private _routerService: Router,
                private _refreshGroupsNavigationService: RefreshGroupsNavigationService) { }

    ngOnInit() {
        this.renderGroupsNavigation();

        this._refreshGroupsNavigationService.refreshGroupsNavigation
            .subscribe(
            success => {
                this.renderGroupsNavigation();
            },
            error => {
                if (error.status === 401)
                    this._routerService.navigate(['unauthorized']);
            });
    }

    renderGroupsNavigation() {
        this._groupsService.getGroups()
            .subscribe(groups => this.groups = groups);
    }
}
