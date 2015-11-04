define('MainUI', ['jquery', 'common'], function (jquery, common) {
    return {
        initialize: function () {
            var mainMenu = $('#mainMenuBar');

            buttonAssignClick = function (cid, click) {
                if (typeof click != 'function')
                    throw new TypeError("click must be a function");

                var results = common.findElementByCid(mainMenu, cid);

                if (results != null)
                    results.click(click);
                else
                    throw new Error("found no control with matching cid");
            };

            buttonAssignClick('btnShowDevConsole', function () { currentForm.showDevTools(); });
            buttonAssignClick('btnEmuRun', function () { currentForm.runEmu(); });


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
