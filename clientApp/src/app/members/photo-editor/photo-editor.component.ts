import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
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

  constructor(private accountService: AccountService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user
    })
  }

  ngOnInit(): void {
    this.initilizeUploader();
  }

  fileOverBase(event: any){
      this.hasBaseDropZoneOver = event; 
  }

  initilizeUploader(){
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
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
        this.toastr.success('Photo Uploaded');
      }
    }
  }  
}
