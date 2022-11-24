import { AvCommunicationService } from './../../../Shared/Services/AvCommunication/avCommunication.service';
import { AvCommunication, AvCommunicationCampaignType, CampaignType } from './../../../Shared/Entity/AVCommunications/avCommunication';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Brand } from 'src/app/Shared/Entity/Brands';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-add-av-communication',
  templateUrl: './add-av-communication.component.html',
  styleUrls: ['./add-av-communication.component.css']
})
export class AddAvCommunicationComponent implements OnInit {
  model:AvCommunication = new AvCommunication();
  
  campaignTypes: MapObject[] = AvCommunicationCampaignType.CampaignType;
  brandList: Brand[] = [];
  
  selectedFileName: string;
  isFormValid: boolean;
  fileError: string;
  isNew: boolean;


  validVideoFormat = ['mp4','mpeg','avi', 'wmv']; 
  validImageFormat = ['jpg','jpeg','png'];

  

  constructor(private router:Router, private route: ActivatedRoute, private brandService: BrandService, 
    private avService:AvCommunicationService) {  

      this.getBrandList();
      if (
        Object.keys(this.route.snapshot.params).length !== 0 &&
        this.route.snapshot.params.id !== "undefined") {
        let avCommunicationId = this.route.snapshot.params.id;
        this.getAvCommunicationById(avCommunicationId);
        this.isNew = false;
      } else {
        this.isNew = true;
      }
   }

  getAvCommunicationById(avCommunicationId: number) {
    this.avService.getById(avCommunicationId).subscribe((data:AvCommunication )=> {
      this.model = data;
      let filePath = this.model.filePath;
      
      let names:string[] = filePath.split("/");
      if (names.length > 0) {
        this.isFormValid = true;
        this.selectedFileName = names[names.length-1]
      }
    })
  }

  ngOnInit() {
  }
  clearFile() { 
    this.model.file = null;
    this.isFormValid = false;
    this.selectedFileName = '';
  }

  campaignChange(){
    this.clearFile();
  }


  getBrandList() {
    this.brandService.getAllForSelect().subscribe(data => {
        this.brandList = data.data;
    })
  }
  public fnSaveProduct() {
    console.log(this.model);
  
     this.model.id == 0 ? this.insert() : this.update();
  }
  public fnRouteProdList() {
    this.router.navigate(['/av-communication/av-communication-list']);
  }

  onChangeInputFile(event: any) {
		if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      this.model.file = file;
			if (!this.isValidFile(file)) return;
      if (!this.isValidSize(file)) return;
      this.isFormValid = true;
      this.selectedFileName = file.name;
      this.model.filePath = this.selectedFileName;
    } else {
      this.clearFile();
		}
	}

	isValidFile(file) {
		const fileExt = file.name.split('.').pop().toLowerCase();
    const validFileTypes = this.model.campaignType === CampaignType.Image ? this.validImageFormat : this.validVideoFormat; 
    if (this.model.campaignType === CampaignType.Video ? [] : []) 
		if (!(validFileTypes.indexOf(fileExt) > -1)) {
			this.fileError = `Invalid file type for ${CampaignType[this.model.campaignType]} campaign`;
      this.clearFile();
			return false;
		}
		this.fileError = '';
		return true;
	}
  isValidSize(file:File){
    const standardSize = this.model.campaignType === CampaignType.Video ? 30 : 1;
    const size = this.getFileSizeInMb(file.size);
    if (size > standardSize ){
      this.fileError = `File size can't be more than ${standardSize} MB for ${CampaignType[this.model.campaignType]} campaign `;
      this.clearFile();
			return false;
    }
    this.fileError = '';
		return true;
  }

  getFileSizeInMb(bytes) { 
    return bytes / (1024*1024); 
  }

  insert(){
    this.avService.save(this.model).subscribe(data => {
      this.router.navigate(['/av-communication/av-communication-list']);
    })
  }

  update(){
    this.avService.update(this.model).subscribe(data => {
      this.router.navigate(['/av-communication/av-communication-list']);
    })
  }
}
