<%@ Control language="C#" Inherits="Shift8Read.Dnn.Consumer.ViewConsumerCategoryKeyword" AutoEventWireup="false"  Codebehind="ViewConsumerCategoryKeyword.ascx.cs" %>
<div id="Shift8ReadConsumeUrls" class="Normal">
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
                    <asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Id") %>' Text='<%# DataBinder.Eval(Container.DataItem,"Keyword") %>' Target="_blank"/>
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