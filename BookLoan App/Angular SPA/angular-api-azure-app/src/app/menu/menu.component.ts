import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, interval } from 'rxjs';
import { combineLatest, Observable, Subscription } from 'rxjs';
import { NotificationList } from '../models/NotificationList';
import { NotificationConfiguration } from '../reference/notificationConfiguration';
import { AuthService } from '../security/auth.service';
import { NotificationService } from '../services/notification-service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  bIsLoggedIn: boolean = false;
  notificationList: NotificationList = new NotificationList();
  subscription: Subscription;
  menuSubscription: Subscription; 
  pollingSubscription: Subscription;
  menuNotification$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false); 
  recentLoansCount$: Observable<number>;
  recentReturnsCount$: Observable<number>; 
  numberOfLoans: number = 0;
  numberOfReturns: number = 0;

  constructor(        
    public router: Router,
    private notificationService: NotificationService, 
    private authService: AuthService) { }

  ngOnInit() {
    this.bIsLoggedIn = this.authService.currUserValue.token !== null; 

    console.log('menu notification.');

    this.notificationList.items = [];

    if (!this.bIsLoggedIn)
      return;

    this.recentLoansCount$ = this.notificationService.getRecentLoansCount();
    this.recentReturnsCount$ = this.notificationService.getRecentReturnsCount();

    this.pollingSubscription = interval(NotificationConfiguration.NOTIFICATION_INTERVAL).subscribe(val => 
      {
        console.log('Polling notification..');
        this.menuNotification$.next(true);
      });

    this.subscription = combineLatest(
      [ 
        this.recentLoansCount$,
        this.recentReturnsCount$,
        this.menuNotification$
      ]
    ).subscribe(([recentLoansCount, recentReturnsCount, menuNotification]) => 
    {
      if (!menuNotification)
        return;

      this.numberOfLoans = recentLoansCount;
      this.numberOfReturns =  recentReturnsCount;
      console.log('number of recent loans = ' + this.numberOfLoans);
      console.log('number of recent returns = ' + this.numberOfReturns);

      this.notificationList.items = [];

      if (this.numberOfLoans > 0)
        this.notificationList.items.push(
          { 
            notificationDescription: "There are " + this.numberOfLoans +  " recent loans",
            count: this.numberOfLoans
          })

      if (this.numberOfReturns > 0)
        this.notificationList.items.push(
          { 
            notificationDescription: "There are " + this.numberOfReturns +  " recent returns",
            count: this.numberOfReturns
          })
    });

  }

  ngOnDestroy()
  {
    if (!this.subscription)
      return;
    this.subscription.unsubscribe();
  }

  logout(): boolean {
    // logs out the user, then redirects him to Welcome View.
    if (!this.authService)
      return false;
    this.authService.logout().
        subscribe((result: any) => {
            console.log("app.logout() - result: " + JSON.stringify(result));
            console.log("app.logout(): successfully called HTTP POST..");
            this.router.navigate([""]);
        },
            (error: any) => console.log(error)
        );
    return false;
  }

  // route to admin users form.
  adminusers()
  {
    this.router.navigate(['accounts']);
  }
}
