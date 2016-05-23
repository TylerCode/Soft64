var jquery = require(__dirname + '/../jquery-1.12.3.min.js');
import React from 'react';
import ReactDOM from 'react-dom';
import injectTapEventPlugin from 'react-tap-event-plugin';
import Main from './Main'; // Our custom react component
import fs from 'fs';
import path from 'path';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';

const muiTheme = getMuiTheme(darkBaseTheme);

/* Scan and load css files */
console.log('scanning css');
var i = 0;
const cssDir = __dirname + '/../css';
const cssFiles = fs.readdirSync(cssDir);
while (i < cssFiles.length) {
  var file = cssFiles[i];
  var link = document.createElement("link");
  link.href = cssDir + '/' + file;
  link.type = "text/css";
  link.rel = "stylesheet";
  document.getElementsByTagName("head")[0].appendChild(link);
  i++;
}

const bgColor = muiTheme.palette.canvasColor.toString() + ' !important';
$('body').css('background-color', bgColor);


//Needed for onTouchTap
//Can go away when react 1.0 release
//Check this repo:
//https://github.com/zilverline/react-tap-event-plugin
injectTapEventPlugin();

// Render the main app react component into the app div.
// For more details see: https://facebook.github.io/react/docs/top-level-api.html#react.render
ReactDOM.render(
  <MuiThemeProvider muiTheme={muiTheme}><Main /></MuiThemeProvider>, document.getElementById('appbody'));
