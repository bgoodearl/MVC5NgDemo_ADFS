(function () {
    'use strict';

    angular
        .module('appRehearsals')
        .controller('rehearsalEdit', rehearsalEdit);

    rehearsalEdit.$inject = ['$routeParams', 'rehearsalDataService'];

    function rehearsalEdit($routeParams, rehearsalDataService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'rehearsalEdit';
        vm.id = $routeParams.id;
        vm.rehearsal = null;
        vm.message = '';
        vm.saveEdit = saveEdit;

        activate();

        function activate() {
            if (vm.id != null) {
                if (isNaN(vm.id)) {
                    vm.message = 'invalid id';
                } else {
                    rehearsalDataService.getRehearsal(vm.id).then(getUserDataComplete, getUserDataError);
                }
            }
        }

        function getUserDataComplete(data) {
            vm.rehearsal = data;
        }

        function getUserDataError(reason) {
            vm.message = reason;
            vm.rehearsal = null;
        }

        function saveEdit() {
            alert('saveEdit not implemented');
        }
    }
})();
