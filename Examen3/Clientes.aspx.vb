Imports System.Text.RegularExpressions

Partial Class Clientes
    Inherits System.Web.UI.Page

    Private ReadOnly _repo As New ClienteRepository()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserName") Is Nothing Then
            Response.Redirect("Login.aspx", False)
            Return
        End If

        lblUser.Text = "Usuario: " & Session("UserName").ToString()

        If Not IsPostBack Then
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        gvClientes.DataSource = _repo.GetAll()
        gvClientes.DataBind()
    End Sub

    Private Function EmailValido(email As String) As Boolean
        Return Regex.IsMatch(email, "^[^@\s]+@[^@\s]+\.[^@\s]+$")
    End Function

    Protected Sub BtnGuardarClick(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Page.Validate() : If Not Page.IsValid Then Exit Sub

        If Not EmailValido(txtEmail.Text.Trim()) Then
            lblMsg.Text = "Email inválido." : Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtNombre.Text) _
           OrElse String.IsNullOrWhiteSpace(txtApellido.Text) _
           OrElse String.IsNullOrWhiteSpace(txtTelefono.Text) Then
            lblMsg.Text = "Nombre, Apellido y Teléfono son obligatorios." : Exit Sub
        End If

        Try
            Dim c As New Cliente With {
                .Nombre = txtNombre.Text.Trim(),
                .Apellido = txtApellido.Text.Trim(),
                .Email = txtEmail.Text.Trim(),
                .Telefono = txtTelefono.Text.Trim()
            }

            Dim id As Integer
            Dim esUpdate As Boolean = Integer.TryParse(txtId.Text, id) AndAlso id > 0

            If esUpdate Then
                c.ClienteId = id
                Dim ok = _repo.Update(c)
                lblMsg.ForeColor = If(ok, Drawing.Color.Green, Drawing.Color.Red)
                lblMsg.Text = If(ok, "Cliente actualizado.", "No se actualizó ningún registro.")
            Else
                Dim newId = _repo.Insert(c)
                txtId.Text = newId.ToString()
                lblMsg.ForeColor = Drawing.Color.Green
                lblMsg.Text = "Cliente agregado con ID " & newId
            End If

            BindGrid()
        Catch ex As Exception
            lblMsg.ForeColor = Drawing.Color.Red
            lblMsg.Text = "Error al guardar: " & ex.Message
        End Try
    End Sub

    Protected Sub BtnCancelarClick(sender As Object, e As EventArgs) Handles btnCancelar.Click
        txtId.Text = ""
        txtNombre.Text = ""
        txtApellido.Text = ""
        txtEmail.Text = ""
        txtTelefono.Text = ""
        gvClientes.SelectedIndex = -1
        lblMsg.Text = ""
    End Sub

    Protected Sub BtnLogoutClick(sender As Object, e As EventArgs) Handles btnLogout.Click
        Session.Clear()
        Response.Redirect("Login.aspx", False)
    End Sub

    Protected Sub GvClientesSelectedIndexChanged(sender As Object, e As EventArgs) Handles gvClientes.SelectedIndexChanged
        Try
            If gvClientes.SelectedDataKey Is Nothing Then Exit Sub

            Dim id As Integer
            If Not Integer.TryParse(Convert.ToString(gvClientes.SelectedDataKey.Value), id) Then Exit Sub

            Dim c As Cliente = _repo.GetById(id)
            If c Is Nothing Then Exit Sub

            txtId.Text = c.ClienteId.ToString()
            txtNombre.Text = c.Nombre
            txtApellido.Text = c.Apellido
            txtEmail.Text = c.Email
            txtTelefono.Text = c.Telefono
        Catch ex As Exception
            lblMsg.ForeColor = Drawing.Color.Red
            lblMsg.Text = "Error al seleccionar: " & ex.Message
        End Try
    End Sub

    Protected Sub GvClientesRowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvClientes.RowDeleting
        Try
            If gvClientes.DataKeys Is Nothing OrElse gvClientes.DataKeys(e.RowIndex) Is Nothing Then
                e.Cancel = True : Exit Sub
            End If

            Dim id As Integer
            If Not Integer.TryParse(Convert.ToString(gvClientes.DataKeys(e.RowIndex).Value), id) Then
                lblMsg.Text = "ID inválido."
                e.Cancel = True : Exit Sub
            End If

            Dim ok = _repo.Delete(id)
            lblMsg.ForeColor = If(ok, Drawing.Color.Green, Drawing.Color.Red)
            lblMsg.Text = If(ok, "Cliente eliminado.", "No se eliminó ningún registro.")
            BindGrid()
            BtnCancelarClick(Nothing, EventArgs.Empty)
        Catch ex As Exception
            lblMsg.ForeColor = Drawing.Color.Red
            lblMsg.Text = "Error al eliminar: " & ex.Message
        End Try
        e.Cancel = True
    End Sub
End Class
