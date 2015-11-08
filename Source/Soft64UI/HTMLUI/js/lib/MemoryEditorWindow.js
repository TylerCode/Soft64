define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window'],
    function (jquery, jqueryui, Window) {

        function d2h(d) {
            var s = (+d).toString(16);
            if (s.length < 2) {
                s = '0' + s;
            }

            return s;
        }

        var MemoryEditorWindow = function (params) {
            if (typeof params == 'object') {
                Window.call(this, params);
            }
        }

        MemoryEditorWindow.prototype = Object.create(Window.prototype);
        MemoryEditorWindow.prototype.constructor = MemoryEditorWindow;

        MemoryEditorWindow.prototype.refresh = function () {
            var gridPixelHeight = $('#hexGrid').height();
            var fontPixelHeight = parseFloat($('#hexGrid').css('font-size'));
            var numLines = gridPixelHeight / (fontPixelHeight + 3.5);
            var width = 16;
            var length = parseInt((numLines * width).toFixed());
            length -= length % width;

            n64Memory.virtualMemoryAddress = 0;
            var buffer = new Uint8Array(n64Memory.readVirtualMemory(length), 0, length);

            /* TODO: convert loops into single loop based on length */

            for (var y = 0; y < numLines; y++) {
                for (var x = 0; x < width; x++) {
                    var bufferPosition = (y * width) + x;

                    if (typeof buffer[bufferPosition] == 'undefined')
                        continue;

                    $('#hexGrid').append('<span class=\'hexchar\' id=' + x.toString() + y.toString() + '>' + d2h(buffer[bufferPosition]) + '</span>');
                    //$('#asciiGrid').append("<span class='asciichar'>.</span>");
                }

                $('#hexGrid').append("<br />");
                //$('#asciiGrid').append("<br />");
            }
        }

        MemoryEditorWindow.prototype.initialize = function () {
            this.refresh();

            /* call the base function*/
            Window.prototype.initialize.call(this);
        }

        return MemoryEditorWindow;
});