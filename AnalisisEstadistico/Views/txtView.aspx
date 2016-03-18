<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="txtView.aspx.cs" Inherits="AnalisisEstadistico.txtView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:FileUpload ID="fileReader" runat="server" />
        <p>
            <asp:Button ID="buttonUpload" runat="server" OnClick="buttonUpload_Click" Text="Upload" />
        </p>
        <asp:Label ID="message" runat="server"></asp:Label>
    </form>
</body>
</html>
