import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuComponent } from './menu.component';
import { MaterialModule } from '../material/material.module';
import { Router } from '@angular/router';
import { AuthService } from '../security/auth.service';
import { TestAuthService } from '../security/test-auth.service';
import { NotificationService } from '../services/notification-service';
import { HttpClientTestingModule  } from '@angular/common/http/testing';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { TestNotificationService } from '../services/test/test-notification-service';

describe('MenuComponent', () => {
  let component: MenuComponent;
  let fixture: ComponentFixture<MenuComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuComponent ],
      imports: [ MaterialModule ],
      providers: [
        HttpClientTestingModule,
        HttpClient, 
        HttpHandler,
        { provide: AuthService, useClass: TestAuthService }, 
        { provide: NotificationService, useClass: TestNotificationService },
        { provide: Router, useValue: mockRouter }
      ]      

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
});
