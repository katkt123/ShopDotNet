<%@ Page Async="true" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="OpenAI.aspx.cs" Inherits="Foodie.User.OpenAI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>OpenAI Integration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>OpenAI Assistant</h2>
    <asp:TextBox ID="txtUserInput" runat="server" Width="400px" Placeholder="Nhập câu hỏi của bạndasadaas"></asp:TextBox>
    <asp:Button ID="btnCallOpenAI" runat="server" Text="Gửi yêu cầu" OnClick="btnCallOpenAI_Click" />
    <br /><br />
    <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Green"></asp:Label>
</asp:Content>
