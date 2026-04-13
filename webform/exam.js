const examScore = document.getElementById('examScore');//分值标签
const examSideAiToggle = document.getElementById('examSideAiToggle');
const examSideAiStatus = document.getElementById('examSideAiStatus');
const examSideAiSetting = document.getElementById('examSideAiSetting');
// 试卷数据结构
const mycid = document.getElementById('HiddenCid').value;
const myeid = document.getElementById('HiddenEid').value;
const myjson = decodeURIComponent(atob(document.getElementById('HiddenExamjson').value));
console.log("编号测试：",mycid,myeid);
//console.log("数据测试：",myjson);
const localStorageFlag = "examData_"+mycid+"_"+myeid;
//const localStorageFlag = "examData"

let examData = {
    eid:myeid,
    cid:mycid,
    timestamp:null,
    title: "",
    description: "",
    enableAiAssessment: false,
    questions: []
};

//console.log("初始数据结构",examData);

// 连线题状态管理
let matchingState = {
    activeQuestion: null,
    selectedLeft: null,
    selectedRight: null,
    connections: {},
    tempLine: null
};

// 学生信息
let studentInfo = {
    name: "测试学生",
    id: "2024001",
    class: "测试班级"
};

// 提交结果数据
let submissionResult = null;

function updateScore(){
    const total_score = examData.questions.reduce((sum, q) => sum + q.score, 0);
    examScore.innerText = total_score+" 分";
}

function syncExamAiSetting() {
    const enabled = !!examData.enableAiAssessment;
    if (examSideAiToggle) {
        examSideAiToggle.checked = enabled;
    }
    if (examSideAiStatus) {
        examSideAiStatus.innerText = enabled ? '当前试卷已启用' : '当前试卷未启用';
    }
    if (examSideAiSetting) {
        examSideAiSetting.classList.toggle('is-enabled', enabled);
    }
}

// 初始化函数
function init() {
    setupEventListeners();
    
    if(myeid=="0"){
        loadExamData();
        console.log("加载缓存数据");
    }  
    else{  
        readExamData();//读取数据库，renderQuestions将在数据加载完成后调用
    }
    examData.eid = myeid;//设定当前试卷ID，很重要
    examData.cid = mycid; 
    renderQuestions();
}

// 从数据库加载试卷数据
function readExamData() {               
    if (myjson) {
        examData = JSON.parse(myjson);
        if (typeof examData.enableAiAssessment !== 'boolean') {
            examData.enableAiAssessment = false;
        }
        console.log("读取数据库",examData);
        saveExamData();
        // 数据加载完成后重新渲染页面
    }
}

// 从localStorage加载试卷数据
function loadExamData() {
    const savedData = localStorage.getItem(localStorageFlag);
    if (savedData) {
        examData = JSON.parse(savedData);
        if (typeof examData.enableAiAssessment !== 'boolean') {
            examData.enableAiAssessment = false;
        }
    }
}

// 保存试卷数据到localStorage
function saveExamData() {
    examData.timestamp = Math.floor(Date.now() / 1000);
    localStorage.setItem(localStorageFlag, JSON.stringify(examData));
    updateScore();
}

// 设置事件监听器
function setupEventListeners() {
    // 按钮事件
    const previewModalBtn = document.getElementById('previewModalBtn');
    if (previewModalBtn) {
        previewModalBtn.addEventListener('click', showPreview);
    }
        
    const saveExamBtn = document.getElementById('saveExamBtn');
    if (saveExamBtn) {
        saveExamBtn.addEventListener('click', saveExamAsJson);
    }
    
    const importBtn = document.getElementById('importBtn');
    if (importBtn) {
        importBtn.addEventListener('click', () => {
            document.getElementById('importFileInput').click();
        });
    }
    
    const importFileInput = document.getElementById('importFileInput');
    if (importFileInput) {
        importFileInput.addEventListener('change', handleImportJson);
    }
    
    const clearExamBtn = document.getElementById('clearExamBtn');
    if (clearExamBtn) {
        clearExamBtn.addEventListener('click', clearAllQuestions);
    }
    
    const closeBtn = document.querySelector('.close');
    if (closeBtn) {
        closeBtn.addEventListener('click', hidePreview);
    }

    // 点击空白处关闭预览
    window.addEventListener('click', (e) => {
        if (e.target === document.getElementById('previewModal')) {
            //hidePreview();
        }
    });
}

// 更新试卷标题
function updateExamTitle(newTitle) {
    examData.title = newTitle;
    saveExamData();
}

// 更新试卷描述
function updateExamDescription(newDescription) {
    examData.description = newDescription;
    saveExamData();
}

function updateExamAiAssessment(enabled) {
    examData.enableAiAssessment = !!enabled;
    syncExamAiSetting();
    saveExamData();
}

