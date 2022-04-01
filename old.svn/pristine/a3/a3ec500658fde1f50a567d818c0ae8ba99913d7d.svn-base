import { Component, OnInit, Input } from '@angular/core';
import { Sensor } from '../../models/sensor';
import { ActivatedRoute } from '@angular/router';
import { SensorService } from 'src/app/services/sensor.service';
import { Location } from '@angular/common';
import { EditState } from 'src/app/enums';
import { FormControl, Validators } from '@angular/forms';
import { MatCheckboxClickAction, MAT_CHECKBOX_CLICK_ACTION } from '@angular/material/checkbox';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-sensor-detail',
  templateUrl: './sensor-detail.component.html',
  styleUrls: ['./sensor-detail.component.css'],
  providers:[
    {provide:MAT_CHECKBOX_CLICK_ACTION, useValue:'noop'}
  ]

})
export class SensorDetailComponent implements OnInit {

  @Input() sensor: Sensor;
  constructor(
    private route: ActivatedRoute,
    private sensorService: SensorService,
    private location: Location 
  ) { }

  requiredFormControl: FormControl = new FormControl('', [
    Validators.required,
  ]);
  EditState:typeof EditState = EditState;
  action:EditState = EditState.Unknown;
  actionName = "";
  sensorName: string = null;

  ngOnInit() {
    this.route.data.subscribe(x=>{
      if(x['action'] != null){
        this.action = (x['action']);
        switch(this.action){
          case EditState.Create:
            this.actionName = "新規作成";
            this.sensor = new Sensor();
            this.sensor.members = [];
            this.sensor.status = "未登録";
            break;

          case EditState.Register:
            this.actionName = "登録";
            this.getSensor();
            break;

          case EditState.Detail:
            this.actionName = "詳細";
            this.getSensor();
            break;

          case EditState.Delete:
            this.actionName = "削除";
            this.getSensor();
            break;
        }
      }
    });
  }

  getSensor():void{
    const id = this.route.snapshot.paramMap.get('id');
    this.sensorService.getSensor(id).subscribe(x => {
      this.sensor = JSON.parse(JSON.stringify(x));
      this.sensorName = this.sensor.name;
    });
  }

  goBack(): void{
    this.location.back();
  }

  create(): void {
    this.sensorService.addSensor(this.sensor).subscribe(x => {
      this.sensor = x;
      this.goBack();
    });
  }

  register(): void{
    const id = this.route.snapshot.paramMap.get('id');
    this.sensor.registered = true;
    this.sensor.status = "登録完了";
    this.sensorService.updateSensor(id, this.sensor).subscribe(x => {
      this.goBack();
    });
  }

  update(): void{
    const id = this.route.snapshot.paramMap.get('id');
    this.sensorService.updateSensor(id, this.sensor).subscribe(x => {
      this.goBack();
    });
  }

  delete(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.sensorService.deleteSensor(id).subscribe(x => {
      this.goBack();
    });

  }

}
