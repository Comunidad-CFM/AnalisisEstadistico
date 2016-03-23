<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="AnalisisEstadistico.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <title>Análisis estadístico</title>
    <link rel="stylesheet" type="text/css" href="./node_modules/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="./css/ubuntu.css"/>
    <link rel="stylesheet" type="text/css" href="./css/styles.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <label>Clasificación y análisis estadístico</label>
        </div>
        <div class="container content">
            <div class="row col-md-12">
                <div class="col-md-3">
                    <label><span class="label label-default">Archivos</span></label>
                    <div>
                        <span class="btn btn-default btn-file">
	            	        <span class="glyphicon glyphicon-folder-open"></span>&nbsp;
    				        Elegir archivo <asp:FileUpload ID="fileReader" runat="server" />
				        </span>
                        <asp:Button class="btn btn-primary" ID="button1" runat="server" OnClick="buttonUpload_Click" Text="Cargar"/>
                    </div>
                </div>
                <div class="col-md-3">
                    <label><span class="label label-default">Web</span></label>
                    <div>
                        <div class="input-group">
                          <asp:TextBox class="form-control" ID="textLink" runat="server" placeholder="Url"></asp:TextBox>
                          <span class="input-group-btn">
                            <asp:Button class="btn btn-primary" ID="buttonCargar" runat="server" OnClick="buttonCargar_Click" Text="Cargar" />
                          </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <label><span class="label label-default">Twitter</span></label>
                    <div>
                        <div class="input-group">
                          <asp:TextBox class="form-control" ID="textTwitter" runat="server" placeholder="Nombre de usuario"></asp:TextBox>
                          <span class="input-group-btn">
                            <asp:Button class="btn btn-primary" ID="buttonTwitter" runat="server" OnClick="searchTweets" Text="Cargar" />
                          </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <label><span class="label label-default">Realizar análisis</span></label>
                    <div>
                        <asp:Button class="btn btn-success" ID="buttonAnalizar" runat="server" OnClick="buttonAnalizar_Click" Text="Analizar"/>
                    </div>
                </div>
            </div>
            <div class="col-md-12 results-box">
                <div class="col-md-6 nopadding">
                    <div>
                        <label>Contenido previo</label>
                    </div>
                    <asp:TextBox class="textArea" ID="contentBox" runat="server" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="col-md-6 nopadding">
                    <div>
                        <label>Resultado del análisis</label>
                    </div>
                    <asp:TextBox class="textArea" ID="resultBox" runat="server" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