// 创建新题目
function createNewQuestion(type) {
    const questionId = 'q_' + Date.now();
    
    let newQuestion = {
        id: questionId,
        type: type,
        title: '', // 使用HTML格式
        score: 5
    };

    // 根据题目类型设置默认属性
    switch(type) {
        case 'single_choice':
            newQuestion.options = ['', '', '', ''];
            newQuestion.answer = 0; // 默认第一个选项为答案
            break;
        case 'multiple_choice':
            newQuestion.options = ['', '', '', ''];
            newQuestion.answer = [0]; // 默认第一个选项为答案，数组格式
            break;
        case 'fill_blank':
            newQuestion.blanks = [
                { answer: '' },
                { answer: '' }
            ];
            break;
        case 'true_false':
            newQuestion.answer = true; // 默认答案为正确
            break;
        case 'matching':
            newQuestion.leftItems = ['', '', ''];
            newQuestion.rightItems = ['', '', ''];
            newQuestion.answer = {0: 0, 1: 1, 2: 2}; // 默认一一对应
            break;
        case 'sort_question':
            newQuestion.items = ['', '', '', ''];
            newQuestion.answer = [0, 1, 2, 3]; // 正确顺序的索引数组
            break;
        case 'table_question':
            newQuestion.tableData = {
                headers: ['', ''],
                rows: [
                    ['', ''],
                    ['', ''],
                    ['', '']
                ]
            };
            newQuestion.answer = {
                '0,1': '', // 行索引,列索引: 答案
                '1,1': '',
                '2,1': ''
            };
            break;
        case 'short_answer':
            newQuestion.answer = ''; // 简答题的参考答案
            newQuestion.keywords = ['', '']; // 关键词列表
            newQuestion.keywordThreshold = 60; // 关键词匹配阈值（百分比）
            break;
    }

    examData.questions.push(newQuestion);
    renderQuestions();
    saveExamData();
    
    // 自动滚动到新题目
    setTimeout(() => {
        const newQuestionElement = document.querySelector(`[data-id="${questionId}"]`);
        if (newQuestionElement) {
            newQuestionElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, 100);
}
// 生成快速添加按钮区域的HTML
function generateQuickAddSection() {
    // 统计各题型数量
    const typeCounts = {
        'single_choice': 0,
        'multiple_choice': 0,
        'fill_blank': 0,
        'true_false': 0,
        'matching': 0,
        'sort_question': 0,
        'table_question': 0,
        'short_answer': 0
    };
    
    examData.questions.forEach(question => {
        if (typeCounts.hasOwnProperty(question.type)) {
            typeCounts[question.type]++;
        }
    });
    
    return `
        <div class="quick-add-section">
            <div class="quick-add-title">添加新题目</div>
            <div class="quick-add-buttons">
                <button class="quick-add-btn" onclick="createNewQuestion('single_choice')" data-type="single_choice">
                    <div class="quick-btn-icon">单</div>
                    <span>单选题</span>
                    ${typeCounts.single_choice > 0 ? `<div class="question-count">${typeCounts.single_choice}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('multiple_choice')" data-type="multiple_choice">
                    <div class="quick-btn-icon">多</div>
                    <span>多选题</span>
                    ${typeCounts.multiple_choice > 0 ? `<div class="question-count">${typeCounts.multiple_choice}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('fill_blank')" data-type="fill_blank">
                    <div class="quick-btn-icon">填</div>
                    <span>填空题</span>
                    ${typeCounts.fill_blank > 0 ? `<div class="question-count">${typeCounts.fill_blank}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('true_false')" data-type="true_false">
                    <div class="quick-btn-icon">判</div>
                    <span>判断题</span>
                    ${typeCounts.true_false > 0 ? `<div class="question-count">${typeCounts.true_false}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('matching')" data-type="matching">
                    <div class="quick-btn-icon">连</div>
                    <span>连线题</span>
                    ${typeCounts.matching > 0 ? `<div class="question-count">${typeCounts.matching}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('sort_question')" data-type="sort_question">
                    <div class="quick-btn-icon">排</div>
                    <span>排序题</span>
                    ${typeCounts.sort_question > 0 ? `<div class="question-count">${typeCounts.sort_question}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('table_question')" data-type="table_question">
                    <div class="quick-btn-icon">表</div>
                    <span>表格题</span>
                    ${typeCounts.table_question > 0 ? `<div class="question-count">${typeCounts.table_question}</div>` : ''}
                </button>
                <button class="quick-add-btn" onclick="createNewQuestion('short_answer')" data-type="short_answer">
                    <div class="quick-btn-icon">简</div>
                    <span>简答题</span>
                    ${typeCounts.short_answer > 0 ? `<div class="question-count">${typeCounts.short_answer}</div>` : ''}
                </button>
            </div>
        </div>
    `;
}

// 渲染所有题目
function renderQuestions() {
    console.log("开始渲染所有题目");
    const editArea = document.getElementById('editArea');
    syncExamAiSetting();
    
    // 先渲染试卷信息区域
    const examInfoHTML = `
        <div class="exam-info-edit" id="examInfoEdit">
            <input type="text" class="exam-title-edit" id="examTitleEdit" 
                value="${examData.title}" 
                onchange="updateExamTitle(this.value)"
                placeholder="请输入测验标题">
            <span class="exam-info-label">测验描述</span>
            <textarea class="exam-desc-edit" id="examDescEdit" 
                    onchange="updateExamDescription(this.value)"
                    placeholder="请输入测验描述">${examData.description}</textarea>
        </div>
    `;
    
    if (examData.questions.length === 0) {
        // 即使没有题目，也要显示快速添加按钮
        const quickAddHTML = `
            <div class="empty-message">暂无题目，请使用下方快捷按钮添加题目</div>
            ${generateQuickAddSection()}
        `;
        editArea.innerHTML = examInfoHTML + quickAddHTML;
        return;
    }
    
    let questionsHTML = examInfoHTML;
    
    examData.questions.forEach((question, index) => {
        const questionElement = document.createElement('div');
        questionElement.className = 'question-item';
        questionElement.dataset.id = question.id;
        
        // 左侧控制区域 - 整合所有功能
        const leftControlsHTML = `
            <div class="question-controls-new">
                <div class="question-number-badge">${index + 1}</div>
                <div class="question-type-badge">${getQuestionTypeText(question.type)}</div>
                
                <!-- 分值控制 -->
                <div class="score-control-compact">
                    <input type="number" class="score-input-compact" value="${question.score}" min="1" max="100"
                           onchange="updateQuestionScore('${question.id}', this.value)" title="分值">
                    <span class="score-label-compact">分</span>
                </div>
                
                <!-- 移动按钮 - 水平排列 -->
                <div class="move-controls-compact">
                    <button class="action-btn move-btn" onclick="moveQuestionUp('${question.id}')" ${index === 0 ? 'disabled' : ''} title="上移">
                        ⇧
                    </button>
                    <button class="action-btn move-btn" onclick="moveQuestionDown('${question.id}')" ${index === examData.questions.length - 1 ? 'disabled' : ''} title="下移">
                        ⇩
                    </button>
                </div>
                
                <!-- 操作按钮 - 水平排列 -->
                <div class="question-actions-compact">
                    <button class="action-btn duplicate-btn" onclick="duplicateQuestion('${question.id}')" title="复制题目">
                        ❏
                    </button>
                    <button class="action-btn delete-btn" onclick="deleteQuestion('${question.id}')" title="删除题目">
                        ✖
                    </button>
                </div>
            </div>
        `;
        
        // 右侧内容区域 - 移除悬浮工具栏
        let rightContentHTML = `
            <div class="question-content-new">
                <!-- 题目内容编辑器 -->
                <div class="question-editor-container">
                    <div class="rich-text-editor" id="editor-${question.id}"></div>
                </div>
                
                <!-- 题目选项/设置区域 -->
                <div class="question-options-container" id="options-container-${question.id}">
        `;
        
        // 根据题目类型渲染不同内容
        switch(question.type) {
            case 'single_choice':
                rightContentHTML += `
                    <div class="options-list">
                `;
                question.options.forEach((option, optIndex) => {
                    const isCorrect = optIndex === question.answer;
                    const optionData = parseOptionData(option);
                    rightContentHTML += `
                        <div class="option-item-new ${isCorrect ? 'correct-option' : ''}" id="option-${question.id}-${optIndex}">
                            <div class="option-control">
                                <input type="radio" name="correct_${question.id}" ${isCorrect ? 'checked' : ''}
                                       onchange="setQuestionCorrectAnswer('${question.id}', ${optIndex})" class="option-radio">
                                <span class="option-letter">${String.fromCharCode(65 + optIndex)}</span>
                            </div>
                            <div class="option-content-container">
                                <input type="text" class="option-input-new" value="${optionData.text}" 
                                       onchange="updateOptionText('${question.id}', ${optIndex}, this.value)"
                                       placeholder="选项内容">
                                ${optionData.image ? `
                                    <img src="${getImageUrl(optionData.image)}" alt="选项图片" class="option-image-preview">
                                ` : ''}
                            </div>
                            <div class="option-controls-right">
                                <button class="matching-image-btn" onclick="document.getElementById('option-image-input-${question.id}-${optIndex}').click()" title="上传图片">
                                    📷
                                </button>
                                <input type="file" id="option-image-input-${question.id}-${optIndex}" 
                                       style="display: none;" accept="image/*" 
                                       onchange="handleOptionImageUpload('${question.id}', ${optIndex}, this)">
                                ${optionData.image ? `
                                    <button class="matching-image-btn" onclick="removeOptionImage('${question.id}', ${optIndex})" title="删除图片">
                                        🗑️
                                    </button>
                                ` : ''}
                            </div>
                            <button class="option-delete-btn" onclick="deleteOption('${question.id}', ${optIndex})" 
                                    ${question.options.length <= 2 ? 'disabled' : ''}>
                                <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor">
                                    <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                                </svg>
                            </button>
                        </div>
                    `;
                });
                rightContentHTML += `
                    </div>
                    <button class="add-option-btn-new" onclick="addOption('${question.id}')">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
                            <path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/>
                        </svg>
                        添加选项
                    </button>
                `;
                break;
                
            case 'multiple_choice':
                rightContentHTML += `
                    <div class="options-list">
                `;
                question.options.forEach((option, optIndex) => {
                    const isCorrect = Array.isArray(question.answer) && question.answer.includes(optIndex);
                    const optionData = parseOptionData(option);
                    rightContentHTML += `
                        <div class="option-item-new ${isCorrect ? 'correct-option' : ''}" id="option-${question.id}-${optIndex}">
                            <div class="option-control">
                                <input type="checkbox" name="correct_${question.id}" ${isCorrect ? 'checked' : ''}
                                       onchange="setMultipleChoiceAnswer('${question.id}', ${optIndex}, this.checked)" class="option-checkbox">
                                <span class="option-letter">${String.fromCharCode(65 + optIndex)}</span>
                            </div>
                            <div class="option-content-container">
                                <input type="text" class="option-input-new" value="${optionData.text}" 
                                       onchange="updateOptionText('${question.id}', ${optIndex}, this.value)"
                                       placeholder="选项内容">
                                ${optionData.image ? `
                                    <img src="${getImageUrl(optionData.image)}" alt="选项图片" class="option-image-preview">
                                ` : ''}
                            </div>
                            <div class="option-controls-right">
                                <button class="matching-image-btn" onclick="document.getElementById('option-image-input-${question.id}-${optIndex}').click()" title="上传图片">
                                    📷
                                </button>
                                <input type="file" id="option-image-input-${question.id}-${optIndex}" 
                                       style="display: none;" accept="image/*" 
                                       onchange="handleOptionImageUpload('${question.id}', ${optIndex}, this)">
                                ${optionData.image ? `
                                    <button class="matching-image-btn" onclick="removeOptionImage('${question.id}', ${optIndex})" title="删除图片">
                                        🗑️
                                    </button>
                                ` : ''}
                            </div>
                            <button class="option-delete-btn" onclick="deleteOption('${question.id}', ${optIndex})" 
                                    ${question.options.length <= 2 ? 'disabled' : ''}>
                                <svg width="14" height="14" viewBox="0 0 24 24" fill="currentColor">
                                    <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                                </svg>
                            </button>
                        </div>
                    `;
                });
                rightContentHTML += `
                    </div>
                    <button class="add-option-btn-new" onclick="addOption('${question.id}')">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
                            <path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/>
                        </svg>
                        添加选项
                    </button>
                `;
                break;
                
            case 'fill_blank':
                rightContentHTML += `
                    <div class="fill-blank-settings">
                        <div class="fill-blank-answers-grid" id="fill-blank-answers-${question.id}">
                `;
                (question.blanks || []).forEach((blank, index) => {
                    rightContentHTML += `
                        <div class="blank-answer-card">
                            <div class="blank-header">
                                <span class="blank-number">${index + 1}</span>
                                <button class="blank-delete-btn" onclick="deleteFillBlank('${question.id}', ${index})"
                                        ${(question.blanks || []).length <= 1 ? 'disabled' : ''}>
                                    ✖
                                </button>
                            </div>
                            <input type="text" class="blank-answer-input" value="${blank.answer}"
                                   onchange="updateFillBlankAnswer('${question.id}', ${index}, this.value)"
                                   placeholder="标准答案">
                        </div>
                    `;
                });
                rightContentHTML += `
                        </div>
                        <button class="refresh-blanks-btn" onclick="refreshFillBlanks('${question.id}')">
                            <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
                                <path d="M17.65 6.35C16.2 4.9 14.21 4 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08c-.82 2.33-3.04 4-5.65 4-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z"/>
                            </svg>
                            从题目内容刷新空白
                        </button>
                        <div class="fill-blank-tip">
                            💡 在题目内容中使用 ___ 标记空白处
                        </div>
                    </div>
                `;
                break;
                
            case 'true_false':
                rightContentHTML += `
                    <div class="true-false-options">
                        <div class="tf-option ${question.answer ? 'selected' : ''}" onclick="setQuestionCorrectAnswer('${question.id}', true)">
                            <div class="tf-radio ${question.answer ? 'checked' : ''}"></div>
                            <span class="tf-label">正确</span>
                        </div>
                        <div class="tf-option ${!question.answer ? 'selected' : ''}" onclick="setQuestionCorrectAnswer('${question.id}', false)">
                            <div class="tf-radio ${!question.answer ? 'checked' : ''}"></div>
                            <span class="tf-label">错误</span>
                        </div>
                    </div>
                `;
                break;
                
            case 'matching':
                rightContentHTML += `
                    <div class="form-group">
                        <div class="form-label">连线项目设置</div>
                        <div class="matching-edit-container">
                            <div class="matching-edit-column">
                                <div class="matching-edit-title">左侧项目</div>
                `;
                question.leftItems.forEach((item, index) => {
                    const itemData = parseMatchingItemData(item);
                    rightContentHTML += `
                        <div class="matching-item-input-container">
                            <input type="text" class="matching-item-text-input" 
                                   value="${itemData.text}" 
                                   onchange="updateMatchingItemText('${question.id}', 'left', ${index}, this.value)"
                                   placeholder="项目名称">
                            ${itemData.image ? `
                                <img src="${getImageUrl(itemData.image)}" alt="预览" class="matching-item-image-preview">
                            ` : ''}
                            <div class="matching-item-controls">
                                <button class="matching-image-btn" onclick="document.getElementById('image-input-left-${question.id}-${index}').click()" title="上传图片">
                                    📷
                                </button>
                                <input type="file" id="image-input-left-${question.id}-${index}" 
                                       style="display: none;" accept="image/*" 
                                       onchange="handleImageUpload('${question.id}', 'left', ${index}, this)">
                                ${itemData.image ? `
                                    <button class="matching-image-btn" onclick="removeMatchingItemImage('${question.id}', 'left', ${index})" title="删除图片">
                                        🗑️
                                    </button>
                                ` : ''}
                                <button class="delete-matching-item-btn" onclick="deleteMatchingItem('${question.id}', 'left', ${index})" ${question.leftItems.length <= 2 ? 'disabled' : ''} title="删除项目">✖</button>
                            </div>
                        </div>
                    `;
                });
                rightContentHTML += `
                                <button class="add-option-btn" onclick="addMatchingItem('${question.id}', 'left')">
                                    <span>+ 添加左侧项目</span>
                                </button>
                            </div>
                            <div class="matching-edit-column">
                                <div class="matching-edit-title">右侧项目</div>
                `;
                question.rightItems.forEach((item, index) => {
                    const itemData = parseMatchingItemData(item);
                    rightContentHTML += `
                        <div class="matching-item-input-container">
                            <input type="text" class="matching-item-text-input" 
                                   value="${itemData.text}" 
                                   onchange="updateMatchingItemText('${question.id}', 'right', ${index}, this.value)"
                                   placeholder="项目名称">
                            ${itemData.image ? `
                                <img src="${getImageUrl(itemData.image)}" alt="预览" class="matching-item-image-preview">
                            ` : ''}
                            <div class="matching-item-controls">
                                <button class="matching-image-btn" onclick="document.getElementById('image-input-right-${question.id}-${index}').click()" title="上传图片">
                                    📷
                                </button>
                                <input type="file" id="image-input-right-${question.id}-${index}" 
                                       style="display: none;" accept="image/*" 
                                       onchange="handleImageUpload('${question.id}', 'right', ${index}, this)">
                                ${itemData.image ? `
                                    <button class="matching-image-btn" onclick="removeMatchingItemImage('${question.id}', 'right', ${index})" title="删除图片">
                                        🗑️
                                    </button>
                                ` : ''}
                                <button class="delete-matching-item-btn" onclick="deleteMatchingItem('${question.id}', 'right', ${index})" ${question.rightItems.length <= 2 ? 'disabled' : ''} title="删除项目">✖</button>
                            </div>
                        </div>
                    `;
                });
                rightContentHTML += `
                                <button class="add-option-btn" onclick="addMatchingItem('${question.id}', 'right')">
                                    <span>+ 添加右侧项目</span>
                                </button>
                            </div>
                        </div>
                    </div>
                `;
                break;
                
            case 'sort_question':
                rightContentHTML += `
                    <div class="form-group">
                        <div class="form-label">排序项目设置</div>
                        <div class="sort-items-container" id="sort-items-${question.id}">
                `;
                question.items.forEach((item, index) => {
                    rightContentHTML += `
                        <div class="sort-item-input-container">
                            <span class="sort-item-index">${index + 1}</span>
                            <input type="text" class="sort-item-input" value="${item}" 
                                   onchange="updateSortItem('${question.id}', ${index}, this.value)"
                                   placeholder="排序项目内容">
                            <button class="delete-sort-item-btn" onclick="deleteSortItem('${question.id}', ${index})" 
                                    ${question.items.length <= 2 ? 'disabled' : ''} title="删除项目">✖</button>
                        </div>
                    `;
                });
                rightContentHTML += `
                        </div>
                        <button class="add-option-btn" onclick="addSortItem('${question.id}')">
                            <span>+ 添加排序项目</span>
                        </button>
                        <div class="form-group" style="margin-top: 15px;">
                            <div class="form-label">正确顺序预览</div>
                            <div class="sort-preview" id="sort-preview-${question.id}">
                                ${question.items.map((item, idx) => `<span class="sort-preview-item">${idx + 1}. ${item}</span>`).join('')}
                            </div>
                        </div>
                    </div>
                `;
                break;
                
            case 'table_question':
                rightContentHTML += `
                    <div class="form-group">
                        <div class="form-label">表格设置</div>
                        <div class="table-editor-container">
                            <table class="table-editor" id="table-editor-${question.id}">
                                <thead>
                                    <tr>
                `;
                question.tableData.headers.forEach((header, index) => {
                    rightContentHTML += `
                        <th>
                            <div class="table-header-container">
                                <input type="text" class="table-header-input" value="${header}"
                                       onchange="updateTableHeader('${question.id}', ${index}, this.value)"
                                       placeholder="列标题">
                                ${index > 0 ? `
                                    <button class="delete-table-col-btn" onclick="deleteTableColumn('${question.id}', ${index})" title="删除列">
                                        <svg width="12" height="12" viewBox="0 0 24 24" fill="currentColor">
                                            <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                                        </svg>
                                    </button>
                                ` : ''}
                            </div>
                        </th>
                    `;
                });
                rightContentHTML += `
                                        <th class="table-actions">
                                            <button class="add-table-col-btn" onclick="addTableColumn('${question.id}')">+列</button>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                `;
                question.tableData.rows.forEach((row, rowIndex) => {
                    rightContentHTML += `<tr>`;
                    row.forEach((cell, colIndex) => {
                        const isAnswerCell = question.answer[`${rowIndex},${colIndex}`] !== undefined;
                        rightContentHTML += `
                            <td>
                                ${colIndex === 0 ? `
                                    <!-- 第一列：行标题，不能设为答题区 -->
                                    <input type="text" class="table-cell-input" 
                                           value="${cell}"
                                           onchange="updateTableCell('${question.id}', ${rowIndex}, ${colIndex}, this.value)"
                                           placeholder="行标题">
                                ` : `
                                    <!-- 其他列：可设为答题区的单元格 -->
                                    <div class="table-cell-container ${isAnswerCell ? 'answer-cell-container' : ''}">
                                        ${isAnswerCell ? `
                                            <!-- 答题区域：只显示标准答案设置 -->
                                            <input type="text" class="table-answer-input" 
                                                   value="${question.answer[`${rowIndex},${colIndex}`] || ''}"
                                                   onchange="updateTableAnswer('${question.id}', ${rowIndex}, ${colIndex}, this.value)"
                                                   placeholder="请输入标准答案">
                                        ` : `
                                            <!-- 普通内容区域 -->
                                            <input type="text" class="table-cell-input" 
                                                   value="${cell}"
                                                   onchange="updateTableCell('${question.id}', ${rowIndex}, ${colIndex}, this.value)"
                                                   placeholder="内容">
                                        `}
                                        
                                        <div class="table-cell-controls">
                                            <label class="table-answer-checkbox">
                                                <input type="checkbox" ${isAnswerCell ? 'checked' : ''}
                                                       onchange="toggleTableAnswer('${question.id}', ${rowIndex}, ${colIndex}, this.checked)">
                                                <span>${isAnswerCell ? '取消答题区' : '设为答题区'}</span>
                                            </label>
                                        </div>
                                    </div>
                                `}
                            </td>
                        `;
                    });
                    rightContentHTML += `
                        <td class="table-actions">
                            <button class="delete-table-row-btn" onclick="deleteTableRow('${question.id}', ${rowIndex})"
                                    ${question.tableData.rows.length <= 1 ? 'disabled' : ''}>✖</button>
                        </td>
                    </tr>`;
                });
                rightContentHTML += `
                                </tbody>
                            </table>
                            <button class="add-option-btn" onclick="addTableRow('${question.id}')">
                                <span>+ 添加行</span>
                            </button>
                        </div>
                    </div>
                `;
                break;
                
            case 'short_answer':
                rightContentHTML += `
                    <div class="short-answer-settings">
                        <div class="form-group">
                            <div class="form-label">参考答案</div>
                            <textarea class="short-answer-input" 
                                      onchange="updateShortAnswer('${question.id}', this.value)"
                                      placeholder="请输入参考答案..."
                                      rows="4">${question.answer || ''}</textarea>
                        </div>
                        <div class="form-group">
                            <div class="form-label">关键词设置 <span class="form-hint">（用于自动评分，用空格分隔）</span></div>
                            <input type="text" class="keywords-input" 
                                   value="${(question.keywords || []).join(' ')}"
                                   onchange="updateShortAnswerKeywords('${question.id}', this.value)"
                                   placeholder="例如：计算机 网络 协议 TCP IP">
                        </div>
                        <div class="form-group">
                            <div class="form-label">匹配阈值 <span class="form-hint">（包含多少比例关键词算正确）</span></div>
                            <div class="threshold-control">
                                <input type="range" class="threshold-slider" 
                                       min="30" max="100" step="10"
                                       value="${question.keywordThreshold || 60}"
                                       onchange="updateKeywordThreshold('${question.id}', this.value)"
                                       oninput="updateThresholdDisplay('${question.id}', this.value)">
                                <span class="threshold-value" id="threshold-${question.id}">${question.keywordThreshold || 60}%</span>
                            </div>
                        </div>
                        <div class="auto-score-preview">
                            <div class="form-label">自动评分预览</div>
                            <div class="score-example">
                                <small>学生答案包含 <strong>${question.keywordThreshold || 60}%</strong> 以上关键词将获得满分</small>
                            </div>
                        </div>
                    </div>
                `;
                break;
        }
        
        rightContentHTML += `</div>`;
        
        questionElement.innerHTML = leftControlsHTML + rightContentHTML;
        questionsHTML += questionElement.outerHTML;
    });
    
    // 添加快速添加按钮
    questionsHTML += generateQuickAddSection();
    
    editArea.innerHTML = questionsHTML;
    
    // 初始化所有富文本编辑器
    initAllRichTextEditors();
    
}

// 解析连线题项目数据已移至文件末尾

// 序列化连线题项目数据
function serializeMatchingItemData(text, image) {
    if (image) {
        return JSON.stringify({ text: text || '', image: image });
    }
    return text || '';
}

// 解析单选题选项数据已移至文件末尾

// 序列化单选题选项数据
function serializeOptionData(text, image) {
    if (image) {
        return JSON.stringify({ text: text || '', image: image });
    }
    return text || '';
}

// 初始化所有富文本编辑器 - Summernote 版本
function initAllRichTextEditors() {
    examData.questions.forEach(question => {
        initSummernoteEditor(question.id, question.title);
    });
}

// 初始化 Summernote 富文本编辑器
function initSummernoteEditor(questionId, content) {
    const editorId = `editor-${questionId}`;
    const editorElement = document.getElementById(editorId);
    
    if (!editorElement || editorElement.__summernoteInitialized) return;
    
    // 清空原有内容并设置类名
    editorElement.innerHTML = '';
    editorElement.className = 'summernote-editor-container';
    
    // 创建编辑器内容区域
    const editorContent = document.createElement('div');
    editorContent.id = `summernote-content-${questionId}`;
    editorContent.innerHTML = content ;
    editorElement.appendChild(editorContent);
    
    // 标记已初始化
    editorElement.__summernoteInitialized = true;
    
    // 使用 jQuery 初始化 Summernote
    $(`#summernote-content-${questionId}`).summernote({
        // 基本配置
        placeholder: '点击此处编辑题目内容...',
        height: 120,
        minHeight: 80,
        maxHeight: 300,
        
        // 语言设置 - 中文（语言包会自动处理所有提示文本）
        lang: 'zh-CN',
        
        // 禁用默认的图片处理以避免blob URL
        disableDragAndDrop: false,
        
        // 工具栏配置 - 适合题目编辑的精简版
        toolbar: [
            ['style', ['bold', 'italic', 'clear']],
            ['insert', ['customPicture']], // 使用自定义图片按钮
            ['code', ['codeblock']], // 插入代码块
            ['misc', ['pastetext']], // 粘贴纯文本 + 代码视图
            ['misc', ['blank']], // 粘贴纯文本 + 代码视图
            ['view', ['undo', 'redo']]
        ],
        
        // 字体大小选项
        fontSizes: ['10', '11', '12', '14', '16', '18', '20', '24'],
        
        // 自定义按钮
        buttons: {
            customPicture: function(context) {
                var ui = $.summernote.ui;
                var button = ui.button({
                    contents: '<i class="note-icon-picture"></i>',
                    tooltip: '插入图片',
                    click: function() {
                        // 创建隐藏的文件输入框
                        var fileInput = document.createElement('input');
                        fileInput.type = 'file';
                        fileInput.accept = 'image/*';
                        fileInput.style.display = 'none';
                        
                        fileInput.onchange = function() {
                            var file = this.files[0];
                            if (file) {
                                // 直接上传并插入图片，不依赖questionId
                                uploadImage(file,
                                    function(response) {
                                        // 上传成功 - 直接插入图片，使用完整路径显示
                                        const imageHtml = `<img src="${getImageUrl(response)}" >`;
                                        context.invoke('editor.pasteHTML', imageHtml);
                                    },
                                    function(error) {
                                        // 上传失败 - 显示错误信息
                                        alert('图片上传失败：' + error);
                                    }
                                );
                            }
                        };
                        
                        // 触发文件选择
                        document.body.appendChild(fileInput);
                        fileInput.click();
                        document.body.removeChild(fileInput);
                    }
                });
                return button.render();
            },
            blank: function(context) {
                var ui = $.summernote.ui;
                var button = ui.button({
                    contents: '<i class="note-icon-pencil"></i>',
                    tooltip: '插入填空符',
                    click: function() {
                        // 确保编辑器有焦点
                        context.invoke('editor.focus');
                        
                        // 在光标位置插入填空符
                        setTimeout(function() {
                            try {
                                context.invoke('editor.insertText', '___');
                            } catch (e) {
                                // 备用方案
                                context.invoke('editor.pasteHTML', '___');
                            }
                        }, 10);
                    }
                });
                return button.render();
            },
            codeblock: function(context) {
                var ui = $.summernote.ui;
                var button = ui.button({
                    contents: '<i class="note-icon-code"></i>',
                    tooltip: '插入代码块',
                    click: function() {
                        // 创建一个更友好的对话框
                        var modal = $(`
                            <div class="modal fade" tabindex="-1">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">插入代码块</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="mb-3">
                                                <label class="form-label">编程语言（可选）：</label>
                                                <select class="form-select" id="codeLanguage">
                                                    <option value="">无</option>
                                                    <option value="javascript">JavaScript</option>
                                                    <option value="python">Python</option>
                                                    <option value="java">Java</option>
                                                    <option value="cpp">C++</option>
                                                    <option value="html">HTML</option>
                                                    <option value="css">CSS</option>
                                                    <option value="sql">SQL</option>
                                                </select>
                                            </div>
                                            <div class="mb-3">
                                                <label class="form-label">代码内容：</label>
                                                <textarea class="form-control" id="codeContent" rows="8" placeholder="请输入代码内容..."></textarea>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">取消</button>
                                            <button type="button" class="btn btn-primary" id="insertCodeBtn">插入代码</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `);
                        
                        $('body').append(modal);
                        modal.modal('show');
                        
                        // 处理插入按钮点击
                        modal.find('#insertCodeBtn').on('click', function() {
                            var language = modal.find('#codeLanguage').val();
                            var code = modal.find('#codeContent').val();
                            
                            if (code.trim()) {
                                var escapedCode = code.replace(/&/g, '&amp;')
                                                     .replace(/</g, '&lt;')
                                                     .replace(/>/g, '&gt;')
                                                     .replace(/"/g, '&quot;')
                                                     .replace(/'/g, '&#39;');
                                
                                var codeHtml;
                                if (language) {
                                    codeHtml = '<pre class="code-block" data-language="' + language + '"><code class="language-' + language + '">' + escapedCode + '</code></pre>';
                                } else {
                                    codeHtml = '<pre class="code-block"><code>' + escapedCode + '</code></pre>';
                                }
                                
                                // 直接在编辑器内容末尾追加代码块，不考虑光标位置
                                var currentContent = context.invoke('code');
                                var newContent = currentContent + '\n' + codeHtml + '\n';
                                context.invoke('code', newContent);
                                modal.modal('hide');
                            } else {
                                alert('请输入代码内容！');
                            }
                        });
                        
                        // 模态框关闭时移除
                        modal.on('hidden.bs.modal', function() {
                            modal.remove();
                        });
                    }
                });
                return button.render();
            },
            pastetext: function(context) {
                var ui = $.summernote.ui;
                var button = ui.button({
                    contents: '<i class="note-icon-magic"></i>',
                    tooltip: '粘贴纯文本',
                    click: function() {
                        // 保存当前选择范围和编辑器状态
                        var savedSelection = null;
                        var editorElement = context.layoutInfo.editable[0];
                        
                        try {
                            // 获取当前选择
                            var selection = window.getSelection();
                            if (selection.rangeCount > 0) {
                                savedSelection = selection.getRangeAt(0).cloneRange();
                            }
                        } catch (e) {
                            console.log('无法保存选择范围:', e);
                        }
                        
                        // 创建粘贴纯文本对话框
                        var modal = $(`
                            <div class="modal fade" tabindex="-1">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">粘贴纯文本</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="mb-3">
                                                <label class="form-label">请粘贴文本内容（将自动去除所有格式）：</label>
                                                <textarea class="form-control" id="pasteContent" rows="3" placeholder="在此粘贴文本..."></textarea>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">取消</button>
                                            <button type="button" class="btn btn-primary" id="insertTextBtn">插入文本</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `);
                        
                        $('body').append(modal);
                        modal.modal('show');
                        
                        // 自动聚焦到文本框
                        modal.on('shown.bs.modal', function() {
                            modal.find('#pasteContent').focus();
                        });
                        
                        // 处理插入按钮点击
                        modal.find('#insertTextBtn').on('click', function() {
                            var text = modal.find('#pasteContent').val();
                            
                            if (text.trim()) {
                                modal.modal('hide');
                                
                                // 等待模态框完全关闭后再操作
                                setTimeout(function() {
                                    // 恢复编辑器焦点
                                    editorElement.focus();
                                    
                                    // 恢复选择范围
                                    if (savedSelection) {
                                        try {
                                            var selection = window.getSelection();
                                            selection.removeAllRanges();
                                            selection.addRange(savedSelection);
                                        } catch (e) {
                                            console.log('无法恢复选择范围:', e);
                                        }
                                    }
                                    
                                    // 再次确保焦点并插入文本
                                    setTimeout(function() {
                                        try {
                                            // 清理文本内容
                                            var cleanText = text.replace(/<[^>]*>/g, '');
                                            
                                            // 使用原生的插入方法
                                            if (document.execCommand && document.execCommand('insertText', false, cleanText)) {
                                                // 成功使用 execCommand
                                                console.log('使用 execCommand 插入文本');
                                            } else {
                                                // 备用方案：使用 Summernote API
                                                context.invoke('editor.insertText', cleanText);
                                            }
                                        } catch (e) {
                                            console.log('插入文本失败，使用备用方案:', e);
                                            // 最后的备用方案
                                            try {
                                                var plainText = text.replace(/<[^>]*>/g, '').replace(/\n/g, '<br>');
                                                context.invoke('editor.pasteHTML', plainText);
                                            } catch (e2) {
                                                alert('插入文本失败，请重试');
                                            }
                                        }
                                    }, 100);
                                }, 200);
                            } else {
                                alert('请输入文本内容！');
                            }
                        });
                        
                        // 模态框关闭时移除
                        modal.on('hidden.bs.modal', function() {
                            modal.remove();
                        });
                    }
                });
                return button.render();
            }
        },
        
        // 回调函数
        callbacks: {
            onImageUpload: function(files) {
                // 处理图片上传 - 使用服务器上传
                for (let i = 0; i < files.length; i++) {
                    uploadImageToServer(files[i], questionId);
                }
                // 阻止默认的blob URL处理
                return false;
            },
            onPaste: function(e) {
                // 阻止粘贴时可能产生的blob URL
                var clipboardData = e.originalEvent.clipboardData;
                if (clipboardData && clipboardData.items) {
                    for (var i = 0; i < clipboardData.items.length; i++) {
                        var item = clipboardData.items[i];
                        if (item.type.indexOf('image') !== -1) {
                            // 阻止默认的图片粘贴处理，避免blob URL
                            e.preventDefault();
                            var file = item.getAsFile();
                            if (file) {
                                uploadImageToServer(file, questionId);
                            }
                            return false;
                        }
                    }
                }
            },
            onFocus: function() {
                editorElement.classList.add('focused');
            },
            onBlur: function() {
                setTimeout(() => {
                    if (!editorElement.contains(document.activeElement)) {
                        editorElement.classList.remove('focused');
                    }
                }, 100);
            },
            onChange: function(contents) {
                // 更新题目内容
                updateQuestionTitle(questionId, contents);
                
                // 为编辑器中的代码块应用样式
                setTimeout(() => {
                    const editorCodeBlocks = editorElement.querySelectorAll('pre.code-block code');
                    editorCodeBlocks.forEach(block => {
                        if (typeof hljs !== 'undefined' && !block.classList.contains('hljs')) {
                            // 检查代码块是否已经包含HTML标签（已经高亮过）
                            if (block.innerHTML.includes('<span class="hljs-')) {
                                return;
                            }
                            
                            // 检查是否已经高亮过
                            if (block.dataset.highlighted) {
                                delete block.dataset.highlighted;
                            }
                            
                            // 确保代码内容是纯文本，避免HTML注入
                            const codeText = block.textContent || block.innerText || '';
                            if (codeText.trim()) {
                                block.textContent = codeText;
                                hljs.highlightElement(block);
                            }
                        }
                    });
                }, 100);
            }
        }
    });
    
    // 保存编辑器实例引用
    editorElement.__summernoteInstance = $(`#summernote-content-${questionId}`);
}

// 编辑器工具函数 - 轻量级版本
function addSimpleEditorKeyboardShortcuts() {
    // 为轻量级编辑器添加快捷键支持
    document.addEventListener('keydown', function(e) {
        const activeEditor = document.querySelector('.simple-rich-editor.focused .simple-editor');
        if (!activeEditor) return;
        
        // Ctrl+B 加粗
        if (e.ctrlKey && e.key === 'b') {
            e.preventDefault();
            document.execCommand('bold', false, null);
        }
        
        // Ctrl+I 斜体
        if (e.ctrlKey && e.key === 'i') {
            e.preventDefault();
            document.execCommand('italic', false, null);
        }
        
        // Ctrl+U 下划线
        if (e.ctrlKey && e.key === 'u') {
            e.preventDefault();
            document.execCommand('underline', false, null);
        }
    });
}

// 新增一个专门更新拖拽指示器状态的函数
// 获取题目类型文本
function getQuestionTypeText(type) {
    const typeMap = {
        'single_choice': '单选题',
        'multiple_choice': '多选题',
        'fill_blank': '填空题',
        'true_false': '判断题',
        'matching': '连线题',
        'sort_question': '排序题',
        'table_question': '表格题',
        'short_answer': '简答题'
    };
    return typeMap[type] || '未知题型';
}

// 复制题目
function duplicateQuestion(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const newQuestion = JSON.parse(JSON.stringify(question)); // 深拷贝
        newQuestion.id = 'q_' + Date.now();
        
        // 在原题目后面插入
        const index = examData.questions.findIndex(q => q.id === questionId);
        examData.questions.splice(index + 1, 0, newQuestion);
        
        saveExamData();
        renderQuestions();
        
        // 滚动到新题目
        setTimeout(() => {
            const newQuestionElement = document.querySelector(`[data-id="${newQuestion.id}"]`);
            if (newQuestionElement) {
                newQuestionElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        }, 100);
    }
}

// 切换必答状态
function toggleRequired(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.required = !question.required;
        saveExamData();
        renderQuestions();
    }
}

// 移动题目向上
function moveQuestionUp(questionId) {
    const index = examData.questions.findIndex(q => q.id === questionId);
    if (index > 0) {
        // 交换位置
        [examData.questions[index - 1], examData.questions[index]] = [examData.questions[index], examData.questions[index - 1]];
        saveExamData();
        renderQuestions();
    }
}

// 移动题目向下
function moveQuestionDown(questionId) {
    const index = examData.questions.findIndex(q => q.id === questionId);
    if (index < examData.questions.length - 1) {
        // 交换位置
        [examData.questions[index], examData.questions[index + 1]] = [examData.questions[index + 1], examData.questions[index]];
        saveExamData();
        renderQuestions();
    }
}

// 更新题目标题
function updateQuestionTitle(questionId, newTitle) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.title = newTitle;
        saveExamData();
    }
}

// 更新题目分值
function updateQuestionScore(questionId, newScore) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.score = parseInt(newScore) || 1;
        saveExamData();
    }
}

// 更新题目选项
function updateQuestionOption(questionId, optionIndex, newValue) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.options) {
        question.options[optionIndex] = newValue;
        saveExamData();
    }
}

