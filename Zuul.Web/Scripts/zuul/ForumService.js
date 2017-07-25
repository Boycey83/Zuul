zuul.ForumService = function () {
    var self = this;

    self.registerAccount = function (username, emailAddress, password, passwordConfirm) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.registerAccount,
            data: ko.toJSON({ Username: username, EmailAddress: emailAddress, Password: password, PasswordConfirm: passwordConfirm }),
            contentType: 'application/json'
        });
    };

    self.requestPasswordReset = function (emailAddress) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.requestPasswordReset,
            data: ko.toJSON({ EmailAddress: emailAddress }),
            contentType: 'application/json'
        });
    };

    self.updatePassword = function (emailAddress, authenticationCode, password, passwordConfirm) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.updatePassword,
            data: ko.toJSON({
                EmailAddress: emailAddress,
                AuthenticationCode: authenticationCode,
                Password: password,
                PasswordConfirm: passwordConfirm
            }),
            contentType: 'application/json'
        });
    };

    self.login = function (username, password) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.login,
            data: ko.toJSON({ Username: username, Password: password }),
            contentType: 'application/json'
        });
    };

    self.logout = function () {
        return $.post(zuul.constants.api.logout);
    };

    self.getPosts = function () {
        return $.get(zuul.constants.api.getPosts);
    };

    self.getThreads = function () {
        return $.get(zuul.constants.api.getThreads);
    };

    self.getThreadsSorted = function (sortOrder) {
        return $.get(zuul.constants.api.getThreadsSorted.format(sortOrder));
    };

    self.getThreadsWithPageNumber = function (pageNumber) {
        return $.get(zuul.constants.api.getThreadsWithPageNumber.format(pageNumber));
    };

    self.getThreadsSortedWithPageNumber = function (sortOrder, pageNumber) {
        return $.get(zuul.constants.api.getThreadsSortedWithPageNumber.format(sortOrder, pageNumber));
    };

    self.getThreadReplies = function (threadId) {
        return $.get(zuul.constants.api.getThreadReplies.format(threadId));
    };

    self.createThread = function (title, message) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.createThread,
            data: ko.toJSON({ Title: title, Message: message }),
            contentType: 'application/json'
        });
    };

    self.createReplyToThread = function (threadId, title, message) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.createReplyToThread.format(threadId),
            data: ko.toJSON({ Title: title, Message: message }),
            contentType: 'application/json'
        });
    };

    self.createReplyToPost = function (threadId, postId, title, message) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.createReplyToPost.format(threadId, postId),
            data: ko.toJSON({ Title: title, Message: message }),
            contentType: 'application/json'
        });
    };

    self.createPost = function (post) {
        return $.ajax({
            type: "POST",
            url: zuul.constants.api.createPost,
            data: ko.toJSON(post),
            contentType: 'application/json'
        });
    };

    self.resetUrl = function () {
        window.history.pushState(null, null, zuul.constants.urls.base);
    };

    self.addUrlToVisited = function (url) {
        history.replaceState(null, null, url);
        history.replaceState(null, null, zuul.constants.urls.base)
    };
};