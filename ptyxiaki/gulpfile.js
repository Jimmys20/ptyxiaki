var gulp = require('gulp');
//const del = require('del');

var libraries = [
  "node_modules/font-awesome/**/*",
  "node_modules/bootstrap/**/*",
  "node_modules/jquery/**/*",
  "node_modules/popper.js/**/*",
  "node_modules/datatables.net/**/*",
  "node_modules/datatables.net-bs4/**/*"
];


gulp.task('copy', function() {
  return gulp.src(libraries, { base: 'node_modules' })
    .pipe(gulp.dest('wwwroot/lib'));
});

//gulp.task('default', ['copy']);

//gulp.task('clean', () => {

//});