import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { ImagebannerComponent } from './imagebanner.component';

describe('ImagebannerComponent', () => {
  let component: ImagebannerComponent;
  let fixture: ComponentFixture<ImagebannerComponent>;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };
  let mockActiveRoute = {
    navigate: jasmine.createSpy('navigate'),
    url: jasmine.createSpy('url')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImagebannerComponent ],
      providers: [
        HttpClientTestingModule,
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: undefined }
      ]      
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImagebannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
