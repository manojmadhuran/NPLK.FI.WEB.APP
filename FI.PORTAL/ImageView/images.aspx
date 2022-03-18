<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="images.aspx.cs" Inherits="FI.PORTAL.images" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <link rel="stylesheet" href="../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../Content/dist/css/adminlte.css" />
</head>
<body style="background-color:#eeeeee">
    <form id="form1" runat="server">
        <div class="row">
            
        </div>
        <div class="row" style="margin:10px;">
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <asp:Image ID="image1" runat="server" ImageUrl="~/Content/dist/img/404.png" Width="100%" Height="100%" />
                    </div>
                    <div class="card-footer">
                        <asp:Button ID="btndownload1" runat="server"  Text="Download" CssClass="btn btn-success" Font-Size="Medium" OnClick="btndownload1_Click"/>
                    </div>
                </div>
                
            </div>
            <div class="col-md-4">
                  <div class="card">
                    <div class="card-body">
                        <asp:Image ID="image2" runat="server" ImageUrl="~/Content/dist/img/404.png" Width="100%" Height="100%" />
                    </div>
                    <div class="card-footer">
                        <asp:Button ID="btndownload2" runat="server"  Text="Download" CssClass="btn btn-success" Font-Size="Medium" OnClick="btndownload2_Click"/>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                  <div class="card">
                    <div class="card-body">
                        <asp:Image ID="image3" runat="server" ImageUrl="~/Content/dist/img/404.png" Width="100%" Height="100%" />
                    </div>
                    <div class="card-footer">
                        <asp:Button ID="btndownload3" runat="server"  Text="Download" CssClass="btn btn-success" Font-Size="Medium" OnClick="btndownload3_Click"/>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
