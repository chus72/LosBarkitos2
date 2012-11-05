Imports System.Data.SqlClient

Public Class ExtraP
    Dim NumeroFactura As Integer

    Private Sub Rios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button
        btn = TryCast(sender, Button)
        NumeroFactura = AgregarViajeBDD(CDbl(btn.Text), 0, 0, 2, 1)
        lblNum.Text = CStr(NumeroFactura)

    End Sub

    Private Sub Solar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button
        btn = TryCast(sender, Button)
        NumeroFactura = AgregarViajeBDD(CDbl(btn.Text), 0, 0, 2, 2)
        lblNum.Text = CStr(NumeroFactura)
    End Sub

    Private Function AgregarViajeBDD(ByVal Precio As Double, ByVal A As Integer, ByVal N As Integer, ByVal PV As Integer, ByVal TB As Integer) As Integer
        Dim cmd As SqlCommand
        Dim pTicket, pPrecio, pA, pN, pPV, pTP, pFec As SqlParameter
        Dim NumeroTicket As Integer = TicketSiguiente()


        If chkNegro.Checked = False Then

            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "insert into Viaje (Numero,Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@tic,@pre, @fec ,@adu,@nin,@cpv,@ctb) "
            pTicket = New SqlParameter("@tic", NumeroTicket)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Precio)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pA = New SqlParameter("@adu", A)
            pA.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pA)
            pN = New SqlParameter("@nin", N)
            pN.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pN)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pTP = New SqlParameter("@ctb", TB)
            pTP.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTP)
            pFec = New SqlParameter("@Fec", CDate(DateTimePicker1.Text))
            pFec.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pFec)
            cmd.ExecuteNonQuery()
            GuardarTicketSiguiente(NumeroTicket + 1)
            Beep()
            '    lblMedia.Text = CStr(Media())
            Return NumeroTicket

            'Tiquet de barkitos en negro
        ElseIf chkNegro.Checked = True Then
            Randomize()
            NumeroTicket = CInt(10000 * Rnd() + 1)
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "insert into ViajeN (Numero,Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@tic,@pre, @Fec ,@adu,@nin,@cpv,@ctb) "
            pTicket = New SqlParameter("@tic", NumeroTicket)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Precio)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pA = New SqlParameter("@adu", A)
            pA.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pA)
            pN = New SqlParameter("@nin", N)
            pN.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pN)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pTP = New SqlParameter("@ctb", TB)
            pTP.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTP)
            pFec = New SqlParameter("@Fec", CDate(DateTimePicker1.Text))
            cmd.Parameters.Add(pFec)
            cmd.ExecuteNonQuery()
            'GuardarTicketSiguiente(NumeroTicket + 1)
            Beep()
            'lblMedia.Text = CStr(Media())
            Return NumeroTicket

        End If
    End Function

    Private Sub ExtraP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each boton As Button In Panel1.Controls
            AddHandler boton.Click, AddressOf Rios_Click
        Next
        For Each boton1 As Button In Panel2.Controls
            AddHandler boton1.Click, AddressOf Solar_Click
        Next
    End Sub

    Private Function TicketSiguiente() As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Numero_Ticket from Contadores"
        Try
            Return CInt(cmd.ExecuteScalar)

        Catch ex As Exception
            Return 999
        End Try
    End Function

    Private Sub GuardarTicketSiguiente(ByVal t As Integer)
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "update Contadores set Numero_Ticket = " & CStr(t)
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnROtro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnROtro.Click
        NumeroFactura = AgregarViajeBDD(CDbl(txtPrecioRio.Text), 0, 0, 2, 1)
        lblNum.Text = CStr(NumeroFactura)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumeroFactura = AgregarViajeBDD(CDbl(txtPrecioSolar.Text), 0, 0, 2, 1)
        lblNum.Text = CStr(NumeroFactura)

    End Sub

    Private Sub btnEntrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEntrar.Click
        Dim cmd As SqlCommand
        Dim pFec, pTot As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "insert into Fotos (Total, Dia) values (@tot, @fec) "
        pFec = New SqlParameter("@fec", CDate(DateTimePicker1.Text))
        pFec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pFec)

        pTot = New SqlParameter("@tot", txtFotos.Text)
        pTot.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pTot)

        cmd.ExecuteNonQuery()
        Beep()
        txtFotos.Text = ""

    End Sub


End Class