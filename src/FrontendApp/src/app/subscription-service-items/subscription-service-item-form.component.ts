import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/debounceTime';

import { SubscriptionServiceItem } from './subscription-service-item';
import { SubscriptionServiceItemsService } from './subscription-service-items.service';
import { ServiceItemsSet } from '../service-items-sets/service-items-set';
import { ServiceItemsSetsService } from '../service-items-sets/service-items-sets.service';
import { Customer } from '../customers/customer';
import { CustomersService } from '../customers/customers.service';

@Component({
  selector: 'subscription-service-item-form',
  templateUrl: './subscription-service-item-form.component.html',
  styleUrls: ['./subscription-service-item-form.component.css'],
  providers: [SubscriptionServiceItemsService, ServiceItemsSetsService, CustomersService]
})
export class SubscriptionServiceItemFormComponent implements OnInit  {
    subscriptionServiceItem = new SubscriptionServiceItem();
    subscriptionServiceItemForm: FormGroup;
    currentCustomer: Customer;
    currentCustomerServiceItemsSet: ServiceItemsSet;
    currentCustomerServiceItemsSets = [ServiceItemsSet];
    subscriptionServiceTemplateTypeIsSet = false;
    title: string = " ";
    isBusy: boolean = false;

    constructor(private _fb: FormBuilder,
                private _subscriptionServiceItemsService: SubscriptionServiceItemsService,
                private _serviceItemsSetsService: ServiceItemsSetsService,
                private _customersService: CustomersService, 
                private _routerService: Router,
                private _route: ActivatedRoute) {
        this.createForm();
    }

    createForm() {
        this.subscriptionServiceItemForm = this._fb.group({
            serviceTemplate: [],
            serviceCategoryType: ['', Validators.required],
            remoteSystemServiceCode: ['', Validators.required],
            name: ['', Validators.required],
            subName: [],
            isSubNamePrinted: [],
            specificLocation: ['', Validators.required],
            serviceItemCustomerSpecificTag: [],
            notes: [],
            isValueVariable: ['', Validators.required],
            netValue: ['', Validators.required],
            quantity: ['', Validators.required],
            vatRate: ['', Validators.required],
            grossValueAdded: ['', Validators.required],
            isManual: [],
            isBlocked: [],
            isSuspended: [],
            startDate: ['', Validators.required],
            endDate: [],
            isArchived: [],
            serviceItemsSetId: ['', Validators.required]
        });
    }

    ngOnInit() {
        this.isBusy = true;

        var id = this._route.params
            .subscribe(params => {
                var subscriptionServiceItemId = +params["subscriptionServiceItemId"];
                var customerId = +params["customerId"];
                var serviceItemsSetId = +params["serviceItemsSetId"];

                this._customersService.getCustomer(customerId)
                    .subscribe(
                    customer => {
                        this.currentCustomer = customer;
                        this.title = subscriptionServiceItemId ? "Edytuj usługę abonamentową kontrahenta " + this.currentCustomer.customerCode + " - " + this.currentCustomer.name : "Dodaj usługę abonamentową " + this.currentCustomer.customerCode + " - " + this.currentCustomer.name;
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
                        this.currentCustomerServiceItemsSets = serviceItemsSets;
                    },
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['customers']);
                        }
                    });

                    // Add SubscriptionServiceItem
                    if (!subscriptionServiceItemId) {
                        this.isBusy = false;

                        return;
                    }

