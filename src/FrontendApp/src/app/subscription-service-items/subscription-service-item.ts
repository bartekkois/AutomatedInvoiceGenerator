export class SubscriptionServiceItem {
    id: number;
    serviceTemplate: number;
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
    startDate: string;
    endDate: string;
    isArchived: boolean;
    serviceItemsSetId: number;
}
