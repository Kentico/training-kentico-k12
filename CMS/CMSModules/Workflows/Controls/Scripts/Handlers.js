/*
*   This class contains handlers for selecting graph elements.
*/
var GraphSelectionHandler = $class({

    /*
    *   General handler for purposes of deselecting element.
    */
    getDeselectItemHandler: function (graph) {
        return function (e) {
            var selectedItem = graph.selectedItem;
            if (selectedItem != null) {
                if (selectedItem.removeReattachHelper) {
                    selectedItem.removeReattachHelper();
                    $cmsj(selectedItem.canvas).removeClass("Selected");
                    selectedItem.endpoints[0].node.ensureNotFront();
                    selectedItem.endpoints[1].node.ensureNotFront();
                    selectedItem.endpoints[0].removeClass("Selected");
                    selectedItem.endpoints[1].removeClass("Selected");
                } else if (selectedItem.hasClass('Node')) {
                    selectedItem.removeClass("Selected");
                    var node = selectedItem.data("nodeObject");
                    node.ensureNotFront();
                }
                graph.selectedItem = null;
            }
        };
    },


    /*
    *   Handles node selection.
    */
    getNodeSelectHandler: function (graph) {
        var deselectItem = graphSelectHandler.getDeselectItemHandler(graph);

        return function (event) {
            var selectedItem = graph.selectedItem;
            if (selectedItem != event.currentTarget) {
                var nodeJQ = $cmsj(event.currentTarget);
                deselectItem();
                nodeJQ.addClass("Selected");
                graph.selectedItem = nodeJQ;
                var node = nodeJQ.data("nodeObject");
                node.ensureFront();
            }
            event.stopPropagation();
        };
    },


    /*
    *   Handles connection selection.
    */
    getConnectionSelectHandler: function (graph) {
        var deselectItem = graphSelectHandler.getDeselectItemHandler(graph);

        return function (connection, event) {
            var selectedItem = graph.selectedItem;
            if (selectedItem != connection) {
                deselectItem();
                graph.selectedItem = connection;
                connection.createReattachHelper();
                $cmsj(connection.canvas).addClass("Selected");
                connection.endpoints[0].node.ensureFront();
                connection.endpoints[1].node.ensureFront();
                connection.endpoints[0].addClass("Selected");
                connection.endpoints[1].addClass("Selected");
            }
            if (event) {
                event.stopPropagation();
            }
        };
    },


    /*
    *   Handles endpoint selection.
    */
    getEndpointSelectHandler: function (graph) {
        var endpointShouldBeSelected = graphSelectHandler.endpointShouldBeSelected;
        var selectConnection = graphSelectHandler.getConnectionSelectHandler(graph);

        return function (event) {
            var endpoint = $cmsj(this).data('jsPlumbObject');
            if (!endpoint) {
                endpoint = $cmsj(this).parent().data('jsPlumbObject');
            }

            if (endpointShouldBeSelected(endpoint)) {
                selectConnection(endpoint.connections[0], event);
            }
        };
    },


    /*
    *   Controls whether or not should connection on endpoint be selected.
    */
    endpointShouldBeSelected: function (endpoint) {
        return endpoint.isSource && !endpoint.isReattachHelper && endpoint.connections[0];
    },


    /*
    *   Highlights node to notify that connection can be attached.
    */
    highlightNode: function (event, ui) {
        var endpoint = ui.draggable.data("jsPlumbObject");
        var node = $cmsj(this).data("nodeObject");
        if (endpoint && node != endpoint.node && endpoint.oppositeEndpoint && node != endpoint.oppositeEndpoint.node) { // is potential situation for highlighting
            var idx = endpoint.idx;
            if (idx == 0) {
                var sourcepoint = node.getWildcardSourcePoint();
                if (sourcepoint && !sourcepoint.isFull()) {
                    node.nodeJQ.addClass("Highlight");
                }
            } else {
                node.nodeJQ.addClass("Highlight");
            }
        }
    },


    /*
    *   Highlights endpoint to notify that connection can be attached.
    */
    highlightEndpoint: function (event, ui) {
        var endpoint = ui.draggable.data("jsPlumbObject");
        var target = $cmsj(this).data("jsPlumbObject");
        if (endpoint && (!target.isFull() || endpoint.idx == 1) && endpoint.oppositeEndpoint && endpoint.oppositeEndpoint.node != target.node) { // is potential situation for highlighting
            var idx = endpoint.idx;
            if ((idx == 1 && (!target.isSource || target.isReattachHelper) && target.node != endpoint.node) || (idx == 0 && target.isSource)) {  // Is correct source or target point
                $cmsj(this).addClass("Highlight");
            }
        }
    },


    /*
    *   Removes highlight from element.
    */
    removeHighlight: function () {
        $cmsj(this).removeClass("Highlight");
    },

    /*
    *   Reacts on mousemove event and checks for atempts to drag connection.
    */
    getConnectionDraggingHandler: function (graph) {
        var selectConnection = graphSelectHandler.getConnectionSelectHandler(graph);
        return function (connector, e) {
            if (!connector.isMouseDown()) {
                return;
            }
            var targetJQ = $cmsj(connector.canvas);
            var hoverConnections = $cmsj("._jsPlumb_hover._jsPlumb_connector").not(targetJQ);
            graphSelectHandler.removeHoverFromConnections(hoverConnections);
            var conn = targetJQ.data("jsPlumbObject");
            var newEvent = $cmsj.extend(true, {}, e);
            if (graph.selectedItem != conn) {
                selectConnection(conn);
            }
            graphControlHandler.stopEvent(e);
            newEvent.currentTarget = newEvent.target = conn.endpoints[1].canvas;

            var startDragging = graphControlHandler.getStartDraggingConnection(newEvent, graph, connector);
            var condition = graphSelectHandler.getIsEndpointDragHandlerComplete(conn.endpoints[1]);
            graphControlHandler.waitUntil(startDragging, condition, 10);
        }
    },


    /*
    *   Checks if dragged endpoint was already painted.
    */
    getIsEndpointDragHandlerComplete: function (endpoint) {
        return function () {
            return $cmsj(endpoint.canvas).width() > 0 && endpoint.canvas.src.length > 0;
        }
    },


    /*
    *   Changes all connections back from hover state.
    */
    getRemoveHoverFromAllConnectionsHandler: function(){
        return function (e) {
            if (!e.jsPlumbProcessed && $cmsj(".ui-draggable-dragging").length == 0) {
                var hoverConnections = $cmsj("._jsPlumb_hover._jsPlumb_connector");
                graphSelectHandler.removeHoverFromConnections(hoverConnections);
            }
        }
    },


    /*
    *   Changes connections back from hover state.
    */
    removeHoverFromConnections: function (connectionsJQ) {
        connectionsJQ.each(function () {
            var connection = $cmsj(this).data("jsPlumbObject");
            connection.setHover(false);
        });
    }
});


