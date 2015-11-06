define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window'],
    function (jquery, jqueryui, Window) {

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

            // var buffer = currentForm.readMem(0, numLines * width);

            for (var y = 0; y < numLines; y++) {
                for (var x = 0; x < width; x++) {


                    $('#hexGrid').append('<span class=\'hexchar\' id=' + x.toString() + y.toString() + '>' + '00' + '</span>');
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