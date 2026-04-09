# 更新日志 (CHANGELOG)

## 2026-04-07

### 新功能
- **数据库首次安装初始化增强**：`upgrade.aspx` 现在支持在已连通 SQL Server 但目标数据库不存在时，直接自动创建 `learnsite` 数据库并完成基础表结构导入与初始化迁移；当目标数据库已存在但仍为空库时，也可一键完成建表与初始化
- **后台数据库升级页初始化入口**：`manager/dbupgrade.aspx` 新增“创建数据库并初始化 / 创建数据表并初始化”入口，后台可直接识别“数据库不存在”“空库未初始化”“需要升级”“已是最新版”等不同状态并执行对应操作
- **Reveal.js 演示能力增强**：任务与学案相关页面补充 Reveal.js 演示模式切换、官方主题支持和全屏状态联动，改善学生查看演示型内容时的展示体验

### 问题修复
- **空库初始化握手失败修复**：针对新建数据库后立即连接目标库时偶发出现的 `pre-login handshake` 异常，增加数据库就绪等待与短时重试机制，降低首次初始化失败概率
- **数据库连接与跳转流程修复**：优化升级页和相关入口页的数据库可用性判断、跳转与提示信息，减少连接异常时的误导性提示
- **Reveal.js 与离线资源兼容修复**：将 Reveal.js 主题和相关样式资源补齐为本地化引用，修复主题切换、离线访问和页面局部样式丢失问题

### 升级增强
- **数据库初始化公共工具抽取**：新增 `DatabaseSetupHelper`，统一封装连接配置读取、`master` 可用性检查、目标数据库存在性判断、建库、等待新库可连接与建表重试逻辑，减少升级页与后台升级页的重复实现
- **SQL 脚本导入能力补强**：`SqlHelper` 新增可指定目标连接串的建表导入入口，方便首次安装、空库初始化和后续扩展初始化流程复用
- **页面样式与资源外置化延续**：继续整理教师端、管理端与任务相关页面的内联样式/脚本，统一资源组织方式，提升离线可用性与后续维护性

### 涉及文件
- 数据库初始化与升级：`upgrade.aspx`、`upgrade.aspx.cs`、`manager/dbupgrade.aspx`、`manager/dbupgrade.aspx.cs`、`App_Code/Utility/DatabaseSetupHelper.cs`、`App_Code/Utility/SqlHelper.cs`
- Reveal.js 与任务展示：任务编辑/展示相关页面、Reveal.js 本地主题与样式资源

## 2026-04-06

### 新功能
- **AI 量规生成 skill 场景落地**：`teacher/gauge.aspx` 新增量规创建 AI 流程，点击“添加量规”后通过 SSE 实时显示后端真实进度，自动调用默认 AI Provider 生成量规项，并在 `teacher/gaugeitem.aspx` 中展示生成结果、Provider 名称和明细预览
- **量规 AI 二次生成增强**：`teacher/gaugeitem.aspx` 新增“重新用AI生成一次”和“追加AI生成”两个入口，支持覆盖生成与保留原量规后追加生成，过程同样使用 SSE 实时进度
- **学生测验 AI 评估 skill**：新增默认 `student_exam` 场景 skill“AI测验评估助手”，学生在 `student/myexam.aspx` 提交测验后会自动调用默认 AI Provider 生成评估摘要、分析建议和学习日志，并把结果入库
- **教师端实时动态 AI 详情 modal**：`teacher/start.aspx` 的实时动态学生卡片支持点击查看详情，modal 中展示学生 AI 测验评估概览、Provider、Skill、生成时间、学习日志和可读化答题记录
- **学生测验 AI 评估存储表**：新增 `AIStudentExamAssessment` 表及迁移 `1.9.1.1`，用于保存学生测验 AI 分析结果、答题日志与学习日志
- **问卷设置中心**：新增 `teacher/surveysettings.aspx`，教师可集中配置问卷开关、说明和相关参数，配合 `Survey` 的 BLL/DAL/Model 调整，补齐问卷管理入口
- **测验与问卷体验升级**：重做 `student/myexam.aspx`、`webform/exam.aspx`、`webform/preview.aspx` 与 `lessons/presurvey.aspx` 的界面和交互，并新增问卷设置中心，统一测验、预览、打印和问卷管理体验
- **活动页与编辑器能力增强**：新增 `CustomActivityCatalog`、`IframeUrlHelper`，提升 `student/iframe.aspx` 的活动解析能力，同时升级像素画编辑器以及多类学生活动页、AI 工具页的工具栏和操作体验

