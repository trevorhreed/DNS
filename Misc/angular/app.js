var dns;
(function (dns) {
    dns.ngApp = angular.module("myapp");
    var MainController = (function () {
        function MainController($scope, $http, $timeout) {
            this.$scope = $scope;
            this.$http = $http;
            this.$timeout = $timeout;
            $scope.people = [];
            $scope.selectedPersonIndex = null;
            $scope.person = this.NewPerson();
            $http.get("/api/people").success(function (response) {
                if (response.Code == 200) {
                    $scope.people = response.Data;
                }
            });
            $scope.edit = function (index) {
                $scope.person = angular.copy($scope.people[index]);
                $scope.selectedPersonIndex = index;
            };
            $scope.save = function () {
                if ($scope.person.Id == null) {
                    $http.post("/api/people", $scope.person).success(function (response) {
                        $scope.person.Id = response.Guid;
                        $scope.people.push($scope.person);
                        $scope.deselectPerson();
                    });
                }
                else {
                    $http.put("/api/people/" + $scope.person.Id, $scope.person).success(function () {
                        $scope.people[$scope.selectedPersonIndex] = $scope.person;
                        $scope.deselectPerson();
                    });
                }
            };
            $scope.deselectPerson = function () {
                $scope.selectedPersonIndex = $scope.person = null;
            };
            $scope.del = function (index) {
                var person = $scope.people[index];
                if (person) {
                    $http.delete("/api/people/" + person.Id).success(function () {
                        $scope.people.splice(index, 1);
                    });
                }
            };
        }
        MainController.prototype.NewPerson = function () {
            return {
                Name: "",
                Email: "",
                Phones: [
                    ""
                ],
                Address: {
                    Street: "",
                    Apt: "",
                    City: "",
                    State: "",
                    Zip: ""
                }
            };
        };
        MainController.$inject = ["$scope", "$http", "$timeout"];
        return MainController;
    })();
    dns.ngApp.controller("main", MainController);
})(dns || (dns = {}));
//# sourceMappingURL=app.js.map