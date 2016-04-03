<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="AnalisisEstadistico.index" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Análisis estadístico</title>
    <link rel="stylesheet" type="text/css" href="./node_modules/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="./css/ubuntu.css" />
    <link rel="stylesheet" type="text/css" href="./css/styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-default" style="background-color: #2196F3;">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-ex1-collapse">
                    <span class="sr-only">Desplegar navegación</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" style="color: black;">Clasificación y análisis estadístico</a>
            </div>
            <div class="collapse navbar-collapse navbar-ex1-collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" style="color: black;">Leer <b class="caret"></b></a>
                        <ul class="dropdown-menu">
                            <asp:Button class="btn btn-nav" runat="server" OnClick="click_readLink" Text="URL" />
                            <asp:Button class="btn btn-nav" runat="server" OnClick="click_readFolder" Text="Carpeta" />
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" style="color: black;">Descomprimir <b class="caret"></b></a>
                        <ul class="dropdown-menu">
                            <asp:Button class="btn btn-nav" runat="server" OnClick="click_unpackageZIP" Text="ZIP" />
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" style="color: black;">Cargar <b class="caret"></b></a>
                        <ul class="dropdown-menu">
                            <asp:Button class="btn btn-nav" runat="server" OnClick="click_readHtmlJsonXmlTxt" Text="HTML/JSON/XML/TXT" />
                            <asp:Button class="btn btn-nav" runat="server" OnClick="click_readDoc" Text="DOC" />
                            <asp:Button class="btn btn-nav" runat="server" OnClick="click_searchTweets" Text="Twitter" />
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"><span class="glyphicon glyphicon-cog rotate"></span></a>
                        <ul class="dropdown-menu">
                            <asp:Button class="btn btn-nav" runat="server" OnClick="sentimentAnalysis" Text="Análisis del sentimiento" />
                            <asp:Button class="btn btn-nav" runat="server" OnClick="languageAnalysis" Text="Análisis de lenguaje" />
                            <asp:Button class="btn btn-nav" runat="server" OnClick="tweetsAnalysis" Text="Analizar tweets" />
                            <asp:Button class="btn btn-nav" runat="server" OnClick="cleanTextArea" Text="Limpiar" />
                        </ul>
                    </li>
                </ul>
            </div>
        </nav>
        <div class="container content">
            <div class="row col-md-12">
                <div class="col-md-2">
                    <label><span class="label label-default">Archivos</span></label>
                    <div>
                        <span class="btn btn-default btn-file">
                            <span class="glyphicon glyphicon-folder-open"></span>&nbsp;
    				        Elegir archivo
                            <asp:FileUpload ID="fileReader" runat="server" />
                        </span>
                    </div>
                </div>
                <div class="col-md-3">
                    <label><span class="label label-default">URL | Carpeta</span></label>
                    <div>
                        <asp:TextBox class="form-control" ID="textLinkFolder" runat="server" placeholder="Url or path"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label><span class="label label-default">Twitter</span></label>
                    <div>
                        <asp:TextBox class="form-control" ID="textTwitter" runat="server" placeholder="Username tweets"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12 results-box">
                <div class="col-md-6 nopadding">
                    <div>
                        <label>Contenido previo</label>
                    </div>
                    <asp:TextBox class="textArea" ID="contentBox" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="col-md-6 nopadding">
                    <div>
                        <label>Resultado del análisis</label>
                    </div>
                    <asp:TextBox class="textArea" ID="resultBox" runat="server" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="container">
            <asp:Chart ID="textChart" runat="server" Width="555px" Visible="False">
                <series>
                    <asp:Series Name="Letras">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
                <Titles>
                    <asp:Title Text="Porcentaje de letras en texto" />
                </Titles>
            </asp:Chart>
            <asp:Chart ID="langChart" runat="server" Width="555px" Visible="False">
                <series>
                    <asp:Series Name="Letras">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea2">
                    </asp:ChartArea>
                </chartareas>
                <Titles>
                   <asp:Title Text="Porcentaje de letras en el idioma encontrado" />
                </Titles>
            </asp:Chart>
            <asp:Chart ID="sentimentPercentChart" runat="server" Visible="False" Width="555px">
                <series>
                    <asp:Series Name="resultados">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
                <Titles>
                   <asp:Title Text="Porcentaje de las palabras en el texto" />
                </Titles>
            </asp:Chart>
            <asp:Chart ID="sentimentScoresChart" runat="server" Visible="False" Width="555px">
                <series>
                    <asp:Series Name="Series1">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
                <Titles>
                   <asp:Title Text="Puntaje de las palabras" />
                </Titles>
            </asp:Chart>
        </div>
    </form>

        <script type="text/javascript" src="./node_modules/jquery/dist/jquery.min.js"></script>
        <script type="text/javascript" src="./node_modules/bootstrap/dist/js/bootstrap.min.js"></script>
    </body>
</html>
