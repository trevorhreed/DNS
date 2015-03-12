 /// <reference path="../../init.ts" />

module dns {
	interface IMainControllerScope extends ng.IScope {
		test: string;
		person: Object;
	}
	class MainController {
		public static $inject = ["$scope", "$http", "$timeout"];
		constructor(private $scope: IMainControllerScope, private $http: ng.IHttpService, private $timeout: ng.ITimeoutService) {
			$scope.test = "AngularJS Rocks!";
			$scope.person = {};
			$http.get("/api/Default/1").success((data) => {
				$timeout(() => {
					$scope.person = data;
					$http.post("/api/Default", $scope.person);
				});
			});
		}
	}
	ngApp.controller("MainController", MainController);
}