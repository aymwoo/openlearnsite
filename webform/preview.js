const mylid = document.getElementById('HiddenLid').value;
const mydone = document.getElementById('HiddenDone').value;
const mycid = document.getElementById('HiddenCid').value;
const myeid = document.getElementById('HiddenEid').value;
const Examjson = document.getElementById('HiddenExamjson').value;
const jsonData = decodeURIComponent(atob(Examjson));
//console.log("编号测试：",mycid,myeid);
const localStorageFlag = "examData_"+ mycid +"_"+ myeid ;

let examData = {
    eid: myeid,
    cid: mycid,
    timestamp: null,
    title: "信息科技测验",
    description: "请仔细阅读题目并作答",
    enableAiAssessment: false,
    questions: []
};

// 学生信息
let studentInfo = {
    name: "测试学生",
    id: "2024001",
    class: "测试班级"
};

// 连线题状态管理
let matchingState = {
    activeQuestion: null,
    selectedLeft: null,
    selectedRight: null,
    connections: {},
    tempLine: null
};

// 提交结果
let submissionResult = null;
const startTimestamp = Date.now();
// 初始化
document.addEventListener('DOMContentLoaded', function() {
    loadExamData();
    syncAssessmentCopy();
    renderExam();
    setupEventListeners();
    initCodeHighlighting();
});

function getAssessmentCopy(enableAi) {
    return enableAi ? {
        loadingTitle: '正在提交测验并生成 AI 评估',
        loadingDesc: '系统正在提交测验结果，请稍候。',
        summaryLabel: 'AI 简短反馈'
    } : {
        loadingTitle: '正在提交测验并生成规则评估摘要',
        loadingDesc: '系统正在提交测验结果，并生成规则评估摘要，请稍候。',
        summaryLabel: '规则评估摘要'
    };
}

function syncAssessmentCopy() {
    var copy = getAssessmentCopy(!!examData.enableAiAssessment);
    var loadingTitle = document.getElementById('examAiLoadingTitle');
    var loadingDesc = document.getElementById('examAiLoadingDesc');
    var summaryLabel = document.getElementById('examAiSummaryLabel');

    if (loadingTitle) loadingTitle.textContent = copy.loadingTitle;
    if (loadingDesc) loadingDesc.textContent = copy.loadingDesc;
    if (summaryLabel) summaryLabel.textContent = copy.summaryLabel;
}

// 从localStorage加载试卷数据
function loadExamData() {
    if(jsonData) {
        examData = JSON.parse(jsonData);
        if (typeof examData.enableAiAssessment !== 'boolean') {
            examData.enableAiAssessment = false;
        }
        examData.eid = myeid;//设定当前试卷ID，很重要
        examData.cid = mycid; 
        console.log("读取数据库：",myeid);
        //console.log("读取数据库：",examData);
    }
}

// 渲染试卷
function renderExam() {
    const examTitle = document.getElementById('examTitle');
    const examDescription = document.getElementById('examDescription');
    const questionsContainer = document.getElementById('questionsContainer');
    const emptyState = document.getElementById('emptyState');

    const total_score = examData.questions.reduce((sum, q) => sum + q.score, 0);
    if (examTitle) {
        examTitle.textContent = examData.title || '课堂测验';
    }
    examDescription.textContent = examData.description +"    满分为"+total_score+"分";

    if (examData.questions.length === 0) {
        questionsContainer.style.display = 'none';
        emptyState.style.display = 'block';
        return;
    }

    questionsContainer.style.display = 'block';
    emptyState.style.display = 'none';

    let html = '';
    examData.questions.forEach((question, index) => {
        html += renderQuestion(question, index);
    });

    questionsContainer.innerHTML = html;

    // 初始化交互式题型
    setTimeout(() => {
        initMatchingQuestions();
    }, 100);
}

