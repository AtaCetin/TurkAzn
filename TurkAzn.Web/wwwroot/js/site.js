var app = new Framework7({
    on: {
        init: function() {
            console.log('App initialized');
        },
        pageInit: function() {
            console.log('Page initialized');
        },
    },
    root: '#app',
    statusbar: {
        iosOverlaysWebView: true,
    },
    theme: 'auto',
    name: 'TurkAzn',
    id: 'com.atacetin.TurkAzn',
    routes: [
        {
            path: '/about/',
            url: 'about.html',
        },
    ],
});

