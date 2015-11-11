define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window'],
    function (jquery, jqueryui, Window) {

        function d2h(d) {
            var s = (+d).toString(16);
            if (s.length < 2) {
                s = '0' + s;
            }

            return s.toUpperCase();
        }

        function appendHexCell(hexGrid, position, value, width) {
            var hexElement = $(document.createElement('div'));
            hexElement.addClass('hexchar');
            hexElement.attr('data-cellx', position.x.toString());
            hexElement.attr('data-celly', position.y.toString());
            hexElement.css('width', parseInt(hexGrid.css('font-size')) * 2);
            hexElement.html(d2h(value));
            hexElement.appendTo(hexGrid);
            

            if (position.x >= (width - 1))
                hexGrid.append("<br />");
        }

        function getCell (grid, x, y) {
            return grid.find('[data-cellx=' + x.toString() + ']' + '[data-celly=' + y.toString() + ']');
        }

        var MemoryEditorWindow = function (params) {
            if (typeof params == 'object') {
                Window.call(this, params);
            }
        }

        MemoryEditorWindow.prototype = Object.create(Window.prototype);
        MemoryEditorWindow.prototype.constructor = MemoryEditorWindow;

        MemoryEditorWindow.prototype.selectedCell = { x: 0, y: 0 };

        MemoryEditorWindow.prototype.gridWidth = 0;

        MemoryEditorWindow.prototype.gridHeight = 0;

        MemoryEditorWindow.prototype.memoryBuffer = new Uint8Array(0);

        MemoryEditorWindow.prototype.currentAddress = 0;

        MemoryEditorWindow.prototype.refresh = function () {
            /* Clear the hex grid */
            var hexGrid = this.getElementByCid('hexGrid');
            hexGrid.html("");

            var gridPixelHeight = hexGrid.height();
            var fontPixelHeight = parseFloat(hexGrid.css('font-size'));
            var numLines = gridPixelHeight / (fontPixelHeight + 3.5);
            this.gridWidth = 16|0;
            var length = parseInt((numLines * this.gridWidth).toFixed()) | 0;
            length -= length % this.gridWidth;
            this.gridHeight = (length / this.gridWidth) | 0;

            var addressField = this.getElementByCid('txtBoxAddress');
            this.currentAddress = parseInt(addressField.value, 16) | 0;

            n64Memory.virtualMemoryAddress = isNaN(this.currentAddress) ? 0 | 0 : this.currentAddress;

            this.memoryBuffer = new Uint8Array(n64Memory.readVirtualMemory(length), 0, length);

            for (var i = 0; i < length; i++) {
                if (typeof this.memoryBuffer[i] == 'undefined')
                    continue;

                var x = i % this.gridWidth;
                var y = (i - x) / this.gridWidth;

                appendHexCell(hexGrid, { x: x, y: y }, this.memoryBuffer[i], this.gridWidth);

                /* Register cell events */
                var thisWindow = this;
                var cell = getCell(hexGrid, x, y);

                (function (x, y, cell) {
                    cell.click(function () {
                        thisWindow.moveCaret(x, y);
                    })
                })(x, y, cell); //pass in the current value
            }
        }

        MemoryEditorWindow.prototype.moveCaretNext = function () {
            var x = this.selectedCell.x;
            var y = this.selectedCell.y;

            if (++x > this.gridWidth) {
                x = 0;

                if (++y > this.gridHeight) {
                    return;
                }
            }

            this.moveCaret(x, y);
        }

        MemoryEditorWindow.prototype.leftNibble = false;

        MemoryEditorWindow.prototype.moveCaret = function (x, y) {
            this.leftNibble = false;
            this.selectedCell.x = x;
            this.selectedCell.y = y;
            var cell = getCell(this.getElementByCid('hexGrid'), x, y);
            var hexGridCaret = this.getElementByCid('hexGridCaret');
            hexGridCaret.css('height', cell.outerHeight());
            hexGridCaret.css('width', cell.outerWidth());
            hexGridCaret.offset(cell.offset());
        }

        MemoryEditorWindow.prototype.updateHexValue = function (x, y, newValue) {
            getCell(this.getElementByCid('hexGrid'), x, y).html(d2h(newValue));
        }

        MemoryEditorWindow.prototype.writeNextNibble = function (value) {
            var bufferOffset = (this.selectedCell.y * this.gridWidth) + this.selectedCell.x;
            var oldValue = this.memoryBuffer[bufferOffset];

            var byteValue = this.leftNibble ? (oldValue & 0xF) | (value << 4) : (oldValue | value);

            if (!this.leftNibble)
                this.leftNibble = true;
            else
                this.leftNibble = false;

            this.memoryBuffer[bufferOffset] = byteValue;
            this.updateHexValue(this.selectedCell.x, this.selectedCell.y, byteValue);

            n64Memory.virtualMemoryAddress = this.currentAddress + bufferOffset;
            n64Memory.writeVirtualMemoryByte(byteValue);

            if (!this.leftNibble)
                this.moveCaretNext();
        }

        MemoryEditorWindow.prototype.initialize = function () {
            this.refresh();

            var thisWindow = this;

            this.getElementByCid('buttonRefresh').click(function () {
                thisWindow.refresh();
            });

            /* Register hex grid key events */
            var hexGrid = this.getElementByCid('hexGrid');
            hexGrid.keypress(function (e) {
                var char = String.fromCharCode(e.keyCode);

                switch (char.toLowerCase()) {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f': {
                        var nibble = parseInt(char, 16) | 0;
                        thisWindow.writeNextNibble(nibble);
                        break;
                    }

                    default: break;
                }
            })

            /* call the base function*/
            Window.prototype.initialize.call(this);
        }

        return MemoryEditorWindow;
});