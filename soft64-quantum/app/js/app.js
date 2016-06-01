'use strict';

var _electron = require('electron');

var _electron2 = _interopRequireDefault(_electron);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var app = _electron2.default.app; // jshint esversion:6

var BrowserWindow = _electron2.default.BrowserWindow;
var mainWindow = void 0;

function createWindow() {
  mainWindow = new BrowserWindow({ width: 1024, height: 768 });
  mainWindow.loadURL('file://' + __dirname + '/../index.html');
  mainWindow.webContents.toggleDevTools();

  /* When window is closed, make mainWindow null */
  mainWindow.on('closed', function () {
    mainWindow = null;
  });
}

app.on('ready', createWindow);

app.on('window-all-closed', function () {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', function () {
  if (mainWindow === null) {
    createWindow();
  }
});
//# sourceMappingURL=app.js.map
