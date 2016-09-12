(function () {
    var AdminController = [
        "$scope", "Service", "$http", "Utilities",
        "$timeout", "UserService", "CategoryService", "GetLink",
        function ($scope, Service, $http, Utilities,
            $timeout, UserService, CategoryService, GetLink) {

            var createUserProfile = function () {
                var userProfile = {
                    Id: "",
                    FirstName: "",
                    LastName: "",
                    Email: "",
                    Password: "",
                    Birthday: "",
                    DateJoined: "",
                    ActivityID: "",
                    ImageLink: "",
                    About: "",
                    CurrentImageLink: "",
                    ImageLinks: {
                        fullImageLink: "",
                        thumbnailImageLink: "",
                        mediumImageLink: ""
                    }
                };

                return userProfile;
            };

            var GetUserProfile = function (userObject) {
                var onSuccess = function (response) {
                    $scope.userProfile = response.data;

                    Utilities.closeLoading("#settings-wrapper", "#Loading-container");
                };

                var onFail = function () {
                };

                UserService.getUserProfile(onSuccess, onFail);
            };

            var EditProfile = function (isEdit) {
                $scope.isEdit = isEdit ? true : false;
            };
            var SavePassword = function () {

                var onSuccess = function (data) {
                    var isAuthenticated = data.data;
                    if (isAuthenticated) {
                        if ($scope.newPassword === $scope.confirmPassword) {
                            var onSuccess = function (data) {
                                if (data.data) {
                                    $scope.changePassClass = "success";
                                    $scope.changePasswordNotifier = "Successfully changed your password.";

                                    $scope.oldPassword = "";
                                    $scope.newPassword = "";
                                    $scope.confirmPassword = "";
                                }
                            };
                            var onFail = function (reason) { };

                            UserService.changeNewPassword(onSuccess, onFail, $scope.userProfile.Email, $scope.newPassword);
                        } else {
                            $scope.changePasswordNotifier = "New password doesn't match.";
                            $scope.changePassClass = "alert";
                        }
                    } else {
                        $scope.changePasswordNotifier = "Wrong password.";
                        $scope.changePassClass = "alert";
                    }
                };
                var onFail = function (reason) { };

                if ($scope.oldPassword && $scope.newPassword && $scope.confirmPassword != null) {
                    UserService.authenticateUser(onSuccess, onFail, $scope.userProfile.Email, $scope.oldPassword);
                } else {
                    $scope.changePasswordNotifier = "Please supply all the necessary fields.";
                    $scope.changePassClass = "alert";
                }
            };

            var SaveCategory = function (mode) {
                switch (mode) {
                    case "Add":
                        InsertNewCategory();
                        break;
                    case "Edit":
                        UpdateCategory();
                        break;
                    case "Delete":
                        DeleteCategory();
                        break;
                }
            };
            var ResetCategoryMode = function () {
                $scope.isAddCategory = false;
                $scope.isEditCategory = false;
                $scope.isDeleteCategory = false;
                $scope.categoryMode = "";
            };
            var SetCategoryMode = function (mode) {
                switch (mode) {
                    case "add":
                        $scope.isAddCategory = true;
                        $scope.categoryMode = "Add";
                        break;
                    case "edit":
                        $scope.isEditCategory = true;
                        $scope.categoryMode = "Edit";
                        break;
                    case "delete":
                        $scope.isDeleteCategory = true;
                        $scope.categoryMode = "Delete";
                }
            };

            //Authentication
            var AuthenticateUser = function () {
                var onSuccess = function (response) {
                    var authentication = response.data;
                    $scope.authenticated = authentication;

                    if (authentication) {
                        window.location.href = "settings";
                    } else {

                    }
                };
                var onFail = function () {
                };
                $http.post("/User/AuthenticateUser/", { email: $scope.credentials.email, password: $scope.credentials.password })
                    .then(onSuccess, onFail);
            };

            //CRUD
            var EditPassword = function (isEdit) {
                $scope.isEditPassword = isEdit ? true : false;

            };
            var InsertNewCategory = function () {
                var onSuccess = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        ResetCategoryMode();
                        $("#successCategory").foundation("reveal", "open");
                        GetCategories();
                    }
                };

                var onFail = function () { };


                var category = {
                    ID: 0,
                    Name: $scope.newCategoryItem,
                    Position: 0,
                    Type: "blog",
                    DisplayOrder: $scope.categoryList.length + 1
                };
                if ($scope.newCategoryItem !== "") {
                    CategoryService.insertCategory(onSuccess, onFail, category);
                }
            };
            var DeleteCategory = function () {
                var onSuccess = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        ResetCategoryMode();
                        $("#successCategory").foundation("reveal", "open");
                        GetCategories();
                    } else {
                        $("#failCategory").foundation("reveal", "open");
                        $scope.failAlertBody = "Please make sure there are no blog(s) saved in the chosen category.";
                    }
                };

                var onFail = function () { };
                if ($scope.categoryItem != null) {
                    CategoryService.deleteCategory(onSuccess, onFail, $scope.categoryItem);
                }
            };
            var UpdateCategory = function () {
                var onSuccess = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        $scope.isEditCategory = false;
                        $("#successCategory").foundation("reveal", "open");
                        GetCategories();
                    }
                };

                var onFail = function () { };
                if ($scope.newCategoryItem != "") {
                    CategoryService.updateCategory(onSuccess, onFail, $scope.newCategoryList);
                }

            };

            var GetCategories = function () {
                var onSuccess = function (response) {
                    $scope.categoryList = response.data;
                    $scope.newCategoryList = angular.copy($scope.categoryList);
                    $scope.newCategoryItem = "";
                };
                var onFail = function () {
                };
                CategoryService.getCategories(onSuccess, onFail);
            };
            var CloseSuccessAlert = function () {
                $("#successCategory").foundation("reveal", "close");
            };

            //Utilities
            var ShowLoginForm = function () {
                $timeout(function () { Utilities.closeLoading("#login-container-wrapper", "#Loading-container") }, 500);
            };

            var GetSingleBlogLink = function (id, title) {
                return GetLink.SingleBlog(id, title);
            };

            var ShowPanel = function (panel) {

                if ($scope.currentPanel !== "")
                    $($scope.currentPanel).addClass("hide");

                $scope.currentPanel = panel;

                $(panel).removeClass("hide");
            };



            //Authentication
            $scope.authenticated = true;
            $scope.authenticateUser = AuthenticateUser;

            //User Profile
            $scope.userProfile = createUserProfile();
            $scope.editProfile = EditProfile;
            $scope.getUserProfile = GetUserProfile;
            $scope.isEdit = false;

            //Password
            $scope.editPassword = EditPassword;
            $scope.isEditPassword = false;
            $scope.savePassword = SavePassword;

            //Category
            $scope.isEditCategory = false;
            $scope.isAddCategory = false;
            $scope.isDeleteCategory = false;
            $scope.categoryMode = "";
            $scope.newCategoryList = "";
            $scope.categoryList = "";
            $scope.setCategoryMode = SetCategoryMode;
            $scope.resetCategoryMode = ResetCategoryMode;

            //functions
            $scope.getCategories = GetCategories;
            $scope.saveCategory = SaveCategory;
            $scope.successFullyAddedCategory = false;

            //Utilities
            $scope.currentPanel = "";
            $scope.closeSuccessAlert = CloseSuccessAlert;
            $scope.showLoginForm = ShowLoginForm;
            $scope.getSingleBlogLink = GetSingleBlogLink;
            $scope.showPanel = ShowPanel;

            $scope.loadCharts = function() {
                // Load the Visualization API and the corechart package.
                google.charts.load('current', { 'packages': ['corechart'] });

                // Set a callback to run when the Google Visualization API is loaded.
                google.charts.setOnLoadCallback(drawChart);

                // Callback that creates and populates a data table,
                // instantiates the pie chart, passes in the data and
                // draws it.
                function drawChart() {

                    // Create the data table.
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Topping');
                    data.addColumn('number', 'Slices');
                    data.addRows([
                        ['Mushrooms', 3],
                        ['Onions', 1],
                        ['Olives', 1],
                        ['Zucchini', 1],
                        ['Pepperoni', 2]
                    ]);

                    // Set chart options
                    var options = {
                        'title': 'How Much Pizza I Ate Last Night',
                        'width': 700,
                        'height': 500
                    };

                    // Instantiate and draw our chart, passing in some options.
                    var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
                    chart.draw(data, options);

                    // Instantiate and draw our chart, passing in some options.
                    var chart = new google.visualization.PieChart(document.getElementById('other_div'));
                    chart.draw(data, options);
                }
            };


        }
    ];

    app.controller("AdminController", AdminController);
}());