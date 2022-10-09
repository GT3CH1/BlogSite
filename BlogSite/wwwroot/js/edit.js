tinymce.init({
    height: 600,
    selector: 'textarea',
    plugins: 'autosave image codesample lists link autolink code hr',
    toolbar: 'undo redo | bold italic underline forecolor removeformat | fontselect fontsizeselect formatselect | alignleft aligncenter alignright | image codesample numlist bullist insert link | hr restoredraft',
    toolbar_drawer: 'floating',
    autosave_ask_before_unload: false,
    autosave_restore_when_empty: true,
    paste_as_text: true,
    paste_enable_default_filters: false,
    default_link_target: "_blank",
    browser_spellcheck: true,
    convert_urls: false,
    images_upload_url: '/Image/UploadImage',
    codesample_global_prismjs: true,
    themes: "modern",
    codesample_languages: [
        {text: 'HTML/XML', value: 'markup'},
        {text: 'JavaScript', value: 'javascript'},
        {text: 'CSS', value: 'css'},
        {text: 'PHP', value: 'php'},
        {text: 'Bash', value: 'bash'},
        {text: 'Python', value: 'python'},
        {text: 'Java', value: 'java'},
        {text: 'C', value: 'c'},
        {text: 'C#', value: 'csharp'},
        {text: 'C++', value: 'cpp'},
        {text: 'SQL', value: 'sql'},
        {text: 'LaTeX', value: 'latex'},
        {text: 'Markdown', value: 'markdown'},
        {text: 'XML', value: 'xml'},
        {text: 'Powershell', value: 'powershell'},
        {text: 'YAML', value: 'yaml'},
        {text: 'Dockerfile', value: 'dockerfile'},
        {text: 'JSON', value: 'json'}
    ],
    codesample_global_prismjs: true,
    content_css: [
        '/lib/tinymce/plugins/codesample/css/prism.css',
        '/css/codeformat.css'
    ],

    images_upload_handler: function (blobInfo, success, failure) {
        var xhr, formData;
        xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open('POST', '/Image/UploadImage');
        xhr.onload = function () {
            var json;
            if (xhr.status != 200) {
                failure('HTTP Error: ' + xhr.status);
                console.error(xhr.responseText);
                return;
            }
            json = JSON.parse(xhr.responseText);
            if (!json || typeof json.location != 'string') {
                failure('Invalid JSON: ' + xhr.responseText);
                console.error(xhr.responseText);
                return;
            }
            success(json.location);
        };
        formData = new FormData();
        formData.append('fileData', blobInfo.base64());
        formData.append('fileName', blobInfo.filename());
        xhr.send(formData);
    },
});