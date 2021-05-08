var express = require("express");
var multer = require("multer");
var app = express();
var path = require("path");
var fs = require('fs');

// Allow cross origin resource sharing (CORS) within our application
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});

var storage = multer.diskStorage({
  destination: function (req, file, cb) {
    cb(null, 'upload/')
  },
  filename: function (req, file, cb) {
    cb(null, file.originalname.split('.')[0] + path.extname(file.originalname));
  }
})

var upload = multer({ storage: storage })

// "files" should be the same name as what's coming from the field name on the client side.
app.post("/upload", upload.any(), function(req, res) {
    res.send(req.files);
    console.log("files = ", req.files);
});
app.get("/upload/:track_name",function(req,res){
  // перебірать путі до файлов
  // делать чек не пустой лі файл
  var track_name=req.params.track_name;
  fs.createReadStream('upload/'+ track_name ).pipe(res);
  console.log("Test");
});
var server = app.listen(3000, function() {
    console.log("Listening on port %s...", server.address().port);
});