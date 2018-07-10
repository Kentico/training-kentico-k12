function PredictiveSearchExtender(clientID, textBoxID, resultsHolderID, minChars, selectionEnabled, selectedCssClass, resultsCssClass) {

    var me = this;

    this.clientID = clientID;
    this.resultsID = 'results_' + clientID;
    this.textBoxID = textBoxID;
    this.resultsHolderID = resultsHolderID;

    this.minChars = minChars;
    this.selectedCssClass = selectedCssClass;
    this.resultsCssClass = resultsCssClass;
    this.selectionEnabled = selectionEnabled;

    this.timeout = undefined;
    this.hideTimeout = undefined;
    this.doNotHide = false;
    this.selectedResult = -1;
    this.lastSearchQuery = '';

    this.RecieveSearchResults = function (arg, context) {
        me.selectedResult = -1; //unselect result
        me.lastSearchQuery = context; // save last completed query

        $cmsj('#' + me.resultsID).html(arg);  //insert results 

        // register hover event handler to each result
        if (me.selectionEnabled) {
            $cmsj('#' + me.resultsID + ' > :not(.nonSelectable)').hover(
                function () {
                    $cmsj('#' + me.resultsID + ' > *').removeClass(me.selectedCssClass);
                    $cmsj(this).addClass(me.selectedCssClass);
                    me.selectedResult = $cmsj('#' + me.resultsID + ' > :not(.nonSelectable)').index($cmsj(this));
                }, function () { }
            );

            $cmsj('#' + me.resultsID + ' > .nonSelectable').hover(
                function () {
                    $cmsj('#' + me.resultsID + ' > *').removeClass(me.selectedCssClass);
                }, function () { }
            );
        }

        // show results
        if (arg != '') {
            $cmsj('#' + me.resultsID).show();
        } else {
            $cmsj('#' + me.resultsID).hide();
        }
    };

    this.HideResults = function () {
        $cmsj('#' + me.resultsID).hide();
    };

    // navigates the results when down arrow key is pressed
    this.MoveSelectedDown = function () {
        var searchResultsDiv = $cmsj('#' + me.resultsID);
        var searchResults = $cmsj('#' + me.resultsID + ' > :not(.nonSelectable)');

        if (searchResultsDiv.is(':visible')) { // results are visible - navigate results
            if (me.selectedResult + 1 <= searchResults.length - 1) { // result is not the last
                me.selectedResult++;
                searchResults.removeClass(me.selectedCssClass);
                searchResults.eq(me.selectedResult).addClass(me.selectedCssClass);
            } else { // result is the last - unselect result (loop)
                me.selectedResult = -1;
                searchResults.removeClass(me.selectedCssClass);
            }
        } else if (searchResultsDiv.html() != '') { // show results
            me.selectedResult = 0;
            searchResults.removeClass(me.selectedCssClass);
            searchResults.eq(me.selectedResult).addClass(me.selectedCssClass);
            searchResultsDiv.show();
        }
    };

    // navigates the results when up arrow key is pressed
    this.MoveSelectedUp = function () {
        var searchResults = $cmsj('#' + me.resultsID + ' > :not(.nonSelectable)');

        if (me.selectedResult > 0) { // result is not first - navigate
            me.selectedResult--;
            searchResults.removeClass(me.selectedCssClass);
            searchResults.eq(me.selectedResult).addClass(me.selectedCssClass);
        } else if (me.selectedResult == 0) { // result is first - unselect result
            me.selectedResult = -1;
            searchResults.removeClass(me.selectedCssClass);
        } else if (me.selectedResult == -1) { // no result is selected - loop (start drom bottom)
            me.selectedResult = searchResults.length - 1;
            searchResults.removeClass(me.selectedCssClass);
            searchResults.eq(me.selectedResult).addClass(me.selectedCssClass);
        }
    };

    // character was typed in search text box
    this.KeyChanged = function () {
        var query = $cmsj('#' + me.textBoxID).val(); // get the search query

        if (me.timeout) { clearTimeout(me.timeout); } // if call timeout is set, reset it (cancel the search)

        if (query.length >= me.minChars) { // query is long enough
            if (query !== me.lastSearchQuery) // query is different from the last query
            {
                me.timeout = setTimeout(function () { me.CallPredictiveSearch(query, query); }, 500); // set call timeout (start the search countdown)
            }
        } else { // query is not long enough - hide results
            me.timeout = setTimeout(function () {
                $cmsj('#' + me.resultsID).html('').hide();
                me.lastSearchQuery = '';
            }, 500);
        }
    };

    // redirects to selected result 
    // tries to find href attribute and redirects to ist's URL
    this.RedirectToResult = function (e) {
        var selectedResultElem = $cmsj('#' + me.resultsID + ' .' + me.selectedCssClass); // get selected result 

        if (selectedResultElem.length == 1) { // selected result is found and only one result is selected
            var href = '';
            href = selectedResultElem.attr('href'); // search for href in result element

            // search for href in childs elements 
            if (href == undefined || href == '') {
                href = selectedResultElem.find('[href]').attr('href');
            }

            // go to search result
            if (href != undefined && href != '') {
                e.preventDefault();
                document.location.href = href;
            }
        }
    };

    // startup
    $cmsj(function () {

        // inserts results div to seach holder element
        $cmsj('#' + me.resultsHolderID).html('<div id=\'' + me.resultsID + '\' class=\'' + me.resultsCssClass  + '\'></div>');
    
        // hides the results
        me.HideResults();

        // disables autocomplete for search text box
        $cmsj('#' + me.textBoxID).attr('autocomplete', 'off') 
        .keyup(me.KeyChanged) // search query text change is registered on keyup because on keydown text is not written in the textbox
        .keydown(function (e) { // keydown because we want to support loop (key is not up while looping)
            switch (e.keyCode) {
                case 38: // up
                    if (me.selectionEnabled) { me.MoveSelectedUp(); }
                    e.preventDefault();
                    break;
                case 40: // down
                    if (me.selectionEnabled) { me.MoveSelectedDown(); }
                    e.preventDefault();
                    break;
                case 27: //escape
                    $cmsj('#' + me.resultsID).hide();
                    break;
            }
        })
        .bind('keypress keydown keyup', function(e) { // enter
            if (e.keyCode == 13) { me.RedirectToResult(e); }
        })
        .blur(function () { // registers blur event - when focus from search text box is lost, hide the results
            if (!me.doNotHide) { // check if focus was lost on purpuse, results shouldn't be hidden in that case
                if (me.hideTimeout) { clearTimeout(me.hideTimeout); } // results are not hidden immidiately (effects)
                me.hideTimeout = setTimeout(function () { me.HideResults(); }, 300);
            }
        });

        // when mouse is pressed in the results element area, prevent the results from hidding
        $cmsj('#' + me.resultsID).mousedown(function () {
            me.doNotHide = true;
        })
        // when mouse is released, check if hiding is disabled (mouse was pressed in the results area)
        // and enable hidding and restore focus to text box. 
        .mouseup(function () { 
            if (me.doNotHide) {
                me.doNotHide = false;
                $cmsj('#' + me.textBoxID).focus();
            }
        })
        // when mouse was pressed in the results area and mouse leaves the results area enable hiding and restore focus
        .mouseleave(function () {
            if (me.doNotHide) {
                me.doNotHide = false;
                $cmsj('#' + me.textBoxID).focus();
            }
        });
    });
}