import { Component, OnInit } from '@angular/core';
import { Sensor } from '../../models/sensor';
import { SENSORS } from '../../mock-data/mock-sensors';
import { SensorService } from '../../services/sensor.service';
import { Observable, of} from 'rxjs'
import { MatCheckboxClickAction, MAT_CHECKBOX_CLICK_ACTION } from '@angular/material/checkbox';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { SensorMemberService } from '../../services/sensor-member.service';

@Component({
  selector: 'app-sensors',
  templateUrl: './sensors.component.html',
  styleUrls: ['./sensors.component.css'],
  providers:[
    {provide:MAT_CHECKBOX_CLICK_ACTION, useValue:'noop'}
  ]
})

export class SensorsComponent implements OnInit {

  sensors: Sensor[] = [];
  hasNext: boolean = false;
  isSystemAdmin:boolean = false;
  constructor(
    private sensorService: SensorService,
    private sensorMemberService:SensorMemberService,
    private authService:AuthorizeService
  ) { }

  ngOnInit() {
    this.getSensors();
    this.authService.isSysAdmin().subscribe(b => this.isSystemAdmin = b);
  }

  getSensors():void{
    this.sensors = [];
    this.sensorService.getSensors().subscribe(x => {
      this.sensors = x;
      this.hasNext = this.sensorService.hasNext();
    });
  }

  getMembers(sensorId: string): void {
    var i:number = this.sensors.findIndex(x => x.id === sensorId);
    this.sensorMemberService.getMembers(sensorId).subscribe(sensor => {
      this.sensors[i] = sensor;
    });
  }

}
