Imports ClassLibrary

Public Class SearchFilters

    Private mViewModel As MainViewModel

    Public Sub New(viewModel As MainViewModel)
        mViewModel = viewModel
    End Sub

    Public Shared Function GetSearchPredicate(viewmodel As MainViewModel) As Predicate(Of Object)

        Return New Predicate(Of Object)(Function(parameter As Object)

                                            Dim taskModel = DirectCast(parameter, TaskViewModel)

                                            If viewmodel.ShowOnlyCompleted AndAlso taskModel.Task.Completed Then
                                                Return False
                                            End If

                                            If String.IsNullOrEmpty(viewmodel.SearchInput) Then
                                                Return True
                                            End If

                                            If viewmodel.ShowOnlyTags Then
                                                For Each t In taskModel.Task.Tags
                                                    Return If(t.Contains(viewmodel.SearchInput), True, False)
                                                Next
                                            End If

                                            If taskModel.Task.Description.Contains(viewmodel.SearchInput) AndAlso viewmodel.ShowOnlyDescription Then
                                                Return True
                                            End If

                                            Return If(taskModel.Task.Title.Contains(viewmodel.SearchInput), True, False) AndAlso viewmodel.ShowOnlyTitle
                                        End Function)
    End Function

    Public Function Incompleted() As Predicate(Of Object)
        Return New Predicate(Of Object)(Function(parameter As Object)
                                            Dim taskModel = DirectCast(parameter, TaskViewModel)
                                            If mViewModel.ShowOnlyIncompleted AndAlso taskModel.Task.Completed Then
                                                Return False
                                            End If

                                            If String.IsNullOrEmpty(mViewModel.SearchInput) Then
                                                Return True
                                            End If

                                            If mViewModel.ShowOnlyTags Then
                                                For Each t In taskModel.Task.Tags
                                                    Return If(t.Contains(mViewModel.SearchInput), True, False)
                                                Next
                                            End If

                                            If taskModel.Task.Description.Contains(mViewModel.mSearchInput) AndAlso mViewModel.ShowOnlyDescription Then
                                                Return True
                                            End If

                                            Return If(taskModel.Task.Title.Contains(mViewModel.SearchInput), True, False) AndAlso mViewModel.ShowOnlyTitle
                                        End Function)
    End Function

    Public Function Completed() As Predicate(Of Object)
        Return New Predicate(Of Object)(Function(parameter As Object)
                                            Dim taskModel = DirectCast(parameter, TaskViewModel)
                                            If mViewModel.ShowOnlyCompleted AndAlso Not taskModel.Task.Completed Then
                                                Return False
                                            End If

                                            If String.IsNullOrEmpty(mViewModel.SearchInput) Then
                                                Return True
                                            End If

                                            If mViewModel.ShowOnlyTags Then
                                                For Each t In taskModel.Task.Tags
                                                    Return If(t.Contains(mViewModel.SearchInput), True, False)
                                                Next
                                            End If

                                            If taskModel.Task.Description.Contains(mViewModel.mSearchInput) AndAlso mViewModel.ShowOnlyDescription Then
                                                Return True
                                            End If

                                            Return If(taskModel.Task.Title.Contains(mViewModel.SearchInput), True, False) AndAlso mViewModel.ShowOnlyTitle
                                        End Function)
    End Function

End Class