### 问题修复
- **量规 AI 重新生成安全性优化**：覆盖模式改为“先生成后替换”，避免 AI 调用失败时先清空原量规项
- **答题日志可读性提升**：教师端实时动态 modal 不再直接显示原始答题 JSON，而是解析为逐题的正确/错误清单和学生作答内容
- **教师端富文本编辑器样式修复**：排查所有接入 Vditor 和 WangEditor 的教师页面，补齐缺失的 CSS 引入，修复 `programedit.aspx` 等页面因漏载样式导致的工具栏、按钮和编辑区错位放大问题
- **静态前端库本地化**：将教师端编辑器资源以及全站使用的 Tailwind utilities、Font Awesome、clipboard.js、FileSaver.js 等静态库切换为本地文件，减少对外部 CDN 的依赖
- **学生测验 AI 评估兼容修复**：当数据库尚未创建 `AIStudentExamAssessment` 表时，学生提交测验不再失败，教师端实时动态也不会因读取 AI 评估表异常而整体失效；同时补充了教师端学生管理页控件类型不一致导致的 `InvalidCastException` 修复
- **Tailwind 本地版本回退修复**：将误替换为 Tailwind v4 本地文件的页面统一改回匹配原 CDN 的 `2.2.19 utilities` 本地版本，修复教师端顶部栏、侧边栏等布局样式回归问题
- **问卷、测验与活动页稳定性修复**：修正 `Survey` 数据映射、测验提交流程、像素画编辑处理和活动目录逻辑，减少教师端与学生端在问卷、测验和活动页中的异常情况
- **课程与升级页逻辑修正**：补齐 `teacher/courseshow.aspx.cs`、`upgrade.aspx.cs`、`start.aspx.cs` 等页面的边界处理，提升课程展示和升级流程稳定性

### 升级增强
- **数据库升级中心预检查**：`upgrade.aspx` 新增数据库类型识别、升级结论、风险等级、待执行迁移列表和风险提示，帮助用户在“解压覆盖旧站点再升级”的场景下先判断是否适合直接升级
- **升级迁移自动补齐**：升级入口现在会在旧补丁链之后自动执行 `DbMigration.RunAllPending()`，确保新版本迁移（例如 `1.9.1.1`）不会因为旧升级入口遗漏而失效
- **升级检查辅助工具**：新增“重新检查数据库”和“导出检查报告”能力，并显示当前连接数据库名、最近预检查时间，以及按结构变更 / 初始化数据 / 性能优化分组的迁移清单
- **升级页长列表与高风险交互优化**：对“升级前检查结果”“风险提示”“待执行迁移”及分组迁移列表增加默认折叠、展开/收起按钮和高风险高亮；当系统判断“不能直接升级”时，会自动禁用升级按钮，避免误操作
- **升级页首屏信息压缩与版本对比优化**：将数据库名、预检查时间和迁移版本对比统一到紧凑概览区，移除多余版本目标卡片，简化为“旧版本 → 新版本”展示，并把升级说明改为默认摘要 + 按需展开，缩短首屏高度

### 涉及文件
- AI 量规与测验评估：`teacher/gauge*`、`teacher/start.aspx`、`student/myexam.aspx`、`student/uploadexam.ashx`
- 问卷与试卷：`teacher/surveysettings.aspx`、`lessons/presurvey.aspx`、`webform/exam.aspx`、`webform/preview.aspx`
- 活动页与编辑器：`student/iframe.aspx`、`student/Scm.master.cs`、`teacher/pixel*.aspx`、`js/toolbar-buttons.css`
- 后端与迁移：`App_Code/Common/*`、`App_Code/Dal/Survey.cs`、`App_Code/Bll/AIStudentExamAssessment.cs`、`App_Code/Utility/DbMigration.cs`

