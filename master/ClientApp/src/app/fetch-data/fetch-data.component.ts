import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProxyService } from '../Services/proxy.service';
import { CrawlerResponse, CrawlerResponseArray } from '../Models/crawlerResponse';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  cacheData: CrawlerResponseArray;
  constructor(private proxyService: ProxyService) {
    this.onRefresh();
  }

  onRefresh() {
    this.proxyService.getCrawlerData(10).subscribe(res => {
      if (res != null) {
        this.cacheData = res;
        console.log(this.cacheData);
      }
    });
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
