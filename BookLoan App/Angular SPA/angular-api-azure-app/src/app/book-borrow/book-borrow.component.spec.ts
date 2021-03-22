import { HttpClient, HttpHandler } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { AuthService } from '../security/auth.service';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { TestApiService } from '../services/test-api.service';

import { BookBorrowComponent } from './book-borrow.component';

describe('BookBorrowComponent', () => {
  let component: BookBorrowComponent;
  let fixture: ComponentFixture<BookBorrowComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookBorrowComponent ],
      imports: [ MaterialModule, FormsModule ],
      providers: [
        HttpClientTestingModule,
        HttpClient, 
        HttpHandler,
        { provide: AuthService, useClass: AuthService },
        { provide: ApiService, useClass: TestApiService }, 
        { provide: LookupService, useClass: LookupService },
        { provide: Router, useValue: mockRouter }, 
        { provide: ActivatedRoute, useValue: undefined }
      ]      

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookBorrowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
