Public Class TaskEventArgs
    Inherits EventArgs

    Public Sub New(type As TaskEventType, task As Task)
        Me.Task = task
        Me.Type = type
    End Sub

    Public ReadOnly Property Type As TaskEventType
    Public ReadOnly Property Task As Task

End Class
