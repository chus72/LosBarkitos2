Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Chus.BarControls

Public Class dlgBkt
    Dim DGPrinter As DataGridPrinter
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgBkt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetupDGW()
        LlenarDGW(Now)
    End Sub

    Private Sub LlenarDGW(ByVal Fecha As Date)

        'Primero en A-------------------------------------------------
        Dim Consulta As String
        Dim fec As Date
        DGW.Rows.Clear()
        fec = Format(Fecha, "dd/MM/yyyy")
        Consulta = "SELECT * FROM Viaje WHERE convert(char(10),fecha,103) = '" & Format(Fecha, "dd/MM/yyyy") & "'"
        '        Dim cnt As New SqlConnection("Data Source=CHUSITO\SQLEXPRESS;initial catalog = Tienda;User ID='SERVIDOR'; Password='Jesus'")

        Dim cmd As New SqlCommand(Consulta, LosBarkitos.cnt)
        Dim Lector As SqlDataReader


        Dim Ti As String
        Dim Pr As Integer
        Dim fe As Date


        'Creo una linea de un producto y lo pongo en el datagrid
        Dim Linea As String()

        Lector = cmd.ExecuteReader

        While Lector.Read
            Ti = Lector("cod_Barca").ToString.Trim
            Pr = CDbl(Lector("Precio"))
            fe = Format(Lector("Fecha"), "hh:mm:ss")
            Linea = {Ti, Pr, fe}
            DGW.Rows.Add(Linea)
        End While
        ' PonerTotales()

        Lector.Close()

        ' AHORA EN B--------------------------------------
        Dim LectorB As SqlDataReader
        cmd.CommandText = "SELECT * FROM ViajeN WHERE convert(char(10),fecha,103) = '" & Format(Fecha, "dd/MM/yyyy") & "'"
        '        Dim cnt As New SqlConnection("Data Source=CHUSITO\SQLEXPRESS;initial catalog = Tienda;User ID='SERVIDOR'; Password='Jesus'")

        'Creo una linea de un producto y lo pongo en el datagrid
        Linea = {}

        LectorB = cmd.ExecuteReader

        While LectorB.Read
            Ti = LectorB("cod_Barca").ToString.Trim
            Pr = CDbl(LectorB("Precio"))
            fe = Format(LectorB("Fecha"), "hh:mm:ss")
            Linea = {Ti, Pr, fe}
            DGW.Rows.Add(Linea)
        End While

        LectorB.Close()
        PonerTotales()
    End Sub

    Private Sub PonerTotales()
        Dim Total As String = "Barcas : "
        Dim NTotal As Integer = TotalBarcas(Format(dtpFecha.Value, "dd/MM/yyyy"))
        Dim sDinero As String = "Total :"
        Dim Dinero As Double = TotalEuros(Format(dtpFecha.Value, "dd/MM/yyyy"))
        Dim linea As String() = {Total, NTotal, sDinero, Dinero}

        DGW.Rows.Add(linea)
        Total = "Ferry : "
        NTotal = TotalFerry(Format(dtpFecha.Value, "dd/MM/yyyy"))
        sDinero = "Total : "
        Dinero = TotalEurosF(Format(dtpFecha.Value, "dd/MM/yyyy"))
        linea = {Total, NTotal, sDinero, Dinero}
        DGW.Rows.Add(linea)
    End Sub

    Private Function TotalFerry(ByVal fecha As Date) As Integer
        ' Primero en A-------------------------------------
        Dim cmd As New SqlCommand("SELECT count(*) FROM tickets WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio = '10'", LosBarkitos.cnt)

        Dim ta As Integer = cmd.ExecuteScalar

        ' Ahora en B--------------------------------------
        cmd.CommandText = "SELECT count(*) FROM ticketsN WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio='10'"
        Dim tb As Integer = cmd.ExecuteScalar

        Return ta + tb
    End Function

    Private Function TotalEurosF(ByVal fecha As Date) As Double
        'Primero en A-----------------------------
        Dim cmd As New SqlCommand("SELECT SUM(Precio) FROM Tickets WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio='10'", LosBarkitos.cnt)

        Dim tA As Double = 0
        Try
            tA = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tA = 0
        End Try
        'Ahora en B--------------------------

        cmd.CommandText = "SELECT SUM(Precio) FROM TicketsN WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio='10'"

        Dim tB As Double = 0
        Try
            tB = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tB = 0
        End Try


        Return tA + tB

    End Function


    Private Function TotalFerry(ByVal fecha As Date, ByVal PV As Integer) As Integer
        ' Primero en A-------------------------------------
        Dim cmd As New SqlCommand("SELECT count(*) FROM tickets WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio = '10' and cod_PV = '" & PV & "'", LosBarkitos.cnt)

        Dim ta As Integer = cmd.ExecuteScalar

        ' Ahora en B--------------------------------------
        cmd.CommandText = "SELECT count(*) FROM ticketsN WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio='10' and cod_PV = '" & PV & "'"
        Dim tb As Integer = cmd.ExecuteScalar

        Return ta + tb
    End Function

    Private Function TotalEurosF(ByVal fecha As Date, ByVal PV As Integer) As Double
        'Primero en A-----------------------------
        Dim cmd As New SqlCommand("SELECT SUM(Precio) FROM Tickets WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio='10' and cod_PV = '" & PV & "'", LosBarkitos.cnt)

        Dim tA As Double = 0
        Try
            tA = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tA = 0
        End Try
        'Ahora en B--------------------------

        cmd.CommandText = "SELECT SUM(Precio) FROM TicketsN WHERE convert(char(10),fecha,103) = '" & fecha & "' and Precio='10' and cod_PV = '" & PV & "'"

        Dim tB As Double = 0
        Try
            tB = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tB = 0
        End Try


        Return tA + tB

    End Function

    Private Function TotalBarcas(ByVal fecha As Date) As Integer
        'Primero en A----------------------
        Dim cmd As New SqlCommand("SELECT count(*) FROM Viaje WHERE convert(char(10),fecha,103) = '" & fecha & "'", LosBarkitos.cnt)

        Dim tA As Integer = cmd.ExecuteScalar

        'Primero en B--------------------
        cmd.CommandText = "SELECT count(*) FROM ViajeN WHERE convert(char(10),fecha,103) = '" & fecha & "'"
        Dim tB As Integer = cmd.ExecuteScalar

        Return tA + tB

    End Function

    Private Function TotalBarcasImp(ByVal fecha As Date, ByVal PV As Integer) As Integer
        'Primero en A----------------------
        Dim cmd As New SqlCommand("SELECT count(*) FROM Viaje WHERE convert(char(10),fecha,103) = '" & fecha & "' and cod_PV = '" & PV & "'", LosBarkitos.cnt)

        Dim tA As Integer = cmd.ExecuteScalar

        'Primero en B--------------------
        cmd.CommandText = "SELECT count(*) FROM ViajeN WHERE convert(char(10),fecha,103) = '" & fecha & "' and cod_PV = '" & PV & "'"
        Dim tB As Integer = cmd.ExecuteScalar

        Return tA + tB

    End Function

    Private Function TotalEuros(ByVal fecha As Date) As Double
        'primero en A--------------------
        Dim cmd As New SqlCommand("SELECT SUM(Precio) FROM Viaje WHERE convert(char(10),fecha,103) = '" & fecha & "'", LosBarkitos.cnt)

        Dim tA As Double = 0
        Try
            tA = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tA = 0
        End Try
        'ahora en B--------------------------
        cmd.CommandText = "SELECT SUM(Precio) FROM ViajeN WHERE convert(char(10),fecha,103) = '" & fecha & "'"
        Dim tB As Double = 0
        Try
            tB = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tB = 0
        End Try

        Return tA + tB
    End Function

    Private Function TotalEurosImp(ByVal fecha As Date, ByVal PV As Integer) As Double
        'primero en A--------------------
        Dim cmd As New SqlCommand("SELECT SUM(Precio) FROM Viaje WHERE convert(char(10),fecha,103) = '" & fecha & "' and cod_PV = '" & PV & "'", LosBarkitos.cnt)

        Dim tA As Double = 0
        Try
            tA = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tA = 0
        End Try
        'ahora en B--------------------------
        cmd.CommandText = "SELECT SUM(Precio) FROM ViajeN WHERE convert(char(10),fecha,103) = '" & fecha & "' and cod_PV = '" & PV & "'"
        Dim tB As Double = 0
        Try
            tB = cmd.ExecuteScalar

        Catch ex As Exception
            '        MsgBox(ex.Message)
            tB = 0
        End Try

        Return tA + tB

    End Function

    Private Sub SetupDGW()
        DGW.ColumnCount = 4
        DGW.Width = Me.Width * 0.75
        DGW.Left = Me.Width / 2 - DGW.Width / 2


        With DGW
            .Columns(0).ReadOnly = True
            .Columns(1).ReadOnly = True
            .Columns(2).ReadOnly = True
            .Columns(3).ReadOnly = True

            .Columns(0).Name = "Tipo"
            .Columns(1).Name = "Precio"
            .Columns(2).Name = "Fecha"
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells


            .Font = New Font("Verdana", 9)
        End With
        'ajusto la anchura de dgwlineas
        DGW.Width = (DGW.Columns(0).Width + _
                                    DGW.Columns(1).Width + _
                                    DGW.Columns(2).Width + _
                                    DGW.Columns(3).Width) * 1.5
    End Sub

    '  Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
    '      If SetupImprimir() Then
    '          PrintDocument1.Print()
    '      End If
    ' End Sub



    Private Sub dtpFecha_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFecha.ValueChanged
        DGW.Rows.Clear()
        LlenarDGW(dtpFecha.Text)
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim total As String = Nothing
        Dim more As Boolean = DGPrinter.DrawDataGridView(e.Graphics)
        If more Then
            e.HasMorePages = True
        End If
    End Sub

    Private Function SetupImprimir() As Boolean
        Dim MyPrintDialog As New PrintDialog
        Dim Titulo As String

        If MyPrintDialog.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Return False
        End If
        Titulo = dtpFecha.Text
        PrintDocument1.DocumentName = "Listado Barcas"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(1, 1, 1, 1)

        DGPrinter = New DataGridPrinter(DGW, PrintDocument1, True, True, Titulo, New Font("Verdana", 11, FontStyle.Bold, GraphicsUnit.Point), _
                                      Color.Black, True, True, True)
        Return True
    End Function

    '   Private Sub DGW_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGW.CellContentDoubleClick
    ' Dim Num As Integer = DGW.Item(0, DGW.CurrentRow.Index).Value
    ' Dim Hora As Date = DGW.Item(3, DGW.CurrentRow.Index).Value
    ' Dim res As MsgBoxResult = MsgBox("BORRAR TICKET " & Num, MsgBoxStyle.YesNo, "BORRAR")
    '     If res = MsgBoxResult.Yes Then
    '         BorrarTiquetBkt(Num, Hora)
    '     End If
    '     LlenarDGW(Now)
    ' End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim PVenta As Integer
        Dim tic As Ticket
        tic = New Ticket
        Dim txtPVenta As String = ""
        Dim cmd As New SqlCommand
        PVenta = LosBarkitos.QuePuntoVenta()


        Select Case PVenta
            Case LosBarkitos.cBase
                txtPVenta = "Oficina"
            Case LosBarkitos.cBarkitos
                txtPVenta = "Taquilla"
            Case LosBarkitos.cTPV2
                txtPVenta = "Taquilla"
            Case LosBarkitos.cOficina
                txtPVenta = "Portátil"
        End Select

        tic.AddHeaderLine(dtpFecha.Value.Date & " -- " & txtPVenta)

        tic.AddHeaderLine("")
        tic.AddHeaderLine("----------------------")
        'Primero en A--------------------------------------
        cmd.CommandText = "SELECT * FROM Viaje WHERE convert(char(10),fecha,103) = '" & Format(dtpFecha.Value, "dd/MM/yyyy") & "' and cod_PV = '" & PVenta & "'"
        Dim lector As SqlDataReader

        Dim Ti, Pr, Fe As String
        cmd.Connection = LosBarkitos.cnt
        lector = cmd.ExecuteReader

        While lector.Read
            Ti = lector("cod_Barca").ToString.Trim
            Select Case Ti
                Case "1"
                    Ti = "RIO"
                Case "2"
                    Ti = "ELEC"
                Case "3"
                    Ti = "Gold"
            End Select
            Pr = CDbl(lector("Precio"))
            Fe = Format(lector("Fecha"), "hh:mm:ss")

            tic.AddItem(Ti, "        " & Pr, Fe)

        End While

        lector.Close()
        'Ahora en B --------------------------------------------

        cmd.CommandText = "SELECT * FROM ViajeN WHERE convert(char(10),fecha,103) = '" & Format(dtpFecha.Value, "dd/MM/yyyy") & "' and cod_PV = '" & PVenta & "'"

        lector = cmd.ExecuteReader

        While lector.Read
            Ti = lector("cod_Barca").ToString.Trim
            Select Case Ti
                Case "1"
                    Ti = "RIO"
                Case "2"
                    Ti = "ELEC"
                Case "3"
                    Ti = "Gold"
            End Select
            Pr = CDbl(lector("Precio"))
            Fe = Format(lector("Fecha"), "hh:mm:ss")
            tic.AddItem(Ti, "        " & Pr, Fe)
        End While

        lector.Close()
        tic.AddItem("", "", "")
        'PONGO LOS TOTALES
        Dim NTotal As Integer = TotalBarcasImp(Format(dtpFecha.Value, "dd/MM/yyyy"), PVenta)

        Dim Dinero As Double = TotalEurosImp(Format(dtpFecha.Value, "dd/MM/yyyy"), PVenta)

        tic.AddItem("Barcas: ", "        " & CStr(NTotal), CStr(Dinero))


        NTotal = TotalFerry(Format(dtpFecha.Value, "dd/MM/yyyy"), PVenta)
        Dinero = TotalEurosF(Format(dtpFecha.Value, "dd/MM/yyyy"), PVenta)
        'tic.AddItem("Ferry: ", "        " & CStr(NTotal), CStr(Dinero))

        If PVenta = 1 Then
            tic.PrintTicket("SAMSUNG SRP-350")
        ElseIf PVenta = 2 Then
            tic.PrintTicket("SRP350 Partial Cut")
        ElseIf PVenta = LosBarkitos.cTPV2 Then
            tic.PrintTicket("SRP350 Partial")
        ElseIf PVenta = LosBarkitos.cOficina Then
            tic.PrintTicket("EPSON Stylus Photo R300 Series")
            ' tic.PrintTicket("EPSON Stylus Photo R300 Series")
        End If
        'AbrirCajon()

    End Sub
End Class