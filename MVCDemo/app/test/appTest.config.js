(function () {
    'use strict';

    angular
        .module('appTest')
        .factory('appConfig', appConfig);

    appConfig.$inject = ['$log'];

    function appConfig($log) {
        var configData = {
            token: null
        };

        setAppConfigData($log);
        return configData;

        function setAppConfigData($log) {
            try {
                var tokenData = JSON.parse($('#at-jtk1').attr('data-tk'));
                if (tokenData != null) {
                    configData.token = tokenData.tk;
                }
                if ((configData.token == null) || (configData.token.length == 0)) {
                    $log.log('Error', 'could not get token from at-jtk1');
                }
            } catch (error) {
                $log.log('Error', error);
            }
        }
    }
})();