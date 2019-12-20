window.medioClinic = window.medioClinic || {};

(function (eventPage) {
    /** Base URL for the airports API endpoint. */
    eventPage.airportsBaseUrl = "http://localhost/kentico12_medioclinic/api/airports?searchphrase=";

    /** Vue.js initialization data */
    eventPage.appData = {
        flights: [],
        placeHolderInputText: "Enter city of departure",
        autoCompleteResult: [],
        autoCompleteProgress: false,
        selectedAirportName: "",
        selectedAirportIataCode: "",
        skypickerProgress: false
    };

    /**
     * Searches for airports by a search phrase.
     * @param {object} self The original lexical scope.
     * @param {string} searchPhrase The search phrase.
     */
    eventPage.onKeyUpAutoCompleteEventHandler = function (self, searchPhrase) {
        self.autoCompleteResult = [];
        self.autoCompleteProgress = false;

        if (searchPhrase.length > 1) {
            self.autoCompleteProgress = true;

            fetch(window.medioClinic.eventPage.airportsBaseUrl + searchPhrase)
                .then(response => response.json())
                .then(json => {
                    var newData = [];
                    json.forEach(function (item, index) {
                        if (item.AirportName.toLowerCase().indexOf(searchPhrase.toLowerCase()) >= 0) {
                            newData.push(item);
                        }
                    });
                    self.autoCompleteResult = newData;
                    self.autoCompleteProgress = false;
                })
                .catch(e => {
                    self.autoCompleteProgress = false;
                    self.autoCompleteResult = [];
                });
        } else {
            self.autoCompleteProgress = false;
            self.autoCompleteResult = [];
        }
    };

    /**
     * Searches for flights from a specific airport.
     * @param {object} self The original lexical scope.
     * @param {string} airportIataCode Selected airport IATA code. 
     * @param {string} airportName Selected airport name.
     * @param {string} destinationIataCode Destination airport IATA code.
     * @param {string} startDate Formatted start date.
     * @param {string} endDate Formatted end date.
     */
    eventPage.onSelectedAutoCompleteEventHandler = function (self, airportIataCode, airportName, destinationIataCode, startDate, endDate) {
        self.autoCompleteProgress = false;
        self.autoCompleteResult = [];
        self.selectedAirportName = airportName;
        self.selectedAirportIataCode = airportIataCode;
        self.skypickerProgress = true;
        var skypickerUrl = window.medioClinic.eventPage.getSkypickerUrl(airportIataCode, destinationIataCode, startDate, endDate);

        fetch(skypickerUrl)
            .then(response => response.json())
            .then(json => {
                self.flights = json.data;
                self.skypickerProgress = false;
            })
            .catch(e => self.skypickerProgress = false);
    };

    /**
     * Gets full Skypicker/Kiwi API URL for a certain flight search.
     * @param {string} startingPointIataCode Starting point IATA code.
     * @param {string} destinationIataCode Destination IATA code.
     * @param {string} startDate Formatted start date.
     * @param {string} endDate Formatted end date.
     * @returns {string} Full request URL.
     */
    eventPage.getSkypickerUrl = function (startingPointIataCode, destinationIataCode, startDate, endDate) {
        return "https://api.skypicker.com/flights?max_stopovers=0&partner=picky&curr=USD&sort=duration"
            + "&fly_from="
            + startingPointIataCode
            + "&fly_to="
            + destinationIataCode
            + "&dateFrom="
            + startDate
            + "&dateTo="
            + endDate;
    };

    /**
     * Gets local date string.
     * @param {number} unixTimeStamp UNIX time stamp.
     * @returns {string} Date string.
     */
    eventPage.getDate = function (unixTimeStamp) {
        var date = getDateInternal(unixTimeStamp);

        return date.toLocaleDateString();
    };

    /**
     * Gets local time string.
     * @param {number} unixTimeStamp UNIX time stamp.
     * @returns {string} Time string.
     */
    eventPage.getTime = function (unixTimeStamp) {
        var date = getDateInternal(unixTimeStamp);

        return date.toLocaleTimeString();
    };

    var getDateInternal = function (unixTimeStamp) {
        return new Date(unixTimeStamp * 1000);
    };
}(window.medioClinic.eventPage = window.medioClinic.eventPage || {}));