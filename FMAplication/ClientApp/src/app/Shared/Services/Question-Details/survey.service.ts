import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Survey } from '../../Entity';
import { IPaginator } from '../../interfaces';

@Injectable({
  providedIn: 'root'
})
export class SurveyService {

  public url: string;
  public surveyUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = `${baseUrl}api/v1/questionSet`;
    this.surveyUrl = `${baseUrl}api/v1/survey`;
  }

  public getAllQuestionSet() {
    return this.http.get(`${this.url}/get-surveys`);
  }

  public getQuestionsetById(id: number) {
    return this.http.get(`${this.url}/get-survey/${id}`);
  }

  public deleteSurvey(id: number) {
    return this.http.delete(`${this.url}/delete-survey/${id}`);
  }

  public createSurvey(model: any) {
    return this.http.post<any>(`${this.url}/create-survey/`, model);
  }

  public updateSurvey(model: any) {
    return this.http.put(`${this.url}/update-survey/`, model);
  }

  public createNewSurvey(model: {data:Survey[]}) {
    return this.http.post(`${this.surveyUrl}`, model);
  }
  public getExistingSurvey(model: {data:Survey[]}) {
    return this.http.post<Survey[]>(`${this.surveyUrl}/getExistingSurveys`, model);
  }
  public getSurveys(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string,salesPointId:number) {
    return this.http.get<IPaginator<Survey>>(`${this.surveyUrl}/getSurveys?pageSize=${pageSize}&pageIndex=${pageIndex}&search=${search}&fromDateTime=${fromDate}&toDateTime=${toDate}&salesPointId=${salesPointId}`);
  }

  public getSurveyById(id:number){
    return this.http.get<Survey>(`${this.surveyUrl}/getSurvey/${id}`);
  }
  public editSurvey(survey:Survey){
    return this.http.put<Survey>(`${this.surveyUrl}`,survey);
  }

}
