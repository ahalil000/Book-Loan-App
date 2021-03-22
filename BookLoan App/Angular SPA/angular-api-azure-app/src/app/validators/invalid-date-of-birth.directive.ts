import { Directive, Input } from "@angular/core";
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from "@angular/forms";

@Directive({
  selector: '[appDateOfBirth][ngModel]',
  providers: [{provide: NG_VALIDATORS, useExisting: ForbiddenDateOfBirthDirective, multi: true}]
})

export class ForbiddenDateOfBirthDirective implements Validator {
  @Input('appDateOfBirth') forbiddenDateOfBirth: string;

  validate(control: AbstractControl): {[value: string]: any} | null {
    return this.forbiddenDateOfBirth ? 
    dateOfBirthValidator()(control): null;
    //dateOfBirthValidator(new Date(this.forbiddenDateOfBirth))(control): null;
  }
}

//export function dateOfBirthValidator(dobVal: Date): ValidatorFn {
export function dateOfBirthValidator(): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const isForbiddenDob = control.value ? control.value.getTime().valueOf() >= (new Date()).getTime().valueOf(): false;
      return isForbiddenDob ? {forbiddenDateOfBirth: {value: control.value}} : null;
    };
}

