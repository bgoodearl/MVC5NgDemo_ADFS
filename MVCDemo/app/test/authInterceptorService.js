(function () {
    'use strict';

    angular
        .module('appTest')
        .factory('authInterceptorService', authInterceptorService);

    authInterceptorService.$inject = ['$q', '$location', 'appConfig'];

    function authInterceptorService($q, $location, appConfig) {
        var serviceFactory = {
            request: _request,
            responseError: _responseError
        };

        return serviceFactory;

        function _request(config) {
            config.headers = config.headers || {};

            if (appConfig.token) {
                config.headers.Authorization = 'Bearer ' + appConfig.token;
            }
            return config;
        }

        function _responseError(rejection) {
            if (rejection.status == 401) {
                $location.path('/authError')
            }
            return $q.reject(rejection);
        }

    }
})();