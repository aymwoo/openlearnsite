<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="helper.aspx.cs" Inherits="Teacher_helper" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/helper.css" />

    <div class="placehold helper-page">
        <div class="lesson-shell">
            <section class="lesson-hero">
                <div class="lesson-hero__content">
                    <div class="lesson-hero__intro">
                        <span class="lesson-hero__eyebrow">Help Center</span>
                        <h1 class="lesson-hero__title">帮助中心</h1>
                        <p class="lesson-hero__subtitle">查看平台使用说明、常用支持渠道，以及教师端可以直接访问的静态教学工具入口。</p>
                    </div>
                    <div class="lesson-hero-panel">
                        <div class="lesson-hero-panel__label">使用建议</div>
                        <div class="lesson-hero-panel__text">建议优先阅读说明文档，再通过站点与群组获取支持，能够更快定位安装、使用和课堂准备中的常见问题。</div>
                    </div>
                </div>
            </section>

            <div class="helper-grid">
                <section class="lesson-card lesson-theme--blue">
                    <div class="lesson-card__head">
                        <div class="helper-section-head">
                            <span class="helper-section-icon helper-section-icon--blue">
                                <img alt="" src="../images/book.gif" class="helper-section-icon__img" />
                            </span>
                            <div>
                                <h2 class="lesson-card__title">帮助说明</h2>
                                <p class="lesson-card__desc">平台安装、使用与常见支持方式</p>
                            </div>
                        </div>
                    </div>
                    <div class="lesson-card__body">
                        <div class="helper-notice-list">
                            <div class="helper-notice-item">1. 安装和使用请先仔细阅读说明必读目录中的相关资料。</div>
                            <div class="helper-notice-item">2. LearnSite 学习平台 QQ 群：5847120。</div>
                            <div class="helper-notice-item">3. LearnSite 帮助网站：<a href="http://www.openlearnsite.com" target="_blank" class="helper-inline-link">www.openlearnsite.com</a>（上海 倪老师）。</div>
                        </div>
                    </div>
                </section>

                <section class="lesson-card lesson-theme--teal">
                    <div class="lesson-card__head">
                        <div class="helper-section-head">
                            <span class="helper-section-icon helper-section-icon--teal">
                                <img alt="" src="../images/find.gif" class="helper-section-icon__img" />
                            </span>
                            <div>
                                <h2 class="lesson-card__title">友情链接</h2>
                                <p class="lesson-card__desc">推荐教学辅助资源与交流渠道</p>
                            </div>
                        </div>
                    </div>
                    <div class="lesson-card__body">
                        <div class="helper-linkout-card">
                            <div class="helper-linkout-title">ITtools3 信息技术教学辅助平台</div>
                            <div class="helper-linkout-meta">温岭 陈老师</div>
                            <div class="helper-linkout-note">QQ 群号：176809529</div>
                        </div>
                    </div>
                </section>
            </div>

            <section class="lesson-card lesson-theme--amber">
                <div class="lesson-card__head">
                    <div class="helper-section-head">
                        <span class="helper-section-icon helper-section-icon--amber">
                            <img alt="" src="../images/lock.png" class="helper-section-icon__img" />
                        </span>
                        <div>
                            <h2 class="lesson-card__title">静态链接</h2>
                            <p class="lesson-card__desc">常用教学工具快速入口</p>
                        </div>
                    </div>
                </div>
                <div class="lesson-card__body">
                    <div class="helper-tool-grid">
                        <a href="../scratch/index.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/program.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">积木编程</span>
                            </div>
                        </a>
                        <a href="../mxgraph/index.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/mxgraph.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">流程图</span>
                            </div>
                        </a>
                        <a href="../blockly/index.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/vote.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">Blockly 游戏</span>
                            </div>
                        </a>
                        <a href="../pixelartmaker/index.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/pixel.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">像素画</span>
                            </div>
                        </a>
                        <a href="../python/index.aspx" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/python.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">Python 绘图编程</span>
                            </div>
                        </a>
                        <a href="../plugins/excalidraw/index.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/excalidraw.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">手绘画布</span>
                            </div>
                        </a>
                        <a href="../luckysheetbottle/index.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/excel.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">在线协作表格</span>
                            </div>
                        </a>
                        <a href="../plugins/km/test.html" target="_blank" class="helper-tool-link">
                            <div class="helper-tool-link__content">
                                <img src="../images/kitymind.png" alt="" class="helper-tool-link__icon" />
                                <span class="helper-tool-link__title">在线思维导图</span>
                            </div>
                        </a>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
