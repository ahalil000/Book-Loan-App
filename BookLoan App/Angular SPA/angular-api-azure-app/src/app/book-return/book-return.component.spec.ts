import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { AuthService } from '../security/auth.service';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { TestApiService } from '../services/test-api.service';

import { BookReturnComponent } from './book-return.component';

describe('BookReturnComponent', () => {
  let component: BookReturnComponent;
  let fixture: ComponentFixture<BookReturnComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookReturnComponent ],
      imports: [ MaterialModule, FormsModule ],
      providers: [
        HttpClientTestingModule,
        //HttpClient,
        { provide: ApiService, useClass: TestApiService }, 
        { provide: LookupService, useClass: LookupService },
        { provide: Router, useValue: mockRouter }, 
        { provide: AuthService, useClass: TestApiService },     
        { provide: ActivatedRoute, useValue: undefined }
      ]      

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookReturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
