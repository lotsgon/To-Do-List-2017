Imports System.ComponentModel
Imports ClassLibrary

Public Class TaskViewModel
    Implements INotifyPropertyChanged

    Private mTask As Task

    Public Sub New(task As Task)
        Me.mTask = task
        AddHandler mTask.TaskChanged, AddressOf mTask_Changed
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property Task As Task
        Get
            Return mTask
        End Get
        Set(value As Task)
            mTask = value
        End Set
    End Property

    Public Sub mTask_Changed(sender As Object, e As TaskEventArgs)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Task)))
    End Sub

End Class
