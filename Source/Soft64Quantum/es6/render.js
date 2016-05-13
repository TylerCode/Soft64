import React from 'react';
import ReactDOM from 'react-dom';
import injectTapEventPlugin from 'react-tap-event-plugin';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import {Tabs, Tab} from 'material-ui/Tabs';
import Slider from 'material-ui/Slider';

injectTapEventPlugin();

const styles = {
  headline: {
    fontSize: 24,
    paddingTop: 16,
    marginBottom: 12,
    fontWeight: 400,
  },
};

function handleActive(tab) {
  alert(`A tab with this route property ${tab.props.route} was activated.`);
}

const TabsExampleSimple = () => (
   <MuiThemeProvider muiTheme={getMuiTheme()}>
    <Tabs>
      <Tab label="Item One" >
        <div>
          <h2 style={styles.headline}>Tab One</h2>
          <p>
            This is an example tab.
          </p>
          <p>
            You can put any sort of HTML or react component in here. It even keeps the component state!
          </p>
          <Slider name="slider0" defaultValue={0.5} />
        </div>
      </Tab>
      <Tab label="Item Two" >
        <div>
          <h2 style={styles.headline}>Tab Two</h2>
          <p>
            This is another example tab.
          </p>
        </div>
      </Tab>
      <Tab
        label="onActive"
        route="/home"
        onActive={handleActive}>
        <div>
          <h2 style={styles.headline}>Tab Three</h2>
          <p>
            This is a third example tab.
          </p>
        </div>
      </Tab>
    </Tabs>
  </MuiThemeProvider>
);

ReactDOM.render(React.createElement(TabsExampleSimple), document.getElementById('example'));
