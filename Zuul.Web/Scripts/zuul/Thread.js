zuul.Thread = function (data) {
    var self = this;

    self.id = ko.observable(data ? data.Id : null);
    self.title = ko.observable(data ? data.Title : null);
    self.message = ko.observable(data ? data.Message : null);
    self.postedByUsername = ko.observable(data ? data.PostedByUsername : null);
    self.postedByEmailAddress = ko.observable(data ? data.PostedByEmailAddress : null);
    self.postCount = ko.observable(data ? data.PostCount : null);
    self.createdDateTimeUtc = ko.observable(data ? moment.utc(data.CreatedDateTimeUtc) : null);
    self.lastPostDateTimeUtc = ko.observable(data ? moment.utc(data.LastPostDateTimeUtc) : null);
    self.replies = ko.observableArray();
    self.isExpanded = ko.observable(false);
    self.isSelected = ko.observable(false);

    self.url = ko.computed(function () {
        return zuul.constants.urls.thread.format(self.id(), self.postCount());
    });

    self.createdDateTime = ko.computed(function () {
        if (self.createdDateTimeUtc()) {
            return self.createdDateTimeUtc().local();
        }
    });

    self.lastPostDateTime = ko.computed(function () {
        if (self.lastPostDateTimeUtc()) {
            return self.lastPostDateTimeUtc().local();
        }
    });

    self.lastPostDateString = ko.computed(function () {
        if (!self.isExpanded()) {
            return self.lastPostDateTime().calendar();
        }
        return self.createdDateTime().calendar() + " " + self.createdDateTime().format("HH:mm");
    });

    self.createdTimeString = ko.computed(function () {
        return self.createdDateTime().format("HH:mm");
    });

    self.createdDateString = ko.computed(function () {
        return self.createdDateTime().format("DD.MM.YY");
    });

    self.mailtoUrl = ko.computed(function () {
        return zuul.constants.mailtoTemplate.format(self.postedByEmailAddress());
    });

    self.hasReplies = ko.computed(function () {
        return self.replies().length > 0;
    });
}