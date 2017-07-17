var gulp = require("gulp"),
    fs = require("fs"),
    sass = require("gulp-sass");

// other content removed

gulp.task("sass", function () {
    return gulp.src('assets/styles/site.scss')
        .pipe( sass( { outputStyle: 'compressed' } ) )
        .pipe(gulp.dest('wwwroot/css'));
});

var sassSrc = 'assets/styles/site.scss';

gulp.task( 'automate', function() {
    gulp.watch( [ sassSrc ] );
});

gulp.task( 'default', ['automate'] );
