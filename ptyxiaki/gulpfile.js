const gulp = require('gulp');
//const del = require('del');

const libraries = [
  "node_modules/font-awesome/**/*",
  "node_modules/bootstrap/**/*",
  "node_modules/jquery/**/*"
];

gulp.task('default', ['copy']);

gulp.task('copy', /*['clean'],*/ () => {
  return gulp.src(libraries, { base: 'node_modules' })
    .pipe(gulp.dest('wwwroot/lib'));
});

//gulp.task('clean', () => {

//});