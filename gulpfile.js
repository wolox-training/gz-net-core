var gulp = require('gulp'),
    sass = require('gulp-sass'),
    gutil = require('gulp-util');
    pug = require('gulp-pug'),
    ext_replace = require('gulp-ext-replace'),
    del = require('del'),
    vinylPaths = require('vinyl-paths');

function getDestPath(filePath, pugFileFolderName, viewFolderName)
{
    var destPath;

    if(pugFileFolderName != 'undefined' && viewFolderName != 'undefined')
    {
        destPath = filePath.replace(pugFileFolderName, viewFolderName);
    }

    else destPath = filePath;
    
    var folderPathIndex = destPath.lastIndexOf("/");
    destPath = destPath.substring(0, folderPathIndex + 1);

    return destPath;
}

function getFileName(file)
{
    var folderPathIndex = file.lastIndexOf("/");
    return file.substring(folderPathIndex + 1);
}

function compilePugFile(file)
{
    return gulp.src(file)
        .pipe(pug({pretty: true}))
        .pipe(gulp.dest(getDestPath(file, "PugViews", "Views")));
}

function renameHtmlFile(file)
{
    return gulp.src(file)
        .pipe(ext_replace('.cshtml'))
        .pipe(gulp.dest(getDestPath(file)));
}

function deleteHtmlFile(file)
{
    file = file.replace(".cshtml", ".html");
    return gulp.src(file)
        .pipe(vinylPaths(del));
}

// Tasks
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
    }).on('change', function(event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
        compilePugFile(event.path);
    })
});

gulp.task('watch:replace', function () {
    gulp.watch([
        'Views/**/*',
        '!Views/**/*.cshtml'
    ], {
        interval: 250
    }).on('change', function(event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
        renameHtmlFile(event.path);
    })
});

gulp.task('watch:delete', function () {
    gulp.watch([
        'Views/**/*.cshtml'
    ], {
        interval: 250
    }).on('change', function (event) {
        gutil.log(`File ${event.path} was ${event.type}, running task.`);
        deleteHtmlFile(event.path);
    })
});

gulp.task( 'default', ['watch:sass', 'watch:pug', 'watch:replace', 'watch:delete'] );
