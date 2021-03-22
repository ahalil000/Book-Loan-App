import { Component } from '@angular/core'
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { delay } from 'rxjs/operators';

@Component({
    selector: 'account-users',
    templateUrl: './account-users.component.html'
})
export class AccountUsersComponent {

    heading  = "User Account List";

    user = {}
    users
    
    hasLoaded$ = new BehaviorSubject<boolean>(false);
    mode: ProgressSpinnerMode = 'indeterminate';
    value = 50;

    apidelay = 0;

    constructor(private api: ApiService, private router: Router) {
        console.log("account users component created");
    }

    ngOnInit() {
        this.api.getUsers().pipe(delay(this.apidelay)).subscribe(res => {
            this.hasLoaded$.next(true);
            this.users = res
            console.log("read users from API");
        })
    }

    ngOnDestroy()
    {
    }

    selectUserById(id) {
        this.router.navigate(['accounts/account-user-view', id]);     
    }

    eventNewRecord(event)
    {
        this.router.navigate(['accounts/account-user-new']);     
        console.log("new user account data entry form being opened.");
    }

}