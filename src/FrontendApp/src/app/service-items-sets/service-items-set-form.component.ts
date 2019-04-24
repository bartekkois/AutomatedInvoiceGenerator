import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';

import { ServiceItemsSet } from '../service-items-sets/service-items-set';
import { ServiceItemsSetsService } from '../service-items-sets/service-items-sets.service';
import { Customer } from '../customers/customer';
import { CustomersService } from '../customers/customers.service';

@Component({
  selector: 'service-items-set-form',
  templateUrl: './service-items-set-form.component.html',
  styleUrls: ['./service-items-set-form.component.css'],
  providers: [ServiceItemsSetsService, CustomersService]
})
export class ServiceItemsSetFormComponent implements OnInit  {
    serviceItemsSet = new ServiceItemsSet();
    serviceItemsSetForm: FormGroup;
    currentCustomer: Customer;
    title: string = " ";
    alertIsVisible: boolean = false;
    alertMessage: string = "";
    isBusy: boolean = false;

    constructor(private _fb: FormBuilder,
                private _serviceItemsSetsService: ServiceItemsSetsService,
                private _customersService: CustomersService, 
                private _routerService: Router,
                private _route: ActivatedRoute) {
        this.createForm();
    }

    createForm() {
        this.serviceItemsSetForm = this._fb.group({
            name: ['', Validators.required]
        });
    }

    ngOnInit() {
        this.isBusy = true;

        var id = this._route.params
            .subscribe(params => {
                var customerId = +params["customerId"];
                var serviceItemsSetId = +params["serviceItemsSetId"];

                this._customersService.getCustomer(customerId)
                    .subscribe(
                    customer => {
                        this.currentCustomer = customer;
                        this.title = serviceItemsSetId ? "Edytuj zestaw usług kontrahenta " + this.currentCustomer.customerCode + " - " + this.currentCustomer.name : "Dodaj zestaw usług kontrahenta " + this.currentCustomer.customerCode + " - " + this.currentCustomer.name;
                    },
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['customers']);
                        }
                    });

                // Add ServiceItemsSet
                if (!serviceItemsSetId) {
                    this.serviceItemsSet.customerId = customerId;
                    this.serviceItemsSet.name = "domyślny";
                    this.isBusy = false;

                    return;
                }

                // Edit ServiceItemsSet                
                this._serviceItemsSetsService.getServiceItemsSet(serviceItemsSetId)
                  .subscribe(
                    serviceItemsSet => {
                        this.serviceItemsSet = serviceItemsSet;
                        this.isBusy = false;
                  },
                  error => {
                      if (error.status === 401)
                          this._routerService.navigate(['unauthorized']);

                      if (error.status === 404) 
                          this._routerService.navigate(['customers']);

                      if (error.status === 409) {
                          this.alertMessage = "Wystąpił błąd. Element został zmodyfikowany przez innego użytkownika";
                          this.alertIsVisible = true;
                      }

                      this.isBusy = false;
                  });
            });
    }

    save() {
        var result;

        if (this.serviceItemsSet.id)
            result = this._serviceItemsSetsService.updateServiceItemsSet(this.serviceItemsSet);
        else
            result = this._serviceItemsSetsService.addServiceItemsSet(this.serviceItemsSet)

        result.subscribe(success => {
            this.serviceItemsSetForm.markAsPristine();
            this._routerService.navigate(['customer', this.currentCustomer.id, 'serviceItemsSet']);
        });
    }

    canDeactivate() {
        if (this.serviceItemsSetForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
