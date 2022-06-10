import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'
  ]
})
export class MemberMessagesComponent implements OnInit {  
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() messages: Message[];
  @Input() recipientId: number;
  messageContent: string;
  loading = false;

  constructor(public messageService: MessageService) { }

  ngOnInit(): void {    
  }

  sendMessage(){
    this.loading = true;
    this.messageService.sendMessage(this.recipientId, this.messageContent).then(() => {
      // this.messages.push(message);
      this.messageForm.reset(); 
    }).finally(() => this.loading = false);
  }

  deleteMessage(id: number, deleteBoth:string){
    this.messageService.deleteMessage(id, deleteBoth).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id),1);
    })
  }
}
