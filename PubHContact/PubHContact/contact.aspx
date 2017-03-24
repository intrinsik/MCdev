<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="PubHContact.contact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="True" DataSourceID="xmlDs" DataTextField="topic" DataValueField="topic" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged"/>
        <asp:XmlDataSource ID="xmlDs" runat="server" DataFile="~/TopicsCategories.xml"/><br/><br/>

        <asp:Label runat="server" ID="lblEmailtobe">mailto:</asp:Label><br/><br/>
        <asp:Button runat="server" ID="btnSend" Text="Send" />
    </div>
    </form>
</body>
</html>
