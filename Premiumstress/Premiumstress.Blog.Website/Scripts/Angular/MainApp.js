
var app = angular.module("BucketList", [
        "textAngular", "angular-loading-bar", "ngTouch", "ngSanitize", "ngRoute"
])
    .config([
        "$provide", function ($provide) {


            // this demonstrates how to register a new tool and add it to the default toolbar
            $provide.decorator("taOptions", [
                "$delegate", function (taOptions) {
                    // $delegate is the taOptions we are decorating
                    // here we override the default toolbars and classes specified in taOptions.
                    taOptions.toolbar = [
                        ["h1", "h2", "h3", "h4", "h5", "h6", "p", "quote"],
                        ["bold", "italics", "underline", "ul", "ol", "clear"],
                        ["justifyLeft", "justifyCenter", "justifyRight"],
                        ["html", "insertImage", "insertLink"]
                    ];
                    taOptions.classes = {
                        focussed: "focussed",
                        toolbar: "blog-button-group",
                        toolbarGroup: "blog-button-group",
                        toolbarButton: "blog-button",
                        toolbarButtonActive: "active",
                        disabled: "disabled",
                        textEditor: "without-error",
                        htmlEditor: "form-control"
                    };

                    return taOptions; // whatever you return will be the taOptions
                }
            ]);
        }
    ]).config([
        "$routeProvider", "$locationProvider",
        function ($routeProvider, $locationProvider) {
            $routeProvider.
                //Default view for Blog Index page
                when("/", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/AllArticlesPage.html",
                    caseInsensitiveMatch: true
                })

                //Routing for Blog page
                .when("/page/:pageNum", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/AllArticlesPage.html",
                    caseInsensitiveMatch: true
                }).when("/category/:category", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/CategoryPage.html",
                    caseInsensitiveMatch: true
                }).when("/category/:category/page/:categoryPage", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/CategoryPage.html",
                    caseInsensitiveMatch: true
                }).when("/tag/:tagName", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/TagPage.html",
                    caseInsensitiveMatch: true
                }).when("/tag/:tagName/page/:tagPage", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/TagPage.html",
                    caseInsensitiveMatch: true
                }).when("/search/:word", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/Search.html",
                    caseInsensitiveMatch: true
                }).when("/search/:word/page/:searchPage", {
                    controller: "BlogController",
                    templateUrl: "Template/Blog/Search.html",
                    caseInsensitiveMatch: true
                }).when("/author/:userId/:authorName", {
                    controller: "BlogController",
                    templateUrl: "Template/Author/AuthorPage.html",
                    caseInsensitiveMatch: true
                }).when("/author/:userId/:authorName/page/:userPageNum", {
                    controller: "BlogController",
                    templateUrl: "Template/Author/AuthorPage.html",
                    caseInsensitiveMatch: true
                })
                //Settings
                .when("/settings", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Index.html",
                    caseInsensitiveMatch: true

                    //User
                }).when("/settings/user/add", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/User/Add.html",
                    caseInsensitiveMatch: true
                }).when("/settings/user/edit", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/User/Edit.html",
                    caseInsensitiveMatch: true
                }).when("/settings/user/delete", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/User/Delete.html",
                    caseInsensitiveMatch: true
                })
                    //Article
                .when("/settings/article/add", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Article/Add.html",
                    caseInsensitiveMatch: true
                }).when("/settings/article/edit", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Article/Edit.html",
                    caseInsensitiveMatch: true
                }).when("/settings/article/publish", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Article/Publish.html",
                    caseInsensitiveMatch: true
                })
                    //Category
                .when("/settings/category/add", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Category/Add.html",
                    caseInsensitiveMatch: true
                }).when("/settings/category/edit", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Category/Edit.html",
                    caseInsensitiveMatch: true
                }).when("/settings/category/delete", {
                    controller: "BlogController",
                    templateUrl: "Template/Settings/Category/Delete.html",
                    caseInsensitiveMatch: true
                })
                    //Tag
                .when("/settings/tag/add", {
                    controller: "TagController",
                    templateUrl: "Template/Settings/Tags/Add.html",
                    caseInsensitiveMatch: true
                }).when("/settings/tag/edit", {
                    controller: "TagController",
                    templateUrl: "Template/Settings/Tags/Edit.html",
                    caseInsensitiveMatch: true
                }).when("/settings/tag/delete", {
                    controller: "TagController",
                    templateUrl: "Template/Settings/Tags/Delete.html",
                    caseInsensitiveMatch: true
                })

                    //Password
                .when("/settings/password/change", {
                    controller: "AdminController",
                    templateUrl: "Template/Settings/Password/Index.html",
                    caseInsensitiveMatch: true
                })

                .otherwise({
                    redirectTo: "/"
                });



            $locationProvider.html5Mode({
                enabled: true,
                requireBase: false
            });

        }
    ]);
