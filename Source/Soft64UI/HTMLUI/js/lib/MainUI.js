define('MainUI', ['jquery', 'common', 'jqueryui'], function (jquery, common, jqueryui) {
    var windowList;

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
                var thisZ = parseInt($(this).css('z-index'));
                var topZ = windowList.length - 1;

                /* Return if already top window */
                if (thisZ == topZ) return;

                /* Compute the number of windows to change */
                var diff = topZ - thisZ;
                var count = diff;

                /* Set this to top */
                $(this).css('z-index', topZ);
                
                /* Reorder Z indicies */
                for (var i = 0; i < (topZ + 1) && count > 0; i++) {
                    var window = $(windowList[i]);
                    var windowZ = parseInt(window.css('z-index'));
                    var isSelf = $(this).is(window);
                    var zMaskLevel = (topZ - diff);

                    if (!isSelf && windowZ > zMaskLevel) {
                        window.css('z-index', --windowZ);
                        count--;
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