// 更新单选题选项文本内容
function updateOptionText(questionId, optionIndex, newText) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.options) {
        const currentOption = question.options[optionIndex];
        const optionData = parseOptionData(currentOption);
        
        // 更新文本，保留图片
        const newOptionData = serializeOptionData(newText, optionData.image);
        question.options[optionIndex] = newOptionData;
        saveExamData();
    }
}

// 处理单选题选项图片上传
function handleOptionImageUpload(questionId, optionIndex, fileInput) {
    const file = fileInput.files[0];
    if (!file) return;
    
    // 检查文件类型
    if (!file.type.startsWith('image/')) {
        alert('请选择图片文件！');
        return;
    }
    
    // 检查文件大小（限制为2MB）
    if (file.size > 2 * 1024 * 1024) {
        alert('图片大小不能超过2MB！');
        return;
    }
    
    // 使用服务器上传，不再使用base64
    uploadImage(file,
        function(response) {
            // 上传成功 - response是图片文件名
            const question = examData.questions.find(q => q.id === questionId);
            if (question && question.options) {
                const currentOption = question.options[optionIndex];
                const optionData = parseOptionData(currentOption);
                
                // 更新选项数据（保留文本，添加图片文件名）
                const newOptionData = serializeOptionData(optionData.text, response);
                question.options[optionIndex] = newOptionData;
                
                saveExamData();
                renderQuestions();
            }
        },
        function(error) {
            alert('图片上传失败：' + error);
        }
    );
    
    // 重置文件输入
    fileInput.value = '';
}

// 删除单选题选项的图片
function removeOptionImage(questionId, optionIndex) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.options) {
        const currentOption = question.options[optionIndex];
        const optionData = parseOptionData(currentOption);
        
        // 移除图片，保留文本
        const newOptionData = serializeOptionData(optionData.text, null);
        question.options[optionIndex] = newOptionData;
        
        saveExamData();
        renderQuestions();
    }
}

// 设置题目正确答案
function setQuestionCorrectAnswer(questionId, answer) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.answer = answer;
        saveExamData();
        renderQuestions(); // 重新渲染以更新正确选项样式
    }
}

// 设置多选题正确答案
function setMultipleChoiceAnswer(questionId, optionIndex, isChecked) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        // 确保答案是数组格式
        if (!Array.isArray(question.answer)) {
            question.answer = [];
        }
        
        if (isChecked) {
            // 添加到答案数组
            if (!question.answer.includes(optionIndex)) {
                question.answer.push(optionIndex);
            }
        } else {
            // 从答案数组中移除
            const index = question.answer.indexOf(optionIndex);
            if (index > -1) {
                question.answer.splice(index, 1);
            }
        }
        
        // 验证至少有一个正确答案
        if (question.answer.length === 0) {
            alert('多选题至少需要选择一个正确答案！');
            question.answer = [0]; // 默认选择第一个选项
        }
        
        saveExamData();
        renderQuestions(); // 重新渲染以更新正确选项样式
    }
}

// 更新填空题答案
function updateFillBlankAnswer(questionId, blankIndex, newAnswer) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.blanks && question.blanks[blankIndex]) {
        question.blanks[blankIndex].answer = newAnswer;
        saveExamData();
    }
}

// 更新简答题答案
function updateShortAnswer(questionId, newAnswer) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.answer = newAnswer;
        saveExamData();
    }
}

// 更新简答题关键词
function updateShortAnswerKeywords(questionId, keywordsStr) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        // 将空格分隔的字符串转换为数组，并去除空白
        question.keywords = keywordsStr.split(/\s+/).map(k => k.trim()).filter(k => k.length > 0);
        saveExamData();
    }
}

// 更新关键词匹配阈值
function updateKeywordThreshold(questionId, threshold) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.keywordThreshold = parseInt(threshold);
        saveExamData();
    }
}

// 更新阈值显示
function updateThresholdDisplay(questionId, value) {
    const display = document.getElementById(`threshold-${questionId}`);
    if (display) {
        display.textContent = value + '%';
    }
}

