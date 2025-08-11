Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class DatabaseHelper

    ' Cambia si tu connectionString tiene otro nombre en Web.config
    Private Const ConnectionStringName As String = "Login"


    Shared Sub New()
        Try
            EnsureErrorLogTableExists()
        Catch

        End Try
    End Sub

    Public Shared Function GetConnection() As SqlConnection
        Dim cs = ConfigurationManager.ConnectionStrings(ConnectionStringName)
        If cs Is Nothing OrElse String.IsNullOrEmpty(cs.ConnectionString) Then
            Throw New InvalidOperationException(
                "No se encontró la cadena de conexión '" & ConnectionStringName & "' en Web.config.")
        End If
        Return New SqlConnection(cs.ConnectionString)
    End Function

    Public Shared Function ExecuteNonQuery(sql As String,
                                           Optional parameters As IEnumerable(Of SqlParameter) = Nothing,
                                           Optional isStoredProcedure As Boolean = False) As Boolean
        Using cn = GetConnection()
            Using cmd As New SqlCommand(sql, cn)
                For Each p As SqlParameter In SafeParams(parameters)
                    If p IsNot Nothing Then cmd.Parameters.Add(p)
                Next

                If isStoredProcedure Then
                    cmd.CommandType = CommandType.StoredProcedure
                End If

                Try
                    cn.Open()
                    Dim affected As Integer = cmd.ExecuteNonQuery()
                    Return affected > 0
                Catch ex As Exception

                    LogError(ex, sql)
                    Throw New Exception("Error al ejecutar el comando: " & ex.Message)
                End Try
            End Using
        End Using
    End Function


    Public Shared Function ExecuteQuery(query As String,
                                        Optional parameters As IEnumerable(Of SqlParameter) = Nothing,
                                        Optional isStoredProcedure As Boolean = False) As DataTable
        If String.IsNullOrWhiteSpace(query) Then
            Throw New ArgumentException("La consulta no puede estar vacía.")
        End If

        Dim dt As New DataTable()
        Using cn = GetConnection()
            Using cmd As New SqlCommand(query, cn)
                For Each p As SqlParameter In SafeParams(parameters)
                    If p IsNot Nothing Then cmd.Parameters.Add(p)
                Next

                If isStoredProcedure Then
                    cmd.CommandType = CommandType.StoredProcedure
                End If

                Try
                    cn.Open()
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                Catch ex As Exception

                    LogError(ex, query)
                    Throw New Exception("Error al ejecutar la consulta: " & ex.Message)
                End Try
            End Using
        End Using

        Return dt
    End Function

    Private Shared Sub EnsureErrorLogTableExists()
        Dim sql As String =
"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ErrorLog')
BEGIN
    CREATE TABLE ErrorLog (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ErrorMessage NVARCHAR(4000),
        ErrorSeverity INT,
        ErrorState INT,
        ErrorProcedure NVARCHAR(200),
        ErrorLine INT,
        ErrorDateTime DATETIME DEFAULT GETDATE()
    )
END"
        Try
            Using cn = GetConnection()
                Using cmd As New SqlCommand(sql, cn)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch

        End Try
    End Sub

    Private Shared Sub LogErrorToFile(message As String)
        Try
            Dim logPath As String = "C:\Logs\error_log.txt"
            Dim dir = Path.GetDirectoryName(logPath)
            If Not String.IsNullOrEmpty(dir) AndAlso Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If
            File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}")
        Catch

        End Try
    End Sub

    Private Shared Sub LogError(ex As Exception, Optional query As String = "")
        Dim fullMessage As String =
            "Message: " & ex.Message & Environment.NewLine &
            "StackTrace: " & If(ex.StackTrace, "") & Environment.NewLine &
            "InnerException: " & If(If(ex.InnerException IsNot Nothing, ex.InnerException.Message, Nothing), "N/A") & Environment.NewLine &
            "Query: " & query

        Dim severity As Integer = 16
        Dim state As Integer = 1
        Dim procedureName As String = If(ex.TargetSite IsNot Nothing, ex.TargetSite.Name, "")
        Dim lineNumber As Integer = 0


        If ex.StackTrace IsNot Nothing Then
            Dim marker As String = ":line "
            Dim idx As Integer = ex.StackTrace.LastIndexOf(marker, StringComparison.Ordinal)
            If idx >= 0 Then
                Dim after As String = ex.StackTrace.Substring(idx + marker.Length)
                Dim digits As String = ""
                For Each ch As Char In after
                    If Char.IsDigit(ch) Then
                        digits &= ch
                    Else
                        Exit For
                    End If
                Next
                Integer.TryParse(digits, lineNumber)
            End If
        End If

        Try
            Using cn = GetConnection()
                Using cmd As New SqlCommand("
INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorDateTime)
VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, GETDATE())", cn)

                    cmd.Parameters.AddWithValue("@ErrorMessage", fullMessage)
                    cmd.Parameters.AddWithValue("@ErrorSeverity", severity)
                    cmd.Parameters.AddWithValue("@ErrorState", state)
                    cmd.Parameters.AddWithValue("@ErrorProcedure", procedureName)
                    cmd.Parameters.AddWithValue("@ErrorLine", lineNumber)

                    cn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch

            LogErrorToFile(fullMessage)
        End Try
    End Sub


    Private Shared Function SafeParams(parameters As IEnumerable(Of SqlParameter)) As IEnumerable(Of SqlParameter)
        If parameters Is Nothing Then
            Return New List(Of SqlParameter)()
        End If
        Return parameters
    End Function

    Public Shared Function CreateParameter(name As String, value As Object) As SqlParameter
        Return New SqlParameter(name, If(value IsNot Nothing, value, DBNull.Value))
    End Function

End Class
