

Imports System.Data.SqlClient
'Imports System.Drawing.Printing


Public Class Fotos
    Dim FechaListado As DateTime
    Dim DGPrinter As DataGridPrinter

    Dim Mes(12) As String
    Dim Año(10) As String


    Private Sub Fotos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim m As DateTime
        DG.Height = Me.Height * 0.9

        DG.Width = Me.Width * 0.65
        DG.Left = 1


        Año(0) = "2007"
        Año(1) = "2008"
        Año(2) = "2009"
        Año(3) = "2010"
        Año(4) = "2011"
        Mes(0) = "Enero"
        Mes(1) = "Febrero"
        Mes(2) = "Marzo"
        Mes(3) = "Abril"
        Mes(4) = "Mayo"
        Mes(5) = "Junio"
        Mes(6) = "Julio"
        Mes(7) = "Agosto"
        Mes(8) = "Septiembre"
        Mes(9) = "Octubre"
        Mes(10) = "Noviembre"
        Mes(11) = "Diciembre"
        m = Now
        For i As Integer = 0 To m.Month - 1
            cmbMes.Items.Add(Mes(i))
        Next
        cmbAño.Items.Add(Año(0))
        cmbAño.Items.Add(Año(1))
        cmbAño.Items.Add(Año(2))
        cmbAño.Items.Add(Año(3))
        cmbAño.Items.Add(Año(4))

        cmbMes.Text = Mes(m.Month - 1)
        cmbAño.Text = Año(4)
        cmbAño.SelectedIndex = 2

        pnlControles.Left = CInt((DG.Left + DG.Width + Me.Width) / 2) - CInt(pnlControles.Width / 2)
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left

        LlenarDG(cmbMes.SelectedIndex + 1)

        DG.Columns.Item(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub

    Public Sub LlenarDG(ByVal F As String)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter

        Dim TotMes As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select dia as [       F e c h a       ], Total as [      T o t a l    ] from Fotos "
        cmd.CommandText = cmd.CommandText & " where  datepart(mm, dia)= '" & CStr(F) & "' and year(dia) = '" & cmbAño.Text & "'"
        cmd.CommandText = cmd.CommandText & " order by dia"
  
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        DG.DataSource = ds
        DG.DataMember = "Diario"
        DG.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(1).DefaultCellStyle.Format = "###.## €"

        TotMes = SumarMes(F)
        lblTotal.Text = "Total: " & TotMes


    End Sub

    Private Function SumarMes(ByVal f As Integer) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(total) from fotos where datepart(mm, dia)= '" & CStr(f) & "' and year(dia) = '" & cmbAño.Text & "'"
        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar

        Catch ex As Exception
            Return 0
        End Try


    End Function


    Private Sub lblFechaHoy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        FechaListado = Now
        LlenarDG(FechaListado)
    End Sub

    Private Sub lblDiarioCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDiarioCerrar.Click
        Me.Close()
    End Sub


    ' Private Sub btnIr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIr.Click
    '     LlenarDG(CDate(dtpFecha.Text))
    '     FechaListado = CDate(dtpFecha.Text)
    ' End Sub

    Private Function SetupImprimir() As Boolean
        Dim MyPrintDialog As New PrintDialog()
        Dim Titulo As String
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

        Titulo = "Listado día : " & CStr(cmbMes.Text)
        If LosBarkitos.chkNegro.Checked = True Then Titulo = Titulo & " (N)"

        PrintDocument1.DocumentName = "Informe Barkitos"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(1, 1, 1, 1)


        'Envio el documento a imprimir
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, True, False, 0, 0, SumarMes(cmbMes.SelectedIndex + 1), SumarMes(cmbMes.SelectedIndex + 1))

        Return True
    End Function


    Private Sub lblImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblImprimir.Click
        If SetupImprimir() Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim Total As String = Nothing
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


    Private Sub cmbMes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMes.SelectedIndexChanged
        LlenarDG(cmbMes.SelectedIndex + 1)
    End Sub

    Private Sub cmbAño_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAño.SelectedIndexChanged
        LlenarDG(cmbMes.SelectedIndex + 1)
    End Sub

End Class
