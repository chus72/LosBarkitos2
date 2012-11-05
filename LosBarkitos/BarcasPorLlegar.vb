Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports Chus.ChusArrayControles
Imports Chus.LosBarkitos
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Drawing.Printing
Imports Chus.BarControls

Public Class fBarcasPorLlegar
    Public Orden As String

    Private Sub BarcasPorLlegar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Se llena el datagrid por orden de llegada de las barcas
        LlenarDG(Orden)
        Me.Height = DG.Height
    End Sub

    Private Sub LlenarDG(ByVal orden As String)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Barcas.Nombre as [    B a r c a   ], TipoBarca.tipo as [   T i p o  ], "
        cmd.CommandText = cmd.CommandText & " convert(char(8),Barcas.Hora, 108) as [    Hora salida  ], "
        cmd.CommandText = cmd.CommandText & " convert(char(8), Barcas.Reservas, 108) as [   Hora disponible   ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Barcas ON Barcas.Cod_Barca = TipoBarca.codigo "
        cmd.CommandText = cmd.CommandText & " where Barcas.Estado = 'A' "
        If orden = "Hora" Then
            cmd.CommandText = cmd.CommandText & " order by Barcas.Hora"
        ElseIf orden = "Reservas" Then
            cmd.CommandText = cmd.CommandText & " order by Barcas.Reservas"
        End If

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "BarcasFuera")

        DG.DataSource = ds
        DG.DataMember = "BarcasFuera"
        DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Sub

    Private Sub lblDiarioCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDiarioCerrar.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Si esta activado el boton OK quiere decir que esta interesado en una reserva
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lblOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblOk.Click
        'imprime el ticket correspondiente
        'LosBarkitos.lblTicket_Click(sender, e)
        ' Imprime el ticket reserva
        ImprimirTicketReserva(LosBarkitos.TipoBarca, CStr(HoraEstimada(LosBarkitos.TipoBarca)))
        ActualizarHoraDisponible(LosBarkitos.TipoBarca)
        AgregarReservaBDD(ReservaSiguiente(LosBarkitos.TipoBarca), LosBarkitos.TipoBarca, HoraEstimada(LosBarkitos.TipoBarca))
        ActualizarContadorReserva(LosBarkitos.TipoBarca)

        Me.Close()
    End Sub

    Private Sub ImprimirTicketReserva(ByVal TB As Integer, ByVal HE As String)
        Dim tic As Ticket
        Dim PVenta As Integer
        tic = New Ticket
        Dim NumeroReserva As Integer


        PVenta = LosBarkitos.QuePuntoVenta()

        tic.AddHeaderLine(" R E S E R V A ")
        '  tic.AddHeaderLine("C/ Carmançó, 1")
        '  tic.AddHeaderLine("17487 Castelló d'Empúries")
        '  tic.AddHeaderLine("Tel. 972.45.25.79")
        '  tic.AddHeaderLine("Fax. 972.45.63.24")
        '  tic.AddHeaderLine("")

        Select Case TB
            Case 1 : tic.AddItem("1", " Barkito", "Reserva")
            Case 2 : tic.AddItem("1", " Solar", "Reserva")
            Case 3 : tic.AddItem("1", " Gold", "Reserva")
        End Select
        NumeroReserva = ReservaSiguiente(TB)

        tic.FontSize = 9
        tic.AddTotal("NUMERO: ", CStr(NumeroReserva))

        If HE = Nothing Then HE = "--:--:--"
        tic.AddFooterLine("Hora Estimada: " & HE)
        tic.AddFooterLine("--------------------------------")
        tic.AddFooterLine("losbarkitos.com - marinaferry.es")
        tic.AddFooterLine("--------------------------------")
        tic.AddFooterLine("")
        tic.AddFooterLine("")

        If PVenta = 1 Then
            tic.PrintTicket("SAMSUNG SRP-350")
        ElseIf PVenta = 2 Then
            tic.PrintTicket("SRP350 Partial Cut")
        ElseIf PVenta = cTPV2 Then
            tic.PrintTicket("SRP350 Partial")
        ElseIf PVenta = 3 Then
            tic.PrintTicket("EPSON Stylus Photo R300 Series")
        End If
    End Sub

    Private Function ReservaSiguiente(ByVal TB As Integer) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        Select Case TB
            Case 1 : cmd.CommandText = "select NumeroReservaRio from Contadores"
            Case 2 : cmd.CommandText = "select NumeroReservaSolar from Contadores"
            Case 3 : cmd.CommandText = "select NumeroReservaGold from Contadores"
        End Select
        Return CInt(cmd.ExecuteScalar)

    End Function

    Private Sub ActualizarContadorReserva(ByVal TB As Integer)
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        Select Case TB
            Case 1
                cmd.CommandText = "update Contadores set NumeroReservaRio = NumeroReservaRio + 1"
            Case 2
                cmd.CommandText = "update Contadores set NumeroReservaSolar = NumeroReservaSolar + 1"
            Case 3
                cmd.CommandText = "update Contadores set NumeroReservaGold = NumeroReservaGold + 1"
        End Select
        cmd.ExecuteNonQuery()
    End Sub

    Public Function HoraEstimada(ByVal TB As Integer) As String
        Dim cmd As SqlCommand
        Dim tipo As SqlParameter
        Dim Result As String

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select top(1) with ties convert(char(8), Barcas.Reservas, 108)  from  Barcas "
        cmd.CommandText = cmd.CommandText & " where Barcas.Cod_Barca = @T and Barcas.Estado='A'  "
        cmd.CommandText = cmd.CommandText & " order by Barcas.Reservas asc"
        tipo = New SqlParameter("@T", TB)
        tipo.Direction = ParameterDirection.Input
        cmd.Parameters.Add(tipo)

        Result = CStr(cmd.ExecuteScalar)
        If Result = "" Then
            Return "00:00:00"
        Else
            Return Result
        End If
    End Function

    Private Sub ActualizarHoraDisponible(ByVal TB As Integer)
        Dim cmd As SqlCommand
        Dim HE As DateTime
        Dim NumeroBarca As Integer

        NumeroBarca = BuscarPrimeraLibre(TB)

        HE = CDate(HoraEstimada(TB))
        HE = HE.AddHours(1)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "update Barcas set Reservas = '" & HE & "'"
        cmd.CommandText = cmd.CommandText & " where Barcas.Codigo = " & CStr(NumeroBarca) & " and Barcas.Estado='A'  "
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception

        End Try
    End Sub

    Private Function BuscarPrimeraLibre(ByVal TB As Integer) As Integer
        Dim cmd As SqlCommand
        Dim tipo As SqlParameter
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select top(1) with ties Codigo  from  Barcas "
        cmd.CommandText = cmd.CommandText & " where Barcas.Cod_Barca = @T and Barcas.Estado='A'  "
        cmd.CommandText = cmd.CommandText & " order by Barcas.Reservas asc"
        tipo = New SqlParameter("@T", TB)
        tipo.Direction = ParameterDirection.Input
        cmd.Parameters.Add(tipo)

        Return CStr(cmd.ExecuteScalar)
    End Function

    Private Sub AgregarReservaBDD(ByVal N As Integer, ByVal TB As String, ByVal HE As DateTime)
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "insert into Reservas (Numero,TB,HE,Hora,Hecho) values (" & N & "," & TB & ", '" & HE & "' , getdate(),'F')"
        cmd.ExecuteNonQuery()
        Beep()

    End Sub

End Class