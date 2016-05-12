os = require('os');
path = require('path');
expandHomeDir = require('expand-home-dir')
electron_bin = path.resolve('./build/Soft64' + '-' + os.platform() + '-' + os.arch());
app_bin = path.resolve('../../Binary');

module.exports = (grunt) ->
  grunt.initConfig
    pkg: grunt.file.readJSON('package.json')

    babel:
      build:
        options:
          sourceMap: true;
          presets: ['babel-preset-es2015', 'babel-preset-react']
        files: [
              'app/js/app.js': 'es6/app.js'
            , 'app/js/render.js' : 'es6/render.js'
          ]

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
      build: ['build', 'app/js']
      deploy: app_bin

    'electron-packager':
      build:
        options:
          platform  : os.platform()
          arch      : os.arch()
          cwd       : "./"
          dir       : './app'
          out       : 'build'
          icon      : 'Soft64.png'
          ignore    : ''
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
            cwd: 'app'
            stdout: true
            stderr: true
            failOnError: true

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-electron-packager');
  grunt.loadNpmTasks('grunt-chmod');
  grunt.loadNpmTasks('grunt-auto-install');
  grunt.loadNpmTasks('grunt-babel');

  grunt.registerTask 'build', ['auto_install', 'clean:build', 'copy:build', 'babel:build', 'electron-packager']
  grunt.registerTask 'deploy', ['clean:deploy', 'copy:deploy', 'chmod:deploy']
  grunt.registerTask 'default', ['build', 'deploy']
