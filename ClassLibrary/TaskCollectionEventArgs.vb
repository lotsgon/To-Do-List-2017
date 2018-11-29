Public Class TaskCollectionEventArgs
    Inherits EventArgs

    Public Sub New(type As TaskCollectionEventType, task As Task())
        Me.Tasks = task
        Me.Type = type
    End Sub

    Public Sub New(type As TaskCollectionEventType, task As Task(), index As Integer)
        Me.New(type, task)
        Me.Index = index
    End Sub

    Public ReadOnly Property Type As TaskCollectionEventType
    Public ReadOnly Property Tasks As Task()
    Public ReadOnly Property Index As Integer

End Class
