require.config({
    baseUrl: 'js/lib',
    paths: {
        jquery: 'jquery-1.11.3.min',
        common: 'common',
        MainUI: 'MainUI',
    }
});

require(['MainUI'], function (MainUI) {
    MainUI.initialize();
})


