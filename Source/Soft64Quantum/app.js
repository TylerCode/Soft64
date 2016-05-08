"use strict";

const electron = require('electron');
const reactDOM = require('react');
const app = electron.app;
const BrowserWindow = electron.BrowserWindow;
let mainWindow

function createWindow () {
    mainWindow = new BrowserWindow({width: 800, height: 600});
    mainWindow.loadURL('file://' + __dirname + '/index.html');
    mainWindow.webContents.toggleDevTools();
    mainWindow.on('closed', function () {
    mainWindow = null
  })
}

app.on('ready', createWindow)

app.on('window-all-closed', function () {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})

app.on('activate', function () {
  if (mainWindow === null) {
    createWindow()
  }
})

ReactDOM.render(
  <h1>Hello, world!</h1>,
  document.getElementById('example')
);
