(function () {
    'use strict';

    function ProntoDashboard($scope, $http, logResource) {
        var apiUrl;
        var vm = this;
        vm.loading = true;
        vm.UserLogHistory = [];

        function init() {
            apiUrl = Umbraco.Sys.ServerVariables["Pronto"]["GetDashboardDataApiUrl"];
            $scope.data = [];
            $scope.getUsefulLinks();
            $scope.getUserLog();
        }

        $scope.getUserLog = function () {
            var userLogOptions = {
                pageSize: 10,
                pageNumber: 1,
                orderDirection: "Descending",
                sinceDate: new Date(2018, 0, 1)
            };

            logResource.getPagedUserLog(userLogOptions)
                .then(function (response) {
                    console.log(response);
                    vm.UserLogHistory = response;
                    vm.UserLogHistory.items = response.items;
                });
        };

        $scope.getUsefulLinks = function () {
            $http({
                method: 'GET',
                url: apiUrl + 'GetDashboardData/',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                console.log(response.data);
                vm.dashboard = response.data;
                vm.loading = false;
            });
        };

        init();
    }

    angular.module('umbraco').controller('Pronto.Dashboard.Controller', ProntoDashboard);

})();