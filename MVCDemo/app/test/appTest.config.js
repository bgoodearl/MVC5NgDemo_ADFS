(function () {
    'use strict';

    angular
        .module('appTest')
        .factory('appTest', appTest);

    appTest.$inject = ['$http'];

    function appTest($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();