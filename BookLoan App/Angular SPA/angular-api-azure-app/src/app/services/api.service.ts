import { Inject, Injectable } from '@angular/core'
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { Book } from 'src/app/models/Book';
import { Observable, Subject} from 'rxjs'
import { environment } from 'src/environments/environment';
import { Loan } from '../models/Loan';
import { BookLoanStatus } from '../models/BookLoanStatus';
import { TopBooksLoanedReportViewModel } from '../models/TopBooksLoanedReport';
import { UserAccount } from '../models/UserAccount';
import { UserRole } from '../models/UserRole';
import { TopMemberLoansReport } from '../models/TopMemberLoansReport';
//import { RecentMemberOverdueLoansReport } from '../models/RecentMemberOverdueLoans';
import { AllMemberLoansReport } from '../models/AllMemberLoansReport';
import { AppConfig, APP_CONFIG } from '../app.config';


@Injectable()
export class ApiService {

    private baseAPI: string = environment.baseApiUrl;
    private baseLoanAPI: string = environment.baseLoanApiUrl;
    private baseIdentityAPI: string = environment.baseIdentityApiUrl;
    private selectedBook = new Subject<any>();
    bookSelected = this.selectedBook.asObservable();

    constructor(@Inject(APP_CONFIG) private config: AppConfig, private http: HttpClient) {}


    getUserRoles(userName): Observable<string[]> {
        return this.http.get<string[]>(this.config.baseIdentityApiUrl + '/ManageUser/GetUserRoles/' + encodeURIComponent(userName)); 
    }

    getUsers(): Observable<UserAccount[]> {
        return this.http.get<UserAccount[]>(this.config.baseIdentityApiUrl + '/ManageUser/UserList'); 
    }

    getUser(id): Observable<UserAccount> {
        return this.http.get<UserAccount>(this.config.baseIdentityApiUrl + '/Users/' + id); 
    }

    getUserRoleInfoById(id): Observable<UserRole> {
        return this.http.get<UserRole>(this.config.baseIdentityApiUrl + '/ManageUser/UserRoleEdit/' + id); 
    }

    registerUser(useraccount)
    {
        return this.http.post<UserAccount>(this.config.baseIdentityApiUrl + '/Users/Register', useraccount);
    }

    updateUser(useraccount)
    {
        return this.http.put<UserAccount>(this.config.baseIdentityApiUrl + '/Users/' + useraccount.id, useraccount);
    }

    addusertorole(userrole)
    {
        return this.http.post<UserRole>(this.config.baseIdentityApiUrl + '/ManageUser/AddRole', userrole);
    }

    deleteuserfromrole(userrole)
    {
        return this.http.post<UserRole>(this.config.baseIdentityApiUrl + '/ManageUser/DeleteRole', userrole);
    }

    getBooks(): Observable<Book[]> {
        return this.http.get<Book[]>(this.config.baseApiUrl + '/Book/List'); 
    }


    getBook(id): Observable<Book>
    {
        return this.http.get<Book>(this.config.baseApiUrl + '/Book/Details/' + id); 
    }

    selectBook(book) {
        this.selectedBook.next(book)
    }

    saveBook(book) 
    {
        return this.http.post<Book>(this.config.baseApiUrl + '/Book/Create', book);
    }

    updateBook(book) 
    {
        return this.http.post<Book>(this.config.baseApiUrl + '/Book/Edit/' + book.id, book);
    }

    loanBook(loan) 
    {
        return this.http.post<Loan>(this.config.baseLoanApiUrl + '/Loan/Create', loan);
    }

    returnBook(loan) 
    {
        return this.http.post<Loan>(this.config.baseLoanApiUrl + '/Loan/Return', loan);
    }

    getBookLoanStatus(id): Observable<BookLoanStatus>
    {
        return this.http.get<BookLoanStatus>(this.config.baseLoanApiUrl + '/Loan/GetBookLoanStatus/' + id);
    }   

    getGetTopBooksLoaned(): Observable<TopBooksLoanedReportViewModel[]>
    {
        return this.http.get<TopBooksLoanedReportViewModel[]>(this.config.baseLoanApiUrl + '/Loan/GetTopBooksLoaned');
    }

    getGetTopMemberLoans(): Observable<TopMemberLoansReport[]>
    {
        return this.http.get<TopMemberLoansReport[]>(this.config.baseLoanApiUrl + '/Loan/GetTopMemberLoans');
    }

    getLoansReportAllMembers(): Observable<AllMemberLoansReport[]>
    {
        return this.http.get<AllMemberLoansReport[]>(this.config.baseLoanApiUrl + '/Loan/GetLoanReportAllMembers');
    }
}