import { Input, QueryList, ViewChild } from '@angular/core';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatChip, MatChipSelectionChange } from '@angular/material';

@Component({
  selector: 'app-single-select-group',
  templateUrl: './single-select-group.component.html',
  styleUrls: ['./single-select-group.component.scss']
})
export class SingleSelectGroupComponent implements OnInit {

  @ViewChild("chiplist", {static: false}) chipList = new QueryList<MatChip>();
  @Output() selectionChange: EventEmitter<MatChipSelectionChange> = new EventEmitter();

  @Input('selectedItem')
  set selectedItem(value: string)
  {
      if (!value)
        return;
      this._selectedItem = value;
  }
  get selectedItem()
  {
      return this._selectedItem;
  }

  public _selectedItem: string = "";
 
  private _availableItems: string[] = [];
  @Input('availableItems') 
  set availableItems (value: string[])
  {
      if (!value)
        return;      
      this._availableItems = value;
      this._selectedItem = value[0];
  }
  
  get availableItems()
  {
      return this._availableItems;
  }

  get selectableItems()
  {
      return this._availableItems.filter(i => i.valueOf() !== this.selectedItem);
  }

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit()
  {
  }

  onSelect(value: any)
  {
    this.selectionChange.emit(value);
    this._selectedItem = value;
    const firstChip = this.chipList['chips'].first;
    firstChip.select();
    firstChip.focus();
  }

  selectNthItem(item: number)
  {
    const firstChip = this.chipList['chips']._results[item];
    if (!firstChip)
      return;
    firstChip.select();
    firstChip.focus();
    this.selectionChange.emit(firstChip.value.trim());
    this.selectedItem = firstChip.value.trim();
  }

}
