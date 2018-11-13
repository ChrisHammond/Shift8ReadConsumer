<%@ Control Language="C#" Inherits="Shift8Read.Dnn.Consumer.EditConsumerUrl" AutoEventWireup="false"
    CodeBehind="EditConsumerUrl.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<asp:Panel ID="pnlUrlEdit" runat="server">
    <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblLastChecked" runat="server" Visible="false"></asp:Label>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label ID="lblName" runat="server" CssClass="Normal" ControlName="txtName" />
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblUrl" runat="server" CssClass="Normal" ControlName="txtUrl" />
            <asp:TextBox ID="txtUrl" runat="server"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblBaseWebsiteUrl" runat="server" CssClass="Normal" ControlName="txtBaseWebsiteUrl" />
            
            <asp:TextBox ID="txtBaseWebsiteUrl" runat="server" />
        </div>

        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
            <div class="dnnFormItem">
                <dnn:label ID="lblUserId" runat="server" CssClass="Normal" ControlName="txtUserId" />
                
                <asp:TextBox ID="txtUserId" runat="server"></asp:TextBox>
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblApproved" runat="server" CssClass="Normal" ControlName="chkApproved" />
                
                <asp:CheckBox ID="chkApproved" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblAutoApproved" runat="server" CssClass="Normal" ControlName="chkAutoApproved" />
                <asp:CheckBox ID="chkAutoApproved" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblCategoryId" runat="server" CssClass="Normal" ControlName="ddlCategory" />
                <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="ItemId" />
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblStripHTML" runat="server" CssClass="Normal" ControlName="chkStripHTML" />
                
                <asp:CheckBox ID="chkStripHTML" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblDefaultThumbnail" runat="server" CssClass="Normal" ControlName="txtDefaultThumbnail" />
                <asp:TextBox ID="txtDefaultThumbnail" runat="server" />
            </div>
        </asp:Panel>

        <asp:LinkButton ID="lbSubmit" runat="server" resourcekey="lbSubmit" OnClick="lbSubmit_Click" CssClass="dnnPrimaryAction" />
        <asp:LinkButton ID="lbDelete" runat="server" resourcekey="lbDelete" OnClick="lbDelete_Click" CssClass="dnnSecondaryAction" />
        </fieldset>
</asp:Panel>
<asp:Panel ID="pnlMessage" runat="server">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</asp:Panel>
