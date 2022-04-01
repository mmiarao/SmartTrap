import { Pipe, PipeTransform } from '@angular/core';
import { Member } from '../models/member';
import { Sensor } from '../models/sensor';
import { MembersComponent } from '../components/members/members.component';

@Pipe({
  name: 'memberFilter'
})
export class MemberFilterPipe implements PipeTransform {

  transform(members: Member[], sensor:Sensor, hideRegMember:boolean = true): any {
    if (!sensor.members) return members;
    var r = members.filter(x => {
      var result: boolean = hideRegMember;
      sensor.members.forEach(y=>{
        if(y.id === x.id){
          result = !hideRegMember;
        }
      })
      return result;
    });
    return r;
  }

}
