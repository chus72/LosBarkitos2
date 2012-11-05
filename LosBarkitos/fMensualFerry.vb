'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient

Public Class fMensualFerry
    Dim MesListado As String
    Dim DGPrinter As DataGridPrinter
    Dim Mes(12) As String
    Dim Ano(6) As String
    Dim m As DateTime




    Private Sub Mensual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        DG.Height = Me.Height

        Ano(0) = "2008"
        Ano(1) = "2009"
        Ano(2) = "2010"
        Ano(3) = "2011"
        Ano(4) = "2012"
        Ano(5) = "2013"

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
        For i As Integer = 0 To 11
            cmbMes.Items.Add(Mes(i))
        Next
        cmbMes.Text = Mes(m.Month - 1)
        'Añado los años pertinentes
        cmbAno.Items.Add(Ano(0))
        cmbAno.Items.Add(Ano(1))
        cmbAno.Items.Add(Ano(2))
        cmbAno.Items.Add(Ano(3))
        cmbAno.Items.Add(Ano(4))
        cmbAno.Text = m.Year

        pnlControles.Left = CInt((DG.Left + DG.Width + Me.Width) / 2) - CInt(pnlControles.Width / 2)
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left


        LlenarDG(cmbMes.SelectedIndex + 1)
        DG.Columns.Item(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub

    Private Sub LlenarDG(ByVal F As Integer)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter

        ' En oficial
        If LosBarkitos.chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select  convert(char(10),Tickets.Fecha,103) as [       F e c h a    ],count(*) as [Pasajeros], round(sum(Tickets.Precio)/1.18,2) as [        B a s e     ], "
            cmd.CommandText = cmd.CommandText & " convert(decimal(5,2),round((sum(Tickets.Precio)-sum(Tickets.Precio)/1.18),2)) as [       I . V . A    ], "
            cmd.CommandText = cmd.CommandText & " convert(decimal(7,2),round(sum(Tickets.Precio),2)) as [       T o t a l    ] from Tickets "
            cmd.CommandText = cmd.CommandText & " where datepart(mm, Tickets.Fecha) =  " & CStr(F) & " and datepart(yyyy,Tickets.Fecha) = " & cmbAno.Text
            cmd.CommandText = cmd.CommandText & " group by convert(char(10),Tickets.Fecha,103)"
            cmd.CommandText = cmd.CommandText & " order by  convert(char(10),Tickets.Fecha, 103) "

            ds = New DataSet
            da = New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds, "Mes")

            DG.DataSource = ds
            DG.DataMember = "Mes"

            lblTotalMes.Text = "Total: " & SumarMes(F) & " €"
            lblViajesMes.Text = "Pasajeros: " & SumarViajesMes(F)
            DG.Columns.Item(1).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(2).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(3).DefaultCellStyle.Format = "###.##"
            DG.Columns.Item(4).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            ' En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select  convert(char(10),TicketsN.Fecha,103) as [       F e c h a    ],count(*) as [Pasajeros], round(sum(TicketsN.Precio)/1.18,2) as [        B a s e     ], "
            cmd.CommandText = cmd.CommandText & " convert(decimal(5,2),round((sum(TicketsN.Precio)-sum(TicketsN.Precio)/1.18),2)) as [       I . V . A    ], "
            cmd.CommandText = cmd.CommandText & " convert(decimal(7,2),round(sum(TicketsN.Precio),2)) as [       T o t a l    ] from TicketsN "
            cmd.CommandText = cmd.CommandText & " where datepart(mm, TicketsN.Fecha) =  " & CStr(F) & " and datepart(yyyy,TicketsN.Fecha) = " & cmbAno.Text
            cmd.CommandText = cmd.CommandText & " group by convert(char(10),TicketsN.Fecha,103)"
            ds = New DataSet
            da = New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds, "Mes")

            DG.DataSource = ds
            DG.DataMember = "Mes"

            lblTotalMes.Text = "Total: " & SumarMes(F) & " €"
            lblViajesMes.Text = "Viajes: " & SumarViajesMes(F)
            DG.Columns.Item(1).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(2).DefaultCellStyle.Format = "###.##"
            DG.Columns.Item(3).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(4).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        End If

    End Sub


    Private Function SumarViajesMes(ByVal f As Integer) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        ' En oficial
        If LosBarkitos.chkNegro.Checked = False Then
            cmd.CommandText = "select count(Tickets.Precio) from Tickets "
            cmd.CommandText = cmd.CommandText & " where datepart(mm, Tickets.Fecha) =  " & CStr(f) & " and datepart(yyyy,Tickets.Fecha) = " & cmbAno.Text
            ' En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd.CommandText = "select count(TicketsN.Precio) from TicketsN "
            cmd.CommandText = cmd.CommandText & " where datepart(mm, TicketsN.Fecha) =  " & CStr(f) & " and datepart(yyyy,TicketsN.Fecha) = " & cmbAno.Text
        End If
        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Return cmd.ExecuteScalar

    End Function

    Private Function SumarMes(ByVal f As Integer) As Double
        Dim cmd As SqlCommand
        Dim fec As SqlParameter


        If LosBarkitos.HayDiasMesFerry(f) Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            ' En oficial
            If LosBarkitos.chkNegro.Checked = False Then
                cmd.CommandText = "select sum(Tickets.Precio) from Tickets "
                cmd.CommandText = cmd.CommandText & " where datepart(mm, Tickets.Fecha) =  " & CStr(f) & " and datepart(yyyy,Tickets.Fecha) = " & cmbAno.Text
                ' En negro
            ElseIf LosBarkitos.chkNegro.Checked = True Then
                cmd.CommandText = "select sum(TicketsN.Precio) from TicketsN "
                cmd.CommandText = cmd.CommandText & " where datepart(mm, TicketsN.Fecha) =  " & CStr(f) & " and datepart(yyyy,TicketsN.Fecha) = " & cmbAno.Text
            End If
            fec = New SqlParameter("@fech", f)
            fec.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec)
            Try
                Return cmd.ExecuteScalar
            Catch
                Return 0
            End Try
        Else
            Return 0
        End If
    End Function

    Private Sub lblCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblCerrar.Click
        Me.Close()
    End Sub

    Private Sub cmbMes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMes.SelectedIndexChanged
        If cmbAno.Text = "" Then cmbAno.Text = m.Year

        LlenarDG(cmbMes.SelectedIndex + 1)
    End Sub

    Private Sub lblImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblImprimir.Click
        If SetupImprimir Then
            PrintDocument1.Print()
        End If

    End Sub

    Private Function SetupImprimir() As Boolean
        Dim MyPrintDialog As New PrintDialog()
        Dim Titulo As String
        Dim m As Integer

        m = cmbMes.SelectedIndex

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

        Titulo = "Listado mes: " & Mes(m)

        PrintDocument1.DocumentName = "Customers Report"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(10, 10, 10, 10)


        'Envio el documento a imprimir
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, False, False, 0, 0, SumarViajesMes(cmbMes.SelectedIndex + 1), SumarMes(cmbMes.SelectedIndex + 1))

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
    Private Sub cmbAno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAno.SelectedIndexChanged
        LlenarDG(cmbMes.SelectedIndex + 1)

    End Sub
End Class