import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.css'],
})
export class ImageUploaderComponent {

  @Output() onChangeFile = new EventEmitter();
  @Input() url: string = '';
  @Input() title: string = '';
  @Input() width: string = '220px';
  @Input() height: string = '250px';
  @Input() removable: boolean = false;

  elementCustomId:string = '';
	imageError: string;
  selectedFile: File = null;
  constructor() {
    this.elementCustomId = this.randomId();
  }



  // for Image --- start
	onChangeInputFile(event: any) {
		if (event.target.files && event.target.files[0]) {
			const reader = new FileReader();
      const file = event.target.files[0];
      this.selectedFile = file;
			if (!this.isValidImage(file)) return;
			reader.onload = (loadEvent: any) => {
				this.url = loadEvent.target.result;
			};
			reader.readAsDataURL(file);
      this.onChangeFile.emit(this.selectedFile);
    }
	}

	isValidImage(file) {
		const fileExt = file.name.split('.').pop().toLowerCase();
		if (!(['jpg','jpeg','png'].indexOf(fileExt) > -1)) {
			this.imageError = 'Invalid file type';
      this.clearImage();
			return false;
		}
		this.imageError = '';
		return true;
	}

	onRemoveInputFile() {
		this.clearImage();
  }
  
  clearImage() { 
    this.url = '';
    this.selectedFile = null;
    this.onChangeFile.emit(this.selectedFile);
  }

  randomId() {
    // Math.random should be unique because of its seeding algorithm.
    // Convert it to base 36 (numbers + letters), and grab the first 9 characters
    // after the decimal.
    return 'inputFile' + Math.random().toString(36).substr(2, 9);
  };
	// for Image --- end
}
