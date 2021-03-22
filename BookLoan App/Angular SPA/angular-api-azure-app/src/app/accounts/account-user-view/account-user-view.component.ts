import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { LookupService } from '../../services/lookup.service';
import { UserAccount } from '../../models/UserAccount';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatSnackBar } from '@angular/material';
import { LocalDateFormatter } from '../../utilities/localDateFormatter';


@Component({
  selector: 'account-user-view',
  templateUrl: './account-user-view.component.html',
  styleUrls: ['./account-user-view.component.scss']
})
export class AccountUserViewComponent implements OnInit {

  heading = "User Account View";
  
  useraccount: UserAccount = new UserAccount();
  id: any;

  roles = [];
  userRoles: string;
  dobString: string;
  hideDate: boolean = false;


  constructor(private api: ApiService, private lookupService: LookupService, 
      private activatedRoute: ActivatedRoute,
      private snackBar: MatSnackBar,  
      private router: Router) {}

  ngOnInit() {
    if (!this.activatedRoute)
      return;
    this.id = this.activatedRoute.snapshot.params["id"];

    //console.log("BookDetailComponent instantiated with the following id : " + this.id);

    this.lookupService.getRoles().subscribe(r =>  
      {
          this.roles = r.map(r => r.role);
          this.userRoles = 'Member';
        });

    const getUser$ = this.api.getUser(this.id);
    const getUserRoles$ = this.api.getUserRoleInfoById(this.id);

    combineLatest(
      [
        getUser$,
        getUserRoles$
      ]
    ).subscribe(([curruser, userroles]) => {
          if (!curruser || !userroles)
            return;
        console.log("user name = " + curruser.userName);        

        this.userRoles = userroles['userRoles'] && userroles['userRoles'].length > 0  ? userroles['userRoles'].toString() : "Not Defined";
        this.useraccount = curruser;  
        const dob = new Date(curruser.dob);
        this.dobString = LocalDateFormatter.showDateAsDDMMYYYY(dob);
        this.hideDate = dob.getFullYear() === 1;
      }
    );
  }

  addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    this.useraccount.dob = event.value;
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000,
      panelClass: ['blue-snackbar']      
    });
  }

  eventBack(event)
  {
    this.router.navigate(['accounts/account-users']);     
  }

  eventEditRecord(id: number)
  {
    this.router.navigate(['accounts/account-user-edit', id]);     
    console.log("edit user account data entry form being opened.");
  }
}
