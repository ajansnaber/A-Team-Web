CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = "youtube,colordialog,colorbutton,panelbutton,justify,sourcedialog,font,confighelper,emojione";
	config.toolbar = "AJANSNABER";
    config.toolbar_AJANSNABER =
    [
        ["Sourcedialog","Cut","Copy","Paste","PasteText","PasteFromWord"],
        ["Undo","Redo","-","Find","Replace","-","SelectAll","RemoveFormat"],
        ["Image", "Table", "Youtube", "HorizontalRule", "SpecialChar"],
        ["Link", "Unlink"],
        "/",
        ["Font", "FontSize", "TextColor", "BGColor", "Emojione"],
        ["Styles", "Format"],
        ["JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"],
        ["Bold", "Italic", "Strike", "Underline"],
        ["NumberedList","BulletedList","-","Outdent","Indent","Blockquote"]
        ];
    config.language = "tr";

    var roxyFileman = "/scripts/plugins/ckeditor/fileman/index.html";
    config.filebrowserBrowseUrl = roxyFileman;
    config.filebrowserImageBrowseUrl = roxyFileman + '?type=image';
	config.removeButtons = 'Underline,Subscript,Superscript';
	config.format_tags = 'p;h1;h2;h3;pre';
    config.removeDialogTabs = 'link:upload;image:upload';
};
