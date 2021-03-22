import { Injectable } from '@angular/core'
import { Book } from 'src/app/models/Book';
import { Observable, Subject, of } from 'rxjs'
import { ApiService } from '../services/api.service';
import { getBooks } from '../services/test/test-books';
import { UserAccount } from '../models/UserAccount';
import { getUsers } from './test/test-users';
import { TopBooksLoanedReportViewModel } from '../models/TopBooksLoanedReport';
import { getTopBooksLoaned } from './test/test-get-top-books-loaded';
import { TopMemberLoansReport } from '../models/TopMemberLoansReport';
import { getTopMemberLoans } from './test/test-get-top-member-loans';
import { AllMemberLoansReport } from '../models/AllMemberLoansReport';
import { getAllMemberLoans } from './test/test-all-member-loans';
import { AppConfig, APP_CONFIG } from '../app.config';

@Injectable()
export class TestApiService extends ApiService {

    //private baseAPI: string = environment.baseApiUrl;
    //private selectedBook = new Subject<any>();
    //bookSelected = this.selectedBook.asObservable();

    books = getBooks();
    users = getUsers();
    topLoanedBooks = getTopBooksLoaned();
    topMemberLoans = getTopMemberLoans();
    allMemberLoansReport = getAllMemberLoans();

    constructor() {
        super(null, null);
    }

    getBooks(): Observable<Book[]> {
        return of(this.books);
    }

    getBook(id): Observable<Book>
    {
        return of(this.books.find(b => b.id === id));
    }

    saveBook(book) 
    {
        return of(new Book());
    }

    updateBook(book) 
    {
        return of(new Book());
    }

    getUsers(): Observable<UserAccount[]> {
        return of(this.users); 
    }

    getGetTopBooksLoaned(): Observable<TopBooksLoanedReportViewModel[]>
    {
        return of(this.topLoanedBooks);
    }

    getGetTopMemberLoans(): Observable<TopMemberLoansReport[]>
    {
        return of(this.topMemberLoans);
    }

    getLoansReportAllMembers(): Observable<AllMemberLoansReport[]>
    {
        return of(this.allMemberLoansReport);
    }
}