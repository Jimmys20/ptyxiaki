var gulp = require('gulp');
var del = require('del');

var libraries = [
  'node_modules/bootstrap/**/*',
  'node_modules/datatables.net/**/*',
  'node_modules/datatables.net-bs4/**/*',
  'node_modules/datatables.net-responsive/**/*',
  'node_modules/datatables.net-responsive-bs4/**/*',
  'node_modules/jquery/**/*',
  'node_modules/jquery-datetimepicker/**/*',
  'node_modules/jquery-validation/**/*',
  'node_modules/jquery-validation-unobtrusive/**/*',
  'node_modules/select2/**/*'
];

gulp.task('clean', function () {
  return del('wwwroot/lib');
});

gulp.task('copy', function () {
  return gulp.src(libraries, { base: 'node_modules' }).pipe(gulp.dest('wwwroot/lib'));
});

gulp.task('default', gulp.series('clean', 'copy'));
