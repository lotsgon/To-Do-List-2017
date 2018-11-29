Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports ClassLibrary

Public Class CreateAndEditTaskWindow
    Implements INotifyPropertyChanged

    Private mTask As Task
    Private ReadOnly mObservableTagList As New ObservableCollection(Of String)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataContext = Me

        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ConfirmTask, Sub(s, e) Me.DialogResult = True,
                                                  Sub(s, e) e.CanExecute = Not String.IsNullOrWhiteSpace(TaskTitle) And
                                                                           Not String.IsNullOrWhiteSpace(Description) And
                                                                           Tags.Count > 0))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ConfirmCancel, Sub(s, e) Me.DialogResult = False))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ConfirmAddTag, Sub(s, e)
                                                                                  mObservableTagList.Add(NewTag)
                                                                                  NewTag = Nothing
                                                                                  RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(NewTag)))
                                                                              End Sub,
                                                                              Sub(s, e)
                                                                                  e.CanExecute = Not String.IsNullOrWhiteSpace(NewTag)
                                                                              End Sub))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ConfirmRemoveTag, Sub(s, e)
                                                                                     SelectedTag = e.Parameter.ToString()
                                                                                     mObservableTagList.Remove(SelectedTag)
                                                                                     SelectedTag = Nothing
                                                                                 End Sub))
    End Sub

    Public Sub New(taskModel As TaskViewModel)
        Me.New()

        IsEditMode = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsEditMode)))

        mTask = taskModel.Task

        For Each t In mTask.Tags
            mObservableTagList.Add(t)
        Next

        TaskTitle = mTask.Title
        Description = mTask.Description

    End Sub

    Public Property IsEditMode As Boolean
    Public Property SelectedTag As String
    Public Property NewTag As String
    Public Property TaskTitle As String
    Public Property Description As String
    Public ReadOnly Property Tags As ObservableCollection(Of String)
        Get
            Return mObservableTagList
        End Get
    End Property

End Class
