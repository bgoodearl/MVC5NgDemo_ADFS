(function () {
    'use strict';

    angular
        .module('appTest')
        .controller('testMain', testMain);

    testMain.$inject = ['$location']; 

    function testMain($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Test Main';

        activate();

        function activate() {
        }
    }
})();
