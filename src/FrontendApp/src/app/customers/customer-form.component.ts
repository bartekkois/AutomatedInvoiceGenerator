import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { Customer } from './customer';
import { CustomersService } from './customers.service';
import { Group } from '../groups/group';
import { GroupsService } from '../groups/groups.service';

@Component({
  selector: 'customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.css'],
  providers: [CustomersService, GroupsService]
})
export class CustomerFormComponent implements OnInit {
    customer = new Customer();
    customerForm: FormGroup;
    groups = [Group];
    title: string = " ";
    isBusy: boolean = false;

    constructor(private _fb: FormBuilder,
                private _customersService: CustomersService,
                private _groupsService: GroupsService,
                private _routerService: Router,
                private _route: ActivatedRoute) {
        this.createForm();
    }

    createForm() {
        this.customerForm = this._fb.group({
            customerCode: ['', Validators.required],
            name: ['', Validators.required],
            brandName: [],
            location: [],
            notes: [],
            invoiceCustomerSpecificTag: [],
            invoiceDelivery: ['', Validators.required],
            priceCalculation: ['', Validators.required],
            paymentMethod: ['', Validators.required],
            paymentPeriod: [],
            isVatEu: [],
            isBlocked: [],
            isSuspended: [],
            isArchived: [],
            groupId: ['', Validators.required]
        });
    }

    ngOnInit() {
        this.isBusy = true;

        this._groupsService.getGroups()
            .subscribe(groups => this.groups = groups);

        var id = this._route.params
            .subscribe(params => {
                var id = +params["id"];

                this.title = id ? "Edytuj kontrahenta" : "Dodaj kontrahenta";
                
                if (!id) {
                    this.customer.invoiceDelivery = 0;
                    this.customer.priceCalculation = 0;
                    this.customer.paymentMethod = 0;
                    this.customer.paymentPeriod = 14;
                    this.customer.isVatEu = false;
                    this.customer.isBlocked = false;
                    this.customer.isSuspended = false;
                    this.customer.isArchived = false;

                    this.isBusy = false;
                    return;
                }

                this._customersService.getCustomer(id)
                    .subscribe(
                    customer => {
                        this.customer = customer;
                        this.isBusy = false;
                    }, 
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['customers']);
                        }
                        this.isBusy = false;
                    });
            });
    }

    save() {
        var result;

        if (this.customer.id)
            result = this._customersService.updateCustomer(this.customer);
        else
            result = this._customersService.addCustomer(this.customer)

        result.subscribe(success => {
            this.customerForm.markAsPristine();
            this._routerService.navigate(['customers']);
        });
    }

    canDeactivate() {
        if (this.customerForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
