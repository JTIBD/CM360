import { Injectable } from '@angular/core';
import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { strict } from 'assert';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

    constructor() { }

  toFormData(obj) {
    let formData = new FormData();
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        formData.append(property, value);
      }
    }

    return formData;
  }

  setUserInfoToLocalStorage(value) {
    localStorage.setItem('userinfo', JSON.stringify(value));
  }

  getUserInfoFromLocalStorage(): any {
    if(!localStorage.getItem('userinfo')) return null;
    return JSON.parse(localStorage.getItem('userinfo'));
  }

  setActivityPermissionToSessionStorage(value) {
    localStorage.setItem('activitypermission', JSON.stringify(value));
  }

  getActivityPermissionToSessionStorage(): any {
    if(!localStorage.getItem('activitypermission')) return null;
    return JSON.parse(localStorage.getItem('activitypermission'));
    }

    DownloadFile(file: Blob | ArrayBuffer, fileName: string, type: string): void {
        var blob = new Blob([file], { "type": type })
        
        var link = document.createElement("a");
        let reader = new FileReader();
        reader.readAsDataURL(blob);
        link.download = fileName;
        reader.onload = function () {
            if (typeof (reader.result) === 'string') {
                link.href = reader.result; // data url
                link.click();
            }

        };
        link.click();
    }
    ngbDateToDate(date: NgbDateStruct): Date | null {
      if (!!date) {
        return new Date(date.year, date.month-1, date.day);
      }
      return null;
    }
  
   dateToNgbDate(date: Date): NgbDateStruct | null {
      if (date != null) {
          let newDate = new Date(date);
        let ngbDateStruct = { day: newDate.getDate(), month: newDate.getMonth()+1, year: newDate.getFullYear() } as NgbDateStruct;
        return ngbDateStruct;
      }
      return null;
    }  
}
