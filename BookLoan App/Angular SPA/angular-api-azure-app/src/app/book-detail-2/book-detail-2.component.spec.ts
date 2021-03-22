import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { TestApiService } from '../services/test-api.service';

import { BookDetail2Component } from './book-detail-2.component';

describe('BookDetail2Component', () => {
  let component: BookDetail2Component;
  let fixture: ComponentFixture<BookDetail2Component>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookDetail2Component ],
      imports: [ MaterialModule, FormsModule, BrowserAnimationsModule ],
      providers: [
        { provide: ApiService, useClass: TestApiService },
        { provide: LookupService, useClass: LookupService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: undefined }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookDetail2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
