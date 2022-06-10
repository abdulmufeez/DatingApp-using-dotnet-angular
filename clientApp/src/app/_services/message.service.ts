import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginatedHeaders, getPaginatedResults } from '../_helpers/paginationHelper';
import { MessageParams } from '../_models/messageParams';
import { Message } from '../_models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/User';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.dotnetUrl;
  messageParams: MessageParams;

  hubUrl = environment.dotnetHubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient) { 
    this.messageParams = new MessageParams();
  }

  createHubConnection(user: User, recipientId: number){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?otherusername=' + recipientId, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('MessageThread', messages => {
      this.messageThreadSource.next(messages);
    })

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.messageThreadSource.next([...messages, message]);
      })
    })

    this.hubConnection.on('UpdateGroup', (group: Group) => {
      if (group.connections.some(x => x.userId === recipientId)) {
        this.messageThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if (!message.messageRead) {
              message.messageRead = new Date(Date.now())
            }
          })
          this.messageThreadSource.next([...messages]); 
        })
      }
    })
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();}
  }

  getMessages(messageParams: MessageParams){
    let params = getPaginatedHeaders(messageParams.pageNumber, messageParams.pageSize);
    params = params.append('container', messageParams.container);
    return getPaginatedResults<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(recipientId: number){
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + recipientId.toString());
  }

  async sendMessage(recipientId: number, content: string){
    return this.hubConnection.invoke('SendMessageAsync', {recipientId, content}).catch(error => console.log(error));
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


