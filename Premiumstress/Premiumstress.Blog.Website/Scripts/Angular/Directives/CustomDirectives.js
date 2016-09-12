(function () {

    app.directive('toggleChildrenClass', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.bind('click', function () {

                    $(this).siblings('.collapsable-list').toggleClass(attrs.toggleChildrenClass);
                    var arrowIcon = $(this).children('i.collapse-icon');

                    if (arrowIcon.hasClass('fa-plus-circle')) {
                        arrowIcon.removeClass('fa-plus-circle');
                        arrowIcon.addClass('fa-minus-circle');
                    } else {
                        arrowIcon.removeClass('fa-minus-circle');
                        arrowIcon.addClass('fa-plus-circle');
                    }
                });
            }
        };
    });

    app.directive('headerSidebar', function () {
        return {
            restrict: 'E',
            template: function (elem, attr) {

                var title = attr.title;
                var isCollapsed = attr.isCollapsed;
                var iconStyle = attr.iconStyle;

                var template;
                var arrowIconCurrent = "fa-plus-circle";

                if (isCollapsed) {
                    collapsableList = "collapsable-list collapsed-list";
                    arrowIconCurrent = "fa-minus-circle";
                } else {
                    collapsableList = "collapsable-list";
                }

                switch (title) {
                    //Blog
                    case "Category":
                        template = "Template/Categories.html";
                        break;
                    case "Suggested Articles":
                        template = "Template/Blog/SuggestedArticle.html";
                        break;
                    case "Tags":
                        template = "Template/Tags.html";
                        break;
                    case "Follow Us":
                        template = "Template/SocialsIcon.html";
                        break;
                    case "Popular Posts":
                        template = "Template/Blog/PopularPosts.html";
                        break;
                    case "Recent Comments":
                        template = "Template/Blog/RecentComments.html";
                        iconStyle = "fa-comment-o";
                        //Settings
                    case "User":
                        template = "Template/Settings/User/Navigation.html";
                        iconStyle = "fa-user";
                        break;
                    case "Article":
                        template = "Template/Settings/Article/Navigation.html";
                        iconStyle = "fa-file-text";
                        break;
                    case "Category Settings":
                        template = "Template/Settings/Category/Navigation.html";
                        iconStyle = "fa-folder-open";
                        title = "Category";
                        break;
                    case "Tag Settings":
                        template = "Template/Settings/Tags/Navigation.html";
                        iconStyle = "fa-tags";
                        title = "Tag";
                        break;
                    case "Password":
                        template = "Template/Settings/Password/Navigation.html";
                        iconStyle = "fa-tags";
                        title = "Password";
                        break;
                }

                var icon = "<i class=\"fa header-icon " + iconStyle + "\"></i>";
                var header = '<div class="collapsed-list">'
                    + '<div class="header-background">' 
                    //+ icon +
                    + "<span class='unselectable'>" + title + "</span>"
                    //+ "<i class='fa collapse-icon " + arrowIconCurrent + " text-left'></i>"
                    + "</div>";
                var collapsibleList = '<div class="' + collapsableList + '">'
                    + '<ul><li class="text-left"><div data-ng-include="\'' + template + '\'"></div></li></ul></div></div>';

                return header + collapsibleList;

            }
        }
    });

    app.directive('categoryLink', function ($parse) {
        var redirectPage, target;
        return {
            restrict: 'E',
            template: function () {
                return '<a ng-href="category/{{formatForUrl(category.Name)}}" ng-bind="category.Name"  ng-class="getClassForCategoryLink(category.Name)" target="{{redirectPage}}"> </a>';
            }
        }
    });

    app.directive('tagLink', function ($parse) {
        var redirectPage, target;
        return {
            restrict: 'E',
            template: function () {
                return '<a ng-href="tag/{{formatForUrl(tag.Keywords)}}" ng-bind="tag.Keywords" target="{{redirectPage}}"></a>';
            }

        }
    });

    app.directive("customOnChange", function () {
        return {
            restrict: "A",
            link: function (scope, element, attrs) {
                var onChangeHandler = scope.$eval(attrs.customOnChange);
                element.bind("change", onChangeHandler);
            }
        };
    });

    app.directive("customIconDir", function () {

        return {
            restrict: "E",
            template: function (elem, attr) {

                var kind = attr.kind;
                var from = attr.from;
                var title, iconStyle, nghideShowCondition;

                switch (from) {
                    case "editprofile":
                        GetProfileIconProperties();
                        break;
                    case "editcategory":
                        GetCategoryIconProperties();
                        break;
                }

                switch (kind) {
                    case "Add":
                        iconStyle = "fi-plus";
                        break;
                    case "Edit":
                        iconStyle = "fi-wrench";
                        break;
                    case "Delete":
                        iconStyle = "fi-x";
                        break;
                    case "Cancel":
                        iconStyle = "fi-x-circle";
                        break;
                }

                var icon = "<i class=\"" + iconStyle + " icon-style\" " + title + " " + nghideShowCondition + " ></i>";

                return icon;


                function GetCategoryIconProperties() {
                    if (kind == "Cancel")
                        nghideShowCondition = "ng-show=\"isEditCategory ||isAddCategory || isDeleteCategory\"";
                    else
                        nghideShowCondition = "ng-hide=\"isEditCategory ||isAddCategory || isDeleteCategory\"";

                    if (kind != "Edit")
                        title = "title = " + kind + " Category";
                    else
                        title = "title = " + kind + " Categories";
                };

                function GetProfileIconProperties() {
                    if (kind == "Edit") {
                        title = "title = " + kind + " Profile";
                        nghideShowCondition = "ng-hide=\"isEdit\"";
                    } else {
                        title = "title = " + kind + " Editing";
                        nghideShowCondition = "ng-hide=\"!isEdit\"";
                    }
                };
            }
        };
    });

    app.directive("genericTable", function () {
       
        function BuildColumnLink(linkProp, model) {
            var columnLink = '';
            if (linkProp != undefined) {
                columnLink += "<a ";
                if (linkProp.linkFunction != undefined) {
                    columnLink += "href='{{" + linkProp.linkFunction + "}}' ";
                }
                if (linkProp.clickFunction != undefined) {
                    columnLink += "ng-click='" + linkProp.clickFunction + "'";
                }
                columnLink += "class='blog-title-formatting'" +
                    "ng-bind='" + model + "'  " +
                    "target='_blank'" +
                    "title='{{" + model + "}}'" +
                    "></a>";
            }
            return columnLink;
        };

        return {
            restrict: "E",
            template: function (elem, attrs) {
                var listmodel;
                var tableHeaders = '';
                var column = '';
                var headers = JSON.parse(attrs.header);
                var columns = JSON.parse(attrs.columns);
                var dataList = attrs.list;

                //Build table headers
                angular.forEach(headers, function (value, pair) {
                    tableHeaders += '<th ng-bind="\'' + value + '\'"></th>';
                });

                angular.forEach(columns, function (value, pair) {
                    var td = "<td ";
                    angular.forEach(value, function (columnVal, columnPair) {

                        if (columnVal.rowClass != undefined)
                            td += "class='" + columnVal.rowClass + "'";

                        //If data column has a link
                        if (columnVal.link != undefined) {
                            //Loop into link properties and build the link
                            angular.forEach(columnVal.link, function (linkProp, linkPair) {
                                if (linkProp != undefined) {
                                    td += ">";
                                    td += BuildColumnLink(linkProp, columnVal.model);
                                    td += "</td>";
                                }
                            });
                        } else if (columnVal.icon != undefined) {
                            if (columnVal.icon === "withApproval") {
                                td += ">";
                                td += "<blog-approval-icon type='approvalButtons'></blog-approval-icon>";
                            } else if (columnVal.icon === "approvalStatus") {
                                td += ">";
                                td += "<blog-approval-icon type='appovalStatus'></blog-approval-icon>";
                            }

                        }
                            //Otherwise go to default
                        else {
                            td += "ng-bind='" + columnVal.model + "' title='{{" + columnVal.model + "}}'";
                            td += "></td>";
                        }

                    });
                    column += td;
                });


                var table = "<table class='large-12 column'>" +
                    "<thead>" +
                    "<tr>" + tableHeaders + "</tr>" +
                    "</thead>" +
                    "<tbody>" +
                    "<tr ng-repeat='vm in " + dataList + "'>" +
                    column +
                    "</tr>" +
                    "</tbody>" +
                    "</table>";

                return table;
            }
        };
    });

    app.directive("blogApprovalIcon", function () {
        return {
            restrict: "E",
            template: function (elem, attrs) {

                var type = attrs.type;
                var icon = '';

                if (type === 'approvalButtons') {
                    icon += "<i title='No' " +
                        "ng-click='vm.IsApproved == null ? approveBlog(vm.ID,true,currentPageNum,vm.IsOwner) : vm.IsApproved ? approveBlog(vm.ID,vm.IsApproved,currentPageNum,vm.IsOwner) : null' " +
                        "class='fa fa-times-circle' " +
                        "ng-show='vm.IsOwner' " +
                        'ng-class="vm.IsApproved == null ? \'blog-disapproved\' :  vm.IsApproved ? \'blog-disapproved\' : \'blog-neutral\' "></i>';

                    icon += "<i title='Yes' " +
                        "ng-click='vm.IsApproved == null ? approveBlog(vm.ID,false,currentPageNum,vm.IsOwner) : !vm.IsApproved ? approveBlog(vm.ID,vm.IsApproved,currentPageNum,vm.IsOwner) : null' " +
                        "class='fa fa-check-circle' " +
                        "ng-show='vm.IsOwner' " +
                        'ng-class="vm.IsApproved == null ? \'blog-approved\' :  !vm.IsApproved ? \'blog-approved\' : \'blog-neutral\' "></i>';
                } else if (type === 'appovalStatus') {
                    icon += "<i title='Not Yet Published' " +
                        "class='fa unselectable fa-close blog-disapproved' " +
                        "ng-if='!vm.IsApproved && vm.IsApproved != null'" +
                        "></i>";
                    icon += "<i title='Already Published' " +
                        "class='fa unselectable fa-check blog-approved' " +
                        "ng-if='vm.IsApproved && vm.IsApproved != null'" +
                        "></i>";
                }
                return icon;

            }
        };
    });
}());