import { Injectable, Inject } from '@angular/core';
import { SensorService } from './sensor.service';
import { Observable, of, Subject } from 'rxjs';
import { MessageService } from './message.service';
import { Member } from '../models/member';
import { Sensor } from '../models/sensor';
import { SENSORS } from '../mock-data/mock-sensors';
import { MEMBERS } from '../mock-data/mock-uiser';
import { MemberSensor } from '../models/member-sensor';
import { MemberService } from './member.service';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from './error-handler.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SensorMemberService {

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private errorHandler: ErrorHandlerService,
    private messageService: MessageService,
    private sensorService: SensorService,
    private memberService: MemberService,
  ) { }

  private sensors: Sensor[] = [];

  //通報設定されたMemberリスト取得
  //Sensorのプロパティとして設定し返す
  //サーバ側で全取得処理を行うためnextKey(次レコードの読み込み)は無し
  getMembers(sensorId: string, count: number = 20): Observable<Sensor> {

    if (!sensorId) return;

    var sensorInMem = this.sensors.find(x => x.id === sensorId);
    if (sensorInMem)
      return of(sensorInMem);

    return new Observable<Sensor>(o => {
      var url = this.baseUrl + "api/SensorMember/" + sensorId + "?count=" + count;
      this.http.get<string[]>(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(memberIds => {
        this.sensorService.getSensor(sensorId).subscribe(sensor => {
          this.sensors.push(sensor);
          sensor.members = [];
          memberIds.forEach(id => {
            this.memberService.getMember(id).subscribe(member => {
              sensor.members.push(member);
              o.next(sensor);
            });
          });
          o.next(sensor);
        })
      });
    });

    //var sub: Subject<Sensor> = new Subject<Sensor>();
    //var url = this.baseUrl + "api/SensorMember/" + sensorId + "?count=" + count;
    //this.http.get<string[]>(url).pipe(
    //  catchError(this.errorHandler.onError)
    //).subscribe(memberIds => {
    //  this.sensorService.getSensor(sensorId).subscribe(sensor => {
    //    this.sensors.push(sensor);
    //    sensor.members = [];
    //    memberIds.forEach(id => {
    //      this.memberService.getMember(id).subscribe(member => {
    //        sensor.members.push(member);
    //        sub.next(sensor);
    //      });
    //    });
    //    sub.next(sensor);
    //  })
    //});
    //return sub;
  }


  addMemberSensor(member: Member, sensor: Sensor): Observable<Sensor>{
    return new Observable<Sensor>(o => {
      var url = this.baseUrl + "api/SensorMember";

      this.http.post(url, {
        SensorId: sensor.id,
        MemberId: member.id,
      }).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        sensor.members.push(member);
        o.next(sensor);
      });
    });
  }

  deleteMemberSensor(memberId: string, sensorId: string): Observable<Sensor>{

    return new Observable<Sensor>(o => {
      var url = this.baseUrl + "api/SensorMember/Sensor/" + sensorId + "/Member/" + memberId;
      this.http.delete(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.sensorService.getSensor(sensorId).subscribe(sensor => {
          sensor.members.splice(sensor.members.findIndex(x => x.id === memberId), 1);
          o.next(sensor);
        })
      });
    });
  }
}
