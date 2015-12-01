define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window', "HexEditor"],
    function (jquery, jqueryui, Window, HexEditor) {

        var MemoryEditorWindow = function (params) {
            if (typeof params == 'object') {
                Window.call(this, params);
            }
        }

        MemoryEditorWindow.prototype.hexEditor = null;

        MemoryEditorWindow.prototype = Object.create(Window.prototype);
        MemoryEditorWindow.prototype.constructor = MemoryEditorWindow;

        MemoryEditorWindow.prototype.refresh = function () {
            var addressField = this.getElementByCid('txtBoxAddress');
            this.hexEditor.currentAddress = parseInt(addressField.val(), 16) | 0;
            this.hexEditor.refresh();
        }

        MemoryEditorWindow.prototype.initialize = function () {
            this.hexEditor = new HexEditor({
                'hexGrid': this.getElementByCid('hexGrid'),
                'asciiGrid': this.getElementByCid('asciiGrid'),
                'editorCaret': this.getElementByCid('hexGridCaret'),
                'getVAddress': function () { return n64Memory.virtualMemoryAddress | 0 },
                'setVAddress': function (address) { n64Memory.virtualMemoryAddress = address | 0 },
                'readVMemory': function (length) { return new Uint8Array(n64Memory.readVirtualMemory(length | 0), 0, length | 0); }
            });

            this.refresh();

            var thisWindow = this;

            this.getElementByCid('buttonRefresh').click(function () {
                thisWindow.refresh();
            });

            this.getElementByCid('buttonPC').click(function () {
                thisWindow.getElementByCid('txtBoxAddress').val(n64Memory.getPC().toString(16).toUpperCase());
                thisWindow.refresh();
            })

            this.hexEditor.initialize();

            /* call the base function*/
            Window.prototype.initialize.call(this);
        }

        return MemoryEditorWindow;
});