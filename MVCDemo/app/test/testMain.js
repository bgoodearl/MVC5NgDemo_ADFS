(function () {
    'use strict';

    angular
        .module('appTest')
        .controller('testMain', testMain);

    testMain.$inject = ['$location', 'appConfig', 'testDataService'];

    function testMain($location, appConfig, testDataService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Test Main';
        vm.message = 'Initial Message...';
        vm.count = '0';
        vm.serverValue1 = '';
        vm.accessToken = '';
        vm.accessTokenLen = 0;
        vm.testAuthInfo = null;
        vm.getTestAuthInfo = getTestAuthInfo;

        activate();

        function activate() {
            if (appConfig.token) {
                vm.accessToken = appConfig.token;
                vm.accessTokenLen = appConfig.token.length;
            }

            vm.message = 'Loaded';
        }

        function getTestAuthInfo() {
            if (vm.accessTokenLen) {
                vm.message = 'Getting Data...';
                testDataService.getTestAuthInfo().then(getTestAuthInfoComplete, getTestAuthInfoError);
            } else {
                vm.message = 'no token - cannot get data';
            }
        }

        function getTestAuthInfoComplete(data) {
            vm.testAuthInfo = data;
            vm.message = 'Got Test Auth Info';
        }
        function getTestAuthInfoError(reason) {
            vm.testAuthInfo = null;
            vm.message = reason;
        }

    }
})();
