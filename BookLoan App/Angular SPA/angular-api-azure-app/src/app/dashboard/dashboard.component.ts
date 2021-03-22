import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, combineLatest } from 'rxjs';
import { RecentMemberOverdueLoansReport } from '../models/RecentMemberOverdueLoans';
import { TopBooksLoanedReportViewModel } from '../models/TopBooksLoanedReport';
import { TopMemberLoansReport } from '../models/TopMemberLoansReport';
import { ApiService } from '../services/api.service';
import { DateUtilities } from '../utilities/dateUtilities';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  title: string = "Angular Azure App";
  topBooks: TopBooksLoanedReportViewModel[] = [];
  topMembers: TopMemberLoansReport[] = [];
  overdueLoans: RecentMemberOverdueLoansReport[] = [];
  hasLoadedBooks$ = new BehaviorSubject<boolean>(false);
  hasLoadedMembers$ = new BehaviorSubject<boolean>(false);
  hasLoadedOverdueLoans$ = new BehaviorSubject<boolean>(false);

  constructor(private api: ApiService,  
    private router: Router) {}

  ngOnInit() {
    const topBooks$ = this.api.getGetTopBooksLoaned();
    const topMembers$ = this.api.getGetTopMemberLoans();
    const memberLoans$ = this.api.getLoansReportAllMembers();

    combineLatest(
      [
        topBooks$,
        topMembers$,
        memberLoans$
      ]
    ).subscribe(([topbooks, topmembers, memberloans]) => 
      {        
        if (this.isNullOrEmpty(topbooks) || 
            this.isNullOrEmpty(topmembers) || 
            this.isNullOrEmpty(memberloans))
          return;
        console.log("number top books = " + topbooks.length);
        console.log("number top members = " + topmembers.length);
        console.log("number members loans = " + memberloans.length);
        this.topBooks = topbooks.slice(0, 5);
        this.topBooks.map(m => 
        {
            m.title = m.bookDetails.title
        });
        this.hasLoadedBooks$.next(true);
        
        this.topMembers = topmembers.slice(0, 5);
        this.hasLoadedMembers$.next(true);

        memberloans.forEach(m => 
        {
            if (m.status === 'Overdue')
            {
              let overdue: RecentMemberOverdueLoansReport = { ...m, 
                daysOverdue: DateUtilities.GetDaysDifference(new Date(m.dateDue), new Date()) };
              this.overdueLoans.push(overdue);
            }
        });
        this.overdueLoans = this.overdueLoans.slice(0, 5);        
        this.hasLoadedOverdueLoans$.next(true);
      }
    );
  }

  isNull(value: any)
  {
    return !value;
  }

  isNullOrEmpty(value: any[])
  {
    return !value || value.length === 0;
  }

}
