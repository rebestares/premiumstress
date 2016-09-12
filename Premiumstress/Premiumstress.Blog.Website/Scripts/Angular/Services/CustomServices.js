(function () {

    var Service = [
        "$http", function ($http) {

            var PostFunction = function (postObj) {
                $http.post(postObj.Url, postObj.Param)
                    .then(postObj.Success, postObj.Fail);
            };
            var GetFunction = function (postObj) {
                $http.get(postObj.Url, postObj.Param)
                    .then(postObj.Success, postObj.Fail);
            };
            var JsonPFunction = function (postObj) {
                $http.jsonp(postObj.Url).success(postObj.Success);
            };

            var AjaxObj = {
                Post: PostFunction,
                Get: GetFunction,
                JSONP: JsonPFunction
            };
            return AjaxObj;
        }
    ];
    var GetLink = [
        function () {
            return {
                Category: function (category) {
                    category = formatLink(category);
                    return "/category/" + category + "";
                },
                SingleBlog: function (id, title) {
                    title = formatLink(title);
                    return "article/" + id + "/" + title;
                }
            };

            function formatLink(link) {
                if (link != undefined)
                    link = link.split(" ").join("-").toLowerCase();
                return link;
            }
        }
    ];
    var Utilities = [
        "$rootScope", "Service", function ($rootScope, Service) {

            function toggleElements(elementToHide,elementToShow,classToToggle) {
                $(elementToHide).addClass(classToToggle);
                $(elementToShow).removeClass(classToToggle);
            };

            return {
                showElements: function (element) {
                    var elToShow = element[0];
                    var elToHide = element[1];
                    var classToToggle = element[2];
                    toggleElements(elToHide, elToShow, classToToggle);
                },
                showElement: function (elToShow, elToHide, classToToggle) {
                    toggleElements(elToHide, elToShow, classToToggle);
                },
                closeLoading: function (wrapper, container) {
                    //Make the content visible
                    $(wrapper).removeClass("hide-content").addClass("show-content");
                    //Make the footer visible
                    $("#footer-container").removeClass("hide-content").addClass("show-content");
                    //Remove the loading container
                    $(container).remove();
                    $rootScope.isLoaded = true;
                },
                alertBox: function (type, message) {
                    var alert, modalType;

                    if (type == "success")
                        modalType = "success-reveal-class";

                    alert = "<div id=\"custom-alertbox\"";
                    alert += "class=\"" + modalType + " small\" data-reveal aria-labelledby=\"modalTitle\" aria-hidden=\"true\" role=\"dialog\">";
                    alert += "<a class=\"close-reveal-modal-success\" aria-label=\"Close\" ng-click=\"closeAlertBox()\">&#215;</a>";
                    alert += "<h2 id=\"modalTitle\">Success!</h2><p class=\"lead\">" + message + "</p></div>";

                    $(".alertbox-container").html(alert);
                },
                alertDiv: function (type, message, holder) {
                    var alertDiv;
                    alertDiv = "<div data-alert class=\"alert-box " + type + " radius\">" + message + "<a href=\"#\" class=\"close\">&times;</a></div>";

                    holder.append(alertDiv).foundation();
                }
            };
        }
    ];
    var UploadImageService = [
        "$http", function ($http) {
            var uploadImage = function (UploadImageObj) {
                var fd = new FormData();
                fd.append("file", UploadImageObj.CurrentImageLink);

                if (UploadImageObj.IsEdit) {
                    fd.append("fileName", UploadImageObj.FullImageLink);
                    fd.append("thumbnailFilename", UploadImageObj.ThumbnailImageLink);
                }

                $http.post(UploadImageObj.Service, fd, {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).success(UploadImageObj.OnSuccess, UploadImageObj.OnFail);
            };
            return uploadImage;
        }
    ];

    var BlogService = ["$rootScope", "Service", "$http", function ($rootScope, Service, $http) {
        return {
            getBlogs: function (onSuccess, onFail, param) {
                var postObj = {
                    Url: "/Blog/GetBlogs/",
                    Param: {
                        pageNumber: param.pageNumber,
                        module: param.module,
                        sortProperty: param.sortProperty,
                        sortOrder: param.sortOrder
                    },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            getSingleBlog:function(onSuccess, onFail, param) {
                var postObj = {
                    Url: "/Blog/GetSingleBlog/",
                    Param: { id: param.ID, isEdit: param.IsEdit },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            promoteBlog:function(onSuccess, onFail, id) {
                var postObj = {
                    Url: "/Blog/PromoteBlog/",
                    Param: {id:id},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            updateBlog: function (onSuccess, onFail, blog) {
                var postObj = {
                    Url: "/Blog/UpdateBlog/",
                    Param: { model: blog },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            insertBlog: function (onSuccess, onFail, blog) {
                var postObj = {
                    Url: "/Blog/InsertBlog/",
                    Param: { blog: blog },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            deleteBlog:function(onSuccess, onFail, blog) {
                var postObj = {
                    Url: "/Blog/DeleteBlog/",
                    Param: { blog: blog },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            getFeatured: function (onSuccess, onFail) {
                var postObj = {
                    Url: "/Blog/GetFeatured/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            findBlog: function (onSuccess, onFail,param) {
                var postObj = {
                    Url: "/Blog/FindBlogs/",
                    Param: { word: param.word, page: param.page, module: param.module },
                    Success: onSuccess,
                    Fail: onFail
                };

                Service.Post(postObj);
            },
            getBlogsByCategory: function(onSuccess, onFail, param) {
                var postObj = {
                    Url: "/Blog/GetBlogsByCategory/",
                    Param: {
                        pageNumber: param.pageNumber,
                        categoryName: param.categoryName
                    },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            getBlogsByUserId: function (onSuccess, onFail, param) {
                var postObj = {
                    Url: "/Blog/GetBlogsByUserId/",
                    Param: {
                        pageNumber: param.page,
                        userId: param.userId,
                        authorName: param.authorName
                    },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            getBlogsByTag: function (onSuccess, onFail, param) {
                var postObj = {
                    Url: "/Blog/GetBlogsByTag/",
                    Param: { pageNumber: param.page, tag: param.tag },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            getPromotedBlog: function (onSuccess, onFail) {
                var postObj = {
                    Url: "/Blog/GetPromotedBlog/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Get(postObj);
            },
            getSuggestedBlogs: function (onSuccess, onFail,param) {
                var postObj = {
                    Url: "/Blog/GetSuggestedBlogs/",
                    Param: {
                        numOfBlogs: param.numOfBlogs,
                        blogId: param.id
                    },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            getBlogTags: function (onSuccess, onFail) {
                var postObj = {
                    Url: "/Blog/GetTags/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            addComment: function (onSuccess, onFail, comment) {
                var postObj = {
                    Url: "/Blog/AddComment/",
                    Param: { comment: comment },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            uploadBlogImage: function (onSuccess, onFail, formdata) {
                $http.post("/Blog/UploadBlogImage", formdata, {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).success(onSuccess, onFail);
            },
            approveBlog:function(onSuccess, onFail, param) {
                var postObj = {
                    Url: "/Blog/ApproveBlog/",
                    Param: { id: param.id, approvalStatus: param.approvalStatus },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            }
        }
    }];
    var UserService = [
        "$rootScope", "Service", function($rootScope, Service) {
            return {
                getUserProfile: function (onSuccess, onFail) {
                    var postObj = {
                        Url: "/User/GetCurrentUserProfile/",
                        Param: {},
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                },
                getUser: function (onSuccess, onFail, id) {
                    var postObj = {
                        Url: "/User/GetUser/",
                        Param: { id: id },
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                },
                getAllUsers: function (onSuccess, onFail) {
                    var postObj = {
                        Url: "/User/GetUsers/",
                        Param: {},
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                },
                authenticateUser: function (onSuccess, onFail, email, password) {
                    var postObj = {
                        Url: "/User/AuthenticateUser/",
                        Param: { email: email, password: password },
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                },
                changeNewPassword: function (onSuccess, onFail, email, password) {
                    var postObj = {
                        Url: "/User/ChangePassword/",
                        Param: { email: email, password: password },
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                },
                isEmailUnique: function (onSuccess, onFail, email) {
                    var postObj = {
                        Url: "/User/IsEmailUnique/",
                        Param: { email: email },
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                },
                addUser: function (onSuccess, onFail, user) {
                    var postObj = {
                        Url: "/User/AddUser/",
                        Param: { model: user },
                        Success: onSuccess,
                        Fail: onFail
                    };
                    Service.Post(postObj);
                }
            }
        }
    ];
    var SocialService =  ["$rootScope", "Service", function($rootScope, Service) {
        return {
            getFbLikes:function(onSuccess, onFail) {
                var postObj = {
                    Url: "https://graph.facebook.com/fql?q=select%20%20like_count%20from%20link_stat%20where%20url=%22http://www.facebook.com/premiumstress/%22",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Get(postObj);
            },
            getYoutubeFollowers: function (onSuccess, onFail) {
                var postObj = {
                    Url: "https://www.googleapis.com/youtube/v3/channels?part=statistics&id=UCBw3gmxrgxb5hCtfSQr9bSw&key=AIzaSyCgBOKHgd7jduvD73ohzsOV3UcJ3g5oz6I",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };

                Service.Get(postObj);
            },
            getTwitterFollowers: function (onSuccess, onFail) {
                var postObj = {
                    Url: "http://query.yahooapis.com/v1/public/yql?q=SELECT%20*%20from%20html%20where%20url=%22http://twitter.com/PremiumStress%22%20AND%20xpath=%22//li[contains(@class,%27ProfileNav-item--followers%27)]/a/span[@class=%27ProfileNav-value%27]|//li[contains(@class,%27ProfileNav-item--following%27)]/a/span[@class=%27ProfileNav-value%27]|//li[contains(@class,%27ProfileNav-item--tweets%27)]/a/span[@class=%27ProfileNav-value%27]%22&format=json",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };

                Service.Get(postObj);
            }
        }
    }];
    var CategoryService = ["$rootScope", "Service", function ($rootScope, Service) {
        return {
            getCategories: function (onSuccess, onFail) {
                var postObj = {
                    Url: "/Admin/GetCategories/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            insertCategory: function (onSuccess, onFail,category) {
                var postObj = {
                    Url: "/Admin/InsertCategory/",
                    Param: { category: category },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            updateCategory: function (onSuccess, onFail, categoryList) {
                var postObj = {
                    Url: "/Admin/UpdateCategoryList/",
                    Param: { categories: categoryList },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            deleteCategory: function (onSuccess, onFail, category) {
                var postObj = {
                    Url: "/Admin/DeleteCategory/",
                    Param: { category: category },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            }
        }
    }];

    var TagService = ["$rootScope", "Service", function ($rootScope, Service) {
        return {
            getTags: function (onSuccess, onFail) {
                var postObj = {
                    Url: "/Admin/GetTags/",
                    Param: {},
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            insertCategory: function (onSuccess, onFail, category) {
                var postObj = {
                    Url: "/Admin/InsertCategory/",
                    Param: { category: category },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            updateCategory: function (onSuccess, onFail, categoryList) {
                var postObj = {
                    Url: "/Admin/UpdateCategoryList/",
                    Param: { categories: categoryList },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            },
            deleteCategory: function (onSuccess, onFail, category) {
                var postObj = {
                    Url: "/Admin/DeleteCategory/",
                    Param: { category: category },
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            }
        }
    }];

    app.factory("Service", Service);
    app.factory("Utilities", Utilities);
    app.factory("GetLink", GetLink);
    app.factory("UploadImageService", UploadImageService);
    app.factory("UserService", UserService);
    app.factory("BlogService", BlogService);
    app.factory("SocialService", SocialService);
    app.factory("CategoryService", CategoryService);
    app.factory("TagService", TagService);
}());