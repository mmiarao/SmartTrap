import { Component, Output, ChangeDetectorRef } from '@angular/core';
//import { LoginService } from './services/login.service';
import { MessageService } from './services/message.service';
import { AuthorizeService } from '../api-authorization/authorize.service';
import { Overlay } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { MatSpinner } from '@angular/material';
import { nextTick } from 'q';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
      private messageService:MessageService,
      private cd: ChangeDetectorRef,
      private authService: AuthorizeService,
      private overlay: Overlay
    ) 
  {
      authService.completeSignIn
      authService.isAuthenticated().subscribe(b => {
        if (b) {
          this.overlayRef.detach();
        }
        else {

          //this.overlayRef.detach();
        }
      });
    }
    overlayRef = this.overlay.create({
        hasBackdrop: true,
        positionStrategy: this.overlay
            .position().global().centerHorizontally().centerVertically()
    });

    ngAfterViewInit(): void {
      //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
      //Add 'implements AfterViewInit' to the class.

      //Debug
      // this.authed = true;

    }
    ngOnInit() {
        this.authService.signInState.subscribe(
            x => {
                console.log('SignIn/SignOut  State Changed' + x);
                this.overlayRef.detach();
            },
            err => console.error('Observer got an error: ' + err),
            () => console.log('Observer got a complete notification'),
        );
        // ローディング開始
        this.overlayRef.attach(new ComponentPortal(MatSpinner));
    }
    
}
