module.exports = (grunt) ->
  grunt.initConfig
    pkg: grunt.file.readJSON('package.json')
    # concat:
    #   options:
    #     sourceMap: true
    #   dist:
    #     src: ['src/**/*.js']
    #     dest: 'build/main.js'
    uglify:
      my_target:
        options:
          sourceMap: true
        files:
          'build/main.js' : ['src/**/*.js']

    mkdir:
      all:
        options:
          create: ['build']
    remove:
      dirList: ['build']

  grunt.loadNpmTasks('grunt-mkdir');
  grunt.loadNpmTasks('grunt-remove');
  #grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  # Default task.
  grunt.registerTask 'default', ['remove', 'mkdir', 'uglify']
