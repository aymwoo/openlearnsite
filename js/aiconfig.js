$(document).ready(function() {
    loadProviders();
    loadSkills();
    loadCustomSkills();
});

// ── Providers ──────────────────────────────────────────────────

var providerPresets = [
    { label: '通义千问', icon: '🔮', displayName: '通义千问', providerName: 'Aliyun',      modelName: 'qwen-max',         baseUrl: 'https://dashscope.aliyuncs.com/compatible-mode/v1' },
    { label: '豆包',     icon: '🫘', displayName: '豆包',     providerName: 'Volcengine',  modelName: 'doubao-pro-128k',  baseUrl: 'https://ark.cn-beijing.volces.com/api/v3' },
    { label: '智谱GLM',  icon: '🧠', displayName: '智谱GLM',  providerName: 'ZhipuAI',     modelName: 'glm-4',            baseUrl: 'https://open.bigmodel.cn/api/paas/v4' },
    { label: 'Kimi',     icon: '🌙', displayName: 'Kimi',     providerName: 'Moonshot',    modelName: 'moonshot-v1-8k',   baseUrl: 'https://api.moonshot.cn/v1' },
    { label: 'MiniMax',  icon: '⚡', displayName: 'MiniMax',  providerName: 'MiniMax',     modelName: 'MiniMax-M1',       baseUrl: 'https://api.minimaxi.com/v1' },
    { label: 'DeepSeek', icon: '🐋', displayName: 'DeepSeek', providerName: 'DeepSeek',    modelName: 'deepseek-chat',    baseUrl: 'https://api.deepseek.com/v1' }
];

function renderPresetBtns() {
    var html = '';
    for (var i = 0; i < providerPresets.length; i++) {
        var p = providerPresets[i];
        html += '<button type="button" onclick="applyPreset(' + i + ')" '
             +  'class="inline-flex items-center gap-1.5 px-3 py-1.5 text-xs font-semibold rounded-lg border border-gray-200 bg-gray-50 text-gray-700 hover:bg-blue-50 hover:border-blue-300 hover:text-blue-700 transition-all duration-150 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-offset-1">'
             +  '<span>' + p.icon + '</span><span>' + p.label + '</span></button>';
    }
    $('#presetBtns').html(html);
}

function applyPreset(idx) {
    var p = providerPresets[idx];
    $('#displayName').val(p.displayName);
    $('#providerName').val(p.providerName);
    $('#modelName').val(p.modelName);
    $('#baseUrl').val(p.baseUrl);
    // highlight the selected preset button
    $('#presetBtns button').removeClass('bg-blue-50 border-blue-300 text-blue-700 ring-2 ring-blue-400')
                          .addClass('bg-gray-50 border-gray-200 text-gray-700');
    $('#presetBtns button').eq(idx).removeClass('bg-gray-50 border-gray-200 text-gray-700')
                                  .addClass('bg-blue-50 border-blue-300 text-blue-700 ring-2 ring-blue-400');
}

function loadProviders() {
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: { action: 'list' },
        success: function(res) {
            if (res.success) {
                renderProviders(res.data);
            } else {
                $('#providersList').html('<div class="ai-empty">加载失败: ' + res.msg + '</div>');
            }
        },
        error: function() {
            $('#providersList').html('<div class="ai-empty">网络错误，无法加载数据。</div>');
        }
    });
}

