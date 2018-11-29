Imports System.ComponentModel

''' <summary>
''' All details for each task are stored through this class
''' </summary>
Public Class Task

    Private mTitle As String
    Private mDescription As String
    Private mTags As List(Of String)
    Private mIndex As Integer
    Private mCompleted As Boolean

    Public Event TaskChanged As EventHandler(Of TaskEventArgs)

    Public Sub New(title As String, description As String, tagList As List(Of String), index As Integer)
        mTitle = title
        mDescription = description
        mTags = tagList
        mIndex = index
    End Sub

    Public Sub New(title As String, description As String, tagList As List(Of String), index As Integer, completed As Boolean)
        Me.New(title, description, tagList, index)

        mCompleted = completed
    End Sub

    Public Overridable Property Title As String
        Get
            Return mTitle
        End Get
        Set(value As String)
            mTitle = value
            OnTaskChanged(New TaskEventArgs(TaskEventType.Title, Me))
        End Set
    End Property

    Public Overridable Property Description As String
        Get
            Return mDescription
        End Get
        Set(value As String)
            mDescription = value
            OnTaskChanged(New TaskEventArgs(TaskEventType.Description, Me))
        End Set
    End Property

    Public Overridable Property Completed As Boolean
        Get
            Return mCompleted
        End Get
        Set(value As Boolean)
            mCompleted = value
            OnTaskChanged(New TaskEventArgs(TaskEventType.Completed, Me))
        End Set
    End Property

    Public Overridable Property Index As Integer
        Get
            Return mIndex
        End Get
        Set(value As Integer)
            mIndex = value
            OnTaskChanged(New TaskEventArgs(TaskEventType.Index, Me))
        End Set
    End Property

    Public Overridable Property Tags As List(Of String)
        Get
            Return mTags
        End Get
        Set(value As List(Of String))
            mTags = value
            OnTaskChanged(New TaskEventArgs(TaskEventType.Tags, Me))
        End Set
    End Property

    Private Sub OnTaskChanged(e As TaskEventArgs)
        RaiseEvent TaskChanged(Me, e)
    End Sub
End Class
