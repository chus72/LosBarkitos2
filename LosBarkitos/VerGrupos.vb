'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports System.Drawing.Printing
Public Class VerGrupos
    Dim DGPrinter As DataGridPrinter

    Private Sub VerGrupos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DG.Height = Me.Height * 0.75
        DG.Width = Me.Width * 0.65
        LlenarDG()
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Public Sub LlenarDG()
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter


        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select * from Grupos where codigo<>'900' and codigo<>'999' order by nombre"

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Grupos")

        DG.DataSource = ds
        DG.DataMember = "Grupos"

        DG.Columns.Item(1).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(2).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(3).DefaultCellStyle.Format = "###.##"
        DG.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Sub

    Private Sub DG_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DG.CellValueChanged
        Dim cmd As SqlCommand
        Dim pB, pCom, pN, pC As SqlParameter
        Dim C As Double
        Dim B, Com As Double
        Dim N As String

        C = DG.Item(0, DG.CurrentRow.Index).Value
        N = DG.Item(1, DG.CurrentRow.Index).Value.ToString
        B = DG.Item(2, DG.CurrentRow.Index).Value
        Try
            Com = DG.Item(3, DG.CurrentRow.Index).Value

        Catch ex As Exception
            Com = 0

        End Try
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "update Grupos set Nombre = @nom, Bruto = @bru, Comision = @com where codigo = @cod"

        pB = New SqlParameter("@bru", B)
        pB.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pB)

        pCom = New SqlParameter("@com", Com)
        pCom.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pCom)

        pN = New SqlParameter("@nom", N)
        pN.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pN)

        pC = New SqlParameter("@cod", C)
        pC.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pC)

        cmd.ExecuteNonQuery()

    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        If SetupImprimir Then
            PrintDocument1.Print()
        End If

    End Sub

    Private Function SetupImprimir() As Boolean
        Dim MyPrintDialog As New PrintDialog()
        '    Dim m As Integer

        '        m = cmbMes.SelectedIndex

        MyPrintDialog.AllowCurrentPage = False
        MyPrintDialog.AllowPrintToFile = False
        MyPrintDialog.AllowSelection = False
        MyPrintDialog.AllowSomePages = False
        MyPrintDialog.PrintToFile = False
        MyPrintDialog.ShowHelp = False
        MyPrintDialog.ShowNetwork = False

        If MyPrintDialog.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Return False
        End If

        '       Titulo = "Listado mes: " & Mes(m)

        PrintDocument1.DocumentName = "Customers Report"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(10, 10, 10, 10)


        'Envio el documento a imprimir
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, "Grupos Clientes", New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
           Color.Black, True)

        Return True
    End Function

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim more As Boolean = DGPrinter.DrawDataGridView(e.Graphics)
        If more Then
            e.HasMorePages = True
        End If
    End Sub

    Private Sub btnPrintPreview_Click(ByVal sender As Object, ByVal e As EventArgs)

        If SetupImprimir() Then

            Dim MyPrintPreviewDialog As PrintPreviewDialog = New PrintPreviewDialog()
            MyPrintPreviewDialog.Document = PrintDocument1
            MyPrintPreviewDialog.ShowDialog()
        End If
    End Sub

End Class