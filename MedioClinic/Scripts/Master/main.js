document.addEventListener("DOMContentLoaded", function () {
    var sidenavElement = document.querySelector(".sidenav");
    M.Sidenav.init(sidenavElement);
    var dropdownElement = document.querySelector(".dropdown-trigger");

    M.Dropdown.init(dropdownElement, {
        hover: false
    });
});

(function (medioClinic) {
    /**
     * Shows a system message to the user via the ".kn-system-messages" element.
     * @param {string} message The system message.
     * @param {string} type Either "info", "warning", or "error".
     */
    medioClinic.showMessage = function (message, type) {
        var messageElement = document.querySelector(".kn-system-messages");

        if (message && type) {
            if (type === "info") {
                messageElement.appendChild(buildMessageMarkup(message, "light-blue lighten-5"));
                console.info(message);
            } else if (type === "warning") {
                messageElement.appendChild(buildMessageMarkup(message, "yellow lighten-3"));
                console.warn(message);
            } else if (type === "error") {
                messageElement.appendChild(buildMessageMarkup(message, "red lighten-3"));
                console.error(message);
            }
        }
    };

    /**
     * Builds an HTML element of a system message.
     * @param {string} message The system message.
     * @param {string} cssClasses The CSS class selectors.
     * @returns {HTMLElement} The <p> element.
     */
    var buildMessageMarkup = function (message, cssClasses) {
        var paragraph = document.createElement("p");
        paragraph.classList = cssClasses;
        paragraph.innerText = message;

        return paragraph;
    };
}(window.medioClinic = window.medioClinic || {}));