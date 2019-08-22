(function () {
    var selectorWrapper = document.getElementById("selectorWrapper");

    var preselectCurrentTemplate = function () {
        var currentTemplateIdentifier = kentico.modalDialog.getData().currentTemplateIdentifier;
        var item = selectorWrapper.querySelector('.ktc-template-item[data-identifier="' + currentTemplateIdentifier + '"]');
        if (item !== null) {
            item.classList.add('ktc-FlatSelectedItem');
            item.classList.remove('ktc-FlatItem');
        }
    };

    var addEventListeners = function () {
        var templateElements = selectorWrapper.querySelectorAll('.ktc-template-item');
        Array.prototype.forEach.call(templateElements, function (templateElement) {
            templateElement.addEventListener("click", function ()
            {
                var element = selectorWrapper.querySelector('.ktc-template-item.ktc-FlatSelectedItem');
                if (element !== null) {
                    element.classList.remove('ktc-FlatSelectedItem');
                    element.classList.add('ktc-FlatItem');
                }

                templateElement.classList.add('ktc-FlatSelectedItem');
                templateElement.classList.remove('ktc-FlatItem');
            });
        });
    };

    addEventListeners();
    preselectCurrentTemplate();
})();