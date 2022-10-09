tinymce.PluginManager.add('hr', function (editor, url) {
    editor.ui.registry.addButton('hr', {
        text: 'My button',
        onAction: function () {
            editor.insertContent('<hr />');
        }
    });
});