

define('Window', ['common', 'jquery', 'jqueryui'], function (common, jquery, jqueryui) {
    var windowList = [];

    function Window() {
    }

    Window.prototype.initialize = function () {
        this.rootElement.draggable();
        this.rootElement.resizable();
    }

    Window.prototype.register = function () {
        windowList.push(this.rootElement);
    }

    Window.prototype.title = "";

    Window.prototype.idName = "";

    Window.prototype.X = 0;

    Window.prototype.Y = 0;

    Window.prototype.create = function (url) {
        /* Generate the html for a window */
        this.rootElement = $(document.createElement('div'));
        this.rootElement.attr('id', this.idName);
        this.rootElement.addClass('ui-widget-content windowShell');
        this.rootElement.position(this.X, this.Y);

        var header = $(document.createElement('div'));
        header.addClass('sectionHeader');
        header.html(this.title);

        var content = $(document.createElement('div'));
        content.attr('id', 'windowContent');

        content.html(currentForm.loadHtml(url));

        this.register();

        var frag = document.createDocumentFragment();
        frag.innerHTML = this.rootElement;
        document.body.insertBefore(frag, document.body.childNodes[0]);
    }

    Window.prototype.setTop = function () {
        var thisZ = parseInt($(this.rootElement).css('z-index'));
        var topZ = windowList.length - 1;

        /* Return if already top window */
        if (thisZ == topZ) return;

        /* Compute the number of windows to change */
        var diff = topZ - thisZ;
        var count = diff;

        /* Set this to top */
        $(this.rootElement).css('z-index', topZ);

        /* Reorder Z indicies */
        for (var i = 0; i < (topZ + 1) && count > 0; i++) {
            var window = $(windowList[i]);
            var windowZ = parseInt(window.css('z-index'));
            var isSelf = $(this.rootElement).is(window);
            var zMaskLevel = (topZ - diff);

            if (!isSelf && windowZ > zMaskLevel) {
                window.css('z-index', --windowZ);
                count--;
            }
        }
    }

    Window.prototype.rootElement = "";

    return Window;
});