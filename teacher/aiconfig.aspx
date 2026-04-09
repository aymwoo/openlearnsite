<%@ Page Title="AI 配置中心" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="aiconfig.aspx.cs" Inherits="Teacher_aiconfig" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/aiconfig.css" />

    <div class="ai-config">
        <div class="lesson-shell">

            <!-- Hero Banner -->
            <div class="lesson-hero">
                <div class="lesson-hero__content">
                    <div>
                        <h1 class="lesson-hero__title">AI 配置中心</h1>
                        <p class="lesson-hero__subtitle">管理 AI 服务提供商、系统提示词与技能配置，为课堂 AI 功能提供支撑。</p>
                    </div>
                </div>
            </div>

            <!-- Section 1: AI Provider -->
            <section class="lesson-card lesson-theme--blue">
                <div class="lesson-card__head">
                    <div>
                        <h2 class="lesson-card__title">AI 提供商</h2>
                        <p class="lesson-card__desc">配置连接到不同 AI 服务的 API 参数，可设置默认提供商。</p>
                    </div>
                    <div class="ai-head-actions">
                        <button type="button" onclick="openImportModal()" class="ai-btn ai-btn--green">
                            <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"></path></svg>
                            导入 JSON 配置
                        </button>
                        <button type="button" onclick="openModal()" class="ai-btn ai-btn--primary">
                            <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                            添加 AI 提供商
                        </button>
                    </div>
                </div>
                <div class="lesson-card__body">
                    <div id="providersList" class="ai-providers-grid">
                        <div class="ai-empty">正在加载...</div>
                    </div>
                </div>
            </section>

            <!-- Section 2: AI 提示词 (fixed-area prompts) -->
            <section class="lesson-card lesson-theme--teal">
                <div class="lesson-card__head">
                    <div>
                        <h2 class="lesson-card__title">AI 提示词</h2>
                        <p class="lesson-card__desc">管理各固定功能区域的系统提示词（System Prompt），用于定义 AI 角色与行为规范。</p>
                    </div>
                    <div class="ai-head-actions">
                        <button type="button" onclick="openSkillModal()" class="ai-btn ai-btn--teal">
                            <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                            添加提示词
                        </button>
                    </div>
                </div>
                <div class="lesson-card__body">
                    <div id="skillsList" class="ai-skills-grid">
                        <div class="ai-empty">正在加载...</div>
                    </div>
                </div>
            </section>

            <!-- Section 3: AI Skills 管理 (custom user-defined skills) -->
            <section class="lesson-card lesson-theme--purple">
                <div class="lesson-card__head">
                    <div>
                        <h2 class="lesson-card__title">AI Skills 管理</h2>
                        <p class="lesson-card__desc">自定义技能库，为每个技能设置提示词并指定应用场景，供学生在对应功能中调用。</p>
                    </div>
                    <div class="ai-head-actions">
                        <button type="button" onclick="openCustomSkillModal()" class="ai-btn ai-btn--purple">
                            <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path></svg>
                            添加技能
                        </button>
                    </div>
                </div>
                <div class="lesson-card__body">
                    <div id="customSkillsList" class="ai-skills-grid">
                        <div class="ai-empty">正在加载...</div>
                    </div>
                </div>
            </section>

        </div>
    </div>

    <!-- Add/Edit Provider Modal -->
    <div id="providerModal" class="fixed inset-0 bg-gray-900 bg-opacity-50 backdrop-blur-sm hidden overflow-y-auto h-full w-full z-50 flex items-center justify-center p-4" onclick="closeModalOnOutsideClick(event, 'providerModalContent')">
        <div id="providerModalContent" class="relative w-full max-w-lg shadow-2xl rounded-2xl bg-white border border-gray-100 p-6 md:p-8" onclick="event.stopPropagation()">
            <div>
                <div class="flex justify-between items-center mb-6">
                    <h3 class="text-xl font-bold text-gray-800" id="modalTitle">添加 AI 提供商</h3>
                    <button type="button" onclick="closeModal()" class="text-gray-400 hover:text-gray-600 transition-colors">
                        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
                    </button>
                </div>
                <div id="presetBar" class="mb-5">
                    <p class="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-2.5">快速选择模板</p>
                    <div class="flex flex-wrap gap-2" id="presetBtns"></div>
                </div>

                <div id="providerForm" class="space-y-5">
                    <input type="hidden" id="providerId" value="0">
                    
                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="displayName">显示名称 <span class="text-red-500">*</span></label>
                        <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="displayName" type="text" placeholder="例如: 通义千问" required>
                    </div>
                    
                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="providerName">提供商名称 <span class="text-red-500">*</span></label>
                        <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="providerName" type="text" placeholder="例如: Aliyun" required>
                    </div>

                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="modelName">模型名称 <span class="text-red-500">*</span></label>
                        <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="modelName" type="text" placeholder="例如: qwen-max" required>
                    </div>

                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="apiKey">API Key</label>
                        <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="apiKey" type="password" placeholder="填写对应的 API Key">
                    </div>

                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="baseUrl">Base URL</label>
                        <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="baseUrl" type="text" placeholder="https://api.openai.com/v1">
                    </div>

                    <div class="flex flex-col sm:flex-row items-center justify-between gap-3 sm:gap-0 mt-8 pt-6 border-t border-gray-100">
                        <div class="w-full sm:w-auto flex flex-col items-start gap-1">
                            <div id="testResultMsg" class="text-sm font-medium hidden"></div>
                            <button type="button" onclick="testConnection(event)" class="w-full sm:w-auto bg-green-500 hover:bg-green-600 text-white border-0 border-transparent font-semibold py-2.5 px-5 rounded-lg shadow-sm transition-all duration-200 flex items-center justify-center focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 whitespace-nowrap disabled:opacity-50 disabled:cursor-not-allowed">
                                <svg class="w-4 h-4 mr-2 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"></path></svg>
                                <span>测试连接</span>
                            </button>
                        </div>
                        <div class="flex gap-3 w-full sm:w-auto">
                            <button type="button" onclick="closeModal()" class="flex-1 sm:flex-none bg-white border border-gray-300 hover:bg-gray-50 hover:border-gray-400 text-gray-700 font-semibold py-2.5 px-6 rounded-lg transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-gray-200 focus:ring-offset-1">
                                取消
                            </button>
                            <button type="button" onclick="saveProvider(event)" class="flex-1 sm:flex-none bg-blue-600 hover:bg-blue-700 text-white border-0 border-transparent font-semibold py-2.5 px-8 rounded-lg shadow-sm transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-blue-600 focus:ring-offset-1">
                                保存
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Import JSON Modal -->
    <div id="importModal" class="fixed inset-0 bg-gray-900 bg-opacity-50 backdrop-blur-sm hidden overflow-y-auto h-full w-full z-50 flex items-center justify-center p-4" onclick="closeModalOnOutsideClick(event, 'importModalContent')">
        <div id="importModalContent" class="relative w-full max-w-2xl shadow-2xl rounded-2xl bg-white border border-gray-100 p-6 md:p-8" onclick="event.stopPropagation()">
            <div>
                <div class="flex justify-between items-center mb-6">
                    <h3 class="text-xl font-bold text-gray-800">导入 JSON 配置</h3>
                    <button type="button" onclick="closeImportModal()" class="text-gray-400 hover:text-gray-600 transition-colors">
                        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
                    </button>
                </div>
                <div class="mb-6">
                    <p class="text-sm font-semibold text-gray-700 mb-3">格式示例:</p>
                    <pre class="bg-gray-50 border border-gray-200 p-4 rounded-lg text-xs font-mono text-gray-700 mb-4 overflow-x-auto shadow-inner">
