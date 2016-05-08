os = require('os');

module.exports = (grunt) ->
  grunt.initConfig
    pkg: grunt.file.readJSON('package.json')

    # uglify:
    #   my_target:
    #     options:
    #       sourceMap: true
    #     files:
    #       'build/app/main.js' : ['src/**/*.js']

    # copy:
    #   all:
    #     files: [
    #       {
    #         expand: true
    #         src: [ '*.html' ]
    #         dest: 'build/app'
    #       }]

    clean:
      folder: 'build'

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

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-asar2');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-electron-packager');

  # Default task.
  grunt.registerTask 'default', ['clean', 'electron-packager']
