Imports System.IO

Module Utility

    Public Function IsGenericIOException(ex As Exception) As Boolean
        Return TypeOf ex Is UnauthorizedAccessException OrElse
            TypeOf ex Is IOException OrElse
            TypeOf ex Is NotSupportedException
    End Function
End Module
