

(function () {
    var BlogController = [
        "$scope", "Utilities", "GetLink", "$timeout", "$sce",
        "$routeParams", "$location", "UserService", "BlogService", "SocialService", "CategoryService",
        function ($scope, Utilities, GetLink, $timeout, $sce,
                $routeParams, $location, UserService, BlogService, SocialService, CategoryService) {

            $scope.isPageLoaded = true;
            $scope.isLoaded = function (isLoaded) {
                if (isLoaded)
                    $scope.isPageLoaded = false;
            }

            //#region Implementations
            //#region CRUD Implementation
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
            };
            var ApproveBlog = function (id, status, pageNum, isOwner) {
                var blogApprovalStatus = !status;

                var onSuccess = function (data) {
                    if (data)
                        GetBlogs(pageNum, 'settings');
                };
                var onFail = function () { };
                var param = { id: id, approvalStatus: blogApprovalStatus };

                if (isOwner)
                    BlogService.approveBlog(onSuccess, onFail, param);
            }
            var AddNewBlog = function (isEdit) {
                $scope.transactionInProgress = true;
                var onSuccess = function (response) {
                    $scope.transactionInProgress = false;
                    var blogObj = response.data.blog;
                    var isSuccess = (isEdit) ? response.data.isSuccess : response.data;
                    if (isSuccess) {

                        var alertMessage = isEdit ? "You have successfully updated the blog." : "You have successfully added a new blog.";

                        var alert = "<div data-alert class=\"alert-box success radius\">" + alertMessage + "<a href=\"#\" class=\"close\">&times;</a></div>";
                        if (!$(".alert-box").hasClass("success")) {
                            $(".alert-holder").append(alert).foundation();
                        }

                        if (isEdit) {
                           GetSingleBlogLink(blogObj.ID, blogObj.Title);
                          //  window.location.href = goToLink;
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
                        if ($scope.blog.Keywords === "") {
                            $scope.blog.Keywords = [];
                        } else {
                            var keywordsArr = $scope.blog.Keywords.split(",");

                            if (isEdit && $scope.blog.Keywords == "") {
                            }

                            $scope.blog.Keywords = keywordsArr;
                        }
                    } else if (typeof $scope.blog.keywords === "undefined") {
                        if (isEdit && $scope.blog.Keywords !== "") {
                            $scope.blog.Keywords = $scope.blog.Keywords.split(",");
                            delete $scope.blog.Keywords;
                        } else {
                            $scope.blog.Keywords = [];
                        }
                    }
                }

                function ProceedToServiceCall() {

                    ResetContentContainer();

                    $(".alert-box.alert").remove();
                    if (isEdit && $scope.blog.content !== "" && $scope.blog.title !== "") {
                        if ($scope.blog.Content != null) {
                            delete $scope.blog.Content;
                        }
                        if ($scope.blog.Title != null) {
                            if ($scope.blog.title != null) {
                                delete $scope.blog.Title;
                            }
                        }
                        if ($scope.blog.Category != null) {
                            if ($scope.blog.category != null) {
                                delete $scope.blog.Category;
                            }
                        }
                        //if ($scope.blog.Category != null) {
                            
                        //}
                        //$scope.blog.category.ID = $scope.blog.category.ID;
                        //delete $scope.blog.Category;

                        var onSuccessUploadImage = function (response) {

                            $scope.blog.imageLinks = response;

                            if ($scope.blog.imageLinks != null) {
                                if ($scope.blog.ImageLinks != null) {
                                    delete $scope.blog.ImageLinks;
                                }
                            }
                   
                            BlogService.updateBlog(onSuccess, onFail, $scope.blog);
                        };
                        var onFailUploadImage = function (reason) {
                        };

                        UploadImage(isEdit, onSuccessUploadImage, onFailUploadImage);

                    } else if ($scope.blog.content !== "" && $scope.blog.Title !== "") {

                        delete $scope.blog.title;
                        delete $scope.blog.keywords;
                        $scope.blog.category.ID = $scope.blog.category.ID;

                        var onSuccessUploadImage = function (response) {

                            $scope.blog.imageLinks = response;

                            BlogService.insertBlog(onSuccess, onFail, $scope.blog);
                        };
                        var onFailUploadImage = function (reason) {
                        };
                        UploadImage(isEdit, onSuccessUploadImage, onFailUploadImage);
                    }
                }

                function UploadImage(isThisEdit, onSuccess, onFail) {
                    var fd = new FormData();
                    fd.append("file", $scope.blog.currentImageLink);

                    if (isThisEdit) {
                        fd.append("fileName", $scope.blog.ImageLinks.FullImageLink);
                        fd.append("thumbnailFilename", $scope.blog.ImageLinks.ThumbnailImageLink);

                    }

                    BlogService.uploadBlogImage(onSuccess, onFail, fd);
                }

                function ResetContentContainer() {
                    $(".ta-editor").addClass("without-error");
                    $(".ta-editor").removeClass("input-validation-error");
                };
            };
            var AddComment = function () {

                var onSuccess = function (response) {
                    var isSuccess = response.data;
                    if (isSuccess) {
                        var message = "You have successfully added your comment.";
                        Utilities.alertDiv("success", message, $(".alert-holder-add-comment"));
                        $scope.commenter = CreateCommentObj();
                        $("#blogComment").html("");

                        $timeout(function () { GetSingleBlog($scope.blog.ID, null); }, 1000);
                    }
                };

                var onFail = function (reason) {
                };
                $scope.commenter.BlogID = $scope.blog.ID;
                $scope.commenter.Comment = $("#blogComment").html();

                BlogService.addComment(onSuccess, onFail, $scope.commenter);

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

                    var param = {
                        id: id
                    };

                    BlogService.promoteBlog(onSuccess, onFail, param);
                };
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
                    var onSuccess = function (response) {
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

                    var onFail = function (reason) { };

                    BlogService.deleteBlog(onSuccess, onFail, $scope.blog);
                };
            };
            //#endregion

            function initialize() {
                $scope.getPromotedBlog();
                $scope.getSocialCounts();
                $scope.getCategories();
                $scope.getTags();
                $scope.getFeatured();
            }



            //#region Navigation Implementation
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
                window.location.href = "/edit/" + id + "/" + title + "";
            };
            var GoToSearch = function (word, page) {
                word = word.split(" ").join("-").toLowerCase();
                $location.path("/search/" + word + "/page/" + page + "");
            };
            //#endregion

            //#region Fetching Implementation
            var GetBlogs = function (pageNum, module) {

                module = module != null ? 'settings' : 'blog';

                var onSuccess = function (response) {
                    if (module === 'settings') {


                        $scope.sortOrderDropDown = {
                            availableOptions: [
                              { id: 'asc', name: 'Ascending' },
                              { id: 'desc', name: 'Descending' }
                            ],
                            selectedOption: { id: 'desc', name: 'Descending' } //This sets the default value of the select in the ui
                        };

                        $scope.sortPropertyDropDown = {
                            availableOptions: [
                              { id: 'id', name: 'ID' },
                              { id: 'title', name: 'Title' },
                              { id: 'dateposted', name: 'Date Posted' },
                              { id: 'viewcount', name: 'Views' }
                            ],
                            selectedOption: { id: 'title', name: 'Title' } //This sets the default value of the select in the ui
                        };
                    }
                    SetMultipleBlog(response.data);
                };

                var onFail = function () { };
                var param = { pageNumber: pageNum, module: module }
                BlogService.getBlogs(onSuccess, onFail, param);
            };
            var GetSingleBlog = function (id, isEdit) {

                var onSuccess = function (response) {
                    SetSingleBlog(response.data, isEdit);
                };

                var onFail = function () { };
                var param = {
                    ID: id,
                    IsEdit: isEdit
                }
                BlogService.getSingleBlog(onSuccess, onFail, param);
            };
            var SetMultipleBlog = function (blogReturnObj) {
                $scope.listOfBlogs = blogReturnObj.BlogList;
                $scope.totalBlogs = blogReturnObj.TotalBlogCount;
                $scope.currentModule = blogReturnObj.Module;
                $scope.sortProperty = blogReturnObj.SortProperty;
                $scope.sortOrder = blogReturnObj.SortOrder;
                $scope.currentPageNum = blogReturnObj.CurrentPage == null ? 1 : blogReturnObj.CurrentPage;
                $scope.withCategory = blogReturnObj.WithCategory != null ? blogReturnObj.WithCategory : null;
                $scope.isUserPage = blogReturnObj.IsUserPage != null ? blogReturnObj.IsUserPage : null;
                $scope.pageNumbers = GetPageNumber($scope.totalBlogs);

                if (blogReturnObj.WithCategory != null) {
                    $scope.forPagingCategory = blogReturnObj.WithCategory;
                }

                if (blogReturnObj.WithTag != null)
                    $scope.forPagingTag = blogReturnObj.WithTag;

                $scope.blogContentLoaded = true;

                //For the main loading of the page
                Utilities.closeLoading("#blog-wrapper", "#Loading-container");

                //Loading for ajax call when navigating to tags,categories
                $timeout(function () { Utilities.closeLoading(".blog-list-wrapper", "#blog-loading-container"); }, 400);

                $scope.getCategoryLink = GetCategoryLink;
                $scope.getUserLink = GetUserLink;
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
            var SetSingleBlog = function (blog, isEdit) {

                Utilities.closeLoading("#view-wrapper", "#Loading-container");

                $scope.getKeywordLink = GetKeywordLink;

                if (blog.Title != undefined || blog.Title != null) {

                    $scope.blog = blog;
                    $scope.getUserLink = GetUserLink;
                    $scope.getCategoryLink = GetCategoryLink;
                    $scope.getSingleBlogLink = GetSingleBlogLink;
                    $scope.blog.VideoLink = $scope.blog.VideoLink != null ? $scope.blog.VideoLink : null;
                    $scope.hasVideoLink = $scope.blog.VideoLink != null && $scope.blog.VideoLink !== "" ? true : false;


                    if (isEdit) {
                        if (blog.Category != null)
                            $scope.blog.Category = $scope.categoryList[$scope.blog.Category.DisplayOrder - 2]; //Set default category
                        $scope.htmlVariable = blog.Content; //to reflect blog content into text angular

                        if ($scope.blog.Keywords != null)
                            $scope.blog.Keywords = $scope.blog.Keywords.join(",");
                    }

                    if (!isEdit) {
                        /// * * * CONFIGURATION VARIABLES * * */
                        //////Change to "premiumstress" when going to production
                        var disqus_shortname = "premiumstress";//premiumstresslocal when on local

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
            var GetCategories = function (isEdit, id) {
                var onSuccess = function (response) {
                    $scope.categoryList = response.data;
                    if (isEdit) {
                        GetSingleBlog(id, true);
                    }
                };
                var onFail = function () {
                };
                CategoryService.getCategories(onSuccess, onFail);
            };
            var GetFeatured = function (isFromNotFound) {
                var onSuccess = function (response) {
                    if (isFromNotFound) {
                        $scope.listOfBlogs = response.data;
                        $scope.getUserLink = GetUserLink;
                        $scope.getKeywordLink = GetKeywordLink;

                        $timeout(function () { Utilities.closeLoading("#blog-nofound-wrapper", "#Loading-container") }, 500);
                    } else
                        $scope.featuredBlogList = response.data;
                };

                var onFail = function () {
                };

                BlogService.getFeatured(onSuccess, onFail);
            };
            var FindBlogs = function (word, page, module) {
                var onSuccess = function (response) {
                    SetMultipleBlog(response.data);
                    $scope.searchedWord = word;
                };
                var param =
                {
                    word: word,
                    page: page,
                    module: module
                };
                var onFail = function (reason) {

                };
                BlogService.findBlog(onSuccess, onFail, param);
            };
            var GetBlogsByCategory = function (pageNum, categoryName) {
                var onSuccess = function (response) {
                    SetMultipleBlog(response.data);
                };

                var onFail = function () {
                };

                var param = {
                    pageNumber: pageNum,
                    categoryName: categoryName
                };

                BlogService.getBlogsByCategory(onSuccess, onFail, param);
            };
            var GetBlogsByUserId = function () {

                var userId = $routeParams.userId;
                var page = $routeParams.userPageNum;
                var authorName = $routeParams.authorName;

                var onSuccess = function (response) {

                    $scope.userProfile = response.data;
                    if (page == null)
                        page = 1;

                    var onSuccess = function (response) {
                        SetMultipleBlog(response.data);
                    };

                    var onFail = function () {
                    };

                    var param = {
                        pageNumber: page,
                        userId: userId,
                        authorName: authorName
                    };

                    BlogService.getBlogsByUserId(onSuccess, onFail, param);
                };

                var onFail = function () {
                };

                UserService.getUser(onSuccess, onFail, userId);
            };

            var GetBlogsByTag = function (pageNum, tag) {
                var onSuccess = function (response) {
                    SetMultipleBlog(response.data);
                };

                var onFail = function () {
                };

                var param = {
                    tag: tag,
                    page: pageNum
                };

                BlogService.getBlogsByTag(onSuccess, onFail, param);
            };
            var GetPromotedBlog = function () {


                var onSuccess = function (response) {
                    $scope.promotedBlog = response.data;

                    $scope.getSingleBlogLink = GetSingleBlogLink;
                    GetSuggestedBlogs(2, $scope.promotedBlog.ID);
                };

                var onFail = function () {
                };
                BlogService.getPromotedBlog(onSuccess, onFail);
            };
            var GetSuggestedBlogs = function (numOfBlogs, blogId) {
                var onSuccess = function (response) {
                    $scope.listOfSuggestedBlogs = response.data;
                    $scope.getSingleBlogLink = GetSingleBlogLink;
                };

                var onFail = function () {
                };

                var param = {
                    numOfBlogs: numOfBlogs,
                    blogId: blogId
                };

                BlogService.getSuggestedBlogs(onSuccess, onFail, param);
            };
            var GetTags = function () {
                var onSuccess = function (response) {
                    $scope.tagList = response.data;
                };
                var onFail = function () {
                };
                BlogService.getBlogTags(onSuccess, onFail);
            };

            //#region Pages
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
            var GetSearchPage = function () {
                var word = $routeParams.word;
                var page = $routeParams.searchPage;
                if (page == null)
                    page = 1;

                FindBlogs(word, page);
            };
            //#endregion

            //#endregion

            //#region Link builders Implementation
            var GetKeywordLink = function (keyword) {
                keyword = keyword.replace(/ /g, "-").toLowerCase();
                return "/tag/" + keyword + "";
            };
            var GetUserLink = function (name, id) {
                name = name.split(" ").join("-").toLowerCase();
                return "/author/" + id + "/" + name + "";
            };

            var GetCategoryLink = function (category) {
                return GetLink.Category(category);
            };
            var GetPageNumber = function (num, screen) {

                //screen = 1, small view
                $scope.withPaging = true;

                function getTotalPageNum() {
                    var totalPagination = new Array();
                    for (var i = 0; i <= num - 1; i++) {
                        //5 is the default number of records to be returned
                        if (i % 10 === 0)
                            totalPagination.push(i);
                    }

                    return totalPagination;
                }

                var pagination = getTotalPageNum();

                //Number of page
                $scope.totalPaging = pagination.length;

                var currPage = $scope.currentPageNum;
                var isGreaterThan3 = false;

                function getSecondHalfOfPaging() {
                    var secondHalf = new Array();
                    for (var y = pagination.length - 2; y <= pagination.length; y++) {
                        if (y > 0) {
                            secondHalf.push(y);
                        }
                    }
                    return secondHalf;
                }

                function getFirstHalfOfPaging() {
                    var firstHalf = new Array();
                    var x;
                    //Check if page is greater than 3
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
                        var lastElement = pagination.length;
                        /*
                     * To check if the current page is greater than
                     * the last element of the second half of the paging.
                     */
                        if (currPage > lastElement - 3) {
                            if (screen === 1) {
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

                var firstPartPage = getFirstHalfOfPaging();
                var secondPartPage = getSecondHalfOfPaging();
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

                return toBeReturnedPage; //Return paging array

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
            };
            var NavigateBlog = function (page) {

                if ($scope.currentModule !== 'blog') {
                    module = 'settings';

                    var onSuccess = function (response) {
                        SetMultipleBlog(response.data);
                    };

                    var onFail = function () { };
                    var param = {
                        pageNumber: page,
                        module: module,
                        sortProperty: $scope.sortProperty,
                        sortOrder: $scope.sortOrder
                    };

                    BlogService.getBlogs(onSuccess, onFail, param);
                }
            };
            //#endregion

            //#region Utilities Implementation
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
                    link = link.trim().split(" ").join("-").toLowerCase();
                return link;
            };
            var UploadFile = function (event) {
                $scope.blog.currentImageLink = event.target.files[0];
            };
            var GoTo = function (location) {
                if ($scope.currentPageNum + 1 > $scope.totalPaging) {
                }
                if ($scope.currentPageNum + 1 > $scope.totalPaging) {
                }
            };
            var CloseAlertBox = function () {
                $("#custom-alertbox").foundation("reveal", "close");
            };
            var RedirectLinks = function (redirect) {
                $scope.redirectPage = (redirect) ? "_self" : "";
            };
            var GetDate = function (date) {
                if (date == null) return null;
                var d = new Date(date);
                var n = d.getDate();
                return n;
            };
            var GetMonth = function (date) {
                if (date == null) return null;
                return date.substr(0, 3);
            };
            var TimeSince = function (date) {

                if (date == null) return null;

                if (typeof date !== 'object') {
                    date = new Date(date);
                }

                var seconds = Math.floor((new Date() - date) / 1000);
                var intervalType;

                var interval = Math.floor(seconds / 31536000);
                if (interval >= 1) {
                    intervalType = 'year';
                } else {
                    interval = Math.floor(seconds / 2592000);
                    if (interval >= 1) {
                        intervalType = 'month';
                    } else {
                        interval = Math.floor(seconds / 86400);
                        if (interval >= 1) {
                            intervalType = 'day';
                        } else {
                            interval = Math.floor(seconds / 3600);
                            if (interval >= 1) {
                                intervalType = "hour";
                            } else {
                                interval = Math.floor(seconds / 60);
                                if (interval >= 1) {
                                    intervalType = "minute";
                                } else {
                                    interval = seconds;
                                    intervalType = "second";
                                }
                            }
                        }
                    }
                }

                if (interval > 1 || interval === 0) {
                    intervalType += 's';
                }

                return interval + ' ' + intervalType + ' ago';

            };
            var BuildPagingLink = function (pageNum) {
                if ($scope.forPagingCategory != null && $scope.forPagingTag != null) {
                    if ($scope.currentModule === 'settings') {
                        return "";
                    } else {
                        if ($scope.forPagingCategory === false && $scope.forPagingTag === false && $scope.isUserPage === null)
                            return "page/" + pageNum;
                        else if ($scope.forPagingCategory != false)
                            return "category/" + FormatForUrl($scope.forPagingCategory.CategoryName) + "/page/" + pageNum;
                        else if ($scope.forPagingTag != false)
                            return "tag/" + FormatForUrl($scope.forPagingTag.TagName) + "/page/" + pageNum;
                        else if ($scope.isUserPage != false)
                            return "author/" + $scope.isUserPage.UserId + "/" + FormatForUrl($scope.isUserPage.AuthorName) + "/page/" + pageNum;
                    }
                }
            };
            var SortItems = function (sortProperty, sortOrder) {
                $scope.sortOrder = sortProperty;
                $scope.sortProperty = sortProperty;

                var onSuccess = function (response) {
                    SetMultipleBlog(response.data);
                };
                var onFail = function (reason) { };

                var param = {
                    pageNumber: $scope.currentPageNum,
                    module: "settings",
                    sortProperty: $scope.sortProperty,
                    sortOrder: sortOrder
                };

                if (typeof sortProperty != "undefined" && typeof sortOrder != "undefined")
                    BlogService.getBlogs(onSuccess, onFail, param);
            };
            //#endregion

            //#region Class builders Implementation
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
            //#endregion

            //#region Social Counts
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

                SocialService.getFbLikes(onSuccess, onFail);

            };
            var GetYoutubeFollower = function () {
                var onSuccess = function (response) {
                    var youtubeSubscribersCount = response.data.items[0].statistics.subscriberCount;
                    $scope.youtubeSubscribersCount = youtubeSubscribersCount;
                };
                var onFail = function (reason) { };
                SocialService.getYoutubeFollowers(onSuccess, onFail);
            };
            var GetTwitterFollowerCount = function () {

                var onSuccess = function (response) {
                    var twitterFollowerCount = response.data.query.results.span[1].content;
                    $scope.twitterFollowerCount = twitterFollowerCount;
                };
                var onFail = function (reason) { };

                SocialService.getTwitterFollowers(onSuccess, onFail);
            };
            //#endregion

            var EditArticle = function(id) {
                GetSingleBlog(id, true);
                Utilities.showElement('#article-edit', '.all-blogpost-table','hide');
            };
            var BackToEdit = function () {
                Utilities.showElement('.all-blogpost-table', '#article-edit','hide');
            }
            //#endregion

            //#region setters
            //#region CRUD
            $scope.addNewBlog = AddNewBlog;
            $scope.blog = CreateBlogObj();
            $scope.commenter = CreateCommentObj();
            $scope.editBlogPost = AddNewBlog;
            $scope.deleteBlog = DeleteBlog;
            $scope.addComment = AddComment;
            $scope.promoteBlog = PromoteBlog;
            $scope.approveBlog = ApproveBlog;
            //#endregion

            //#region Navigation
            $scope.goToAdd = GoToAddPage;
            $scope.goToBlog = GoToBlogPage;
            $scope.getSingleBlogLink = GetSingleBlogLink;
            $scope.goToEdit = GotoEdit;
            $scope.goToSearch = GoToSearch;
            //#endregion

            //#region Fetching
            $scope.getBlogs = GetBlogs;
            $scope.getSingleBlog = GetSingleBlog;
            $scope.singleBlog = SetSingleBlog;
            $scope.multipleBlog = SetMultipleBlog;
            $scope.getCategories = GetCategories;
            $scope.getFeatured = GetFeatured;
            $scope.findBlogs = FindBlogs;
            $scope.getBlogsByTag = GetBlogsByTag;
            $scope.getBlogsByCategory = GetBlogsByCategory;
            $scope.getBlogsByUserId = GetBlogsByUserId;
            $scope.getPromotedBlog = GetPromotedBlog;
            $scope.getSuggestedBlogs = GetSuggestedBlogs;
            $scope.getTags = GetTags;

            $scope.getAllBlogsPage = GetAllBlogsPage;
            $scope.getTagPage = GetTagPage;
            $scope.getCategoriesPage = GetCategoriesPage;
            $scope.getSearchPage = GetSearchPage;

            //#endregion

            //#region Class builders
            $scope.getClassForCategoryLink = GetClassForCategoryLink;
            //#endregion

            //#region Api calls
            $scope.getSocialCounts = GetSocialCounts;
            //#endregion

            //#region Loader Handlers
            $scope.blogContentLoaded = false;
            $scope.transactionInProgress = false;
            $scope.redirectPage = "";
            //#endregion

            //#region Utilities
            $scope.uploadFile = UploadFile;
            $scope.goTo = GoTo;
            $scope.closeAlertBox = CloseAlertBox;
            $scope.showFooter = ShowFooter;
            $scope.getUrlForDisqus = GetUrlForDisqus;
            $scope.formatForUrl = FormatForUrl;
            $scope.redirectLinks = RedirectLinks;
            $scope.getDate = GetDate;
            $scope.getMonth = GetMonth;
            $scope.timeSince = TimeSince;
            $scope.sortItems = SortItems;
            //#endregion

            //#region Link builder
            $scope.buildPagingLink = BuildPagingLink;
            $scope.navigateBlog = NavigateBlog;

            //#endregion

            //#region Settings Article
            $scope.editArticle = EditArticle;
            $scope.backToEdit = BackToEdit;
            //#endregion

            //#endregion

            initialize();
        }
    ];

    app.controller("BlogController", BlogController);
}());