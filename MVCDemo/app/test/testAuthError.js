(function () {
    'use strict';

    angular
        .module('appTest')
        .controller('testAuthError', testAuthError);

    testAuthError.$inject = ['$location']; 

    function testAuthError($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Auth Error';

        activate();

        function activate() { }
    }
})();
