(function() {
    var AuthenticationController = [
        "$scope", "$http", function ($scope, $http) {

            //CRUD
            var CreateCredentials = function() {
                var credentials = {
                    email: "",
                    password: ""
                };

                return credentials;
            };
            var CreateUserObj = function() {
                var user = {
                    email: "",
                    firstName: "",
                    lastName: "",
                    password: ""
                };

                return user;
            };
            var ExpandMenu = function() {
                $(".main-menu").toggleClass('expanded');
            };
            //Checker
            var FbLogin = function() {
                ezfb.login(function(res) {
                    /**
                 * no manual $scope.$apply, I got that handled
                 */
                    if (res.authResponse) {
                    }
                }, { scope: "email,user_likes" });
            };
            var IsUserLoggedIn = function(isUserLoggedIn) {
                $scope.loggedUser = isUserLoggedIn == "False" ? false : true;
            };
            //Utilities
            var ResetForm = function() {
                $(".alert-box.alert").remove();
                $scope.credentials = CreateCredentials();
                $scope.user = CreateUserObj();
            };
            var SignOutUser = function() {
                $http.post("/Authentication/Signout/", {})
                    .then(function() {}, function() {});
            };

            //CRUD
            $scope.credentials = CreateCredentials();
            $scope.user = CreateUserObj();
      
            //Checker
            $scope.fblogin = FbLogin;
            $scope.isUserLoggedIn = IsUserLoggedIn;

            //Utilities
            $scope.resetForm = ResetForm;
            $scope.signOutUser = SignOutUser;
            $scope.expandMenu = ExpandMenu;

        }
    ];
    app.controller("AuthenticationController", AuthenticationController);
}());