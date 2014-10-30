(function () {
    'use strict';

    var app = angular.module('appTest');

    // Collect the routes
    app.constant('routes', getRoutes());

    app.config(['$routeProvider', 'routes', 'appConstants', routeConfigurator]);
    function routeConfigurator($routeProvider, routes, appConstants) {

        routes.forEach(function (r) {
            var routeConfig = {
                templateUrl: appConstants.rootPath + r.config.templateUrl,
                title: r.title,
                settings: r.settings
            };
            $routeProvider.when(r.url, routeConfig);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    //use relative paths for templateUrl here.  Root supplied from appConstants
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/test/testMain.html',
                    //controller: 'testMain',
                    title: 'Test Main',
                    settings: {
                        nav: 1
                    }
                }
            },
            {
                url: '/authError',
                config: {
                    templateUrl: 'app/test/testAuthError.html',
                    //controller: 'testAuthError',
                    title: 'Auth Error',
                    settings: {
                        nav: 2
                    }
                }
            }
        ];
    }

})();