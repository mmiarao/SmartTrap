import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';
import { Sensor } from 'src/app/models/sensor';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { EditState } from 'src/app/enums';
import {FormControl, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import { SensorService } from 'src/app/services/sensor.service';
import { Observable, forkJoin } from 'rxjs';
import { MemberService } from '../../services/member.service';
import { SensorMemberService } from 'src/app/services/sensor-member.service';

@Component({
  selector: 'app-member-sensor',
  templateUrl: './member-sensor.component.html',
  styleUrls: ['./member-sensor.component.css']
})
export class MemberSensorComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private memberService: MemberService,
    private sensorService:SensorService,
    private location: Location,    
    private sensorMemberService:SensorMemberService) { }

  EditState:typeof EditState = EditState;
  action:EditState = EditState.Unknown;
  actionName = "通報先設定";

  sensor: Sensor;
  members: Member[] = [];
  toBeAdded:Member;
  toBeRemoved: Member[] = [];
  hasNext: boolean = false;

  ngOnInit() {
    this.getSensorMembers();
  }

  getSensorMembers(): void{
    const id = this.route.snapshot.paramMap.get('id');
    this.memberService.getMembers().subscribe(r => {
      this.members = JSON.parse(JSON.stringify(r));
      this.hasNext = this.memberService.hasNext();
      this.sensorService.getSensor(id).subscribe(s => {
        this.sensorMemberService.getMembers(id).subscribe(
          sensor => {
          this.setSensor(sensor);
          }
        );
      });
    });
  }
  nextMembers(): void {
    this.memberService.next().subscribe(r => {
      this.members = this.members.concat(JSON.parse(JSON.stringify(r)));
      this.hasNext = this.memberService.hasNext();
    });
  }
  add():void{
    if(this.toBeAdded && this.sensor){
      this.sensorMemberService.addMemberSensor(this.toBeAdded, this.sensor).subscribe(sensor =>
      {
        this.setSensor(sensor);
      });
    }
    this.toBeAdded = null;
  }

  private setSensor(sensor: Sensor): void {
    this.sensor = sensor;
  }

  remove():void{

    const list:Observable<any>[] = [];

    if(this.toBeRemoved){
      this.toBeRemoved.forEach(x=>{
        this.sensorMemberService.deleteMemberSensor(x.id, this.sensor.id).subscribe(s => {
          this.setSensor(s);
        });
      });
    }
  }

  goBack():void{
    this.location.back();
  }
}
