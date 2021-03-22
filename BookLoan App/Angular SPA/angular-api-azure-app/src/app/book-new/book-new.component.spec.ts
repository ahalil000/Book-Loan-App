import { HttpClient, HttpHandler } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
//import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { AuthService } from '../security/auth.service';
import { TestAuthService } from '../security/test-auth.service';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { TestApiService } from '../services/test-api.service';
import { BookNewComponent } from './book-new.component';

describe('BookNewComponent', () => {
  let component: BookNewComponent;
  let fixture: ComponentFixture<BookNewComponent>;

  beforeEach(async(() => {
    let component: BookNewComponent;
    let fixture: ComponentFixture<BookNewComponent>;
    let mockRouter = {
      navigate: jasmine.createSpy('navigate')
    };
    TestBed.configureTestingModule({
        declarations: [ BookNewComponent ],
        imports: [ MaterialModule, FormsModule, ReactiveFormsModule ],
        schemas: [CUSTOM_ELEMENTS_SCHEMA],
        providers: [
          //HttpClientTestingModule,
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
    fixture = TestBed.createComponent(BookNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