/*
*   Handles controls of graph - mostly nodes manipulations
*/
var GraphControlHandler = $class({

    /*
    *   Returns handler for starting the connection drag.
    */
    getStartDraggingConnection: function (newEvent, graph, connector) {
        return function () {
            if (!connector.isMouseDown()) {
                return;
            }
            var targetJQ = $cmsj(newEvent.target);
            targetJQ.draggable("option", "cursorAt", { top: targetJQ.height() / 2, left: targetJQ.width() / 2 });
            newEvent.type = "mousedown";
            newEvent.jsPlumbProcessed = true;
            connector.releaseMouse();
            targetJQ.trigger(newEvent);
            graphControlHandler.preventDeselectAfterDragInIE9(targetJQ, graph.graphJQ);
            setTimeout(function (newEvent) {
                return function () {
                    var targetJQ = $cmsj(newEvent.target);
                    targetJQ.draggable("option", "cursorAt", false);
                }
            }(targetJQ), 100);
        };
    },
    

    /*
    *   Prevents deselecting of connection after dropping connection - IE9 hack
    */
    preventDeselectAfterDragInIE9: function(dragJQ, graphJQ) {
        if ($cmsj.browser.msie) {
            graphJQ.bind("click.prevent", function(e) {
                e.stopImmediatePropagation(); e.preventDefault(); e.bubbles = false; e.jsPlumbProcessed = true;
            });
            dragJQ.bind("dragstop", function (graphJQ) {
                return function() {
                    setTimeout(function () { graphJQ.unbind("click.prevent") }, 100);
                }
            }(graphJQ));
        }
    },


    /*
    *   Pooling until condition is true, than calls function.
    */
    waitUntil: function (func, getCondition, cycle) {
        if (getCondition()) {
            func();
        } else {
            setTimeout(graphControlHandler.getWaitUntil(func, getCondition, cycle), cycle);
        }
    },


    /*
    *   Closure over waitUntil function - IE workaround.
    */
    getWaitUntil: function (func, getCondition, cycle) {
        return function () {
            graphControlHandler.waitUntil(func, getCondition, cycle);
        }
    },


    /*
    *   Completelly stops event.
    */
    stopEvent: function (e) {
        if (e) {
            e.stopImmediatePropagation(); e.preventDefault(); e.bubbles = false; e.jsPlumbProcessed = true;
        }
    },


    /*
    *   Handles resize of window.
    */
    getResizeWrapperHandler: function (wrapper) {
        return function (event) {
            var position = wrapper.getViewPosition();

            position.left += wrapper.containerJQ.width();
            position.top += wrapper.containerJQ.height();

            wrapper.setContainerDimensions();
            wrapper.setWrapperDimensions();

            position.left -= wrapper.containerJQ.width();
            position.top -= wrapper.containerJQ.height();

            wrapper.setViewTo(position);
        }
    },


    /*
    *   Event for moving pane by dragging element of graph.
    */
    getMovePaneWhileDraggingHandler: function (wrapper) {
        return function (event) {
            var containerPosition = wrapper.containerJQ.position();
            var containerSize = { width: wrapper.containerJQ.width(), height: wrapper.containerJQ.height() };
            var deltaX = 0;
            var deltaY = 0;
            if (event.clientX < containerPosition.left + 40) {
                deltaX = 1;
            } else if (event.clientX > containerPosition.left + containerSize.width - 40) {
                deltaX = -1;
            }

            if (event.clientY < containerPosition.top + 40) {
                deltaY = 1;
            } else if (event.clientY > containerPosition.top + containerSize.height - 40) {
                deltaY = -1;
            }
            wrapper.moveViewBy(deltaX, deltaY);
        }
    },


    /*
    *   Handles wheel event.
    */
    getWrapperMouseWheelHandler: function (wrapper) {
        return function (event, deltaY) {
            if ($cmsj(".ui-draggable-dragging").length == 0) {
                wrapper.moveViewBy(0, deltaY * 4);
                return false;
            }
        }
    },


    /*
    *   Handles click on button to add switch case.
    */
    getAddSwitchCaseHandler: function (graph) {
        var service = graph.serviceHandler;

        return function (event) {
            var nodeId = $cmsj(this).parent().parent(".Node").data('nodeObject').id;
            var node = graph.getNode(nodeId);

            if (node.sourcePointCanBeAdded()) {
                service.addSwitchCase(nodeId);
            } else {
                alert(node.getMaxSourcePointCountError());
            }
        };
    },


    /*
    *   Handles click on button for removing switch case.
    */
    getRemoveSwitchCaseHandler: function (graph) {
        var service = graph.serviceHandler;

        return function (event) {
            var caseJQ = $cmsj(this).parent(".Case");
            var node = caseJQ.parents(".Node").data('nodeObject');
            var nodeId = node.id;
            var caseId = caseJQ.data('id');

            if (node.sourcePointCanBeRemoved()) {
                var cont = confirm(node.getCaseDeleteConfirmation());
                if (cont) {
                    service.removeSwitchCase(nodeId, caseId);
                }
            } else {
                alert(node.getMinSourcePointCountError());
            }
        };
    },


    /*
    *   Handles keyboard input while graph wrapper has focus.
    */
    getKeyDownHandler: function (graph) {
        return function (event) {
            if (!graph.myJsPlumb.currentlyDragging) {
                var dragging = $cmsj(".ui-draggable-dragging");
                if (dragging.length) {
                    dragging.remove();
                }
                else if (event.keyCode == 46) { //46 == DELETE
                    graph.removeSelectedItem();
                }
            }
        };
    },


    /*
    *   Handler for reattaching / creating connections.
    */
    getAttachConnectionHandler: function (graph) {
        var service = graph.serviceHandler;
        var select = graphSelectHandler.getConnectionSelectHandler(graph);

        return function (value, event) {
            var connection = value.connection;
            var sourceNode = $cmsj('#' + value.sourceId).data('nodeObject');
            var targetNode = $cmsj('#' + value.targetId).data('nodeObject');

            if (connection.ID) {
                if (value.indexEdited == 0) {
                    service.editConnectionStart(connection, sourceNode.id, value.sourceEndpoint.ID);
                    sourceNode.ensureFront();
                } else {
                    service.editConnectionEnd(connection, targetNode.id);
                    targetNode.ensureFront();
                }
            } else {
                service.createConnection(sourceNode.id, targetNode.id, value.sourceEndpoint.ID, connection);
                sourceNode.bindEventsToConnection(connection);
                sourceNode.ensureFront();
                targetNode.ensureFront();
            }
            select(connection, event);
        };
    },


    /*
    *   Handler for setting position of node.
    */
    getSetNodePositionHandler: function (graph) {
        var service = graph.serviceHandler;
        var graphJQ = graph.graphJQ;

        return function (event) {
            var nodeJQ = $cmsj(event.target);
            if (!nodeJQ.hasClass("Node")) {
                nodeJQ = nodeJQ.parents(".Node");
            }
            var node = nodeJQ.data('nodeObject');
            var x = nodeJQ.offset().left - graphJQ.offset().left;
            var y = nodeJQ.offset().top - graphJQ.offset().top;
            service.setNodePosition(node.id, x, y);
        };
    },


    /*
    *   Optimizes dragging of whole graph.
    */
    getStartDraggingHandler: function (graph) {
        var jsPlumbInstance = graph.myJsPlumb;
        return function (e, ui) {
            $cmsj(".UniGraph").addClass("Move");
            var tooltipsJQ = $cmsj(".tooltip");
            tooltipsJQ.clearQueue();
            tooltipsJQ.hide();
            var targetJQ = $cmsj(e.target);

            if (!targetJQ.hasClass("_jsPlumb_endpoint")) {
                jsPlumbInstance.currentlyDragging = true;
            }

            if (targetJQ.hasClass("Node") && graph.snapToGrid) {
                var position = targetJQ.position();
                var width = targetJQ.width() + parseInt(targetJQ.css("border-left-width"));
                var tick = targetJQ.data("uiDraggable").offset.click;

                if (isRTL) {
                    position.left += width;
                }

                tick.top += position.top - graph.roundHeightToGrid(position.top);
                tick.left += position.left - graph.roundHeightToGrid(position.left);
            }
        }
    },


    /*
    *   Optimizes dragging of whole graph.
    */
    getStopDraggingHandler: function (jsPlumbInstance) {
        return function (e, ui) {
            $cmsj(".UniGraph").removeClass("Move");
            if (!ui.helper.hasClass("_jsPlumb_endpoint")) {
                jsPlumbInstance.currentlyDragging = false;
            }
        }
    },


    /*
    *   Handler for adding node.
    */
    getCreateNodeHandler: function (graph) {
        var service = graph.serviceHandler;
        return function (event, ui) {
            if (ui.helper.is(':visible')) {
                var type = ui.helper.attr("rel");
                var action = ui.helper.attr("rev");
                var objectOffset = ui.helper.offset();
                var graphOffset = graph.graphJQ.offset();
                var x = objectOffset.left - graphOffset.left;
                var y = objectOffset.top - graphOffset.top;
                x = graph.roundWidthToGrid(x);
                y = graph.roundHeightToGrid(y);
                var hoverConnections = graphControlHandler.getHoverConnections(graph.graphJQ);
                var connectionIds = graphControlHandler.getConnectionsIds(hoverConnections);
                if (connectionIds.length > 0) {
                    service.createNodeOnConnections(type, action, graph.ID, x, y, connectionIds);
                } else {
                    service.createNode(type, action, graph.ID, x, y);
                }
            }
        }
    },


    /*
    *   Returns hover connections of given graph.
    */
    getHoverConnections: function (graphJQ) {
        return graphJQ.children("._jsPlumb_hover._jsPlumb_connector");
    },


    /*
    *   Returns ids of given connections.
    */
    getConnectionsIds: function (connectionsJQ) {
        var ids = new Array();
        for (var i = 0; i < connectionsJQ.length; i++) {
            var connection = $cmsj(connectionsJQ[i]);
            ids.push(connection.data("jsPlumbObject").ID);
        }
        return ids;
    },


    /*
    *   Handler for showing textboxes used for editing labels.
    */
    getShowTextboxInputHandler: function (graph) {
        var hideHandler = graphControlHandler.getHideTextboxInputHandler(graph);
        var labelEditHandler = graphControlHandler.getLabelEditHandler(hideHandler);
        return function (event) {
            var targetJQ = $cmsj(event.currentTarget).parent();

            if (targetJQ.find(".LabelEdit").length > 0) {
                return;
            }

            var height = targetJQ.parent().innerHeight() - 11;
            var width = targetJQ.parent().innerWidth() - 11;
            var position = targetJQ.position();
            var textboxJQ = $cmsj("<textarea class='LabelEdit' style='width:" + width + "px;height:" + height + "px;top:" + position.top + "px'>");
            textboxJQ.val(targetJQ.text());
            targetJQ.css("color", "transparent");

            targetJQ.prepend(textboxJQ);
            textboxJQ.focus();

            textboxJQ.keydown(labelEditHandler);
            textboxJQ.keyup(graphControlHandler.ensureLabelMaxLength);
            textboxJQ.mousedown(function (event) {
                event.stopPropagation();
            });
            textboxJQ.dblclick(function (event) {
                event.stopPropagation();
            });
            textboxJQ.focusout(hideHandler);

            textboxJQ.keyup();
            graph.enableTextSelection();
            event.stopPropagation();
        }
    },


    /*
    *   Ensures max length of value attribute of target element of event.
    */
    ensureLabelMaxLength: function (event) {
        var target = $cmsj(event.target);
        var text = target.val();
        var textLength = text.length;
        if (textLength > 450) {
            target.val(text.substring(0, 450));
        }
    },


    /*
    *   Handles keyboard input while graph wrapper has focus.
    */
    getLabelEditHandler: function (hideHandler) {
        return function (event) {

            if (event.keyCode == 46) { //46 == DELETE
                event.stopPropagation();
            } else if (event.keyCode == 13) { //13 == ENTER
                hideHandler(event);
                event.preventDefault();
            }
        };
    },


    /*
    *   Handler for hiding textboxes used for editing labels.
    */
    getHideTextboxInputHandler: function (graph) {
        this.service = graph.serviceHandler;
        return function (event) {
            graph.disableTextSelection();
            graphControlHandler.ensureLabelMaxLength(event);
            var inputJQ = $cmsj(event.target);
            var targetJQ = inputJQ.parent();
            var text = $cmsj.trim(inputJQ.val());
            var node = targetJQ.parents(".Node").data("nodeObject");
            var originalPosition = node.nodeJQ.position();

            if (text.length > 0) {
                node.nodeJQ.css("top", -500);
                node.nodeJQ.css("left", -500);
                graphControlHandler.saveLabelChange(targetJQ, inputJQ.val());
                node.setPosition(originalPosition);
            }
            targetJQ.children(".LabelEdit").remove();

            targetJQ.parent().children().each(function (id, element) {
                if (element != targetJQ[0]) {
                    $cmsj(element).show();
                }
            });
            targetJQ.removeAttr("style");

            node.repaintAllEndpoints();
            var container = targetJQ.parents("div.GraphContainer");
            if (typeof (container.focus) == 'function') {
                setTimeout(function () {
                    container.focus();
                }, 200);
            }
        }
    },


    /*
    *   Saves changes of label.
    */
    saveLabelChange: function (labelJQ, name) {
        labelJQ.children(".Editable").text($cmsj.trim(name));
        if (labelJQ.parents(".Case").length == 1) {
            graphControlHandler.saveCaseLabelChange(labelJQ, name);
        } else if (labelJQ.parents(".Node").length == 1) {
            graphControlHandler.saveNodeLabelChange(labelJQ, name);
        }
    },


    /*
    *   Saves changes of case label.
    */
    saveCaseLabelChange: function (labelJQ, name) {
        var node = labelJQ.parents(".Node").data('nodeObject');
        var nodeId = node.id;
        var caseJQ = labelJQ.parent(".Case");
        var caseId = caseJQ.data('id');
        this.service.setSwitchCaseName(nodeId, caseId, name);
    },


    /*
    *   Saves changes of node label.
    */
    saveNodeLabelChange: function (labelJQ, name) {
        var node = labelJQ.parents(".Node").data('nodeObject');
        var nodeId = node.id;
        this.service.setNodeName(nodeId, name);
    },


    /*
    *   Handles click on button for editing switch case.
    */
    editSwitchCaseHandler: function (event) {
        var node = $cmsj(event.target).parents(".Node").data('nodeObject');
        var caseId = $cmsj(event.currentTarget).parents(".Case").addBack().filter(".Case").data('id');
        node.openEditModalDialog(caseId);
    },


    /*
    *   Handler for editing node.  
    */
    editNodeHandler: function (event) {
        var node = $cmsj(event.target).parents(".Node").addBack().filter(".Node").data("nodeObject");
        node.openEditModalDialog();
    },


    /*
    *   Handler for deleting node.
    */
    getRemoveNodeHandler: function (graph) {
        return function (event) {
            event.currentTarget = $cmsj(event.target).parents(".Node");
            graphSelectHandler.getNodeSelectHandler(graph)(event);
            graph.removeSelectedItem();
        };
    },


    /*
    *   Handler for disabling tooltip when dragging.
    */
    tooltipStopEventIfDragging: function () {
        $cmsj(".tooltip:visible").hide();
        return $cmsj(".ui-draggable-dragging").length == 0;
    },


    /*
    *   Propagates mouse move event under drag helper for selecting connections to split. 
    */
    toolbarDragHandler: function (e, ui) {
        if (window.dragHelperWidth == null) {
            var helper = ui.helper;
            window.dragHelperWidth = helper.width();
            var newEvent = $cmsj.extend(true, {}, e);
            helper.hide();
            var target = document.elementFromPoint(arguments[0].clientX, arguments[0].clientY);
            newEvent.currentTarget = newEvent.target = target;
            newEvent.type = "mousemove";
            $cmsj(target).trigger(newEvent);
            helper.show();
        }
        window.dragHelperWidth = null;
    },


    /*
    *   Initializes toolbar to work with current graph.
    */
    initToolbar: function (toolbarId) {
        $cmsj("#" + toolbarId + " .BigButton").bind("drag", graphControlHandler.toolbarDragHandler);
    },
});