// 简答题自动评分函数
function scoreShortAnswer(studentAnswer, question) {
    if (!question.keywords || question.keywords.length === 0) {
        return {
            score: 0,
            maxScore: question.score,
            feedback: '未设置关键词，无法自动评分'
        };
    }
    
    const answer = studentAnswer.toLowerCase();
    const keywords = question.keywords.map(k => k.toLowerCase());
    const threshold = question.keywordThreshold || 60;
    
    // 计算匹配的关键词数量
    let matchedCount = 0;
    const matchedKeywords = [];
    
    keywords.forEach(keyword => {
        if (answer.includes(keyword)) {
            matchedCount++;
            matchedKeywords.push(keyword);
        }
    });
    
    // 计算匹配百分比
    const matchPercentage = (matchedCount / keywords.length) * 100;
    
    // 根据阈值计算得分
    let score = 0;
    if (matchPercentage >= threshold) {
        score = question.score; // 满分
    } else if (matchPercentage >= threshold * 0.5) {
        score = Math.round(question.score * 0.7); // 70%分数
    } else if (matchPercentage > 0) {
        score = Math.round(question.score * 0.3); // 30%分数
    }
    
    return {
        score: score,
        maxScore: question.score,
        matchPercentage: Math.round(matchPercentage),
        matchedKeywords: matchedKeywords,
        totalKeywords: keywords.length,
        feedback: `匹配了 ${matchedCount}/${keywords.length} 个关键词 (${Math.round(matchPercentage)}%)`
    };
}

// 修改现有的更新连线题项目函数
function updateMatchingItem(questionId, side, index, newValue) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        if (side === 'left') {
            question.leftItems[index] = newValue;
        } else {
            question.rightItems[index] = newValue;
        }
        saveExamData();
    }
}

// 添加选项（单选题）
function addOption(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.options) {
        question.options.push(`选项${String.fromCharCode(65 + question.options.length)}`);
        saveExamData();
        renderQuestions();
    }
}

// 删除选项（单选题）
function deleteOption(questionId, optionIndex) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.options && question.options.length > 2) {
        question.options.splice(optionIndex, 1);
        // 如果删除的是正确答案，重置为第一个选项
        if (question.answer === optionIndex) {
            question.answer = 0;
        } else if (question.answer > optionIndex) {
            question.answer--;
        }
        saveExamData();
        renderQuestions();
    } else {
        alert('至少需要保留2个选项');
    }
}

// 添加连线题项目
function addMatchingItem(questionId, side) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        if (side === 'left') {
            question.leftItems.push(`项目${question.leftItems.length + 1}`);
        } else {
            question.rightItems.push(`项目${question.rightItems.length + 1}`);
        }
        
        // 重新设置一一对应的答案
        updateMatchingAnswersToDefault(question);
        saveExamData();
        renderQuestions();
    }
}

// 更新连线题答案为默认一一对应
function updateMatchingAnswersToDefault(question) {
    if (!question.answer) question.answer = {};
    
    // 确保左右项目数量一致，以较少的为准
    const minLength = Math.min(question.leftItems.length, question.rightItems.length);
    
    // 设置一一对应的答案
    for (let i = 0; i < minLength; i++) {
        question.answer[i] = i;
    }
    
    // 清除多余的答案
    Object.keys(question.answer).forEach(key => {
        const index = parseInt(key);
        if (index >= minLength) {
            delete question.answer[key];
        }
    });
}

// 删除连线题项目
function deleteMatchingItem(questionId, side, index) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        if (side === 'left') {
            if (question.leftItems.length <= 2) {
                alert('至少需要保留2个左侧项目');
                return;
            }
            question.leftItems.splice(index, 1);
        } else {
            if (question.rightItems.length <= 2) {
                alert('至少需要保留2个右侧项目');
                return;
            }
            question.rightItems.splice(index, 1);
        }
        
        // 重新设置一一对应的答案
        updateMatchingAnswersToDefault(question);
        saveExamData();
        renderQuestions();
    }
}

// 在删除题目时也要确保更新指示器状态
function deleteQuestion(questionId) {
    if (confirm('确定要删除这个题目吗？')) {
        examData.questions = examData.questions.filter(q => q.id !== questionId);
        renderQuestions(); // 这会自动更新指示器状态
        saveExamData();
    }
}

// 清空所有试题
function clearAllQuestions() {
    if (examData.questions.length === 0) {
        alert('当前试卷中没有题目！');
        return;
    }
    
    const confirmMessage = `确定要清空所有试题吗？\n\n当前试卷：${examData.title}\n题目数量：${examData.questions.length}\n总分：${examData.questions.reduce((sum, q) => sum + q.score, 0)}\n\n此操作不可撤销！`;
    
    if (confirm(confirmMessage)) {
        examData.questions = [];
        saveExamData();
        renderQuestions();
        alert('所有试题已清空！');
    }
}


// 保存试卷为JSON文件
function saveExamAsJson() {
    if (examData.questions.length === 0) {
        alert('请先添加题目再保存试卷！');
        return;
    }
    // 创建JSON字符串
    const jsonString = JSON.stringify(examData);
    const total_score = examData.questions.reduce((sum, q) => sum + q.score, 0);
    const question_count = examData.questions.length;
    // 在控制台输出试卷统计信息

    // 使用FormData传递数据
    var formData = new FormData();
    formData.append('cid', examData.cid);
    formData.append('eid', examData.eid);
    formData.append('title', examData.title);
    formData.append('description', examData.description);
    formData.append('total_score', total_score);
    formData.append('question_count', question_count);
    formData.append('exam_json', btoa(encodeURIComponent(jsonString)));// 解码decodeURIComponent(atob()) 编码btoa(encodeURIComponent())
    
    // 通过服务器保存JSON文件
    const xhr = new XMLHttpRequest();
    xhr.open('POST', 'upimg.ashx?action=json', true);
    // 不设置Content-Type，让浏览器自动设置multipart/form-data
    
    xhr.onreadystatechange = function() {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                const response = xhr.responseText;
                
                if (response.startsWith('ERROR:')) {
                    alert('保存失败：' + response.substring(6));
                    return;
                }
                
                // 服务器返回文件名，创建下载链接
                const returneid = response.trim();
                examData.eid = returneid;
                alert(`试卷已成功保存！\n试卷ID: ${returneid}\n总题目数: ${question_count}\n总分: ${total_score}`);
            } else {
                alert('保存失败：服务器错误 ' + xhr.status);
            }
        }
    };
    
    xhr.send(formData);
}

// 清理题目内容中的base64图片
function cleanBase64Images(htmlContent) {
    if (!htmlContent || typeof htmlContent !== 'string') {
        return htmlContent;
    }
    
    // 查找所有base64图片并替换为提示文本
    const base64ImageRegex = /<img[^>]+src="data:image\/[^"]*"[^>]*>/gi;
    
    if (base64ImageRegex.test(htmlContent)) {
        console.warn('发现题目内容中包含base64图片，已清理');
        return htmlContent.replace(base64ImageRegex, '<span style="color: #999; font-style: italic;">[图片已移除 - 请重新上传]</span>');
    }
    
    return htmlContent;
}

// 清理整个试卷数据中的base64图片
function handleImportJson(e) {
    const file = e.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = function(event) {
        try {
            const resultData = decodeURIComponent(atob(event.target.result));
            const importedData = JSON.parse(resultData);
            
            if (!importedData.questions || !Array.isArray(importedData.questions)) {
                alert('JSON文件格式不正确：缺少questions数组！');
                return;
            }

            if (importedData.questions.length === 0) {
                alert('导入的试卷中没有题目！');
                return;
            }

            if (confirm(`即将导入试卷：\n标题: ${importedData.title || '未命名'}\n题目数: ${importedData.questions.length}\n\n是否继续？`)) {
                examData =importedData ;
                examData.eid = myeid;//设定当前试卷ID，很重要
                examData.cid = mycid; 
                
                console.log("导入数据",examData);
                saveExamData();
                renderQuestions();
                
                alert('试卷导入成功！');
            }
        } catch (error) {
            alert('JSON文件解析失败：' + error.message);
        }
    };

    reader.readAsText(file);
    e.target.value = '';
}
// 显示预览
function showPreview() {
    const previewArea = document.getElementById('previewArea');
    let previewHTML = `
        <div class="exam-info">
            <h1 class="exam-title">${examData.title}</h1>
            <p class="exam-desc">${examData.description}</p>
            <div class="student-info-simple">
                <span class="student-name">姓名：${studentInfo.name}</span>
                <span class="student-id">学号：${studentInfo.id}</span>
            </div>
        </div>
    `;
    
    if (examData.questions.length === 0) {
        previewHTML += '<div class="empty-state"><p>暂无题目，请先添加题目</p></div>';
    } else {
        examData.questions.forEach((question, questionIndex) => {
            // 填空题特殊处理：不显示题目标题，直接显示内容
            if (question.type === 'fill_blank') {
                // 填空题：处理题目内容，将___替换为输入框
                let contentWithBlanks = question.title || '';
                const tempDiv = document.createElement('div');
                tempDiv.innerHTML = contentWithBlanks;
                let textContent = tempDiv.textContent || tempDiv.innerText || '';
                
                (question.blanks || []).forEach((blank, blankIndex) => {
                    const answerLength = blank.answer ? blank.answer.length : 5;
                    const inputWidth = Math.max(60, Math.min(200, answerLength * 12 + 20));
                    const inputHtml = `<input type="text" class="fill-blank-input" id="fill-blank-${question.id}-${blankIndex}" 
                                              style="width: ${inputWidth}px;">`;
                    textContent = textContent.replace('___', inputHtml);
                });
                
                previewHTML += `
                    <div class="preview-question fill-blank-question">
                        <div class="question-header-modern">
                            <div class="question-number-left">${questionIndex + 1}</div>
                            <div class="question-content-center">
                                <div class="question-title-preview">${textContent}</div>
                            </div>
                            <div class="question-score-right">${question.score}分</div>
                        </div>
                `;
            } else {
                previewHTML += `
                    <div class="preview-question">
                        <div class="question-header-modern">
                            <div class="question-number-left">${questionIndex + 1}</div>
                            <div class="question-content-center">
                                <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                            </div>
                            <div class="question-score-right">${question.score}分</div>
                        </div>
                `;
            }
            
            switch(question.type) {
                case 'single_choice':
                    question.options.forEach((option, optIndex) => {
                        const optionData = parseOptionData(option);
                        previewHTML += `
                            <div class="preview-option">
                                <input type="radio" name="q_${question.id}" id="q_${question.id}_${optIndex}" value="q_${question.id}_${optIndex}">
                                <label for="q_${question.id}_${optIndex}">
                                    <strong>${String.fromCharCode(65 + optIndex)}.</strong> 
                                    ${optionData.text ? `<span class="option-text">${optionData.text}</span>` : ''}
                                    ${optionData.image ? `<img src="${getImageUrl(optionData.image)}" alt="选项图片" class="option-preview-image">` : ''}
                                </label>
                            </div>
                        `;
                    });
                    break;
                    
                case 'multiple_choice':
                    question.options.forEach((option, optIndex) => {
                        const optionData = parseOptionData(option);
                        previewHTML += `
                            <div class="preview-option">
                                <input type="checkbox" name="q_${question.id}" id="q_${question.id}_${optIndex}" value="q_${question.id}_${optIndex}">
                                <label for="q_${question.id}_${optIndex}">
                                    <strong>${String.fromCharCode(65 + optIndex)}.</strong> 
                                    ${optionData.text ? `<span class="option-text">${optionData.text}</span>` : ''}
                                    ${optionData.image ? `<img src="${getImageUrl(optionData.image)}" alt="选项图片" class="option-preview-image">` : ''}
                                </label>
                            </div>
                        `;
                    });
                    break;
                    
                case 'fill_blank':
                    // 填空题的内容已经在标题位置处理完成，不需要额外显示
                    break;
                    
                case 'true_false':
                    previewHTML += `
                        <div class="preview-option">
                            <input type="radio" name="q_${question.id}" id="q_${question.id}_true" value="true">
                            <label for="q_${question.id}_true">正确</label>
                        </div>
                        <div class="preview-option">
                            <input type="radio" name="q_${question.id}" id="q_${question.id}_false" value="false">
                            <label for="q_${question.id}_false">错误</label>
                        </div>
                    `;
                    break;
                    
                case 'matching':
                    // 创建随机打乱的项目数组，保持原始索引信息
                    const shuffledLeftItems = question.leftItems.map((item, index) => ({
                        item: item,
                        originalIndex: index
                    })).sort(() => Math.random() - 0.5);
                    
                    const shuffledRightItems = question.rightItems.map((item, index) => ({
                        item: item,
                        originalIndex: index
                    })).sort(() => Math.random() - 0.5);
                    
                    previewHTML += `
                        <div class="matching-container" id="preview-matching-${question.id}">
                            <div class="matching-column">
                    `;
                    
                    shuffledLeftItems.forEach((itemObj, displayIndex) => {
                        const itemData = parseMatchingItemData(itemObj.item);
                        const itemClass = itemData.image ? 'matching-item has-image' : 'matching-item';
                        previewHTML += `
                            <div class="${itemClass}" data-side="left" data-index="${itemObj.originalIndex}" data-display-index="${displayIndex}">
                                ${itemData.image ? `<img src="${getImageUrl(itemData.image)}" alt="连线图片">` : ''}
                                ${itemData.text ? `<div class="matching-item-text">${itemData.text}</div>` : ''}
                            </div>
                        `;
                    });
                    
                    previewHTML += `
                            </div>
                            <div class="matching-center">
                                <div class="matching-center-line"></div>
                                <div class="matching-center-text">连线区域</div>
                            </div>
                            <div class="matching-column">
                    `;
                    
                    shuffledRightItems.forEach((itemObj, displayIndex) => {
                        const itemData = parseMatchingItemData(itemObj.item);
                        const itemClass = itemData.image ? 'matching-item has-image' : 'matching-item';
                        previewHTML += `
                            <div class="${itemClass}" data-side="right" data-index="${itemObj.originalIndex}" data-display-index="${displayIndex}">
                                ${itemData.image ? `<img src="${getImageUrl(itemData.image)}" alt="连线图片">` : ''}
                                ${itemData.text ? `<div class="matching-item-text">${itemData.text}</div>` : ''}
                            </div>
                        `;
                    });
                    
                    previewHTML += `
                            </div>
                            <svg class="matching-lines" width="100%" height="100%" style="position: absolute; top: 0; left: 0; pointer-events: none;"></svg>
                        </div>
                        <div class="matching-instructions">
                            <div class="matching-instructions-content">
                                <p>📝 <strong>连线说明：</strong>点击左侧项目，再点击对应的右侧项目进行连线</p>
                                <button class="matching-reset-btn" onclick="resetMatching('${question.id}')" title="重置连线">
                                    🔄
                                </button>
                            </div>
                        </div>
                    `;
                    break;
                    
                case 'sort_question':
                    previewHTML += `
                        <div class="sort-question-container" id="preview-sort-${question.id}">
                            <div class="sort-items-preview">
                    `;
                    // 打乱顺序显示
                    const shuffledItems = [...question.items].sort(() => Math.random() - 0.5);
                    shuffledItems.forEach((item, idx) => {
                        previewHTML += `
                            <div class="sort-item-preview" draggable="true" data-original-index="${question.items.indexOf(item)}" data-current-index="${idx}">
                                <span class="sort-item-number">${idx + 1}</span>
                                <span class="sort-item-text">${item}</span>
                                <span class="sort-item-handle">⋮⋮</span>
                            </div>
                        `;
                    });
                    previewHTML += `
                            </div>
                            <div class="sort-instructions">请拖拽调整顺序</div>
                        </div>
                    `;
                    break;
                    
                case 'table_question':
                    previewHTML += `
                        <div class="table-question-container">
                            <table class="table-preview">
                                <thead>
                                    <tr>
                    `;
                    question.tableData.headers.forEach(header => {
                        previewHTML += `<th>${header}</th>`;
                    });
                    previewHTML += `
                                    </tr>
                                </thead>
                                <tbody>
                    `;
                    question.tableData.rows.forEach((row, rowIndex) => {
                        previewHTML += `<tr>`;
                        row.forEach((cell, colIndex) => {
                            const isAnswerCell = question.answer[`${rowIndex},${colIndex}`] !== undefined;
                            if (isAnswerCell) {
                                // 预览时不应该预填答案，应该是空的输入框
                                previewHTML += `<td><input type="text" class="table-answer-input" id="table-${question.id}-${rowIndex}-${colIndex}" placeholder="请填写答案"></td>`;
                            } else {
                                previewHTML += `<td>${cell}</td>`;
                            }
                        });
                        previewHTML += `</tr>`;
                    });
                    previewHTML += `
                                </tbody>
                            </table>
                        </div>
                    `;
                    break;
                    
                case 'short_answer':
                    previewHTML += `
                        <div class="short-answer-container">
                            <textarea class="short-answer-textarea" 
                                      id="short-answer-${question.id}"
                                      rows="3"
                                      placeholder="请在此处作答..."></textarea>
                        </div>
                    `;
                    break;
            }
            
            previewHTML += `</div>`;
        });
    }
    
    // 添加提交按钮
    previewHTML += `
        <div class="preview-actions">
            <button class="preview-btn preview-btn-primary" onclick="submitExam()">提交试卷</button>
        </div>
    `;
    
    previewArea.innerHTML = previewHTML;
    
    // 初始化连线题交互
    initMatchingQuestions();
    
    // 初始化代码高亮
    initCodeHighlighting();
    
    document.getElementById('previewModal').style.display = 'block';
}

