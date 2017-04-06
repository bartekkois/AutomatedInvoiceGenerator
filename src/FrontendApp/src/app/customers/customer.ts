export class Customer {
    id: number;
    customerCode: string;
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
}
