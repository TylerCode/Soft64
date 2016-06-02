/* Library imports */
var os = require('os');
var fs = require('fs');
var path = require('path');
var argv = require('yargs').argv;

var debugMode = typeof argv.Debug != 'undefined';
var csBuildType = debugMode ? 'Debug' : "Release";

console.log("Debug Mode: ", debugMode);
console.log("CS Build Type: ", csBuildType);

/* platform string */
var platform = os.platform() + '-' + os.arch();

/* Main Binary path */
var dir_bin = 'Soft64';

function mkList_es6(dir_module, dir_target) {
  var src = path.join(dir_module, 'es6');
  var files = fs.readdirSync(src);
  var x = {};
  for (var file in files) {
    x[path.join(dir_target, files[file])] =
      path.join(src, files[file]);
  }
  return x;
}

function mkList_Less(dir_module, dir_target) {
  var src = path.join(dir_module, 'less');
  var files = fs.readdirSync(src);
  var x = {};
  for (var file in files) {
    x[path.join(dir_target, files[file].slice(0, -5) + 'css')] =
      path.join(src, files[file]);
  }
  return x;
}

function Projects () {
  this.quantum = {};
  this.quantum.dir_module = 'soft64-quantum';
  this.quantum.dir_app = path.join(this.quantum.dir_module, 'app');
  this.quantum.dir_electron = path.join(this.quantum.dir_module, 'Soft64-' + platform);
  this.quantum.es6 = mkList_es6(this.quantum.dir_module, path.join(this.quantum.dir_app, 'js'));
  this.quantum.less = mkList_Less(this.quantum.dir_module, path.join(this.quantum.dir_app, 'css'));
  this.cli = {};
  this.cli.dir_module = 'soft64-cli';
  this.cli.dir_app = path.join(this.cli.dir_module, 'app');
  this.cli.es6 = mkList_es6(this.cli.dir_module, path.join(this.cli.dir_app));
  this.cs = {};
  this.cs.dir_module = 'cs';
  this.cs.sln = path.join(this.cs.dir_module, 'Soft64CoreLibraries.sln');
}

var projects = new Projects();

module.exports = function (grunt) {
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    /* ES6 Grunt Plugin */
    babel: {
      quantum: {
        options: {
          sourceMap: true,
          presets: ['babel-preset-es2015', 'babel-preset-react']
        },
        files: projects.quantum.es6
      },
      cli: {
        options: {
          sourceMap: true,
          presets: ['babel-preset-es2015']
        },
        files: projects.cli.es6
      }
    },

    /* Copy grunt plugin */
    copy: {
      "quantum-build": {
        files: [
          {'src': 'Soft64.png', 'dest': projects.quantum.dir_electron }
        ]
      },
      "quantum-deploy": {
        files: [
          {
            expand: true,
            cwd: projects.quantum.dir_electron,
            src: ["**"],
            dest: dir_bin}
        ]
      },
      "cli-deploy": {
        files: [
          {
            expand: true,
            cwd: projects.cli.dir_app,
            src: ["**"],
            dest: path.join(dir_bin, "cli")
          },
          {
            expand: true,
            cwd: projects.cli.dir_module,
            src: ["package.json"],
            dest: path.join(dir_bin, "cli")
          }
        ]
      }
    },

    /* Clean grunt plugin */
    clean: {
      options: {
        force: true
      },
      "quantum-build": [
        projects.quantum.dir_electron,
        path.join(projects.quantum.dir_app, 'js'),
        path.join(projects.quantum.dir_app, 'css')],
      "bin": dir_bin,
      "cli-build": projects.cli.dir_app
    },

    /* Electron packager grunt plugin */
    'electron-packager': {
      build: {
        options: {
          platform: os.platform(),
          arch: os.arch(),
          dir: projects.quantum.dir_app,
          out: projects.quantum.dir_module,
          icon: 'Soft64.png',
          name: "Soft64",
          overwrite: true,
          asar: true,
          version: "0.37.8"
        }
      }
    },

    /* Chmod grunt plugin */
    chmod: {
      options: {
        mode: '+x'
      },
      deploy: {
        src: dir_bin + '/Soft64'
      }
    },

    /* auto npm update grunt plugin */
    auto_install: {
      quantum: {
        options: {
            cwd: projects.quantum.dir_app,
            stdout: true,
            stderr: true,
            failOnError: true,
            npm: '--production'
        }
      },
      cli: {
        options: {
          cwd: path.join(dir_bin, 'cli'),
          stdout: true,
          stderr: true,
          failOnError: true,
          npm: '--production'
        }
      }
    },

    /* Less grunt plugin */
    less: {
      build: {
        files:  projects.quantum.less
      }
    },

    /* Msbuild plugin */
    msbuild: {
        dev: {
            src: projects.cs.sln,
            options: {
                projectConfiguration: csBuildType,
                 targets: 'Build',
                 version: 12,
                 maxCpuCount: 4,
                buildParameters: {
                    WarningLevel: 2
                },
                nodeReuse:false,
                verbosity: 'quiet'
            }
        }
    },
  });

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-electron-packager');
  grunt.loadNpmTasks('grunt-chmod');
  grunt.loadNpmTasks('grunt-auto-install');
  grunt.loadNpmTasks('grunt-babel');
  grunt.loadNpmTasks('grunt-contrib-less');
  grunt.loadNpmTasks('grunt-msbuild');

  /* Quantum based tasks */
  grunt.registerTask('quantum-build', ['auto_install:quantum', 'clean:quantum-build', 'copy:quantum-build', 'babel:quantum', 'less:build', 'electron-packager']);
  grunt.registerTask('quantum-deploy', ['copy:quantum-deploy', 'chmod:deploy']);
  grunt.registerTask('quantum', ['quantum-build', 'quantum-deploy']);

  /* CLI based tasks */
  grunt.registerTask('cli-build', ['clean:cli-build', 'babel:cli']);
  grunt.registerTask('cli-deploy', ['copy:cli-deploy', 'auto_install:cli']);
  grunt.registerTask('cli', ['cli-build', 'cli-deploy']);

  /* CS tasks */
  grunt.registerTask('cs', ['msbuild:dev']);

  /* general tasks */
  grunt.registerTask('clean-bin', ['clean:bin']);

  grunt.registerTask('default', ['clean-bin', 'cs', 'cli', 'quantum']);
};
