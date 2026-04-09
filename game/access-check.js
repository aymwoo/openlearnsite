(function() {
    var urlParams = new URLSearchParams(window.location.search);
    var token = urlParams.get('token');
    
    if (!token) {
        alert('请从正确的页面访问游戏！');
        if (document.referrer) {
            history.back();
        } else {
            window.close();
        }
        return;
    }
    
    var xhr = new XMLHttpRequest();
    xhr.open('GET', '../api/GameAccess.ashx?action=check&token=' + token, false);
    xhr.send();
    
    try {
        var result = JSON.parse(xhr.responseText);
        if (result.code !== 1) {
            alert(result.msg || '无权访问此游戏！');
            if (document.referrer) {
                history.back();
            } else {
                window.close();
            }
        }
    } catch (e) {
        alert('验证失败，请重新访问！');
        history.back();
    }
    
    history.pushState(null, null, location.href);
    window.addEventListener('popstate', function() {
        history.pushState(null, null, location.href);
    });
    
    document.addEventListener('contextmenu', function(e) {
        e.preventDefault();
        return false;
    });
    
    document.addEventListener('keydown', function(e) {
        if (e.ctrlKey && (e.key === 'u' || e.key === 'U' || e.key === 's' || e.key === 'S')) {
            e.preventDefault();
            return false;
        }
        if (e.key === 'F12') {
            e.preventDefault();
            return false;
        }
    });
})();