function renderProviders(data) {
    if (data.length === 0) {
        $('#providersList').html('<div class="ai-empty">暂无 AI 提供商配置，请点击右上角"添加 AI 提供商"。</div>');
        return;
    }
    var html = '';
    $.each(data, function(i, item) {
        var cardClass = 'ai-provider-card' + (item.IsDefault ? ' ai-provider-card--default' : '');
        var badge = item.IsDefault
            ? '<span class="ai-badge ai-badge--green">默认</span>'
            : '';
        var setDefaultBtn = !item.IsDefault
            ? '<button type="button" onclick="setDefault(' + item.Id + ')" class="ai-card-action-btn ai-card-action-btn--def">设为默认</button>'
            : '';

        html += '<div class="' + cardClass + '">';
        html += '  <div class="ai-provider-card__body">';
        html += '    <div style="display:flex;justify-content:space-between;align-items:flex-start;gap:8px;">';
        html += '      <div>';
        html += '        <p class="ai-provider-card__name">' + escapeHtml(item.DisplayName) + '</p>';
        html += '        <span class="ai-provider-card__sub">' + escapeHtml(item.ProviderName) + '</span>';
        html += '      </div>';
        html += '      <div>' + badge + '</div>';
        html += '    </div>';
        html += '    <div class="ai-provider-card__meta">模型：<strong>' + escapeHtml(item.ModelName) + '</strong></div>';
        html += '  </div>';
        html += '  <div class="ai-provider-card__foot">';
        html += '    <div>' + setDefaultBtn + '</div>';
        html += '    <div style="display:flex;gap:6px;">';
        html += '      <button type="button" onclick=\'duplicateProvider(' + JSON.stringify(item).replace(/'/g, "\\'") + ')\' class="ai-card-action-btn ai-card-action-btn--copy" title="复制配置"><svg style="width:13px;height:13px;margin-right:2px;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z"/></svg>复制</button>';
        html += '      <button type="button" onclick=\'editProvider(' + JSON.stringify(item).replace(/'/g, "\\'") + ')\' class="ai-card-action-btn ai-card-action-btn--edit">编辑</button>';
        html += '      <button type="button" onclick="deleteProvider(' + item.Id + ')" class="ai-card-action-btn ai-card-action-btn--del">删除</button>';
        html += '    </div>';
        html += '  </div>';
        html += '</div>';
    });
    $('#providersList').html(html);
}

function openModal() {
    $('#modalTitle').text('添加 AI 提供商');
    $('#providerId').val('0');
    $('#displayName').val('');
    $('#providerName').val('');
    $('#modelName').val('');
    $('#apiKey').val('');
    $('#baseUrl').val('https://api.openai.com/v1');
    $('#testResultMsg').addClass('hidden').text('');
    renderPresetBtns();
    $('#presetBar').show();
    $('#providerModal').removeClass('hidden').addClass('flex');
}

function editProvider(item) {
    $('#modalTitle').text('编辑 AI 提供商');
    $('#providerId').val(item.Id);
    $('#displayName').val(item.DisplayName);
    $('#providerName').val(item.ProviderName);
    $('#modelName').val(item.ModelName);
    $('#apiKey').val(item.ApiKey);
    $('#baseUrl').val(item.BaseUrl);
    $('#testResultMsg').addClass('hidden').text('');
    $('#presetBar').hide();
    $('#providerModal').removeClass('hidden').addClass('flex');
}

function duplicateProvider(item) {
    $('#modalTitle').text('复制新建 AI 提供商');
    $('#providerId').val('0');
    $('#displayName').val(item.DisplayName + ' (副本)');
    $('#providerName').val(item.ProviderName);
    $('#modelName').val(item.ModelName);
    $('#apiKey').val('');
    $('#baseUrl').val(item.BaseUrl);
    $('#testResultMsg').addClass('hidden').text('');
    $('#presetBar').hide();
    $('#providerModal').removeClass('hidden').addClass('flex');
}

function closeModal() {
    $('#providerModal').addClass('hidden').removeClass('flex');
}

function openImportModal() {
    $('#jsonConfigInput').val('');
    $('#importModal').removeClass('hidden').addClass('flex');
}

function closeImportModal() {
    $('#importModal').addClass('hidden').removeClass('flex');
}

function saveProvider(e) {
    e.preventDefault();
    var data = {
        action: 'save',
        id: $('#providerId').val(),
        displayName: $('#displayName').val(),
        providerName: $('#providerName').val(),
        modelName: $('#modelName').val(),
        apiKey: $('#apiKey').val(),
        baseUrl: $('#baseUrl').val()
    };
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: data,
        success: function(res) {
            if (res.success) {
                closeModal();
                loadProviders();
            } else {
                alert("保存失败: " + res.msg);
            }
        },
        error: function() { alert("网络错误，保存失败。"); }
    });
}

function deleteProvider(id) {
    if (confirm("确定要删除此配置吗？")) {
        $.ajax({
            url: 'aiprovider_api.ashx',
            type: 'POST',
            data: { action: 'delete', id: id },
            success: function(res) {
                if (res.success) { loadProviders(); }
                else { alert("删除失败: " + res.msg); }
            },
            error: function() { alert("网络错误，删除失败。"); }
        });
    }
}

