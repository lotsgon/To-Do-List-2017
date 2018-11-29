Public Class DesignTimeViewModel

    Public Shared ReadOnly sInstance As New DesignTimeViewModel()

    Private Sub New()

        Me.Tasks = New DesignTimeTaskViewModel() {
            New DesignTimeTaskViewModel(New DesignTimeTask("Task Title Here", "I have a description", New String() {"Tag 1", "Tag 2"}))
        }
    End Sub

    Public ReadOnly Property Tasks As DesignTimeTaskViewModel()

    Public ReadOnly Property ListName As String = "Tom's Design time data"
End Class

Public Class DesignTimeTaskViewModel

    Public Sub New(task As DesignTimeTask)
        Me.Task = task
    End Sub

    Public ReadOnly Property Task As DesignTimeTask
End Class

Public Class DesignTimeTask

    Public Sub New(title As String, description As String, tags As String())
        Me.Title = title
        Me.Description = description
        Me.Tags = tags
    End Sub

    Public Property Title As String
    Public Property Description As String
    Public Property Tags As String()
    Public Property Index As Integer = 9999
End Class