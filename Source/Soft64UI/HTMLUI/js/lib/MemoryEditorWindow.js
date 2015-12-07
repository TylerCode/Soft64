define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window', "HexEditor"],
    function (jquery, jqueryui, Window, HexEditor) {
        var MemoryEditorWindow = function (params) {
            if (typeof params == 'object') {
                Window.call(this, params);
            }
        }

        function merge_options(obj1, obj2) {
            var obj3 = {};
            for (var attrname in obj1) { obj3[attrname] = obj1[attrname]; }
            for (var attrname in obj2) { obj3[attrname] = obj2[attrname]; }
            return obj3;
        }

        MemoryEditorWindow.prototype.hexEditor = null;

        MemoryEditorWindow.prototype = Object.create(Window.prototype);
        MemoryEditorWindow.prototype.constructor = MemoryEditorWindow;

        MemoryEditorWindow.prototype.refresh = function () {
            var addressField = this.getElementByCid('txtBoxAddress');
            this.hexEditor.currentAddress = parseInt(addressField.val(), 16) | 0;
            this.hexEditor.refresh();
        }

        MemoryEditorWindow.prototype.setAddressField = function(address) {
            this.getElementByCid('txtBoxAddress').val((address | 0).toString(16).toUpperCase());
            var pageSize = (16 * this.hexEditor.numLines);
            this.getElementByCid('scroller').slider("value", 0 | (4294967295 % (address - (address % pageSize))));
        }

        MemoryEditorWindow.prototype.initialize = function () {
            var hexConfig = {
                'hexGrid': this.getElementByCid('hexGrid'),
                'asciiGrid': this.getElementByCid('asciiGrid'),
                'editorCaret': this.getElementByCid('hexGridCaret'),
            };

            var virtualMemoryConfig = {
                'setOffset': function (address) { n64Memory.virtualMemoryAddress = address | 0 },
                'readBytes': function (length) { return new Uint8Array(n64Memory.readVirtualMemory(length | 0), 0, length | 0); },
                'writeByte': function (value) { n64Memory.writeVirtualMemoryByte(value); },
            };

            this.hexEditor = new HexEditor(merge_options(hexConfig, virtualMemoryConfig));

            this.refresh();

            var thisWindow = this;

            this.getElementByCid('buttonRefresh').click(function () {
                thisWindow.refresh();
            });

            this.getElementByCid('buttonPC').click(function () {
                thisWindow.setAddressField(n64Memory.getPC());
                thisWindow.refresh();
            })

            /* Memory Slider */
            this.getElementByCid('scroller').slider({
                orientation: "vertical",
                range: "min",
                min: 0,
                max: (4294967295 / (16 * this.hexEditor.numLines)) | 0,
                value: 0,
                slide: function (event, ui) {
                    value = (ui.value | 0);
                    thisWindow.hexEditor.currentAddress = value;
                    thisWindow.setAddressField(value);
                    thisWindow.refresh();
                }
            });

            this.hexEditor.initialize();

            /* call the base function*/
            Window.prototype.initialize.call(this);
        }

        return MemoryEditorWindow;
});