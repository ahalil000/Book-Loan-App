import { EventEmitter, Component, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-checkbox-group',
  templateUrl: './checkbox-group.component.html',
  styleUrls: ['./checkbox-group.component.scss']
})
export class CheckboxGroupComponent implements OnInit {

  @Output() userSelectedItems: EventEmitter<string[]> = new EventEmitter();
  @Output() unSelectedUserItems: EventEmitter<string[]> = new EventEmitter();

  @Input('selectedItems')
  set selectedItems(value: string[])
  {
      if (!value)
        return;
      value.forEach(s => 
      {
        const item = this.selectedItemsData.find(i => i.description === s);
        if (item)
          item.isSelected = true;
      });
  }
  get selectedItems()
  {
      return this.selectedItemsData.map(v => v.description);
  }

  private selectedItemsData: SelectedItem[] = [];
 
  private _availableItems: string[] = [];
  @Input('availableItems') 
  set availableItems (value: string[])
  {
      if (!value)
        return;
      
      this._availableItems = value;

      value.forEach(i =>
      {
        this.selectedItemsData.push({ description: i, isSelected: false });
      });
  }
  get availableItems()
  {
      return this._availableItems;
  }

  constructor() { }

  ngOnInit() {
  }

  updateChecked(value: any)
  {
    this.userSelectedItems.emit(
      this.selectedItemsData.filter(i => i.isSelected === true).map(j => j.description)
    );
    this.unSelectedUserItems.emit(
      this.selectedItemsData.filter(i => i.isSelected === false).map(j => j.description)
    );
  }

}

export class SelectedItem
{
    public description: string;
    public isSelected: boolean;
}