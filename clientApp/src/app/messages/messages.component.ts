import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { MessageParams } from '../_models/messageParams';
import { Pagination } from '../_models/pagination';
import { ConfirmWindowService } from '../_services/confirm-window.service';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styles: [ '.img-circle{max-height:50px; margin-right:10px;}'
  ]
})
export class MessagesComponent implements OnInit {
  messages: Message[]
  pagination: Pagination;
  messageParams: MessageParams;
  loadingFlag = false;

  constructor(private messageService: MessageService, 
      private confirmWindowService: ConfirmWindowService) { 
    this.messageParams = this.messageService.getMessageParams();
  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.loadingFlag = true;
    this.messageService.setMessageParams(this.messageParams);
    this.messageService.getMessages(this.messageParams).subscribe(response => {
      this.messages = response.results;
      this.pagination = response.pagination; 
      this.loadingFlag = false;
    });
  }

  deleteMessage(id: number, deleteBoth:string){
    this.confirmWindowService.confirm('Delete Message?','This will not undone after').subscribe(result => {
      if (result){
        this.messageService.deleteMessage(id, deleteBoth).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id),1);
        })
      }
    })    
  }

  pageChanged(event: any) {
    this.messageParams.pageNumber = event.page;
    this.messageService.setMessageParams(this.messageParams);
    this.loadMessages();
  }
}
