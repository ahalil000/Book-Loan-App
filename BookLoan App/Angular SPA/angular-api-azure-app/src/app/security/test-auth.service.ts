import { Injectable } from '@angular/core'
import { User } from '../models/User';
import { BehaviorSubject, of } from 'rxjs';
import { AuthService } from './auth.service';
//import { map, catchError, switchMap } from 'rxjs/operators'; 
//import { stringToKeyValue } from '@angular/flex-layout/extended/typings/style/style-transforms';

@Injectable()
export class TestAuthService extends AuthService {
 
    constructor()
    {
        super(null, null, null);
        var user: User = {
            username: "test",
            token: ""  
        };        
        this.currUser = of(user); 
        this.currUserSubject = new BehaviorSubject<User>(user);
    }

    public get currentUserValue(): User {
        return this.currUserSubject.value;
    }

    register(credentials){
    }    
    
    login(userName: string, password: string) {
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isLoggedIn(): boolean {
        let val = localStorage.getItem('token') != null;    
        return val;
    }

    public get currUserValue(): User {
        return this.currUserSubject.value;
    }

    public get currLoggedInUserValue(): string {
        return this.currUserSubject.value.username.replace('"', '').replace('"', '');
    }

    authenticate(res: User) {
    }

    logout(): any {
    }

}