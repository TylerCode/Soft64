define('PyWindow'
        , ['jquery', 'jqueryui', 'Window']
        , function (jquery, jqueryui, Window) {

            var PyWindow = function (params) {
                if (typeof params == 'object') {
                    Window.call(this, params);
                }
                else {
                    Window.call(this, {});
                }
            }

            PyWindow.prototype = Object.create(Window.prototype);
            PyWindow.prototype.constructor = PyWindow;

            PyWindow.prototype.initialize = function () {
                var thisWindow = this;

                this.getElementByCid('btnRun').click(function () {
                    currentForm.executePython(thisWindow.getElementByCid('scriptBox').val());
                });

                this.getElementByCid('btnRunFile').change(function () {
                    var file = this.files[0];
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        thisWindow.getElementByCid('scriptBox').val(this.result);
                        currentForm.executePython(this.result);
                    }

                    reader.readAsText(file);
                });

                /* call the base function*/
                Window.prototype.initialize.call(this);
            }

            return PyWindow;
        });