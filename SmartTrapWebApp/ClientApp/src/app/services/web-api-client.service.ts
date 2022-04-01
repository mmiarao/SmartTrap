import { Injectable, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from './error-handler.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class WebApiClientService {

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        private errorHandler: ErrorHandlerService    

    ) { }

    public sendLineInvitation(memberId:string, email:string): Observable<Object> {
        return this.http.post(this.baseUrl + "api/Line/Request", {
            "id": memberId,
            "email": email,
        }).pipe(
            catchError(this.errorHandler.onError)
        );
    }

    public changePassword(oldPassword: string, newPassword: string): Observable<Object> {
        return this.http.post(this.baseUrl + "api/Account/ChangePassword", {
            oldPassword: oldPassword,
            newPassword: newPassword,
        }).pipe(
            catchError(this.errorHandler.onError)
        );
    }

  public deleteUser(): Observable<Object> {
    return this.http.post(this.baseUrl + "api/Account/DeleteUser", {}).pipe(
      catchError(this.errorHandler.onError)
    );
  }
   


}
