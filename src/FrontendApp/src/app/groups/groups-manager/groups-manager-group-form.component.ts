import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { GroupsService } from '../groups.service';
import { Group } from '../group';
import { RefreshGroupsNavigationService } from '../../shared/refresh-groups-navigation.service';

@Component({
  selector: 'groups-manager-group-form',
  templateUrl: './groups-manager-group-form.component.html',
  styleUrls: ['./groups-manager-group-form.component.css'],
  providers: [GroupsService]
})
export class GroupsManagerGroupFormComponent implements OnInit {
    group = new Group();
    groupForm: FormGroup;
    title: string;

    constructor(private _fb: FormBuilder,
                private _groupsService: GroupsService,
                private _routerService: Router,
                private _route: ActivatedRoute,
                private _refreshGroupsNavigationService: RefreshGroupsNavigationService) {
        this.createForm();
    }

    createForm() {
        this.groupForm = this._fb.group({
            name: ['', Validators.required],
            description: ['', Validators.required],
            colour: []
        });
    }

    ngOnInit() {
        this._route.params
            .subscribe(params => {
                var id = +params["id"];

                this.title = id ? "Edytuj grupę" : "Dodaj grupę";

                if (!id)
                    return;

                this._groupsService.getGroup(id)
                    .subscribe(
                    group => this.group = group,
                    response => {
                        if (response.status == 404) {
                            this._routerService.navigate(['groups-manager']);
                        }
                    });
            });
    }

    save() {
        var result;

        if (this.group.id)
            result = this._groupsService.updateGroup(this.group);
        else
            result = this._groupsService.addGroup(this.group)

        result.subscribe(success => {
            this.groupForm.markAsPristine();
            this._routerService.navigate(['groups-manager']);
            this._refreshGroupsNavigationService.sendRefreshEvent(); 
        });
    }

    canDeactivate() {
        if (this.groupForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
