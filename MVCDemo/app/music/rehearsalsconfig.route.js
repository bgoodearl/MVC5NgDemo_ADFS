(function () {
    'use strict';

	var app = angular.module('appRehearsals');

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
                    templateUrl: 'app/music/rehearsalList.html',
                    //controller: 'rehearsalList',
                    title: 'Rehearsals',
                    settings: {
                        nav: 1
                    }
                }
            },
            {
                url: '/edit/:id',
                config: {
                    templateUrl: 'app/music/rehearsalEdit.html',
                    //controller: 'rehearsalEdit',
                    title: 'Edit Rehearsal',
                    settings: {
                        nav: 2
                    }
                }
            }
        ];
	}

})();