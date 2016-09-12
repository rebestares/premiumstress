(function() {
    var UserPosterController = [
        "$scope", "Service", "Utilities", "UserService", "$http", function($scope, Service, Utilities, UserService, $http) {

            var GetBlogsPosted = function(userID) {
                var onSuccess = function(response) {
                    $scope.listOfBlogs = response.data;
                    Utilities.closeLoading("#user-wrapper", "#Loading-container");
                };
                var onFail = function(reason) {};

                var postObj = {
                    Url: "/User/GetBlogsPostedOfUser/",
                    Param: { userID: $scope.userProfile.ID },
                    Success: onSuccess,
                    Fail: onFail
                };


                Service.Post(postObj);
            };

            var GetUserProfile = function(id) {
                GetUser(id);
                $scope.isEdit = true;
                Utilities.showElement(".profile-card", ".user-table", "hide");
            };

            var UpdateProfile = function() {

                var onSuccess = function(response) {
                    if (response.data) {
                        GetUser($scope.userProfile.ID);
                    }
                };

                var onFail = function(reason) {

                };

                $scope.userProfile.About = $("#aboutTextArea").html();

                var postObj = {
                    Url: "/User/UpdateUserProfile/",
                    Param: { model: $scope.userProfile },
                    Success: onSuccess,
                    Fail: onFail
                };

                var onSuccessUploadImage = function(response) {
                    $scope.userProfile.ImageLinks = response;
                    Service.Post(postObj);
                };
                var onFailUploadImage = function(reason) {
                };
                UploadImage($scope.isEdit, onSuccessUploadImage, onFailUploadImage);

                function UploadImage(isThisEdit, onSuccessUploadImage, onFailUploadImage) {
                    var uploadUrl = "/User/UploadUserImage";
                    var fd = new FormData();
                    fd.append("file", $scope.userProfile.CurrentImageLink);

                    if (isThisEdit) {
                        if ($scope.userProfile.ImageLinks != null) {
                            fd.append("fileName", $scope.userProfile.ImageLinks.FullImageLink);
                            fd.append("thumbnailFilename", $scope.userProfile.ImageLinks.ThumbnailImageLink);
                        }
                    }

                    $http.post(uploadUrl, fd, {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).success(onSuccessUploadImage, onFailUploadImage);
                }
            };
            var UploadUserPic = function(event) {
                $scope.userProfile.CurrentImageLink = event.target.files[0];
            };

            var BackToAllUser = function() {
                Utilities.showElement(".user-table", ".profile-card", "hide");
            };

            var GetUser = function(id) {
                var onSuccess = function(response) {
                    $scope.userProfile = response.data;
                };

                var onFail = function() {
                };
                UserService.getUser(onSuccess, onFail, id);
            };
            var GetAllUsers = function() {
                var onSuccess = function(response) {
                    $scope.allUsersList = response.data;
                };

                var onFail = function(reason) {};

                UserService.getAllUsers(onSuccess, onFail);
            };
            var SetUser = function(userPoster) {
                $scope.userProfile = createUserPoster();
                $scope.userProfile = userPoster;
            };

            var CheckEmail = function(onSuccessArg, onFailArg) {
                var onSuccess;
                var onFail;

                if (onSuccessArg != undefined && onFailArg != undefined) {
                    onSuccess = onSuccessArg;
                    onFail = onFailArg;
                } else {
                    onSuccess = function(response) {
                        $scope.emailIsUnique = response.data;
                    };
                    onFail = function() {
                    };
                }

                UserService.isEmailUnique(onSuccess, onFail, $scope.addUser.email.$modelValue);
            };

            var RegisterUser = function() {
                if (!$("#save-btn").hasClass("disabled")) {
                    var onSuccess = function(response) {
                        var alertSuccess = "<div data-alert class=\"alert-box success radius\">You have successfully added a new user.</div>";
                        var alertFail = "<div data-alert class=\"alert-box alert radius\">There is a problem on your request make sure there are no errors on your fields.</div>";

                        var isSuccess = response.data;
                        if (isSuccess) {
                            $scope.user = null;

                            if (!$(".alert-box").hasClass("success")) {
                                $("#addUserModal").append(alertSuccess).foundation();

                                GoToStep(1);
                            }

                        } else {
                            if (!$(".alert-box").hasClass("alert")) {
                                $("#addUserModal").append(alertFail).foundation();
                            }
                        }
                    };
                    var onFail = function() {
                    };
                    if ($scope.addUser.$invalid) {
                        var alert = "<div data-alert class=\"alert-box alert main radius\">Please fill in the required fields.</div>";
                        if (!$(".alert-box").hasClass("alert")) {
                            $("#addUserModal").append(alert).foundation();
                        }
                    } else {
                        $(".alert-box.alert.main").remove();
                        UserService.addUser(onSuccess, onFail, $scope.user);
                    }
                }
            };

            var ProceedToStep = function(step) {
                var elemsToToggle = [];
                var regx = /[^\s@]+@[^\s@]+\.[^\s@]+/;

                if (step === 2) {
                    $scope.alertClass = "alert";
                    if ($scope.user != undefined) {
                        if ($scope.user.email !== "") {
                            if (regx.test($scope.user.email)) {
                                var onSuccess = function(response) {
                                    $scope.emailIsUnique = response.data;
                                    if ($scope.emailIsUnique) {
                                        if ($scope.user.password === $scope.user.confirmPassword) {

                                            $scope.alertClass = "";
                                            $scope.alertMessage = "";
                                            GoToStep(2);
                                 
                                        } else {
                                            $scope.alertMessage = "Password doesn't match";
                                        }
                                    }
                                };
                                var onFail = function() {};
                                CheckEmail(onSuccess, onFail);
                            } else {
                                $scope.alertMessage = "Please enter a valid email address";
                            }
                        } else {
                            $scope.alertMessage = "Please supply email field";
                        }

                    } else {
                        $scope.alertMessage = "Please fill up all fields";
                    }
                }
                if (step === 1) {
                    GoToStep(1);
                }
            };

            function GoToStep(stepNum) {
                var elemsToToggle = [];
                if (stepNum === 1) {
                    elemsToToggle = [
                        ["#step-1", "#step-2", "hide"],
                        ["#next-btn", "#prev-btn", "disabled"],
                        ["#details-label", "#account-label", "selected-label"],
                        [null, "#save-btn", "disabled"]
                    ];

                } else if (stepNum === 2) {
                    elemsToToggle = [
                        ["#step-2", "#step-1", "hide"],
                        ["#prev-btn", "#next-btn", "disabled"],
                        ["#account-label", "#details-label", "selected-label"],
                        ["#save-btn", null, "disabled"]
                    ];
                }

                $.each(elemsToToggle, function(pos, element) {
                    Utilities.showElements(element);
                });
            }

            $scope.emailIsUnique = true;
            $scope.getAllUsers = GetAllUsers;
            $scope.registerUser = RegisterUser;
            $scope.checkEmail = CheckEmail;
            $scope.updateProfile = UpdateProfile;
            $scope.uploadUserPic = UploadUserPic;
            $scope.backToAllUser = BackToAllUser;
            $scope.setUser = SetUser;
            $scope.getUserProfile = GetUserProfile;

            //Utilities
            $scope.proceedToStep = ProceedToStep;
        }
    ];

    app.controller("UserController", UserPosterController);
}());