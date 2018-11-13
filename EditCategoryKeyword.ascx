<%@ Control language="C#" Inherits="Shift8Read.Dnn.Consumer.EditCategoryKeyword" AutoEventWireup="false"  Codebehind="EditCategoryKeyword.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<asp:Panel ID="pnlUrlEdit" runat="server">
    <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>

    <div class="CategoryKeywordEdit Wrapper">
        <div class="label">
            <dnn:Label ID="lblKeyword" Runat="server" CssClass="Normal" ControlName="txtName"></dnn:Label>
        </div>
        <div class="property">
            <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="CategoryKeywordEdit Wrapper">
        <div class="label">
            <dnn:Label ID="lblCategoryId" Runat="server" CssClass="Normal" ControlName="txtUrl"></dnn:Label>
        </div>
        <div class="property">
            <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="ItemId"></asp:DropDownList>
        </div>
    </div>
        
    <asp:LinkButton ID="lbSubmit" runat="server" resourcekey="lbSubmit" 
        onclick="lbSubmit_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbDelete" runat="server" resourcekey="lbDelete" 
        onclick="lbDelete_Click"></asp:LinkButton>
</asp:Panel>
<asp:Panel ID="pnlMessage" runat="server">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</asp:Panel>