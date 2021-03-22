import { HttpClient, HttpHandler } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CheckboxGroupComponent } from '../../components/checkbox-group/checkbox-group.component';
import { MaterialModule } from '../../material/material.module';
import { ApiService } from '../../services/api.service';
import { LookupService } from '../../services/lookup.service';
import { TestApiService } from '../../services/test-api.service';

import { AccountUserEditComponent } from './account-user-edit.component';

describe('AccountUserEditComponent', () => {
  let component: AccountUserEditComponent;
  let fixture: ComponentFixture<AccountUserEditComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountUserEditComponent, CheckboxGroupComponent ],
      imports: [ MaterialModule, FormsModule ],
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
    fixture = TestBed.createComponent(AccountUserEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
