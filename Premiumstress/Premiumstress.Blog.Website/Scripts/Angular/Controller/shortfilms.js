(function() {
    var ShortFilmsController = [
        "$scope", "Service", "Utilities", "UploadImageService", function($scope, Service, Utilities, UploadImageService) {
            var createShortFilmObj = function() {
                var shortFilm = {
                    Title: "",
                    Content: "",
                    ShortFilmCategory: {
                        ID: 1,
                        Name: ""
                    }
                };

                var imageLink = {
                    fullImageLink: "",
                    thumbnailImageLink: "",
                    mediumImageLink: ""
                };
                var ShortFilmObj = {
                    ShortFilm: shortFilm,
                    ImageLink: imageLink
                };
                return ShortFilmObj;
            };

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

            var GetUserProfile = function(userObject) {
                var onSuccess = function(response) {
                    $scope.userProfile = response.data;
                    GetBlogsPosted();
                };

                var onFail = function() {
                };
                var postObj = {
                    Url: "/Settings/GetUserProfile/",
                    Param: { userObj: userObject },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            };

            var GetShortFilms = function() {

                var onSuccess = function(response) {
                    $scope.listOfShortFilms = response.data;

                    $scope.shortFilmCount = [];
                    while ($scope.listOfShortFilms.length) {
                        $scope.shortFilmCount.push($scope.listOfShortFilms.splice(0, 3));
                    }
                };
                var onFail = function(reason) {};

                var postObj = {
                    Url: "/ShortFilms/GetShortFilms/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);


                Utilities.closeLoading("#shortfilm-wrapper", "#Loading-container");

            };
            var ShowFooter = function() {
                Utilities.closeLoading("#add-shortfilm-wrapper", "#Loading-container");
            };

            var AddNewShortFilm = function(isEdit) {
                var onSuccess = function(response) {
                    console.log(response);
                    //$scope.transactionInProgress = false;
                    //var blogObj = response.data.blog;
                    //var isSuccess = (isEdit) ? response.data.isSuccess : response.data;
                    //if (isSuccess) {
                    //    var alert = '<div data-alert class="alert-box success radius">You have successfully added a new blog.<a href="#" class="close">&times;</a></div>';
                    //    if (!$('.alert-box').hasClass('success')) {
                    //        $('.alert-holder').append(alert).foundation();
                    //    }

                    //    if (isEdit) {
                    //        var goToLink = GetSingleBlogLink(blogObj.ID, blogObj.Title);
                    //        window.location.href = goToLink;
                    //    }
                    //} else {
                    //    $('.alert-box.success').remove();
                    //    var alert = '<div data-alert class="alert-box alert radius server-error">There is a problem in your request.<a href="#" class="close">&times;</a></div>';
                    //    if (!$('.alert-box').hasClass('alert')) {
                    //        $('.alert-holder').append(alert).foundation();
                    //    }
                    //}

                    $scope.shortFilmObj = createShortFilmObj();
                    $scope.htmlVariable = "";
                    $scope.addShortFilmForm.$setPristine();
                    return true;
                };
                var onFail = function(reason) {
                }; //Fetch blog content

                $scope.shortFilmObj.ShortFilm.Content = $scope.htmlVariable;

                //Upload Image Service
                var onSuccessUploadImage = function(response) {
                    $scope.shortFilmObj.ImageLink = response;

                    var postObj = {
                        Url: "/ShortFilms/AddShortFilms/",
                        Param: { shortfilm: $scope.shortFilmObj },
                        Success: onSuccess,
                        Fail: onFail
                    };

                    Service.Post(postObj);
                };
                var onFailUploadImage = function(reason) {};

                var UploadImageObj = {
                    IsEdit: false,
                    OnSuccess: onSuccessUploadImage,
                    OnFail: onFailUploadImage,
                    Service: "/ShortFilms/UploadShortFilmImage",
                    CurrentImageLink: $scope.currentImageLink,
                    FullImageLink: "",
                    ThumbnailImageLink: ""
                };

                UploadImageService(UploadImageObj);
            };

            $scope.shortFilmObj = createShortFilmObj();
            $scope.getShortFilms = GetShortFilms;
            $scope.showFooter = ShowFooter;
            $scope.addNewShortFilm = AddNewShortFilm;
            $scope.uploadFile = function(event) {
                $scope.currentImageLink = event.target.files[0];
            };
        }
    ];

    app.controller("ShortFilmsController", ShortFilmsController);
}());