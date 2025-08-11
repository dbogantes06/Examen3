Imports System.Data.SqlClient

Public Class AuthRepository
    Public Shared Function ValidateUser(userName As String, password As String) As Boolean
        If String.IsNullOrWhiteSpace(userName) OrElse String.IsNullOrWhiteSpace(password) Then
            Return False
        End If

        Using cn = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE UserName=@u AND [Password]=@p", cn)
                cmd.Parameters.AddWithValue("@u", userName)
                cmd.Parameters.AddWithValue("@p", password)
                cn.Open()
                Dim n As Integer = CInt(cmd.ExecuteScalar())
                Return n > 0
            End Using
        End Using
    End Function
End Class
