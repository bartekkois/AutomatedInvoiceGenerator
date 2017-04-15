﻿import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { CustomersService } from './customers.service';
import { Customer } from './customer';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
    customers: Customer[];
    showArchived: boolean = false;

    constructor(private _customersService: CustomersService,
                private _routerService: Router,
                private _route: ActivatedRoute) {
    }

    ngOnInit() {
        this._route.params
            .subscribe(params => {
                var groupId = +params["id"];

                if (!isNaN(groupId)) {
                    this._customersService.getCustomersByGroup(groupId)
                        .subscribe(
                          customers => this.customers = customers,
                          error => this.customers = []
                        );
                }
                else {
                    this._customersService.getCustomers()
                        .subscribe(
                          customers =>  this.customers = customers,
                          error => this.customers = []
                        );
                }
            });
    }

    toggleShowArchived() {
        this.showArchived = !this.showArchived;
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
                    alert("Usunięcie kontrahenta nie powiodło się !!!");
                    this.customers.splice(index, 0, customer);
                });
        }
    }
}
