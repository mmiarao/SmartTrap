import { Component, OnInit, Input } from '@angular/core';
import { Member } from 'src/app/models/member';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { EditState } from 'src/app/enums';
import {FormControl, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import {ErrorStateMatcher} from '@angular/material/core';
import { MemberService } from '../../services/member.service';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  @Input() member: Member;
  memberName: string;

  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute,
    private memberService: MemberService,
    private location: Location    
  ) { }

  EditState:typeof EditState = EditState;
  action:EditState = EditState.Unknown;
  actionName = "";

  lineBtnSrc = "/assets/line/btn_base.png";

  requiredFormControl:FormControl = new FormControl('', [
    Validators.required,
  ]);

  emailFormControl:FormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  
  ngOnInit() {
    this.route.data.subscribe(x=>{
      if(x['action'] != null){
        this.action = (x['action']);
        switch(this.action){
          case EditState.Create:
            this.createUSer();
            this.actionName = "会員新規登録";
            break;
  
          case EditState.Detail:
              this.getUser();
              this.actionName = "詳細";
            break;
          
          case EditState.Delete:
            this.getUser();
            this.actionName = "削除";
            break;
        }
      }
    });
  }
  getUser():void{
    const id = this.route.snapshot.paramMap.get('id');
    this.memberService.getMember(id).subscribe(x => {
      this.memberName = x.name;
      this.member = JSON.parse(JSON.stringify(x));
    });
  }
  createUSer():void{
    this.member = new Member();
  }

  goBack():void{
    this.location.back();
  }

  register(): void{
    this.member.useEmail = true;
    this.member.useLine = false;
    this.memberService.addMember(this.member).subscribe(next => {
        this.location.back();
      },
      error => {
          this.messageService.add("新規作成エラー");
      });
    
  }
  update(): void{
    const id = this.route.snapshot.paramMap.get('id');
    this.memberService.updateMember(id, this.member).subscribe(x => {
      this.location.back();
      },
      error => {
        this.messageService.add("更新エラー");
      });
  }
  delete():void{
    const id = this.route.snapshot.paramMap.get('id');
    this.memberService.deleteMember(id).subscribe(x => {
      this.location.back();
      },
      error => {
        this.messageService.add("削除エラー");
      });
  }
}
