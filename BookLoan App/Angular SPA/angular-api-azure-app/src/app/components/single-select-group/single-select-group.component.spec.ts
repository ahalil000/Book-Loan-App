import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatChipsModule } from '@angular/material';

import { SingleSelectGroupComponent } from './single-select-group.component';

describe('SingleSelectGroupComponent', () => {
  let component: SingleSelectGroupComponent;
  let fixture: ComponentFixture<SingleSelectGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SingleSelectGroupComponent ],
      imports: [ MatChipsModule ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleSelectGroupComponent);
    component = fixture.debugElement.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set and get available chip items', (() => {
    const items = ['apples', 'oranges', 'bananas'];

    component.availableItems = items;
    const result = component.availableItems;

    expect(result).toEqual(items);
  }));

  it('should set and get selected chip item', (() => {
    const items = ['apples'];

    component.selectedItem = items[0];
    const result = component.selectedItem;

    expect(result).toEqual(items[0]);
  }));

  it('should emit event', () => {
    const items = ['apples', 'oranges', 'bananas'];
    component.availableItems = items;
    
    const pressSpy = spyOn(component['selectionChange'], 'emit');

    const event = {
        data: '0'
    };

    fixture.detectChanges();

    component.onSelect(event);

    expect<any>(pressSpy).toHaveBeenCalledWith(event);
    expect(pressSpy).toHaveBeenCalledTimes(1);
  });


  it('empty list gives no selected chip', () => {  
    component.availableItems = [];
    component.selectedItem = "";
    fixture.detectChanges();
    
    component.selectNthItem(0);
    const result = component.selectedItem;

    expect(result).toBeUndefined();
  });

  it('selected chip is valid', () => {  
    const items = ['apples', 'oranges', 'bananas'];

    component.availableItems = items;
    component.selectedItem = items[2];
    fixture.detectChanges();
    
    component.selectNthItem(1);
    const result = component.selectedItem;

    expect(result).toBeTruthy();
  });

  it('selected chip is now first item', () => {  
    const items = ['apples', 'oranges', 'bananas'];

    component.availableItems = items;
    component.selectedItem = items[2];
    fixture.detectChanges();
    
    component.selectNthItem(0);
    const result = component.selectedItem;

    expect(result).toEqual(items[2]);
  });
});  

