import { OneTimeServiceItem } from '../one-time-service-items/one-time-service-item';
import { SubscriptionServiceItem } from '../subscription-service-items/subscription-service-item';

export class ServiceItemsSet {
    id: number;
    name: string;
    rowVersion: string;
    customerId: number;

    oneTimeServiceItems: OneTimeServiceItem[];
    subscriptionServiceItems: SubscriptionServiceItem[];
}
