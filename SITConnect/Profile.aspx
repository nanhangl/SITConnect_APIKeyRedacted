<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SITConnect.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="changePasswordBackground">
        <div id="changePasswordModal">
            <div>
                <asp:Label ID="Label4" runat="server" Text="Change Password" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                <a href="javascript:closeChangePasswordModal();" id="closeChangePasswordModal">x</a>
            </div>
            <div class="formFieldGrp">
                <asp:Label ID="Label2" runat="server" Text="Current Password"></asp:Label>
                <asp:TextBox ID="tbCurrentPassword" runat="server" CssClass="tbFormField" required="true" TextMode="Password"></asp:TextBox>
            </div>
            <div class="formFieldGrp">
                <asp:Label ID="Label3" runat="server" Text="New Password"></asp:Label>
                <asp:TextBox ID="tbNewPassword" runat="server" CssClass="tbFormField" required="true" minlength="12" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{12,}$" oninvalid="this.setCustomValidity('Password Complexity not met')" oninput="setCustomValidity('')" TextMode="Password"></asp:TextBox>
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
            <asp:Button ID="btnChangePasswordChange" runat="server" OnClick="btnChangePasswordChange_Click" Text="Change" />
        </div>
    </div>
    <br />
    <div></div>
    <div id="profileError" class="alert alert-dismissible" role="alert" hidden>
        <span id="profileErrorText"></span>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <asp:Label ID="Label1" runat="server" Text="Profile" Font-Size="XX-Large" Font-Bold="True"></asp:Label>
    <br />
    <asp:Table ID="Table1" runat="server">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Font-Bold="True">First Name:</asp:TableCell>
            <asp:TableCell runat="server" ID="lbProfileFirstName"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Last Name:</asp:TableCell>
            <asp:TableCell runat="server" ID="lbProfileLastName"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Email:</asp:TableCell>
            <asp:TableCell runat="server" ID="lbProfileEmail"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Date of Birth:</asp:TableCell>
            <asp:TableCell runat="server" ID="lbProfileDateOfBirth"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Credit Card (Encrypted):</asp:TableCell>
            <asp:TableCell runat="server" ID="lbProfileCreditCardEncrypted"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Credit Card (Decrypted):</asp:TableCell>
            <asp:TableCell runat="server" ID="lbProfileCreditCardDecrypted"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClientClick="event.preventDefault();" />
    <span id="changePasswordNotice" runat="server" visible="false"></span>
    <script>
        document.getElementById("MainContent_btnChangePassword").setAttribute("onclick", "event.preventDefault();showChangePasswordModal();");
        if (document.cookie.includes("passwordChange=wrongPassword")) {
            document.getElementById("profileError").style.display = "block";
            document.getElementById("profileError").classList.add("alert-danger");
            document.getElementById("profileErrorText").innerText = "Your current password is incorrect.";
            document.cookie = "passwordChange=";
        } else if (document.cookie.includes("passwordChange=success")) {
            document.getElementById("profileError").style.display = "block";
            document.getElementById("profileError").classList.add("alert-success");
            document.getElementById("profileErrorText").innerHTML = "Your password has been successfully changed. <a href=\"/Profile\">Refresh</a> to view your details.";
            document.cookie = "passwordChange=";
        } else if (document.cookie.includes("passwordChange=notcomplex")) {
            document.getElementById("profileError").style.display = "block";
            document.getElementById("profileError").classList.add("alert-danger");
            document.getElementById("profileErrorText").innerText = "Your new password does not meet complexity requirements.";
            document.cookie = "passwordChange=";
        } else if (document.cookie.includes("passwordChange=previousPass")) {
            document.getElementById("profileError").style.display = "block";
            document.getElementById("profileError").classList.add("alert-danger");
            document.getElementById("profileErrorText").innerText = "You cannot use your previous passwords as your new password.";
            document.cookie = "passwordChange=";
        } else if (document.cookie.includes("passwordChange=minPassAge")) {
            document.getElementById("profileError").style.display = "block";
            document.getElementById("profileError").classList.add("alert-danger");
            document.getElementById("profileErrorText").innerText = "Minimum password age of 5 mins not passed yet.";
            document.cookie = "passwordChange=";
        }

    </script>
</asp:Content>
