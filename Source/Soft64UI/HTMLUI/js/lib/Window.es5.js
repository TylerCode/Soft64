'use strict';

define('Window', ['jquery', 'jqueryui'], function (jquery, jqueryui) {
    var windowList = [];

    function Window(params) {
        if (typeof params == 'object') {
            for (var memberName in params) {
                this[memberName] = params[memberName];
            }
        }
    }

    Window.prototype.initialize = function () {
        var window = this.getInstance();

        window.draggable({
            cancel: '#windowContent'
        });

        window.resizable();
    };

    Window.prototype.register = function () {
        var window = this.getInstance();
        windowList.push('#' + this.idName);
        window.css('z-index', windowList.length - 1);

        /* register z-order hook */
        var _this = this;
        window.mousedown(function () {
            _this.setTop();
        });
    };

    Window.prototype.getInstance = function () {
        return $('#' + this.idName);
    };

    Window.prototype.title = "";

    Window.prototype.idName = "";

    Window.prototype.getElementByCid = function (cid) {
        return this.getInstance().find('[data-cid=' + cid + ']');
    };

    Window.prototype.create = function (url, offset) {
        var window = $(document.createElement('div'));

        window.attr('id', this.idName);
        window.addClass('ui-widget-content windowShell');

        var header = $(document.createElement('div'));
        header.addClass('sectionHeader');
        header.html(this.title);
        header.appendTo(window);

        var content = $(document.createElement('div'));
        content.attr('id', 'windowContent');
        content.html(currentForm.loadHtml(url));
        content.appendTo(window);

        window.appendTo('body');

        this.register();
    };

    Window.prototype.setTop = function () {
        var targetWindow = this.getInstance();
        var thisZ = parseInt(targetWindow.css('z-index'));
        var topZ = windowList.length - 1;

        /* Return if already top window */
        if (thisZ == topZ) return;

        /* Compute the number of windows to change */
        var diff = topZ - thisZ;
        var count = diff;

        /* Set this to top */
        targetWindow.css('z-index', topZ);

        /* Reorder Z indicies */
        for (var i = 0; i < topZ + 1 && count > 0; i++) {
            var window = $(windowList[i]);
            var windowZ = parseInt(window.css('z-index'));
            var isSelf = targetWindow.is(window);
            var zMaskLevel = topZ - diff;

            if (!isSelf && windowZ > zMaskLevel) {
                window.css('z-index', --windowZ);
                count--;
            }
        }
    };

    return Window;
});

