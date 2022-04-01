import { Component, OnInit } from '@angular/core';
import { Sensor } from 'src/app/models/sensor';
import { SensorService } from 'src/app/services/sensor.service';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  trappedSensors:Sensor[] = [];
  errorSensors:Sensor[] = [];
  regSensors:Sensor[] = [];
  unregSensors:Sensor[] = [];
  sensorCount: number = 0;
  hasNextSensor: boolean = false;
  availableSensorCount: number = 0;
  memberCount: number = 0;
  hasNextMember: boolean = false;




  constructor(
    private sensorService:SensorService,
    private memberService:MemberService
    ) { }

  ngOnInit() {
    this.getSensors();
  }

  getSensors():void{
    this.sensorService.getSensors().subscribe(x =>
    {
      this.trappedSensors = x.filter(s=>s.trapped === true);
      this.regSensors = x.filter(s=>s.registered === true);
      this.unregSensors = x.filter(s=>s.registered === false);
      this.errorSensors = x.filter(s=>s.status === '異常');
      this.sensorCount = x.length;
      this.availableSensorCount = this.regSensors.filter(s => s.status === '正常').length;
      this.hasNextSensor = this.sensorService.hasNext();
    });
    this.memberService.getMembers().subscribe(x =>
    {
      this.memberCount = x.length;
      this.hasNextMember = this.memberService.hasNext();
    });
  }

}
