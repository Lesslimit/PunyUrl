(function(punnyUrl, ko, $) {

    var vm = {
        urls: ko.observableArray([]),
        url: ko.observable(''),

        postUrl: function() {
            $.post('/v1.0/api/url/create?url=' + vm.url(),
                function(response) {
                    vm.urls.push(response);
                });
        },

        loadAllUrls: function() {
            $.getJSON('/v1.0/api/url/all',
                function(response) {
                    vm.urls(response);
                });
        }
    }

    ko.applyBindings(vm, document.getElementsByTagName('body')[0]);
})(window.punnyUrl || {}, window.ko, window.$);