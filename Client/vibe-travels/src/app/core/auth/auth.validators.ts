import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class AuthValidators {
  static email(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      if (value.length > 100) {
        return { emailTooLong: { maxLength: 100, actualLength: value.length } };
      }

      const emailRegex = /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/;
      const normalizedValue = value.toLowerCase();

      if (!emailRegex.test(normalizedValue)) {
        return { invalidEmailFormat: true };
      }

      return null;
    };
  }

  static password(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      if (value.length < 8) {
        return { passwordTooShort: { minLength: 8, actualLength: value.length } };
      }

      if (value.length > 100) {
        return { passwordTooLong: { maxLength: 100, actualLength: value.length } };
      }

      if (!/\d/.test(value)) {
        return { passwordMissingDigit: true };
      }

      if (!/[A-Z]/.test(value)) {
        return { passwordMissingUppercase: true };
      }

      return null;
    };
  }
}
