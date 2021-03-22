import { Injectable } from '@angular/core'
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize, map } from 'rxjs/operators';

import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private authenticationService: AuthService,
        private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with basic auth credentials if available
        const currentUser = this.authenticationService.currentUserValue;
        if (currentUser && currentUser.token) {
            request = request.clone({
                setHeaders: { 
                    Authorization: `Bearer ${currentUser.token}`
                }
            }); 
        }
        //return next.handle(request);
        return next.handle(request).pipe(map(event => {
            return event;
        }), catchError(err => {
                if (err.status === 401) {
                    //this.authenticationService.logout().subscribe(s => {});
                    // not authorized so redirect to login page with the return url
                    this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
                }
                if (err.status === 400)
                {
                    const error = err.error ? err.error.message : err.statusText;
                    var newError = error;
                    if (!error && (err.error.errors.length > 0))
                        newError = err.error.errors[0].errorDescription; 
                    return throwError(newError);
                }
            }),
            finalize(() => {})
        )}
}    
        // constructor() {}

    // intercept(req, next) {

    //     var token = localStorage.getItem('token')

    //     var authRequest = req.clone({
    //         headers: req.headers.set('Authorization', `Bearer ${token}`)
    //     })
    //     return next.handle(authRequest)
    // }
    
//}

