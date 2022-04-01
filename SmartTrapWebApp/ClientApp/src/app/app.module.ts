import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
//import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SensorsComponent } from './components/sensors/sensors.component';
import { SensorDetailComponent } from './components/sensor-detail/sensor-detail.component';
import { MessagesComponent } from './components/messages/messages.component';

import { AppRoutingModule } from './app-routing.module';
//import { LoginComponent } from './components/login/login.component';
//import { LoginService } from './services/login.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatFormFieldModule} from '@angular/material/form-field';
import { MatInputModule} from '@angular/material/input';
//import { LogoutComponent } from './components/logout/logout.component';
import { MainnaviComponent } from './components/mainnavi/mainnavi.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule} from '@angular/material/grid-list';
import { MatCheckboxModule} from '@angular/material/checkbox';
import { MatCheckBoxReadOnlyDirective } from './directives/mat-check-box-read-only.directive';
import { MatSelectModule} from '@angular/material/select';
import { MembersComponent } from './components/members/members.component';
import { MemberDetailComponent } from './components/member-detail/member-detail.component';
import { MemberSensorComponent } from './components/member-sensor/member-sensor.component';
import { AccountComponent } from './components/account/account.component';
import { MatBadgeModule} from '@angular/material/badge';
import { MatCardModule} from '@angular/material/card';
import { LineMemberComponent } from './components/line-member/line-member.component';
import { LoadingComponent } from './components/loading/loading.component';
import { MatProgressSpinnerModule, MatSpinner } from '@angular/material';
import { OverlayModule } from '@angular/cdk/overlay';
import { MemberFilterPipe } from './pipes/member-filter.pipe';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SensorsComponent,
    SensorDetailComponent,
    MessagesComponent,
    DashboardComponent,
    //LoginComponent,
    //LogoutComponent,
    MainnaviComponent,
    MatCheckBoxReadOnlyDirective,
    MembersComponent,
    MemberDetailComponent,
    MemberSensorComponent,
    AccountComponent,
    LineMemberComponent,
    LoadingComponent,
    MemberFilterPipe,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatGridListModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatBadgeModule,
      MatCardModule,
      MatProgressSpinnerModule,
      OverlayModule,
      BrowserAnimationsModule

    //RouterModule.forRoot([
    //  { path: '', component: HomeComponent, pathMatch: 'full' },
    //  { path: 'counter', component: CounterComponent },
    //  { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
    //])
  ],
  entryComponents: [
    MatSpinner
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
