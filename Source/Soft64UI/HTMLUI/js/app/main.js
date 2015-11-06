require.config({
    baseUrl: 'js/lib',
    paths: {
        jquery: 'jquery-1.11.3.min',
        jqueryui: 'jquery-ui.min',
        Window: 'Window',
    }
});

/* Initialize the main windows */
require(['Window', 'jquery'],

    function (Window
             , jquery) {

        /* Main Menu Window */
        var menuWindow = new Window();
        menuWindow.title = "Main Menu";
        menuWindow.idName = "mainMenuWindow";
        menuWindow.create('windows/MenuWindow.html');
        menuWindow.initialize();

        /* Cartridge Window */
        var cartWindow = new Window();
        cartWindow.title = "Cartridge";
        cartWindow.idName = "cartridgeWindow";
        cartWindow.create('windows/CartridgeWindow.html');
        cartWindow.initialize();

        /* Emu log Window */
        var emulogWindow = new Window();
        emulogWindow.title = "Emulation Log";
        emulogWindow.idName = "emulogWindow";
        emulogWindow.create('windows/EmulogWindow.html');
        emulogWindow.initialize();

        /* Hook in window logic */
        menuWindow.getElementByCid('btnShowDevConsole').click(function () {
            currentForm.showDevTools();
        });

        menuWindow.getElementByCid('btnEmuRun').click(function () {
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

