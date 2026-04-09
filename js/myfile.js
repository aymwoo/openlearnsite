// Render file type badges with color coding
(function() {
    var colorMap = {
        'zip':'ft-zip','rar':'ft-zip','7z':'ft-zip',
        'exe':'ft-exe','msi':'ft-exe',
        'pdf':'ft-pdf',
        'doc':'ft-doc','docx':'ft-docx',
        'xls':'ft-xls','xlsx':'ft-xlsx',
        'ppt':'ft-ppt','pptx':'ft-pptx',
        'mp4':'ft-mp4','avi':'ft-avi','mov':'ft-mov',
        'mp3':'ft-mp3','wav':'ft-wav',
        'png':'ft-png','jpg':'ft-png','gif':'ft-gif','jpeg':'ft-png'
    };
    // Target the 4th column (index 3) cells in GVSoft
    var table = document.getElementById(window.__myfileConfig.gVSoftId);
    if (!table) return;
    var rows = table.rows;
    for (var i = 1; i < rows.length; i++) {
        var cells = rows[i].cells;
        if (cells.length < 4) continue;
        // File type cell (col index 3)
        var ftCell = cells[3];
        var ext = (ftCell.innerText || ftCell.textContent || '').trim().toLowerCase().replace('.','');
        var cls = colorMap[ext] || 'ft-default';
        if (ext) {
            ftCell.innerHTML = '<span class="ft-badge ' + cls + '">' + ext + '</span>';
        }
        // Download count cell (col index 4)
        var dlCell = cells[4];
        var count = (dlCell.innerText || dlCell.textContent || '').trim();
        if (count !== '') {
            dlCell.innerHTML = '<span class="dl-count"><svg style="width:11px;height:11px" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"/></svg>' + count + '</span>';
        }
    }
    // Style category hyperlinks as cat-btn
    var catTable = document.getElementById(window.__myfileConfig.gVcategoryId);
    if (catTable) {
        var catRows = catTable.rows;
        for (var j = 0; j < catRows.length; j++) {
            var links = catRows[j].getElementsByTagName('a');
            for (var k = 0; k < links.length; k++) {
                links[k].className = 'cat-btn';
                links[k].innerHTML = '<svg class="cat-btn-icon" style="width:14px;height:14px;flex-shrink:0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z"/></svg>' + links[k].textContent;
            }
        }
    }
})();
