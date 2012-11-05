
'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient

Public Class GruposDia
    Dim FechaListado As DateTime
    Dim DGPrinter As DataGridPrinter

    Private Sub GruposDia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        dtpDia.Value = Now
        DG.Height = Me.Height * 0.9

        FechaListado = Now

        pnlControles.Left = CInt((DG.Left + DG.Width + Me.Width) / 2) - CInt(pnlControles.Width / 2)
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left

        LlenarDG(FechaListado)
        DG.Columns.Item(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub LlenarDG(ByVal F As DateTime)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter


        ' En oficial
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        If LosBarkitos.QuePuntoVenta = LosBarkitos.cOficina Then
            cmd.CommandText = "select  convert(char(10),Viajes_Grupo.Fecha,103) as [      F e c h a    ],Grupos.Nombre as [    G r u p o   ], Guias.Nombre as [    G u í a   ],Pax as [Pasajeros], Viajes_Grupo.Pax * Grupos.Bruto as [Bruto],Grupos.Comision as [Comision], Viajes_Grupo.Pax * Grupos.Bruto - Viajes_Grupo.Pax * Grupos.Comision as [N e t o] "
            cmd.CommandText = cmd.CommandText & " from Viajes_Grupo inner join Grupos on Viajes_Grupo.cod_Grupo = Grupos.codigo inner join Guias on Viajes_Grupo.cod_Guia = Guias.codigo "
            cmd.CommandText = cmd.CommandText & " where convert(char(10), Viajes_Grupo.Fecha, 103)= '" & F.ToString.Substring(0, 10) & "'"
            cmd.CommandText = cmd.CommandText & " order by fecha"
        Else
            cmd.CommandText = "select  convert(char(10),Viajes_Grupo.Fecha,103) as [      F e c h a    ],Grupos.Nombre as [    G r u p o   ], Guias.Nombre as [    G u í a   ],Pax as [Pasajeros], Viajes_Grupo.Pax * Grupos.Bruto as [Total] "
            cmd.CommandText = cmd.CommandText & " from Viajes_Grupo inner join Grupos on Viajes_Grupo.cod_Grupo = Grupos.codigo inner join Guias on Viajes_Grupo.cod_Guia = Guias.codigo "
            cmd.CommandText = cmd.CommandText & " where convert(char(10), Viajes_Grupo.Fecha, 103)= '" & F.ToString.Substring(0, 10) & "'"
            cmd.CommandText = cmd.CommandText & " order by fecha"

        End If

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Dia")

        DG.DataSource = ds
        DG.DataMember = "Dia"

        DG.Columns.Item(4).DefaultCellStyle.Format = "####.##"
        If LosBarkitos.QuePuntoVenta = LosBarkitos.cOficina Then
            DG.Columns.Item(5).DefaultCellStyle.Format = "####.##"
            DG.Columns.Item(6).DefaultCellStyle.Format = "###.##"
            DG.Columns.Item(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DG.Columns.Item(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        End If
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Sub


    Private Sub lblDiarioCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDiarioCerrar.Click
        Me.Close()
    End Sub

    Private Sub lblImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblImprimir.Click
        If SetupImprimir() Then
            PrintDocument1.Print()
        End If

    End Sub
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

        Titulo = "Listado Grupos día: " & CStr(FechaListado)

        PrintDocument1.DocumentName = "Customers Report"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(10, 10, 10, 10)


        'Envio el documento a imprimir
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, True, False, 0, 0, SumarViajesDia(dtpDia.Value), SumarDia(dtpDia.Value))

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

    Private Function SumarViajesDia(ByVal f As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        ' En oficial
        If LosBarkitos.chkNegro.Checked = False Then
            cmd.CommandText = "select count(Tickets.Precio) from Tickets "
            cmd.CommandText = cmd.CommandText & " where convert(char(10), Viajes_Grupo.Fecha, 103)= '" & f.ToString.Substring(0, 10) & "'"
            ' En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd.CommandText = "select count(TicketsN.Precio) from TicketsN "
            cmd.CommandText = cmd.CommandText & " where convert(char(10), Viajes_Grupo.Fecha, 103)= '" & f.ToString.Substring(0, 10) & "'"
        End If
        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        Return cmd.ExecuteScalar

    End Function
    Private Function SumarDia(ByVal f As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec As SqlParameter


        Try
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            ' En oficial
            If LosBarkitos.chkNegro.Checked = False Then
                cmd.CommandText = "select sum(Tickets.Precio) from Tickets "
                cmd.CommandText = cmd.CommandText & " where convert(char(10), Viajes_Grupo.Fecha, 103)= '" & f.ToString.Substring(0, 10) & "'"
                ' En negro
            ElseIf LosBarkitos.chkNegro.Checked = True Then
                cmd.CommandText = "select sum(TicketsN.Precio) from TicketsN "
                cmd.CommandText = cmd.CommandText & " where convert(char(10), Viajes_Grupo.Fecha, 103)= '" & f.ToString.Substring(0, 10) & "'"
            End If
            fec = New SqlParameter("@fech", f)
            fec.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec)

            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Sub dtpDia_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpDia.ValueChanged
        LlenarDG(dtpDia.Value)
        FechaListado = dtpDia.Value
    End Sub


    Private Sub DG_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DG.CellValueChanged

        Dim cmd As SqlCommand
        Dim Fec As DateTime
        Dim Cod As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt


        'Si se ha modificado la celda de Pax(celda 3)
        If e.ColumnIndex = 3 Then
            Fec = DG.Item(0, DG.CurrentRow.Index).Value.ToString
            Fec = Mid(Fec, 1, 10)
            cmd.CommandText = "update Viajes_Grupo set Pax = '" & DG.Item(2, DG.CurrentRow.Index).Value & "' "
            cmd.CommandText = cmd.CommandText & "where Fecha = '" & Fec & "' and "
            cmd.CommandText = cmd.CommandText & "Cod_Grupo = ' " & Cod & "'"
            'MessageBox.Show(DG.Item(3, DG.CurrentRow.Index).Value)
            cmd.ExecuteNonQuery()
        End If

    End Sub
End Class