angular.module('Emision')
    .run(function ($rootScope, $templateCache) {
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (typeof (current) !== 'undefined') {
                $templateCache.remove(current.templateUrl);
            }
        });
    })
    .controller('login', function ($scope, $http, $mdDialog, $mdToast, $window, $route, $rootScope) {

        $scope.usuario = {};
        $scope.mostrarusuario = false;
        $scope.NombreUsuario = function (usuario) {

            $scope.mostrarusuario = true;
            $scope.NombreUsuario = usuario.Usuario;
        }

        $scope.usuario = {};
        $scope.Buscar = function () {
            $scope.progreso = true
            $http({
                method: 'POST',
                url: 'Index/IniciarSesion',
                data: JSON.stringify({
                    usuario: $scope.usuario
                }),
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                if (response.data != "") {
                    $rootScope.Usuario = response.data;
                    //$scope.MostrarRespuesta();
                    window.location.href = '/Perfil';
                } else {
                    $scope.MostrarRespuesta("Usuario no Encontrado");
                }
                $scope.progreso = false;
            }, function errorCallback(response, statusText) {
                console.log('error: ' + statusText);
            });
        };


        $scope.AltaUsuario = function (event) {
            $mdDialog.show({
                templateUrl: 'ModalDardeAlta',
                parent: angular.element(document.body),
                targetEvent: event,
                locals: {

                },
                controller:
                    function ($scope, $mdDialog, $mdToast, $window) {
                        $scope.usuario = {};

                        $scope.Agregar = function () {
                            $scope.progreso = true
                            $http({
                                method: 'POST',
                                url: 'Index/GuardarUsuario',
                                data: JSON.stringify({
                                    usuario: $scope.usuario
                                }),
                                headers: { 'Content-Type': 'application/json' }
                            }).then(function (response) {
                                if (response.data != null) {

                                    $scope.Ocultar();
                                } else {
                                    $scope.MostrarRespuesta("Usuario no Encontrado");
                                }
                                $scope.progreso = false;
                            }, function errorCallback(response, statusText) {
                                console.log('error: ' + statusText);
                            });
                        };


                        $scope.Ocultar = function () {
                            $mdDialog.hide();
                        };
                        $scope.Cancelar = function () {
                            $mdDialog.cancel();
                        };
                        $scope.MostrarRespuesta = function (mensaje) {
                            $mdToast.show(
                                $mdToast.simple()
                                    .position("start right")
                                    .textContent(mensaje)
                                    .action("X")
                                    .hideDelay(5000)
                            );
                        };

                        $scope.init = function () {

                        };
                        $scope.init();
                    }
            }).then(function () {

            }, function () {

            });
        };
        $scope.MostrarRespuesta = function (mensaje) {
            $mdToast.show(
                $mdToast.simple()
                    .position("start right")
                    .textContent(mensaje)
                    .action("X")
                    .hideDelay(5000)
            );
        };

        $scope.init = function () {

        };
        $scope.init();


    });