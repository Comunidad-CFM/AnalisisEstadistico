<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="AnalisisEstadistico.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Análisis estadístico</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Hola desde el proyecto</h1>
    </div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Saludar" />
        <p>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </p>
        <asp:FileUpload ID="fileReader" runat="server" />
            <asp:Button ID="buttonUpload" runat="server" OnClick="buttonUpload_Click" Text="Upload" />
        &nbsp;&nbsp;
        <p>
            <asp:TextBox ID="textLink" runat="server"></asp:TextBox>
&nbsp;&nbsp;
            <asp:Button ID="buttonCargar" runat="server" OnClick="buttonCargar_Click" Text="Cargar" />
        </p>
        <asp:Label ID="message" runat="server"></asp:Label>
    </form>
</body>
</html>