function setDefault(id) {
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: { action: 'setdefault', id: id },
        success: function(res) {
            if (res.success) { loadProviders(); }
            else { alert("设置失败: " + res.msg); }
        },
        error: function() { alert("网络错误，设置失败。"); }
    });
}

function testConnection(e) {
    var btn = $(e.currentTarget);
    var span = btn.find('span');
    var originalText = span.text();
    var resultMsg = $('#testResultMsg');

    resultMsg.removeClass('hidden text-green-600 text-red-600').addClass('text-gray-500').text('正在测试中，请稍候...');
    span.text('测试中...');
    btn.prop('disabled', true);

    var data = {
        action: 'test',
        id: $('#providerId').val(),
        modelName: $('#modelName').val(),
        apiKey: $('#apiKey').val(),
        baseUrl: $('#baseUrl').val()
    };

    if (!data.baseUrl || !data.modelName) {
        resultMsg.removeClass('text-gray-500').addClass('text-red-600').text('请至少填写 Base URL 和 模型名称');
        span.text(originalText);
        btn.prop('disabled', false);
        return;
    }

    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: data,
        success: function(res) {
            if (res.success) {
                resultMsg.removeClass('text-gray-500 text-red-600').addClass('text-green-600').text('测试成功！' + res.msg);
            } else {
                resultMsg.removeClass('text-gray-500 text-green-600').addClass('text-red-600').text('测试失败: ' + res.msg);
            }
        },
        error: function() {
            resultMsg.removeClass('text-gray-500 text-green-600').addClass('text-red-600').text('网络错误，测试请求发送失败。');
        },
        complete: function() {
            span.text(originalText);
            btn.prop('disabled', false);
        }
    });
}

function importJsonConfig() {
    var jsonStr = $('#jsonConfigInput').val().trim();
    if (!jsonStr) { alert("请输入 JSON 配置。"); return; }
    try { JSON.parse(jsonStr); } catch (e) { alert("JSON 格式不正确: " + e.message); return; }
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: { action: 'import', config: jsonStr },
        success: function(res) {
            if (res.success) {
                alert(res.msg);
                closeImportModal();
                loadProviders();
            } else {
                alert("导入失败: " + res.msg);
            }
        },
        error: function() { alert("网络错误，导入失败。"); }
    });
}

// ── Skills / 提示词 ────────────────────────────────────────────

function loadSkills() {
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: { action: 'listSkills' },
        success: function(res) {
            if (res.success) {
                renderSkills(res.data);
            } else {
                $('#skillsList').html('<div class="ai-empty">加载失败: ' + res.msg + '</div>');
            }
        },
        error: function() {
            $('#skillsList').html('<div class="ai-empty">网络错误，无法加载数据。</div>');
        }
    });
}

