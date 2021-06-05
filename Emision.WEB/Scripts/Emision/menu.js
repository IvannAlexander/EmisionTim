angular.module('Emision')
    .run(['$anchorScroll', function ($anchorScroll) {
        $anchorScroll.yOffset = 50; // always scroll by 50 extra pixels
    }])
    .run(function ($rootScope, $templateCache) {
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (typeof (current) !== 'undefined') {
                $templateCache.remove(current.templateUrl);
            }
        });
    })
    .controller('menu', function ($mdMedia, $scope, $http, $timeout, $mdDialog, $interval, $mdToast, $mdSidenav, $location, $window, $rootScope, $route) {
        $scope.usuarionombre = '';
        $scope.ObtenerUsuario = function () {
            $http({
                method: 'GET',
                url: 'Perfil/Obtenerusuario',
                headers: { 'Content-Type': 'application/json' },
            }).then(function (response) {
                if (response.data != null) {
                    $scope.usuarionombre = response.data;
                }
                else {
                    window.location.href = '/Index';
                }
            }, function errorCallback(response, statusText) {
                console.log('error: ' + statusText);
            });
        };
        $scope.Usuariotrue = false;
        $scope.tieneusuario = function () {
            if ($rootScope.Usuario != null) {
                $scope.Usuariotrue = true;
            }
        }
        $scope.BorrarUsuario = function () {
            $http({
                method: 'POST',
                url: 'Perfil/BorrarUsuario',
                data: JSON.stringify({}),
                headers: { 'Content-Type': 'application/json' },
            }).then(function (response) {
                if (response.data) {
                    $scope.usuarionombre = ''
                    window.location.href = '/Index';
                }
            }, function errorCallback(response, statusText) {
                console.log('error: ' + statusText);
            });
        };

        $scope.mostrarusuario = false;
        $scope.NombreUsuario = function (usuario) {

            $scope.mostrarusuario = true;
            $scope.NombreUsuario = usuario.Usuario;
        }
        $scope.visible = true;
        $scope.show = function () {
            $scope.visible = !$scope.visible;
        }


        $scope.IngresoUsuario = function (event) {
            $mdDialog.show({
                templateUrl: 'ModalIngresar',
                parent: angular.element(document.body),
                targetEvent: event,
                locals: {

                },
                controller:
                    function ($scope, $mdDialog, $mdToast) {
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
                                if (response.data != null) {
                                    $rootScope.Usuario = response.data;
                                    //$scope.MostrarRespuesta();
                                    window.location.href = '/Perfil';

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
                $scope.NombreUsuario($scope.usuario);
            }, function () {

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
        //Modals


        $scope.isOpen = false;

        $scope.demo = {
            isOpen: false,
            count: 0,
            selectedDirection: 'right'
        };

        $scope.init = function () {
            $scope.ObtenerUsuario();
        }
        $scope.init();
    });