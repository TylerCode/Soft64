'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _remote = require('remote');

var _remote2 = _interopRequireDefault(_remote);

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactDom = require('react-dom');

var _reactDom2 = _interopRequireDefault(_reactDom);

var _Tabs = require('material-ui/Tabs');

var _RaisedButton = require('material-ui/RaisedButton');

var _RaisedButton2 = _interopRequireDefault(_RaisedButton);

var _Slider = require('material-ui/Slider');

var _Slider2 = _interopRequireDefault(_Slider);

var _Style = require('./Style.js');

var _Style2 = _interopRequireDefault(_Style);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; } // jshint esversion:6

var mainWindow = _remote2.default.getCurrentWindow();

function toggleDevTools() {
  _remote2.default.getCurrentWebContents().toggleDevTools();
}

/* Define the main template for the whole app page */

var Main = function (_React$Component) {
  _inherits(Main, _React$Component);

  function Main() {
    _classCallCheck(this, Main);

    return _possibleConstructorReturn(this, Object.getPrototypeOf(Main).apply(this, arguments));
  }

  _createClass(Main, [{
    key: 'render',
    value: function render() {
      return _react2.default.createElement(
        _Tabs.Tabs,
        null,
        _react2.default.createElement(
          _Tabs.Tab,
          { label: 'Start' },
          _react2.default.createElement(
            'div',
            null,
            _react2.default.createElement(_RaisedButton2.default, { onClick: toggleDevTools, label: 'DevTools', primary: true, style: _Style2.default.buttonStyle }),
            _react2.default.createElement(_RaisedButton2.default, { label: 'Load', primary: true, style: _Style2.default.buttonStyle }),
            _react2.default.createElement(_RaisedButton2.default, { label: 'Run', primary: true, style: _Style2.default.buttonStyle }),
            _react2.default.createElement(_RaisedButton2.default, { label: 'Pause', secondary: true, style: _Style2.default.buttonStyle }),
            _react2.default.createElement(_RaisedButton2.default, { label: 'Stop', secondary: true, style: _Style2.default.buttonStyle })
          )
        ),
        _react2.default.createElement(_Tabs.Tab, { label: 'Settings' }),
        _react2.default.createElement(_Tabs.Tab, { label: 'Debugger' })
      );
    }
  }]);

  return Main;
}(_react2.default.Component);

exports.default = Main;
//# sourceMappingURL=Main.js.map
