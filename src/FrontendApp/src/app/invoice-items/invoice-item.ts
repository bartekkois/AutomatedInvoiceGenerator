export class InvoiceItem {
    id: number;
    remoteSystemServiceCode: string;
    description: string;
    quantity: number;
    units: string;
    netUnitPrice: number;
    netValueAdded: number;
    vatRate: number;
    grossValueAdded: number;
    invoicePeriodStartTime: string;
    invoicePeriodEndTime: string;
    invoiceId: number;
    serviceItemId: number;
}
