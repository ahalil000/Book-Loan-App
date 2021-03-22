import { HttpClient, HttpHandler } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../../material/material.module';
import { ApiService } from '../../services/api.service';
import { LookupService } from '../../services/lookup.service';
import { TestApiService } from '../../services/test-api.service';
import { ForbiddenDateOfBirthDirective } from '../../validators/invalid-date-of-birth.directive';
import { AccountUserNewComponent } from './account-user-new.component';

describe('AccountUserNewComponent', () => {
  let component: AccountUserNewComponent;
  let fixture: ComponentFixture<AccountUserNewComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountUserNewComponent, ForbiddenDateOfBirthDirective ],
      imports: [ MaterialModule, FormsModule, ReactiveFormsModule ],
      providers: [
        HttpClient, 
        HttpHandler,
        { provide: ApiService, useClass: TestApiService }, 
        { provide: LookupService, useClass: LookupService },
        { provide: Router, useValue: mockRouter }, 
        { provide: ActivatedRoute, useValue: undefined }
      ]      

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountUserNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