                    // Edit SubscriptionServiceItem                   
                    this._subscriptionServiceItemsService.getSubscriptionServiceItem(subscriptionServiceItemId)
                      .subscribe(
                      subscriptionServiceItem => {
                          this.subscriptionServiceItem = subscriptionServiceItem;
                          this.isBusy = false;
                      },
                      error => {
                          if (error.status === 401)
                              this._routerService.navigate(['unauthorized']);

                          if (error.status === 404) 
                              this._routerService.navigate(['customers']);
                          
                          this.isBusy = false;
                      });
            });


        // Realtime form recalculations
        this.subscriptionServiceItemForm.controls["netValue"].valueChanges
            .debounceTime(100)
            .subscribe((ngModelChange) => {
                this.subscriptionServiceItem.grossValueAdded = (this.subscriptionServiceItem.netValue * this.subscriptionServiceItem.quantity) * (1 + (this.subscriptionServiceItem.vatRate / 100));
            });

        this.subscriptionServiceItemForm.controls["quantity"].valueChanges
            .debounceTime(100)
            .subscribe((ngModelChange) => {
                if (Number.isInteger(this.subscriptionServiceItem.quantity))
                    this.subscriptionServiceItem.grossValueAdded = (this.subscriptionServiceItem.netValue * this.subscriptionServiceItem.quantity) * (1 + (this.subscriptionServiceItem.vatRate / 100));
            });

        this.subscriptionServiceItemForm.controls["vatRate"].valueChanges
            .debounceTime(100)
            .subscribe((ngModelChange) => {
                this.subscriptionServiceItem.grossValueAdded = (this.subscriptionServiceItem.netValue * this.subscriptionServiceItem.quantity) * (1 + (this.subscriptionServiceItem.vatRate / 100));
            });

        this.subscriptionServiceItemForm.controls["grossValueAdded"].valueChanges
            .debounceTime(100)
            .subscribe((ngModelChange) => {
                this.subscriptionServiceItem.netValue = this.subscriptionServiceItem.grossValueAdded / (1 + (this.subscriptionServiceItem.vatRate / 100)) / this.subscriptionServiceItem.quantity;
            });
    }

    save() {
        var result;

        if (this.subscriptionServiceItem.id)
            result = this._subscriptionServiceItemsService.updateSubscriptionServiceItem(this.subscriptionServiceItem);
        else
            result = this._subscriptionServiceItemsService.addSubscriptionServiceItem(this.subscriptionServiceItem)

        result.subscribe(success => {
            this.subscriptionServiceItemForm.markAsPristine();
            this._routerService.navigate(['customers']);
        });
    }

    setSubscriptionServiceItemDefaultValues(subscriptionServiceTemplateType) {
        if (this.subscriptionServiceTemplateTypeIsSet == true) {
            if (!confirm("Czy chcesz odrzucić wybrany szablon usługi oraz wprowadzone dane?"))
                return;
        }

        this.subscriptionServiceTemplateTypeIsSet = true;
        var subscriptionServiceItemTemplate = new SubscriptionServiceItem();

        switch (subscriptionServiceTemplateType) {
            case "0":
                subscriptionServiceItemTemplate.serviceCategoryType = 0;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1200";
                subscriptionServiceItemTemplate.name = "Internet";
                subscriptionServiceItemTemplate.subName = "fiberPORT 100/20 Mb/s";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 120.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 147.60;
                break;
            case "1":
                subscriptionServiceItemTemplate.serviceCategoryType = 5;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1209";
                subscriptionServiceItemTemplate.name = "Dzierżawa modemu z funkcją routera Wi-Fi";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 6.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 7.38;
                break;
            case "2":
                subscriptionServiceItemTemplate.serviceCategoryType = 5;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1209";
                subscriptionServiceItemTemplate.name = "Dzierżawa modemu z funkcją routera Wi-Fi";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 4.87;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 6.00;
                break;
            case "3":
                subscriptionServiceItemTemplate.serviceCategoryType = 0;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1275";
                subscriptionServiceItemTemplate.name = "Opłata za dodatkowy stały publiczny adres IP";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 10.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 12.30;
                break;
            case "4":
                subscriptionServiceItemTemplate.serviceCategoryType = 1;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1210";
                subscriptionServiceItemTemplate.name = "Telefon";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 19.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 23.37;
                break;
            case "5":
                subscriptionServiceItemTemplate.serviceCategoryType = 1;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1210";
                subscriptionServiceItemTemplate.name = "Telefon";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 15.44;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 19.00;
                break;
            case "6":
                subscriptionServiceItemTemplate.serviceCategoryType = 1;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1211";
                subscriptionServiceItemTemplate.name = "Telefon - połączenia telefoniczne";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = true;
                subscriptionServiceItemTemplate.netValue = 0.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 0.00;
                break;
            case "7":
                subscriptionServiceItemTemplate.serviceCategoryType = 2;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1230";
                subscriptionServiceItemTemplate.name = "Dzierżawa pary miedzianej";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 80.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 98.40;
                break;
            case "8":
                subscriptionServiceItemTemplate.serviceCategoryType = 2;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1263";
                subscriptionServiceItemTemplate.name = "Dzierżawa ciemnych włókien światłowodowych";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 80.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 98.40;
                break;
            case "9":
                subscriptionServiceItemTemplate.serviceCategoryType = 3;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "";
                subscriptionServiceItemTemplate.name = "Transmisja danych";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 250.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 307.50;
                break;
            case "10":
                subscriptionServiceItemTemplate.serviceCategoryType = 4;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "";
                subscriptionServiceItemTemplate.name = "Telewizja";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 36.94;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 8;
                subscriptionServiceItemTemplate.grossValueAdded = 39.90;
                break;
            case "11":
                subscriptionServiceItemTemplate.serviceCategoryType = 5;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "1270";
                subscriptionServiceItemTemplate.name = "Opłata za fakturę tradycyjną";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 2.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 2.46;
                break;
            case "12":
                subscriptionServiceItemTemplate.serviceCategoryType = 5;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "";
                subscriptionServiceItemTemplate.name = "Inny";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 0.00;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 0.00;
                break;
            default:
                subscriptionServiceItemTemplate.serviceCategoryType = 5;
                subscriptionServiceItemTemplate.remoteSystemServiceCode = "";
                subscriptionServiceItemTemplate.name = "Inny";
                subscriptionServiceItemTemplate.subName = "";
                subscriptionServiceItemTemplate.isSubNamePrinted = false;
                subscriptionServiceItemTemplate.isValueVariable = false;
                subscriptionServiceItemTemplate.netValue = 0;
                subscriptionServiceItemTemplate.quantity = 1;
                subscriptionServiceItemTemplate.vatRate = 23;
                subscriptionServiceItemTemplate.grossValueAdded = 0;
        }

        subscriptionServiceItemTemplate.serviceItemsSetId = this.subscriptionServiceItem.serviceItemsSetId;
        subscriptionServiceItemTemplate.specificLocation = this.currentCustomer.location;
        subscriptionServiceItemTemplate.serviceItemCustomerSpecificTag = "";
        subscriptionServiceItemTemplate.notes = "";
        subscriptionServiceItemTemplate.isManual = false;
        subscriptionServiceItemTemplate.isBlocked = false;
        subscriptionServiceItemTemplate.isSuspended = false;
        subscriptionServiceItemTemplate.startDate = "";
        subscriptionServiceItemTemplate.endDate = "";
        subscriptionServiceItemTemplate.isArchived = false;

        this.subscriptionServiceItem = subscriptionServiceItemTemplate;
    }

    canDeactivate() {
        if (this.subscriptionServiceItemForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
