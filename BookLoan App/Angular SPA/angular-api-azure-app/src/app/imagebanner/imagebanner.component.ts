import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-imagebanner',
  templateUrl: './imagebanner.component.html',
  styleUrls: ['./imagebanner.component.scss']
})
export class ImagebannerComponent implements OnInit {

  @Input() set imagetitle(value: string)
  {
    this._imagetitle = value;
  }
  get imagetitle() 
  { 
    return this._imagetitle; 
  }
  _imagetitle: string; 
  routestr: string;

  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    if (!this.route)
      return;
    this.route.url.subscribe(url => {
      this.routestr = url[0].path;
    });
  }

}
