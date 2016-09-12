(function () {
    var CategoryController = [
        "$scope", "CategoryService", function ($scope, CategoryService) {


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

            var InsertNewCategory = function () {
                var onSuccess = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        //ResetCategoryMode();
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
                     //   ResetCategoryMode();
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

            $scope.saveCategory = SaveCategory;
            $scope.getCategories = GetCategories;
            $scope.setCategoryMode = SetCategoryMode;
        }
    ];
    app.controller("CategoryController", CategoryController);
}());