import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Question } from '../../Entity/Questions/question';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  public url: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = `${baseUrl}api/v1/question`;
  }

  public getQuestionData() {
    return this.http.get(this.url);
  }

  public getActiveQuestions() {
    return this.http.get(`${this.url}/get-active-questions`);
  }

  public getQuestionById(id: number) {
    return this.http.get(`${this.url}/${id}`);
  }

  public insertQuestion(question: Question) {
    return this.http.post<Question>(`${this.url}/create`, question);
  }

  public updateQuestion(question: Question) {
    return this.http.put<Question>(`${this.url}/update`, question);
  }

  public deleteQuestion(id: number) {
    return this.http.delete<any>(`${this.url}/delete/${id}`);
  }

  public getQuestionOptionsByQuestionId(questionId: number) {
    return this.http.get(`${this.url}/question-options/${questionId}`);
  }
}
