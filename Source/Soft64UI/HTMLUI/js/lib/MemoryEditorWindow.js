define('MemoryEditorWindow', ['jquery', 'jqueryui', 'Window'],
    function (jquery, jqueryui, Window) {

        function d2h(d) {
            var s = (+d).toString(16);
            if (s.length < 2) {
                s = '0' + s;
            }

            return s.toUpperCase();
        }

        function ascii(v) {
            return String.fromCharCode(v).replace(/[^\x21-\x7E]+/g, '.');
        }

        function appendHexCell(grid, position, value, width) {
            var hexElement = $(document.createElement('div'));
            hexElement.addClass('hexchar');
            hexElement.attr('data-cellx', position.x.toString());
            hexElement.attr('data-celly', position.y.toString());
            hexElement.html(d2h(value));
            hexElement.appendTo(grid);
            

            if (position.x >= (width - 1))
                grid.append("<br />");
        }

        function appendAsciiCell(grid, position, value, width) {
            var asciiElement = $(document.createElement('div'));
            asciiElement.addClass('asciichar');
            asciiElement.attr('data-cellx', position.x.toString());
            asciiElement.attr('data-celly', position.y.toString());
            asciiElement.html(ascii(value));
            asciiElement.appendTo(grid);


            if (position.x >= (width - 1))
                grid.append("<br />");
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
            var asciiGrid = this.getElementByCid('asciiGrid');
            hexGrid.html("");
            asciiGrid.html("");

            var gridPixelHeight = hexGrid.height();
            var fontPixelHeight = parseFloat(hexGrid.css('font-size'));
            var numLines = gridPixelHeight / (fontPixelHeight + 3.5);
            this.gridWidth = 16|0;
            var length = parseInt((numLines * this.gridWidth).toFixed()) | 0;
            length -= length % this.gridWidth;
            this.gridHeight = (length / this.gridWidth) | 0;

            var addressField = this.getElementByCid('txtBoxAddress');
            this.currentAddress = parseInt(addressField.val(), 16) | 0;
            n64Memory.virtualMemoryAddress = this.currentAddress;

            this.memoryBuffer = new Uint8Array(n64Memory.readVirtualMemory(length), 0, length);

            this.moveCaret(0, 0);

            for (var i = 0; i < length; i++) {
                if (typeof this.memoryBuffer[i] == 'undefined')
                    continue;

                var x = i % this.gridWidth;
                var y = (i - x) / this.gridWidth;

                appendHexCell(hexGrid, { x: x, y: y }, this.memoryBuffer[i], this.gridWidth);
                appendAsciiCell(asciiGrid, { x: x, y: y }, this.memoryBuffer[i], this.gridWidth);

                /* Register cell events */
                var thisWindow = this;
                var cellHex = getCell(hexGrid, x, y);

                (function (x, y, cell) {
                    cell.click(function () {
                        thisWindow.editTypeMode = 0;
                        thisWindow.moveCaret(x, y);
                    })
                })(x, y, cellHex); //pass in the current value

                var cellAscii = getCell(asciiGrid, x, y);

                (function (x, y, cell) {
                    cell.click(function () {
                        thisWindow.editTypeMode = 1;
                        thisWindow.moveCaret(x, y);
                    })
                })(x, y, cellAscii); //pass in the current value
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
            var targetGrid = null;

            if (this.editTypeMode == 0) {
                targetGrid = this.getElementByCid('hexGrid');
            }
            else {
                targetGrid = this.getElementByCid('asciiGrid');
            }

            this.leftNibble = false;
            this.selectedCell.x = x;
            this.selectedCell.y = y;
            var cell = getCell(targetGrid, x, y);
            var hexGridCaret = this.getElementByCid('hexGridCaret');
            hexGridCaret.css('height', cell.outerHeight());
            hexGridCaret.css('width', cell.outerWidth());
            hexGridCaret.offset(cell.offset());
        }

        MemoryEditorWindow.prototype.updateHexValue = function (x, y, newValue) {
            getCell(this.getElementByCid('hexGrid'), x, y).html(d2h(newValue));
        }

        MemoryEditorWindow.prototype.updateAsciiValue = function (x, y, newValue) {
            getCell(this.getElementByCid('asciiGrid'), x, y).html(ascii(newValue));
        }

        MemoryEditorWindow.prototype.editTypeMode = 0;

        MemoryEditorWindow.prototype.writeNextNibble = function (value) {
            this.editTypeMode = 0;

            var bufferOffset = (this.selectedCell.y * this.gridWidth) + this.selectedCell.x;
            var oldValue = this.memoryBuffer[bufferOffset];

            var byteValue = this.leftNibble ? (oldValue & 0xF) | (value << 4) : (oldValue | value);

            if (!this.leftNibble)
                this.leftNibble = true;
            else
                this.leftNibble = false;

            this.memoryBuffer[bufferOffset] = byteValue;
            this.updateHexValue(this.selectedCell.x, this.selectedCell.y, byteValue);
            this.updateAsciiValue(this.selectedCell.x, this.selectedCell.y, byteValue);

            n64Memory.virtualMemoryAddress = this.currentAddress + bufferOffset;
            n64Memory.writeVirtualMemoryByte(byteValue);

            if (!this.leftNibble)
                this.moveCaretNext();
        }

        MemoryEditorWindow.prototype.writeNextChar = function (char) {
            this.editTypeMode = 1;

            var bufferOffset = (this.selectedCell.y * this.gridWidth) + this.selectedCell.x;
            var byteValue = char.charCodeAt(0);

            this.memoryBuffer[bufferOffset] = byteValue;
            this.updateHexValue(this.selectedCell.x, this.selectedCell.y, byteValue);
            this.updateAsciiValue(this.selectedCell.x, this.selectedCell.y, byteValue);

            n64Memory.virtualMemoryAddress = this.currentAddress + bufferOffset;
            n64Memory.writeVirtualMemoryByte(byteValue);

            this.moveCaretNext();
        }

        MemoryEditorWindow.prototype.initialize = function () {
            this.refresh();

            var thisWindow = this;

            this.getElementByCid('buttonRefresh').click(function () {
                thisWindow.refresh();
            });

            this.getElementByCid('buttonPC').click(function () {
                thisWindow.getElementByCid('txtBoxAddress').val(n64Memory.getPC().toString(16));
                thisWindow.refresh();
            })

            /* Register hex grid key events */
            this.getElementByCid('hexGrid').keypress(function (e) {
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

            this.getElementByCid('asciiGrid').keypress(function (e) {
                thisWindow.writeNextChar(String.fromCharCode(e.keyCode));
            });

            /* call the base function*/
            Window.prototype.initialize.call(this);
        }

        return MemoryEditorWindow;
});