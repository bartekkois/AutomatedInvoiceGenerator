import { InvoiceItemShort } from "../invoice-items/invoice-item-short";

export class  ServiceItem {
    id: number;
    serviceCategoryType: number;
    remoteSystemServiceCode: string;
    name: string;
    subName: string;
    fullName: string;
    isSubNamePrinted: boolean;
    specificLocation: string;
    serviceItemCustomerSpecificTag: string;
    notes: string;
    isValueVariable: boolean;
    netValue: number;
    quantity: number;
    vatRate: number;
    grossValueAdded: number;
    isManual: boolean;
    isBlocked: boolean;
    isSuspended: boolean;
    isArchived: boolean;
    serviceItemsSetId: number;
    invoiceItemsForLastYearShorts: InvoiceItemShort[];
}
