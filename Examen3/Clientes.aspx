<%@ Page Language="vb" AutoEventWireup="false"
    CodeBehind="Clientes.aspx.vb"
    Inherits="Examen3.Clientes" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"><title>Clientes CRUD</title></head>
<body>
<form id="form1" runat="server">
  <div style="max-width:980px;margin:20px auto;font-family:Segoe UI, Arial">
    <div style="display:flex;justify-content:space-between;align-items:center">
      <h2>Clientes</h2>
      <div>
        <asp:Label ID="lblUser" runat="server" />
        &nbsp;<asp:Button ID="btnLogout" runat="server" Text="Cerrar sesión" />
      </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Por favor corrige:" />

    <div style="display:grid;grid-template-columns:1fr 1fr;gap:12px;margin:12px 0;border:1px solid #eee;padding:12px;border-radius:8px">
      <!-- ID solo lectura -->
      <div>
        <asp:Label AssociatedControlID="txtId" runat="server" Text="ID" /><br />
        <asp:TextBox ID="txtId" runat="server" ReadOnly="true" />
      </div>

      <div>
        <asp:Label AssociatedControlID="txtNombre" runat="server" Text="Nombre" /><br />
        <asp:TextBox ID="txtNombre" runat="server" />
        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
            ErrorMessage="Nombre requerido" Display="Dynamic" />
      </div>

      <div>
        <asp:Label AssociatedControlID="txtApellido" runat="server" Text="Apellido" /><br />
        <asp:TextBox ID="txtApellido" runat="server" />
        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
            ErrorMessage="Apellido requerido" Display="Dynamic" />
      </div>

      <div>
        <asp:Label AssociatedControlID="txtEmail" runat="server" Text="Email" /><br />
        <asp:TextBox ID="txtEmail" runat="server" />
        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
            ErrorMessage="Email requerido" Display="Dynamic" />
        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
            ErrorMessage="Email inválido" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" Display="Dynamic" />
      </div>

      <div>
        <asp:Label AssociatedControlID="txtTelefono" runat="server" Text="Teléfono" /><br />
        <asp:TextBox ID="txtTelefono" runat="server" />
        <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
            ErrorMessage="Teléfono requerido" Display="Dynamic" />
      </div>
    </div>

    <div style="margin:6px 0">
      <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
      <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="False"
        DataKeyNames="ClienteId">
      <Columns>
        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
        <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" />
        <asp:CommandField ShowDeleteButton="True" DeleteText="Eliminar" />
      </Columns>
    </asp:GridView>
  </div>
</form>
</body>
</html>
