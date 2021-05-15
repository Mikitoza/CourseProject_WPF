const express = require("express");
const multer  = require("multer");
const path = require('path');
const fs = require('fs');
 
const app = express();

app.use(express.static(__dirname));

app.post("/upload/:album_id", function (req, res) {
  var album_id = req.params.album_id;
  if (!fs.existsSync("albums/")) {
      fs.mkdirSync("albums/");
  }

  if (!fs.existsSync("albums/" + album_id)) {
      fs.mkdirSync("albums/" + album_id);
  }

  let storageConfig = multer.diskStorage({
      destination: (req, file, cb) =>{
          cb(null, "albums/" + album_id);
      },
      filename: (req, file, cb) =>{
          let ext = path.extname(file.originalname.toLocaleLowerCase());
          if(ext === ".png" || ext === ".jpg" || ext === ".jpeg")
              cb(null, "cover.png");
      }
  });

  let upload = multer({storage:storageConfig}).any();
  upload(req, res, function(err) {
      if(err){
          res.end('error');
          console.log(err);
      }
      else
          res.end('ok');
  });
});
app.post("/upload/:album_id/:track_name", function (req, res) {

  var album_id = req.params.album_id;
  var track_name = req.params.track_name;
  if (!fs.existsSync("albums/" + album_id)) {
      fs.mkdirSync("albums/" + album_id);
  }

  let storageConfig = multer.diskStorage({
      destination: (req, file, cb) =>{
          cb(null, "albums/" + album_id +"/");
      },
      filename: (req, file, cb) =>{
          let ext = path.extname(file.originalname.toLocaleLowerCase());
          if(ext === ".mp3")
              cb(null, track_name + ".mp3");
      }
  });

  let upload = multer({storage:storageConfig}).any();
  upload(req, res, function(err) {
      if(err){
          res.end('error');
          console.log(err);
      }
      else
          res.end('ok');
  });
});
app.listen(3000, ()=>{console.log("Server started")});