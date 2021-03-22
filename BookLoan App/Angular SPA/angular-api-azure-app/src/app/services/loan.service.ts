import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs';
import { Genre } from 'src/app/models/Genre';
import { ApiService } from './api.service';

@Injectable()
export class LoanService {

    constructor(private api: ApiService) {}

    getBookLoanReport() //: Observable<any[]> {
    {    
        this.api.getBooks().pipe().subscribe(res => {
            res.forEach(itm => 
                {
                    let book = {...itm};


                })

            console.log("read books from API");
        })
    }

    getBookLoans(id: number) //: Loan[]
    {


    }

}
