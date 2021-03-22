import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { NotificationService } from "../notification-service";


@Injectable()
export class TestNotificationService extends NotificationService 
{
    constructor() { 
        super(null);
    }

    getRecentLoansCount(): Observable<number> {
        return of(1);
    }

    getRecentReturnsCount(): Observable<number> {
        return of(1);
    }    
}