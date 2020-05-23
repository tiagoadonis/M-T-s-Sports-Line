﻿Imports System.Data.SqlClient
Imports System.Security.Cryptography

Public Class Workers
    Dim CMD As SqlCommand
    Dim CN As SqlConnection = New SqlConnection("Data Source = localhost;" &
                                                "Initial Catalog = LojaDesporto; Integrated Security = true;")

    'Add Store Button
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim addStore As New AddStore
        addStore.StartPosition = FormStartPosition.CenterScreen
        addStore.ShowDialog()
    End Sub

    Public Sub loadStores()
        Dim adapter As New SqlDataAdapter("SELECT NumLoja AS Number, Nome AS Name FROM Projeto.Loja", CN)
        Dim table As New DataTable()
        adapter.Fill(table)

        With StoresDataGridView
            .DataSource = table
            .Columns(0).Width = 100
            .Columns(1).Width = 273
        End With
    End Sub

    'Stores DataGridView
    Private Sub DataGridview1_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Dim lastIndex As Integer = -1
        Dim index As Integer = e.RowIndex
        Dim selectedRow As DataGridViewRow = StoresDataGridView.Rows(index)
        Dim numStore As String = selectedRow.Cells(0).Value.ToString

        If (lastIndex <> index) Then
            lastIndex = index
        End If

        Dim ds As New DataSet()

        CMD = New SqlCommand
        CMD.Connection = CN
        CMD.CommandText = "SELECT Funcionario.NumFunc AS Num, Nome AS Name, Morada AS Address
                           FROM (Projeto.Loja JOIN Projeto.Funcionario ON Loja.NumLoja=Funcionario.NumLoja)
                           WHERE Loja.NumLoja = @store"
        CMD.Parameters.Add("@store", SqlDbType.VarChar, 1)
        CMD.Parameters("@store").Value = numStore
        CN.Open()

        Dim adapter As New SqlDataAdapter(CMD)
        adapter.Fill(ds)

        With WorkersDataGridView
            .DataSource = ds.Tables(0)
            .Columns(0).Width = 180
            .Columns(1).Width = 42
            .Columns(2).Width = 37
            .ClearSelection()
        End With
        CN.Close()
    End Sub

    'Workers DataGridView
    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles WorkersDataGridView.CellClick
        Dim index As Integer = e.RowIndex
        Dim selectedRow As DataGridViewRow = WorkersDataGridView.Rows(index)
        Dim numFunc As String = selectedRow.Cells(0).Value.ToString

        'Sales
        Dim ds As New DataSet()

        CMD = New SqlCommand
        CMD.Connection = CN
        CMD.CommandText = "SELECT Compra.NumCompra AS Num, Data AS Date, Montante AS Amount, NIF AS NIF
                           FROM (Projeto.Funcionario JOIN Projeto.Compra ON Funcionario.NumFunc=Compra.NumFunc)
                           WHERE Funcionario.NumFunc = @num"
        CMD.Parameters.Add("@store", SqlDbType.VarChar, 1)
        CMD.Parameters("@num").Value = numFunc
        CN.Open()

        Dim adapter As New SqlDataAdapter(CMD)
        adapter.Fill(ds)

        With SalesDataGridView
            .DataSource = ds.Tables(0)
            .Columns(0).Width = 90
            .Columns(1).Width = 42
            .Columns(2).Width = 37
            .Columns(3).Width = 90
            .ClearSelection()
        End With
        CN.Close()

        'Returns
        Dim ds2 As New DataSet()

        CMD = New SqlCommand()
        CMD.Connection = CN
        CMD.CommandText = "SELECT Devolucao.IDDevolucao AS ID, Data AS Date, Montante AS Amount, NIF AS NIF
                           FROM (Projeto.Funcionario JOIN Projeto.Devolucao ON Funcionario.NumFunc=Devolucao.NumFunc)
                           WHERE Funcionario.NumFunc = @num"
        CMD.Parameters.Add("@store", SqlDbType.VarChar, 1)
        CMD.Parameters("@num").Value = numFunc
        CN.Open()

        Dim adapter2 As New SqlDataAdapter(CMD)
        adapter2.Fill(ds2)

        With ReturnsDataGridView
            .DataSource = ds2.Tables(0)
            .Columns(0).Width = 90
            .Columns(1).Width = 42
            .Columns(2).Width = 37
            .Columns(3).Width = 90
            .ClearSelection()
        End With

        If CN.State = ConnectionState.Open Then
            CN.Close()
        End If
    End Sub

    'To add a new Store
    Public Sub addStore(ByVal storeName As String, ByVal storeLocation As String)
        Dim numStore As Integer = StoresDataGridView.Rows.Count + 1
        Dim table As New DataTable()

        CMD = New SqlCommand()
        CMD.Connection = CN
        CMD.CommandText = "INSERT INTO Projeto.Loja(NumLoja, Nome, Localizacao) 
                                             VALUES (@numStore, @storeName, @storeLocation)"
        CMD.Parameters.Add("@numStore", SqlDbType.Int)
        CMD.Parameters.Add("@storeName", SqlDbType.VarChar, 30)
        CMD.Parameters.Add("@storeLocation", SqlDbType.VarChar, 20)
        CMD.Parameters("@numStore").Value = numStore
        CMD.Parameters("@storeName").Value = storeName
        CMD.Parameters("@storeLocation").Value = storeLocation
        CN.Open()

        'VER COMO PASSAR OS DADOS PARA O storesDataGrid

        CN.Close()

    End Sub

End Class
