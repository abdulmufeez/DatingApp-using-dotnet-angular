import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
// configuring spinner loading
export class BusySpinnerService {
  busyRequestSpinner = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  busy(){
    this.busyRequestSpinner++;
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(255,255,255,0)',
      color: "#333333"
    });
  }

  idle(){
    this.busyRequestSpinner--;
    if(this.busyRequestSpinner <= 0) {
      this.busyRequestSpinner = 0;
      this.spinnerService.hide();
    }
  }
}
