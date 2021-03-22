import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest } from 'rxjs';
import { LookupService } from '../../services/lookup.service';
import { UserAccount } from '../../models/UserAccount';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatSnackBar } from '@angular/material';
import { SelectedRole } from '../../models/SelectedRoles';
import { NewUserRole } from '../../models/NewUserRole';


@Component({
  selector: 'account-user-edit',
  templateUrl: './account-user-edit.component.html',
  styleUrls: ['./account-user-edit.component.scss']
})
export class AccountUserEditComponent implements OnInit {

  heading = "Edit User Account";
  
  useraccount: UserAccount = new UserAccount();
  id: any;

  selectedRole: string;

  dob: Date;
  startDate = new Date();

  userRegistered$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  selectedRoles$: BehaviorSubject<string[]> = new BehaviorSubject<string[]>([]);
  availableRoles$: BehaviorSubject<string[]> = new BehaviorSubject<string[]>([]);
  userSelectedRoles: string[] = [];
  userUnSelectedRoles: string[] = [];

  constructor(private api: ApiService, private lookupService: LookupService, 
      private activatedRoute: ActivatedRoute,
      private snackBar: MatSnackBar,  
      private router: Router) {}

  ngOnInit() {
    if (!this.activatedRoute)
      return;
    this.id = this.activatedRoute.snapshot.params["id"];

    let roles: string[] = [];

    this.lookupService.getRoles().subscribe(r =>  
      {
          roles = r.map(r => r.role);
          this.selectedRole = 'Member';
          this.availableRoles$.next(roles);
      });

    const getUser$ = this.api.getUser(this.id);
    const getUserRole$ = this.api.getUserRoleInfoById(this.id);

    combineLatest(
      [
        getUser$,
        getUserRole$
      ]
    ).subscribe(([curruser, userrole]) => {
          if (!curruser || !userrole)
            return;
        console.log("user name = " + curruser.userName);        

        this.useraccount = curruser;
        this.dob = curruser.dob;
        this.startDate = this.dob;        

        let userRoles: SelectedRole[] = [];

        roles.forEach(r =>
          {
            userRoles.push({ role: r, isSelected: false });
          });
        if (!userrole['userRoles'])
          return;
        let selectedUserRoles: string[] = [];

        userrole['userRoles'].forEach(r => {
            const item = userRoles.find(s => s.role === r);
            if (item) {
              item.isSelected = true;
              selectedUserRoles.push(r);
            }
          });
        this.userSelectedRoles = selectedUserRoles;
        this.userUnSelectedRoles = 
          userRoles.filter(i => i.isSelected === false).map(j => j.role);  
        this.selectedRoles$.next(selectedUserRoles);
      }
    );
  }

  addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    let selectedDate = new Date();   
    selectedDate.setDate(event.value.getDate());
    selectedDate.setMonth(event.value.getMonth());
    selectedDate.setFullYear(event.value.getFullYear());
    this.dob = selectedDate;
    this.useraccount.dob = this.dob;
    this.startDate = selectedDate;
  }

  eventUpdateRecord(event) {
    this.api.updateUser(this.useraccount).subscribe(
      res => {
        console.log("account " + this.useraccount.userName + " updated");
        this.userRegistered$.next(true); 
        this.openSnackBar("Notification", "User updated successfully!");
      },
      err => {
        console.log("account " + this.useraccount.userName + " not updated. Error: " + err);
        var msg = err;
        if (err.includes('password') || err.includes('Password'))
          msg = "Password does not meet requirements.";
        if (err.includes('user name') || err.includes('User name'))
          msg = "User name does not meet requirements.";
        this.userRegistered$.next(false); 
        this.openSnackBar("Data Entry", msg);
      }
    );

    this.userRegistered$.subscribe(u => {
      if (u) {
        this.userSelectedRoles.forEach(r => {
            let userrole: NewUserRole = { loginName: this.useraccount.email, selectedRole: r };            
            this.api.addusertorole(userrole).subscribe(
              res => {
                console.log("role " + r + ' added to user ' + this.useraccount.userName + " account");
                this.userRegistered$.next(false); 
                this.openSnackBar("Notification", "User role added successfully!");
              },
              err => 
              {
                console.log("role " + r + ' cannot be added to user ' + this.useraccount.userName + " account. Error: " + err);
                this.userRegistered$.next(false); 
                var msg = err;
                this.openSnackBar("Data Entry", msg);
              }
            );
        });

        this.userUnSelectedRoles.forEach(r => 
        {
            let userrole: NewUserRole = { loginName: this.useraccount.email, selectedRole: r };            
            this.api.deleteuserfromrole(userrole).subscribe(
              res => {
                console.log("role " + r + ' removed from user ' + this.useraccount.userName + " account");
                this.userRegistered$.next(false); 
                this.openSnackBar("Notification", "User role removed successfully!");
              },
              err => 
              {
                console.log("role " + r + ' cannot be removed from user ' + this.useraccount.userName + " account. Error: " + err);
                this.userRegistered$.next(false); 
                var msg = err;
                this.openSnackBar("Data Entry", msg);
              }
            );
        });
      }
    })
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 3000,
      panelClass: ['blue-snackbar']      
    });
  }

  eventBack(event)
  {
    this.router.navigate(['accounts/account-users']);     
  }

  updateChecked(value: string[])
  {
      this.userSelectedRoles = value;
  }

  updateUnChecked(value: string[])
  {
      this.userUnSelectedRoles = value;
  }

}
