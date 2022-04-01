import { Injectable, Inject } from '@angular/core';
import { Member, MembersResponse } from '../models/member';
import { MessageService } from './message.service';
import { Observable, of, Subject, Subscriber} from 'rxjs';
import { MEMBERS } from '../mock-data/mock-uiser';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from './error-handler.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  private members: Member[] = [];
  private nextKey: string = null;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private errorHandler: ErrorHandlerService,
    private messageService : MessageService
    ) { }

  getMemberCount(): Observable<number> {
    return of(0);
  }

  getMembers(count: number = 20): Observable<Member[]>{

    //メモリに存在すれば返す
    if (this.members.length) {
      return of(this.members);
    }

    //メンバーが存在しなければサーバから取得
    return new Observable<Member[]>(o => {
      var url = this.baseUrl + "api/Member?count=" + count;

      this.http.get<MembersResponse>(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.members = x.members;
        this.nextKey = x.nextKey;
        o.next(x.members);
      });
    });
    //var sub: Subject<Member[]> = new Subject<Member[]>();
    //var url = this.baseUrl + "api/Member?count=" + count;
    
    //var o = this.http.get<MembersResponse>(url).pipe(
    //  catchError(this.errorHandler.onError)
    //);
    //o.subscribe(x => {
    //  this.members = x.members;
    //  this.nextKey = x.nextKey;
    //  sub.next(x.members);
    //});
    //return sub;
  }

  next(count: number = 20): Observable<Member[]> {
    if (this.nextKey == null) return;

    //keyが有効であればサーバから取得
    return new Observable<Member[]>(o => {
      var url = this.baseUrl + "api/Member?count=" + count + "key=" + this.nextKey;

      this.http.get<MembersResponse>(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.members = this.members.concat(x.members);
        this.nextKey = x.nextKey;
        o.next(x.members);
      });
    });
    //var sub: Subject<Member[]> = new Subject<Member[]>();
    //var url = this.baseUrl + "api/Member?count=" + count + "key=" + this.nextKey;

    //this.http.get<MembersResponse>(url).pipe(
    //  catchError(this.errorHandler.onError)
    //).subscribe(x => {
    //  this.members = this.members.concat(x.members);
    //  this.nextKey = x.nextKey;
    //  sub.next(x.members);
    //});
    //return sub;
  }

  hasNext(): boolean {
    return this.nextKey != null;
  }
  private nextMember(id:string, o:Subscriber<Member>):void {
    this.next().subscribe(members => {
      var member = members.find(x => x.id === id);
      if (member) {
        o.next(member);
      }
      else {
        this.nextMember(id, o);
      }
    });
  }
  getMember(id:string):Observable<Member>{
    this.messageService.add("ユーザーID ${id} 取得");
    return new Observable<Member>(o=>{
      this.getMembers().subscribe(x => {
        var member = this.members.find(x => x.id === id);
        if (member) {
          o.next(member);
        }
        else {
          this.nextMember(id, o);
        }
      });
    });
  }

  addMember(member:Member):Observable<Member>
  {
    this.messageService.add("ユーザー新規登録");
    return new Observable<Member>(o => {
      var url = this.baseUrl + "api/Member";
      this.http.post<Member>(url, member).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.members.push(x);
        o.next(x);
      });

    });
  }

  updateMember(id: string, member: Member): Observable<Member>
  {
    this.messageService.add("ユーザー情報更新");
    return new Observable<Member>(o => {
      var url = this.baseUrl + "api/Member/" + id;
      this.http.put<Member>(url, member).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        var idx = this.members.findIndex(x => x.id === id);
        if (idx >= 0) {
          this.members[idx] = x;
          o.next(x);
        }
      });
    });
  }

  deleteMember(id:string):Observable<any>{
    this.messageService.add("ユーザー削除");
    return new Observable<any>(o => {
      var url = this.baseUrl + "api/Member/" + id;
      this.http.delete(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.members.splice(this.members.findIndex(x => x.id === id), 1);
        o.next();
      });
    });
  }
}
