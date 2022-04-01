//import { Injectable, Output, EventEmitter } from '@angular/core';
//import { Observable, of} from 'rxjs';
//import { MessageService } from './message.service';
//import { Account } from '../models/account';

//@Injectable({
//  providedIn: 'root'
//})
//export class LoginService {

//  constructor(
//    private messageService:MessageService    
//    ) 
//    {
//      this.account = new Account();
//      this.account.id = "test@test.com";
//      this.account.password = "password";
//    }
  
//  @Output() authed = new EventEmitter<boolean>();
//  authResult:boolean = false;
//  account:Account;

//  login(id:string, password:string):Observable<boolean>{
//    this.authResult = true;
//    this.messageService.add("ID:" + id + " ログイン成功");
//    this.authed.emit(this.authResult);
//    if(this.authResult){
//      this.account = new Account();
//      this.account.id = id;
//      this.account.password = password;
//    }

    
//    return of(this.authResult);
//  }

//  logout():void{
//    this.account = null;
//    this.authResult = false;
//    this.messageService.add("ログアウトしました");
//    this.authed.emit(this.authResult);
//  }




//}
