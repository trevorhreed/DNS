var gulp        = require('gulp'),
    sass        = require('gulp-sass'),
    minifyCss   = require('gulp-minify-css'),
    minifyHtml  = require('gulp-minify-html'),
    nghtml		= require('gulp-angular-templatecache'),
	ts			= require('gulp-typescript'),
    uglify      = require('gulp-uglify'),
    concat      = require('gulp-concat'),
    sourcemaps  = require('gulp-sourcemaps'),
    watch       = require("gulp-watch"),
    rename      = require('gulp-rename'),
    size        = require('gulp-size'), // logs out the size of combined streams
    rev         = require('gulp-rev'),
    gutil       = require('gulp-util'),
    fs          = require('fs'),
    path        = require('path'),
    del         = require('del'),
    mkdrp       = require('mkdirp');

var SRC_ROOT    = 'src/',
    DST_ROOT    = 'www/',
    SASS_PATH   = '/**/*.scss',
    HTML_PATH   = '/**/*.html',
    JS_PATH     = '/**/*.js';

function getAppFolders(dir) {
    return fs.readdirSync(dir)
        .filter(function(file) {
            return fs.statSync(path.join(dir, file)).isDirectory();
        });
}

function watchSass(glob, appName){
    return function(e){
        gutil.log('SASS Change');
        
        var dstFolder = DST_ROOT + appName + '/';
        del(dstFolder + '*.css');
        del(dstFolder + '*.css.map');
        
        gulp.src(glob)
            .pipe(sourcemaps.init())
            .pipe(sass())
            .pipe(concat(appName + '.css'))
            .pipe(minifyCss())
            .pipe(rev())
            .pipe(sourcemaps.write('.'))
            .pipe(gulp.dest(dstFolder));
    }
}

function watchHtml(glob, appName){
    return function(e){
        gutil.log('HTML Change');
        
        gulp.src(glob)
            .pipe(minifyHtml())
            .pipe(nghtml({
                module: appName, 
                base: function(file){
                    var name = file.path,
                        start = name.lastIndexOf(path.sep) + 1,
                        end = name.lastIndexOf('.');
                    if(end < 0) end = file.path.length - 1;
                    return name.substring(start, end);
                }
            }))
            .pipe(gulp.dest(SRC_ROOT + appName + "/"));
    }
}

function watchJs(glob, appName){
    return function(e){
        gutil.log('  JS Change');
        
        var dstFolder = DST_ROOT + appName + '/';
        del(dstFolder + '*.js');
        del(dstFolder + '*.js.map');
        
    	gulp.src(glob)
            .pipe(sourcemaps.init())
            .pipe(concat(appName + '.js'))
            .pipe(uglify())
            .pipe(rev())
            .pipe(sourcemaps.write('.'))
            .pipe(gulp.dest(dstFolder));
    }
}

gulp.task('default', function(){
    
    getAppFolders(SRC_ROOT)
        .forEach(function(appName, index){
            var sassGlob = SRC_ROOT + appName + SASS_PATH,
                htmlGlob = SRC_ROOT + appName + HTML_PATH,
                jsGlob   = SRC_ROOT + appName + JS_PATH;
        
            watchSass(sassGlob, appName)();
            watchHtml(htmlGlob, appName)();
            watchJs(jsGlob, appName)();
        
            watch(sassGlob, watchSass(sassGlob, appName));
            watch(htmlGlob, watchHtml(htmlGlob, appName));
            watch(jsGlob, watchJs(jsGlob, appName));
        });
    
    /*
    
    TODO:
        
        investigate csso()
        rev()
        inject()
        move jinja2 views to src/, copy to dest
        remove rename, size
    
    */
});