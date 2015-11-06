require.config({
    baseUrl: 'js/lib',
    paths: {
        jquery: 'jquery-1.11.3.min',
        jqueryui: 'jquery-ui.min',
        common: 'common',
        Window: 'window',
        MainUI: 'MainUI',
    }
});

require(['MainUI', 'Window'], function (MainUI, Window) {
    MainUI.initialize();

    var testWindow = new Window();
    testWindow.title = "testing";
    testWindow.create('windows/MenuWindow.html');
    testWindow.initialize();
})

