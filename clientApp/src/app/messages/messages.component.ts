import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { MessageParams } from '../_models/messageParams';
import { Pagination } from '../_models/pagination';
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

  constructor(private messageService: MessageService) { 
    this.messageParams = this.messageService.getMessageParams();
  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.messageService.setMessageParams(this.messageParams);
    this.messageService.getMessages(this.messageParams).subscribe(response => {
      this.messages = response.results;
      this.pagination = response.pagination; 
    });
  }

  pageChanged(event: any) {
    this.messageParams.pageNumber = event.page;
    this.messageService.setMessageParams(this.messageParams);
    this.loadMessages();
  }
}
