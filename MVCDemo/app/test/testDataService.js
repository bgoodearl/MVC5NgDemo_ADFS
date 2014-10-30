(function () {
    'use strict';

    angular
        .module('appTest')
        .factory('testDataService', testDataService);

    testDataService.$inject = ['$q', '$http', '$log', 'appConstants'];

    function testDataService($q, $http, $log, appConstants) {
        var service = {
            getTestAuthInfo: getTestAuthInfo
        };

        return service;

        function getTestAuthInfo() {
            var url = appConstants.rootPath + 'API/TestAuthInfo';
            var d = $q.defer();
            $http.get(url)
                .success(function (data) {
                    d.resolve(data);
                })
                .error(function (data, status) {
                    var reason = '';
                    if (status = 404) {
                        reason = 'data not found';
                    } else {
                        reason = 'get data failed';
                    }
                    $log.log('Error', status.toString() + ' ' + reason);
                    d.reject(reason);
                });
            return d.promise;
        }
    }
})();