/*
*   This class contains handlers for saving the graph content.
*/
var GraphSavingServiceRequest = $class({

    constructor: function (service, graph) {
        this.service = service;
        this.graph = graph;
        this.graph.graphTimer = null;
        this.graph.graphWaitingCount = 0;
        this.bindServiceEvents();
    },

    bindServiceEvents: function () {
        $cmsj(window).bind('beforeunload', function (e) { window.isUnloading = true; });
    },

    showProgress: function () {
        if (window.Loader) {
            window.Loader.show();
        }
    },

    createNodeOnConnections: function (type, actionId, parentId, x, y, connectionIds) {
        this.showProgress();
        this.service.CreateNodeOnConnections(type, actionId, parentId, x, y, connectionIds, graphServiceResponseHandler.createNodeOnConnectionsHandler(this.graph), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    createNode: function (type, actionId, parentId, x, y) {
        this.showProgress();
        this.service.CreateNode(type, actionId, parentId, x, y, graphServiceResponseHandler.createNodeHandler(this.graph), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    setNodePosition: function (id, x, y) {
        this.showProgress();
        var removeObject = graphServiceResponseHandler.removeNodeHandler(this.graph, id);
        this.service.SetNodePosition(id, parseInt(x), parseInt(y), graphServiceResponseHandler.savingCheckHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    addSwitchCase: function (id) {
        this.showProgress();
        this.service.AddSwitchCase(id, graphServiceResponseHandler.repaintNodeHandler(this.graph, id), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    removeSwitchCase: function (nodeId, caseId) {
        var removeObject = graphServiceResponseHandler.removeSwitchCaseHandler(this.graph, nodeId, caseId);
        var repaintObject = graphServiceResponseHandler.repaintNode(this.graph);
        this.showProgress();
        this.service.RemoveSwitchCase(nodeId, caseId, graphServiceResponseHandler.removeHandler(this.graph, removeObject, repaintObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    editConnectionStart: function (connection, sourceNodeId, sourcePointId, value) {
        var removeObject = graphServiceResponseHandler.removeConnectionHandler(this.graph, connection);
        this.showProgress();
        this.service.EditConnectionStart(connection.ID, sourceNodeId, sourcePointId, graphServiceResponseHandler.savingCheckHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    editConnectionEnd: function (connection, targetNodeId) {
        var removeObject = graphServiceResponseHandler.removeConnectionHandler(this.graph, connection);
        this.showProgress();
        this.service.EditConnectionEnd(connection.ID, targetNodeId, graphServiceResponseHandler.savingCheckHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    createConnection: function (sourceNodeId, targetNodeId, sourcePointId, connection) {
        this.showProgress();
        this.service.CreateConnection(sourceNodeId, targetNodeId, sourcePointId, graphServiceResponseHandler.setConnectionIdHandler(this.graph, connection), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    removeNode: function (id) {
        var removeObject = graphServiceResponseHandler.removeNodeHandler(this.graph, id);
        this.showProgress();
        this.service.RemoveNode(id, graphServiceResponseHandler.removeHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    removeConnection: function (connection) {
        var removeObject = graphServiceResponseHandler.removeConnectionHandler(this.graph, connection);
        this.showProgress();
        this.service.RemoveConnection(connection.ID, graphServiceResponseHandler.removeHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    setNodeName: function (nodeId, name) {
        var removeObject = graphServiceResponseHandler.removeNodeHandler(this.graph, nodeId);
        this.showProgress();
        this.service.SetNodeName(nodeId, name, graphServiceResponseHandler.savingCheckHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    setSwitchCaseName: function (nodeId, caseId, name) {
        var removeObject = graphServiceResponseHandler.removeSwitchCaseHandler(this.graph, nodeId, caseId);
        this.showProgress();
        this.service.SetSwitchCaseName(nodeId, caseId, name, graphServiceResponseHandler.savingCheckHandler(this.graph, removeObject), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    },

    refreshNode: function (id) {
        this.showProgress();
        this.service.GetNode(id, graphServiceResponseHandler.repaintNodeHandler(this.graph, id), graphServiceResponseHandler.serviceFailure(this.resourceStrings["CriticalError"]), null);
    }

});

/*
*   Class handling general errors. 
*/
var GraphSavingServiceResponse = $class({

    setConnectionIdHandler: function (graph, connection) {
        var removeObject = graphServiceResponseHandler.removeConnectionHandler(graph, connection);
        return function (response) {
            if (graphServiceResponseHandler.savingCheck(graph, response, removeObject)) {
                connection.ID = response.Data;
                $cmsj(connection.canvas).addClass("conn_" + connection.ID);
            }
        };
    },

    createNodeOnConnectionsHandler: function (graph) {
        return function (response) {
            if (graphServiceResponseHandler.savingCheck(graph, response)) {
                graph.createNodes(response.Nodes);
                graph.createConnections(response.Connections);
            }
        };
    },

    createNodeHandler: function (graph) {
        return function (response) {
            if (graphServiceResponseHandler.savingCheck(graph, response)) {
                graph.createNode(response.Data);
            }
        };
    },

    repaintNodeHandler: function (graph, nodeId) {
        var removeObject = graphServiceResponseHandler.removeNodeHandler(graph, nodeId);
        return function (response) {
            if (graphServiceResponseHandler.savingCheck(graph, response, removeObject) || response.Data) {
                graph.repaintNode(response.Data);
            }
        };
    },

    removeHandler: function (graph, removeObject, repaintObject) {
        return function (response) {
            if (graphServiceResponseHandler.savingCheck(graph, response, removeObject, repaintObject)) {
                removeObject();
            }
        };
    },

    savingCheckHandler: function (graph, removeObject) {
        return function (response) {
            graphServiceResponseHandler.savingCheck(graph, response, removeObject);
        }
    },

    hideProgress: function () {
        if (window.Loader) {
            window.Loader.hide();
        }
    },

    savingCheck: function (graph, response, removeObject, repaintObject) {
        // Handle ScreenLock
        if (window.top.HideScreenLockWarningAndSync) {
            window.top.HideScreenLockWarningAndSync(response.ScreenLockInterval);
        }

        // Hide progress indicator
        graphServiceResponseHandler.hideProgress(graph);

        if (response.StatusCode == "200") {
            return true;
        } else if (response.StatusCode == "404" && typeof (removeObject) == "function") {
            removeObject();
        } else if (response.StatusCode == "400" && typeof (repaintObject) == "function" && response.Data) {
            repaintObject(response.Data);
        }
        alert(response.StatusMessage);
        return false;
    },

    serviceFailure: function (msg) {
        return function (response) {
            if (window.isUnloading) {
                return;
            } else if (response._statusCode == "401") {
                location.reload();
            } else {
                alert(msg);
            }
        }
    },

    removeConnectionHandler: function (graph, connection) {
        return function () {
            graphSelectHandler.getDeselectItemHandler(graph)();
            connection.removeWithAllProperties();
        }
    },

    removeNodeHandler: function (graph, nodeId) {
        return function () {
            graphSelectHandler.getDeselectItemHandler(graph)();
            graph.removeNode(nodeId);
        }
    },

    removeSwitchCaseHandler: function (graph, nodeId, caseId) {
        return function () {
            var node = graph.getNode(nodeId);
            node.removeCaseRow(caseId);
            node.repaintAllEndpoints();
        }
    },

    repaintNode: function (graph) {
        return function (data) {
            graph.repaintNode(data);
        }
    }
});

graphServiceResponseHandler = new GraphSavingServiceResponse();
graphControlHandler = new GraphControlHandler();
graphSelectHandler = new GraphSelectionHandler();
