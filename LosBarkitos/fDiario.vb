'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports System.Drawing.Printing


Public Class fDiario
    Dim FechaListado As DateTime
    Dim DGPrinter As DataGridPrinter



    Private Sub fDiario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        DG.Height = Me.Height * 0.9
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left
        FechaListado = Now
        LlenarDG(Now)
        DG.Columns.Item(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        If LosBarkitos.QuePuntoVenta = LosBarkitos.cBase Then
            LosBarkitos.chkNegro.Checked = False
        End If
        'Poner las columnas a no editables
        For i As Integer = 0 To 4
            DG.Columns.Item(i).ReadOnly = True
        Next
    End Sub

    Public Sub LlenarDG(ByVal F As DateTime)

        'Si el listado es desde el ordenador CELIA sera siempre en BLANCO!!!

        If LosBarkitos.chkNegro.Visible = False Then
            LosBarkitos.chkNegro.Checked = False
        End If

        If LosBarkitos.chkNegro.Checked = False Then
            ListadoA(F)
            'Listado en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            ListadoB(F)
        End If
    End Sub

    Private Function ListadoA(ByVal F As DateTime) As DataSet
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim fec As SqlParameter
        Dim Dias As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Viaje.Numero as [       Factura      ], TipoBarca.tipo as [      B a r c a    ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  Viaje.Fecha as [       F e c h a    ], convert(decimal(6,2),Viaje.Precio) as [     P r e c i o    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Viaje ON Viaje.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & "Barcas ON Barcas.Codigo = Viaje.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = Viaje.cod_PV"
        cmd.CommandText = cmd.CommandText & " where convert(char(10),Viaje.Fecha,103) = convert(char(10), @fech ,103) "

        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viaje.cod_PV =  " & LosBarkitos.cTPV2
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and Viaje.cod_PV = '1' "
        End If
        cmd.CommandText = cmd.CommandText & " order by Viaje.Numero"

        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        DG.DataSource = ds
        DG.DataMember = "Diario"

        Dias = SumarViajesDia(F)
        lblNumViajes.Text = "Viajes: " & Dias
        If Dias > 0 Then
            lblTotal.Text = "Total: " & SumarDia(F) & " €"
        Else
            lblTotal.Text = "Total: 0 €"
        End If
        Return ds

    End Function



    Private Sub ListadoB(ByVal F As DateTime)

        Dim cmd As SqlCommand
        Dim ds, dsa As DataSet
        Dim da As SqlDataAdapter
        Dim fec As SqlParameter
        Dim Dias As Integer

        dsa = ListadoA(F)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select ViajeN.Numero as [       Factura      ], TipoBarca.tipo as [      B a r c a    ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  ViajeN.Fecha as [       F e c h a    ], convert(decimal(5,2),ViajeN.Precio) as [     P r e c i o    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " ViajeN ON ViajeN.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & "Barcas ON Barcas.Codigo = ViajeN.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = ViajeN.cod_PV"
        cmd.CommandText = cmd.CommandText & " where convert(char(10),ViajeN.Fecha,103) = convert(char(10), @fech ,103)"
        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viajeN.cod_PV = '2' "
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and ViajeN.cod_PV = '1' "
        End If
        cmd.CommandText = cmd.CommandText & " order by ViajeN.Numero"

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

        Dias = SumarViajesDia(F)
        lblNumViajes.Text = "Viajes: " & Dias
        If Dias > 0 Then
            lblTotal.Text = "Total: " & SumarDia(F) & " €"
        Else
            lblTotal.Text = "Total: 0 €"
        End If

    End Sub

    Private Function SumarViajesDia(ByVal f As DateTime) As Integer

        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarViajesDiaA(f)

            'Total en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarViajesDiaA(f) + SumarViajesDiaB(f)
        End If

    End Function

    Private Function SumarViajesDiaA(ByVal F As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(Viaje.Precio) from viaje where convert(char(10),Viaje.Fecha,103) = convert(char(10), @fech ,103) "
        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viaje.cod_PV =  '" & LosBarkitos.cTPV2 & "'"
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and viaje.cod_PV = '1'"
        End If
        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        Return cmd.ExecuteScalar
    End Function

    Private Function SumarViajesDiaB(ByVal F As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(ViajeN.Precio) from viajeN where convert(char(10),ViajeN.Fecha,103) = convert(char(10), @fech ,103) "
        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viajeN.cod_PV = ' " & LosBarkitos.cTPV2 & "'"
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and ViajeN.cod_PV = '1' "
        End If

        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try


    End Function

    Private Function SumarDia(ByVal f As DateTime) As Double

        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarDiaA(f)
            'En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarDiaB(f)
        End If
    End Function

    Private Function SumarDiaA(ByVal F As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(Viaje.Precio) from viaje where convert(char(10),Viaje.Fecha,103) = convert(char(10), @fech ,103) "
        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viaje.cod_PV = ' " & LosBarkitos.cTPV2 & "'"
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and Viaje.cod_PV = '1' "
        End If

        fec = New SqlParameter("@fech", F)
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
        Dim fec As SqlParameter
        Dim SumA As Integer

        SumA = SumarDiaA(f)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(ViajeN.Precio) from viajeN where convert(char(10),ViajeN.Fecha,103) = convert(char(10), @fech ,103) "
        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viajeN.cod_PV = ' " & LosBarkitos.cTPV2 & "'"
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and ViajeN.cod_PV = '1' "
        End If

        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar + SumA
        Catch ex As Exception
            Return SumA
        End Try
    End Function

    Private Sub lblFechaHoy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFechaHoy.Click
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

        Titulo = "Listado día : " & CStr(CDate(dtpFecha.Text))
        If LosBarkitos.chkNegro.Checked = True Then Titulo = Titulo & " (N)"

        PrintDocument1.DocumentName = "Informe Barkitos"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(1, 1, 1, 1)


        'Envio el documento a imprimir, en caso de que sea oficial le quito la fecha
        If LosBarkitos.chkNegro.Checked = False Then
            DG = PrepararDGImprimir(DG, dtpFecha.Text)
        End If

        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, True, True, SumarViajesDia(FechaListado), SumarDia(FechaListado), 0, 0)

        Return True
    End Function

    Function PrepararDGImprimir(ByVal DG As DataGridView, ByVal f As DateTime) As DataGridView
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Viaje.Numero as [       Factura      ], TipoBarca.tipo as [      B a r c a    ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & " convert(decimal(5,2),Viaje.Precio) as [     P r e c i o    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Viaje ON Viaje.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & "Barcas ON Barcas.Codigo = Viaje.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = Viaje.cod_PV"
        cmd.CommandText = cmd.CommandText & " where convert(char(10),Viaje.Fecha,103) = convert(char(10), @fech ,103)"
        If rbBarkitos.Checked Then
            cmd.CommandText = cmd.CommandText & " and viaje.cod_PV = '2' "
        End If
        If rbMarina.Checked Then
            cmd.CommandText = cmd.CommandText & " and Viaje.cod_PV = '1' "
        End If
        cmd.CommandText = cmd.CommandText & " order by Viaje.Numero"

        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        DG.DataSource = ds
        DG.DataMember = "Diario"
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

    ''' <summary>
    ''' Aqui se puede modificar un tiquet desde el listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DG_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DG.CellDoubleClick
        Dim fact As Integer
        Dim celda As DataGridViewCell
        Dim fecha As String

        celda = DG.Item(0, DG.CurrentRow.Index)
        'Solo hay una fila y esta en blanco
        If DG.RowCount = 1 Then Return
        fact = CInt(celda.Value)
        celda = DG.Item(3, DG.CurrentRow.Index)
        fecha = celda.Value
        MostrarTicket("bkt", fact, fecha)
    End Sub

    Private Sub MostrarTicket(ByVal Tipo As String, ByVal NumFact As Integer, ByVal fecha As String)
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader

        ModificacionTicketBkt.Show()

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        If LosBarkitos.chkNegro.Checked = False Then
            cmd.CommandText = "select viaje.precio, viaje.fecha, viaje.adultos, viaje.niños, "
            cmd.CommandText = cmd.CommandText & "PuntoVenta.Nombre as NomPV, TipoBarca.tipo as NomBarca from viaje inner join PuntoVenta on viaje.cod_PV=PuntoVenta.Codigo inner join TipoBarca on "
            cmd.CommandText = cmd.CommandText & "Viaje.cod_Barca=TipoBarca.codigo where viaje.numero = '" & NumFact & "' and  convert(char(10),Viaje.Fecha,103) = '" & Mid(fecha, 1, 10) & "' and convert(char(8),viaje.fecha,108) = '" & Mid(fecha, 12, 8) & "'"
        Else
            cmd.CommandText = "select viajeN.precio, viajeN.fecha, viajeN.adultos, viajeN.niños, "
            cmd.CommandText = cmd.CommandText & "PuntoVenta.Nombre as NomPV, TipoBarca.tipo as NomBarca from viajeN inner join PuntoVenta on viajeN.cod_PV=PuntoVenta.Codigo inner join TipoBarca on "
            cmd.CommandText = cmd.CommandText & "ViajeN.cod_Barca=TipoBarca.codigo where viajeN.numero = '" & NumFact & "' and  convert(char(10),ViajeN.Fecha,103) = '" & Mid(fecha, 1, 10) & "' and convert(char(8),viajen.fecha,108) = '" & Mid(fecha, 12, 8) & "'"
        End If

        lector = cmd.ExecuteReader

        ModificacionTicketBkt.txtNumFact.Text = CStr(NumFact)
        lector.Read()
        ModificacionTicketBkt.txtFecha.Text = lector("Fecha")
        ModificacionTicketBkt.txtPrecio.Text = Format(lector("Precio"), "Standard")
        ModificacionTicketBkt.txtAdultos.Text = lector("Adultos")
        ModificacionTicketBkt.txtNiños.Text = lector("niños")
        ModificacionTicketBkt.txtPuntoVenta.Text = lector("NomPV")
        ModificacionTicketBkt.cmbTipoBarca.Text = lector("NomBarca")
        lector.Close()

        If ModificacionTicketBkt.cmbTipoBarca.Text.Trim = "Rio" Then
            ModificacionTicketBkt.cmbTipoBarca.SelectedIndex = 0
        ElseIf ModificacionTicketBkt.cmbTipoBarca.Text.Trim = "Solar" Then
            ModificacionTicketBkt.cmbTipoBarca.SelectedIndex = 1
        Else
            ModificacionTicketBkt.cmbTipoBarca.SelectedIndex = 2
        End If

        ModificacionTicketBkt.Show()

    End Sub



    Private Sub dtpFecha_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFecha.ValueChanged
        LlenarDG(CDate(dtpFecha.Text))
        FechaListado = CDate(dtpFecha.Text)

    End Sub

    Private Sub rbBarkitos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBarkitos.CheckedChanged
        LlenarDG(CDate(dtpFecha.Text))
        FechaListado = CDate(dtpFecha.Text)
    End Sub

    Private Sub rbTotal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTotal.CheckedChanged
        If dtpFecha.Text = "" Then
            LlenarDG(Now)
            FechaListado = Now
        Else
            LlenarDG(CDate(dtpFecha.Text))
            FechaListado = CDate(dtpFecha.Text)
        End If
    End Sub



    Private Sub rbMarina_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMarina.CheckedChanged
        LlenarDG(CDate(dtpFecha.Text))
        FechaListado = CDate(dtpFecha.Text)
    End Sub


End Class