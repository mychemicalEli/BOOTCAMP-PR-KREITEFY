import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const passwordStrengthPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_-])[A-Za-z\d@$!%*?&_-]{8,}$/;
    if (control.value && !passwordStrengthPattern.test(control.value)) {
      return { passwordStrength: 'La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial.' };
    }
    return null;
  };
}
