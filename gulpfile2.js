var gulp = require('gulp'),
    sass = require('gulp-sass'),
    gutil = require('gulp-util');
    pug = require('gulp-pug'),
    ext_replace = require('gulp-ext-replace'),
    del = require('del'),
    vinylPaths = require('vinyl-paths');

gulp.task('pug', function() {
    return gulp.src('PugViews/**/*.pug')
        .pipe(pug({pretty: true}))
        .pipe(gulp.dest('Views/'));
});

gulp.task('replace', function() {
    return gulp.src('Views/**/*.html')
        .pipe(ext_replace('.cshtml'))
        .pipe(gulp.dest('Views'));
});

gulp.task('delete', function () {
    return gulp.src(['Views/**/*.html'])
        .pipe(vinylPaths(del));
});

gulp.task('sass', function () {
    return gulp.src('assets/styles/site.scss')
        .pipe( sass( { outputStyle: 'compressed' } ) )
        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('watch:sass', function () {
    gulp.watch([
        './assets/styles/*.scss',
        '!./bin/**/*',
        '!./obj/**/*',
    ], {
        interval: 250
    }, ['sass']).on('change', function (event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
    })
})

gulp.task('watch:pug', function () {
    gulp.watch([
        'PugViews/**/*.pug'
    ], {
        interval: 250
    },
    ['pug']).on('change', function(event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
    })
});

gulp.task('watch:replace', function () {
    gulp.watch([
        'Views/**/*'
    ], {
        interval: 250
    },
    ['replace']).on('add', function(event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
    })
});

gulp.task('watch:delete', function () {
    gulp.watch([
        'Views/**/*.cshtml'
    ], {
        interval: 250
    },
    ['delete']).on('add', function (event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
    })
});