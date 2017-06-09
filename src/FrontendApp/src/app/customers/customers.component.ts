import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';

import { CustomersService } from './customers.service';
import { Customer } from './customer';
import { SubscriptionServiceItemsService } from './../subscription-service-items/subscription-service-items.service';
import { OneTimeServiceItemsService } from './../one-time-service-items/one-time-service-items.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
    customers: Customer[];
    filteredCustomers: Customer[];
    showArchived: boolean = false;
    isBusy: boolean = false;

    constructor(private _customersService: CustomersService,
                private _subscriptionServiceItemsService: SubscriptionServiceItemsService,
                private _oneTimeServiceItemsService: OneTimeServiceItemsService,
                private _routerService: Router,
                private _route: ActivatedRoute,
                private _localStorageService: LocalStorageService) {
    }

    ngOnInit() {
        this.isBusy = true;

        this._localStorageService.get('showArchived') == "true" ? this.showArchived = true : this.showArchived = false;

        this._route.params
            .subscribe(params => {
                var groupId = +params["id"];

                if (!isNaN(groupId)) {
                    this._customersService.getCustomersByGroup(groupId)
                        .subscribe(
                        customers => {
                            this.customers = customers;
                            this.filteredCustomers = customers
                        },
                        error => {
                            if (error.status === 401)
                                this._routerService.navigate(['unauthorized']);

                            this.customers = [];
                            this.filteredCustomers = [];
                        });
                }
                else {
                    this._customersService.getCustomers()
                        .subscribe(
                        customers => {
                            this.customers = customers;
                            this.filteredCustomers = customers
                        },
                        error => {
                            if (error.status === 401)
                                this._routerService.navigate(['unauthorized']);

                            this.customers = [];
                            this.filteredCustomers = [];
                        });
                }
            });

        this.isBusy = false;
    }

    filterCustomers(filterTerm) {
        if (filterTerm) {
            var filterTermLowerCase = filterTerm.toLowerCase();
            this.filteredCustomers = this.customers.filter(item =>
                   (item.customerCode.toLowerCase().indexOf(filterTermLowerCase) > -1)
                || (item.name.toLowerCase().indexOf(filterTermLowerCase) > -1)
                || (item.location.toLowerCase().indexOf(filterTermLowerCase) > -1));
        }
        else {
            this.filteredCustomers = this.customers;
        }
    }

    toggleShowArchived() {
        this.showArchived = !this.showArchived;
        this._localStorageService.set('showArchived', this.showArchived ? 'true' : 'false');
    }

    deleteCustomer(customer) {
        if (confirm("Czy na pewno chcesz usunąć kontrahenta " + customer.customerCode + " - " + customer.name + "?")) {
            var index = this.customers.indexOf(customer)
            this.customers.splice(index, 1);

            this._customersService.deleteCustomer(customer.id)
                .subscribe(
                success => {
                },
                error => {
                    if (error.status === 401)
                        this._routerService.navigate(['unauthorized']);

                    alert("Usunięcie kontrahenta nie powiodło się !!!");
                    this.customers.splice(index, 0, customer);
                });
        }
    }

    deleteSubscriptionServiceItem(customerId, serviceItemsSetId, subscriptionServiceItemId, subscriptionServiceItem) {
        if (confirm("Czy na pewno chcesz usunąć usługę abonamentową " + subscriptionServiceItem.name + " - " +  subscriptionServiceItem.subName + "?")) {
            var index = this.customers.find(c => c.id == customerId).serviceItemsSets.find(s => s.id == serviceItemsSetId).subscriptionServiceItems.indexOf(subscriptionServiceItem)
            this.customers.find(c => c.id == customerId).serviceItemsSets.find(s => s.id == serviceItemsSetId).subscriptionServiceItems.splice(index, 1);

            this._subscriptionServiceItemsService.deleteSubscriptionServiceItem(subscriptionServiceItem.id)
                .subscribe(
                success => {
                },
                error => {
                    if (error.status === 401)
                        this._routerService.navigate(['unauthorized']);

                    alert("Usunięcie usługi abonamnetowej " + subscriptionServiceItem.name + " - " +  subscriptionServiceItem.subName + " nie powiodło się !!!");
                    this.customers.find(c => c.id == customerId).serviceItemsSets.find(s => s.id == serviceItemsSetId).subscriptionServiceItems.splice(index, 0, subscriptionServiceItem);
                });
        }
    }

    deleteOneTimeServiceItem(customerId, serviceItemsSetId, oneTimeServiceItemId, oneTimeServiceItem) {
        if (confirm("Czy na pewno chcesz usunąć usługę jednorazową " + oneTimeServiceItem.name + " - " +  oneTimeServiceItem.subName + "?")) {
            var index = this.customers.find(c => c.id == customerId).serviceItemsSets.find(s => s.id == serviceItemsSetId).oneTimeServiceItems.indexOf(oneTimeServiceItem)
            this.customers.find(c => c.id == customerId).serviceItemsSets.find(s => s.id == serviceItemsSetId).oneTimeServiceItems.splice(index, 1);

            this._oneTimeServiceItemsService.deleteOneTimeServiceItem(oneTimeServiceItemId)
                .subscribe(
                success => {
                },
                error => {
                    if (error.status === 401)
                        this._routerService.navigate(['unauthorized']);

                    alert("Usunięcie usługi jednorazowej " + oneTimeServiceItem.name + " - " + oneTimeServiceItem.subName + "nie powiodło się !!!");
                    this.customers.find(c => c.id == customerId).serviceItemsSets.find(s => s.id == serviceItemsSetId).oneTimeServiceItems.splice(index, 0, oneTimeServiceItem);
                });
        }
    }
}
