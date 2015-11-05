define('MainUI', ['jquery', 'common', 'jqueryui'], function (jquery, common, jqueryui) {

    var lastWindow = null;
    var windowList;

    function compareZ(a, b) {
        if (a.css('z-index') < css('z-index'))
            return -1;
        if (a.css('z-index') > b.css('z-index'))
            return 1;
        return 0;
    }

    return {

        initialize: function () {
            var mainMenu = $('#mainMenuBar');
            var cartridgeWidget = $('#cartridgeWidget');
            var emulogWidget = $('#emulogWidget');
            var windowList;

            function reorderZ()  {

                /* Create window list */
                windowList = $('.windowShell');

                /* Preset Z order */
                for (var winIndex = 0; winIndex < windowList.length; winIndex++) {
                    $(windowList[winIndex]).css('z-index', winIndex);
                }
            }

            reorderZ();

            /* Add logic for Z-ordering each widget */
            $('.windowShell').mousedown(function () {

                /* Set to top*/
                $(this).css('z-index', windowList.length - 1);

                /* Now push each window down a z level except the one at the bottom*/
                for (var i = 0; i < windowList.length; i++) {

                    var window = $(windowList[i]);

                    if (!window.is($(this))) {
                        var z = parseInt(window.css('z-index'));

                        if (z >= 1) {
                            window.css('z-index', z - 1);
                        }
                    }
                }
            });

            /* Enable draggable */
            mainMenu.draggable();
            cartridgeWidget.draggable();

            emulogWidget.draggable({
                cancel: '#windowContent'
            });

            /* Enable resizing */
            mainMenu.resizable();
            cartridgeWidget.resizable();
            emulogWidget.resizable();

            common.findElementByCid(mainMenu, 'btnShowDevConsole').click(function () {
                currentForm.showDevTools();
            });

            common.findElementByCid(mainMenu, 'btnEmuRun').click(function () {
                currentForm.runEmu();
            });

            mainMenu.on('drag', function (e) {
                console.log('draggigng');
                /*TODO: debug break here*/
            });

            currentForm.on('emulog', function (logger, level, message) {
                var logStyle = "logmessage";

                switch (level.toLowerCase()) {
                    default:
                    case "trace":
                    case "info": break;
                    case "fatal": logStyle = "logmessage_fatal"; break;
                    case "error": logStyle = "logmessage_error"; break;
                    case "warning": logStyle = "logmessage_warning"; break;
                    case "debug": logStyle = "logmessage_debug"; break;
                }

                var output = '<span class=' + logStyle + '>' + logger + ': ' + message + '</span><br />';

                $('#emulog').append(output);
            });

            $('#romFileSelect').change(function (e) {
                var file = this.files[0];
                var reader = new FileReader();

                reader.onload = function (e) {
                    currentForm.insertRomFile(btoa(this.result));
                }

                reader.readAsBinaryString(file);
            });
        }
    }
});
