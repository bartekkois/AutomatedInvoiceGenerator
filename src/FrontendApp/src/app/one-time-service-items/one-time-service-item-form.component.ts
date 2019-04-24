
import {debounceTime} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';


import { OneTimeServiceItem } from './one-time-service-item';
import { OneTimeServiceItemsService } from './one-time-service-items.service';
import { ServiceItemsSet } from '../service-items-sets/service-items-set';
import { ServiceItemsSetsService } from '../service-items-sets/service-items-sets.service';
import { Customer } from '../customers/customer';
import { CustomersService } from '../customers/customers.service';

@Component({
  selector: 'one-time-service-item-form',
  templateUrl: './one-time-service-item-form.component.html',
  styleUrls: ['./one-time-service-item-form.component.css'],
  providers: [OneTimeServiceItemsService, ServiceItemsSetsService, CustomersService]
})
export class OneTimeServiceItemFormComponent implements OnInit  {
    oneTimeServiceItem = new OneTimeServiceItem();
    oneTimeServiceItemForm: FormGroup;
    currentCustomer: Customer;
    currentCustomerServiceItemsSet: ServiceItemsSet;
    currentCustomerServiceItemsSets = [ServiceItemsSet];
    oneTimeServiceTemplateTypeIsSet = false;
    title: string = " ";
    alertIsVisible: boolean = false;
    alertMessage: string = "";
    isBusy: boolean = false;

    constructor(private _fb: FormBuilder,
                private _oneTimeServiceItemsService: OneTimeServiceItemsService,
                private _serviceItemsSetsService: ServiceItemsSetsService,
                private _customersService: CustomersService, 
                private _routerService: Router,
                private _route: ActivatedRoute) {
        this.createForm();
    }

    createForm() {
        this.oneTimeServiceItemForm = this._fb.group({
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
            installationDate: ['', Validators.required],
            isInvoiced: [],
            isArchived: [],
            serviceItemsSetId: ['', Validators.required]
        });
    }

