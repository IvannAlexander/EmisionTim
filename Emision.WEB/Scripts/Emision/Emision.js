angular.module('Emision', ['ngMaterial', 'ngMessages', 'md.data.table', 'md-steppers', 'ngRoute'])
    .config(function ($mdThemingProvider, $mdDateLocaleProvider) {

        $mdDateLocaleProvider.formatDate = function (date) {
            return date ? moment(date)/*.tz("America/Mexico_City")*/.format('DD/MM/YYYY') : '';
        };

        $mdDateLocaleProvider.parseDate = function (dateString) {
            var m = moment(dateString, 'DD/MM/YYYY', true);
            return m.isValid() ? m.toDate() : new Date(NaN);
        };
        // Extienda el tema rojo con un color diferente y haga que el color de contraste sea negro en lugar de blanco.
        // Por ejemplo: el texto del botón resaltado será negro en lugar de blanco.
        var Emision = $mdThemingProvider.extendPalette('Gray', {
            '50': 'F1F3F4',
            '100': 'CFD5DB',
            '200': '9CA9B6',
            '300': '617283',
            '400': '2B4B6F',
            '500': '344C67',
            '600': '2A425E',
            '700': '1F3146',
            '800': '0F253D',
            '900': '0F253D',
            'A100': '7D90A3',
            'A200': '375169',
            'A400': '233E5B',
            'A700': '14263C',
            'contrastDefaultColor': 'light',

            'contrastDarkColors': ['50', '100', //hues qué contraste debe ser 'oscuro' por defecto
                '200', '300', 'A100', 'A200'],
            'contrastLightColors': undefined
        });

        // Registre el nuevo mapa de la paleta de colores con el nombre <code> neonRed </ code>
        $mdThemingProvider.definePalette('Emision', Emision);

        // Usa ese tema para las intenciones primarias

        $mdThemingProvider.theme('default')
            .primaryPalette('Emision', {
                'default': '400',
                'hue-1': '200',
                'hue-2': '600',
                'hue-3': 'A100'
            })
            .accentPalette('grey', {
                'default': '100'
            })
            .warnPalette('red', {
                'default': '400',

            })
            .backgroundPalette('grey', {
                'default': '200'
            });
    });