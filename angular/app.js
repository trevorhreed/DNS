var dns;
(function (dns) {
    dns.ngApp = angular.module("myapp", ["ui.router"]);
    var Routing = (function () {
        function Routing($stateProvider) {
            this.$stateProvider = $stateProvider;
            $stateProvider.state("root", {
                "url": "/",
                "controller": "MainController",
                "templateUrl": "/Scripts/views/main.html"
            });
        }
        Routing.$inject = ["$stateProvider"];
        return Routing;
    })();
})(dns || (dns = {}));
var dns;
(function (dns) {
    var MainController = (function () {
        function MainController($scope, $http, $timeout) {
            this.$scope = $scope;
            this.$http = $http;
            this.$timeout = $timeout;
            $scope.test = "AngularJS Rocks!";
            $scope.person = {};
            $http.get("/api/Default/1").success(function (data) {
                $timeout(function () {
                    $scope.person = data;
                    $http.post("/api/Default", $scope.person);
                });
            });
        }
        MainController.$inject = ["$scope", "$http", "$timeout"];
        return MainController;
    })();
    dns.ngApp.controller("MainController", MainController);
})(dns || (dns = {}));
//# sourceMappingURL=app.js.map