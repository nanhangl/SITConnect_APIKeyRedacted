<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SITConnect._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>SITConnect</h1>
        <p class="lead">The best stationary store in SIT!</p>
        <p><a href="/Login" class="btn btn-primary btn-lg homeHeroBtn">Login to view profile &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Who we are</h2>
            <p>
                SITConnect is a stationary store that provide allow staff and students to purchase stationaries.
            </p>
            <p>
                <a class="btn btn-default" href="https://www.nyp.edu.sg">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>What is this project about</h2>
            <p>
                SITConnect would like to engage your service to develop an online web application to allow staff and students to purchase stationary online.
            </p>
            <p>
                <a class="btn btn-default" href="https://www.nyp.edu.sg">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Who is the developer of this website</h2>
            <p>
                Lim Nan Hang 190704D
            </p>
            <p>
                <a class="btn btn-default" href="https://www.nyp.edu.sg">Learn more &raquo;</a>
            </p>
        </div>
    </div>
    <script>
        if (document.cookie.includes("loggedIn=true")) {
            document.querySelector(".homeHeroBtn").href = "/Profile";
            document.querySelector(".homeHeroBtn").innerText = "View Profile";
        }
    </script>
</asp:Content>
