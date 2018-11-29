Imports System.IO

''' <summary>
''' A collection of tasks stored in a private list inside this class.
''' </summary>
Public Class TaskCollection
    Implements IList(Of Task)

    Private ReadOnly mTasks As List(Of Task) = New List(Of Task)

    ' Only populated if we're loaded from a file
    Private mFilePath As String

    ''' <summary>
    ''' Public event to notify when the lists contents have been changed.
    ''' </summary>
    Public Event CollectionChanged As EventHandler(Of TaskCollectionEventArgs)

    ''' <summary>
    ''' The constructor to make a new instance of TaskCollection
    ''' </summary>
    Public Sub New()
        ' Only this class should have access to the constructor
        ' use Load to create new ones
    End Sub

    ''' <summary>
    ''' Public property to set the active task for the console app.
    ''' </summary>
    Public Property ActiveTask As Task

    ''' <summary>
    ''' Gets the path from which the task list was loaded, if it was loaded from a file.
    ''' </summary>
    Public ReadOnly Property FilePath As String
        Get
            Return mFilePath
        End Get
    End Property

    ''' <summary>
    ''' Public property of Item to get and set an item at index.
    ''' </summary>
    Default Public Property Item(index As Integer) As Task Implements IList(Of Task).Item
        Get
            Return mTasks(index)
        End Get
        Set(value As Task)
            mTasks(index) = value
        End Set
    End Property

    ''' <summary>
    ''' Property of Count on the list to track the count of its items.
    ''' </summary>
    Public ReadOnly Property Count As Integer Implements ICollection(Of Task).Count
        Get
            Return mTasks.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of Task).IsReadOnly

    ''' <summary>
    ''' Inserts the Task into the list at the specified index.
    ''' </summary>
    Public Sub Insert(index As Integer, item As Task) Implements IList(Of Task).Insert
        mTasks.Insert(index, item)
        SynchronizeIndex()

        OnCollectionChanged(New TaskCollectionEventArgs(TaskCollectionEventType.Inserted, New Task() {item}, index))
    End Sub

    ''' <summary>
    ''' Removes the Task at the specified index in the list.
    ''' </summary>
    Public Sub RemoveAt(index As Integer) Implements IList(Of Task).RemoveAt
        If index < 0 OrElse index >= mTasks.Count Then
            Return
        End If

        Dim task = mTasks(index)

        mTasks.RemoveAt(index)
        SynchronizeIndex(index)
        OnCollectionChanged(New TaskCollectionEventArgs(TaskCollectionEventType.Removed, New Task() {task}))


    End Sub

    ''' <summary>
    ''' Adds a task to task collection and passes in task collection to Task constructor for index.
    ''' </summary>
    Public Sub AddTask(title As String, description As String, tags As List(Of String))
        Dim task = New Task(title, description, tags, Me.Count)
        mTasks.Add(task)

        OnCollectionChanged(New TaskCollectionEventArgs(TaskCollectionEventType.Added, New Task() {task}))
    End Sub

    ''' <summary>
    ''' Another implementation of AddTask using the other Task constructor.
    ''' </summary>
    Public Sub AddTask(title As String, description As String, tags As List(Of String), completed As Boolean)
        Dim task = New Task(title, description, tags, Me.Count, completed)
        mTasks.Add(task)

        OnCollectionChanged(New TaskCollectionEventArgs(TaskCollectionEventType.Added, New Task() {task}))
    End Sub

    ''' <summary>
    ''' Clears the list if the list isn't already empty.
    ''' </summary>
    Public Sub Clear() Implements ICollection(Of Task).Clear
        If mTasks.Count = 0 Then
            Return
        End If

        Dim beforeRemoved = mTasks.ToArray()

        mTasks.Clear()
        mFilePath = Nothing

        OnCollectionChanged(New TaskCollectionEventArgs(TaskCollectionEventType.Removed, beforeRemoved))
    End Sub

    ''' <summary>
    ''' Copies the list into an array.
    ''' </summary>
    Public Sub CopyTo(array() As Task, arrayIndex As Integer) Implements ICollection(Of Task).CopyTo
        mTasks.CopyTo(array, arrayIndex)
    End Sub

    ''' <summary>
    ''' Gets the index of the specified Task.
    ''' </summary>
    Public Function IndexOf(item As Task) As Integer Implements IList(Of Task).IndexOf
        Return mTasks.IndexOf(item)
    End Function

    ''' <summary>
    ''' Checks the list to see if it contains the specified Task.
    ''' </summary>
    Public Function Contains(item As Task) As Boolean Implements ICollection(Of Task).Contains
        Return mTasks.Contains(item)
    End Function

    ''' <summary>
    ''' Removes the specified Task from the list.
    ''' </summary>
    Public Function Remove(item As Task) As Boolean Implements ICollection(Of Task).Remove
        Return mTasks.Remove(item)
    End Function

    ''' <summary>
    ''' Gets the enumerator of the list.
    ''' </summary>
    Public Function GetEnumerator() As IEnumerator(Of Task) Implements IEnumerable(Of Task).GetEnumerator
        Return mTasks.GetEnumerator()
    End Function

    ''' <summary>
    ''' This performs a for each loop of the list and calls the required action.
    ''' </summary>
    Public Sub ForEach(action As Action(Of Task))
        For Each task In mTasks
            action(task)
        Next
    End Sub

    ''' <summary>
    ''' Moves the specified task from its current index to its new specified index.
    ''' </summary>
    Public Sub Move(task As Task, newIndex As Integer)

        Dim oldIndex = mTasks.IndexOf(task)

        Me.RemoveAt(oldIndex)
        Me.Insert(newIndex, task)

        Me.SynchronizeIndex(Math.Min(newIndex, oldIndex))

    End Sub

    ''' <summary>
    ''' Another implementation of Move taking in an old index instead of using its current index.
    ''' </summary>
    Public Sub Move(oldIndex As Integer, newIndex As Integer)
        Dim task As Task = mTasks(oldIndex)

        Me.RemoveAt(oldIndex)
        Me.Insert(newIndex, task)

        Me.SynchronizeIndex(Math.Min(newIndex, oldIndex))
    End Sub

    ''' <summary>
    ''' Loads the specified list of tasks from an XML File.
    ''' </summary>
    ''' <returns>
    ''' Returns a boolean to say whether the file has loaded or not
    ''' </returns>
    Public Function LoadFromFile(filePath As String) As Boolean

        If Not File.Exists(filePath) Then
            Return False
        End If

        Me.Clear()

        Dim toDoList As XDocument

        Try
            ' Takes in an relative/absolute path from the user 
            toDoList = XDocument.Load(filePath)

            If Not toDoList.Root.Elements("Task").Any() Then
                Return False
            End If

            For Each taskElement In toDoList.Root.Elements("Task")

                Dim completed = False
                If Not Boolean.TryParse(taskElement.Element("Completed").Value, completed) Then

                    Debug.Fail("How did this non-boolean make it in here?")
                    Continue For
                End If

                Me.AddTask(taskElement.Element("Title").Value,
                       taskElement.Element("Description").Value,
                       taskElement.Element("Tags").Elements("Tag").Select(Function(element) element.Value).ToList(),
                       completed)
            Next

            mFilePath = filePath

            Return True

        Catch ex As Exception When IsGenericIOException(ex)

            Throw New LoadFailedException(ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Saves the current list of tasks to an XML File.
    ''' </summary>
    ''' <returns>
    ''' Returns a Boolean To show whether the file has been saved Or Not.
    ''' </returns>
    ''' <exception cref="SaveFailedException">Thrown when we fail to save due to an exception, such as AccessViolationException.</exception>
    Public Function SaveToFile(filePath As String) As Boolean

        ' Selects all details about each task from the list And makes them child elements within a <Task>
        Dim toDoList As XDocument = New XDocument(New XElement("ToDoList",
                        From task In Me
                        Select New XElement("Task",
                               New XElement("Title", task.Title),
                               New XElement("Description", task.Description),
                               New XElement("Tags",
                               From tag In task.Tags
                               Select New XElement("Tag", tag.ToString())),
                               New XElement("Completed", task.Completed)
                                           )))

        Try
            toDoList.Save(filePath)

            mFilePath = filePath

            Return True

        Catch ex As Exception When IsGenericIOException(ex)

            Throw New SaveFailedException(ex.Message, ex)
        End Try
    End Function

    Private Sub OnCollectionChanged(e As TaskCollectionEventArgs)
        RaiseEvent CollectionChanged(Me, e)
    End Sub

    Private Sub SynchronizeIndex(Optional ByVal startIndex As Integer = 0)

        For Each t In mTasks.Skip(startIndex)
            t.Index = mTasks.IndexOf(t)
        Next
    End Sub

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mTasks.GetEnumerator()
    End Function

    Private Sub Add(item As Task) Implements ICollection(Of Task).Add
        mTasks.Add(item)

        OnCollectionChanged(New TaskCollectionEventArgs(TaskCollectionEventType.Added, New Task() {item}))
    End Sub
End Class