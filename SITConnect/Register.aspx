<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SITConnect.Register" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render=6LepnAMaAAAAACish3N8GX9MRuVOhmVU_GnGBgdF"></script>
    <div class="formRegistration">
        <asp:Label ID="lbMemberRegistration" runat="server" Text="Member Registration"></asp:Label>
        <asp:Label ID="lbRegistrationError" runat="server" ForeColor="#E50000" Visible="False"></asp:Label>
        <div class="formFieldGrp">
            <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
            <asp:TextBox ID="tbFirstName" runat="server" CssClass="tbFormField" pattern="^[A-Za-z0-9\s]+$" oninvalid="if (this.value.length == 0) {this.setCustomValidity('First Name is empty')} else if (/[^A-Za-z0-9\s]/.test(this.value)) {this.setCustomValidity('First Name cannot contain symbols')} else {this.setCustomValidity('')};" oninput="setCustomValidity('')" required="true" MaxLength="50"></asp:TextBox>
        </div>
        <div class="formFieldGrp">
            <asp:Label ID="Label2" runat="server" Text="Last Name"></asp:Label>
            <asp:TextBox ID="tbLastName" runat="server" CssClass="tbFormField" pattern="^[A-Za-z0-9\s]+$" oninvalid="if (this.value.length == 0) {this.setCustomValidity('Last Name is empty')} else if (/[^A-Za-z0-9\s]/.test(this.value)) {this.setCustomValidity('Last Name cannot contain symbols')} else {this.setCustomValidity('')};" oninput="setCustomValidity('')" required="true" MaxLength="50"></asp:TextBox>
        </div>
        <div class="formFieldGrp">
            <asp:Label ID="Label3" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="tbEmail" runat="server" TextMode="Email" CssClass="tbFormField" required="true" MaxLength="50"></asp:TextBox>
        </div>
        <div class="passComplexCheckFrame">
            <div class="passComplexCheck">
                <strong>Your password must use:</strong><br />
                <i class="fas fa-times passwordLength passwordNotMet"></i> 12 characters or more<br />
                <i class="fas fa-times passwordUppercase passwordNotMet"></i> an UPPERCASE character<br />
                <i class="fas fa-times passwordLowercase passwordNotMet"></i> a lowercase character<br />
                <i class="fas fa-times passwordDigit passwordNotMet"></i> a digit<br />
                <i class="fas fa-times passwordSpecial passwordNotMet"></i> a symbol<br />
            </div>
        </div>
        <div class="formFieldGrp">
            <asp:Label ID="Label4" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" CssClass="tbFormField" required="true" minlength="12" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{12,}$" oninvalid="this.setCustomValidity('Password Complexity not met')" oninput="setCustomValidity('')"></asp:TextBox>
        </div>
        <div class="formFieldGrp">
            <asp:Label ID="Label5" runat="server" Text="Date of Birth"></asp:Label>
            <asp:TextBox ID="tbDateOfBirth" runat="server" TextMode="Date" CssClass="tbFormField" required="true"></asp:TextBox>
        </div>
        <asp:Label ID="lbCreditCard" runat="server" Text="Credit Card"></asp:Label>
        <div class="formFieldGrp">
            <asp:Label ID="lbCardNumber" runat="server" Text="Card Number"></asp:Label>
            <div class="cardNumberGrp">
                <asp:TextBox ID="tbCardNumber1" runat="server" TextMode="Number" CssClass="tbCardNumber" onkeyup="cardNumberValidate(1)" pattern="[0-9]{4}" required="true"></asp:TextBox>
                <asp:TextBox ID="tbCardNumber2" runat="server" TextMode="Number" CssClass="tbCardNumber" onkeyup="cardNumberValidate(2)" pattern="[0-9]{4}" required="true"></asp:TextBox>
                <asp:TextBox ID="tbCardNumber3" runat="server" TextMode="Number" CssClass="tbCardNumber" onkeyup="cardNumberValidate(3)" pattern="[0-9]{4}" required="true"></asp:TextBox>
                <asp:TextBox ID="tbCardNumber4" runat="server" TextMode="Number" CssClass="tbCardNumber" onkeyup="cardNumberValidate(4)" pattern="[0-9]{4}" required="true"></asp:TextBox>
            </div>
        </div>
        <div class="formFieldGrp">
            <asp:Label ID="Label8" runat="server" Text="Expiry"></asp:Label>
            <asp:TextBox ID="tbExpiry" runat="server" TextMode="Month" CssClass="tbFormField" required="true"></asp:TextBox>
        </div>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        <asp:Button ID="btnRegister" runat="server" Text="Register" />
    </div>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LepnAMaAAAAACish3N8GX9MRuVOhmVU_GnGBgdF', { action: 'Register' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</asp:Content>
