import { Directive, Input } from "@angular/core";
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from "@angular/forms";

@Directive({
  selector: '[appPublishedYear][ngModel]',
  providers: [{provide: NG_VALIDATORS, useExisting: PublishedYearDirective, multi: true}]
})

export class PublishedYearDirective implements Validator {
  @Input('appPublishedYear') publishedYear: string;

  validate(control: AbstractControl): {[value: string]: any} | null {
    return this.publishedYear ? 
    publishedYearValidator()(control): null;
  }
}

export function publishedYearValidator(): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const currDate = new Date();
      const isValidPubYear = control.value && control.value.length > 0 ? 
        control.value < currDate.getFullYear() && 
        control.value >= 1000: false;
      return !isValidPubYear ? {invalidPublishedYear: {value: control.value}} : null;
    };
}