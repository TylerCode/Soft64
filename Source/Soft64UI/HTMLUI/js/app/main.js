require.config({
    baseUrl: 'js/lib',
    paths: {
        jquery: 'jquery-1.11.3.min',
        jqueryui: 'jquery-ui.min',
        common: 'common',
        MainUI: 'MainUI',
    }
});

require(['MainUI'], function (MainUI) {
    MainUI.initialize();
})

