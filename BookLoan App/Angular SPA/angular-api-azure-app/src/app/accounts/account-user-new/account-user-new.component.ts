import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { LookupService } from '../../services/lookup.service';
import { UserAccount } from '../../models/UserAccount';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatSnackBar } from '@angular/material';
import { NewUserRole } from '../../models/NewUserRole';

@Component({
  selector: 'account-user-new',
  templateUrl: './account-user-new.component.html',
  styleUrls: ['./account-user-new.component.scss']
})
export class AccountUserNewComponent implements OnInit, AfterViewInit {

  heading = "New User Account";
  useraccount: UserAccount = new UserAccount();
  id: any;
  roles = [];
  selectedRole: string;
  dob: Date;
  submitted: boolean = false;

  @ViewChild('userForm', {static: false}) form: HTMLFormElement;
  @ViewChild('dob', {static: false}) dobControl: HTMLFormElement;
  
  userRegistered$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private api: ApiService, private lookupService: LookupService, 
      private activatedRoute: ActivatedRoute,
      private snackBar: MatSnackBar,  
      private router: Router) {}

  ngOnInit() {
    const defaultDate = new Date();
    this.useraccount.dob = defaultDate;

    if (!this.activatedRoute)
      return;

    this.lookupService.getRoles().subscribe(r => {
          this.roles = r.map(s => s.role);
          this.selectedRole = 'Member';
      });
  }

  ngAfterViewInit() {
    this.dob = this.useraccount.dob;
  }
  

  addEvent(event: MatDatepickerInputEvent<Date>) {      
    this.dob = event.value;
    this.useraccount.dob = this.dob;
  }

  eventCreateRecord() {
    this.submitted = true;

    if (!this.form.valid)
      return;

    this.api.registerUser(this.useraccount).subscribe(
      () => {
        console.log("account " + this.useraccount.userName + " updated");
        this.userRegistered$.next(true); 
        this.openSnackBar("Notification", "User created successfully!");
      },
      err => {
        console.log("account " + this.useraccount.userName + " not created. Error: " + err);
        var msg = err;
        if (err.includes('password') || err.includes('Password'))
          msg = "Password does not meet requirements.";
        if (err.includes('user name') || err.includes('User name'))
          msg = "User name does not meet requirements.";
        this.openSnackBar("Data Entry", msg);
      }
    );

    this.userRegistered$.subscribe(u => {
      if (u) {
        //let userrole: UserRole = { email: this.useraccount.email, role: this.selectedRole, userroles: [] };
        let userrole: NewUserRole = { loginName: this.useraccount.email, selectedRole: this.selectedRole };            
        this.api.addusertorole(userrole).subscribe(
          () => {
            console.log("role " + this.selectedRole + ' added to user ' + this.useraccount.userName + " account");
            this.userRegistered$.next(false); 
            this.openSnackBar("Notification", "User created successfully!");
            this.submitted = false;
          },
          err => {
            console.log("role " + this.selectedRole + ' cannot be added to user ' + this.useraccount.userName + " account. Error: " + err);
            var msg = err;
            //if (err.includes('password') || err.includes('Password'))
            //  msg = "Password does not meet requirements.";
            //if (err.includes('user name') || err.includes('User name'))
            //  msg = "User name does not meet requirements.";
            this.openSnackBar("Data Entry", msg);
            this.router.navigate(['account-users']);            
          }
        );
      }
    })
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 3000,
      panelClass: ['blue-snackbar']      
    });
  }

  eventBack()
  {
    this.router.navigate(['accounts/account-users']);     
  }
}
