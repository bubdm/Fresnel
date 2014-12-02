var app = angular.module("myApp", []);

app.controller("MyController", function ($scope) {

    $scope.message = { title: "Hello World!!" };

});


app.controller("MyOtherController", function ($scope) {

    $scope.message = { title: "Goodbye!!" };

});



//function MyController($scope) {
//    $scope.message = { title: "Hello World!!" };
//}
