import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginatedHeaders, getPaginatedResults } from '../_helpers/paginationHelper';
import { MessageParams } from '../_models/messageParams';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.dotnetUrl;
  messageParams: MessageParams;

  constructor(private http: HttpClient) { 
    this.messageParams = new MessageParams();
  }

  getMessages(messageParams: MessageParams){
    let params = getPaginatedHeaders(messageParams.pageNumber, messageParams.pageSize);
    params = params.append('container', messageParams.container);
    return getPaginatedResults<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(recipientId: number){
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + recipientId.toString());
  }

  sendMessage(recipientId: number, content: string){
    return this.http.post<Message>(this.baseUrl + 'messages', {recipientId, content})
  }

  deleteMessage(id: number, deletedBoth:string){
    return this.http.delete(this.baseUrl + 'messages/' + id.toString() + '?deletedBoth=' + deletedBoth);
  }

  // helper methods
  getMessageParams() {
    return this.messageParams;
  }
  
  setMessageParams(params: MessageParams) {
    this.messageParams = params;
  }
}


