module.exports = (grunt) ->
  'use strict'
  require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);
  _ = require('underscore')

  jsLibs = [
    'jquery/dist/jquery.min.js',
    'owl.carousel/dist/owl.carousel.min.js',
    'jquery-mousewheel/jquery.mousewheel.min.js',
    'nanogallery/dist/jquery.nanogallery.min.js',
    'fingerprintjs2/dist/fingerprint2.min.js'
  ].map((path) ->
    return '../wwwroot/js/' + path;
  );

  skinList = ['default', 'dark', 'help', 'pony', 'soft']
  skinConcatConfigCssFiles = {}

  skinStylusConfig =
    options:
      compress: false
      paths: ['blocks/']
      import: [
        'config.styl',
        'mixins/i-mixins__clearfix.styl'
      ]

  skinStylusItemConfig =
    expand: true
    cwd: 'blocks/'
    src: [
      '**/*.styl',
      '!mixins/**/*.styl',
      '!config*.styl',
    ]
    dest: 'blocks'
    ext: '.css'

  for skinName in skinList
    do (skinName) ->
      skinStylusConfig[skinName] = _.extend({}, skinStylusItemConfig, {
        options: {
          import: ['config.styl', 'mixins/**/*.styl', 'config_' + skinName + '.styl']
        }, ext: '_' + skinName + '.css'
      })
      skinConcatConfigCssFiles['../wwwroot/css/style_' + skinName + '.css'] = [
        '../wwwroot/js/owl.carousel/dist/assets/owl.carousel.min.css',
        '../wwwroot/js/nanogallery/dist/css/nanogallery.min.css',
        '../wwwroot/js/nanogallery/dist/css/nanogallery.woff.min.css'
        'blocks/**/*_' + skinName + '.css',
        'assets/**/*.css',

      ]


  @initConfig

    connect:
      server:
        options:
          port: 8000,
          base: '../wwwroot'
          middleware: (connect, options) ->
            return [
# Serve static files.
              connect.static(options.base)
# Show only html files and folders.
              connect.directory(options.base, {
                hidden: false, icons: true, filter: (file) ->
                  return /\.html/.test(file) || !/\./.test(file);
              })
            ]


    copy:
      images:
        files: [{
          expand: true
          flatten: true
          cwd: 'blocks',
          src: ['**/*.{png,jpg,jpeg,gif,svg}', '!**/*_sprite.{png,jpg,jpeg,gif}']
          dest: '../wwwroot/img'
        }]



    clean:
      pubimages:
        src: [
          "../wwwroot/img/*.{png,gif,jpg,jpeg,svg}"
        ]


    imagemin:
      options:
        optimizationLevel: 5
      dist:
        files: [
          {
            expand: true
            cwd: '../wwwroot/img/'
            src: '**/*.{png,jpg,jpeg}'
            dest: '../wwwroot/img/'
          },
          {
            expand: true
            cwd: '../wwwroot'
            src: '*.{png,jpg,jpeg,ico}'
            dest: '../wwwroot'
          },
        ]

    concat:
      js:
        src: ['assets/**/*.js', 'blocks/**/*.js'],
        dest: '../wwwroot/js/script.js'

      jsLibs:
        src: jsLibs.concat('../wwwroot/js/script.min.js'),
        dest: '../wwwroot/js/script.min.js'

      css:
        files:
          skinConcatConfigCssFiles

    uglify:
      dist:
        files:
          '../wwwroot/js/script.min.js': ['<%= concat.js.dest %>']


    jshint:
      files: [
        'blocks/**/*.js'
      ]
      options:
        curly: true
        eqeqeq: true
        eqnull: true
# quotmark: true
        undef: true
        unused: false

        browser: true
        jquery: true
        globals:
          console: true


    watch:
      options:
        livereload: false
        spawn: false

      stylus:
        options:
          livereload: true
        files: [
          'blocks/**/*.styl'
        ]
        tasks: ['stylus', 'newer:concat:css', 'newer:autoprefixer']

      js:
        options:
          livereload: true
        files: [
          '../wwwroot/js/**/*.js',
          'blocks/**/*.js'
        ]
        tasks: ['newer:concat:js']

      grunt:
        options:
          livereload: true
        files: [
          'Gruntfile.coffee'  # auto reload gruntfile config
        ]
        tasks: ['newer:concat:js', 'stylus', 'newer:concat:css', 'newer:autoprefixer']

    

      data:
        options:
          livereload: true
        files: ['data/**/*.json']
        tasks: []

      images:
        files: [
          'blocks/**/*.{png,jpg,jpeg,gif,svg}'
        ]
        tasks: ['copy:images']


    stylus:
      skinStylusConfig


    autoprefixer:
      no_dest:
        options:
          browsers: ['> 1%', 'last 2 versions', 'Firefox ESR', 'Opera 12.1', 'ie 9']       #default
        expand: true
        cwd: '../wwwroot/css/'
        src: '*.css'
        dest: '../wwwroot/css/'


    cssmin: {
      my_target: {
        files: [{
          expand: true,
          cwd: '../wwwroot/css/',
          src: ['*.css'],
          dest: '../wwwroot/css/min/',
          ext: '.css'
        }]
      }
    }

    open:
      mainpage:
        path: 'http://localhost:8000/p-index.html'

  @registerTask('default', ['concat:js', 'stylus', 'newer:concat:css', 'autoprefixer'])
  @registerTask('livereload', ['default', 'connect', 'open', 'watch'])
  @registerTask('publish', ['default', 'uglify', 'concat:jsLibs', 'cssmin'])
