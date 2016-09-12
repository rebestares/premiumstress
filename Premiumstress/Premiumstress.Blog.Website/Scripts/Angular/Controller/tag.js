(function () {
    var TagController = [
        "$scope", "TagService", function ($scope, TagService) {

            var GetAllTags = function() {
                var onSuccess = function(response) {
                    $scope.tags = response.data;
                };
                var onFail = function () { };

                TagService.getTags(onSuccess, onFail);
            };

            $scope.getAllTags = GetAllTags;
        }
    ];
    app.controller("TagController", TagController);
}());