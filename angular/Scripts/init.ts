/// <reference path="typings/angularjs/angular.d.ts" />

module dns {
	export var ngApp: ng.IModule = angular.module("myapp", ["ui.router"])

	class Routing {
		public static $inject = ["$stateProvider"];
		constructor(private $stateProvider: ng.ui.IStateProvider) {
			$stateProvider.state(
					"root", {
						"url": "/",
						"controller": "MainController",
						"templateUrl": "/Scripts/views/main.html"
					}
				);
		}
	}
}
