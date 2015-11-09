define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window'],
    function (jquery, jqueryui, Window) {

        function d2h(d) {
            var s = (+d).toString(16);
            if (s.length < 2) {
                s = '0' + s;
            }

            return s;
        }

        function appendHexCell(x, y, value, width) {
            $('#hexGrid').append('<span class=hexchar data-cid=' + x.toString() + y.toString() + ' >' + d2h(value) + '</span>');

            if (x >= (width - 1))
                $('#hexGrid').append("<br />");
        }

        var MemoryEditorWindow = function (params) {
            if (typeof params == 'object') {
                Window.call(this, params);
            }
        }

        MemoryEditorWindow.prototype = Object.create(Window.prototype);
        MemoryEditorWindow.prototype.constructor = MemoryEditorWindow;

        MemoryEditorWindow.prototype.refresh = function () {
            /* Clear the hex grid */
            $('#hexGrid').html("");

            var gridPixelHeight = $('#hexGrid').height();
            var fontPixelHeight = parseFloat($('#hexGrid').css('font-size'));
            var numLines = gridPixelHeight / (fontPixelHeight + 3.5);
            var width = 16;
            var length = parseInt((numLines * width).toFixed());
            length -= length % width;

            n64Memory.virtualMemoryAddress = 0;
            var buffer = new Uint8Array(n64Memory.readVirtualMemory(length), 0, length);

            for (var i = 0; i < length; i++) {
                if (typeof buffer[i] == 'undefined')
                    continue;

                var x = i % width;
                var y = (i - x) / width;

                appendHexCell(x, y, buffer[i], width);
            }
        }

        MemoryEditorWindow.prototype.initialize = function () {
            this.refresh();

            /* call the base function*/
            Window.prototype.initialize.call(this);
        }

        return MemoryEditorWindow;
});