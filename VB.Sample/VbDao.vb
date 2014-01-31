Imports System
Imports TypeSql
Imports System.Data
Imports System.Collections.Generic

Partial Public Class VbDao
    Inherits DapperDao(Of Result)


    Public Sub New(connectionString As String)
        MyBase.New(connectionString)
    End Sub

    Public Sub New(connection As IDbConnection, Optional transaction As IDbTransaction = Nothing)
        MyBase.New(connection, transaction)
    End Sub

    Public Overloads Function Execute(Optional blah As String = "", Optional buffered As Boolean = True) As IEnumerable(Of Result)
        Dim parameters = New With {.blah = ""}
        Return MyBase.Execute(parameters, buffered)
    End Function

    Protected Overrides ReadOnly Property Sql As String
        Get

        End Get
    End Property
End Class