function formatRichTextForPreview(htmlContent) {
    if (!htmlContent) return '';
    
    // 简单的HTML标签处理，确保安全显示
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = htmlContent;
    
    // 保留图片，但确保它们有适当的样式类
    const images = tempDiv.querySelectorAll('img');
    images.forEach(img => {
        // 为预览模式的图片添加样式类
        img.classList.add('preview-question-image');
        // 确保图片有合适的尺寸限制
        if (!img.style.maxWidth) {
            img.style.maxWidth = '300px';
        }
        if (!img.style.height) {
            img.style.height = 'auto';
        }
    });
    
    // 处理代码块，确保在预览时应用高亮
    const codeBlocks = tempDiv.querySelectorAll('pre.code-block code');
    codeBlocks.forEach(codeElement => {
        // 为代码块添加高亮标记，稍后在DOM插入后进行高亮
        codeElement.classList.add('hljs-to-highlight');
    });
    
    return tempDiv.innerHTML;
}

// 初始化代码高亮
function initCodeHighlighting() {
    // 检查 highlight.js 是否已加载
    if (typeof hljs !== 'undefined') {
        // 高亮所有代码块
        const codeBlocks = document.querySelectorAll('pre.code-block code');
        codeBlocks.forEach(block => {
            // 检查代码块是否已经包含HTML标签（已经高亮过）
            if (block.innerHTML.includes('<span class="hljs-')) {
                // 如果已经包含高亮HTML，跳过重新高亮
                return;
            }
            
            // 检查是否已经高亮过，如果是则先清除高亮状态
            if (block.dataset.highlighted) {
                delete block.dataset.highlighted;
            }
            
            // 移除之前的高亮类
            block.classList.remove('hljs-to-highlight');
            block.classList.remove('hljs');
            
            // 确保代码内容是纯文本，避免HTML注入
            const codeText = block.textContent || block.innerText || '';
            if (codeText.trim()) {
                // 清空内容并设置为纯文本
                block.textContent = codeText;
                // 应用语法高亮
                hljs.highlightElement(block);
            }
        });
    } else {
        console.warn('Highlight.js 未加载，代码高亮功能不可用');
    }
}

// 更新学生信息
function updateStudentInfo(field, value) {
    studentInfo[field] = value;
}

// 初始化连线题交互功能
function initMatchingQuestions() {
    // 为所有连线题添加事件监听
    document.querySelectorAll('.matching-container').forEach(container => {
        const questionId = container.id.replace('preview-matching-', '');
        const question = examData.questions.find(q => q.id === questionId);
        
        if (question && question.type === 'matching') {
            initMatchingQuestion(container, question);
        }
    });
    
    // 初始化排序题拖拽功能
    document.querySelectorAll('.sort-question-container').forEach(container => {
        initSortQuestion(container);
    });
}

// 初始化排序题拖拽功能
function initSortQuestion(container) {
    const itemsContainer = container.querySelector('.sort-items-preview');
    const items = container.querySelectorAll('.sort-item-preview');
    
    items.forEach((item, index) => {
        // 设置初始索引
        item.dataset.currentIndex = index;
        
        item.addEventListener('dragstart', function(e) {
            this.classList.add('dragging');
            e.dataTransfer.effectAllowed = 'move';
            e.dataTransfer.setData('text/plain', this.dataset.currentIndex);
            
            // 使用淡蓝色背景而不是透明度，避免布局问题
            setTimeout(() => {
                this.classList.add('dragging'); // CSS会处理视觉效果
            }, 0);
        });
        
        item.addEventListener('dragend', function(e) {
            this.classList.remove('dragging');
            // 不需要手动设置透明度，CSS类会处理
            
            // 清除所有插入指示器
            container.querySelectorAll('.drop-indicator').forEach(indicator => {
                indicator.remove();
            });
            
            // 安全网：确保所有排序项目都可见
            container.querySelectorAll('.sort-item-preview').forEach(sortItem => {
                sortItem.classList.remove('dragging');
            });
        });
    });
    
    // 为容器添加拖拽事件
    itemsContainer.addEventListener('dragover', function(e) {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
        
        const draggingItem = container.querySelector('.dragging');
        if (!draggingItem) return;
        
        const afterElement = getDragAfterElement(itemsContainer, e.clientY);
        const dropIndicator = getOrCreateDropIndicator();
        
        if (afterElement == null) {
            itemsContainer.appendChild(dropIndicator);
        } else {
            itemsContainer.insertBefore(dropIndicator, afterElement);
        }
    });
    
    itemsContainer.addEventListener('drop', function(e) {
        e.preventDefault();
        const draggedIndex = parseInt(e.dataTransfer.getData('text/plain'));
        const draggingItem = container.querySelector('.dragging');
        const dropIndicator = container.querySelector('.drop-indicator');
        
        if (draggingItem) {
            // 确保拖拽元素恢复正常状态
            draggingItem.classList.remove('dragging');
            
            if (dropIndicator) {
                // 在指示器位置插入拖拽的元素
                dropIndicator.parentNode.insertBefore(draggingItem, dropIndicator);
                dropIndicator.remove();
            }
            
            // 更新所有项目的索引和编号
            updateSortItemsOrder(container);
        }
        
        // 清理所有可能残留的指示器
        container.querySelectorAll('.drop-indicator').forEach(indicator => {
            indicator.remove();
        });
    });
    
    itemsContainer.addEventListener('dragleave', function(e) {
        // 只有当鼠标真正离开容器时才移除指示器
        if (!itemsContainer.contains(e.relatedTarget)) {
            const dropIndicator = container.querySelector('.drop-indicator');
            if (dropIndicator) {
                dropIndicator.remove();
            }
        }
    });
}

// 获取拖拽后应该插入的位置 - 改进版本
function getDragAfterElement(container, y) {
    const draggableElements = [...container.querySelectorAll('.sort-item-preview:not(.dragging)')];
    
    // 如果没有其他元素，返回null（插入到末尾）
    if (draggableElements.length === 0) {
        return null;
    }
    
    // 检查是否在第一个元素之前
    const firstElement = draggableElements[0];
    if (firstElement) {
        const firstBox = firstElement.getBoundingClientRect();
        if (y < firstBox.top + firstBox.height / 2) {
            return firstElement; // 插入到第一个元素之前
        }
    }
    
    // 检查是否在最后一个元素之后
    const lastElement = draggableElements[draggableElements.length - 1];
    if (lastElement) {
        const lastBox = lastElement.getBoundingClientRect();
        if (y > lastBox.bottom - lastBox.height / 2) {
            return null; // 插入到末尾
        }
    }
    
    // 在中间元素之间查找最佳插入位置
    let closestElement = null;
    let closestDistance = Number.POSITIVE_INFINITY;
    
    draggableElements.forEach(child => {
        const box = child.getBoundingClientRect();
        const centerY = box.top + box.height / 2;
        const distance = Math.abs(y - centerY);
        
        if (distance < closestDistance) {
            closestDistance = distance;
            closestElement = child;
        }
    });
    
    if (closestElement) {
        const box = closestElement.getBoundingClientRect();
        const centerY = box.top + box.height / 2;
        
        // 如果鼠标在元素中心线上方，插入到该元素之前
        // 如果鼠标在元素中心线下方，插入到该元素之后
        if (y < centerY) {
            return closestElement;
        } else {
            return closestElement.nextElementSibling;
        }
    }
    
    return null;
}

// 获取或创建拖拽指示器 - 改进版本
function getOrCreateDropIndicator() {
    let indicator = document.querySelector('.drop-indicator');
    if (!indicator) {
        indicator = document.createElement('div');
        indicator.className = 'drop-indicator';
        indicator.innerHTML = '<div class="drop-line"></div>';
    }
    // 添加显示类以触发动画
    setTimeout(() => indicator.classList.add('show'), 10);
    return indicator;
}

// 更新排序项目的顺序和编号
function updateSortItemsOrder(container) {
    const items = container.querySelectorAll('.sort-item-preview');
    items.forEach((item, index) => {
        item.dataset.currentIndex = index;
        const numberSpan = item.querySelector('.sort-item-number');
        if (numberSpan) {
            numberSpan.textContent = index + 1;
        }
    });
}

// 交换排序项目位置（保留原函数以兼容性，但不再使用）
function swapSortItems(container, fromIndex, toIndex) {
    const items = Array.from(container.querySelectorAll('.sort-item-preview'));
    const fromItem = items[fromIndex];
    const toItem = items[toIndex];
    
    // 交换DOM位置
    if (fromIndex < toIndex) {
        toItem.parentNode.insertBefore(fromItem, toItem.nextSibling);
    } else {
        toItem.parentNode.insertBefore(fromItem, toItem);
    }
    
    // 更新索引
    updateSortItemsOrder(container);
}

// 初始化单个连线题
function initMatchingQuestion(container, question) {
    const leftItems = container.querySelectorAll('.matching-item[data-side="left"]');
    const rightItems = container.querySelectorAll('.matching-item[data-side="right"]');
    const svg = container.querySelector('.matching-lines');
    
    // 初始化连线状态
    matchingState.connections[question.id] = {};
    
    // 为左侧项目添加点击事件
    leftItems.forEach(item => {
        item.addEventListener('click', function() {
            const index = parseInt(this.dataset.index);
            
            // 如果已经选中了右侧项目，创建连线
            if (matchingState.selectedRight !== null && matchingState.activeQuestion === question.id) {
                createConnection(question.id, matchingState.selectedLeft, matchingState.selectedRight, svg);
                resetSelection(container);
                updateConnectionCount(question.id);
            } 
            // 否则选中左侧项目
            else {
                resetSelection(container);
                this.classList.add('selected');
                matchingState.activeQuestion = question.id;
                matchingState.selectedLeft = index;
                matchingState.selectedRight = null;
                
                // 创建临时连线
                createTempLine(container, this, svg);
            }
        });
    });
    
    // 为右侧项目添加点击事件
    rightItems.forEach(item => {
        item.addEventListener('click', function() {
            const index = parseInt(this.dataset.index);
            
            // 如果已经选中了左侧项目，创建连线
            if (matchingState.selectedLeft !== null && matchingState.activeQuestion === question.id) {
                createConnection(question.id, matchingState.selectedLeft, index, svg);
                resetSelection(container);
                updateConnectionCount(question.id);
            } 
            // 否则选中右侧项目
            else {
                resetSelection(container);
                this.classList.add('selected');
                matchingState.activeQuestion = question.id;
                matchingState.selectedLeft = null;
                matchingState.selectedRight = index;
            }
        });
    });
    
    // 鼠标移动时更新临时连线
    container.addEventListener('mousemove', function(e) {
        if (matchingState.tempLine && matchingState.selectedLeft !== null) {
            updateTempLine(container, e, svg);
        }
    });
}

// 创建连线
function createConnection(questionId, leftIndex, rightIndex, svg) {
    const connections = matchingState.connections[questionId];
    
    // 检查左侧项目是否已经连接，如果已连接则删除旧连接
    const leftIndexStr = leftIndex.toString();
    if (connections[leftIndexStr] !== undefined) {
        const oldRightIndex = connections[leftIndexStr];
        removeConnection(questionId, leftIndex, oldRightIndex, svg);
    }
    
    // 检查右侧项目是否已经连接，如果已连接则删除旧连接
    for (const leftIdx in connections) {
        if (connections[leftIdx] === rightIndex) {
            removeConnection(questionId, parseInt(leftIdx), rightIndex, svg);
            break;
        }
    }
    
    // 保存新连接 - 确保索引格式一致
    const rightIndexNum = parseInt(rightIndex);
    matchingState.connections[questionId][leftIndexStr] = rightIndexNum;
    
    // 获取左右项目元素
    const leftItem = document.querySelector(`#preview-matching-${questionId} .matching-item[data-side="left"][data-index="${leftIndex}"]`);
    const rightItem = document.querySelector(`#preview-matching-${questionId} .matching-item[data-side="right"][data-index="${rightIndex}"]`);
    
    if (leftItem && rightItem) {
        // 标记项目为已连接
        leftItem.classList.add('connected');
        rightItem.classList.add('connected');
        
        // 绘制连线
        drawConnectionLine(leftItem, rightItem, svg, questionId, leftIndex, rightIndex);
    }
}

// 移除连线
function removeConnection(questionId, leftIndex, rightIndex, svg) {
    const leftIndexStr = leftIndex.toString();
    const rightIndexNum = parseInt(rightIndex);
    
    // 删除连接记录
    delete matchingState.connections[questionId][leftIndexStr];
    
    // 移除连线元素
    const line = svg.querySelector(`line[data-connection="${questionId}-${leftIndex}-${rightIndex}"]`);
    if (line) {
        line.remove();
    }
    
    // 移除项目的已连接状态
    const leftItem = document.querySelector(`#preview-matching-${questionId} .matching-item[data-side="left"][data-index="${leftIndex}"]`);
    const rightItem = document.querySelector(`#preview-matching-${questionId} .matching-item[data-side="right"][data-index="${rightIndex}"]`);
    
    if (leftItem) {
        leftItem.classList.remove('connected');
    }
    if (rightItem) {
        rightItem.classList.remove('connected');
    }
}

// 绘制连线 - 改为直线
function drawConnectionLine(leftItem, rightItem, svg, questionId, leftIndex, rightIndex) {
    const leftRect = leftItem.getBoundingClientRect();
    const rightRect = rightItem.getBoundingClientRect();
    const containerRect = svg.getBoundingClientRect();
    
    // 计算连线起点和终点（项目中心点）
    const startX = leftRect.right - containerRect.left;
    const startY = leftRect.top + leftRect.height / 2 - containerRect.top;
    const endX = rightRect.left - containerRect.left;
    const endY = rightRect.top + rightRect.height / 2 - containerRect.top;
    
    // 创建直线路径
    const line = document.createElementNS("http://www.w3.org/2000/svg", "line");
    line.setAttribute("x1", startX);
    line.setAttribute("y1", startY);
    line.setAttribute("x2", endX);
    line.setAttribute("y2", endY);
    line.setAttribute("class", "matching-line connected");
    line.setAttribute("data-connection", `${questionId}-${leftIndex}-${rightIndex}`);
    
    svg.appendChild(line);
}

// 创建临时连线 - 改为直线
function createTempLine(container, leftItem, svg) {
    // 移除之前的临时连线
    if (matchingState.tempLine) {
        matchingState.tempLine.remove();
    }
    
    const leftRect = leftItem.getBoundingClientRect();
    const containerRect = container.getBoundingClientRect();
    
    // 计算起点
    const startX = leftRect.right - containerRect.left;
    const startY = leftRect.top + leftRect.height / 2 - containerRect.top;
    
    // 创建临时连线（直线）
    const line = document.createElementNS("http://www.w3.org/2000/svg", "line");
    line.setAttribute("x1", startX);
    line.setAttribute("y1", startY);
    line.setAttribute("x2", startX);
    line.setAttribute("y2", startY);
    line.setAttribute("class", "matching-line temp");
    
    svg.appendChild(line);
    matchingState.tempLine = line;
}

// 更新临时连线 - 改为直线
function updateTempLine(container, e, svg) {
    if (!matchingState.tempLine) return;
    
    const containerRect = container.getBoundingClientRect();
    const mouseX = e.clientX - containerRect.left;
    const mouseY = e.clientY - containerRect.top;
    
    // 获取起点（选中的左侧项目）
    const leftItem = container.querySelector('.matching-item[data-side="left"].selected');
    const leftRect = leftItem.getBoundingClientRect();
    const startX = leftRect.right - containerRect.left;
    const startY = leftRect.top + leftRect.height / 2 - containerRect.top;
    
    // 更新临时连线端点
    matchingState.tempLine.setAttribute("x2", mouseX);
    matchingState.tempLine.setAttribute("y2", mouseY);
}

// 重置选择状态
function resetSelection(container) {
    container.querySelectorAll('.matching-item.selected').forEach(item => {
        item.classList.remove('selected');
    });
    
    // 移除临时连线
    if (matchingState.tempLine) {
        matchingState.tempLine.remove();
        matchingState.tempLine = null;
    }
    
    matchingState.selectedLeft = null;
    matchingState.selectedRight = null;
}

