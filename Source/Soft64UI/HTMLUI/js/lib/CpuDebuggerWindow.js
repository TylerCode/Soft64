define('CpuDebuggerWindow', [
    'jquery'
    , 'jqueryui'
    , 'Window'],
    function (
        jquery
        , jqueryui
        , Window
        ) {

        var CpuDebuggerWindow = function (params) {
            if (typeof params == 'object') {
                Window.call(this, params);
            }
        }

        CpuDebuggerWindow.prototype = Object.create(Window.prototype);
        CpuDebuggerWindow.prototype.constructor = CpuDebuggerWindow;

        return CpuDebuggerWindow;
    });