// 渲染单个题目
function renderQuestion(question, index) {
    const questionNumber = index + 1;
    let html = '';

    switch(question.type) {
        case 'single_choice':
            html = renderSingleChoice(question, questionNumber);
            break;
        case 'multiple_choice':
            html = renderMultipleChoice(question, questionNumber);
            break;
        case 'fill_blank':
            html = renderFillBlank(question, questionNumber);
            break;
        case 'true_false':
            html = renderTrueFalse(question, questionNumber);
            break;
        case 'matching':
            html = renderMatching(question, questionNumber);
            break;
        case 'sort_question':
            html = renderSortQuestion(question, questionNumber);
            break;
        case 'table_question':
            html = renderTableQuestion(question, questionNumber);
            break;
        case 'short_answer':
            html = renderShortAnswer(question, questionNumber);
            break;
        default:
            html = `<div class="question-item">未知题型</div>`;
    }

    return html;
}

// 渲染单选题
function renderSingleChoice(question, questionNumber) {
    let optionsHtml = '';
    question.options.forEach((option, optIndex) => {
        const optionData = parseOptionData(option);
        optionsHtml += `
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

    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            ${optionsHtml}
        </div>
    `;
}

// 渲染多选题
function renderMultipleChoice(question, questionNumber) {
    let optionsHtml = '';
    question.options.forEach((option, optIndex) => {
        const optionData = parseOptionData(option);
        optionsHtml += `
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

    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            ${optionsHtml}
        </div>
    `;
}

// 渲染填空题
function renderFillBlank(question, questionNumber) {
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

    return `
        <div class="preview-question fill-blank-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${textContent}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
        </div>
    `;
}

// 渲染判断题
function renderTrueFalse(question, questionNumber) {
    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            <div class="preview-option">
                <input type="radio" name="q_${question.id}" id="q_${question.id}_true" value="true">
                <label for="q_${question.id}_true">正确</label>
            </div>
            <div class="preview-option">
                <input type="radio" name="q_${question.id}" id="q_${question.id}_false" value="false">
                <label for="q_${question.id}_false">错误</label>
            </div>
        </div>
    `;
}

// 渲染连线题
function renderMatching(question, questionNumber) {
    // 创建随机打乱的项目数组，保持原始索引信息
    const shuffledLeftItems = question.leftItems.map((item, index) => ({
        item: item,
        originalIndex: index
    })).sort(() => Math.random() - 0.5);
    
    const shuffledRightItems = question.rightItems.map((item, index) => ({
        item: item,
        originalIndex: index
    })).sort(() => Math.random() - 0.5);
    
    let leftItemsHtml = '';
    let rightItemsHtml = '';
    
    shuffledLeftItems.forEach((itemObj, displayIndex) => {
        const itemData = parseMatchingItemData(itemObj.item);
        const itemClass = itemData.image ? 'matching-item has-image' : 'matching-item';
        leftItemsHtml += `
            <div class="${itemClass}" data-side="left" data-index="${itemObj.originalIndex}" data-display-index="${displayIndex}">
                ${itemData.image ? `<img src="${getImageUrl(itemData.image)}" alt="连线图片">` : ''}
                ${itemData.text ? `<div class="matching-item-text">${itemData.text}</div>` : ''}
            </div>
        `;
    });
    
    shuffledRightItems.forEach((itemObj, displayIndex) => {
        const itemData = parseMatchingItemData(itemObj.item);
        const itemClass = itemData.image ? 'matching-item has-image' : 'matching-item';
        rightItemsHtml += `
            <div class="${itemClass}" data-side="right" data-index="${itemObj.originalIndex}" data-display-index="${displayIndex}">
                ${itemData.image ? `<img src="${getImageUrl(itemData.image)}" alt="连线图片">` : ''}
                ${itemData.text ? `<div class="matching-item-text">${itemData.text}</div>` : ''}
            </div>
        `;
    });

    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            <div class="matching-container" id="preview-matching-${question.id}">
                <div class="matching-column">
                    ${leftItemsHtml}
                </div>
                <div class="matching-column">
                    ${rightItemsHtml}
                </div>
                <svg class="matching-lines" width="100%" height="100%" style="position: absolute; top: 0; left: 0; pointer-events: none;"></svg>
            </div>
            <div class="matching-instructions">
                <div class="matching-instructions-content">
                    <p>📝 <strong>连线说明：</strong>点击左侧项目，再点击对应的右侧项目进行连线</p>
                    <button  type="button"  class="matching-reset-btn" onclick="resetMatching('${question.id}')" title="重置连线">
                        🔄
                    </button>
                </div>
            </div>
        </div>
    `;
}

// 解析连线题项目数据（文本和图片）
function parseMatchingItemData(item) {
    if (typeof item !== 'string') {
        return { text: item, image: null };
    }
    
    if (item.includes('||IMG:')) {
        const parts = item.split('||IMG:');
        return {
            text: parts[0] || '',
            image: parts[1] || null
        };
    }
    
    return { text: item, image: null };
}

// 渲染排序题
function renderSortQuestion(question, questionNumber) {
    let itemsHtml = '';
    // 打乱顺序显示
    const shuffledItems = [...question.items].sort(() => Math.random() - 0.5);
    shuffledItems.forEach((item, idx) => {
        itemsHtml += `
            <div class="sort-item-preview" draggable="true" data-original-index="${question.items.indexOf(item)}" data-current-index="${idx}">
                <span class="sort-item-number">${idx + 1}</span>
                <span class="sort-item-text">${item}</span>
                <span class="sort-item-handle">⋮⋮</span>
            </div>
        `;
    });

    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            <div class="sort-question-container" id="preview-sort-${question.id}">
                <div class="sort-items-preview">
                    ${itemsHtml}
                </div>
                <div class="sort-instructions">请拖拽调整顺序</div>
            </div>
        </div>
    `;
}

// 渲染表格题
function renderTableQuestion(question, questionNumber) {
    let headersHtml = '';
    let rowsHtml = '';

    question.tableData.headers.forEach(header => {
        headersHtml += `<th>${header}</th>`;
    });

    question.tableData.rows.forEach((row, rowIndex) => {
        rowsHtml += `<tr>`;
        row.forEach((cell, colIndex) => {
            const isAnswerCell = question.answer[`${rowIndex},${colIndex}`] !== undefined;
            if (isAnswerCell) {
                // 预览时不应该预填答案，应该是空的输入框
                rowsHtml += `<td><input type="text" class="table-answer-input" id="table-${question.id}-${rowIndex}-${colIndex}" placeholder="请填写答案"></td>`;
            } else {
                rowsHtml += `<td>${cell}</td>`;
            }
        });
        rowsHtml += `</tr>`;
    });

    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            <div class="table-question-container">
                <table class="table-preview">
                    <thead>
                        <tr>
                            ${headersHtml}
                        </tr>
                    </thead>
                    <tbody>
                        ${rowsHtml}
                    </tbody>
                </table>
            </div>
        </div>
    `;
}

// 渲染简答题
function renderShortAnswer(question, questionNumber) {
    return `
        <div class="preview-question">
            <div class="question-header-modern">
                <div class="question-number-left">${questionNumber}</div>
                <div class="question-content-center">
                    <div class="question-title-preview">${formatRichTextForPreview(question.title)}</div>
                </div>
                <div class="question-score-right">${question.score}分</div>
            </div>
            <div class="short-answer-container">
                <textarea class="short-answer-textarea" 
                          id="short-answer-${question.id}"
                          rows="3"
                          placeholder="请在此处作答..."></textarea>
            </div>
        </div>
    `;
}

// 格式化选项文本（处理图片）
function formatOptionText(option) {
    if (typeof option !== 'string') return option;
    
    if (option.includes('||IMG:')) {
        const parts = option.split('||IMG:');
        const text = parts[0];
        const imageName = parts[1];
        const imageUrl = getImageUrl(imageName);
        return `${text}<br><img src="${imageUrl}" class="option-image" alt="选项图片">`;
    }
    
    return option;
}

// 解析选项数据（文本和图片）
function parseOptionData(option) {
    if (typeof option !== 'string') {
        return { text: option, image: null };
    }
    
    if (option.includes('||IMG:')) {
        const parts = option.split('||IMG:');
        return {
            text: parts[0] || '',
            image: parts[1] || null
        };
    }
    
    return { text: option, image: null };
}

// 格式化富文本
function formatRichText(html) {
    if (!html) return '';
    
    let formatted = html;
    
    // 处理图片路径
    formatted = formatted.replace(
        /src="([^"]+)"/g,
        (match, src) => {
            if (src.includes('uploads/')) {
                return `src="${src}"`;
            }
            return match;
        }
    );
    
    // 处理代码块
    formatted = formatted.replace(
        /<pre><code>([\s\S]*?)<\/code><\/pre>/g,
        (match, code) => {
            return `<div class="code-block"><code>${escapeHtml(code)}</code></div>`;
        }
    );
    
    return formatted;
}

// 格式化富文本用于预览
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

// 转义HTML
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// 获取图片URL
function getImageUrl(fileName) {
    if (!fileName) return '';
    if (fileName.startsWith('http')) return fileName;
    if (fileName.startsWith('/')) return fileName;
    return `../webform/uploads/${fileName}`;
}

// 初始化连线题
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

// 设置事件监听器
function setupEventListeners() {    
    const submitBtn = document.getElementById('submitBtn');
    const submitImg = document.getElementById('submitImg');
    const submitScore = document.getElementById('submitScore');
    if(mydone=="0"){
        if (submitBtn) {
            submitBtn.addEventListener('click', submitExam);
        }
    }
    else{
        submitBtn.style.display = 'none';
        submitImg.style.display = 'block';
        submitScore.style.display = 'block';
    }
}

function submitExam() {
    const finished = confirm("确定要提交答卷吗？");
    
    if (finished) {
        const endTimestamp = Date.now();
        const submissionData = collectSubmissionData();
        const result = gradeExam(submissionData);
    
        submissionResult = result;
        showResultModal(result);
    
        // 隐藏浮动按钮，因为结果页面有返回按钮
        const floatingButtons = document.querySelector('.floating-buttons');
        if (floatingButtons) {
            floatingButtons.style.display = 'none';
        }
        const score = result.earnedScore;
        const spend = Math.ceil((endTimestamp - startTimestamp) / (1000 * 60));
        const qcount = examData.questions ? examData.questions.length : 0;
        const adataStr = JSON.stringify(submissionData);
        
        console.log("答题得分：", score);
        console.log("用时（分）：", spend);
        console.log("提交数据：", submissionData);

        if (examData.enableAiAssessment) {
            showExamAiLoading('系统正在提交测验结果，请稍候。');
            submitExamWithAi(score, spend, qcount, adataStr);
            return;
        }

        submitExamWithoutAi(score, spend, qcount, adataStr);
    }
}

function submitExamWithoutAi(score, spend, qcount, adataStr) {
    showExamAiLoading('系统正在提交测验结果，并生成规则评估摘要，请稍候。', false);
    submitExamWithAi(score, spend, qcount, adataStr, false);
}

function submitExamWithAi(score, spend, qcount, adataStr, enableAi) {
    if (enableAi === undefined) enableAi = true;
    var formData = new FormData();
    formData.append('Lid', mylid);
    formData.append('Cid', mycid);
    formData.append('Eid', myeid);
    formData.append('Ascore', score);
    formData.append('Aspent', spend);
    formData.append('Qcount', qcount);
    formData.append('Adata', adataStr);
    formData.append('EnableAiAssessment', enableAi ? '1' : '0');

    var xhr = new XMLHttpRequest();
    xhr.open('POST', 'uploadanswer.ashx', true);

    xhr.onreadystatechange = function() {
        if (xhr.readyState !== 4) return;

        if (xhr.status !== 200) {
            closeExamAiLoading();
            console.warn('AI 评估提交失败，降级为普通提交：HTTP ' + xhr.status);
            submitExamXhr(score, spend, adataStr);
            return;
        }

        var payload = parseExamAiSseData(xhr.responseText);
        if (!payload || payload.ok !== true) {
            closeExamAiLoading();
            console.warn('AI 评估提交失败，降级为普通提交：', payload && payload.message ? payload.message : '未知错误');
            submitExamXhr(score, spend, adataStr);
            return;
        }

        if (window.LearnStatus && typeof window.LearnStatus.submitted === 'function') {
            window.LearnStatus.submitted();
        }

        showExamAiLoading(payload.message || (enableAi ? '提交成功，AI 测验评估已生成。' : '提交成功，规则评估摘要已生成。'), enableAi);

        var summaryBox = document.getElementById('examAiSummary');
        var summaryText = document.getElementById('examAiSummaryText');
        if (summaryBox && summaryText && payload.summary) {
            summaryText.innerHTML = payload.summary;
            summaryBox.style.display = 'block';
        }

        window.setTimeout(function () { closeExamAiLoading(); }, 2000);
    };

    xhr.send(formData);
}

// 降级用的 XHR 提交方式（不含 AI 评估）
function submitExamXhr(score, spend, adataStr) {
    var formData = new FormData();
    formData.append('Lid', mylid);
    formData.append('Cid', mycid);
    formData.append('Eid', myeid);
    formData.append('Ascore', score);
    formData.append('Aspend', spend);
    formData.append('Adata', adataStr);

    var xhr = new XMLHttpRequest();
    xhr.open('POST', 'upimg.ashx?action=answer', true);

    xhr.onreadystatechange = function() {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                var response = xhr.responseText;
                if (response.startsWith('ERROR:')) {
                    alert('保存失败：' + response.substring(6));
                    return;
                }
                if (window.LearnStatus && typeof window.LearnStatus.submitted === 'function') {
                    window.LearnStatus.submitted();
                }
            } else {
                alert('保存失败：服务器错误 ' + xhr.status);
            }
        }
    };

    xhr.send(formData);
}

