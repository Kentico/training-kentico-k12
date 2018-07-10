
function InsertSelectedItem(obj){
    if ((window.wopener) && (obj)) {
		if ((obj.editor_clientid != null) && (obj.editor_clientid != '')) {

		    var editor = window.wopener.document.getElementById(obj.editor_clientid);
			if (editor != null) {
				var url = null;
				var guid = null;
				if ((obj.img_url) && (obj.img_url != '')) {
					url = obj.img_url;
				}
				else if ((obj.av_url) && (obj.av_url != '')) {
					url = obj.av_url;
				}
				else if ((obj.url_url) && (obj.url_url != '')) {
					url = obj.url_url;
					guid = (obj.url_guid ? obj.url_guid : null);
				}
				if ((guid == null) && (url != null)) {
					guid = url.match(/([a-f\d]{8}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{12})/i)[1];
				}
				if ((editor.value != null) && (guid != null)) {
					editor.value = guid;
					if (editor.onchange) {
						editor.onchange();
					}
				}
			}
		}
	}
}

function GetSelectedItem(editorId){
	var obj = null;
	if ((editorId) && (editorId != '')) {
	    if (window.wopener) {
		    var editorNewId = editorId.replace(/txtPath$/, 'hidValue');
		    var editor = window.wopener.document.getElementById(editorNewId);
			if ((editor != null) && (editor.value) && (editor.value != '')) {
				obj = new Object();
				var guid = editor.value.match(/([a-f\d]{8}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{12})/i);
				if (guid) {
					obj.url_guid = editor.value;
					obj.url_url = '~/getfile/' + editor.value + '/default.aspx';
				}
			}
		}
	}
	return obj;
}

