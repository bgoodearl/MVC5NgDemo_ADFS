(function () {
    'use strict';

    angular
        .module('appRehearsals')
        .factory('rehearsalDataService', rehearsalDataService);

    rehearsalDataService.$inject = ['$q', '$http', '$log', 'appConstants'];

    function rehearsalDataService($q, $http, $log, appConstants) {
        var service = {
            getRehearsal: getRehearsal,
            getRehearsals: getRehearsals
        };

        return service;

        function getRehearsals() {
            var d = $q.defer();
            var url = appConstants.rootPath + 'API/Rehearsals';
            $http.get(url)
                .success(function(data) {
                    d.resolve(data);
                })
                .error(function(data, status) {
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

        function getRehearsal(id) {
            var d = $q.defer();
            var url = appConstants.rootPath + 'API/Rehearsals/' + id;
            $http.get(url)
                .success(function(data) {
                    d.resolve(data);
                })
                .error(function(data, status) {
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