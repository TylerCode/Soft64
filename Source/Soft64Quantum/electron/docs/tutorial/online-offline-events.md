# Online/Offline Event Detection

Online and offline event detection can be implemented in the renderer process
using standard HTML5 APIs, as shown in the following example.

_main.js_

```javascript
const electron = require('electron');
const app = electron.app;
const BrowserWindow = electron.BrowserWindow;

var onlineStatusWindow;
app.on('ready', function() {
  onlineStatusWindow = new BrowserWindow({ width: 0, height: 0, show: false });
  onlineStatusWindow.loadURL('file://' + __dirname + '/online-status.html');
});
```

_online-status.html_

```html
<!DOCTYPE html>
<html>
<body>
<script>
  var alertOnlineStatus = function() {
    window.alert(navigator.onLine ? 'online' : 'offline');
  };

  window.addEventListener('online',  alertOnlineStatus);
  window.addEventListener('offline',  alertOnlineStatus);

  alertOnlineStatus();
</script>
</body>
</html>
```

There may be instances where you want to respond to these events in the
main process as well. The main process however does not have a
`navigator` object and thus cannot detect these events directly. Using
Electron's inter-process communication utilities, the events can be forwarded
to the main process and handled as needed, as shown in the following example.

_main.js_

```javascript
const electron = require('electron');
const app = electron.app;
const ipcMain = electron.ipcMain;
const BrowserWindow = electron.BrowserWindow;

var onlineStatusWindow;
app.on('ready', function() {
  onlineStatusWindow = new BrowserWindow({ width: 0, height: 0, show: false });
  onlineStatusWindow.loadURL('file://' + __dirname + '/online-status.html');
});

ipcMain.on('online-status-changed', function(event, status) {
  console.log(status);
});
```

_online-status.html_

```html
<!DOCTYPE html>
<html>
<body>
<script>
  const ipcRenderer = require('electron').ipcRenderer;
  var updateOnlineStatus = function() {
    ipcRenderer.send('online-status-changed', navigator.onLine ? 'online' : 'offline');
  };

  window.addEventListener('online',  updateOnlineStatus);
  window.addEventListener('offline',  updateOnlineStatus);

  updateOnlineStatus();
</script>
</body>
</html>
```