    ngOnInit() {
        this.isBusy = true;

        var id = this._route.params
            .subscribe(params => {
                var oneTimeServiceItemId = +params["oneTimeServiceItemId"];
                var customerId = +params["customerId"];
                var serviceItemsSetId = +params["serviceItemsSetId"];

                this._customersService.getCustomer(customerId)
                    .subscribe(
                    customer => {
                        this.currentCustomer = customer;
                        this.title = oneTimeServiceItemId ? "Edytuj usługę jednorazową kontrahenta " + this.currentCustomer.customerCode + " - " + this.currentCustomer.name : "Dodaj usługę jednorazową " + this.currentCustomer.customerCode + " - " + this.currentCustomer.name;
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

                    // Add OneTimeServiceItem
                    if (!oneTimeServiceItemId) {
                        this.isBusy = false;

                        return;
                    }

                    // Edit OneTimeServiceItem                  
                    this._oneTimeServiceItemsService.getOneTimeServiceItem(oneTimeServiceItemId)
                      .subscribe(
                      oneTimeServiceItem => {
                          this.oneTimeServiceItem = oneTimeServiceItem;
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


        // Realtime form recalculations
        this.oneTimeServiceItemForm.controls["netValue"].valueChanges.pipe(
            debounceTime(100))
            .subscribe((ngModelChange) => {
                this.oneTimeServiceItem.grossValueAdded = (this.oneTimeServiceItem.netValue * this.oneTimeServiceItem.quantity) * (1 + (this.oneTimeServiceItem.vatRate / 100));
            });

        this.oneTimeServiceItemForm.controls["quantity"].valueChanges.pipe(
            debounceTime(100))
            .subscribe((ngModelChange) => {
                if (Number.isInteger(this.oneTimeServiceItem.quantity))
                    this.oneTimeServiceItem.grossValueAdded = (this.oneTimeServiceItem.netValue * this.oneTimeServiceItem.quantity) * (1 + (this.oneTimeServiceItem.vatRate / 100));
            });

        this.oneTimeServiceItemForm.controls["vatRate"].valueChanges.pipe(
            debounceTime(100))
            .subscribe((ngModelChange) => {
                this.oneTimeServiceItem.grossValueAdded = (this.oneTimeServiceItem.netValue * this.oneTimeServiceItem.quantity) * (1 + (this.oneTimeServiceItem.vatRate / 100));
            });

        this.oneTimeServiceItemForm.controls["grossValueAdded"].valueChanges.pipe(
            debounceTime(100))
            .subscribe((ngModelChange) => {
                this.oneTimeServiceItem.netValue = this.oneTimeServiceItem.grossValueAdded / (1 + (this.oneTimeServiceItem.vatRate / 100)) / this.oneTimeServiceItem.quantity;
            });
    }

    save() {
        var result;

        if (this.oneTimeServiceItem.id)
            result = this._oneTimeServiceItemsService.updateOneTimeServiceItem(this.oneTimeServiceItem);
        else
            result = this._oneTimeServiceItemsService.addOneTimeServiceItem(this.oneTimeServiceItem)

        result.subscribe(success => {
            this.oneTimeServiceItemForm.markAsPristine();
            this._routerService.navigate(['customers']);
        });
    }

    setOneTimeServiceItemDefaultValues(oneTimeServiceTemplateType) {
        if (this.oneTimeServiceTemplateTypeIsSet == true) {
            if (!confirm("Czy chcesz odrzucić wybrany szablon usługi oraz wprowadzone dane?"))
                return;
        }

        this.oneTimeServiceTemplateTypeIsSet = true;
        var oneTimeServiceItemTemplate = new OneTimeServiceItem();

        switch (oneTimeServiceTemplateType) {
            case "0":
                oneTimeServiceItemTemplate.serviceCategoryType = 0;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1201";
                oneTimeServiceItemTemplate.name = "Internet %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 49.00;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 60.27;
                break;
            case "1":
                oneTimeServiceItemTemplate.serviceCategoryType = 5;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1211";
                oneTimeServiceItemTemplate.name = "Telefon %DETALE% - instalacja (firma)";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 19.00;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 23.37;
                break;
            case "2":
                oneTimeServiceItemTemplate.serviceCategoryType = 5;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1211";
                oneTimeServiceItemTemplate.name = "Telefon %DETALE% - instalacja (prywatny)";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 15.45;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 19.00;
                break;
            case "3":
                oneTimeServiceItemTemplate.serviceCategoryType = 0;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1275";
                oneTimeServiceItemTemplate.name = "Dzierżawa pary miedzianej %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 99.00;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 121.77;
                break;
            case "4":
                oneTimeServiceItemTemplate.serviceCategoryType = 1;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1210";
                oneTimeServiceItemTemplate.name = "Dzierżawa ciemnych włókien światłowodowych %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 99.00;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 121.77;
                break;
            case "5":
                oneTimeServiceItemTemplate.serviceCategoryType = 1;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1241";
                oneTimeServiceItemTemplate.name = "Transmisja danych %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 99.00;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 121.77;
                break;
            case "6":
                oneTimeServiceItemTemplate.serviceCategoryType = 1;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "1300";
                oneTimeServiceItemTemplate.name = "Telewizja %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = true;
                oneTimeServiceItemTemplate.netValue = 80.49;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 99.00;
                break;
            case "7":
                oneTimeServiceItemTemplate.serviceCategoryType = 5;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "";
                oneTimeServiceItemTemplate.name = "Inny %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 0.00;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 0.00;
                break;
            default:
                oneTimeServiceItemTemplate.serviceCategoryType = 5;
                oneTimeServiceItemTemplate.remoteSystemServiceCode = "";
                oneTimeServiceItemTemplate.name = "Inny %DETALE% - instalacja";
                oneTimeServiceItemTemplate.subName = "";
                oneTimeServiceItemTemplate.isSubNamePrinted = false;
                oneTimeServiceItemTemplate.isValueVariable = false;
                oneTimeServiceItemTemplate.netValue = 0;
                oneTimeServiceItemTemplate.quantity = 1;
                oneTimeServiceItemTemplate.vatRate = 23;
                oneTimeServiceItemTemplate.grossValueAdded = 0;
        }

        oneTimeServiceItemTemplate.serviceItemsSetId = this.oneTimeServiceItem.serviceItemsSetId;
        oneTimeServiceItemTemplate.specificLocation = this.currentCustomer.location;
        oneTimeServiceItemTemplate.serviceItemCustomerSpecificTag = "";
        oneTimeServiceItemTemplate.notes = "";
        oneTimeServiceItemTemplate.isManual = false;
        oneTimeServiceItemTemplate.isBlocked = false;
        oneTimeServiceItemTemplate.isSuspended = false;
        oneTimeServiceItemTemplate.installtionDate = "";
        oneTimeServiceItemTemplate.isInvoiced = false;
        oneTimeServiceItemTemplate.isArchived = false;

        this.oneTimeServiceItem = oneTimeServiceItemTemplate;
    }

    canDeactivate() {
        if (this.oneTimeServiceItemForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
