// jshint esversion:6

import $ from 'jquery';
import Main from './Main';
import fs from 'fs';
import path from 'path';
import React from 'react';
import ReactDOM from 'react-dom';
import injectTapEventPlugin from 'react-tap-event-plugin';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';

window.$ = $;

const muiTheme = getMuiTheme(darkBaseTheme);

//Needed for onTouchTap
//Can go away when react 1.0 release
//Check this repo:
//https://github.com/zilverline/react-tap-event-plugin
injectTapEventPlugin();


/* Scan and load css files */
console.log('scanning css');
var i = 0;
const cssFiles = fs.readdirSync(path.join(__dirname, '..', 'css'));
while (i < cssFiles.length) {
  var file = cssFiles[i];
  var link = document.createElement("link");
  link.href = 'css/' + file;
  link.type = "text/css";
  link.rel = "stylesheet";
  document.getElementsByTagName("head")[0].appendChild(link);
  i++;
}

/* Set the body background color the theme's back color */
$('body').css('background-color', muiTheme.palette.canvasColor);

// Render the main app react component into the app div.
// For more details see: https://facebook.github.io/react/docs/top-level-api.html#react.render
ReactDOM.render(
  <MuiThemeProvider muiTheme={muiTheme}><Main /></MuiThemeProvider>, document.getElementById('appbody'));
