import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Router } from '@angular/router'
import { User } from '../models/User';
import { Observable, BehaviorSubject, throwError, Subject } from 'rxjs';
import { ApiService } from '../services/api.service';
import { of } from 'rxjs';

@Injectable()
export class AuthService {

    public currUserSubject: BehaviorSubject<User>;
    public currUser: Observable<User>;

    public currUserRoles: BehaviorSubject<string[]>;
    public userRoles: Observable<string[]>;

    public loginResponse: Subject<string> = new Subject<string>();

    public get isUserAdmin() 
    {
        return this.isUserInAdminRole();
    } 
  
    constructor(private http: HttpClient, private apiService: ApiService, private router: Router) {
        var sUser: string = localStorage.getItem('currentUser');
        var sToken: string = localStorage.getItem('token');

        var user: User = {
            username: sUser,
            token: sToken  
        };        
        this.currUserSubject = new BehaviorSubject<User>(user);
        this.currUser = this.currUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currUserSubject.value;
    }

    register(credentials){
        this.http.post<any>('http://localhost/BookLoan.Identity.API/api/Users/Authenticate', credentials).
            subscribe(res =>{
                this.authenticate(res)
            }
        )
    }    
    
    login(userName: string, password: string) {
        var config = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            })
        };        

        this.http.post<any>('http://localhost/BookLoan.Identity.API/api/Users/Authenticate', { "UserName": userName, "Password": password }, config).
            subscribe(
                res => { this.authenticate(res) 
                
                    this.apiService.getUserRoles(this.currLoggedInUserValue).subscribe(r => 
                        {
                            this.authenticateRole(r);
                            console.log("getUserRoles(): user " + 
                                this.currLoggedInUserValue + " is member of " + (r ? 0 : r.length) + " roles.");                                
                            this.loginResponse.next("login success");
                        });
                },
                error => { 
                    console.error('There was an error!', error); 
                    this.loginResponse.next(error);
                },
                ()=> {  
                }
            );
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isLoggedIn(): boolean {
        let val = localStorage.getItem('token') != null;    
        return val;
    }

    isUserInAdminRole(): boolean
    {
        if (!localStorage.getItem('role'))
            return false;
        return localStorage.getItem('role').includes('Admin');
    }

    public get currUserValue(): User {
        return this.currUserSubject.value;
    }

    public get currLoggedInUserValue(): string {
        if (!this.currUserSubject.value.username)
            return ""; 
        return this.currUserSubject.value.username.replace('"', '').replace('"', '');
    }


    authenticate(res: User) {
        localStorage.setItem('token', res.token)
        localStorage.setItem('currentUser', JSON.stringify(res.username));
        this.currUserSubject.next(res);
        this.router.navigate(['/'])
    }

    authenticateRole(roles: string[]) {
        localStorage.setItem('role', roles.toString());
    }


    logout(): Observable<boolean> {
        console.log("logout(): logging out current user..");
        console.log("logout(): removing auth local storage..");
        localStorage.removeItem('token');
        localStorage.removeItem('currentUser');
        return of(true);
    }
}