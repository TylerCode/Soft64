# Library imports
os = require('os');
fs = require('fs');
path = require('path');
expandHomeDir = require('expand-home-dir')

# Setup paths variables to main folers
src = path.resolve('Source/Soft64Quantum');
electron_bin = path.resolve(src, 'Soft64-' + os.platform() + '-' + os.arch());
app_dir = path.resolve(src, 'app');
app_bin = path.resolve('Binary');

# ES6 Setup
js_bin = path.resolve(app_dir, 'js');
js_src = path.resolve(src, 'es6');
js_files = fs.readdirSync(js_src);
js_files_babel = {};

i = 0
while i < js_files.length
  file = js_files[i]
  js_files_babel[path.resolve(js_bin, file)] = path.resolve(js_src, file)
  i++

# LESS Setup
less_src = path.resolve(src, 'less');
less_bin = path.resolve(app_dir, 'css');
less_files = fs.readdirSync(less_src);
less_files_plugin = {};

i = 0
while i < less_files.length
  file = less_files[i]
  less_files_plugin[path.resolve(less_bin, file.slice(0, -5) + '.css')] = path.resolve(less_src, file)
  i++

# Grunt main entry point
module.exports = (grunt) ->
  grunt.initConfig
    pkg: grunt.file.readJSON('package.json')

    babel:
      build:
        options:
          sourceMap: true;
          presets: ['babel-preset-es2015', 'babel-preset-react']
        files: js_files_babel

    copy:
      build:
        files: [
          {'src': 'Soft64.png', 'dest': electron_bin}
        ]
      deploy:
        files: [
          {
            expand: true
            cwd: electron_bin
            src: [ '**' ]
            dest: app_bin
          },
        ]

    clean:
      options:
        force: true
      build: [electron_bin, js_bin, less_bin]
      deploy: app_bin

    'electron-packager':
      build:
        options:
          platform  : os.platform()
          arch      : os.arch()
          dir       : path.resolve(src, 'app')
          out       : src
          icon      : 'Soft64.png'
          name      : 'Soft64'
          version   : '0.36.7'
          overwrite : true
          asar : true

    chmod:
      options:
        mode: '+x'
      deploy:
        src: app_bin + '/Soft64'

    auto_install:
        local: {}
        subdir:
          options:
            cwd: app_dir
            stdout: true
            stderr: true
            failOnError: true

    less:
      build:
        files: less_files_plugin


  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-electron-packager');
  grunt.loadNpmTasks('grunt-chmod');
  grunt.loadNpmTasks('grunt-auto-install');
  grunt.loadNpmTasks('grunt-babel');
  grunt.loadNpmTasks('grunt-contrib-less');

  grunt.registerTask 'build', ['auto_install', 'clean:build', 'copy:build', 'babel:build', 'less:build', 'electron-packager']
  grunt.registerTask 'deploy', ['clean:deploy', 'copy:deploy', 'chmod:deploy']
  grunt.registerTask 'default', ['build', 'deploy']
