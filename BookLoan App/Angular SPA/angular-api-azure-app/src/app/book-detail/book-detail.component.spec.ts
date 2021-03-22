import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookDetailComponent } from './book-detail.component';
import { MaterialModule } from '../material/material.module';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { TestApiService } from '../services/test-api.service';
import { LookupService } from '../services/lookup.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('BookDetailComponent', () => {
  let component: BookDetailComponent;
  let fixture: ComponentFixture<BookDetailComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookDetailComponent ],
      imports: [ MaterialModule, FormsModule, BrowserAnimationsModule ],
      providers: [
        { provide: ApiService, useClass: TestApiService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: undefined },
        { provide: LookupService, useClass: LookupService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
