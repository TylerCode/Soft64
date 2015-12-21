var windowList = [];

function tileWindows() {
    var lastWindow = '#menuBar';
    if (windowList.length > 0) {
        for (var i in windowList) {
            $(windowList[i]).position({
                of: $(lastWindow),
                my: 'left top',
                at: 'left bottom',
                collision: 'none',
            });
            lastWindow = windowList[i];
        }
    }
}

define('Window', ['jquery', 'jqueryui'], function (jquery, jqueryui) {
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
            snap: true,
            cancel: '#windowContent',
            scroll: true,
            scrollSensitivity: 10000,
            scrollSpeed: 10,
            stack: ".windowShell",
        });

        window.resizable();
    }

    Window.prototype.register = function () {
        var window = this.getInstance();
        windowList.push('#' + this.idName);
    }

    Window.prototype.getInstance = function() {
        return $('#' + this.idName);
    }

    Window.prototype.title = "";

    Window.prototype.idName = "";

    Window.prototype.getElementByCid = function (cid) {
        return this.getInstance().find('[data-cid=' + cid + ']');
    }

    Window.prototype.create = function (url, offset) {
        for (var i in windowList) {
            if (windowList[i] == this.idName) {
                throw new Error("window id already exists");
            }
        }

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

        window.appendTo('#windowContainer');

        this.register();
    }

    return Window;
});