function renderSkills(data) {
    if (data.length === 0) {
        $('#skillsList').html('<div class="ai-empty">暂无提示词配置，请点击右上角"添加提示词"。</div>');
        return;
    }
    var html = '';
    $.each(data, function(i, item) {
        var badge = item.IsActive
            ? '<span class="ai-badge ai-badge--active">已启用</span>'
            : '<span class="ai-badge ai-badge--gray">已停用</span>';
        var safePrompt = escapeHtml(item.PromptContent);
        var jsonStr = JSON.stringify(item).replace(/'/g, "&#39;").replace(/"/g, "&quot;");

        html += '<div class="ai-skill-card">';
        html += '  <div class="ai-skill-card__body">';
        html += '    <div style="display:flex;justify-content:space-between;align-items:flex-start;gap:8px;margin-bottom:10px;">';
        html += '      <h3 class="ai-skill-card__name">' + escapeHtml(item.SkillName) + '</h3>';
        html += '      <div>' + badge + '</div>';
        html += '    </div>';
        html += '    <p class="ai-skill-card__prompt" title="' + safePrompt + '">' + safePrompt + '</p>';
        html += '  </div>';
        html += '  <div class="ai-skill-card__foot">';
        html += '    <button type="button" onclick="editSkill(' + jsonStr + ')" class="ai-card-action-btn ai-card-action-btn--edit">编辑</button>';
        html += '    <button type="button" onclick="deleteSkill(' + item.Id + ')" class="ai-card-action-btn ai-card-action-btn--del">删除</button>';
        html += '  </div>';
        html += '</div>';
    });
    $('#skillsList').html(html);
}

function openSkillModal() {
    $('#skillModalTitle').text('添加提示词');
    $('#skillId').val('0');
    $('#skillName').val('');
    $('#promptContent').val('');
    $('#skillIsActive').prop('checked', true);
    $('#skillModal').removeClass('hidden').addClass('flex');
}

function editSkill(item) {
    $('#skillModalTitle').text('编辑提示词');
    $('#skillId').val(item.Id);
    $('#skillName').val(item.SkillName);
    $('#promptContent').val(item.PromptContent);
    $('#skillIsActive').prop('checked', item.IsActive);
    $('#skillModal').removeClass('hidden').addClass('flex');
}

function closeSkillModal() {
    $('#skillModal').addClass('hidden').removeClass('flex');
}

function saveSkill(e) {
    e.preventDefault();
    var data = {
        action: 'saveSkill',
        id: $('#skillId').val(),
        skillName: $('#skillName').val(),
        promptContent: $('#promptContent').val(),
        isActive: $('#skillIsActive').is(':checked')
    };
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: data,
        success: function(res) {
            if (res.success) {
                closeSkillModal();
                loadSkills();
            } else {
                alert("保存失败: " + res.msg);
            }
        },
        error: function() { alert("网络错误，保存失败。"); }
    });
}

function deleteSkill(id) {
    if (confirm("确定要删除此提示词吗？")) {
        $.ajax({
            url: 'aiprovider_api.ashx',
            type: 'POST',
            data: { action: 'deleteSkill', id: id },
            success: function(res) {
                if (res.success) { loadSkills(); }
                else { alert("删除失败: " + res.msg); }
            },
            error: function() { alert("网络错误，删除失败。"); }
        });
    }
}

// ── Utilities ──────────────────────────────────────────────────

function escapeHtml(unsafe) {
    if (!unsafe) return '';
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function closeModalOnOutsideClick(event, contentId) {
    var modalContent = document.getElementById(contentId);
    if (modalContent && !modalContent.contains(event.target)) {
        if (contentId === 'providerModalContent') closeModal();
        else if (contentId === 'importModalContent') closeImportModal();
        else if (contentId === 'skillModalContent') closeSkillModal();
        else if (contentId === 'customSkillModalContent') closeCustomSkillModal();
    }
}

document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        if (!$('#providerModal').hasClass('hidden')) closeModal();
        if (!$('#importModal').hasClass('hidden')) closeImportModal();
        if (!$('#skillModal').hasClass('hidden')) closeSkillModal();
        if (!$('#customSkillModal').hasClass('hidden')) closeCustomSkillModal();
    }
});

// ── Custom Skills ──────────────────────────────────────────────

function loadCustomSkills() {
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: { action: 'listCustomSkills' },
        success: function(res) {
            if (res.success) {
                renderCustomSkills(res.data);
            } else {
                $('#customSkillsList').html('<div class="ai-empty">加载失败: ' + res.msg + '</div>');
            }
        },
        error: function() {
            $('#customSkillsList').html('<div class="ai-empty">网络错误，无法加载数据。</div>');
        }
    });
}

var scopeLabels = {
    'chat':    'AI 对话',
    'console': '编程控制台',
    'mission': '任务辅助',
    'writing': '写作助手',
    'quiz':    '习题解析',
    'review':  '作品点评',
    'gauge':   '量规生成',
    'student_exam': '学生测验评估'
};

