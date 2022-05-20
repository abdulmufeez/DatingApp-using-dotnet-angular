import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
  styles: [
  ]
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;
  user: User;
  baseUrl = environment.dotnetUrl;

  uploader: FileUploader;
  hasBaseDropZoneOver = false;

  constructor(private accountService: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user
    })
  }

  ngOnInit(): void {
    this.initilizeUploader();
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe(() => { // setting mainphoto
      this.user.photoUrl = photo.url;   // setting main photo in user model
      this.accountService.setCurrentUser(this.user);  // saving updated user

      this.member.mainPhotoUrl = photo.url;
      this.member.photos.forEach(p => {
        if (p.isMain) p.isMain = false;
        if (p.id === photo.id) p.isMain = true;        
      })
      this.toastr.success('Picture set to Profile Photo');
    })
  }

  fileOverBase(event: any) {
    this.hasBaseDropZoneOver = event;
  }

  initilizeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024   //10 mb    
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if (photo.isMain) {
          this.user.photoUrl = photo.url;
          this.member.mainPhotoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
        }
        this.toastr.success('Photo Uploaded');
      }
    }
  }

  deletePhoto(photo: Photo) {
    if (confirm('Are you sure want to delete this photo')) {
      this.memberService.deletePhoto(photo.id).subscribe(() => {
        this.member.photos = this.member.photos.filter(p => p.id !== photo.id); // filtering after deleting photo
        this.toastr.success('Photo Deleted');
      })
    }
  }
}