// 更新连线计数
function updateConnectionCount(questionId) {
    const count = Object.keys(matchingState.connections[questionId] || {}).length;
    const question = examData.questions.find(q => q.id === questionId);
    const total = question ? question.leftItems.length : 0;
    
    const countElement = document.getElementById(`connection-count-${questionId}`);
    if (countElement) {
        countElement.textContent = `已连线: ${count}/${total}`;
    }
}

// 重置连线题
function resetMatching(questionId) {
    const container = document.getElementById(`preview-matching-${questionId}`);
    if (!container) return;
    
    const svg = container.querySelector('.matching-lines');
    
    // 清除所有连线
    if (svg) {
        svg.innerHTML = '';
    }
    
    // 重置连接状态
    matchingState.connections[questionId] = {};
    
    // 重置项目样式
    container.querySelectorAll('.matching-item').forEach(item => {
        item.classList.remove('connected');
    });
    
    resetSelection(container);
}

// 提交试卷
function submitExam() {
    const submissionData = collectSubmissionData();
    submissionResult = gradeExam(submissionData);
    showSubmissionResult(submissionResult);
}

// 收集提交数据
function collectSubmissionData() {
    const submissionData = {
        examId: 'exam_' + Date.now(),
        examTitle: examData.title,
        submitTime: new Date().toISOString(),
        studentInfo: studentInfo,
        answers: [],
        summary: {
            totalQuestions: examData.questions.length,
            answeredQuestions: 0,
            correctAnswers: 0,
            totalScore: 0,
            earnedScore: 0,
            accuracy: 0
        }
    };

    // 收集每道题的答案
    examData.questions.forEach((question) => {
        const userAnswer = getUserAnswer(question);
        const isCorrect = checkAnswer(question, userAnswer);
        // 连线题按正确连接数计算得分
        const earnedScore = calculateEarnedScore(question, userAnswer, isCorrect);
        
        // 获取正确答案（根据题型不同处理）
        let correctAnswer;
        switch(question.type) {
            case 'fill_blank':
                // 填空题的正确答案存储在 question.blanks 数组中
                correctAnswer = (question.blanks || []).map(blank => blank.answer);
                break;
            case 'short_answer':
                // 简答题的正确答案和关键词
                correctAnswer = {
                    answer: question.answer,
                    keywords: question.keywords || [],
                    threshold: question.keywordThreshold || 60
                };
                break;
            case 'matching':
            case 'sort_question':
            case 'table_question':
            case 'single_choice':
            case 'true_false':
            default:
                // 其他题型的正确答案存储在 question.answer 中
                correctAnswer = question.answer;
        }
        
        submissionData.answers.push({
            questionId: question.id,
            questionType: question.type,
            questionTitle: question.title,
            userAnswer: userAnswer,
            correctAnswer: correctAnswer,
            isCorrect: isCorrect,
            score: question.score,
            earnedScore: earnedScore
        });

        // 更新统计信息
        let isAnswered = false;
        if (userAnswer !== null && userAnswer !== undefined) {
            switch(question.type) {
                case 'fill_blank':
                    // 填空题：至少有一个空白被填写
                    isAnswered = Array.isArray(userAnswer) && userAnswer.some(answer => answer.trim() !== '');
                    break;
                case 'multiple_choice':
                    // 多选题：至少选择了一个选项
                    isAnswered = Array.isArray(userAnswer) && userAnswer.length > 0;
                    break;
                case 'matching':
                    // 连线题：至少有一个连线
                    isAnswered = Object.keys(userAnswer).length > 0;
                    break;
                case 'table_question':
                    // 表格题：至少有一个单元格被填写
                    isAnswered = Object.values(userAnswer).some(value => value.trim() !== '');
                    break;
                case 'short_answer':
                    // 简答题：有文本内容就算已作答
                    isAnswered = userAnswer && userAnswer.toString().trim().length > 0;
                    break;
                default:
                    // 其他题型：有答案就算已作答
                    isAnswered = true;
            }
        }
        
        if (isAnswered) {
            submissionData.summary.answeredQuestions++;
        }
        if (isCorrect) submissionData.summary.correctAnswers++;
        submissionData.summary.totalScore += question.score;
        submissionData.summary.earnedScore += earnedScore;
    });

    submissionData.summary.accuracy = 
        submissionData.summary.totalScore > 0 ? 
        submissionData.summary.earnedScore / submissionData.summary.totalScore : 0;

    return submissionData;
}

// 计算得分（特别处理连线题）
// 同时修复连线题评分显示，增加更详细的统计信息
function calculateEarnedScore(question, userAnswer, isCorrect) {
    if (question.type === 'matching') {
        const correctConnections = question.answer;
        const userConnections = userAnswer || {};
        
        let correctCount = 0;
        let totalCount = Object.keys(correctConnections).length;
        
        if (totalCount === 0) return 0;
        
        // 计算正确连接数
        for (let leftIndex in correctConnections) {
            const leftIndexStr = leftIndex.toString();
            const correctRightIndex = correctConnections[leftIndex];
            const userRightIndex = userConnections[leftIndexStr];
            
            if (userRightIndex === correctRightIndex) {
                correctCount++;
            }
        }
        
        // 按正确比例计算得分
        const correctRatio = correctCount / totalCount;
        return Math.round(question.score * correctRatio);
    }
    
    if (question.type === 'multiple_choice') {
        // 多选题特殊评分逻辑：选对得分，选错扣分
        const correctAnswers = question.answer || [];
        const userAnswers = userAnswer || [];
        
        if (correctAnswers.length === 0) return 0;
        
        // 计算每个选项的分值
        const scorePerOption = question.score / correctAnswers.length;
        
        let earnedScore = 0;
        
        // 检查用户选择的每个选项
        userAnswers.forEach(userChoice => {
            if (correctAnswers.includes(userChoice)) {
                // 选对了，加分
                earnedScore += scorePerOption;
            } else {
                // 选错了，扣分
                earnedScore -= scorePerOption;
            }
        });
        
        // 检查是否有正确答案未被选择（漏选不扣分，但也不得分）
        // 这里不需要额外处理，因为漏选的选项不会在userAnswers中
        
        // 确保得分不为负数
        return Math.max(0, Math.round(earnedScore));
    }
    
    if (question.type === 'table_question') {
        const correctAnswers = question.answer;
        const userAnswers = userAnswer || {};
        
        let correctCount = 0;
        let totalCount = Object.keys(correctAnswers).length;
        
        if (totalCount === 0) return 0;
        
        for (let key in correctAnswers) {
            if (userAnswers[key] && userAnswers[key].toString().trim().toLowerCase() === correctAnswers[key].toString().trim().toLowerCase()) {
                correctCount++;
            }
        }
        
        const correctRatio = correctCount / totalCount;
        return Math.round(question.score * correctRatio);
    }
    
    if (question.type === 'short_answer') {
        // 简答题使用关键词匹配自动评分
        const scoreResult = scoreShortAnswer(userAnswer, question);
        return scoreResult.score;
    }
    
    return isCorrect ? question.score : 0;
}

// 获取用户答案
function getUserAnswer(question) {
    switch(question.type) {
        case 'single_choice':
            const selected = document.querySelector(`input[name="q_${question.id}"]:checked`);
            return selected ? parseInt(selected.id.split('_').pop()) : null;
            
        case 'multiple_choice':
            const selectedOptions = document.querySelectorAll(`input[name="q_${question.id}"]:checked`);
            return Array.from(selectedOptions).map(input => parseInt(input.id.split('_').pop()));
            
        case 'fill_blank':
            const answers = [];
            (question.blanks || []).forEach((blank, index) => {
                const input = document.getElementById(`fill-blank-${question.id}-${index}`);
                answers.push(input ? input.value : '');
            });
            return answers;
            
        case 'true_false':
            const selectedTF = document.querySelector(`input[name="q_${question.id}"]:checked`);
            return selectedTF ? selectedTF.value === 'true' : null;
            
        case 'matching':
            return matchingState.connections[question.id] || {};
            
        case 'sort_question':
            const sortContainer = document.getElementById(`preview-sort-${question.id}`);
            if (sortContainer) {
                const items = Array.from(sortContainer.querySelectorAll('.sort-item-preview'));
                return items.map(item => parseInt(item.dataset.originalIndex));
            }
            return [];
            
        case 'table_question':
            const tableAnswers = {};
            Object.keys(question.answer).forEach(key => {
                const [row, col] = key.split(',');
                const input = document.getElementById(`table-${question.id}-${row}-${col}`);
                tableAnswers[key] = input ? input.value : '';
            });
            return tableAnswers;
            
        case 'short_answer':
            const shortAnswerInput = document.getElementById(`short-answer-${question.id}`);
            return shortAnswerInput ? shortAnswerInput.value : '';
            
        default:
            return null;
    }
}

// 检查答案
function checkAnswer(question, userAnswer) {
    if (userAnswer === null || userAnswer === undefined) return false;
    
    let correctCount = 0;
    let totalCount = 0;
    
    switch(question.type) {
        case 'single_choice':
        case 'true_false':
            return userAnswer === question.answer;
            
        case 'multiple_choice':
            if (!Array.isArray(userAnswer) || !Array.isArray(question.answer)) return false;
            
            // 检查用户选择的选项数量是否与正确答案数量相同
            if (userAnswer.length !== question.answer.length) return false;
            
            // 检查每个用户选择的选项是否都在正确答案中
            const sortedUserAnswer = [...userAnswer].sort((a, b) => a - b);
            const sortedCorrectAnswer = [...question.answer].sort((a, b) => a - b);
            
            return sortedUserAnswer.every((answer, index) => answer === sortedCorrectAnswer[index]);
            
            
        case 'fill_blank':
            if (!Array.isArray(userAnswer)) return false;
            
            // 获取所有正确答案
            const correctAnswers = (question.blanks || []).map(blank => 
                blank.answer.toString().trim().toLowerCase()
            );
            
            // 获取所有用户答案（去除空白）
            const userAnswers = userAnswer.map(answer => 
                answer.toString().trim().toLowerCase()
            ).filter(answer => answer !== '');
            
            // 如果用户答案数量不够，返回false
            if (userAnswers.length !== correctAnswers.length) {
                return false;
            }
            
            // 检查每个用户答案是否在正确答案中（不考虑顺序）
            const correctAnswersCopy = [...correctAnswers];
            for (let userAns of userAnswers) {
                const index = correctAnswersCopy.indexOf(userAns);
                if (index === -1) {
                    return false; // 找不到匹配的答案
                }
                correctAnswersCopy.splice(index, 1); // 移除已匹配的答案
            }
            
            return correctAnswersCopy.length === 0; // 所有答案都被匹配
            
        case 'matching':
            const userConnections = userAnswer;
            const correctConnections = question.answer;
            
            // 检查用户是否完成了所有连线
            if (Object.keys(userConnections).length !== Object.keys(correctConnections).length) {
                return false;
            }
            
            // 检查每个连线是否正确
            for (let leftIndex in correctConnections) {
                // 统一转换为字符串格式进行比较
                const leftIndexStr = leftIndex.toString();
                const correctRightIndex = correctConnections[leftIndex];
                const userRightIndex = userConnections[leftIndexStr];
                
                if (userRightIndex !== correctRightIndex) {
                    return false;
                }
            }
            
            return true;
            
        case 'sort_question':
            if (!Array.isArray(userAnswer) || userAnswer.length !== question.answer.length) {
                return false;
            }
            return userAnswer.every((item, index) => item === question.answer[index]);
            
        case 'table_question':
            const tableCorrectAnswers = question.answer;
            const tableUserAnswers = userAnswer || {};
            
            correctCount = 0;
            totalCount = Object.keys(tableCorrectAnswers).length;
            
            if (totalCount === 0) return false;
            
            for (let key in tableCorrectAnswers) {
                if (tableUserAnswers[key] && tableUserAnswers[key].toString().trim().toLowerCase() === tableCorrectAnswers[key].toString().trim().toLowerCase()) {
                    correctCount++;
                }
            }
            
            return correctCount === totalCount;
            
        case 'short_answer':
            // 简答题使用关键词匹配评分，这里只是检查是否有答案
            return userAnswer && userAnswer.toString().trim().length > 0;
            
        default:
            return false;
    }
}

// 评分
function gradeExam(submissionData) {
    return submissionData;
}

// 修复显示提交结果的函数
function showSubmissionResult(result) {
    const previewArea = document.getElementById('previewArea');
    
    let resultHTML = `
        <div class="submission-result">
            <div class="result-title">试卷提交成功！</div>
            <div class="result-summary">
                <div class="result-item">
                    <div class="result-value">${result.summary.earnedScore}/${result.summary.totalScore}</div>
                    <div class="result-label">总分</div>
                </div>
                <div class="result-item">
                    <div class="result-value">${result.summary.correctAnswers}/${result.summary.totalQuestions}</div>
                    <div class="result-label">正确题数</div>
                </div>
                <div class="result-item">
                    <div class="result-value">${(result.summary.accuracy * 100).toFixed(1)}%</div>
                    <div class="result-label">正确率</div>
                </div>
            </div>
            <div class="result-details">
                <div class="result-details-title">题目详情</div>
    `;
    
    result.answers.forEach((answer, index) => {
        const isCorrectClass = answer.isCorrect ? '' : 'incorrect';
        const scoreClass = answer.earnedScore > 0 ? '' : 'zero';
        const question = examData.questions.find(q => q.id === answer.questionId);
        
        // 对于连线题，在题目详情中只显示文本
        let questionTitle = answer.questionTitle;
        if (question && question.type === 'matching') {
            // 移除图片相关的HTML内容，只保留纯文本
            questionTitle = questionTitle.replace(/<img[^>]*>/g, '');
        }
        
        resultHTML += `
            <div class="question-result ${isCorrectClass}">
                <div class="question-result-header">
                    <div class="question-result-number">第${index + 1}题</div>
                    <div class="question-result-score ${scoreClass}">${answer.earnedScore}/${answer.score}分</div>
                </div>
                <div class="question-result-title">${formatRichTextForPreview(questionTitle)}</div>
                <div class="question-result-answer">
                    <strong>你的答案:</strong> ${formatAnswer(answer.userAnswer, answer.questionType, question)}<br>
                    <strong>正确答案:</strong> ${formatAnswer(answer.correctAnswer, answer.questionType, question)}
                </div>
            </div>
        `;
    });
    
    resultHTML += `
            </div>
        </div>
        <div class="preview-actions">
            <button type="button" class="preview-btn preview-btn-primary" onclick="hidePreview()">返回编辑</button>
        </div>
    `;
    
    previewArea.innerHTML = resultHTML;
    
    // 保存提交记录
    saveSubmission(result);
}

// 修复格式化答案显示函数
function formatAnswer(answer, questionType, question) {
    if (answer === null || answer === undefined) return '未作答';
    
    switch(questionType) {
        case 'single_choice':
            return String.fromCharCode(65 + answer);
        case 'multiple_choice':
            if (Array.isArray(answer)) {
                return answer.map(index => String.fromCharCode(65 + index)).join('、');
            }
            return '未作答';
        case 'true_false':
            return answer ? '正确' : '错误';
        case 'fill_blank':
            if (Array.isArray(answer)) {
                return answer.map((ans, index) => `空白${index + 1}: ${ans || '未填写'}`).join('； ');
            }
            return answer ? answer.toString() : '未作答';
        case 'matching':
            return formatMatchingAnswer(answer, question);
        case 'sort_question':
            if (Array.isArray(answer)) {
                return answer.map((index, pos) => `${pos + 1}. ${question.items[index]}`).join('； ');
            }
            return '未排序';
        case 'table_question':
            const tableAnswers = [];
            Object.keys(answer).forEach(key => {
                const [row, col] = key.split(',');
                tableAnswers.push(`${question.tableData.headers[col]}: ${answer[key]}`);
            });
            return tableAnswers.join('； ') || '未填写';
        default:
            return answer.toString();
    }
}

// 新增连线题答案格式化函数
function formatMatchingAnswer(userAnswer, question) {
    const connections = userAnswer;
    const count = Object.keys(connections).length;
    
    if (count === 0) return '未连线';
    
    let result = [];
    for (let leftIndex in connections) {
        const leftItem = question.leftItems[leftIndex];
        const leftData = parseMatchingItemData(leftItem);
        const rightIndex = connections[leftIndex];
        const rightItem = question.rightItems[rightIndex];
        const rightData = parseMatchingItemData(rightItem);
        
        // 只显示文本内容，不显示图片
        result.push(`${leftData.text} → ${rightData.text}`);
    }
    
    return result.join('； ');
}

