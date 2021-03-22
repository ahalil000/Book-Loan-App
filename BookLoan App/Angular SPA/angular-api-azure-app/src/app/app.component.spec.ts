//import { HttpClient, HttpHandler } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { MaterialModule } from './material/material.module';
import { MenuComponent } from './menu/menu.component';
import { AuthService } from './security/auth.service';
import { TestAuthService } from './security/test-auth.service';
import { NotificationService } from './services/notification-service';
import { TestNotificationService } from './services/test/test-notification-service';

describe('AppComponent', () => {
  //const spy = jasmine.createSpyObj('NotificationService', ['getRecentLoansCount', 'GetRecentReturnsCount']);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MaterialModule
      ],
      declarations: [
        AppComponent, MenuComponent
      ],
      providers: [
        //HttpClientTestingModule,
        //HttpClient, 
        //HttpHandler,
        { provide: NotificationService, useClass: TestNotificationService }, 
        { provide: AuthService, useClass: TestAuthService }
      ]      

    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Angular Demo App'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    fixture.detectChanges();
    expect(app.title).toEqual('Angular Demo App');
  });

  it('should render title in a h1 tag', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('h2').textContent).toContain('Angular Demo App');
  });
});
