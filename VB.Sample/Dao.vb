Imports System
Imports System.Core
Imports System.Xml
Imports System.Xml.Linq
Imports TypeSql
Imports System.Data
Imports System.Collections.Generic

Partial Public Class CustomerLastNameByIdQueryResult
    Private _LastName As String
    Public Property LastName As String
        Get
            Return _LastName
        End Get
        Private Set(value As String)
            _LastName = value
        End Set
    End Property
End Class

Public Interface ICustomerLastNameByIdQuery
    Function Execute(Optional id As Integer = Nothing, Optional buffered As Boolean = True) As IEnumerable(Of CustomerLastNameByIdQueryResult)
End Interface

Partial Public Class CustomerLastNameByIdQuery
    Inherits DapperDao(Of CustomerLastNameByIdQueryResult)
    Implements ICustomerLastNameByIdQuery

    Public Sub New(connectionString As String)
        MyBase.New(connectionString)
    End Sub

    Public Sub New(connection As IDbConnection, Optional transaction As IDbTransaction = Nothing)
        MyBase.New(connection, transaction)
    End Sub

    Public Overloads Function Execute(Optional id As Integer = Nothing, Optional buffered As Boolean = True) As IEnumerable(Of CustomerLastNameByIdQueryResult) Implements ICustomerLastNameByIdQuery.Execute
        Dim parameters = New With {.id = id}
        Return MyBase.Execute(parameters, buffered)
    End Function

    Protected Overrides ReadOnly Property Sql As String
        Get
            Return <string><![CDATA[SELECT [LastName] FROM SalesLT.Customer WHERE CustomerID = @id]]></string>
        End Get
    End Property
End Class
