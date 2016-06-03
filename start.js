var os = require('os');
var exec = require('child_process').exec;
var path = require('path');
var child;
var cmd = 'Soft64/Soft64'

if (os.platform() == 'win32') {
  console.log("Win32 Platform");
  cmd = path.resolve('Soft64/Soft64.exe');
}

child = exec(cmd, function (error, stdout, stderr) {
  console.log('stdout: ' + stdout);
  console.log('stderr: ' + stderr);
  if (error !== null) {
    console.log('exec error: ' + error);
  }
});
