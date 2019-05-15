(function () {
    'use strict';

    function ProntoDashboard($scope, $http, logResource, entityResource) {
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
            var today = new Date();
            var startDate = new Date().setDate(today.getDate() - 30);
            var userLogOptions = {
                pageSize: 100,
                pageNumber: 1,
                orderDirection: "Descending",
                sinceDate: new Date(startDate)
            };

            logResource.getPagedUserLog(userLogOptions)
                .then(function (response) {
                    var filteredLogEntries = [];
                    var ids = [];
                    angular.forEach(response.items, function (item) {
                        if (item.nodeId > 0 && !ids.includes(item.nodeId)) {
                            ids.push(item.nodeId);
                            if (item.logType === "Save" || item.logType === "Publish") {
                                if (item.comment.match("(\\bContent\\b|\\bMedia\\b)")) {
                                    if (item.comment.indexOf("Media") > -1) {
                                        item.editUrl = "media/media/edit/" + item.nodeId;
                                        item.entityType = "Media";
                                    }
                                    if (item.comment.indexOf("Content") > -1) {
                                        item.editUrl = "content/content/edit/" + item.nodeId;
                                        item.entityType = "Document";
                                    }
                                }
                                if (typeof item.entityType !== 'undefined') {
                                    entityResource.getById(item.nodeId, item.entityType).then(function (ent) {
                                        item.Content = ent;
                                    });

                                    filteredLogEntries.push(item);
                                }
                            }
                        }
                    });
                    vm.UserLogHistory.items = filteredLogEntries;
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
                vm.dashboard = response.data;
                vm.loading = false;
            });
        };

        init();
    }

    angular.module('umbraco').controller('Pronto.Dashboard.Controller', ProntoDashboard);

})();