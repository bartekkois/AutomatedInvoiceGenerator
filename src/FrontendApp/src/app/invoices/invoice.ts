import { InvoiceItem } from '../invoice-items/invoice-item';
import { CustomerShort } from '../customers/customerShort';

export class Invoice {
    id: number;
    description: string;
    invoiceDate: string;
    isExported: boolean;
    invoiceDelivery: number;
    priceCalculation: number;
    paymentMethod: number;
    paymentPeriod: number;
    rowVersion: string;
    customerId: number;
    customer: CustomerShort;
    serviceItemSetId: number;

    invoiceItems: InvoiceItem[];
}
