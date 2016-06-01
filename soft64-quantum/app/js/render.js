'use strict';

var _jquery = require('jquery');

var _jquery2 = _interopRequireDefault(_jquery);

var _Main = require('./Main');

var _Main2 = _interopRequireDefault(_Main);

var _fs = require('fs');

var _fs2 = _interopRequireDefault(_fs);

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactDom = require('react-dom');

var _reactDom2 = _interopRequireDefault(_reactDom);

var _reactTapEventPlugin = require('react-tap-event-plugin');

var _reactTapEventPlugin2 = _interopRequireDefault(_reactTapEventPlugin);

var _darkBaseTheme = require('material-ui/styles/baseThemes/darkBaseTheme');

var _darkBaseTheme2 = _interopRequireDefault(_darkBaseTheme);

var _getMuiTheme = require('material-ui/styles/getMuiTheme');

var _getMuiTheme2 = _interopRequireDefault(_getMuiTheme);

var _MuiThemeProvider = require('material-ui/styles/MuiThemeProvider');

var _MuiThemeProvider2 = _interopRequireDefault(_MuiThemeProvider);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

// jshint esversion:6

window.$ = _jquery2.default;

var muiTheme = (0, _getMuiTheme2.default)(_darkBaseTheme2.default);

//Needed for onTouchTap
//Can go away when react 1.0 release
//Check this repo:
//https://github.com/zilverline/react-tap-event-plugin
(0, _reactTapEventPlugin2.default)();

/* Scan and load css files */
console.log('scanning css');
var i = 0;
var cssFiles = _fs2.default.readdirSync(_path2.default.join(__dirname, '..', 'css'));
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
(0, _jquery2.default)('body').css('background-color', muiTheme.palette.canvasColor);

// Render the main app react component into the app div.
// For more details see: https://facebook.github.io/react/docs/top-level-api.html#react.render
_reactDom2.default.render(_react2.default.createElement(
  _MuiThemeProvider2.default,
  { muiTheme: muiTheme },
  _react2.default.createElement(_Main2.default, null)
), document.getElementById('appbody'));
//# sourceMappingURL=render.js.map
