os = require('os');
path = require('path');
electron_bin = path.resolve('./build/Soft64' + '-' + os.platform() + '-' + os.arch());
app_bin = path.resolve('../../Binary');

module.exports = (grunt) ->
  grunt.initConfig
    pkg: grunt.file.readJSON('package.json')

    copy:
      deploy:
        files: [
          {
            expand: true
            cwd: electron_bin
            src: [ '**' ]
            dest: app_bin
          }]

    clean:
      options:
        force: true
      build: 'build'
      deploy: app_bin

    'electron-packager':
      build:
        options:
          platform  : os.platform()
          arch      : os.arch()
          dir       : 'src'
          out       : 'build'
          #icon      : './test/app/recursos/icon'
          name      : 'Soft64'
          version   : '0.36.7'
          overwrite : true
          asar : true

    chmod:
      options:
        mode: '+x'
      deploy:
        src: app_bin + '/Soft64'

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-electron-packager');
  grunt.loadNpmTasks('grunt-chmod');

  grunt.registerTask 'build', ['clean:build', 'electron-packager']
  grunt.registerTask 'deploy', ['clean:deploy', 'copy:deploy', 'chmod:deploy']
  grunt.registerTask 'default', ['build', 'deploy']