[
  {
    "DisplayName": "通义千问",
    "ProviderName": "Aliyun",
    "ModelName": "qwen-max",
    "ApiKey": "YOUR_API_KEY",
    "BaseUrl": "https://dashscope.aliyuncs.com/compatible-mode/v1"
  }
]
                    </pre>
                    <textarea id="jsonConfigInput" rows="10" class="box-border w-full max-w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none font-mono text-sm" placeholder="粘贴 JSON 配置..."></textarea>
                </div>
                <div class="flex flex-col-reverse sm:flex-row items-center justify-end gap-3 mt-6 pt-6 border-t border-gray-100">
                    <button type="button" onclick="closeImportModal()" class="w-full sm:w-auto bg-white border border-gray-300 hover:bg-gray-50 text-gray-700 font-semibold py-2.5 px-6 rounded-lg transition-colors duration-200">
                        取消
                    </button>
                    <button type="button" onclick="importJsonConfig()" class="w-full sm:w-auto bg-green-600 hover:bg-green-700 text-white font-semibold py-2.5 px-6 rounded-lg shadow-sm transition-colors duration-200 flex items-center justify-center">
                        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"></path></svg>
                        导入
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Add/Edit Skill Modal -->
    <div id="skillModal" class="fixed inset-0 bg-gray-900 bg-opacity-50 backdrop-blur-sm hidden overflow-y-auto h-full w-full z-50 flex items-center justify-center p-4" onclick="closeModalOnOutsideClick(event, 'skillModalContent')">
        <div id="skillModalContent" class="relative w-full max-w-lg shadow-2xl rounded-2xl bg-white border border-gray-100 p-6 md:p-8" onclick="event.stopPropagation()">
            <div>
                <div class="flex justify-between items-center mb-6">
                    <h3 class="text-xl font-bold text-gray-800" id="skillModalTitle">添加提示词</h3>
                    <button type="button" onclick="closeSkillModal()" class="text-gray-400 hover:text-gray-600 transition-colors">
                        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
                    </button>
                </div>
                <div id="skillForm" class="space-y-5">
                    <input type="hidden" id="skillId" value="0">

                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="skillName">提示词名称 <span class="text-red-500">*</span></label>
                        <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="skillName" type="text" placeholder="例如: Python 编程助手" required>
                    </div>

                    <div>
                        <label class="block text-gray-700 text-sm font-semibold mb-2" for="promptContent">提示词内容 <span class="text-red-500">*</span></label>
                        <textarea class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200 outline-none" id="promptContent" rows="6" placeholder="请在这里输入系统提示词内容..." required></textarea>
                    </div>

                    <div>
                        <label class="flex items-center space-x-3 cursor-pointer">
                            <input type="checkbox" id="skillIsActive" class="form-checkbox h-5 w-5 text-blue-600 rounded border-gray-300 focus:ring-blue-500" checked>
                            <span class="text-gray-700 text-sm font-semibold">是否启用</span>
                        </label>
                    </div>

                    <div class="flex gap-3 justify-end mt-8 pt-6 border-t border-gray-100">
                        <button type="button" onclick="closeSkillModal()" class="w-full sm:w-auto bg-white border border-gray-300 hover:bg-gray-50 hover:border-gray-400 text-gray-700 font-semibold py-2.5 px-6 rounded-lg transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-gray-200 focus:ring-offset-1">
                            取消
                        </button>
                        <button type="button" onclick="saveSkill(event)" class="w-full sm:w-auto bg-blue-600 hover:bg-blue-700 text-white border-0 border-transparent font-semibold py-2.5 px-8 rounded-lg shadow-sm transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-blue-600 focus:ring-offset-1">
                            保存
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="/js/aiconfig.js"></script>

    <!-- Add/Edit Custom Skill Modal -->
    <div id="customSkillModal" class="fixed inset-0 bg-gray-900 bg-opacity-50 backdrop-blur-sm hidden overflow-y-auto h-full w-full z-50 flex items-center justify-center p-4" onclick="closeModalOnOutsideClick(event, 'customSkillModalContent')">
        <div id="customSkillModalContent" class="relative w-full max-w-lg shadow-2xl rounded-2xl bg-white border border-gray-100 p-6 md:p-8" onclick="event.stopPropagation()">
            <div class="flex justify-between items-center mb-6">
                <h3 class="text-xl font-bold text-gray-800" id="customSkillModalTitle">添加技能</h3>
                <button type="button" onclick="closeCustomSkillModal()" class="text-gray-400 hover:text-gray-600 transition-colors">
                    <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
                </button>
            </div>
            <div class="space-y-5">
                <input type="hidden" id="customSkillId" value="0">

                <div>
                    <label class="block text-gray-700 text-sm font-semibold mb-2" for="customSkillName">技能名称 <span class="text-red-500">*</span></label>
                    <input class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200 outline-none" id="customSkillName" type="text" placeholder="例如: 代码审查助手">
                </div>

                <div>
                    <label class="block text-gray-700 text-sm font-semibold mb-2" for="customPromptContent">提示词内容 <span class="text-red-500">*</span></label>
                    <textarea class="box-border w-full max-w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-lg focus:bg-white focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200 outline-none" id="customPromptContent" rows="5" placeholder="请输入该技能的系统提示词..."></textarea>
                </div>

                <div>
                    <label class="block text-gray-700 text-sm font-semibold mb-2">应用场景 <span class="text-gray-400 font-normal">（可多选）</span></label>
                    <div class="grid grid-cols-2 gap-2 p-3 bg-gray-50 border border-gray-200 rounded-lg">
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="chat"> AI 对话
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="console"> 编程控制台
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="mission"> 任务辅助
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="writing"> 写作助手
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="quiz"> 习题解析
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="review"> 作品点评
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="gauge"> 量规生成
                        </label>
                        <label class="flex items-center gap-2 cursor-pointer text-sm text-gray-700 font-medium">
                            <input type="checkbox" class="custom-scope-cb h-4 w-4 text-purple-600 rounded border-gray-300" value="student_exam"> 学生测验评估
                        </label>
                    </div>
                </div>

                <div>
                    <label class="flex items-center space-x-3 cursor-pointer">
                        <input type="checkbox" id="customSkillIsActive" class="form-checkbox h-5 w-5 text-purple-600 rounded border-gray-300" checked>
                        <span class="text-gray-700 text-sm font-semibold">是否启用</span>
                    </label>
                </div>

                <div class="flex gap-3 justify-end pt-6 border-t border-gray-100">
                    <button type="button" onclick="closeCustomSkillModal()" class="bg-white border border-gray-300 hover:bg-gray-50 text-gray-700 font-semibold py-2.5 px-6 rounded-lg transition-all duration-200">
                        取消
                    </button>
                    <button type="button" onclick="saveCustomSkill(event)" class="bg-purple-600 hover:bg-purple-700 text-white border-0 font-semibold py-2.5 px-8 rounded-lg shadow-sm transition-all duration-200">
                        保存
                    </button>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
