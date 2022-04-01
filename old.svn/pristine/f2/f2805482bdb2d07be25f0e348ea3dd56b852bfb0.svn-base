import { Component, OnInit } from '@angular/core';
import { WebApiClientService } from '../../services/web-api-client.service';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { ErrorHandlerService } from '../../services/error-handler.service';
import { catchError } from 'rxjs/operators';
//import { Account } from 'src/app/models/account';
//import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

//  account:Account;
    id: string;
  oldPwd:string;
  newPwd:string;
  newPwdConfirm:string;
    constructor(
        private authService: AuthorizeService,
        private api: WebApiClientService,
      private errorHandler:ErrorHandlerService
    //private loginService:LoginService
  ) { }

    ngOnInit() {
        this.authService.getUser().pipe(
            catchError(this.errorHandler.onError)
        ).subscribe(
            u => {
                this.id = u.name;
            }
        )
    //this.account = this.loginService.account;
    }

  update():void{
    if(!this.newPwd)return;
    if(!this.oldPwd)return;
    if(this.newPwd != this.newPwdConfirm){
      alert("確認用パスワードが異なります");
      return;
    }
      this.api.changePassword(this.oldPwd, this.newPwd).subscribe(
          result => {
              alert("パスワードが正常に更新されました");
              this.oldPwd = "";
              this.newPwd = "";
              this.newPwdConfirm = "";
          }
      );
  }

  delete(): void {
    this.api.deleteUser().subscribe(next => {})
  }

}
