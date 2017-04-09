ko.bindingHandlers.linkify = {
    init: function (element, valueAccessor) {
        $(element).linkify(valueAccessor());
    },
    update: function (element, valueAccessor) {
        $(element).linkify(valueAccessor());
    }
};