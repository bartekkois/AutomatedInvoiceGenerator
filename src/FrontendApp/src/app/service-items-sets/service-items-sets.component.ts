import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { ServiceItemsSet } from '../service-items-sets/service-items-set';
import { ServiceItemsSetsService } from '../service-items-sets/service-items-sets.service';
import { Customer } from '../customers/customer';
import { CustomersService } from '../customers/customers.service';

@Component({
  selector: 'app-service-items-sets',
  templateUrl: './service-items-sets.component.html',
  styleUrls: ['./service-items-sets.component.css']
})
export class ServiceItemsSetsComponent implements OnInit {
    serviceItemsSets: ServiceItemsSet[];
    filteredServiceItemsSets: ServiceItemsSet[];
    currentCustomer: Customer;
    currentCustomerId: number;
    title: string;
    showArchived: boolean = false;
    isBusy: boolean = false;

    constructor(private _customersService: CustomersService,
                private _serviceItemsSetsService: ServiceItemsSetsService,
                private _routerService: Router,
                private _route: ActivatedRoute) {
    }

    ngOnInit() {
        this.isBusy = true;

        this._route.params
            .subscribe(params => {
                var customerId = +params["customerId"];
                this.currentCustomerId = customerId;

                this._customersService.getCustomer(customerId)
                    .subscribe(
                    customer => {
                        this.currentCustomer = customer;
                        this.title = "Zestawy usług kontrahenta " + this.currentCustomer.name;
                    },
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['customers']);
                        }
                    });

                this._serviceItemsSetsService.getServiceItemsSetsByCustomer(customerId)
                    .subscribe(
                    serviceItemsSets => {
                        this.serviceItemsSets = serviceItemsSets;
                        this.filteredServiceItemsSets = serviceItemsSets;
                        this.isBusy = false;
                    },
                    error => {
                        this.serviceItemsSets = [];
                        this.filteredServiceItemsSets = [];
                        this.isBusy = false;
                    }
                );
            });
    }

    toggleShowArchived() {
        this.showArchived = !this.showArchived;
    }

    deleteServiceItemsSet(customerId, serviceItemsSetId, serviceItemsSet) {
        if (confirm("Czy na pewno chcesz usunąć zestaw usług " + serviceItemsSet.name + "?")) {
            var index = this.serviceItemsSets.indexOf(serviceItemsSet)
            this.serviceItemsSets.splice(index, 1);

            this._serviceItemsSetsService.deleteServiceItemsSet(serviceItemsSetId)
                .subscribe(
                success => {
                },
                error => {
                    alert("Usunięcie zestawu usług " + serviceItemsSet.name + " nie powiodło się !!!");
                    this.serviceItemsSets.splice(index, 0, serviceItemsSet);
                });
        }
    }
}
