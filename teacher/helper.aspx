<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="helper.aspx.cs" Inherits="Teacher_helper" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <div class="placehold bg-slate-50 min-h-full px-4 py-6 md:px-6 md:py-8">
        <div class="mx-auto max-w-6xl space-y-6">
            <section class="rounded-xl bg-gradient-to-r from-blue-600 to-blue-500 px-6 py-6 text-white shadow-sm">
                <div class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                    <div>
                        <div class="text-xs font-semibold uppercase tracking-[0.18em] text-blue-100">Help Center</div>
                        <h1 class="mt-2 text-2xl font-bold tracking-tight">帮助中心</h1>
                        <p class="mt-2 max-w-2xl text-sm leading-6 text-blue-50">查看平台使用说明、常用支持渠道，以及教师端可直接访问的静态工具入口。</p>
                    </div>
                    <div class="rounded-xl border border-white/20 bg-white/10 px-4 py-3 text-sm text-blue-50 backdrop-blur-sm">
                        建议优先阅读说明文档，再通过站点与群组获取支持。
                    </div>
                </div>
            </section>

            <div class="grid grid-cols-1 gap-6 lg:grid-cols-2">
                <section class="rounded-xl border border-slate-200 bg-white shadow-sm overflow-hidden">
                    <div class="border-b border-slate-100 px-6 py-4">
                        <div class="flex items-center gap-3">
                            <span class="inline-flex h-10 w-10 items-center justify-center rounded-lg bg-blue-50">
                                <img alt="" src="../images/book.gif" class="h-5 w-5 object-contain" />
                            </span>
                            <div>
                                <h2 class="text-lg font-semibold text-slate-900">帮助说明</h2>
                                <p class="text-sm text-slate-500">平台安装、使用与支持方式</p>
                            </div>
                        </div>
                    </div>
                    <div class="px-6 py-5">
                        <div class="space-y-4 text-sm leading-7 text-slate-600">
                            <div class="rounded-lg border border-slate-200 bg-slate-50 px-4 py-3">1. 安装和使用请先仔细阅读说明必读目录中的相关资料。</div>
                            <div class="rounded-lg border border-slate-200 bg-slate-50 px-4 py-3">2. LearnSite 学习平台 QQ 群：5847120。</div>
                            <div class="rounded-lg border border-slate-200 bg-slate-50 px-4 py-3">3. LearnSite 帮助网站：<a href="http://www.openlearnsite.com" target="_blank" class="font-medium text-blue-600 hover:text-blue-700">www.openlearnsite.com</a>（上海 倪老师）。</div>
                        </div>
                    </div>
                </section>

                <section class="rounded-xl border border-slate-200 bg-white shadow-sm overflow-hidden">
                    <div class="border-b border-slate-100 px-6 py-4">
                        <div class="flex items-center gap-3">
                            <span class="inline-flex h-10 w-10 items-center justify-center rounded-lg bg-emerald-50">
                                <img alt="" src="../images/find.gif" class="h-5 w-5 object-contain" />
                            </span>
                            <div>
                                <h2 class="text-lg font-semibold text-slate-900">友情链接</h2>
                                <p class="text-sm text-slate-500">推荐教学辅助资源</p>
                            </div>
                        </div>
                    </div>
                    <div class="px-6 py-5">
                        <div class="rounded-lg border border-slate-200 bg-slate-50 px-4 py-4 text-sm leading-7 text-slate-600">
                            ITtools3 信息技术教学辅助平台（温岭 陈老师）<br />
                            QQ 群号：176809529
                        </div>
                    </div>
                </section>
            </div>

            <section class="rounded-xl border border-slate-200 bg-white shadow-sm overflow-hidden">
                <div class="border-b border-slate-100 px-6 py-4">
                    <div class="flex items-center gap-3">
                        <span class="inline-flex h-10 w-10 items-center justify-center rounded-lg bg-amber-50">
                            <img alt="" src="../images/lock.png" class="h-5 w-5 object-contain" />
                        </span>
                        <div>
                            <h2 class="text-lg font-semibold text-slate-900">静态链接</h2>
                            <p class="text-sm text-slate-500">常用教学工具快速入口</p>
                        </div>
                    </div>
                </div>
                <div class="px-6 py-6">
                    <div class="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
                        <a href="../scratch/index.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/program.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">积木编程</span>
                            </div>
                        </a>
                        <a href="../mxgraph/index.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/mxgraph.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">流程图</span>
                            </div>
                        </a>
                        <a href="../blockly/index.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/vote.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">Blockly 游戏</span>
                            </div>
                        </a>
                        <a href="../pixelartmaker/index.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/pixel.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">像素画</span>
                            </div>
                        </a>
                        <a href="../python/index.aspx" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/python.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">Python 绘图编程</span>
                            </div>
                        </a>
                        <a href="../plugins/excalidraw/index.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/excalidraw.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">手绘画布</span>
                            </div>
                        </a>
                        <a href="../luckysheetbottle/index.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/excel.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">在线协作表格</span>
                            </div>
                        </a>
                        <a href="../plugins/km/test.html" target="_blank" class="group rounded-xl border border-slate-200 bg-slate-50 px-4 py-4 transition-all hover:border-blue-200 hover:bg-blue-50/60">
                            <div class="flex items-center gap-3">
                                <img src="../images/kitymind.png" alt="" class="h-5 w-5 object-contain" />
                                <span class="text-sm font-semibold text-slate-800 group-hover:text-blue-700">在线思维导图</span>
                            </div>
                        </a>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
