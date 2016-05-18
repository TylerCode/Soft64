import Remote from 'remote';
import React from 'react';
import ReactDOM from 'react-dom';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import injectTapEventPlugin from 'react-tap-event-plugin';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import {Tabs, Tab} from 'material-ui/Tabs';
import RaisedButton from 'material-ui/RaisedButton'
import Slider from 'material-ui/Slider';
const mainWindow = Remote.getCurrentWindow();

/* Define some temp style stuff, later be moved into a css file */
const buttonStyle = {
  margin: 12,
};

const muiTheme = getMuiTheme(darkBaseTheme);


function toggleDevTools() {
  Remote.getCurrentWebContents().toggleDevTools();
}

/* Define the main template for the whole app page */
class Main extends React.Component {
  render() {
    return (<MuiThemeProvider muiTheme={muiTheme}>
      <Tabs>
        <Tab label="Start">
          <div style={{'background-color': muiTheme.palette.canvasColor}}>
            <RaisedButton onClick={toggleDevTools} label="DevTools" primary={true} style={buttonStyle} />
            <RaisedButton label="Load" primary={true} style={buttonStyle} />
            <RaisedButton label='Run' primary={true} style={buttonStyle} />
            <RaisedButton label='Pause' secondary={true} style={buttonStyle} />
            <RaisedButton label='Stop' secondary={true} style={buttonStyle} />
          </div>
        </Tab>
        <Tab label='Settings'>
        </Tab>
        <Tab label='Debugger'>
        </Tab>
      </Tabs>
    </MuiThemeProvider>);
  }
}

export default Main;
