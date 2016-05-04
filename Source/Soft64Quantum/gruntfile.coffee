module.exports = (grunt) ->
  grunt.initConfig
    pkg: grunt.file.readJSON('package.json')
    typescript:
      base: 'src/**/*.ts'
      dest: 'build/app.js'
      options:
        module: 'amd'
    mkdir:
      all:
        options:
          create: ['build']
    remove:
      dirList: ['build']

  grunt.loadNpmTasks('grunt-mkdir');
  grunt.loadNpmTasks('grunt-remove');
  grunt.loadNpmTasks('grunt-typescript')

  # Default task.
  grunt.registerTask 'default', ['remove', 'mkdir', 'typescript']
