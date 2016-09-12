(function() {

    var ContactController = [
        "$scope", "Service", "$timeout", "Utilities", function($scope, Service, $timeout, Utilities) {
            var CreateEmailObj = function() {
                var Email = {
                    Name: "",
                    Body: "",
                    EmailAddress: ""
                };
                return Email;
            };
            var GetStaffProfiles = function() {
                var onSuccess = function(response) {
                    $scope.staff = response.data;
                    $timeout(function() { Utilities.closeLoading("#contact-us-wrapper", "#Loading-container") }, 500);
                };
                var onFail = function(reason) {

                };
                var postObj = {
                    Url: "/Contact/GetStaffProfiles/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Get(postObj);
            };

            var SendEmailToAdmin = function() {
                $scope.transactionInProgress = true;
                var onSuccess = function(response) {
                    $scope.transactionInProgress = false;
                    if (response.data) {
                        $("#successAlert").foundation("reveal", "open");
                        $scope.email = CreateEmailObj();
                    }
                };
                var onFail = function(reason) {

                };
                var postObj = {
                    Url: "/Admin/SendToAdmin/",
                    Param: { email: $scope.email },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            };

            var CloseAlertSuccessAlert = function() {
                $("#successAlert").foundation("reveal", "close");
            };
            $scope.getStaffProfiles = GetStaffProfiles;
            $scope.sendEmailToAdmin = SendEmailToAdmin;
            $scope.email = CreateEmailObj();
            $scope.closeSuccessAlert = CloseAlertSuccessAlert;
            $scope.transactionInProgress = false;
        }
    ];

    app.controller("ContactController", ContactController);
}());