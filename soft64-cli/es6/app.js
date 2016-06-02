// jshint esversion:6

import repl from 'repl';
import colors from 'colors';
//import edge from 'edge';
import util from 'util';

console.log("Soft64 Emulator REPL CLI 1.0".magenta);

var help = (function() {
  console.log("Soft64 command help: ".magenta);
  console.log("start() - start emulator".magenta);
  console.log("exit() - exit program".magenta);
  console.log("help() - print this help".magenta);
});

help();


var context = repl.start({
  prompt: 'EMU # '.magenta,
  terminal: true,
  replMode: repl.REPL_MODE_STRICT,
  ignoreUndefined: true,
  useGlobal: true
}).context;

context.help = help;

context.start = function () {
  console.log("Start emulator");

  return "Success";
};

context.exit = function () {
  process.exit(0);
};