// 保存提交数据
function saveSubmission(submissionData) {
    // 保存到localStorage
    const submissions = JSON.parse(localStorage.getItem('examSubmissions') || '[]');
    submissions.push(submissionData);
    localStorage.setItem('examSubmissions', JSON.stringify(submissions));
    
    // 在控制台打印提交数据（方便测试查看）
    //console.log('试卷提交数据:', JSON.stringify(submissionData, null, 2));
}


// 隐藏预览
function hidePreview() {
    document.getElementById('previewModal').style.display = 'none';
    // 重置连线状态
    matchingState.connections = {};
    matchingState.activeQuestion = null;
    matchingState.selectedLeft = null;
    matchingState.selectedRight = null;
}

// 处理图片上传
function handleImageUpload(questionId, side, index, fileInput) {
    const file = fileInput.files[0];
    if (!file) return;
    
    // 检查文件类型
    if (!file.type.startsWith('image/')) {
        alert('请选择图片文件！');
        return;
    }
    
    // 检查文件大小（限制为2MB）
    if (file.size > 2 * 1024 * 1024) {
        alert('图片大小不能超过2MB！');
        return;
    }
    
    // 使用服务器上传，不再使用base64
    uploadImage(file,
        function(response) {
            // 上传成功 - response是图片文件名
            const question = examData.questions.find(q => q.id === questionId);
            if (question) {
                const currentItem = side === 'left' ? question.leftItems[index] : question.rightItems[index];
                const itemData = parseMatchingItemData(currentItem);
                
                // 更新项目数据（保留文本，添加图片文件名）
                const newItemData = serializeMatchingItemData(itemData.text, response);
                
                if (side === 'left') {
                    question.leftItems[index] = newItemData;
                } else {
                    question.rightItems[index] = newItemData;
                }
                
                saveExamData();
                renderQuestions();
            }
        },
        function(error) {
            alert('图片上传失败：' + error);
        }
    );
    
    // 重置文件输入
    fileInput.value = '';
}

// 更新连线题项目文本内容
function updateMatchingItemText(questionId, side, index, newText) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const currentItem = side === 'left' ? question.leftItems[index] : question.rightItems[index];
        const itemData = parseMatchingItemData(currentItem);
        
        // 更新文本，保留图片
        const newItemData = serializeMatchingItemData(newText, itemData.image);
        
        if (side === 'left') {
            question.leftItems[index] = newItemData;
        } else {
            question.rightItems[index] = newItemData;
        }
        saveExamData();
        
        // 重新渲染以更新答案设置界面
        renderQuestions();
    }
}

// 删除连线题项目的图片
function removeMatchingItemImage(questionId, side, index) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const currentItem = side === 'left' ? question.leftItems[index] : question.rightItems[index];
        const itemData = parseMatchingItemData(currentItem);
        
        // 移除图片，保留文本
        const newItemData = itemData.text || '';
        
        if (side === 'left') {
            question.leftItems[index] = newItemData;
        } else {
            question.rightItems[index] = newItemData;
        }
        saveExamData();
        renderQuestions();
    }
}

// ========== 排序题相关函数 ==========
function updateSortItem(questionId, index, newValue) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.items[index] = newValue;
        saveExamData();
        updateSortPreview(questionId);
    }
}

function addSortItem(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.items.push(`项目${question.items.length + 1}`);
        question.answer.push(question.items.length - 1);
        saveExamData();
        renderQuestions();
    }
}

function deleteSortItem(questionId, index) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.items.length > 2) {
        question.items.splice(index, 1);
        // 重新调整答案数组
        question.answer = question.items.map((_, idx) => idx);
        saveExamData();
        renderQuestions();
    } else {
        alert('至少需要保留2个排序项目');
    }
}

function updateSortPreview(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    const previewElement = document.getElementById(`sort-preview-${questionId}`);
    if (question && previewElement) {
        previewElement.innerHTML = question.items.map((item, idx) => 
            `<span class="sort-preview-item">${idx + 1}. ${item}</span>`
        ).join('');
    }
}

// ========== 改进的填空题相关函数 ==========
function refreshFillBlanks(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const summernoteId = `summernote-content-${questionId}`;
        let content = '';
        
        if ($(`#${summernoteId}`).length > 0) {
            content = $(`#${summernoteId}`).summernote('code') || '';
            question.title = content;
        }
        
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = content;
        const textContent = tempDiv.textContent || tempDiv.innerText || '';
        const blanks = (textContent.match(/___/g) || []).length;
        const currentBlanks = (question.blanks || []).length;
        
        if (blanks > currentBlanks) {
            if (!question.blanks) question.blanks = [];
            for (let i = currentBlanks; i < blanks; i++) {
                question.blanks.push({
                    answer: `答案${i + 1}`
                });
            }
        } else if (blanks < currentBlanks) {
            question.blanks = question.blanks.slice(0, blanks);
        }
        
        saveExamData();
        renderQuestions();
    }
}

function deleteFillBlank(questionId, blankIndex) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.blanks && question.blanks.length > 1) {
        question.blanks.splice(blankIndex, 1);
        saveExamData();
        renderQuestions();
    } else {
        alert('至少需要保留1个空白');
    }
}

// ========== 表格题相关函数 ==========
function updateTableHeader(questionId, index, newValue) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.tableData.headers[index] = newValue;
        saveExamData();
    }
}

function updateTableCell(questionId, rowIndex, colIndex, newValue) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.tableData.rows[rowIndex][colIndex] = newValue;
        saveExamData();
    }
}

function toggleTableAnswer(questionId, rowIndex, colIndex, isAnswer) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const key = `${rowIndex},${colIndex}`;
        if (isAnswer) {
            // 设置为答题区域时，初始化标准答案，清空显示内容
            question.answer[key] = question.answer[key] || '';
            question.tableData.rows[rowIndex][colIndex] = ''; // 清空显示内容
        } else {
            // 取消答题区域时，删除答案设置
            delete question.answer[key];
        }
        saveExamData();
        renderQuestions();
    }
}

function updateTableAnswer(questionId, rowIndex, colIndex, newAnswer) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const key = `${rowIndex},${colIndex}`;
        question.answer[key] = newAnswer;
        saveExamData();
    }
}

function addTableRow(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const newRow = question.tableData.headers.map((_, index) => 
            index === 0 ? `行${question.tableData.rows.length + 1}` : ''
        );
        question.tableData.rows.push(newRow);
        saveExamData();
        renderQuestions();
    }
}

function deleteTableRow(questionId, rowIndex) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.tableData.rows.length > 1) {
        question.tableData.rows.splice(rowIndex, 1);
        // 清理相关答案
        const newAnswer = {};
        Object.keys(question.answer).forEach(key => {
            const [row, col] = key.split(',').map(Number);
            if (row < rowIndex) {
                newAnswer[key] = question.answer[key];
            } else if (row > rowIndex) {
                newAnswer[`${row - 1},${col}`] = question.answer[key];
            }
        });
        question.answer = newAnswer;
        saveExamData();
        renderQuestions();
    } else {
        alert('至少需要保留1行');
    }
}

function addTableColumn(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        question.tableData.headers.push(`列${question.tableData.headers.length + 1}`);
        question.tableData.rows.forEach(row => row.push(''));
        saveExamData();
        renderQuestions();
    }
}

function deleteTableColumn(questionId, colIndex) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.tableData.headers.length > 2) {
        question.tableData.headers.splice(colIndex, 1);
        question.tableData.rows.forEach(row => row.splice(colIndex, 1));
        // 清理相关答案
        const newAnswer = {};
        Object.keys(question.answer).forEach(key => {
            const [row, col] = key.split(',').map(Number);
            if (col < colIndex) {
                newAnswer[key] = question.answer[key];
            } else if (col > colIndex) {
                newAnswer[`${row},${col - 1}`] = question.answer[key];
            }
        });
        question.answer = newAnswer;
        saveExamData();
        renderQuestions();
    } else {
        alert('至少需要保留2列');
    }
}

// ==================== 题库导入功能 ====================

let bankState = {
    banks: [],
    questions: [],
    selectedQuestions: new Set(),
    pageIndex: 1,
    pageSize: 10,
    totalPages: 0,
    total: 0
};

function initBankImport() {
    console.log('initBankImport called');
    const importFromBankBtn = document.getElementById('importFromBankBtn');
    console.log('importFromBankBtn:', importFromBankBtn);
    if (importFromBankBtn) {
        importFromBankBtn.addEventListener('click', function() {
            console.log('Button clicked');
            openBankModal();
        });
    } else {
        console.log('Button not found');
    }
}

function openBankModal() {
    console.log('openBankModal called');
    const modal = document.getElementById('bankModal');
    console.log('Modal element:', modal);
    if (modal) {
        modal.classList.add('show');
        loadBanks();
        loadBankQuestions();
    } else {
        console.log('Modal not found');
        alert('弹窗元素未找到');
    }
}

function closeBankModal() {
    document.getElementById('bankModal').classList.remove('show');
    bankState.selectedQuestions.clear();
    updateSelectedCount();
}

function loadBanks() {
    fetch('QuestionBankApi.ashx?action=getBanks')
        .then(response => response.json())
        .then(function(data) {
            if (data.needUpgrade) {
                document.getElementById('bankQuestionList').innerHTML = 
                    '<div class="bank-empty"><div class="bank-empty-icon">⚠️</div><p>题库表尚未创建</p><p style="font-size:0.875rem;">请先访问 <a href="../manager/dbupgrade.aspx" target="_blank" style="color:#3b82f6;">数据库升级页面</a> 创建题库表</p></div>';
                return;
            }
            if (data.success) {
                bankState.banks = data.data;
                var select = document.getElementById('bankSelect');
                select.innerHTML = '<option value="0">全部题库</option>';
                if (data.data && data.data.length > 0) {
                    data.data.forEach(function(bank) {
                        select.innerHTML += '<option value="' + bank.bankId + '">' + bank.bankName + ' (' + bank.questionCount + '题)</option>';
                    });
                } else {
                    select.innerHTML = '<option value="0">暂无题库</option>';
                }
            }
        })
        .catch(function(err) {
            console.error('加载题库列表失败:', err);
            document.getElementById('bankQuestionList').innerHTML = '<div class="bank-empty"><div class="bank-empty-icon">❌</div><p>加载失败，请刷新重试</p></div>';
        });
}

function loadBankQuestions(page) {
    page = page || 1;
    bankState.pageIndex = page;
    var bankId = document.getElementById('bankSelect').value;
    var questionType = document.getElementById('questionTypeFilter').value;
    var difficulty = document.getElementById('difficultyFilter').value;
    var keyword = document.getElementById('keywordFilter').value;
    
    var url = 'QuestionBankApi.ashx?action=getQuestions&bankId=' + bankId + '&pageIndex=' + page + '&pageSize=' + bankState.pageSize;
    if (questionType && questionType !== '0') url += '&questionType=' + questionType;
    if (difficulty && difficulty !== '0') url += '&difficulty=' + difficulty;
    if (keyword) url += '&keyword=' + encodeURIComponent(keyword);
    
    document.getElementById('bankQuestionList').innerHTML = '<div class="bank-loading">加载中...</div>';
    
    fetch(url)
        .then(response => response.json())
        .then(function(data) {
            if (data.needUpgrade) {
                document.getElementById('bankQuestionList').innerHTML = 
                    '<div class="bank-empty"><div class="bank-empty-icon">⚠️</div><p>题库表尚未创建</p><p style="font-size:0.875rem;">请先访问 <a href="../manager/dbupgrade.aspx" target="_blank" style="color:#3b82f6;">数据库升级页面</a> 创建题库表</p></div>';
                return;
            }
            if (data.success) {
                bankState.questions = data.data;
                bankState.total = data.total;
                bankState.totalPages = data.totalPages;
                renderQuestionList();
                renderPagination();
            } else {
                document.getElementById('bankQuestionList').innerHTML = '<div class="bank-empty"><div class="bank-empty-icon">📭</div><p>' + (data.message || '加载失败') + '</p></div>';
            }
        })
        .catch(function(err) {
            console.error('加载题目列表失败:', err);
            document.getElementById('bankQuestionList').innerHTML = '<div class="bank-empty"><div class="bank-empty-icon">❌</div><p>加载失败，请刷新重试</p></div>';
        });
}

function renderQuestionList() {
    var container = document.getElementById('bankQuestionList');
    
    if (bankState.questions.length === 0) {
        container.innerHTML = '<div class="bank-empty"><div class="bank-empty-icon">📭</div><p>暂无题目</p></div>';
        return;
    }
    
    var html = '';
    bankState.questions.forEach(function(q) {
        var isSelected = bankState.selectedQuestions.has(q.questionId);
        var diffClass = q.difficulty === 1 ? 'easy' : (q.difficulty === 2 ? 'medium' : 'hard');
        
        html += '<div class="bank-question-item ' + (isSelected ? 'selected' : '') + '" onclick="toggleQuestion(' + q.questionId + ')">';
        html += '<div class="bank-question-checkbox">';
        html += '<input type="checkbox" ' + (isSelected ? 'checked' : '') + ' onclick="event.stopPropagation(); toggleQuestion(' + q.questionId + ')">';
        html += '</div>';
        html += '<div class="bank-question-content">';
        html += '<div class="bank-question-meta">';
        html += '<span class="bank-question-type">' + q.questionTypeName + '</span>';
        html += '<span class="bank-question-difficulty ' + diffClass + '">' + q.difficultyName + '</span>';
        html += '</div>';
        html += '<div class="bank-question-text">' + stripHtml(q.questionText || q.questionContent || '') + '</div>';
        html += '</div></div>';
    });
    
    container.innerHTML = html;
}

function renderPagination() {
    var container = document.getElementById('bankPagination');
    
    if (bankState.totalPages <= 1) {
        container.innerHTML = '<span>共 ' + bankState.total + ' 题</span>';
        return;
    }
    
    var html = '<button onclick="loadBankQuestions(' + (bankState.pageIndex - 1) + ')" ' + (bankState.pageIndex <= 1 ? 'disabled' : '') + '>上一页</button>';
    
    var startPage = Math.max(1, bankState.pageIndex - 2);
    var endPage = Math.min(bankState.totalPages, bankState.pageIndex + 2);
    
    for (var i = startPage; i <= endPage; i++) {
        html += '<button onclick="loadBankQuestions(' + i + ')" class="' + (i === bankState.pageIndex ? 'active' : '') + '">' + i + '</button>';
    }
    
    html += '<button onclick="loadBankQuestions(' + (bankState.pageIndex + 1) + ')" ' + (bankState.pageIndex >= bankState.totalPages ? 'disabled' : '') + '>下一页</button>';
    html += '<span>共 ' + bankState.total + ' 题</span>';
    
    container.innerHTML = html;
}

function toggleQuestion(questionId) {
    if (bankState.selectedQuestions.has(questionId)) {
        bankState.selectedQuestions.delete(questionId);
    } else {
        bankState.selectedQuestions.add(questionId);
    }
    updateSelectedCount();
    renderQuestionList();
}

function toggleSelectAll() {
    var selectAll = document.getElementById('selectAllQuestions');
    if (selectAll.checked) {
        bankState.questions.forEach(function(q) { bankState.selectedQuestions.add(q.questionId); });
    } else {
        bankState.questions.forEach(function(q) { bankState.selectedQuestions.delete(q.questionId); });
    }
    updateSelectedCount();
    renderQuestionList();
}

function updateSelectedCount() {
    document.getElementById('selectedCount').textContent = bankState.selectedQuestions.size;
}

function searchQuestions(event) {
    if (event.key === 'Enter') {
        loadBankQuestions(1);
    }
}

function importSelectedQuestions() {
    if (bankState.selectedQuestions.size === 0) {
        alert('请先选择要导入的题目');
        return;
    }
    
    var questionIds = Array.from(bankState.selectedQuestions).join(',');
    
    fetch('QuestionBankApi.ashx?action=importQuestions&questionIds=' + questionIds)
        .then(response => response.json())
        .then(function(data) {
            if (data.success && data.data) {
                data.data.forEach(function(q) {
                    var newQuestion = convertToExamQuestion(q);
                    examData.questions.push(newQuestion);
                });
                saveExamData();
                renderQuestions();
                closeBankModal();
                alert('成功导入 ' + data.data.length + ' 道题目');
            } else {
                alert('导入失败：' + (data.message || '未知错误'));
            }
        })
        .catch(function(err) {
            console.error('导入题目失败:', err);
            alert('导入失败，请重试');
        });
}

