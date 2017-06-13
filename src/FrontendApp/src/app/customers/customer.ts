import { ServiceItemsSet } from '../service-items-sets/service-items-set';


export class Customer {
    id: number;
    customerCode: string;
    shippingCustomerCode: string;
    name: string;
    brandName: string;
    location: string;
    notes: string;
    invoiceCustomerSpecificTag: string;
    invoiceDelivery: number;
    priceCalculation: number;
    paymentMethod: number;
    paymentPeriod: number;
    isVatEu: boolean;
    isBlocked: boolean;
    isSuspended: boolean;
    isArchived: boolean;
    groupId: number;

    serviceItemsSets: ServiceItemsSet[];
}
