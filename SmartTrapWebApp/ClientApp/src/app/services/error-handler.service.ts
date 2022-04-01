import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  constructor(private messageService:MessageService) { }
  public onError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
        // A client-side or network error occurred. Handle it accordingly.
        //this.messageService.add('エラー:'+ error.error.message);
        console.error('エラー:', error.error.message);
    } else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong,
        //this.messageService.add('エラーコード:'+ error.status);
        //this.messageService.add('エラーメッセージ:'+ error.error);
        console.error(
            `エラーコード: ${error.status}, ` +
            `エラーメッセージ: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
        'サーバエラーです　しばらくしてから再度実行してください');
  };
}