// AI 评估加载遮罩层
function showExamAiLoading(message) {
    var loading = document.getElementById('examAiLoading');
    var title = document.getElementById('examAiLoadingTitle');
    var desc = document.getElementById('examAiLoadingDesc');
    var enableAi = examData && typeof examData.enableAiAssessment === 'boolean' ? examData.enableAiAssessment : false;
    if (arguments.length > 1) enableAi = !!arguments[1];
    var copy = getAssessmentCopy(enableAi);
    if (title) title.textContent = copy.loadingTitle;
    if (desc && message) desc.innerHTML = message;
    else if (desc) desc.textContent = copy.loadingDesc;
    if (loading) loading.style.display = 'flex';
}

function closeExamAiLoading() {
    var loading = document.getElementById('examAiLoading');
    if (loading) loading.style.display = 'none';
}

function parseExamAiSseData(data) {
    try { return JSON.parse(data); } catch (e) { return null; }
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

    examData.questions.forEach((question) => {
        const userAnswer = getUserAnswer(question);
        const isCorrect = checkAnswer(question, userAnswer);
        const earnedScore = calculateEarnedScore(question, userAnswer, isCorrect);
        
        let correctAnswer;
        switch(question.type) {
            case 'fill_blank':
                correctAnswer = (question.blanks || []).map(blank => blank.answer);
                break;
            case 'short_answer':
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

        let isAnswered = false;
        if (userAnswer !== null && userAnswer !== undefined) {
            switch(question.type) {
                case 'fill_blank':
                    isAnswered = Array.isArray(userAnswer) && userAnswer.some(answer => answer.trim() !== '');
                    break;
                case 'matching':
                    isAnswered = Object.keys(userAnswer).length > 0;
                    break;
                case 'table_question':
                    isAnswered = Object.values(userAnswer).some(value => value.trim() !== '');
                    break;
                case 'short_answer':
                    isAnswered = userAnswer && userAnswer.toString().trim().length > 0;
                    break;
                default:
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

// 评分简答题
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
    
    let matchedCount = 0;
    const matchedKeywords = [];
    
    keywords.forEach(keyword => {
        if (answer.includes(keyword)) {
            matchedCount++;
            matchedKeywords.push(keyword);
        }
    });
    
    const matchPercentage = (matchedCount / keywords.length) * 100;
    
    let score = 0;
    if (matchPercentage >= threshold) {
        score = question.score;
    } else if (matchPercentage >= threshold * 0.5) {
        score = Math.round(question.score * 0.7);
    } else if (matchPercentage > 0) {
        score = Math.round(question.score * 0.3);
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

function calculateEarnedScore(question, userAnswer, isCorrect) {
    if (question.type === 'matching') {
        const correctConnections = question.answer;
        const userConnections = userAnswer || {};
        
        let correctCount = 0;
        let totalCount = Object.keys(correctConnections).length;
        
        if (totalCount === 0) return 0;
        
        for (let leftIndex in correctConnections) {
            const leftIndexStr = leftIndex.toString();
            const correctRightIndex = correctConnections[leftIndex];
            const userRightIndex = userConnections[leftIndexStr];
            
            if (userRightIndex === correctRightIndex) {
                correctCount++;
            }
        }
        
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
        const scoreResult = scoreShortAnswer(userAnswer, question);
        return scoreResult.score;
    }
    
    return isCorrect ? question.score : 0;
}

// 评分试卷
function gradeExam(submissionData) {
    return {
        totalScore: submissionData.summary.totalScore,
        earnedScore: submissionData.summary.earnedScore,
        scorePercentage: submissionData.summary.totalScore > 0 ? (submissionData.summary.earnedScore / submissionData.summary.totalScore * 100).toFixed(2) : 0,
        correctCount: submissionData.summary.correctAnswers,
        totalCount: submissionData.summary.totalQuestions,
        answeredCount: submissionData.summary.answeredQuestions,
        submissionData: submissionData
    };
}

// 显示结果（与模态预览页面保持一致）
function showResultModal(result) {
    const questionsContainer = document.getElementById('questionsContainer');
    const emptyState = document.getElementById('emptyState');
    const examInfo = document.querySelector('.exam-info');
    
    // 隐藏exam-info区域
    if (examInfo) {
        examInfo.style.display = 'none';
    }
    
    // 隐藏空状态提示
    if (emptyState) {
        emptyState.style.display = 'none';
    }
    
    // 确保questionsContainer可见，用于显示结果
    if (questionsContainer) {
        questionsContainer.style.display = 'block';
    }
    
    let resultHTML = `
        <div class="submission-result">
            <div class="result-title">试卷提交成功！</div>
            <div class="result-summary">
                <div class="result-item">
                    <div class="result-value">${result.submissionData.summary.earnedScore}/${result.submissionData.summary.totalScore}</div>
                    <div class="result-label">总分</div>
                </div>
                <div class="result-item">
                    <div class="result-value">${result.submissionData.summary.correctAnswers}/${result.submissionData.summary.totalQuestions}</div>
                    <div class="result-label">正确题数</div>
                </div>
                <div class="result-item">
                    <div class="result-value">${(result.submissionData.summary.accuracy * 100).toFixed(1)}%</div>
                    <div class="result-label">正确率</div>
                </div>
            </div>
            <div class="result-details">
                <div class="result-details-title">题目详情</div>
    `;
    
    result.submissionData.answers.forEach((answer, index) => {
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
                    <div><strong>你的答案:</strong> ${formatAnswer(answer.userAnswer, answer.questionType, question)}</div>
                </div>
            </div>
        `;
    });
    //<div class="question-good-answer"><strong>正确答案:</strong> ${formatAnswer(answer.correctAnswer, answer.questionType, question)}</div>
    // 如果没有题目，显示提示信息
    if (result.submissionData.answers.length === 0) {
        resultHTML += `
            <div class="empty-exam-message">
                <p>试卷为空，没有题目需要作答。</p>
            </div>
        `;
    }
    
    resultHTML += `
            </div>
        </div>
    `;
    
    questionsContainer.innerHTML = resultHTML;
    
    // 保存提交记录
    saveSubmission(result.submissionData);
}



// 格式化答案显示函数
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
        case 'short_answer':
            return answer ? answer.toString() : '未作答';
        default:
            return answer.toString();
    }
}

// 连线题答案格式化函数
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

// 初始化代码高亮
function initCodeHighlighting() {
    document.querySelectorAll('pre code').forEach((block) => {
        // 检查代码块是否已经包含HTML标签（已经高亮过）
        if (block.innerHTML.includes('<span class="hljs-')) {
            // 如果已经包含高亮HTML，跳过重新高亮
            return;
        }
        
        // 检查是否已经高亮过，如果是则先清除高亮状态
        if (block.dataset.highlighted) {
            delete block.dataset.highlighted;
        }
        // 移除已有的hljs类，避免重复高亮
        block.classList.remove('hljs');
        
        // 确保代码内容是纯文本，避免HTML注入
        const codeText = block.textContent || block.innerText || '';
        if (codeText.trim()) {
            // 清空内容并设置为纯文本
            block.textContent = codeText;
            // 应用代码高亮
            hljs.highlightElement(block);
        }
    });
}
