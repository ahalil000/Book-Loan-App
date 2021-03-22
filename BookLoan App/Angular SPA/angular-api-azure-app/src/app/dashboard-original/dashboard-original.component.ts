import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest } from 'rxjs';
import { TopBooksLoanedReportViewModel } from '../models/TopBooksLoanedReport';
import { TopMemberLoansReport } from '../models/TopMemberLoansReport';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-dashboard-original',
  templateUrl: './dashboard-original.component.html',
  styleUrls: ['./dashboard-original.component.scss']
})
export class DashboardOriginalComponent implements OnInit {

  title: string = "Angular Azure App";
  topBooks: TopBooksLoanedReportViewModel[] = [];
  topMembers: TopMemberLoansReport[] = [];
  hasLoadedBooks$ = new BehaviorSubject<boolean>(false);
  hasLoadedMembers$ = new BehaviorSubject<boolean>(false);
  
  constructor(private api: ApiService,  
    private router: Router) {}

  ngOnInit() {
    const topBooks$ = this.api.getGetTopBooksLoaned();
    const topMembers$ = this.api.getGetTopMemberLoans();

    combineLatest(
      [
        topBooks$,
        topMembers$
      ]
    ).subscribe(([topbooks, topmembers]) => {        
        if (this.isNullOrEmpty(topbooks) || this.isNullOrEmpty(topmembers))
          return;
        // if (!topbooks || !topmembers)
        //   return;
        console.log("number top books = " + topbooks.length);
        console.log("number top members = " + topmembers.length);
        this.topBooks = topbooks.slice(0, 5);
        this.topBooks.map(m => 
          {
              m.title = m.bookDetails.title
          });
        this.hasLoadedBooks$.next(true);
        this.topMembers = topmembers.slice(0, 5);
        this.hasLoadedMembers$.next(true);
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
