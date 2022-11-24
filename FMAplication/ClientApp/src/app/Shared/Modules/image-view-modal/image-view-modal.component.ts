import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-image-view-modal',
  templateUrl: './image-view-modal.component.html',
  styleUrls: ['./image-view-modal.component.css']
})
export class ImageViewModalComponent implements OnInit {

  constructor(public activeModal: NgbActiveModal) { }

  imageSrc:string = "";
  imageTitle:string = "";
  ngOnInit() {
  }

  download() { 
    let link = document.createElement('a');
    link.href =  this.imageSrc;
    link.download = this.imageTitle;
    link.click();
  }

}
