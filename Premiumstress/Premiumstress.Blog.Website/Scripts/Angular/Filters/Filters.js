(function() {

    app.filter("BlogTitleFormat", function() {
        return function(text) {
            if (text != undefined) {
                return blogTitle = text.length > 120 ? text.substring(0, 120) + "..." : text;
            }
        };
    });

    app.filter("BlogContentFormat", function() {
        return function(text) {

            if (text != undefined) {
                return blogTitle = text.length > 199 ? text + "..." : text;
            }
        };
    });

    app.filter("trusted", [
        "$sce", function($sce) {
            return function(url) {
                return $sce.trustAsResourceUrl(url);
            };
        }
    ]);


    app.filter("PagingFormat", function() {

        return function(pages) {
            var pagesReturn;
            if (pages != null)
                if (pages.length > 6) {
                    pagesReturn = pages.length - 3;
                }
            return pagesReturn;
        };
    });

    app.filter("searchResultImg", function() {
        return function(input) {
            if (input == null) {
                return "/Images/BlogImages/defaultpic.jpg";
            } else {
                return input;
            }
        };
    });

    app.filter("emailFormat", function() {
        return function(email) {
            if (email == null) {
                return "#";
            } else {
                return email;
            }
        };
    });

    app.filter("replaceToSpace", function() {
        return function(string) {
            if (string != null) {
                return string.replace(/-/g, " ");
            }
        };
    });

    app.filter("timeSince", function() {
        return function(date) {
            if (typeof date !== "object") {
                date = new Date(date);
            }

            var seconds = Math.floor((new Date() - date) / 1000);
            var intervalType;

            var interval = Math.floor(seconds / 31536000);
            if (interval >= 1) {
                intervalType = "year";
            } else {
                interval = Math.floor(seconds / 2592000);
                if (interval >= 1) {
                    intervalType = "month";
                } else {
                    interval = Math.floor(seconds / 86400);
                    if (interval >= 1) {
                        intervalType = "day";
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
                intervalType += "s";
            }

            return interval + " " + intervalType;
        };
    });
}());