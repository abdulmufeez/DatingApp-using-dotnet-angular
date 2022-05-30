import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { UserPresenceService } from 'src/app/_services/user-presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'
  ]
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  messages: Message[] = [];

  //these are for when we specifically click on any tab then its laod and open
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;

  //for photo gallery
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private memberService: MembersService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private messageService: MessageService,
    public userPresenceService: UserPresenceService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.member = data['member'];
    })
    this.route.queryParams.subscribe(params => {
      params['tab'] ? this.selectTab(params['tab']) : this.selectTab(0);
    })    

    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]    

    this.galleryImages = this.getImages();
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos){
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      })
    }
    return imageUrls;
  }  

  addLike(member: Member){
    this.memberService.addLike(member.id).subscribe(() => {
      this.toastr.success('You have liked ' + member.knownAs);
    })
  }

  loadMessages(){
    this.messageService.getMessageThread(this.member.id).subscribe(messages => {
      this.messages = messages;
    })
  }

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }

  // to activated specific tab
  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if (this.activeTab.heading === 'Photos') {
      this.galleryImages = this.getImages();
    }
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.loadMessages();
    }  
  }
}
