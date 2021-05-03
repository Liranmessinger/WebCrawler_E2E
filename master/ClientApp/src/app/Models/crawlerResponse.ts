export class CrawlerResponseArray {
    public data: Array<CrawlerResponse>;
    constructor() {
        this.data = new Array<CrawlerResponse>();
    }
}

export class CrawlerResponse {
    public domain: string;
    public lastUpdate: string;
    public crawlerData: Array<CrawlerData>;
    constructor() {
        this.crawlerData = new Array<CrawlerData>();
    }
}

export class CrawlerData {
    public title: string;
    public url: string;
}