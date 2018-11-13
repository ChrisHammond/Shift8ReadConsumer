<%@ Control Language="C#" AutoEventWireup="false" Inherits="Shift8Read.Dnn.Consumer.Settings" Codebehind="Settings.ascx.cs" %>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td><asp:Label ID="lblModuleId" runat="server" resourcekey="lblModuleId"></asp:Label></td>
        <td><asp:DropDownList ID="ddlModules" runat="server" ></asp:DropDownList></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCategory" runat="server" resourcekey="lblCategory"></asp:Label></td>
        <td><asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="ItemId"></asp:DropDownList></td>
    </tr>
</table>