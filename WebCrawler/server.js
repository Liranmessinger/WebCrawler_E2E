var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var fetchData = require('./node-fetch.js');

var router = express.Router();

//start body parser configuration
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use(bodyParser.json());
//end body parser configuration

app.post('/getWebPageTitle', function (req, res) {
    const url = req.body.url;
    if (url) {
        fetchData.getData(url).then((text) => {
            var title = 'error on process';
            if (text && text.includes('<title>'))
                title = text.split('<title>')[1].split('</title>')[0];
            console.log(title);
            const webpage = {
                Title: title,
                Url: url
            }
            res.json(webpage);
        }).catch(e => console.log(e));
    }
})

app.get('/getUserTest', function (req, res) {
    var result = 'liran123';
    console.log(result);
    res.end(result);
})

var server = app.listen(8081, function () {
    var host = server.address().address
    var port = server.address().port
    console.log("Example app listening at http://%s:%s", host, port)
})