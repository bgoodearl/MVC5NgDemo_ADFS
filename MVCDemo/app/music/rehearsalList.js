(function () {
    'use strict';

    angular
        .module('appRehearsals')
        .controller('rehearsalList', rehearsalList);

    rehearsalList.$inject = ['$location', 'rehearsalDataService'];

    function rehearsalList($location, rehearsalDataService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'rehearsalList';
        vm.rehearsalCount = 0;
        vm.rehearsals = [];
        vm.initializing = true;
        vm.addRehearsal = addRehearsal;
        vm.editRehearsal = editRehearsal;

        activate();

        function activate() {
            var promises = [getRehearsals()];
        }

        function getRehearsals() {
            return rehearsalDataService.getRehearsals().then(getUserDataComplete, getUserDataError);
        }

        function getUserDataComplete(data) {
            vm.rehearsals = data;
            vm.rehearsalCount = vm.rehearsals.length;
            vm.initializing = false;
            return vm.rehearsals;
        }

        function getUserDataError(reason) {
            vm.message = reason;
            vm.rehearsal = null;
        }

        function addRehearsal() {
            alert('addRehearsal not implemented');
        }

        function editRehearsal(val) {
            if ((val != null) && (val.id != null)) {
                var path = '/edit/' + val.id.toString();
                $location.path(path);
            }
        }
    }
})();
