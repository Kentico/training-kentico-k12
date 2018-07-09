define(function () {
    return function (applicationName, siteFolderName) {
        require.config({
            baseUrl: applicationName + '/CMSPages/GetResource.ashx?scriptfile=/' + siteFolderName + '/Scripts/',
            paths: {
                'domReady': 'Vendor/DomReady/DomReady',
                'jQuery': 'Vendor/jQuery/jQuery.min'
            },
            shim: {
                jQuery: {
                    exports: 'jQuery',
                    init: function() {
                        return this.jQuery.noConflict(true);
                    }
                }
            }
        });

        require(['Modules/ScrollToMap', 'Modules/FloatingBox', 'Modules/MobileMenu',
                 'Modules/DeleteAddress', 'Modules/ChangeImgSrcToBackground',
                'Modules/ScrollToForm', 'Modules/ProductFilter', 'Modules/ResponsiveMenuDropdown'], function () {

        });
    }
});