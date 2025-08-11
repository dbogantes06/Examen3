Imports System.Data
Imports System.Data.SqlClient

Public Class ClienteRepository
    Public Function GetAll() As DataTable
        Dim dt As New DataTable()
        Using cn = DatabaseHelper.GetConnection(),
              da As New SqlDataAdapter("SELECT ClienteId,Nombre,Apellido1,Apellido2,Email,Telefono FROM Clientes ORDER BY ClienteId DESC", cn)
            da.Fill(dt)
        End Using
        Return dt
    End Function

    Public Function GetById(id As Integer) As Cliente
        Dim c As Cliente = Nothing
        Using cn = DatabaseHelper.GetConnection(),
              cmd = New SqlCommand("SELECT ClienteId,Nombre,Apellido1,Apellido2,Email,Telefono FROM Clientes WHERE ClienteId=@id", cn)
            cmd.Parameters.AddWithValue("@id", id)
            cn.Open()
            Using rd = cmd.ExecuteReader()
                If rd.Read() Then
                    c = New Cliente() With {
                        .ClienteId = CInt(rd("ClienteId")),
                        .Nombre = rd("Nombre").ToString(),
                        .Apellido = rd("Apellido1").ToString(),
                        .Email = rd("Email").ToString(),
                        .Telefono = rd("Telefono").ToString()
                    }
                End If
            End Using
        End Using
        Return c
    End Function

    Public Function Insert(c As Cliente) As Integer
        Using cn = DatabaseHelper.GetConnection(),
              cmd = New SqlCommand("
                INSERT INTO Clientes(Nombre,Apellido1,Apellido2,Email,Telefono)
                VALUES(@Nombre,@Apellido1,@Apellido2,@Email,@Telefono);
                SELECT SCOPE_IDENTITY();", cn)
            cmd.Parameters.AddWithValue("@Nombre", c.Nombre)
            cmd.Parameters.AddWithValue("@Apellido1", c.Apellido)
            cmd.Parameters.AddWithValue("@Email", c.Email)
            cmd.Parameters.AddWithValue("@Telefono", c.Telefono)
            cn.Open()
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    Public Function Update(c As Cliente) As Boolean
        Using cn = DatabaseHelper.GetConnection(),
              cmd = New SqlCommand("
                UPDATE Clientes
                   SET Nombre=@Nombre, Apellido1=@Apellido1, Apellido2=@Apellido2, Email=@Email, Telefono=@Telefono
                 WHERE ClienteId=@Id;", cn)
            cmd.Parameters.AddWithValue("@Nombre", c.Nombre)
            cmd.Parameters.AddWithValue("@Apellido1", c.Apellido)
            cmd.Parameters.AddWithValue("@Email", c.Email)
            cmd.Parameters.AddWithValue("@Telefono", c.Telefono)
            cmd.Parameters.AddWithValue("@Id", c.ClienteId)
            cn.Open()
            Return cmd.ExecuteNonQuery() = 1
        End Using
    End Function

    Public Function Delete(id As Integer) As Boolean
        Using cn = DatabaseHelper.GetConnection(),
              cmd = New SqlCommand("DELETE FROM Clientes WHERE ClienteId=@Id", cn)
            cmd.Parameters.AddWithValue("@Id", id)
            cn.Open()
            Return cmd.ExecuteNonQuery() = 1
        End Using
    End Function
End Class
