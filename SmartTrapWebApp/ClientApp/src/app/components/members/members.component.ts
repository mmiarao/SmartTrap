import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';
import { MemberService } from '../../services/member.service';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {

  members:Member[] = [];
    constructor(
        private messageService: MessageService,
        private memberService: MemberService
    ) {
        this.loadCount = 20;
    }

  ngOnInit() {
    this.getMembers();
  }
  hasNext: boolean;
  loadCount: number;
  getMembers(): void{
      this.memberService.getMembers(this.loadCount).subscribe(x => {
        this.members = x;
        this.hasNext = this.memberService.hasNext();
      });
  }
  next(): void {
    this.memberService.next(this.loadCount).subscribe(x => {
      this.members = this.members.concat(x);
      this.hasNext = this.memberService.hasNext();
    });
  }
}
