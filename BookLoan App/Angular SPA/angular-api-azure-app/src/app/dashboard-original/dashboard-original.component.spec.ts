import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { ApiService } from '../services/api.service';
import { TestApiService } from '../services/test-api.service';

import { DashboardOriginalComponent } from './dashboard-original.component';

describe('DashboardComponent', () => {
  let component: DashboardOriginalComponent;
  let fixture: ComponentFixture<DashboardOriginalComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardOriginalComponent ],
      imports: [ MaterialModule ], 
      providers: [
        HttpClientTestingModule,
        { provide: Router, useValue: mockRouter },
        { provide: ApiService, useClass: TestApiService }, 
        { provide: ActivatedRoute, useValue: undefined }
      ]      
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardOriginalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