function convertToExamQuestion(dbQuestion) {
    var questionId = 'q_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    
    var newQuestion = {
        id: questionId,
        type: dbQuestion.questionType,
        title: dbQuestion.questionTitle || '',
        score: dbQuestion.score || 5
    };
    
    switch(dbQuestion.questionType) {
        case 'single_choice':
            newQuestion.options = dbQuestion.options || ['', '', '', ''];
            newQuestion.answer = 0;
            if (Array.isArray(dbQuestion.options)) {
                newQuestion.options = dbQuestion.options.map(function(opt) { return opt.content || opt.label || opt; });
                var correctIdx = dbQuestion.options.findIndex(function(opt) { return opt.isCorrect; });
                if (correctIdx >= 0) newQuestion.answer = correctIdx;
            }
            break;
        case 'multiple_choice':
            newQuestion.options = dbQuestion.options || ['', '', '', ''];
            newQuestion.answer = [];
            if (Array.isArray(dbQuestion.options)) {
                newQuestion.options = dbQuestion.options.map(function(opt) { return opt.content || opt.label || opt; });
                newQuestion.answer = dbQuestion.options
                    .map(function(opt, idx) { return opt.isCorrect ? idx : -1; })
                    .filter(function(idx) { return idx >= 0; });
            }
            break;
        case 'true_false':
            newQuestion.answer = dbQuestion.answer === '对' || dbQuestion.answer === true;
            break;
        case 'fill_blank':
            newQuestion.blanks = [];
            if (Array.isArray(dbQuestion.answer)) {
                dbQuestion.answer.forEach(function(ans) {
                    newQuestion.blanks.push({ answer: ans });
                });
            } else if (typeof dbQuestion.answer === 'string') {
                dbQuestion.answer.split('|').forEach(function(ans) {
                    newQuestion.blanks.push({ answer: ans.trim() });
                });
            }
            if (newQuestion.blanks.length === 0) {
                newQuestion.blanks = [{ answer: '' }];
            }
            break;
        case 'short_answer':
            newQuestion.answer = dbQuestion.answer || '';
            newQuestion.keywords = [];
            newQuestion.keywordThreshold = 60;
            break;
        case 'matching':
            newQuestion.leftItems = [];
            newQuestion.rightItems = [];
            newQuestion.answer = {};
            break;
        case 'sort_question':
            newQuestion.items = [];
            newQuestion.answer = [];
            break;
        default:
            newQuestion.answer = dbQuestion.answer || '';
    }
    
    return newQuestion;
}

function stripHtml(html) {
    if (!html) return '';
    var result = html.replace(/<[^>]*>/g, '');
    result = result.replace(/&nbsp;/g, ' ');
    result = result.replace(/&lt;/g, '<');
    result = result.replace(/&gt;/g, '>');
    result = result.replace(/&amp;/g, '&');
    if (result.length > 100) result = result.substring(0, 100) + '...';
    return result;
}

// 初始化应用
window.onload = function() {
    init();
    initBankImport();
    
    // 初始化代码高亮（如果 highlight.js 已加载）
    if (typeof hljs !== 'undefined') {
        hljs.configure({
            languages: ['javascript', 'python', 'java', 'cpp', 'html', 'css', 'sql']
        });
    }
};

// ==================== 图片上传功能 ====================

/**
 * 图片上传配置
 */
const ImageUploadConfig = {
    uploadUrl: 'upimg.ashx?action=image',
    maxFileSize: 5 * 1024 * 1024, // 5MB
    allowedTypes: ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/bmp']
};

/**
 * 通用图片上传函数
 * @param {File} file - 要上传的文件
 * @param {Function} onSuccess - 成功回调函数
 * @param {Function} onError - 错误回调函数
 * @param {Function} onProgress - 进度回调函数（可选）
 */
function uploadImage(file, onSuccess, onError, onProgress) {
    // 验证文件
    if (!validateImageFile(file)) {
        if (onError) onError('文件验证失败');
        return;
    }
    
    // 创建FormData
    const formData = new FormData();
    formData.append('file', file);
    
    // 创建XMLHttpRequest
    const xhr = new XMLHttpRequest();
    
    // 监听上传进度
    if (onProgress && xhr.upload) {
        xhr.upload.addEventListener('progress', function(e) {
            if (e.lengthComputable) {
                const percentComplete = (e.loaded / e.total) * 100;
                onProgress(percentComplete);
            }
        });
    }
    
    // 监听响应
    xhr.addEventListener('load', function() {
        if (xhr.status === 200) {
            const response = xhr.responseText.trim();
            if (response.startsWith('ERROR:')) {
                // 错误响应
                const errorMessage = response.substring(6); // 移除 "ERROR:" 前缀
                if (onError) onError(errorMessage);
            } else {
                // 成功响应，直接返回图片路径字符串
                if (onSuccess) onSuccess(response);
            }
        } else {
            if (onError) onError('网络错误：' + xhr.status);
        }
    });
    
    // 监听错误
    xhr.addEventListener('error', function() {
        if (onError) onError('网络连接失败');
    });
    
    // 发送请求
    xhr.open('POST', ImageUploadConfig.uploadUrl, true);
    xhr.send(formData);
}

/**
 * 验证图片文件
 * @param {File} file - 要验证的文件
 * @returns {boolean} - 验证结果
 */
function validateImageFile(file) {
    if (!file) {
        alert('请选择文件');
        return false;
    }
    
    // 检查文件大小
    if (file.size > ImageUploadConfig.maxFileSize) {
        alert('文件大小不能超过5MB');
        return false;
    }
    
    // 检查文件类型
    if (ImageUploadConfig.allowedTypes.indexOf(file.type) === -1) {
        alert('只支持 JPG、PNG、GIF、BMP 格式的图片');
        return false;
    }
    
    return true;
}

/**
 * 上传图片到服务器（用于富文本编辑器）
 * @param {File} file - 要上传的文件
 * @param {string} questionId - 题目ID
 */
function uploadImageToServer(file, questionId) {
    // 验证文件
    if (!validateImageFile(file)) {
        return;
    }
    
    uploadImage(file,
        function(response) {
            // 上传成功 - response是文件名，需要补全完整路径
            const fullImageUrl = getImageUrl(response);
            const imageHtml = `<img src="${fullImageUrl}">`;
            $(`#summernote-content-${questionId}`).summernote('pasteHTML', imageHtml);
        },
        function(error) {
            // 上传失败 - 显示错误信息
            alert('图片上传失败：' + error);
        }
    );
}

/**
 * 获取图片URL（补全完整路径）
 * @param {string} fileName - 图片文件名
 * @returns {string} - 完整的图片URL
 */
function getImageUrl(fileName) {
    if (!fileName) return '';
    // 如果已经是完整路径，直接返回
    if (fileName.includes('/')) {
        return fileName;
    }
    // 否则补全路径
    return '../webform/uploads/' + fileName;
}

/**
 * 处理选项图片上传
 * @param {string} questionId - 题目ID
 * @param {number} optionIndex - 选项索引
 * @param {HTMLInputElement} input - 文件输入元素
 */
function handleOptionImageUpload(questionId, optionIndex, input) {
    const file = input.files[0];
    if (!file) return;
    
    // 显示上传进度
    showUploadProgress(questionId, optionIndex, 'option');
    
    uploadImage(file, 
        function(response) {
            // 上传成功 - response直接是图片路径
            hideUploadProgress(questionId, optionIndex, 'option');
            updateOptionImage(questionId, optionIndex, response);
            input.value = ''; // 清空文件输入
        },
        function(error) {
            // 上传失败
            hideUploadProgress(questionId, optionIndex, 'option');
            alert('图片上传失败：' + error);
            input.value = ''; // 清空文件输入
        },
        function(progress) {
            // 更新进度
            updateUploadProgress(questionId, optionIndex, 'option', progress);
        }
    );
}

/**
 * 处理连线题图片上传
 * @param {string} questionId - 题目ID
 * @param {string} side - 'left' 或 'right'
 * @param {number} itemIndex - 项目索引
 * @param {HTMLInputElement} input - 文件输入元素
 */
function handleImageUpload(questionId, side, itemIndex, input) {
    const file = input.files[0];
    if (!file) return;
    
    // 显示上传进度
    showUploadProgress(questionId, itemIndex, 'matching_' + side);
    
    uploadImage(file,
        function(response) {
            // 上传成功 - response直接是图片路径
            hideUploadProgress(questionId, itemIndex, 'matching_' + side);
            updateMatchingItemImage(questionId, side, itemIndex, response);
            input.value = ''; // 清空文件输入
        },
        function(error) {
            // 上传失败
            hideUploadProgress(questionId, itemIndex, 'matching_' + side);
            alert('图片上传失败：' + error);
            input.value = ''; // 清空文件输入
        },
        function(progress) {
            // 更新进度
            updateUploadProgress(questionId, itemIndex, 'matching_' + side, progress);
        }
    );
}

/**
 * 更新选项图片
 * @param {string} questionId - 题目ID
 * @param {number} optionIndex - 选项索引
 * @param {string} imagePath - 图片相对路径
 */
function updateOptionImage(questionId, optionIndex, imagePath) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question && question.options) {
        // 更新选项数据，支持图片信息
        const option = question.options[optionIndex];
        const optionData = parseOptionData(option);
        optionData.image = imagePath;
        
        // 重新组合选项数据
        question.options[optionIndex] = optionData.text + (imagePath ? `||IMG:${imagePath}` : '');
        
        saveExamData();
        renderQuestions();
    }
}

/**
 * 更新连线题项目图片
 * @param {string} questionId - 题目ID
 * @param {string} side - 'left' 或 'right'
 * @param {number} itemIndex - 项目索引
 * @param {string} imagePath - 图片相对路径
 */
function updateMatchingItemImage(questionId, side, itemIndex, imagePath) {
    const question = examData.questions.find(q => q.id === questionId);
    if (question) {
        const items = side === 'left' ? question.leftItems : question.rightItems;
        if (items && items[itemIndex] !== undefined) {
            const itemData = parseMatchingItemData(items[itemIndex]);
            itemData.image = imagePath;
            
            // 重新组合项目数据
            items[itemIndex] = itemData.text + (imagePath ? `||IMG:${imagePath}` : '');
            
            saveExamData();
            renderQuestions();
        }
    }
}

/**
 * 删除选项图片
 * @param {string} questionId - 题目ID
 * @param {number} optionIndex - 选项索引
 */
function removeOptionImage(questionId, optionIndex) {
    if (confirm('确定要删除这张图片吗？')) {
        updateOptionImage(questionId, optionIndex, null);
    }
}

/**
 * 删除连线题项目图片
 * @param {string} questionId - 题目ID
 * @param {string} side - 'left' 或 'right'
 * @param {number} itemIndex - 项目索引
 */
function removeMatchingItemImage(questionId, side, itemIndex) {
    if (confirm('确定要删除这张图片吗？')) {
        updateMatchingItemImage(questionId, side, itemIndex, null);
    }
}

/**
 * 显示上传进度
 * @param {string} questionId - 题目ID
 * @param {number} index - 索引
 * @param {string} type - 类型
 */
function showUploadProgress(questionId, index, type) {
    const progressId = `upload-progress-${questionId}-${index}-${type}`;
    
    // 移除已存在的进度条
    const existingProgress = document.getElementById(progressId);
    if (existingProgress) {
        existingProgress.remove();
    }
    
    // 创建进度条
    const progressDiv = document.createElement('div');
    progressDiv.id = progressId;
    progressDiv.className = 'upload-progress';
    progressDiv.innerHTML = `
        <div class="upload-progress-bar">
            <div class="upload-progress-fill" style="width: 0%"></div>
        </div>
        <div class="upload-progress-text">上传中... 0%</div>
    `;
    
    // 找到合适的位置插入进度条
    let targetElement;
    if (type === 'option') {
        targetElement = document.querySelector(`#option-${questionId}-${index}`);
    } else if (type.startsWith('matching_')) {
        const side = type.split('_')[1];
        targetElement = document.querySelector(`#image-input-${side}-${questionId}-${index}`);
        if (targetElement) {
            targetElement = targetElement.parentElement;
        }
    }
    
    if (targetElement) {
        targetElement.appendChild(progressDiv);
    }
}

/**
 * 更新上传进度
 * @param {string} questionId - 题目ID
 * @param {number} index - 索引
 * @param {string} type - 类型
 * @param {number} progress - 进度百分比
 */
function updateUploadProgress(questionId, index, type, progress) {
    const progressId = `upload-progress-${questionId}-${index}-${type}`;
    const progressDiv = document.getElementById(progressId);
    
    if (progressDiv) {
        const progressFill = progressDiv.querySelector('.upload-progress-fill');
        const progressText = progressDiv.querySelector('.upload-progress-text');
        
        if (progressFill) {
            progressFill.style.width = progress + '%';
        }
        if (progressText) {
            progressText.textContent = `上传中... ${Math.round(progress)}%`;
        }
    }
}

/**
 * 隐藏上传进度
 * @param {string} questionId - 题目ID
 * @param {number} index - 索引
 * @param {string} type - 类型
 */
function hideUploadProgress(questionId, index, type) {
    const progressId = `upload-progress-${questionId}-${index}-${type}`;
    const progressDiv = document.getElementById(progressId);
    
    if (progressDiv) {
        progressDiv.remove();
    }
}

/**
 * 解析选项数据（支持图片）
 * @param {string} optionText - 选项文本
 * @returns {Object} - 解析后的选项数据
 */
function parseOptionData(optionText) {
    if (typeof optionText !== 'string') {
        return { text: optionText, image: null };
    }
    
    const parts = optionText.split('||IMG:');
    return {
        text: parts[0] || '',
        image: parts[1] || null
    };
}

/**
 * 解析连线题项目数据（支持图片）
 * @param {string} itemText - 项目文本
 * @returns {Object} - 解析后的项目数据
 */
function parseMatchingItemData(itemText) {
    if (typeof itemText !== 'string') {
        return { text: itemText, image: null };
    }
    
    const parts = itemText.split('||IMG:');
    return {
        text: parts[0] || '',
        image: parts[1] || null
    };
}

// 测试连线题答案检查（调试用）
function debugMatchingAnswer(questionId) {
    const question = examData.questions.find(q => q.id === questionId);
    if (!question || question.type !== 'matching') {
        console.log('不是连线题或题目不存在');
        return;
    }
    
    const userAnswer = matchingState.connections[questionId] || {};
    const correctAnswer = question.answer;
    
    console.log('=== 连线题答案调试 ===');
    console.log('题目ID:', questionId);
    console.log('正确答案:', correctAnswer);
    console.log('用户答案:', userAnswer);
    console.log('正确答案键类型:', Object.keys(correctAnswer).map(k => typeof k));
    console.log('用户答案键类型:', Object.keys(userAnswer).map(k => typeof k));
    
    // 逐个检查连线
    for (let leftIndex in correctAnswer) {
        const leftIndexStr = leftIndex.toString();
        const correctRightIndex = correctAnswer[leftIndex];
        const userRightIndex = userAnswer[leftIndexStr];
        
        console.log(`左侧${leftIndex} -> 正确:${correctRightIndex}, 用户:${userRightIndex}, 匹配:${userRightIndex === correctRightIndex}`);
    }
    
    const isCorrect = checkAnswer(question, userAnswer);
    console.log('最终结果:', isCorrect);
}

// 调试所有连线题的答案设置
function debugAllMatchingQuestions() {
    console.log('=== 所有连线题答案调试 ===');
    const matchingQuestions = examData.questions.filter(q => q.type === 'matching');
    
    if (matchingQuestions.length === 0) {
        console.log('当前试卷中没有连线题');
        return;
    }
    
    matchingQuestions.forEach((question, index) => {
        console.log(`\n连线题 ${index + 1}:`);
        console.log('题目:', question.title.replace(/<[^>]*>/g, ''));
        console.log('左侧项目:', question.leftItems.map(item => item.split('||')[0]));
        console.log('右侧项目:', question.rightItems);
        console.log('当前答案:', question.answer);
        
        const isOneToOne = checkIfOneToOneMapping(question.answer, question.leftItems.length, question.rightItems.length);
        console.log('是否一一对应:', isOneToOne);
        
        if (!isOneToOne) {
            console.log('❌ 需要修复');
        } else {
            console.log('✅ 答案正确');
        }
    });
}

// 在浏览器控制台中可以调用：debugMatchingAnswer('题目ID') 或 debugAllMatchingQuestions()
