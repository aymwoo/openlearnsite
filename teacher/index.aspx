<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Teacher_index" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        #MenuDiv { display: none !important; }

        .teacher-login-btn {
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
            width: 100%;
            padding: 0.85rem 1.5rem;
            border: none;
            border-radius: 0.375rem;
            background: linear-gradient(135deg, #4f46e5 0%, #4338ca 50%, #6366f1 100%);
            background-size: 200% 200%;
            color: #ffffff;
            font-size: 0.95rem;
            font-weight: 700;
            letter-spacing: 0.04em;
            cursor: pointer;
            box-shadow:
                0 4px 14px -3px rgba(79, 70, 229, 0.55),
                0 1px 3px rgba(0, 0, 0, 0.08),
                inset 0 1px 0 rgba(255, 255, 255, 0.15);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            overflow: hidden;
            animation: teacher-btn-bg 4s ease infinite;
        }

        @keyframes teacher-btn-bg {
            0%, 100% { background-position: 0% 50%; }
            50% { background-position: 100% 50%; }
        }

        .teacher-login-btn:hover {
            transform: translateY(-2px);
            box-shadow:
                0 8px 25px -4px rgba(79, 70, 229, 0.6),
                0 4px 10px rgba(0, 0, 0, 0.08),
                inset 0 1px 0 rgba(255, 255, 255, 0.2);
        }

        .teacher-login-btn:active {
            transform: translateY(0);
            box-shadow:
                0 2px 8px -2px rgba(79, 70, 229, 0.5),
                inset 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .teacher-login-btn:focus-visible {
            outline: none;
            box-shadow:
                0 0 0 3px rgba(99, 102, 241, 0.35),
                0 4px 14px -3px rgba(79, 70, 229, 0.55);
        }

        .teacher-login-btn::before {
            content: "";
            position: absolute;
            top: 0;
            left: -100%;
            width: 60%;
            height: 100%;
            background: linear-gradient(
                90deg,
                transparent 0%,
                rgba(255, 255, 255, 0.15) 50%,
                transparent 100%
            );
            transform: skewX(-20deg);
            animation: teacher-btn-shine 3.5s ease-in-out infinite;
        }

        @keyframes teacher-btn-shine {
            0%, 100% { left: -100%; }
            50% { left: 120%; }
        }

        @keyframes teacher-btn-shine {
            0%, 100% { left: -100%; }
            50% { left: 120%; }
        }

        .teacher-login-card {
            background: #ffffff;
            padding: 2.5rem;
            border-radius: 1.25rem;
            width: clamp(20rem, 90vw, 28rem);
            border: 1px solid rgba(148, 163, 184, 0.18);
            box-shadow:
                0 20px 60px -20px rgba(15, 23, 42, 0.15),
                0 0 0 1px rgba(255, 255, 255, 0.8) inset;
        }

        .teacher-login-card input[type="text"],
        .teacher-login-card input[type="password"] {
            width: 100% !important;
            box-sizing: border-box;
        }

        @media (max-width: 480px) {
            .teacher-login-card {
                padding: 1.5rem;
                border-radius: 1rem;
                width: calc(100vw - 2rem);
            }
        }

        .teacher-login-divider {
            display: flex;
            align-items: center;
            gap: 0.75rem;
            margin: 1.5rem 0 0.25rem;
            color: #94a3b8;
            font-size: 0.78rem;
            font-weight: 500;
        }

        .teacher-login-divider::before,
        .teacher-login-divider::after {
            content: "";
            flex: 1;
            height: 1px;
            background: linear-gradient(90deg, transparent, #e2e8f0, transparent);
        }
    </style>
    <div class="flex items-center justify-center min-h-[calc(100vh-16rem)] py-12 px-4 sm:px-6 lg:px-8">
        <div class="teacher-login-card">
            <div class="text-center mb-8">
                <h2 class="phead text-3xl font-extrabold text-slate-900 tracking-tight bg-transparent h-auto">教师登录</h2>
                <p class="mt-2 text-sm text-slate-600">欢迎来到信息科技教学平台</p>
            </div>
            <div class="space-y-6">
                <div>
                    <label for="Textname" class="block text-sm font-medium text-slate-700 mb-1">账号</label>
                    <asp:TextBox ID="Textname" runat="server" SkinID="TextBoxNormal" CssClass="w-full px-4 py-3 border border-slate-300 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all duration-300 shadow-sm text-slate-900 placeholder-slate-400"></asp:TextBox>
                </div>
                <div>
                    <label for="Textpwd" class="block text-sm font-medium text-slate-700 mb-1">密码</label>
                    <asp:TextBox ID="Textpwd" runat="server" TextMode="Password" SkinID="TextBoxNormal" CssClass="w-full px-4 py-3 border border-slate-300 rounded-xl focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all duration-300 shadow-sm text-slate-900 placeholder-slate-400"></asp:TextBox>
                </div>
                <div class="min-h-[24px]">
                    <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed" CssClass="text-sm text-red-600 font-medium block text-center"></asp:Label>
                </div>
                <div class="teacher-login-divider">安全登录</div>
                <div>
                    <asp:Button ID="Btnlogin" runat="server" onclick="Btnlogin_Click" CssClass="teacher-login-btn" Text="安全登录" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

