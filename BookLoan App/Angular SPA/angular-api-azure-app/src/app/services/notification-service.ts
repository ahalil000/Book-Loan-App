import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { Observable } from "rxjs";


@Injectable()
export class NotificationService 
{
    private baseLoanAPI: string = environment.baseLoanApiUrl;

    constructor(private http: HttpClient) { }

    getRecentLoansCount(): Observable<number> {
        return this.http.get<number>(this.baseLoanAPI + '/Loan/GetRecentLoansCount');
    }

    getRecentReturnsCount(): Observable<number> {
        return this.http.get<number>(this.baseLoanAPI + '/Loan/GetRecentReturnsCount');
    }    
}