<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" AutoEventWireup="true" StylesheetTheme="Student"  CodeFile="description.aspx.cs" Inherits="Student_description" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
<div class="w-full max-w-5xl mx-auto space-y-6">
    <!-- Title Card -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 sm:p-10 overflow-hidden">
        <div class="course-node-head text-center border-b border-slate-100" style="padding:24px 24px 28px;margin:-24px -24px 0;">
            <asp:Label ID="LabelMtitle" runat="server" CssClass="course-node-title text-2xl sm:text-3xl font-extrabold text-slate-800 tracking-tight"></asp:Label>
        </div>
        
        <link href="../kindeditor/plugins/syntaxhighlighter/styles/shCore.css" rel="stylesheet" type="text/css" />
        <link href="../kindeditor/plugins/syntaxhighlighter/styles/shThemeRDark.css" rel="stylesheet" type="text/css" />
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shCore.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushCss.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushJScript.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushVb.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushCSharp.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushCpp.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushPython.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushPhp.js" type="text/javascript"></script>
        <script src="../kindeditor/plugins/syntaxhighlighter/scripts/shBrushXml.js" type="text/javascript"></script>
        <script type="text/javascript">SyntaxHighlighter.all();</script>
        
        <!-- Content Area -->
        <div id="Mcontent" class="mt-8 text-slate-700 leading-loose text-lg" style="word-wrap:break-word; word-break:break-word;" runat="server">	
        </div>
        
        <!-- Star Rating Section -->
        <div class="mt-8 pt-6 border-t border-slate-100">
            <div class="flex flex-col items-center gap-4">
                <div class="rating-text">
                    <span class="text-sm font-semibold text-slate-500 mr-2">学习评价</span>
                    <span class="star" data-value="1">★</span>
                    <span class="star" data-value="2">★</span>
                    <span class="star" data-value="3">★</span>
                    <span class="star" data-value="4">★</span>
                    <span class="star" data-value="5">★</span>
                </div>
                
                <asp:Button ID="Btnread" runat="server" onclick="Btnread_Click" Text="确定" 
                    ToolTip="选择后评价" Enabled="False"
                    CssClass="px-8 py-2.5 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md border-0 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed" />
                
                <input id="TextBoxStar" name="TextBoxStar" type="hidden" value="0"/>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    const stars = document.querySelectorAll('.star');
    var currentRating = 0;
    const mystar ="<%= mystar %>";

    stars.forEach(star => {
        star.addEventListener('click', setRating);
        star.addEventListener('mouseover', addHover);
        star.addEventListener('mouseout', removeHover);
    });

    
    function initRating() {
        if(mystar>0){
            stars.forEach(star => {
                const starValue = parseInt(star.dataset.value);
                star.classList.toggle('active', starValue <= mystar);
            });       
        }    
    }
    initRating();

    function setRating(e) {
        const value = parseInt(e.target.dataset.value);
        currentRating =value;
        stars.forEach(star => {
            const starValue = parseInt(star.dataset.value);
            star.classList.toggle('active', starValue <= value);
        });

        document.getElementById("TextBoxStar").value = value;
        setbar();             
    }

    function addHover(e) {
        if(mystar == 0){
            const hoverValue = parseInt(e.target.dataset.value);
            stars.forEach(star => {
                const starValue = parseInt(star.dataset.value);
                star.classList.toggle('active', starValue <= hoverValue);
            });
        }
    }

    function removeHover() {        
        if(mystar == 0){
            stars.forEach(star => {
                const starValue = parseInt(star.dataset.value);
                star.classList.toggle('active', starValue <= currentRating);
            });
        }
    }

    function setbar() {
        var btnid = "<%= Btnread.ClientID %>";
        if (document.getElementById(btnid) != null) {
             document.getElementById(btnid).disabled = false;
        }
    }
        
</script>
</asp:Content>
