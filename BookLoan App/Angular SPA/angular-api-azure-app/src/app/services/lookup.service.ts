import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs';
import { Genre } from 'src/app/models/Genre';
import { Role } from '../models/Role';

@Injectable()
export class LookupService {

    //genres: Observable<Genre[]> = new Observable<Genre[]>(); 

    constructor() {}

    getGenres(): Observable<Genre[]> {
        let arrGenres: Genre[] = [];
        arrGenres.push({id: 1, genre: "Childrens"});
        arrGenres.push({id: 2, genre: "Family sage"});
        arrGenres.push({id: 3, genre: "Fantasy"});
        arrGenres.push({id: 4, genre: "Fiction"});
        arrGenres.push({id: 5, genre: "Folklore"});
        arrGenres.push({id: 6, genre: "Horror"});
        arrGenres.push({id: 7, genre: "Mystery"});
        arrGenres.push({id: 8, genre: "Non-fiction"});
        arrGenres.push({id: 9, genre: "Sci-fi"}); 
        return of(arrGenres); 
    }

    getRoles(): Observable<Role[]> {
        let arrRoles: Role[] = [];
        arrRoles.push({id: 1, role: "Member"});
        arrRoles.push({id: 2, role: "Manager"});
        arrRoles.push({id: 3, role: "Admin"});
        return of(arrRoles); 
    }
}
