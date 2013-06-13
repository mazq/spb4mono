(function () {
    // Load plugin specific language pack
    tinymce.PluginManager.requireLangPack('changemode');

    tinymce.create('tinymce.plugins.ChangeModePlugin', {
        init: function (ed, url) {
            ed.addCommand('mceChangeMode', function () {
                var ChangeModeFunctionScript = $("#" + ed.id).data("ChangeModeFunctionScript");
                if (ChangeModeFunctionScript)
                    ChangeModeFunctionScript(ed);
            });

            // Register buttons
            ed.addButton('changemode', { title: 'changemode.title', cmd: 'mceChangeMode' });

            ed.onPostRender.add(function (ed, cm) {
                var ChangeMode_IsFull = $("#" + ed.id).data("ChangeMode_IsFull");               
                var cMode = document.getElementById(ed.id + '_changemode');
                var span;
                if (cMode) {
                    cMode.style.marginLeft = '10px';
                    cMode.style.width = '60px';
                    cMode.style.textAlign = 'center';
                    cMode.style.verticalAlign = 'middle';
                    cMode.style.color = '#005eac';
                    cMode.style.textDecoration = 'underline';

                    if (ChangeMode_IsFull)
                        cMode.innerHTML = ed.getLang('changemode.simple');
                    else
                        cMode.innerHTML = ed.getLang('changemode.full');
                }
            });
        },

        getInfo: function () {
            return {
                longname: 'changemode',
                author: 'Tunynet inc.',
                authorurl: 'http://www.tunynet.com/',
                infourl: 'http://www.tunynet.com/',
                version: "1.1"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('changemode', tinymce.plugins.ChangeModePlugin);
})();