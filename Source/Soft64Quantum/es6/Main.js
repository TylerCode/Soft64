import Remote from 'remote';
import React from 'react';
import ReactDOM from 'react-dom';
import {Tabs, Tab} from 'material-ui/Tabs';
import RaisedButton from 'material-ui/RaisedButton'
import Slider from 'material-ui/Slider';
import MainStyle from './Style.js';
const mainWindow = Remote.getCurrentWindow();

function toggleDevTools() {
  Remote.getCurrentWebContents().toggleDevTools();
}

/* Define the main template for the whole app page */
class Main extends React.Component {
  render() {
    return (
      <Tabs>
        <Tab label="Start">
          <div>
            <RaisedButton onClick={toggleDevTools} label="DevTools" primary={true} style={MainStyle.buttonStyle} />
            <RaisedButton label="Load" primary={true} style={MainStyle.buttonStyle} />
            <RaisedButton label='Run' primary={true} style={MainStyle.buttonStyle} />
            <RaisedButton label='Pause' secondary={true} style={MainStyle.buttonStyle} />
            <RaisedButton label='Stop' secondary={true} style={MainStyle.buttonStyle} />
          </div>
        </Tab>
        <Tab label='Settings'>
        </Tab>
        <Tab label='Debugger'>
        </Tab>
      </Tabs>);
  }
}

export default Main;
