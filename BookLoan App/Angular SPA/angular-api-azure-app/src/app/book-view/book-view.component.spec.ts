import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { ApiService } from '../services/api.service';
import { LookupService } from '../services/lookup.service';
import { TestApiService } from '../services/test-api.service';

import { BookViewComponent } from './book-view.component';

describe('BookViewComponent', () => {
  let component: BookViewComponent;
  let fixture: ComponentFixture<BookViewComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookViewComponent ],
      imports: [ MaterialModule, FormsModule ],
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
    fixture = TestBed.createComponent(BookViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
