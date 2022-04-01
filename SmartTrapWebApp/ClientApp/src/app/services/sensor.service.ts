import { Injectable, Inject } from '@angular/core';
import { SENSORS } from '../mock-data/mock-sensors';
import { Sensor, SensorResponse } from '../models/sensor';
import {Observable, of, Subject, Subscriber }from 'rxjs'
import { MessageService } from './message.service';
import { SensorMemberService } from './sensor-member.service';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from './error-handler.service';
import { catchError } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/authorize.service';

@Injectable({
  providedIn: 'root'
})
export class SensorService {

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private errorHandler: ErrorHandlerService,
    private messageService : MessageService,
    private authService:AuthorizeService
    ) { }

  private sensors: Sensor[] = [];
  private nextKey: string;
 
  getSensors(count: number = 20): Observable<Sensor[]> {

    //メモリに存在すれば返す
    if (this.sensors.length) {
      return of(this.sensors);
    }

    return new Observable<Sensor[]>(o => {
      var url = this.baseUrl + "api/Sensor?count=" + count;
      this.http.get<SensorResponse>(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.sensors = x.sensors;
        this.nextKey = x.nextKey;
        o.next(x.sensors);
      });
    });
  }

  next(count: number = 20): Observable<Sensor[]> {
    if (this.nextKey == null) return;

    return new Observable<Sensor[]>(o => {
      var url = this.baseUrl + "api/Sensor?count=" + count + "key=" + this.nextKey;
      this.http.get<SensorResponse>(url).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        this.sensors = this.sensors.concat(x.sensors);
        this.nextKey = x.nextKey;
        o.next(x.sensors);
      });
    });
  }

  hasNext(): boolean {
    return this.nextKey != null;
  }
  private nextSensor(id: string, o: Subscriber<Sensor>):void {
    this.next().subscribe(n => {
      var sensor = this.sensors.find(n => n.id === id);
      if (sensor) {
        o.next(sensor);
      }
      else {
        this.nextSensor(id, o);
      }
    });
  }
  getSensor(id: string): Observable<Sensor>{
    return new Observable<Sensor>(
      o => {
        this.messageService.add('センサーID ${id} 取得');
        this.getSensors().subscribe(x => {
          var sensor = this.sensors.find(x => x.id === id);
          if (sensor) {
            o.next(sensor);
          }
          else {
            this.nextSensor(id, o);
          }
        });
      },
    );
  }
  
  addSensor(sensor: Sensor): Observable<Sensor> {
    this.messageService.add("センサー登録");
    return new Observable<Sensor>(o => {
      this.authService.isSysAdmin().subscribe(x => {
        var url = this.baseUrl + "api/Sensor";
        var r = this.http.post<Sensor>(url, sensor).pipe(
          catchError(this.errorHandler.onError)
        ).subscribe(s => {
          this.sensors.push(s);
          o.next(s);
        });
      });
    });
  }

  updateSensor(id: string, sensor: Sensor): Observable<Sensor> {
    this.messageService.add("センサー更新");
    return new Observable<Sensor>(o => {
      var url = this.baseUrl + "api/Sensor/" + id;
      this.http.put<Sensor>(url, sensor).pipe(
        catchError(this.errorHandler.onError)
      ).subscribe(x => {
        var idx = this.sensors.findIndex(x => x.id === id);
        if (idx >= 0) {
          this.sensors[idx] = x;
          o.next(x);
        }
      });
    });
  }

  deleteSensor(id: string): Observable<any> {
    this.messageService.add("ユーザー削除");
    return new Observable<Sensor>(o => {
      var url = this.baseUrl + "api/Sensor/" + id;
      var r = this.http.delete(url).pipe(
        catchError(this.errorHandler.onError)
      );
      r.subscribe(x => {
        this.sensors.splice(this.sensors.findIndex(x => x.id === id), 1);
        o.next();
      });
    });
  }

}
