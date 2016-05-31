'use strict';

var fs = require('fs');
var path = require('path');

/* Setup paths variables to main folers */
var src = path.join('./', '.');
var bin = path.join('./', 'app');

/* ES6 Setup */
var es6_src = path.join(src, 'es6');
var es6_src_files = fs.readdirSync(es6_src);
var es6_bin_files = (function () {
  var x = {};
  for (var file in es6_src_files) x[path.join(bin, es6_src_files[file])] = path.join(es6_src, es6_src_files[file]);
  console.log(x);
  return x; })();


module.exports = function (grunt) {
	grunt.initConfig({
		pkg: grunt.file.readJSON('package.json')
    , babel: {
      build: {
      options: {
        sourceMap: true
        , presets: ['babel-preset-es2015']
      }
      , files: es6_bin_files
    }
  }
	});

  grunt.loadNpmTasks('grunt-babel');
  grunt.registerTask('build', ['babel:build']);
  grunt.registerTask('default', ['build']);
};
