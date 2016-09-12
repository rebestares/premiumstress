(function() {
    var MainController = [
        "$scope", "$rootScope", function($scope, $rootScope) {
            $rootScope.isLoaded = false;
        }
    ];

    app.controller("MainController", MainController);
}());