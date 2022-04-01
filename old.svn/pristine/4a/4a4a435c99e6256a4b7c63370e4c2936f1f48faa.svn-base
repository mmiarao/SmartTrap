import { Component, OnInit, Input, Inject } from '@angular/core';
import { Member } from 'src/app/models/member';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { EditState } from 'src/app/enums';
import {FormControl, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import {ErrorStateMatcher} from '@angular/material/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { catchError } from 'rxjs/operators';
import { MemberService } from '../../services/member.service';
import { WebApiClientService } from '../../services/web-api-client.service';

@Component({
  selector: 'app-line-member',
  templateUrl: './line-member.component.html',
  styleUrls: ['./line-member.component.css']
})
export class LineMemberComponent implements OnInit {

  @Input() member: Member;

    constructor(
      private api: WebApiClientService,
      private route: ActivatedRoute,
      private memberService: MemberService,
      private location: Location,
    ) { }

  ngOnInit() {
    this.getMember();
  }
    private memberId: string;
  getMember():void{
      this.memberId = this.route.snapshot.paramMap.get('id');
      this.memberService.getMember(this.memberId).subscribe(x=>this.member = x);
  }
  
  goBack():void{
    this.location.back();
  }

  send(): void{
      this.api.sendLineInvitation(this.memberId.toString(), this.member.email).subscribe(
          result => {
              alert(this.member.email + "にLINE通報設定メールを送信しました");
              this.location.back();
          }
      );
  }

}
