
Imports System.Data.SqlClient

Public Class fDiarioFerry
    Dim FechaListado As DateTime
    Dim DGPrinter As DataGridPrinter

    Private Sub fDiarioFerry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Listado diario de Marina Ferry"
        DG.Height = Me.Height * 0.9
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left
        FechaListado = Now
        LlenarDG(Now)
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        'Poner las columnas de DG a no editables
        For i As Integer = 0 To 3
            DG.Columns.Item(i).ReadOnly = True
        Next
    End Sub

    Private Sub LlenarDG(ByVal F As DateTime)
        'Listado de MF en oficial
        If LosBarkitos.chkNegro.Checked = False Then
            ListadoA(F)

            'Listado de MF en Negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            ListadoB(F)
        End If

    End Sub

    Private Function ListadoA(ByVal F As DateTime)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim fec As SqlParameter
        Dim Dias As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Tickets.Numero as [       Factura      ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  Tickets.Fecha as [       F e c h a    ], convert(decimal(5,2),Tickets.Precio) as [     P r e c i o    ] from  PuntoVenta INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Tickets ON Tickets.cod_PV = PuntoVenta.Codigo "
        cmd.CommandText = cmd.CommandText & " where convert(char(10),Tickets.Fecha,103) = convert(char(10), @fech ,103) "
        cmd.CommandText = cmd.CommandText & " order by Tickets.Numero"
        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        DG.DataSource = ds
        DG.DataMember = "Diario"
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dias = SumarTicketsDia(F)
        lblNumViajes.Text = "Tickets: " & Dias
        If Dias > 0 Then
            lblTotal.Text = "Total: " & SumarDia(F) & " €"
        Else
            lblTotal.Text = "Total: 0 €"
        End If
        Return ds
    End Function

    Private Function ListadoB(ByVal F As DateTime)
        Dim cmd As SqlCommand
        Dim ds, dsa As DataSet
        Dim da As SqlDataAdapter
        Dim fec As SqlParameter
        Dim Dias As Integer

        dsa = ListadoA(F)


        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select TicketsN.Numero as [       Factura      ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  TicketsN.Fecha as [       F e c h a    ], convert(decimal(5,2),TicketsN.Precio) as [     P r e c i o    ] from  PuntoVenta INNER JOIN"
        cmd.CommandText = cmd.CommandText & " TicketsN ON TicketsN.cod_PV = PuntoVenta.Codigo "
        cmd.CommandText = cmd.CommandText & " where convert(char(10),TicketsN.Fecha,103) = convert(char(10), @fech ,103) order by TicketsN.Fecha "
        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        ds.Merge(dsa)
        DG.DataSource = ds
        DG.DataMember = "Diario"
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dias = SumarTicketsDia(F)
        lblNumViajes.Text = "Tickets: " & Dias
        If Dias > 0 Then
            lblTotal.Text = "Total: " & SumarDia(F) & " €"
        Else
            lblTotal.Text = "Total: 0 €"
        End If
        Return ds

    End Function

    Private Function SumarTicketsDia(ByVal f As DateTime) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        'Tickets en oficial
        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarTicketsDiaA(f)
            'Tickets en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarTicketsDiaB(f)
        End If


    End Function

    Private Function SumarTicketsDiaA(ByVal f As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(Tickets.Precio) from Tickets where convert(char(10),Tickets.Fecha,103) = convert(char(10), @fech ,103)"
        fec = New SqlParameter("@fech", f)
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function SumarTicketsDiaB(ByVal f As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec As New SqlParameter("@fech", f)
        Dim A As Integer

        A = SumarTicketsDiaA(f)
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(TicketsN.Precio) from TicketsN where convert(char(10),TicketsN.Fecha,103) = convert(char(10), @fech ,103)"
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar + A
        Catch ex As Exception
            Return A
        End Try
    End Function

    Private Function SumarDia(ByVal f As DateTime) As Double
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        ' En oficial
        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarDiaA(f)
            ' En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarDiaB(f)

        End If

    End Function

    Private Function SumarDiaA(ByVal f As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec As New SqlParameter("@fech", f)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(Tickets.Precio) from Tickets where convert(char(10),Tickets.Fecha,103) = convert(char(10), @fech ,103)"

        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function SumarDiaB(ByVal f As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec As New SqlParameter("@fech", f)
        Dim A As Integer = SumarDiaA(f)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(TicketsN.Precio) from TicketsN where convert(char(10),TicketsN.Fecha,103) = convert(char(10), @fech ,103)"

        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar + A
        Catch ex As Exception
            Return A
        End Try

    End Function

    Private Sub lblFechaHoy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFechaHoy.Click
        FechaListado = Now
        LlenarDG(FechaListado)
    End Sub

    Private Sub lblDiarioCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDiarioCerrar.Click
        Me.Close()
    End Sub


    Private Sub btnIr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LlenarDG(CDate(dtpFecha.Text))
        FechaListado = CDate(dtpFecha.Text)
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

        Titulo = "Listado día : " & CStr(CDate(dtpFecha.Text))

        PrintDocument1.DocumentName = "Informe MarinaFerry"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(1, 1, 1, 1)


        'Envio el documento a imprimir fdiarioFerry
        'Envio el documento a imprimir, en caso de que sea oficial le quito la fecha
        If LosBarkitos.chkNegro.Checked = False Then
            DG = PrepararDGImprimir(DG, dtpFecha.Text)
        End If


        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, False, True, SumarTicketsDia(FechaListado), SumarDia(FechaListado), 0, 0)

        Return True

    End Function

    Function PrepararDGImprimir(ByVal DG As DataGridView, ByVal f As DateTime) As DataGridView
        Dim cmd As SqlCommand
        Dim fec As SqlParameter
        Dim ds As DataSet
        Dim da As SqlDataAdapter


        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Tickets.Numero as [       Factura      ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  convert(decimal(5,2),Tickets.Precio) as [     P r e c i o    ] from  PuntoVenta INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Tickets ON Tickets.cod_PV = PuntoVenta.Codigo "
        cmd.CommandText = cmd.CommandText & " where convert(char(10),Tickets.Fecha,103) = convert(char(10), @fech ,103)"
        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        DG.DataSource = ds
        DG.DataMember = "Diario"
        DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Return DG
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


    Private Sub DG_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DG.CellDoubleClick
        Dim fact As Integer
        Dim prec As Double
        Dim celda As DataGridViewCell

        LosBarkitos.TipoBarca = 4

        celda = DG.Item(0, DG.CurrentRow.Index)
        'Solo hay una fila y esta en blanco
        If DG.RowCount = 1 Then Return
        fact = CInt(celda.Value)
        celda = DG.Item(3, DG.CurrentRow.Index)
        prec = CDbl(celda.Value)

        If LosBarkitos.QuePuntoVenta() <> LosBarkitos.cBarkitos Then
            LosBarkitos.ImprimirTicket("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\LogoFerry.jpg", _
                           fact, prec, 4)
        Else
            LosBarkitos.ImprimirTicket("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\LogoFerry.jpg", _
                            fact, prec, 4)
        End If
    End Sub


    Private Sub btnBorrarUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrarUltimo.Click
        Dim cmd As SqlCommand
        Dim N As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        ' Si el listado es en negro
        If LosBarkitos.chkNegro.Checked = True Then
            cmd.CommandText = "select top(1) TicketsN.Numero from TicketsN order by fecha desc"
            N = cmd.ExecuteScalar
            cmd.CommandText = "delete  from TicketsN where TicketsN.Numero = '" & N & "'"
            Dim m As Integer = cmd.ExecuteScalar()
            LlenarDG(Now)
        End If

        'Si el listado es oficial
        If LosBarkitos.chkNegro.Checked = False Then
            Dim cont As Integer
            cmd.CommandText = "select Numero_Ticket_Ferry from contadores"
            cont = cmd.ExecuteScalar
            cmd.CommandText = "delete from Tickets where Tickets.Numero = '" & cont - 1 & "'"
            cmd.ExecuteScalar()
            LlenarDG(Now)
            cmd.CommandText = "update contadores set Numero_Ticket_Ferry = '" & cont - 1 & "'"
            cmd.ExecuteScalar()
        End If
    End Sub


    Private Sub dtpFecha_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFecha.ValueChanged
        LlenarDG(CDate(dtpFecha.Text))
        FechaListado = CDate(dtpFecha.Text)

    End Sub

End Class