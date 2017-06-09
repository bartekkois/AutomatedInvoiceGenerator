import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, RouterModule, Routes } from '@angular/router';
import { GroupsService } from '../groups.service';
import { Group } from '../group';
import { RefreshGroupsNavigationService } from '../../shared/refresh-groups-navigation.service';

@Component({
  selector: 'groups-manager',
  templateUrl: './groups-manager.component.html',
  styleUrls: ['./groups-manager.component.css']
})
export class GroupsManagerComponent implements OnInit {
    groups: [Group];

    constructor(private _groupsService: GroupsService,
                private _routerService: Router,
                private _refreshGroupsNavigationService: RefreshGroupsNavigationService) {
    }

    ngOnInit() {
        this._groupsService.getGroups()
            .subscribe(groups => this.groups = groups);
    }

    deleteGroup(group) {
        if (confirm("Czy na pewno chcesz usunąć grupę " + group.name + "?")) {
            var index = this.groups.indexOf(group)
            this.groups.splice(index, 1);

            this._groupsService.deleteGroup(group.id)
                .subscribe(
                success => {
                    this._refreshGroupsNavigationService.sendRefreshEvent();
                },
                error => {
                    if (error.status === 401)
                        this._routerService.navigate(['unauthorized']);

                    alert("Usunięcie grupy nie powiodło się !!!");
                    this.groups.splice(index, 0, group);
                });
        }
    }
}
