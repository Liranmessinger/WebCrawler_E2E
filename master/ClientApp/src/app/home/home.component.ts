import { Component } from '@angular/core';
import { ProxyService } from '../Services/proxy.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private urls: string;
  constructor(private proxyService: ProxyService) {

  }

  onSubmitUrls() {
    let requestArray: Array<string> = new Array<string>();
    if (this.urls.includes(',')) {
      requestArray = this.urls.split(',');
    } else {
      requestArray.push(this.urls);
    }
    this.proxyService.postCrawlerData(requestArray).subscribe(res => {
      if (res != null) {
        alert(res);
        this.urls = '';
        return true;
      }
    });
  }

}
