/// <reference path="typings/angularjs/angular.d.ts" />
var app = angular.module("myapp", []);
app.controller("main", function ($scope, $http, $timeout) {
    $scope.test = "AngularJS Rocks!";
    $scope.person = {};
    $http.get("/api/Default/1").success(function (data) {
        $timeout(function () {
            $scope.person = data;
            $http.post("/api/Default", $scope.person);
        });
    });
});
//# sourceMappingURL=app.js.map