'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports System.Drawing.Printing

Public Class fEstadisticaGuia
    Dim FechaListado As DateTime
    Dim DGPrinter As DataGridPrinter

    Private Sub fEstadisticaGuia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean

        DG.Height = Me.Height * 0.9
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left
        FechaListado = Now
        'DG.Columns.Item(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Codigo,Nombre from Guias order by Nombre "
        lector = cmd.ExecuteReader
        b = lector.Read()
        Do While b = True
            cmbGuia.Items.Add(lector("Nombre"))
            b = lector.Read()
        Loop
        lector.Close()



    End Sub

    Private Sub LlenarDG()
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim nom As SqlParameter

        'Listado de los Grupos
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Viajes_Grupo.Fecha as [           Fecha          ], Guias.Nombre as [          Guía         ],"
        cmd.CommandText = cmd.CommandText & "  Grupos.Nombre as [         Grupo        ], Viajes_Grupo.Pax as [           P a x         ] from  Guias INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Viajes_grupo ON Viajes_Grupo.Cod_Guia = Guias.Codigo INNER JOIN Grupos"
        cmd.CommandText = cmd.CommandText & " on Viajes_Grupo.Cod_Grupo = Grupos.Codigo"
        cmd.CommandText = cmd.CommandText & " where Guias.Nombre = @nom order by Viajes_Grupo.Fecha"
        nom = New SqlParameter("@nom", cmbGuia.Text)
        nom.Direction = ParameterDirection.Input
        cmd.Parameters.Add(nom)
        ds = New DataSet
        da = New SqlDataAdapter
        Try
            da.SelectCommand = cmd
            da.Fill(ds, "Guia")

            DG.DataSource = ds
            DG.DataMember = "Guia"
            DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            'Suma el total de los pax del guia
            lblTotal.Text = cmbGuia.Text & " : " & TotalPaxGuia() & " pax"

        Catch ex As Exception
            MsgBox("No hay datos para el guía " & cmbGuia.Text, MsgBoxStyle.Information)
        End Try

    End Sub
    Private Function SumarTicketsDia() As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        'Tickets en oficial
        If LosBarkitos.chkNegro.Checked = False Then
            cmd.CommandText = "select count(Tickets.Precio) from Tickets where convert(char(10),Tickets.Fecha,103) = convert(char(10), @fech ,103)"
            'Tickets en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd.CommandText = "select count(TicketsN.Precio) from TicketsN where convert(char(10),TicketsN.Fecha,103) = convert(char(10), @fech ,103)"
        End If

        Return cmd.ExecuteScalar
    End Function


    Private Sub cmbGrupo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGuia.SelectedIndexChanged
        Me.Text = "Estadísticas del guía : " & cmbGuia.Text
        LlenarDG()
    End Sub

    Private Sub lblDiarioCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDiarioCerrar.Click
        Me.Close()
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

        Titulo = "Listado Grupo : " & CStr(cmbGuia.Text)

        PrintDocument1.DocumentName = "Informe MarinaFerry"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(1, 1, 1, 1)


        'Envio el documento a imprimir
        ' El Penultimo parametro es el total de los pax del guia
        ' el ultimo = 1 para guia
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, "Grupos", New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), Color.Aqua, True, TotalPaxGuia, 1)
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

    Private Function TotalPaxGuia() As Integer
        Dim cmd As SqlCommand
        Dim nom As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(Pax) from  Viajes_Grupo INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Grupos ON Viajes_Grupo.Cod_Grupo = Grupos.Codigo INNER JOIN Guias"
        cmd.CommandText = cmd.CommandText & " on Viajes_Grupo.Cod_Guia = Guias.Codigo"
        cmd.CommandText = cmd.CommandText & " where Guias.Nombre = @nom "
        nom = New SqlParameter("@nom", cmbGuia.Text)
        nom.Direction = ParameterDirection.Input
        cmd.Parameters.Add(nom)
        Return CInt(cmd.ExecuteScalar)
    End Function

    Private Sub DG_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DG.CellValueChanged
        Dim cmd As SqlCommand
        Dim Fec As DateTime
        Dim Cod As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        'Si se ha modificado la columna de precios
        cmd.CommandText = "select Codigo from Guias where Nombre= '" & DG.Item(1, DG.CurrentRow.Index).Value & "'"
        Cod = cmd.ExecuteScalar
        If e.ColumnIndex = 3 Then
            Fec = DG.Item(0, DG.CurrentRow.Index).Value.ToString
            Fec = Mid(Fec, 1, 10)
            cmd.CommandText = "update Viajes_Grupo set Pax = '" & DG.Item(3, DG.CurrentRow.Index).Value & "' "
            cmd.CommandText = cmd.CommandText & "where Fecha = '" & Fec & "' and "
            cmd.CommandText = cmd.CommandText & "Cod_Guia = ' " & Cod & "'"
            'MessageBox.Show(DG.Item(3, DG.CurrentRow.Index).Value)
            cmd.ExecuteNonQuery()
        End If
    End Sub


End Class