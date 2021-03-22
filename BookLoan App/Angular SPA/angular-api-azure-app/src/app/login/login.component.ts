import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../security/auth.service';
//import { MaterialModule } from '../material/material.module';
import { MatSnackBar } from '@angular/material';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {

    heading = "Login Form";

    form
    
    constructor(private auth: AuthService, private snackBar: MatSnackBar, private fb: FormBuilder) {
        this.form = fb.group({
            email: ['', Validators.required],
            password: ['', Validators.required]
        })
    }

    login(username: string, password: string): void {
        this.auth.login(username, password);
        this.auth.loginResponse.subscribe(r => 
        {
            this.openSnackBar("Login", r);
        }); 
    }

    openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
          duration: 3000,
          panelClass: ['blue-snackbar']      
        });
      }    

}