(function () {
    "use strict";

    angular.module("ContactModule", [
        // Angular modules 
        "ngRoute"

        // Custom modules 

        // 3rd Party Modules
    ]).config([
        "$routeProvider", "$locationProvider",
        function ($routeProvider, $locationProvider) {
            $routeProvider

                //Default view for Contact Index page
                .when("/", {
                    controller: "ContactController",
                    templateUrl: "Template/Contact/ContactUs.html",
                    caseInsensitiveMatch: true
                }) //Routing for Contact page
                .when("/us/", {
                    controller: "ContactController",
                    templateUrl: "Template/Contact/ContactUs.html",
                    caseInsensitiveMatch: true
                }).when("/specific/", {
                    controller: "ContactController",
                    templateUrl: "Template/Contact/ContactSpecificPerson.html",
                    caseInsensitiveMatch: true
                })
                .otherwise({
                    redirectTo: "/"
                });
        }
    ]);
})();

