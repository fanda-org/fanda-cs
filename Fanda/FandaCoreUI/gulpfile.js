"use strict";
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
//var uglify = require('gulp-uglify');
//var concat = require('gulp-concat');
var rimraf = require("rimraf");
var merge = require('merge-stream');

//gulp.task("minify", function () {
//    var streams = [
//        gulp.src(["wwwroot/js/*.js", '!wwwroot/js/tour*.js', '!wwwroot/js/contact.js'])
//            .pipe(uglify())
//            .pipe(concat("wilderblog.min.js"))
//            .pipe(gulp.dest("wwwroot/lib/site")),
//        gulp.src(["wwwroot/js/contact.js"])
//            .pipe(uglify())
//            .pipe(concat("contact.min.js"))
//            .pipe(gulp.dest("wwwroot/lib/site"))
//    ];

//    return merge(streams);
//});

// Dependency Dirs
var deps = {
    "@coreui/coreui": {
        "dist/**/*": ""
    },
    "@coreui/icons": {
        "css/*": "css",
        "fonts/*": "fonts"
    },
    "@coreui/coreui-plugin-chartjs-custom-tooltips": {
        "dist/js/*": "js"
    },
    "select2": {
        "dist/**/*": ""
    },
    "select2-bootstrap4-theme": {
        "dist/**/*": ""
    },
    "jquery-ajax-unobtrusive": {
        "dist/**/*": ""
    },
    "simple-line-icons": {
        "css/*": "css",
        "fonts/*": "fonts"
    },
    "bootstrap4c-custom-switch": {
        "dist/**/*": ""
    },
    "datatables.net": {
        "js/*": "js"
    },
    "datatables.net-bs4": {
        "css/*": "css",
        "js/*": "js"
    },
    "perfect-scrollbar": {
        "dist/**/*": ""
    },
    "pace-progressbar": {
        "pace.js": "",
        "pace.min.js": "",
        "themes/**/*": "themes"
    },
    "jsgrid": {
        "dist/**/*": ""
    },
    "chart.js": {
        "dist/Chart.js": "",
        "dist/Chart.min.js": ""
    },
    "bootstrap": {
        "dist/**/*": ""
    },
    "jquery": {
        "dist/**/*": ""
    },
    "jquery-validation": {
        "dist/**/*": ""
    },
    "jquery-validation-unobtrusive": {
        "dist/**/*": ""
    },
    "popper.js": {
        "dist/umd/**/*": ""
    },
    "sweetalert": {
        "dist/**/*": ""
    }
};

gulp.task("clean", function (cb) {
    return rimraf("wwwroot/vendor/", cb);
});

gulp.task("copy-node_modules", function () {
    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/vendor/" + prop + "/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);
});

gulp.task("default", gulp.series('clean', 'copy-node_modules'));