define('HexEditor', ['jquery'], function (jquery) {

    var HexEditor = function (params) {
        if (typeof params == 'object') {
            for (var memberName in params) {
                this[memberName] = params[memberName];
            }
        }
    }

    HexEditor.prototype.initialize = function () {
        var thisEditor = this;

        /* Register hex grid key events */
        this.hexGrid.keypress(function (e) {
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
                    thisEditor.writeNextNibble(nibble);
                    break;
                }

                default: break;
            }
        })

        this.asciiGrid.keypress(function (e) {
            thisEditor.writeNextChar(String.fromCharCode(e.keyCode));
        });
    }

    HexEditor.prototype.selectedCell = { x: 0, y: 0 };
    HexEditor.prototype.gridWidth = 0;
    HexEditor.prototype.gridHeight = 0;
    HexEditor.prototype.memoryBuffer = new Uint8Array(0);
    HexEditor.prototype.currentAddress = 0;
    HexEditor.prototype.leftNibble = false;
    HexEditor.prototype.editTypeMode = 0;

    /* Overridden by configuration */
    HexEditor.prototype.getVAddress = function () { return 0 };
    HexEditor.prototype.setVAddress = function (v) { };
    HexEditor.prototype.readVMemory = function (length) {
        return new Uint8Array([], 0, 0);
    }

    HexEditor.prototype.refresh = function () {
        /* Clear the hex grid */
        var hexGrid = this.hexGrid;
        var asciiGrid = this.asciiGrid;
        hexGrid.html("");
        asciiGrid.html("");

        var gridPixelHeight = hexGrid.height();
        var fontPixelHeight = parseFloat(hexGrid.css('font-size'));
        var numLines = gridPixelHeight / (fontPixelHeight + 3.5);
        this.gridWidth = 16 | 0;
        var length = parseInt((numLines * this.gridWidth).toFixed()) | 0;
        length -= length % this.gridWidth;
        this.gridHeight = (length / this.gridWidth) | 0;

        this.setVAddress(this.currentAddress);

        this.memoryBuffer = this.readVMemory(length);

        this.moveCaret(0, 0);

        for (var i = 0; i < length; i++) {
            if (typeof this.memoryBuffer[i] == 'undefined')
                continue;

            var x = i % this.gridWidth;
            var y = (i - x) / this.gridWidth;

            appendHexCell(hexGrid, { x: x, y: y }, this.memoryBuffer[i], this.gridWidth);
            appendAsciiCell(asciiGrid, { x: x, y: y }, this.memoryBuffer[i], this.gridWidth);

            /* Register cell events */
            var thisEditor = this;
            var cellHex = getCell(hexGrid, x, y);

            (function (x, y, cell) {
                cell.click(function () {
                    thisEditor.editTypeMode = 0;
                    thisEditor.moveCaret(x, y);
                })
            })(x, y, cellHex); //pass in the current value

            var cellAscii = getCell(asciiGrid, x, y);

            (function (x, y, cell) {
                cell.click(function () {
                    thisEditor.editTypeMode = 1;
                    thisEditor.moveCaret(x, y);
                })
            })(x, y, cellAscii); //pass in the current value
        }
    }

    HexEditor.prototype.moveCaretNext = function () {
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

    HexEditor.prototype.moveCaret = function (x, y) {
        var targetGrid = null;

        if (this.editTypeMode == 0) {
            targetGrid = this.hexGrid;
        }
        else {
            targetGrid = this.asciiGrid;
        }

        this.leftNibble = false;
        this.selectedCell.x = x;
        this.selectedCell.y = y;
        var cell = getCell(targetGrid, x, y);
        var hexGridCaret = this.editorCaret;
        hexGridCaret.css('height', cell.outerHeight());
        hexGridCaret.css('width', cell.outerWidth());
        hexGridCaret.offset(cell.offset());
    }

    HexEditor.prototype.updateHexValue = function (x, y, newValue) {
        getCell(this.hexGrid, x, y).html(d2h(newValue));
    }

    HexEditor.prototype.updateAsciiValue = function (x, y, newValue) {
        getCell(this.asciiGrid, x, y).html(ascii(newValue));
    }

    HexEditor.prototype.writeNextNibble = function (value) {
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

    HexEditor.prototype.writeNextChar = function (char) {
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

    function getCell(grid, x, y) {
        return grid.find('[data-cellx=' + x.toString() + ']' + '[data-celly=' + y.toString() + ']');
    }

    return HexEditor;
});