const example_image_upload_handler = (blobInfo, progress) => new Promise((resolve, reject) => {
    const xhr = new XMLHttpRequest();
    xhr.withCredentials = false;
    xhr.open('POST', '/Image/Upload');

    xhr.upload.onprogress = (e) => {
        progress(e.loaded / e.total * 100);
    };

    xhr.onload = () => {
        if (xhr.status === 403) {
            reject({message: 'HTTP Error: ' + xhr.status, remove: true});
            return;
        }

        if (xhr.status < 200 || xhr.status >= 300) {
            reject('HTTP Error: ' + xhr.status);
            return;
        }

        const json = JSON.parse(xhr.responseText);

        if (!json || typeof json.location != 'string') {
            reject('Invalid JSON: ' + xhr.responseText);
            return;
        }

        resolve(json.location);
    };

    xhr.onerror = () => {
        reject('Image upload failed due to a XHR Transport error. Code: ' + xhr.status);
    };

    const formData = new FormData();
    formData.append('files', blobInfo.blob(), blobInfo.filename());

    xhr.send(formData);
});


tinymce.init({
    height: 600,
    selector: 'textarea',
    plugins: 'autosave image imagetools codesample lists link autolink code hr',
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

    images_upload_handler: example_image_upload_handler
});