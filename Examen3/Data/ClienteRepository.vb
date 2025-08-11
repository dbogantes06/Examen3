Imports System.Data
Imports System.Data.SqlClient

Public Class ClienteRepository

    ' LISTA para el Grid
    Public Function GetAll() As DataTable
        Dim dt As New DataTable()
        Using cn = DatabaseHelper.GetConnection()
            Using da As New SqlDataAdapter("
                SELECT ClienteId, Nombre, Apellido, Email, Telefono
                FROM Clientes
                ORDER BY ClienteId DESC", cn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    ' OBTENER por Id
    Public Function GetById(id As Integer) As Cliente
        Using cn = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
                SELECT ClienteId, Nombre, Apellido, Email, Telefono
                FROM Clientes
                WHERE ClienteId = @id", cn)
                cmd.Parameters.AddWithValue("@id", id)
                cn.Open()
                Using rd = cmd.ExecuteReader()
                    If Not rd.Read() Then Return Nothing

                    Dim c As New Cliente With {
                        .ClienteId = Convert.ToInt32(rd("ClienteId")),
                        .Nombre = Convert.ToString(rd("Nombre")),
                        .Apellido = Convert.ToString(rd("Apellido")),
                        .Email = Convert.ToString(rd("Email")),
                        .Telefono = Convert.ToString(rd("Telefono"))
                    }
                    Return c
                End Using
            End Using
        End Using
    End Function

    ' INSERT (devuelve nuevo Id)
    Public Function Insert(c As Cliente) As Integer
        Using cn = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
                INSERT INTO Clientes (Nombre, Apellido, Email, Telefono)
                OUTPUT INSERTED.ClienteId
                VALUES (@Nombre, @Apellido, @Email, @Telefono)", cn)
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre)
                cmd.Parameters.AddWithValue("@Apellido", c.Apellido)
                cmd.Parameters.AddWithValue("@Email", c.Email)
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono)
                cn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    ' UPDATE (True si toco 1 fila)
    Public Function Update(c As Cliente) As Boolean
        Using cn = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
                UPDATE Clientes
                SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email, Telefono=@Telefono
                WHERE ClienteId=@Id", cn)
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre)
                cmd.Parameters.AddWithValue("@Apellido", c.Apellido)
                cmd.Parameters.AddWithValue("@Email", c.Email)
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono)
                cmd.Parameters.AddWithValue("@Id", c.ClienteId)
                cn.Open()
                Return cmd.ExecuteNonQuery() = 1
            End Using
        End Using
    End Function

    ' DELETE (True si borro 1)
    Public Function Delete(id As Integer) As Boolean
        Using cn = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("DELETE FROM Clientes WHERE ClienteId=@Id", cn)
                cmd.Parameters.AddWithValue("@Id", id)
                cn.Open()
                Return cmd.ExecuteNonQuery() = 1
            End Using
        End Using
    End Function

End Class

