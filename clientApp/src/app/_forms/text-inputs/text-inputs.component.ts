import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RegisterFormComponent } from 'src/app/register-form/register-form.component';

@Component({
  selector: 'app-text-inputs',
  templateUrl: './text-inputs.component.html',
  styles: [
  ]  
})
export class TextInputsComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() type = 'text';

  constructor(@Self() public ngControl: NgControl) { 
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }
}
