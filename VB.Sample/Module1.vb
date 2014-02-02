Module Module1

    Sub Main()
        Dim query As New CustomerLastNameByIdQuery("AdventureWorks")
        Dim result = query.Execute(11).Single()
    End Sub

End Module
