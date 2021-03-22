import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Angular Demo App';
  //routestr: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) 
  {}

  ngOnInit()
  {
    //this.route.url.subscribe(url => {
    //  this.routestr = url[0].path;
    //});
  }

}
