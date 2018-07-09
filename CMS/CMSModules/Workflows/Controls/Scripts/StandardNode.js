var JsPlumbStandardNode = $class({

    Extends: JsPlumbAbstractNode,

    constructor: function (readOnly, graph, definition) {
        JsPlumbAbstractNode.call(this, readOnly, graph, definition);

        this.everySideAnchors = [[0.5, 0, 0, -1], [0.5, 1, 0, 1], [0, 0.5, -1, 0], [1, 0.5, 1, 0]];
        this.everySideFakeAnchors = [   
                                        [0.1, 0, 0, -1], [0.1, 1, 0, 1], [0, 0.1, -1, 0], [1, 0.1, 1, 0],
                                        [0.3, 0, 0, -1], [0.3, 1, 0, 1], [0, 0.3, -1, 0], [1, 0.3, 1, 0],
                                        [0.5, 0, 0, -1], [0.5, 1, 0, 1], [0, 0.5, -1, 0], [1, 0.5, 1, 0],
                                        [0.7, 0, 0, -1], [0.7, 1, 0, 1], [0, 0.7, -1, 0], [1, 0.7, 1, 0],
                                        [0.9, 0, 0, -1], [0.9, 1, 0, 1], [0, 0.9, -1, 0], [1, 0.9, 1, 0]
                                    ];

        //Fake endpoint definitions are used instead of actual target point
        //You can specify better anchors and prevents changing position of selected connection
        this.fakeEndpointDefinition = {
            dynamicAnchors: this.everySideFakeAnchors,
            isSource: false,
            isTarget: true,
            reattach: true,
            endpoint: ["Blank"]
        };

        this.standardSourcePointTemplate = {
            anchor: "BottomRight",
            attachedAnchors: this.everySideAnchors,
            endpoint: ["Image", { url: this.graph.addresses["ImagesUrl"] + "/endpoint_standard.png"}],
            endpointSelected: ["Image", { url: this.graph.addresses["ImagesUrl"] + "/endpoint_standard_selected.png"}],
            tooltip: graph.getReadOnlyResourceString("SourcepointStandardTooltip"),
            reattachHelperTooltip: graph.getReadOnlyResourceString("ReattachHelperTooltip"),
            isSource: true,
            isTarget: true,
            reattach: true,
            maxConnections: 1,
            dragOptions: { disabled: this.readOnly }
        };

        this.timeoutSourcePointTemplate = {
            anchor: "BottomCenter",
            endpoint: ["Image", { url: this.graph.addresses["ImagesUrl"] + "/endpoint_timeout.png"}],
            endpointSelected: ["Image", { url: this.graph.addresses["ImagesUrl"] + "/endpoint_standard_selected.png"}],
            tooltip: graph.getReadOnlyResourceString("SourcepointTimeoutTooltip"),
            reattachHelperTooltip: graph.getReadOnlyResourceString("ReattachHelperTooltip"),
            isSource: true,
            isTarget: true,
            reattach: true,
            maxConnections: 1,
            dragOptions: { disabled: this.readOnly },
            connectorStyle: { lineWidth: 2, strokeStyle: '#bdbbbb' }
        };

        //Redefinition of standard templates
        this.sourcePointTemplates = {
            standard: this.standardSourcePointTemplate,
            timeout: this.timeoutSourcePointTemplate
        };

        this.targetPointTemplate = {
            anchor: "Center",
            endpoint: ["Blank"],
            isSource: false,
            isTarget: true,
            reattach: true,
            maxConnections: -1,
            fakeEndpointDefinition: this.fakeEndpointDefinition
        };

        this.connectionTemplate = { lineWidth: 2, strokeStyle: '#1175ae' };
    },


    /*
    *   Sets node to be have timeout properties
    */
    setTimeoutProperties: function () {
        this.standardSourcePointTemplate.dynamicAnchors = [[0.5, 0, 0, -1], [0, 0.5, -1, 0], [1, 0.5, 1, 0]];
        this.standardSourcePointTemplate.attachedAnchors = undefined;
    },


    /*
    *   Removes timeout properties of node.
    */
    removeTimeoutProperties: function () {
        this.standardSourcePointTemplate.dynamicAnchors = undefined;
        this.standardSourcePointTemplate.attachedAnchors = this.everySideAnchors;
    }
});
