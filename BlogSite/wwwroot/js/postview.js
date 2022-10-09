var clipboardArr = [];
$(document).ready(function () {
    $('.code-toolbar').each(function (index) {
        var toolbarId = 'toolbar-' + index;
        $(this).children('.toolbar').addClass(toolbarId);
        $('.' + toolbarId).append('<div class="copy w3-button w3-dark-gray">Copy</div>');
    });

    $("pre").each(function (index) {
        $(this).addClass('line-numbers');
    });

    const clipboard = new Clipboard('.copy', {
        target: (trigger) => {
            console.log();
            return trigger.parentElement.parentElement.children[0];
        }
    });
    clipboard.on('success', (event) => {
        event.trigger.textContent = 'Copied!';
        setTimeout(() => {
            event.clearSelection();
            event.trigger.textContent = 'Copy';
        }, 1000);
    });
    Prism.highlightAll();
});