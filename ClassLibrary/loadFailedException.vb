Public Class LoadFailedException
    Inherits Exception

    Public Sub New(message As String, actualException As Exception)
        MyBase.New(message, actualException)
    End Sub
End Class
