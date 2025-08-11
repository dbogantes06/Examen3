Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack AndAlso Session("UserName") IsNot Nothing Then
            Response.Redirect("Clientes.aspx", False)
            Return
        End If
    End Sub

    Protected Sub BtnLoginClick(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim u As String = txtUser.Text.Trim()
        Dim p As String = txtPass.Text.Trim()

        If String.IsNullOrWhiteSpace(u) OrElse String.IsNullOrWhiteSpace(p) Then
            lblError.Text = "Usuario y contraseña son obligatorios."
            Exit Sub
        End If

        Try
            If AuthRepository.ValidateUser(u, p) Then
                Session("UserName") = u
                Response.Redirect("Clientes.aspx", False)
            Else
                lblError.Text = "Credenciales inválidas."
            End If
        Catch ex As Exception
            lblError.Text = "Error: " & ex.Message
        End Try
    End Sub
End Class
