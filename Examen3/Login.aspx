<%@ Page Language="vb" AutoEventWireup="false"
    CodeBehind="Login.aspx.vb"
    Inherits="Examen3.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"><title>Login</title></head>
<body>
<form id="form1" runat="server" style="max-width:420px;margin:40px auto;font-family:Segoe UI,Arial">
  <h2>Iniciar sesión</h2>

  <asp:ValidationSummary ID="vs" runat="server" HeaderText="Corrige:" />

  <div style="margin:6px 0">
    <asp:Label runat="server" AssociatedControlID="txtUser" Text="Usuario (email o username)"></asp:Label><br />
    <asp:TextBox ID="txtUser" runat="server" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUser" ErrorMessage="Usuario requerido" Display="Dynamic" />
  </div>

  <div style="margin:6px 0">
    <asp:Label runat="server" AssociatedControlID="txtPass" Text="Contraseña"></asp:Label><br />
    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass" ErrorMessage="Contraseña requerida" Display="Dynamic" />
  </div>

  <div style="margin:10px 0">
    <asp:Button ID="btnLogin" runat="server" Text="Entrar" />
  </div>

  <asp:Label ID="lblError" runat="server" ForeColor="Red" />
</form>
</body>
</html>
