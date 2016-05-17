import Remote from 'remote';
import React from 'react';
import ReactDOM from 'react-dom';
import injectTapEventPlugin from 'react-tap-event-plugin';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import {Tabs, Tab} from 'material-ui/Tabs';
import RaisedButton from 'material-ui/RaisedButton'
import Slider from 'material-ui/Slider';
const mainWindow = Remote.getCurrentWindow();

/* This is a needed hack plugin for material-ui */
injectTapEventPlugin();

/* Define some temp style stuff, later be moved into a css file */
const style = {
  margin: 12,
};

const muiTheme = getMuiTheme();


function toggleDevTools() {
  Remote.getCurrentWebContents().toggleDevTools();
}

/* Define the main template for the whole app page */
const MainPage = () => (
  <MuiThemeProvider muiTheme={muiTheme}>
    <Tabs>
      <Tab label="Start">
        <div>
          <RaisedButton onClick={toggleDevTools} label="DevTools" primary={true} style={style} />
          <RaisedButton label="Load" primary={true} style={style} />
          <RaisedButton label='Run' primary={true} style={style} />
          <RaisedButton label='Pause' secondary={true} style={style} />
          <RaisedButton label='Stop' secondary={true} style={style} />
        </div>
      </Tab>
      <Tab label='Settings'>
      </Tab>
      <Tab label='Debugger'>
      </Tab>
    </Tabs>
  </MuiThemeProvider>
);

/* Render the HTML to the body */
ReactDOM.render(
  React.createElement(MainPage),
  document.getElementById('appbody')
);
