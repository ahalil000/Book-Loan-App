import { HttpClient, HttpHandler } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { AuthService } from '../security/auth.service';
import { TestAuthService } from '../security/test-auth.service';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { TestApiService } from '../services/test-api.service';

import { BookDetail3Component } from './book-detail-3.component';

describe('BookDetail3Component', () => {
  let component: BookDetail3Component;
  let fixture: ComponentFixture<BookDetail3Component>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookDetail3Component ],
      imports: [ MaterialModule, FormsModule ],
      providers: [
        HttpClientTestingModule,
        HttpClient, 
        HttpHandler,
        { provide: ApiService, useClass: TestApiService }, 
        { provide: LookupService, useClass: LookupService },
        { provide: Router, useValue: mockRouter }, 
        { provide: AuthService, useClass: TestAuthService },
        { provide: ActivatedRoute, useValue: undefined }
      ]      

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookDetail3Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
