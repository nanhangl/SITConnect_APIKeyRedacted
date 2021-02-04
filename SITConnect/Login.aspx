<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render=6LepnAMaAAAAACish3N8GX9MRuVOhmVU_GnGBgdF"></script>
    <div class="formLogin">
        <asp:Label ID="lbLogin" runat="server" Text="Member Login"></asp:Label>
        <asp:Label ID="lbLoginError" runat="server" ForeColor="#E50000" Visible="False"></asp:Label>
        <div class="formFieldGrp">
            <asp:Label ID="Label3" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="tbLoginEmail" runat="server" TextMode="Email" CssClass="tbFormField" required="true" MaxLength="50"></asp:TextBox>
        </div>
        <div class="formFieldGrp">
            <div><asp:Label ID="Label4" runat="server" Text="Password"></asp:Label><asp:HyperLink ID="hlForgotPassword" runat="server" NavigateUrl="javascript:forgotPassword()" style="float:right;">Forgot Password?</asp:HyperLink></div>
            <asp:TextBox ID="tbLoginPassword" runat="server" TextMode="Password" CssClass="tbFormField" required="true"></asp:TextBox>
        </div>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" />
    </div>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LepnAMaAAAAACish3N8GX9MRuVOhmVU_GnGBgdF', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });

        function forgotPassword() {
            var email = document.querySelector("#MainContent_tbLoginEmail").value;
            location.href = `/Login?forgotPass=true&email=${email}`
        }
    </script>
</asp:Content>
