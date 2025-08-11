Imports System.Data.SqlClient

Public Class UsuarioRepository
    Public Function ValidateUser(userName As String, password As String) As Boolean
        Using cn = DatabaseHelper.GetConnection(),
              cmd = New SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE UserName=@u AND [Password]=@p", cn)
            cmd.Parameters.AddWithValue("@u", userName)
            cmd.Parameters.AddWithValue("@p", password)
            cn.Open()
            Return Convert.ToInt32(cmd.ExecuteScalar()) = 1
        End Using
    End Function
End Class
