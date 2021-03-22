import { Injectable } from '@angular/core';
import { Router, Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { LookupService } from '../services/lookup.service';
import { ApiService } from '../services/api.service';

@Injectable()
export class BookDetailComponentPageResolver implements Resolve<{ any }> {

    constructor(private router: Router,
        private lookupSvc: LookupService,
        private apiSvc: ApiService
    ) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        let id = route.paramMap.get("id");
        return forkJoin(
            [
              this.apiSvc.getBook(id),
              this.lookupSvc.getGenres()
            ]
        ).pipe(map(
            resp => 
            {             
              return {
                id: id,
                title: resp[0].title,
                author: resp[0].author,
                yearPublished: resp[0].yearPublished,
                genre: resp[0].genre,
                edition: resp[0].edition, 
                isbn: resp[0].isbn,
                location: resp[0].location,
                genres: resp[1]                
              }
            }
        ));
    }    
}
