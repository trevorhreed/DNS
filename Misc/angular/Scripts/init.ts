/// <reference path="typings/angularjs/angular.d.ts" />

module dns {
	export var ngApp: ng.IModule = angular.module("myapp")

	/*
	 * Main Controller
	 */

	interface IMainControllerScope extends ng.IScope {
		test: string;
		person: Person;
		selectedPersonIndex: number;
		people: Person[];

		save(): void;
		deselectPerson(): void;
		edit(index: number): void;
		del(index: number): void;
	}
	class MainController {
		public static $inject = ["$scope", "$http", "$timeout"];
		constructor(private $scope: IMainControllerScope, private $http: ng.IHttpService, private $timeout: ng.ITimeoutService) {
			$scope.people = [];
			$scope.selectedPersonIndex = null;
			$scope.person = this.NewPerson();

			$http.get("/api/people").success((response: PeopleResponse) => {
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
					$http.post("/api/people", $scope.person)
						.success(function (response: NewPersonResponse) {
						$scope.person.Id = response.Guid;
						$scope.people.push($scope.person);
						$scope.deselectPerson();
					});
				} else {
					$http.put("/api/people/" + $scope.person.Id, $scope.person)
						.success(function () {
						$scope.people[$scope.selectedPersonIndex] = $scope.person;
						$scope.deselectPerson();
					});
				}
			};
			$scope.deselectPerson = function () {
				$scope.selectedPersonIndex = $scope.person = null;
			};

			$scope.del = function (index) {
				var person: Person = $scope.people[index];
				if (person) {
					$http.delete("/api/people/" + person.Id)
					.success(function () {
						$scope.people.splice(index, 1);
					});
				}
			};
		}
		
		private NewPerson(): Person {
			return <Person>{
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
			}
		} 
	}
	ngApp.controller("main", MainController);
	
	/*
	 * Models
	 */

	interface Response {
		Code: number;
		Message: string;
	}
	interface PeopleResponse extends Response {
		Data: Person[];
	}
	interface NewPersonResponse extends Response {
		Guid: string;
	}
	interface Address {
		Street: string;
		Apt?: string;
		City: string;
		State: string;
		Zip: string;
	}
	interface Person {
		Id?: string;
		Name: string;
		Email: string;
		Phones: string[];
		Address: Address;
	}
}
