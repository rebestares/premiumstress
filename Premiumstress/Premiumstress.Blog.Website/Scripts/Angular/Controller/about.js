(function () {
    var AboutController = ['Utilities', '$scope', function (Utilities, $scope) {
        var ShowFooter = function () {
            Utilities.closeLoading('#about-wrapper', '#Loading-container');
        }

        $scope.showFooter = ShowFooter;
    }];

    app.controller("AboutController", AboutController);
}());

