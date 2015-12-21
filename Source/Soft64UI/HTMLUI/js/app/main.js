var jsMinExt = ".es5.min";
var cssMinExt = ".min.css";

if (mode == 'debug') {
    jsMinExt = ""
    cssMinExt = ".css"
}

require.config({
    baseUrl: 'js/lib',
    paths: {
        jquery: 'jquery-1.11.3.min',
        jqueryui: 'jquery-ui.min',
        Window: 'Window' + jsMinExt,
        MemoryEditorWindow: 'MemoryEditorWindow' + jsMinExt,
        HexEditor: 'HexEditor' + jsMinExt,
        PyWindow: 'PyWindow' + jsMinExt,
    }
});

/* Initialize the main windows */
require(
    [   'Window'
        , 'jquery'
        , 'MemoryEditorWindow'
        , 'PyWindow'],

    function (Window
             , jquery
             , MemoryEditorWindow
             , PyWindow) {

        $('head').append($('<link rel="stylesheet" type="text/css" />').attr('href', "css/MainStyles" + cssMinExt));

        /* Cartridge Window */
        var cartWindow = new Window({ title: "Cartridge", idName: "cartridgeWindow" });
        cartWindow.create('windows/CartridgeWindow.html');
        cartWindow.initialize();

        /* Emu log Window */
        var emulogWindow = new Window({ title: "Emulation Log", idName: "emulogWindow" });
        emulogWindow.create('windows/EmulogWindow.html');
        emulogWindow.initialize();

        /* Hook in window logic */
        $('#btnShowDevConsole').click(function () {
            currentForm.showDevTools();
        });

        $('#btnEmuRun').click(function () {
            currentForm.runEmu();
        });

        $('#btnTileWindows').click(function () {
            tileWindows();
        });

        $('#btnPyWin').click(function () {
            var win = new PyWindow({ title: 'Python Script Window', idName: 'pyWindow' });
            win.create('windows/PyWindow.html');
            win.initialize();
        });

        $('#btnDebugMemory').click(function () {
            var win = new MemoryEditorWindow({ title: 'Memory Editor', idName: 'memoryEditorWindow' });
            win.create('windows/MemoryEditorWindow.html');
            win.initialize();
        });

        $('#btnDebugStart').click(function () {
            currentForm.setDebugStart();
            currentForm.runEmu();
        });

        var emulogContainer = emulogWindow.getElementByCid('emulog');

        /* Register emulation log events */
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

            emulogContainer.append(output);
            var logContent = $("#emulogWindow #windowContent");
            logContent.scrollTop(logContent.height());
        });

        /* Event for when user selects cartridge file */
        cartWindow.getElementByCid('romFileSelect').change(function (e) {
            var file = this.files[0];
            var reader = new FileReader();

            reader.onload = function (e) {
                currentForm.insertRomFile(btoa(this.result));
            }

            reader.readAsBinaryString(file);
        });
});