## 2026-04-05

### 新功能
- **学生端 `prog-*` 设计系统落地**：将使用 `Scm.master` 的学生页面统一迁移到 `prog-*` 视觉与布局体系，减少对运行时 Tailwind 的依赖，并统一按钮、卡片、侧栏与内容区样式

### 问题修复
- **学案导航与 sticky 侧栏修复**：调整 `Scm.master` 导航解析逻辑，并移除影响 `position: sticky` 的裁剪样式，修复学案导航不显示和侧栏滚动消失的问题
- **旧皮肤样式覆盖修复**：压制 `buttonSkinPink`、`HyperLinkPink` 等皮肤注入的内联样式，避免按钮尺寸、字号和背景色破坏新版页面布局

### 涉及文件
- 母版页与导航：`student/Scm.master`
- 学生端页面：`student/program.aspx`、`student/showcourse.aspx`、`student/txtform.aspx`、`student/showmission.aspx`、`student/showtask.aspx`、`student/console.aspx` 等

## 2026-04-03

### 新功能
- **教师端界面现代化升级**：manager 与教师后台页面统一改为更轻量的浅色视觉体系，优化卡片、表格、布局宽度、机房选择和上传交互
- **AI 教学能力接入**：任务创建页新增 AI 教学助手侧边栏，`aiprovider_api.ashx` 新增 chat 接口，支持通过默认 AI Provider 生成内容
- **课程与后台功能增强**：课程编辑支持学案封面图上传与预览，管理控制台增加快捷退出入口

### 问题修复
- **空值检查增强**：`start.aspx.cs` 和 `student.aspx.cs` 添加 room model 空值检查，防止 null 引用异常
- **课程列表组件优化**：`courseshow.aspx` 从 GridView 改为 Repeater，简化代码结构
- **删除逻辑优化**：课程菜单删除操作使用模型获取数据，避免依赖行索引
- **复选框样式改进**：教师添加页面的权限复选框使用自定义样式，对齐更合理

### 升级增强
- **后台交互与代码结构优化**：精简课程菜单拖拽排序逻辑，合并冗余分支，并在多个后台页面中同步提升可维护性与交互一致性

### 涉及文件
- 教师与管理后台：`teacher/systeminfo.aspx`、`teacher/works.aspx`、`teacher/student.aspx`、`teacher/index.aspx`、`teacher/Teach.master`
- 课程与任务：`teacher/courseedit.aspx`、`teacher/courseshow.aspx`、`teacher/missionadd.aspx`
- 学生与工具页：`student/chat.aspx`、`student/kitymind.aspx` 及相关学生页面
- AI 与开发环境：`aiprovider_api.ashx`、测试项目、`start_dev.sh`

## 2026-04-02

### 新功能
- **AI 模型提供商管理上线**：新增 `aiprovider.aspx` 页面，支持新增、编辑、删除、测试连接和批量导入 AI Provider 配置，并自动完成建表和默认数据初始化
- **编辑器与教师后台现代化升级**：课程与活动编辑页支持 KindEditor、WangEditor、Vditor 切换，教师后台同步完成一轮现代化 UI 改造
- **学生端与周边工具界面重构**：学生页面及 `student/chat.aspx`、`student/kitymind.aspx` 等工具页改为更现代的自适应布局与交互风格
- **测试与开发环境补强**：引入 xUnit 测试基础设施，并新增 `start_dev.sh` 以简化本地开发环境启动

