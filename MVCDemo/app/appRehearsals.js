(function () {
    'use strict';

    var appRehearsals = angular.module('appRehearsals', [
        // Angular modules 
        //'ngAnimate',
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        
    ]);

    //Pick up rootPath from div in _Layout.cshtml
    appRehearsals.constant('appConstants', initializeAppConstants());
    appRehearsals.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    });

    function initializeAppConstants() {
        var appConstants = {
            rootPath: '/',
            rootPathSet: false
        };
        var pathData = JSON.parse($('#lo-appdata').attr('data-appdata'));
        if (pathData != null) {
            appConstants.rootPath = pathData.rootPath;
            appConstants.rootPathSet = true;
        }
        return appConstants;
    }

    // Execute bootstrapping code and any dependencies.
    // TODO: inject services as needed.
    appRehearsals.run(['$route', '$log', 'appConstants',
        function ($route, $log, appConstants) {
            // Include $route to kick-start the router
            if (!appConstants.rootPathSet) {
                $log.log('Error', 'rootPath not set in appConstants - default used.');
            }
        }
    ]);

})();