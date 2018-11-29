Public Class MainWindow

    Private mViewModel As New MainViewModel()


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataContext = mViewModel

        Me.CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf mViewModel.Open_Executed))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.AddTask, AddressOf mViewModel.AddTask_Executed))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ClearList, AddressOf mViewModel.ClearList_Executed, AddressOf mViewModel.ClearList_CanExecute))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.DeleteTask, AddressOf mViewModel.DeleteTask_Executed))
        Me.CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf mViewModel.Save_Executed, AddressOf mViewModel.Save_CanExecute))
        Me.CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf mViewModel.SaveAs_Executed, AddressOf mViewModel.SaveAs_CanExecute))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ExitApplication, AddressOf mViewModel.ExitApplication_Executed))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.CheckCompleted, AddressOf mViewModel.CheckCompleted_Executed))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ConfirmCancel, AddressOf MainViewModel.ConfirmCancel_Executed))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.MoveTaskUp, AddressOf mViewModel.MoveTaskUp_Executed, AddressOf mViewModel.MoveTaskUp_CanExecute))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.MoveTaskDown, AddressOf mViewModel.MoveTaskDown_Executed, AddressOf mViewModel.MoveTaskDown_CanExecute))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.ClearFilter, AddressOf mViewModel.ClearFilter_Execute, AddressOf mViewModel.ClearFilter_CanExecute))
        Me.CommandBindings.Add(New CommandBinding(TaskCommands.EditTask, AddressOf mViewModel.EditTask_Executed))

    End Sub
End Class
