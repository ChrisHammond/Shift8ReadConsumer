<%@ Control language="C#" Inherits="Shift8Read.Dnn.Consumer.ViewConsumerUrls" AutoEventWireup="false"  Codebehind="ViewConsumerUrls.ascx.cs" %>
<div id="Shift8ReadConsumeUrls" class="Normal">
    <asp:RadioButtonList ID="rblApproved" runat="server" 
        onselectedindexchanged="rblApproved_SelectedIndexChanged" AutoPostBack="true" CssClass="Normal">
        <asp:ListItem text="Approved" Value="true" Selected="true" />
        <asp:ListItem text="Not Approved" Value="false" />
    </asp:RadioButtonList>
    <asp:GridView ID="gvConsumerUrls" runat="server"
            EnableViewState="true" 
            AlternatingRowStyle-CssClass="DataGrid_AlternatingItem"                
            HeaderStyle-CssClass="DataGrid_Header"
            RowStyle-CssClass="DataGrid_Item"
            PagerStyle-CssClass="Normal"
            CssClass="Normal" 
            AutoGenerateColumns="false" >
        <Columns>
            <asp:TemplateField ShowHeader="true"  HeaderText="Link" SortExpression="Name">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Url") %>' Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' Target="_blank"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="true"  HeaderText="Date Created" SortExpression="DateCreated">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DateCreated")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="true"  HeaderText="Last Updated" SortExpression="LastUpdated">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "LastUpdated")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="true"  HeaderText="Last Checked" SortExpression="LastChecked">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "LastChecked")%>
                </ItemTemplate>
            </asp:TemplateField>



            <asp:TemplateField ShowHeader="true"  HeaderText="Approved" SortExpression="Approved">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem,"Approved") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="true"  HeaderText="Auto Approved" SortExpression="AutoApproved">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "AutoApproved")%>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="true"  HeaderText="Category ID" SortExpression="CategoryId">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "CategoryId")%>
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField ShowHeader="false"  HeaderText="Edit">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# BuildEditUrl(DataBinder.Eval(Container.DataItem,"Id")) %>' Text="edit" /> 
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="lnkAddUrl" resourcekey="lnkAddUrl" runat="server"></asp:HyperLink>
</div>