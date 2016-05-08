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

    # mkdir:
    #   all:
    #     options:
    #       create: ['build']
    clean:
      folder: 'build'

    asar:
      all:
        files:
          'build/Soft64/resources/app.asar': ['build/main.js', 'src/index.html']
          'build/Soft64/resources/app_modules.asar': ['node_modules/']
    copy:
      all:
        files: [
          {
            expand: true
            cwd: 'node_modules/electron-prebuilt/dist'
            src: [ '**' ]
            dest: 'build/Soft64/'
          }]

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-asar2');
  grunt.loadNpmTasks('grunt-contrib-copy');

  # Default task.
  grunt.registerTask 'default', ['clean', 'copy', 'uglify', 'asar']
