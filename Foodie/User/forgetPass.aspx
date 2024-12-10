<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="forgetPass.aspx.cs" Inherits="Foodie.User.forgetPass" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section layout_padding">  
        <div class="container">  
            <div class="heading_container">  
                <div class="align-self-end">  
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>  
                </div>  
            </div>  
            <asp:Label ID="lblHeaderMsg" runat="server" Text="<h2>User Registration</h2>"></asp:Label>  
            <div class="row">
                <div class="col-md-6">  
                    <div class="form_container">   
                        <div>  
                            
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required" ControlToValidate="txtEmailForgot" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>  
                            <asp:TextBox ID="txtEmailForgot" runat="server" CssClass="form-control" placeholder="Enter Email" ToolTip="Email"></asp:TextBox>  
                        </div>
                    </div>  
                </div>    

                <div class="row pl-4">  
                    <div class="btn_box">  
                        <asp:Button ID="btnForgot" runat="server" Text="Register" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white" OnClick="btn_click"
                            />
                    </div>  
                </div>

                <div class="row p-5">  
                    <div style="align-items:center">  
                        <asp:Image ID="imgUser" runat="server" CssClass="img-thumbnail" />  
                    </div>  
                </div>  
            </div>  
        </div>  
    </section> 
</asp:Content>
