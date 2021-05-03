import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { CrawlerResponse, CrawlerResponseArray } from '../Models/crawlerResponse';

@Injectable({
    providedIn: 'root'
})

export class ProxyService {
    apiURL: String;
    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        }),
        params: {}
    };

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.apiURL = baseUrl;
    }

    getCrawlerData(minutes: number): Observable<CrawlerResponseArray> {
        return this.httpClient.get<CrawlerResponseArray>
        (this.apiURL + 'api/WebCrawler/getCrawlerData/' + minutes.toString(), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.handleError)
            );
    }

    postCrawlerData(data: Array<string>): Observable<string> {
        return this.httpClient.post(
            this.apiURL + 'api/WebCrawler/postCrawlerData',
            JSON.stringify(data),
            { ...this.httpOptions, responseType: 'text' });
    }


    handleError(error) {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
            // Get client-side error
            errorMessage = error.error.message;
        } else {
            // Get server-side error
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        console.log(errorMessage);
        return throwError(errorMessage);
    }

}