### 问题修复
- **AI 接口与编辑器兼容修复**：修复 `aiprovider_api.ashx` 编译问题、Vditor `html2md` 初始化异常，并切换到更可用的编辑器静态资源来源
- **教师端页面结构修复**：修复 `teacher/works.aspx`、`teacher/student.aspx`、`teacher/index.aspx` 和 `teacher/Teach.master` 中的结构与样式问题，提升后台页面稳定性
- **开发环境与弹窗逻辑修复**：替换不可用的 MSSQL 镜像，并将教师模块的 TinyBox 弹窗统一为母版页共享 Modal 组件

### 升级增强
- **后台入口与可维护性增强**：教师导航栏新增 AI 模型提供商入口，多个页面在这次重构中同步减少重复代码并提升可维护性

### 涉及文件
- AI Provider 与编辑器：`aiprovider.aspx`、`aiprovider_api.ashx`、`teacher/courseedit.aspx`、`teacher/missionadd.aspx`、`teacher/missionedit.aspx`
- 教师后台：`teacher/systeminfo.aspx`、`teacher/works.aspx`、`teacher/student.aspx`、`teacher/index.aspx`、`teacher/Teach.master`
- 学生与工具页：`student/chat.aspx`、`student/kitymind.aspx` 及相关学生页面
- 测试与开发环境：测试项目、`start_dev.sh`

## 2026-04-01

### 问题修复
- **SQL 注入安全修复**：修复 `SurveyFeedback.GetClassScore` 与 `Soft.cs` 中的 SQL 注入风险，统一改为更安全的参数化查询方式

### 升级增强
- **核心查询与批处理性能优化**：对成绩统计、自动分配、排序、列表拼接和批量更新等逻辑进行集中优化，减少 N+1 查询、字符串分配和逐条数据库写入带来的开销
- **基础代码清理与重构**：清理冗余构造函数、注释代码和多处重复实现，重构 SQL 脚本解析与资源释放逻辑，提升底层代码可维护性

### 涉及文件
- 安全修复：`SurveyFeedback`、`Soft.cs`
- 性能优化：`TopicReply`、`Students`、`Computers`、`TurtleQuestion`、`Courses`、`SoftCategory`、`TxtFormBack`
- 基础重构：`SharpZip.cs`、`UpdateStscore`、`DbLinkEdit`、`SqlHelper`、`SurveyQuestion` 及多个工具类

## 2026-03-31

### 升级增强
- **常规业务更新**：同步罗老师的常规功能与业务调整

## 2026-03-27

### 新功能
- **项目源码基线切换**：以周老师 `LearnSiteCode2026-1-27` 源码为基础继续开展后续改造

### 升级增强
- **工程兼容性升级**：批量统一项目文件编码为 UTF-8，并更新 `web.config` 以支持 .NET 4.8 运行环境

### 涉及文件
- 项目配置与编码：全站项目文件、`web.config`

## 2026-03-01

### 升级增强
- **上游版本同步**：同步 `2026-1-5` 版本的更新内容，为后续改造提供统一基础

## 2026-02-26

### 新功能
- **学案模板补充**：新增来源于 `openlearnsite.com` 的学案模板资源

### 升级增强
- **项目结构与文档整理**：完成项目结构梳理，并更新 `README` 说明文档

### 涉及文件
- 项目结构与文档：目录结构、`README`
- 模板资源：学案模板相关文件

## 2026-02-25

### 新功能
- **Docker 环境配置补充**：新增 `web.config.docker`，为容器化部署与开发环境提供独立配置

### 升级增强
- **项目初始化整理**：统一全站文件编码为 UTF-8，调整目录结构，并同步优化项目说明文档

### 涉及文件
- 配置文件：`web.config.docker`
- 工程整理：全站文件编码、目录结构、项目说明文档

## 2026-01-10

### 新功能
- **源码版本入库**：导入 LearnSite 信息学习平台 `2025-12-30` 版源码，以及 `LearnSiteCode2026-1-5` 版源码

### 涉及文件
- 基础源码：LearnSite 平台初始代码与对应资源文件

## 2025-11-01

### 新功能
- **新课标能力增强**：补充更多符合 `2025-09-19` 新课标要求的功能内容

## 2024-09-14

### 新功能
- **项目初始化**：完成项目初始提交（Initial commit）