function renderCustomSkills(data) {
    if (data.length === 0) {
        $('#customSkillsList').html('<div class="ai-empty">暂无自定义技能，请点击右上角"添加技能"。</div>');
        return;
    }
    var html = '';
    $.each(data, function(i, item) {
        var badge = item.IsActive
            ? '<span class="ai-badge ai-badge--active">已启用</span>'
            : '<span class="ai-badge ai-badge--gray">已停用</span>';
        var safePrompt = escapeHtml(item.PromptContent);
        var jsonStr = JSON.stringify(item).replace(/'/g, "&#39;").replace(/"/g, "&quot;");

        // Scope tags
        var scopeHtml = '';
        if (item.SkillScope && item.SkillScope.trim() !== '') {
            var scopes = item.SkillScope.split(',');
            $.each(scopes, function(j, s) {
                s = s.trim();
                if (s && scopeLabels[s]) {
                    scopeHtml += '<span class="ai-scope-tag">' + scopeLabels[s] + '</span>';
                }
            });
        }
        var scopeBlock = scopeHtml
            ? '<div class="ai-scope-tags">' + scopeHtml + '</div>'
            : '<div class="ai-scope-tags"><span style="font-size:12px;color:#94a3b8;">未指定应用场景</span></div>';

        html += '<div class="ai-skill-card">';
        html += '  <div class="ai-skill-card__body">';
        html += '    <div style="display:flex;justify-content:space-between;align-items:flex-start;gap:8px;margin-bottom:10px;">';
        html += '      <h3 class="ai-skill-card__name">' + escapeHtml(item.SkillName) + '</h3>';
        html += '      <div>' + badge + '</div>';
        html += '    </div>';
        html += '    <p class="ai-skill-card__prompt" title="' + safePrompt + '">' + safePrompt + '</p>';
        html += scopeBlock;
        html += '  </div>';
        html += '  <div class="ai-skill-card__foot">';
        html += '    <button type="button" onclick="editCustomSkill(' + jsonStr + ')" class="ai-card-action-btn ai-card-action-btn--edit">编辑</button>';
        html += '    <button type="button" onclick="deleteCustomSkill(' + item.Id + ')" class="ai-card-action-btn ai-card-action-btn--del">删除</button>';
        html += '  </div>';
        html += '</div>';
    });
    $('#customSkillsList').html(html);
}

function openCustomSkillModal() {
    $('#customSkillModalTitle').text('添加技能');
    $('#customSkillId').val('0');
    $('#customSkillName').val('');
    $('#customPromptContent').val('');
    $('#customSkillIsActive').prop('checked', true);
    $('.custom-scope-cb').prop('checked', false);
    $('#customSkillModal').removeClass('hidden').addClass('flex');
}

function editCustomSkill(item) {
    $('#customSkillModalTitle').text('编辑技能');
    $('#customSkillId').val(item.Id);
    $('#customSkillName').val(item.SkillName);
    $('#customPromptContent').val(item.PromptContent);
    $('#customSkillIsActive').prop('checked', item.IsActive);
    // Restore scope checkboxes
    $('.custom-scope-cb').prop('checked', false);
    if (item.SkillScope) {
        var scopes = item.SkillScope.split(',');
        $.each(scopes, function(i, s) {
            $('.custom-scope-cb[value="' + s.trim() + '"]').prop('checked', true);
        });
    }
    $('#customSkillModal').removeClass('hidden').addClass('flex');
}

function closeCustomSkillModal() {
    $('#customSkillModal').addClass('hidden').removeClass('flex');
}

function saveCustomSkill(e) {
    e.preventDefault();
    var scopes = [];
    $('.custom-scope-cb:checked').each(function() {
        scopes.push($(this).val());
    });
    var data = {
        action: 'saveCustomSkill',
        id: $('#customSkillId').val(),
        skillName: $('#customSkillName').val(),
        promptContent: $('#customPromptContent').val(),
        skillScope: scopes.join(','),
        isActive: $('#customSkillIsActive').is(':checked')
    };
    $.ajax({
        url: 'aiprovider_api.ashx',
        type: 'POST',
        data: data,
        success: function(res) {
            if (res.success) {
                closeCustomSkillModal();
                loadCustomSkills();
            } else {
                alert("保存失败: " + res.msg);
            }
        },
        error: function() { alert("网络错误，保存失败。"); }
    });
}

function deleteCustomSkill(id) {
    if (confirm("确定要删除此技能吗？")) {
        $.ajax({
            url: 'aiprovider_api.ashx',
            type: 'POST',
            data: { action: 'deleteCustomSkill', id: id },
            success: function(res) {
                if (res.success) { loadCustomSkills(); }
                else { alert("删除失败: " + res.msg); }
            },
            error: function() { alert("网络错误，删除失败。"); }
        });
    }
}
