import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { delay, finalize, Observable } from 'rxjs';
import { BusySpinnerService } from '../_services/busy-spinner.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busySpinnerService: BusySpinnerService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // when sending request to server start spinner
    this.busySpinnerService.busy();
    return next.handle(request).pipe(
      delay(500),
      // when request complete hide spinner
      finalize(() => {
        this.busySpinnerService.idle();
      })
    );
  }
}
