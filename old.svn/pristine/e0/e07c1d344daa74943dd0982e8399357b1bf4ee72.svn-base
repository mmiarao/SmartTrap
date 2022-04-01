import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SensorsComponent} from './components/sensors/sensors.component'
import { SensorDetailComponent } from './components/sensor-detail/sensor-detail.component';
//import { LogoutComponent } from './components/logout/logout.component';
import { EditState } from './enums';
import { MembersComponent } from './components/members/members.component';
import { MemberDetailComponent } from './components/member-detail/member-detail.component';
import { MemberSensorComponent } from './components/member-sensor/member-sensor.component';
import { AccountComponent } from './components/account/account.component';
import { LineMemberComponent } from './components/line-member/line-member.component';

const routes: Routes = [
  { path : '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'sensors', component: SensorsComponent, canActivate: [AuthorizeGuard]},
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthorizeGuard] },
  { path: 'sensors/delete/:id', component: SensorDetailComponent, data: { action: EditState.Delete }, canActivate: [AuthorizeGuard] },
  { path: 'sensors/detail/:id', component: SensorDetailComponent, data: { action: EditState.Detail }, canActivate: [AuthorizeGuard]},
  { path: 'sensors/register/:id', component: SensorDetailComponent, data: { action: EditState.Register }, canActivate: [AuthorizeGuard]},
  { path: 'sensors/create', component: SensorDetailComponent, data: { action: EditState.Create }, canActivate: [AuthorizeGuard] },
  { path: 'members', component: MembersComponent, canActivate: [AuthorizeGuard]},
  { path: 'members/detail/:id', component: MemberDetailComponent, data: { action: EditState.Detail }, canActivate: [AuthorizeGuard]},
  { path: 'members/create', component: MemberDetailComponent, data: { action: EditState.Create }, canActivate: [AuthorizeGuard]},
  { path: 'members/delete/:id', component: MemberDetailComponent, data: { action: EditState.Delete }, canActivate: [AuthorizeGuard]},
  { path: 'sensors/member-sensor/:id', component: MemberSensorComponent, canActivate: [AuthorizeGuard]},
  { path: 'account', component: AccountComponent, canActivate: [AuthorizeGuard]},
  { path: 'members/line/:id', component: LineMemberComponent, canActivate: [AuthorizeGuard]},
  //{ path : 'logout', component: LogoutComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {  }
