<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reset.aspx.cs" Inherits="SITConnect.Reset" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="formLogin">
        <asp:Label ID="lbLogin" runat="server" Text="Password Reset"></asp:Label>
        <asp:Label ID="lbLoginError" runat="server" ForeColor="#E50000" Visible="False"></asp:Label>
        <div class="formFieldGrp">
            <asp:Label ID="lbNewPass" runat="server" Text="New Password"></asp:Label>
            <asp:TextBox ID="tbNewPassword" runat="server" TextMode="Password" CssClass="tbFormField" required="true" minlength="12" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{12,}$" oninvalid="this.setCustomValidity('Password Complexity not met')" oninput="setCustomValidity('')"></asp:TextBox>
        </div>
        <div class="passComplexCheckFrame2">
            <div class="passComplexCheck">
                <strong>Your password must use:</strong><br />
                <i class="fas fa-times passwordLength passwordNotMet"></i> 12 characters or more<br />
                <i class="fas fa-times passwordUppercase passwordNotMet"></i> an UPPERCASE character<br />
                <i class="fas fa-times passwordLowercase passwordNotMet"></i> a lowercase character<br />
                <i class="fas fa-times passwordDigit passwordNotMet"></i> a digit<br />
                <i class="fas fa-times passwordSpecial passwordNotMet"></i> a symbol<br />
            </div>
        </div>
        <asp:Button ID="btnReset" runat="server" OnClientClick="event.preventDefault();checkResetPassInputs();" Text="Reset" />
    </div>
</asp:Content>
