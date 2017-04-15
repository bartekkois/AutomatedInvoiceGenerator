﻿import { OneTimeServiceItem } from '../one-time-service-items/one-time-service-item';
import { SubscriptionServiceItem } from '../subscription-service-items/subscription-service-item';

export class ServiceItemSet {
    id: number;
    customerId: string;

    oneTimeServiceItems: OneTimeServiceItem[];
    subscriptionServiceItems: SubscriptionServiceItem[];
}
