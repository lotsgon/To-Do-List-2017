Option Strict On

Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Reflection
Imports System.Windows.Interop
Imports ClassLibrary
Imports Microsoft.Win32

Public Class MainViewModel
    Implements INotifyPropertyChanged

    Private mSelectedTaskIndex As Integer
    Private mSelectedTask As Task
    Private mObservableTasks As ObservableCollection(Of TaskViewModel)
    Private mTaskCollectionView As ListCollectionView
    Private WithEvents mTasks As TaskCollection
    Private mShowOnlyCompleted As Boolean
    Private mShowOnlyIncompleted As Boolean
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New()

        mTasks = New TaskCollection()
        mObservableTasks = New ObservableCollection(Of TaskViewModel)()
        AddHandler mTasks.CollectionChanged, AddressOf mTasksReal_CollectionChanged
        mTaskCollectionView = New ListCollectionView(mObservableTasks)
        ShowOnlyTitle = True
        ShowOnlyTags = True
        ShowOnlyDescription = True
    End Sub

    Public Property ShowOnlyCompleted As Boolean
        Get
            Return mShowOnlyCompleted
        End Get
        Set(value As Boolean)
            mShowOnlyCompleted = value

            If mShowOnlyCompleted Then
                Me.ShowOnlyIncompleted = False
            End If

            mTaskCollectionView.Filter = New SearchFilters(Me).Completed()

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ShowOnlyCompleted)))
        End Set
    End Property

    Public Property ShowOnlyIncompleted As Boolean
        Get
            Return mShowOnlyIncompleted
        End Get
        Set(value As Boolean)
            mShowOnlyIncompleted = value

            If mShowOnlyIncompleted Then
                Me.ShowOnlyCompleted = False
            End If

            mTaskCollectionView.Filter = New SearchFilters(Me).Incompleted()

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ShowOnlyIncompleted)))
        End Set
    End Property

    Public Property ShowOnlyTitle As Boolean
    Public Property ShowOnlyDescription As Boolean
    Public Property ShowOnlyTags As Boolean

    Public Property SelectedTask As Task
        Get
            If mObservableTasks.Count > 0 Then
                Return mObservableTasks.ElementAt(SelectedTaskIndex).Task
            End If
            Return Nothing
        End Get
        Set(value As Task)
            mSelectedTask = value
        End Set
    End Property
    Public Property SelectedTaskIndex As Integer
        Get
            If mSelectedTaskIndex > -1 Then
                Return mSelectedTaskIndex
            End If
            Return Nothing
        End Get
        Set(value As Integer)
            mSelectedTaskIndex = value
        End Set
    End Property

    Public mSearchInput As String

    Public ReadOnly Property ListName As String
        Get
            If (mTasks.FilePath Is Nothing) Then
                Return "No Current List"
            End If
            Return IO.Path.GetFileName(mTasks.FilePath).Split("."c)(0)
        End Get
    End Property

    Public Property SearchInput As String
        Get
            Return mSearchInput
        End Get
        Set(value As String)
            mSearchInput = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(mSearchInput)))

            mTaskCollectionView.Filter = SearchFilters.GetSearchPredicate(Me)
        End Set
    End Property

    Public ReadOnly Property Tasks As ListCollectionView
        Get
            Return mTaskCollectionView
        End Get
    End Property

    Public Sub mTasksReal_CollectionChanged(sender As Object, e As TaskCollectionEventArgs)

        Select Case e.Type
            Case TaskCollectionEventType.Added
                For Each task In e.Tasks
                    Dim mTaskModel As New TaskViewModel(task)
                    mObservableTasks.Add(mTaskModel)
                Next
            Case TaskCollectionEventType.Removed
                For Each task In e.Tasks

                    Dim viewmodel = mObservableTasks.FirstOrDefault(Function(t) t.Task.Title.Equals(task.Title, StringComparison.OrdinalIgnoreCase))

                    If viewmodel Is Nothing Then
                        Debug.Fail("Why was this task not in the observable collection?")
                        Continue For
                    End If

                    mObservableTasks.Remove(viewmodel)
                Next
                For Each task In mObservableTasks
                    task.Task.Index = mObservableTasks.IndexOf(task)
                Next
            Case TaskCollectionEventType.Inserted
                For Each task In e.Tasks
                    Dim mTaskModel As New TaskViewModel(task)
                    mObservableTasks.Insert(e.Index, mTaskModel)
                Next
        End Select
    End Sub

    Public Sub ClearFilter_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        mTaskCollectionView.Filter = Nothing
        mShowOnlyCompleted = False
        mShowOnlyIncompleted = False
        ShowOnlyTitle = True
        ShowOnlyDescription = True
        ShowOnlyTags = True

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ShowOnlyCompleted)))
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ShowOnlyIncompleted)))
    End Sub

    Public Sub ClearFilter_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = mTaskCollectionView.Filter IsNot Nothing
    End Sub

    Public Sub Open_Executed(sender As Object, e As ExecutedRoutedEventArgs)

        If mTasks.Count > 0 Then

            If MessageBox.Show("To open a new file you need to clear the list. Are you sure?", "Question",
                               MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.No Then
                Return
            End If
        End If

        Dim openFileDialog As New OpenFileDialog() With {
            .Filter = "XML Files(*.xml)|*.xml;",
            .InitialDirectory = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            .Multiselect = False,
            .Title = "Please browse to a saved task list"
            }

        If openFileDialog.ShowDialog() = True Then

            If Not mTasks.LoadFromFile(openFileDialog.FileName) Then
                System.Windows.MessageBox.Show("Could not load file!")
            End If
        End If

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ListName)))
    End Sub

    Public Sub DeleteTask_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Dim i = mTasks.IndexOf(DirectCast(e.Parameter, TaskViewModel).Task)
        mTasks.RemoveAt(i)
    End Sub

    Public Sub SaveAs_Executed(sender As Object, e As ExecutedRoutedEventArgs)

        Dim sfd As New SaveFileDialog() With {
            .Filter = "XML Files(*.xml)|*.xml;",
            .AddExtension = True,
            .OverwritePrompt = True,
            .InitialDirectory = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            .Title = "Please browse to a directory to save your task list"}

        If Not String.IsNullOrEmpty(mTasks.FilePath) Then
            sfd.InitialDirectory = IO.Path.GetDirectoryName(mTasks.FilePath)
            sfd.FileName = IO.Path.GetFileName(mTasks.FilePath)
        End If

        If sfd.ShowDialog() = True Then
            Try
                mTasks.SaveToFile(sfd.FileName)

            Catch ex As SaveFailedException
                MessageBox.Show($"Could not save the task list! {ex.Message}",
                            "Tom's Awesom Todo app",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Public Sub Save_Executed(sender As Object, e As ExecutedRoutedEventArgs)

        If mTasks.FilePath Is Nothing Then
            SaveAs_Executed(sender, e)
            Return
        End If

        Try
            mTasks.SaveToFile(mTasks.FilePath)

        Catch ex As SaveFailedException
            MessageBox.Show($"Could not save the task list! {ex.Message}",
                        "Tom's Awesom Todo app",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error)
        End Try
    End Sub

    Public Sub Save_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = mTasks.Count > 0
    End Sub

    Public Sub SaveAs_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = mTasks.Count > 0
    End Sub

    Public Sub AddTask_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Dim window = New CreateAndEditTaskWindow() With {
            .Owner = Application.Current.MainWindow,
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If window.ShowDialog() = True Then

            mTasks.AddTask(window.TaskTitle, window.Description, window.Tags.ToList())
        End If
    End Sub

    Public Sub EditTask_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Dim window = New CreateAndEditTaskWindow(DirectCast(e.Parameter, TaskViewModel)) With {
            .Owner = Application.Current.MainWindow,
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If window.ShowDialog() = True Then

            DirectCast(e.Parameter, TaskViewModel).Task.Title = window.TaskTitle
            DirectCast(e.Parameter, TaskViewModel).Task.Description = window.Description
            DirectCast(e.Parameter, TaskViewModel).Task.Tags = window.Tags.ToList()
        End If

    End Sub

    Public Sub ClearList_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        mTasks.Clear()
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ListName)))
    End Sub

    Public Sub ClearList_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = mTasks.Count > 0
    End Sub

    Public Sub CheckCompleted_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Dim taskModel As TaskViewModel = DirectCast(e.Parameter, TaskViewModel)
        Dim completed = taskModel.Task.Completed
        taskModel.Task.Completed = completed
    End Sub

    Public Sub MoveTaskUp_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        If SelectedTask IsNot Nothing Then
            mTasks.Move(SelectedTask.Index, SelectedTask.Index - 1)
            If SelectedTaskIndex = SelectedTask.Index Then
                SelectedTaskIndex -= 2
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedTaskIndex)))
            Else
                SelectedTaskIndex -= 1
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedTaskIndex)))
            End If
        End If
    End Sub

    Public Sub MoveTaskDown_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        If SelectedTask IsNot Nothing Then
            mTasks.Move(SelectedTask.Index, SelectedTask.Index + 1)
            SelectedTaskIndex += 1
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedTaskIndex)))
        End If
    End Sub

    Public Sub MoveTaskUp_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If SelectedTask Is Nothing Then
            e.CanExecute = mTasks.Count > 2 And SelectedTask IsNot Nothing
        Else
            e.CanExecute = mTasks.Count > 2 And SelectedTask.Index > 0 And SelectedTask IsNot Nothing
        End If
    End Sub

    Public Sub MoveTaskDown_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If SelectedTask Is Nothing Then
            e.CanExecute = mTasks.Count > 2 And SelectedTask IsNot Nothing
        Else
            e.CanExecute = mTasks.Count > 2 And SelectedTask.Index < mTasks.Count - 1 And SelectedTask IsNot Nothing
        End If
    End Sub

    Public Shared Sub ConfirmCancel_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Dim window As Window = DirectCast(sender, Window)
        window.Close()
    End Sub

    Public Sub ExitApplication_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        System.Windows.Application.Current.Shutdown()
    End Sub
End Class


