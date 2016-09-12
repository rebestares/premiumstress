(function () {
    'use strict';


    var BlogController = [
    "$scope", "$http", "Service", "GetCategoriesService", "Utilities", "GetLink", "$timeout", "$sce", "$routeParams",
    function ($scope, $http, Service, GetCategoriesService, Utilities, GetLink, $timeout, $sce, $routeParams) {
        var CreateBlogObj = function () {
            var blog = {
                title: "",
                content: "",
                keywords: undefined,
                category: {
                    ID: 1,
                    Name: ""
                },
                imageLinks: {
                    fullImageLink: "",
                    thumbnailImageLink: "",
                    mediumImageLink: ""
                },
                comments: {
                    Name: "",
                    Email: "",
                    CommentID: "",
                    Website: "",
                    Comment: ""
                },
                VideoLink: ""
            };
            return blog;
        };

        var CreateCommentObj = function () {
            var commenter = {
                Name: "",
                Email: "",
                Website: "",
                Comment: "",
                BlogID: "",
                CommentID: "",
                IsReply: ""
            };
            return commenter;
        }; //Navigation
        var GoToAddPage = function () {
            window.location.href = "/blog/add/";
        };
        var GoToBlogPage = function () {
            window.location.href = "#/";
        };
        var GetSingleBlogLink = function (id, title) {
            return GetLink.SingleBlog(id, title);
        };
        var GotoEdit = function (id, title) {
            title = title.split(" ").join("-").toLowerCase();
            window.location.href = "/blog/edit/" + id + "/" + title + "";
        };
        var GoToSearch = function (word, page) {
            word = word.split(" ").join("-").toLowerCase();
            window.location.href = "/blog/search/" + word + "/" + page + "";
        };


        //Get Pages
        var GetAllBlogsPage = function () {
            var pageNumber = $routeParams.pageNum;
            GetBlogs(pageNumber);
        };
        var GetTagPage = function () {
            var tag = $routeParams.tagName;
            var page = $routeParams.tagPage;

            if (page == null)
                page = 1;

            GetBlogsByTag(page, tag);
        };
        var GetCategoriesPage = function () {
            var category = $routeParams.category;
            var page = $routeParams.categoryPage;
            if (page == null)
                page = 1;


            GetBlogsByCategory(page, category);
        };

        //CRUD
        var AddNewBlog = function (isEdit) {
            $scope.transactionInProgress = true;
            var onSuccess = function (response) {
                $scope.transactionInProgress = false;
                var blogObj = response.data.blog;
                var isSuccess = (isEdit) ? response.data.isSuccess : response.data;
                if (isSuccess) {
                    var alert = "<div data-alert class=\"alert-box success radius\">You have successfully added a new blog.<a href=\"#\" class=\"close\">&times;</a></div>";
                    if (!$(".alert-box").hasClass("success")) {
                        $(".alert-holder").append(alert).foundation();
                    }

                    if (isEdit) {
                        var goToLink = GetSingleBlogLink(blogObj.ID, blogObj.Title);
                        window.location.href = goToLink;
                    }
                } else {
                    $(".alert-box.success").remove();
                    var alert = "<div data-alert class=\"alert-box alert radius server-error\">There is a problem in your request.<a href=\"#\" class=\"close\">&times;</a></div>";
                    if (!$(".alert-box").hasClass("alert")) {
                        $(".alert-holder").append(alert).foundation();
                    }
                }

                $scope.blog = CreateBlogObj();
                $scope.htmlVariable = "";
                $scope.addBlogForm.$setPristine();
                return true;
            };
            var onFail = function (reason) {
            }; //Fetch blog content
            $scope.blog.content = $scope.htmlVariable;

            //Add Blog Validation
            if ($scope.addBlogForm.$invalid) {
                if (!isEdit && $scope.addBlogForm.$invalid ||
                    isEdit && !$scope.addBlogForm.title.$pristine && !$scope.addBlogForm.blogContent.$pristine ||
                    isEdit && $scope.addBlogForm.blogContent.$dirty && $scope.addBlogForm.blogContent.$invalid ||
                    isEdit && $scope.addBlogForm.title.$dirty && $scope.addBlogForm.title.$invalid) {
                    $(".alert-box.success").remove();
                    $(".alert-box.server-error").remove();
                    var alert = "<div data-alert class=\"alert-box alert radius\">Please fill in the required fields.<a href=\"#\" class=\"close\">&times;</a></div>";

                    if (!$(".alert-box").hasClass("alert")) {
                        $(".alert-holder").append(alert).foundation();
                    }

                    if ($scope.addBlogForm.blogContent.$invalid) {
                        $(".ta-editor").removeClass("without-error");
                        $(".ta-editor").addClass("input-validation-error");
                    } else {
                        ResetContentContainer();
                    }
                } else {
                    GetKeywords();
                    ProceedToServiceCall();
                }
            } else {
                GetKeywords();
                ProceedToServiceCall();
            }

            function GetKeywords() {
                if (typeof $scope.blog.Keywords === "string") {
                    if ($scope.blog.Keywords == "") {
                        $scope.blog.Keywords = [];
                    } else {
                        var keywordsArr = $scope.blog.Keywords.split(",");

                        if (isEdit && $scope.blog.Keywords == "") {
                        }

                        $scope.blog.Keywords = keywordsArr;
                    }
                } else if (typeof $scope.blog.keywords === "undefined") {
                    if (isEdit && $scope.blog.Keywords != "") {
                        $scope.blog.Keywords = $scope.blog.Keywords.split(",");
                        delete $scope.blog.Keywords;
                    } else {
                        $scope.blog.Keywords = [];
                    }
                }
            }

            function ProceedToServiceCall() {
                ResetContentContainer();
                var postObj = {
                    Param: { model: $scope.blog },
                    Url: "",
                    Success: onSuccess,
                    Fail: onFail
                };

                $(".alert-box.alert").remove();
                if (isEdit && $scope.blog.content != "" && $scope.blog.title != "") {
                    if ($scope.blog.Content != null) {
                        delete $scope.blog.Content;
                    }
                    if ($scope.blog.Title != null) {
                        if ($scope.blog.title != null) {
                            delete $scope.blog.Title;
                        }
                    }
                    $scope.blog.category.ID = $scope.blog.category.ID;
                    delete $scope.blog.Category;

                    var onSuccessUploadImage = function (response) {
                        $scope.blog.imageLinks = response;
                        postObj.Url = "/Blog/UpdateBlog/";

                        if ($scope.blog.imageLinks != null) {
                            if ($scope.blog.ImageLinks != null) {
                                delete $scope.blog.ImageLinks;
                            }
                        }
                        Service.Post(postObj);
                    };
                    var onFailUploadImage = function (reason) {
                    };
                    UploadImage(isEdit, onSuccessUploadImage, onFailUploadImage);
                } else if ($scope.blog.content != "" && $scope.blog.Title != "") {

                    var postObj = {
                        Url: "/Blog/InsertBlog/",
                        Param: { blog: $scope.blog },
                        Success: onSuccess,
                        Fail: onFail
                    };

                    delete $scope.blog.title;
                    delete $scope.blog.keywords;
                    $scope.blog.category.ID = $scope.blog.category.ID;

                    var onSuccessUploadImage = function (response) {
                        $scope.blog.imageLinks = response;
                        postObj.Url = "/Blog/InsertBlog/";

                        Service.Post(postObj);
                    };
                    var onFailUploadImage = function (reason) {
                    };
                    UploadImage(isEdit, onSuccessUploadImage, onFailUploadImage);
                }
            }

            function UploadImage(isThisEdit, onSuccessUploadImage, onFailUploadImage) {
                var uploadUrl = "/Blog/UploadBlogImage";
                var fd = new FormData();
                fd.append("file", $scope.blog.currentImageLink);

                if (isThisEdit) {
                    fd.append("fileName", $scope.blog.ImageLinks.FullImageLink);
                    fd.append("thumbnailFilename", $scope.blog.ImageLinks.ThumbnailImageLink);

                }

                $http.post(uploadUrl, fd, {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).success(onSuccessUploadImage, onFailUploadImage);
            }

            function ResetContentContainer() {
                $(".ta-editor").addClass("without-error");
                $(".ta-editor").removeClass("input-validation-error");
            };
        };

        var AddComment = function () {

            var onSuccessAddComment = function (response) {
                var isSuccess = response.data;
                if (isSuccess) {
                    var message = "You have successfully added your comment.";
                    Utilities.alertDiv("success", message, $(".alert-holder-add-comment"));
                    $scope.commenter = CreateCommentObj();
                    $("#blogComment").html("");

                    $timeout(function () { GetSingleBlog($scope.blog.ID, null); }, 1000);
                }
            };

            var onFailAddComment = function (reason) {
            };
            $scope.commenter.BlogID = $scope.blog.ID;
            $scope.commenter.Comment = $("#blogComment").html();

            var param = { comment: $scope.commenter };
            var postObj = {
                Url: "/Blog/AddComment/",
                Param: param,
                Success: onSuccessAddComment,
                Fail: onFailAddComment
            };
            Service.Post(postObj);
        };
        var GetBlogs = function (pageNum) {
            if (pageNum == null)
                pageNum = 1;
            var onSuccessGetBlogs = function (response) {
                SetMultipleBlog(response.data);
            };

            var onFailGetBlogs = function () {
            };
            var postObj = {
                Url: "/Blog/GetBlogs/",
                Param: { pageNumber: pageNum },
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Post(postObj);
        };
        var PromoteBlog = function (id) {

            $("#blogModalConfirmation").foundation("reveal", "open");
            $scope.blogModalMessage = "Current promoted blog would be replaced. Are you sure you want to promote this blog? ";
            $scope.deleteStatus = "<i class=\"fa fa-comment\"/> Promote";
            $scope.userChose = false;

            $scope.onConfirmNo = function () {
                $("#blogModalConfirmation").foundation("reveal", "close");
            };

            $scope.onConfirmYes = function () {

                var param = { id: id };

                $scope.userChose = true;
                var onSuccess = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        $scope.blogModalMessage = "You have successfully updated the promoted blog.";
                        $scope.deleteStatus = "<i class=\"fi-check icon-style\"/>Success!";
                    } else {
                        $scope.blogModalMessage = "There's something wrong with your request.";
                        $scope.deleteStatus = "Fail!";
                    }
                };

                var onFail = function (reason) { };

                var postObj = {
                    Url: "/Blog/PromoteBlog/",
                    Param: param,
                    Success: onSuccess,
                    Fail: onFail
                };
                Service.Post(postObj);
            };
        };

        var GetPromotedBlog = function () {


            var onSuccessGetBlogs = function (response) {
                $scope.promotedBlog = response.data;

                $scope.getSingleBlogLink = GetSingleBlogLink;
                GetSuggestedBlogs(2, $scope.promotedBlog.ID);
            };

            var onFailGetBlogs = function () {
            };
            var param = {};

            var postObj = {
                Url: "/Blog/GetPromotedBlog/",
                Param: param,
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Get(postObj);
        };
        var GetSuggestedBlogs = function (numbOfBlogs, blogId) {
            var onSuccessGetBlogs = function (response) {
                $scope.listOfSuggestedBlogs = response.data;
                $scope.getSingleBlogLink = GetSingleBlogLink;
            };

            var onFailGetBlogs = function () {
            };
            var param = { numOfBlogs: numbOfBlogs, blogId: blogId };

            var postObj = {
                Url: "/Blog/GetSuggestedBlogs/",
                Param: param,
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Post(postObj);
        };
        var GetBlogsByTag = function (pageNum, tag) {
            var onSuccessGetBlogs = function (response) {
                SetMultipleBlog(response.data);
            };

            var onFailGetBlogs = function () {
            };

            var postObj = {
                Url: "/Blog/GetBlogsByTag/",
                Param: { pageNumber: pageNum, tag: tag },
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Post(postObj);
        };
        var GetBlogsByCategory = function (pageNum, category) {
            var onSuccessGetBlogs = function (response) {
                SetMultipleBlog(response.data);
            };

            var onFailGetBlogs = function () {
            };

            var postObj = {
                Url: "/Blog/GetBlogsByCategory/",
                Param: { pageNumber: pageNum, category: category },
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Post(postObj);
        };
        var GetSingleBlog = function (id, isEdit) {
            var onSuccessGetBlogs = function (response) {
                SetSingleBlog(response.data, isEdit);
            };

            var onFailGetBlogs = function () {
            };
            var param = { id: id };

            var postObj = {
                Url: "/Blog/GetSingleBlog/",
                Param: param,
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Post(postObj);
        };
        var GetFeatured = function (isFromNotFound) {
            var onSuccessGetBlogs = function (response) {
                if (isFromNotFound) {
                    $scope.listOfBlogs = response.data;
                    $scope.getUserLink = GetUserLink;
                    $scope.getKeywordLink = GetKeywordLink;

                    $timeout(function () { Utilities.closeLoading("#blog-nofound-wrapper", "#Loading-container") }, 500);
                } else
                    $scope.featuredBlogList = response.data;
            };

            var onFailGetBlogs = function () {
            };
            var postObj = {
                Url: "/Blog/GetFeatured/",
                Param: {},
                Success: onSuccessGetBlogs,
                Fail: onFailGetBlogs
            };
            Service.Post(postObj);
        };
        var GetCategories = function (isEdit, id) {
            var onSuccess = function (response) {
                $scope.categoryList = response.data;
                if (isEdit) {
                    GetSingleBlog(id, true);
                }
            };
            var onFail = function () {
            };
            GetCategoriesService(onSuccess, onFail);
        };
        var SetSingleBlog = function (blog, isEdit) {

            Utilities.closeLoading("#view-wrapper", "#Loading-container");

            $scope.getKeywordLink = GetKeywordLink;

            if (blog.Title != undefined || blog.Title != null) {

                $scope.blog = blog;
                $scope.getUserLink = GetUserLink;
                $scope.getCategoryLink = GetCategoryLink;
                $scope.getSingleBlogLink = GetSingleBlogLink;
                $scope.blog.VideoLink = $scope.blog.VideoLink != null ? $scope.blog.VideoLink : null;
                $scope.hasVideoLink = $scope.blog.VideoLink != "" | $scope.blog.VideoLink != null ? true : false;


                if (isEdit) {
                    $scope.blog.category = $scope.categoryList[$scope.blog.Category.ID - 1]; //Set default category
                    $scope.htmlVariable = blog.Content; //to reflect blog content into text angular

                    if ($scope.blog.Keywords != null)
                        $scope.blog.Keywords = $scope.blog.Keywords.join(",");
                }

                if (!isEdit) {
                    /// * * * CONFIGURATION VARIABLES * * */
                    //////Change to "premiumstress" when going to production
                    var disqus_shortname = "premiumstresslocal";

                    ///* * * DON'T EDIT BELOW THIS LINE * * */
                    (function () {
                        var dsq = document.createElement("script");
                        dsq.type = "text/javascript";
                        dsq.async = true;
                        dsq.src = "//" + disqus_shortname + ".disqus.com/embed.js";
                        (document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]).appendChild(dsq);
                    })();
                }
            }
        };
        var SetMultipleBlog = function (blogReturnObj) {
            $scope.listOfBlogs = blogReturnObj.BlogList;
            $scope.totalBlogs = blogReturnObj.TotalBlogCount;
         
            $scope.currentPageNum = blogReturnObj.CurrentPage == null ? 1 : blogReturnObj.CurrentPage;
            $scope.withCategory = blogReturnObj.WithCategory != null ? blogReturnObj.WithCategory : null;
            console.log($scope.currentPageNum);
            if (blogReturnObj.WithCategory != null) {
                $scope.forPagingCategory = blogReturnObj.WithCategory;
            }

            if (blogReturnObj.WithTag != null)
                $scope.forPagingTag = blogReturnObj.WithTag;

            $scope.blogContentLoaded = true;

            Utilities.closeLoading("#blog-wrapper", "#Loading-container");

            $scope.getPageNumber = GetPageNumber;
            $scope.getCategoryLink = GetCategoryLink;
            $scope.getUserLink = GetUserLink;
            $scope.getPageLink = GetPageLink;
            $scope.getKeywordLink = GetKeywordLink;
            $scope.getClassForPageNum = GetClassForPageNum;

            $scope.getSingleBlogLink = GetSingleBlogLink;


            //////Change to "premiumstress" when going to production
            //var disqus_shortname = "premiumstresslocal";

            /////* * * DON'T EDIT BELOW THIS LINE * * */
            //(function () {
            //    var dsq = document.createElement('script');
            //    dsq.type = 'text/javascript';
            //    dsq.async = true;
            //    dsq.id = "dsq-count-scr";
            //    dsq.src = '//' + disqus_shortname + '.disqus.com/count.js';
            //    (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
            //})();
        };
        var FindBlogs = function (word, page) {
            var onSuccessFindBlogs = function (response) {
                SetMultipleBlog(response.data);
                $scope.searchedWord = word;
            };
            var onFailFindBlogs = function (reason) {

            };
            var postObj = {
                Url: "/Blog/FindBlogs/",
                Param: { word: word, page: page },
                Success: onSuccessFindBlogs,
                Fail: onFailFindBlogs
            };

            Service.Post(postObj);
        };
        var DeleteBlog = function () {
            $("#blogModalConfirmation").foundation("reveal", "open");
            $scope.blogModalMessage = "Are you sure you want to delete this blog?";
            $scope.deleteStatus = "<i class=\"fi-x icon-style\"/>Delete";
            $scope.userChose = false;

            $scope.onConfirmNo = function () {
                $("#blogModalConfirmation").foundation("reveal", "close");
            };

            $scope.onConfirmYes = function () {
                $scope.userChose = true;
                var onSuccessDelete = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        $scope.blogModalMessage = "You have successfully deleted the blog.";
                        $scope.deleteStatus = "<i class=\"fi-check icon-style\"/>Success!";
                        window.location.href = "/blog/";
                    } else {
                        $scope.blogModalMessage = "There's something wrong with your request.";
                        $scope.deleteStatus = "Fail!";
                    }
                };

                var onFailDelete = function (reason) { };

                var postObj = {
                    Url: "/Blog/DeleteBlog/",
                    Param: { blog: $scope.blog },
                    Success: onSuccessDelete,
                    Fail: onFailDelete
                };
                Service.Post(postObj);
            };
        };

        //Link builders
        var GetKeywordLink = function (keyword) {
            keyword = keyword.replace(/ /g, "-").toLowerCase();
            return "/blog/tag/" + keyword + "";
        };
        var GetUserLink = function (name, id) {
            name = name.split(" ").join("-").toLowerCase();
            return "/user/" + id + "/" + name + "";
        };
        var GetPageLink = function (page) {
            var pageLink = "";

            //Check if it is category or tag page or main page
            if ($scope.forPagingCategory != undefined && $scope.forPagingTag != undefined) {

                if ($scope.forPagingCategory.WithCategory != undefined && $scope.forPagingCategory.WithCategory) {

                    pageLink = page === 1 ? "/blog/category/" + $scope.forPagingCategory.CategoryName + "/page/" + 1 + "" :
                        "/blog/category/" + $scope.forPagingCategory.CategoryName + "/page/" + page;

                } else if ($scope.forPagingTag.WithTag != undefined && $scope.forPagingTag.WithTag) {

                    pageLink = page === 1 ? "/blog/tag/" + $scope.forPagingTag.TagName + "/page/" + 1 + "" :
                        "/blog/tag/" + $scope.forPagingTag.TagName + "/page/" + page;

                } else {
                    if (page == 1) {
                        pageLink = "/blog/";
                    } else if (page === "...") {
                        pageLink = "#";
                    } else {
                        pageLink = "/blog/page/" + page;
                    }
                }

            }
                //Default would be search page
            else {

                if ($scope.searchedWord != null) {

                    var searchedWord = $scope.searchedWord;
                    searchedWord = searchedWord.split(" ").join("-").toLowerCase();

                    if (page === 1)
                        pageLink = "/blog/result/" + searchedWord + "/1";
                    else if (page === "...")
                        pageLink = "#";
                    else
                        pageLink = "/blog/result/" + searchedWord + "/" + page;

                }
            }
            return pageLink;
        };
        var GetCategoryLink = function (category) {
            return GetLink.Category(category);
        };
        var GetPageNumber = function (num, screen) {
            //screen = 1, small view
            $scope.withPaging = true;

            var pagination = getTotalPageNum();
            $scope.totalPaging = pagination.length;
            console.log($scope.currentPageNum);
            var currPage = $scope.currentPageNum;
            var isGreaterThan3 = false;
            var firstPartPage = getFirstHalfOfPaging();

            var secondPartPage = getSecondHalfOfPaging();
            if ($scope.PagingArray != null) {
                return $scope.PagingArray; //Return same array if paging array is not null to avoid looping problems
            }

            var toBeReturnedPage = new Array();

            if (screen != 1) {
                if (isGreaterThan3)
                    firstPartPage.reverse();

                var secondPartFirsEl = secondPartPage[0];
                var firstPartLastEl = firstPartPage[firstPartPage.length - 1];

                if (secondPartFirsEl - 1 > firstPartLastEl) //To check if '...' is needed
                    firstPartPage.push("...");

                if ($scope.totalPaging > 3)
                    toBeReturnedPage = arrayUnique(firstPartPage.concat(secondPartPage));
                else
                    toBeReturnedPage = firstPartPage;

                $scope.lastPage = toBeReturnedPage[toBeReturnedPage.length - 1];
            } else {
                if (currPage >= 3)
                    toBeReturnedPage = firstPartPage.reverse();
                else
                    toBeReturnedPage = firstPartPage;
            }

            $scope.PagingArray = toBeReturnedPage; //Return paging array
            return $scope.PagingArray;


            //Paging Functions
            function arrayUnique(array) {
                var a = array.concat();
                for (var i = 0; i < a.length; ++i) {
                    for (var j = i + 1; j < a.length; ++j) {
                        if (a[i] === a[j])
                            a.splice(j--, 1);
                    }
                }

                return a;
            };

            //Get Paging Numbers
            function getTotalPageNum() {
                var totalPagination = new Array();
                for (i = 0; i <= num - 1; i++) {
                    //5 is the default number of records to be returned
                    if (i % 10 == 0)
                        totalPagination.push(i);
                }
                return totalPagination;
            };

            function getFirstHalfOfPaging() {
                var firstHalf = new Array();
                if (currPage <= 3) {
                    var toBeFetched = $scope.totalPaging < 3 ? $scope.totalPaging : 3;
                    for (x = 1; x <= toBeFetched; x++) {
                        if (x > 0) {
                            firstHalf.push(x);
                        }
                    }
                } else {
                    var ctr = 2;
                    isGreaterThan3 = true;
                    var lastElement = $scope.lastPage;
                    /*
                 * To check if the current page is greater than
                 * the last element of the second half of the paging.
                 */
                    if (currPage > lastElement - 3) {
                        if (screen == 1) {
                            firstHalf = getSecondHalfOfPaging();
                            firstHalf.reverse();
                        } else {
                            for (x = lastElement - 3; 0 <= ctr; ctr--) {
                                if (x > 0) {
                                    firstHalf.push(x);
                                }
                                x--;
                            }
                        }
                    } else {
                        for (x = currPage + 1; 0 <= ctr; ctr--) {
                            if (x > 0) {
                                firstHalf.push(x);
                            }
                            x--;
                        }
                    }
                }

                return firstHalf;
            }

            function getSecondHalfOfPaging() {
                var secondHalf = new Array();
                for (var y = pagination.length - 2; y <= pagination.length; y++) {
                    if (y > 0) {
                        secondHalf.push(y);
                    }
                }
                return secondHalf;
            }
        };


        //Utilities
        var ShowFooter = function () {
            Utilities.closeLoading("#add-blog-wrapper", "#Loading-container");
        };
        var GetUrlForDisqus = function (id, title) {
            var url = window.location.href;
            var link = GetSingleBlogLink(id, title);
            while (link.charAt(0) === "/")
                link = link.substr(1);
            while (url.charAt(0) === "/")
                link = link.substr(1);
            link = link.slice(0, -1);

            return url + link + "#disqus_thread";
        };
        var FormatForUrl = function (link) {
            if (link != undefined)
                link = link.split(" ").join("-").toLowerCase();
            return link;
        };

        //Class builders
        var GetClassForPageNum = function (pageNum, CurrentPageNum) {
            var pageNumClass;
            //return pageNum == CurrentPageNum ? 'current' : '';
            if (pageNum == CurrentPageNum) {
                pageNumClass = "current";
            } else if (pageNum == "...") {
                pageNumClass = "unavailable";
            } else {
                pageNumClass = "";
            }

            return pageNumClass;
        };
        var GetClassForCategoryLink = function (category) {
            if ($routeParams.category != null) {
                $routeParams.category = $routeParams.category.replace("-", " ");
                return category.toLowerCase() == $routeParams.category ? "active-category" : "";
            }
        };


        //Social Counts
        var GetSocialCounts = function () {
            GetTwitterFollowerCount();
            GetFbLikesCount();
            GetYoutubeFollower();
        };
        var GetFbLikesCount = function () {

            var onSuccess = function (response) {
                var fbPageLikeCount = response.data.data[0].like_count;
                $scope.fbFollowersCount = fbPageLikeCount;
            };
            var onFail = function (reason) { };
            var postObj = {
                Url: "https://graph.facebook.com/fql?q=select%20%20like_count%20from%20link_stat%20where%20url=%22http://www.facebook.com/premiumstress/%22",
                Param: {},
                Success: onSuccess,
                Fail: onFail
            };
            Service.Get(postObj);
        };
        var GetYoutubeFollower = function () {
            var onSuccess = function (response) {
                var youtubeSubscribersCount = response.data.items[0].statistics.subscriberCount;
                $scope.youtubeSubscribersCount = youtubeSubscribersCount;
            };
            var onFail = function (reason) { };
            var postObj = {
                Url: "https://www.googleapis.com/youtube/v3/channels?part=statistics&id=UCBw3gmxrgxb5hCtfSQr9bSw&key=AIzaSyCgBOKHgd7jduvD73ohzsOV3UcJ3g5oz6I",
                Param: {},
                Success: onSuccess,
                Fail: onFail
            };

            Service.Get(postObj);
        };

        var GetTwitterFollowerCount = function () {

            var onSuccess = function (response) {
                var twitterFollowerCount = response.data.query.results.span[1].content;
                $scope.twitterFollowerCount = twitterFollowerCount;
            };
            var onFail = function (reason) { };
            var postObj = {
                Url: "http://query.yahooapis.com/v1/public/yql?q=SELECT%20*%20from%20html%20where%20url=%22http://twitter.com/PremiumStress%22%20AND%20xpath=%22//li[contains(@class,%27ProfileNav-item--followers%27)]/a/span[@class=%27ProfileNav-value%27]|//li[contains(@class,%27ProfileNav-item--following%27)]/a/span[@class=%27ProfileNav-value%27]|//li[contains(@class,%27ProfileNav-item--tweets%27)]/a/span[@class=%27ProfileNav-value%27]%22&format=json",
                Param: {},
                Success: onSuccess,
                Fail: onFail
            };

            Service.Get(postObj);

        };
        var SubscribeUser = function () {

            //var onSuccess = function (data) {
            //    alert("A verification email was sent into your email");
            //}
            //var onFail = function (reason) { }

            //var postObj = {
            //    Url: "http://maillist.Smarterasp.net/subscribe?l=37147&s=0021E1F01289C706C71F4F5B9B12B244&firstname=rebie&lastname=estares&email=rebie20@gmail.com&callback=JSON_CALLBACK",
            //    Success: onSuccess,
            //    Fail: onFail
            //};

            //Service.JSONP(postObj);
        };


        //BLOG CRUD
        $scope.addNewBlog = AddNewBlog;
        $scope.blog = CreateBlogObj();
        $scope.commenter = CreateCommentObj();
        $scope.editBlogPost = AddNewBlog;
        $scope.deleteBlog = DeleteBlog;
        $scope.addComment = AddComment;
        $scope.promoteBlog = PromoteBlog;

        //Navigation
        $scope.goToAdd = GoToAddPage;
        $scope.goToBlog = GoToBlogPage;
        $scope.getSingleBlogLink = GetSingleBlogLink;
        $scope.goToEdit = GotoEdit;
        $scope.goToSearch = GoToSearch;

        //Fetching
        $scope.getBlogs = GetBlogs;
        $scope.getSingleBlog = GetSingleBlog;
        $scope.singleBlog = SetSingleBlog;
        $scope.multipleBlog = SetMultipleBlog;
        $scope.getCategories = GetCategories;
        $scope.getFeatured = GetFeatured;
        $scope.findBlogs = FindBlogs;
        $scope.getBlogsByTag = GetBlogsByTag;
        $scope.getBlogsByCategory = GetBlogsByCategory;
        $scope.getPromotedBlog = GetPromotedBlog;
        $scope.getSuggestedBlogs = GetSuggestedBlogs;
        $scope.getClassForCategoryLink = GetClassForCategoryLink;
        $scope.getAllBlogsPage = GetAllBlogsPage;
        $scope.getTagPage = GetTagPage;
        $scope.getCategoriesPage = GetCategoriesPage;

        //Api calls
        $scope.getSocialCounts = GetSocialCounts;
        $scope.subscribeUser = SubscribeUser;

        //Loader Handlers
        $scope.blogContentLoaded = false;
        $scope.transactionInProgress = false;

        //Utilities
        $scope.uploadFile = function (event) {
            $scope.blog.currentImageLink = event.target.files[0];
        };
        $scope.goTo = function (location) {
            if ($scope.currentPageNum + 1 > $scope.totalPaging) {
            }
            if ($scope.currentPageNum + 1 > $scope.totalPaging) {
            }
        };
        $scope.closeAlertBox = function () {
            $("#custom-alertbox").foundation("reveal", "close");
        };
        $scope.showFooter = ShowFooter;
        $scope.getUrlForDisqus = GetUrlForDisqus;
        $scope.formatForUrl = FormatForUrl;

        //Link builder
        $scope.buildPagingLink = function (pageNum) {
            if ($scope.forPagingCategory != null && $scope.forPagingTag != null) {
                if ($scope.forPagingCategory === false && $scope.forPagingTag === false)
                    return "#/page/" + pageNum;
                else if ($scope.forPagingCategory != false)
                    return "#/category/" + FormatForUrl($scope.forPagingTag.CategoryName) + "/page/" + pageNum;
                else if ($scope.forPagingTag != false)
                    return "#/tag/" + FormatForUrl($scope.forPagingTag.TagName) + "/page/" + pageNum;
            }
        }
    }
    ];

    var blogModule = angular.module('BlogModule', [
        // Angular modules 
        'ngRoute'

        // Custom modules 
        // 3rd Party Modules
    ]);
    
    blogModule.controller("BlogController", BlogController);

    blogModule.config(["$routeProvider", "$locationProvider",
    function ($routeProvider, $locationProvider) {
        $routeProvider.
          //Default view for Blog Index page
          when("/", {
              controller: "BlogController",
              templateUrl: "Template/Blog/AllArticlesPage.html",
              caseInsensitiveMatch: true
          })
          //Default view for Contact Index page
          //.when("/", {
          //    controller: "ContactController",
          //    templateUrl: "Template/Contact/ContactUs.html",
          //    caseInsensitiveMatcSh: true
          //})

          //Routing for Blog page
          .when("/page/:pageNum", {
              controller: "BlogController",
              templateUrl: "Template/Blog/AllArticlesPage.html",
              caseInsensitiveMatch: true
          }).when("/category/:category", {
              controller: "BlogController",
              templateUrl: "Template/Blog/CategoryPage.html",
              caseInsensitiveMatch: true
          }).when("/category/:category/:categoryPage", {
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
          })

          ////Routing for Contact page
          //.when("/us/", {
          //    controller: "ContactController",
          //    templateUrl: "Template/Contact/ContactUs.html",
          //    caseInsensitiveMatch: true
          //}).when("/specific/", {
          //    controller: "ContactController",
          //    templateUrl: "Template/Contact/ContactSpecificPerson.html",
          //    caseInsensitiveMatch: true
          //})
      .otherwise({
          redirectTo: "/"
      });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
        // $locationProvider.html5Mode(true);
    }]);
})();