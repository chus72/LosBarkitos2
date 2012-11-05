Imports System.Data.SqlClient
Public Class Extra

    Private Sub Extra_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim dsDatos As DataSet
        Dim daGuias As SqlDataAdapter

        'El relleno de los dos combobox se pueden hacer de dos formas:
        '1:
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Codigo,Nombre from Grupos order by Nombre "
        lector = cmd.ExecuteReader
        b = lector.Read()
        Do While b = True
            cmbGrupo.Items.Add(lector("Nombre"))
            b = lector.Read()
        Loop
        lector.Close()

        '       cmd.CommandText = "select codigo, Nombre from Guias order by Codigo"
        '       lector = cmd.ExecuteReader
        '       b = lector.Read
        '       Do While b = True
        ' cmbGuia.Items.Add(lector("Nombre"))
        ' b = lector.Read
        ' Loop
        ' lector.Close()
        '2:

        daGuias = New SqlDataAdapter("select codigo, Nombre from Guias order by Nombre", LosBarkitos.cnt)
        dsDatos = New DataSet
        daGuias.Fill(dsDatos, "Guias")
        cmbGuia.DataSource = dsDatos.Tables("Guias")
        cmbGuia.DisplayMember = dsDatos.Tables("Guias").Columns("Nombre").ToString
        cmbGuia.SelectedIndex = 0

    End Sub


    Private Sub btbBlanco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btbBlanco.Click
        Dim cmd As SqlCommand
        Dim NumeroFactura As Integer

        LosBarkitos.chkNegro.Checked = False

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        NumeroFactura = AgregarViajeBDD(CDbl(txtPrecio.Text), CInt(txtPaxGrupo.Text), 1)
        lblNum.Text = CStr(NumeroFactura)

        MsgBox("Grupo Entrado", MsgBoxStyle.Information)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim cmd As SqlCommand
        Dim numGrupo, numGuia As Integer
        Dim pNG, pNGu, pPax, pFec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = "select codigo from Grupos where nombre = '" & cmbGrupo.Text & "'"
        numGrupo = cmd.ExecuteScalar
        cmd.CommandText = "select codigo from Guias where nombre = '" & cmbGuia.Text & "'"
        numGuia = cmd.ExecuteScalar

        cmd.CommandText = "insert into Viajes_Grupo(cod_Grupo, cod_Guia, Fecha, Pax) values (@gru, @gui, @Fec,@pax)"
        pNG = New SqlParameter("@gru", numGrupo)
        pNG.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pNG)

        pNGu = New SqlParameter("@gui", numGuia)
        pNGu.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pNGu)

        pPax = New SqlParameter("@pax", CInt(txtPaxGrupo.Text))
        pPax.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPax)

        pFec = New SqlParameter("@Fec", CDate(dtpFecha.Text))
        pFec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pFec)

        Try
            cmd.ExecuteNonQuery()
            Beep()
        Catch ex As Exception
            MsgBox("No se ha podido introducir el grupo", MsgBoxStyle.Information, ex.InnerException)
        End Try

    End Sub

    Private Sub btnNegro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNegro.Click
        Dim cmd As SqlCommand
        Dim NumeroFactura As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        LosBarkitos.chkNegro.Checked = True

        NumeroFactura = AgregarViajeBDD(CDbl(txtPrecio.Text), CInt(txtPaxGrupo.Text), 1)

        MsgBox("Grupo Entrado", MsgBoxStyle.Information)

    End Sub

    Private Sub btnParticular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParticular.Click
        Dim cmd As SqlCommand
        Dim NumeroFactura As Integer

        LosBarkitos.chkNegro.Checked = False

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        NumeroFactura = AgregarViajeBDD(CDbl(txtPrecio.Text), 1, 1)
        lblNum.Text = CStr(NumeroFactura)

        Beep()
    End Sub

    Private Function AgregarViajeBDD(ByVal Precio As Double, ByVal Num As Integer, ByVal PV As Integer) As Integer
        Dim i As Integer
        Dim NumeroTicket As Integer
        'Oficial
        If LosBarkitos.chkNegro.Checked = False Then
            NumeroTicket = LosBarkitos.TicketFerrySiguiente()
            For i = 1 To Num
                InsertarViajeFerry(NumeroTicket, Precio, PV, dtpFecha.Text)
                NumeroTicket += 1
            Next
            LosBarkitos.GuardarTicketFerrySiguiente(NumeroTicket)
            Return NumeroTicket - 1
            Beep()
            ' Negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            If Num = 1 Then
                Randomize()
                NumeroTicket = CInt(Rnd() * LosBarkitos.TicketFerrySiguiente() - 1)
            ElseIf Num > 1 Then
                NumeroTicket = CInt(LosBarkitos.TicketFerrySiguiente() - Num - 1)
            End If
            For i = 1 To Num
                InsertarViajeFerry(NumeroTicket, Precio, PV, dtpFecha.Text)
                NumeroTicket += 1
            Next
            Return NumeroTicket
            Beep()
        End If

    End Function

    Private Sub InsertarViajeFerry(ByVal NT As Integer, ByVal Pr As Double, ByVal PV As Integer, ByVal Fecha As String)
        Dim cmd As SqlCommand
        Dim pTicket, pPrecio, pPV, pPart, pFec As SqlParameter
        Dim Partis As Boolean

        If Pr = CDbl(10) Or Pr = CDbl(5) Or Pr = CDbl(3) Then
            Partis = True
        Else
            Partis = False
        End If


        ' Viaje marinaFerry en oficial
        If LosBarkitos.chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "insert into Tickets (Numero,Precio ,Fecha, cod_PV,Part) values (@tic,@pre, @fec, @cpv,@par) "
            pTicket = New SqlParameter("@tic", NT)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Pr)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pFec = New SqlParameter("@fec", CDate(Fecha))
            pFec.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pFec)
            pPart = New SqlParameter("@par", Partis)
            pPart.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPart)
            cmd.ExecuteNonQuery()
            ' Viaje MF en Negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "insert into TicketsN (Numero,Precio ,Fecha, cod_PV, Part) values (@tic,@pre, @fec, @cpv,@par) "
            pTicket = New SqlParameter("@tic", NT)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Pr)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pFec = New SqlParameter("@fec", CDate(Fecha))
            pFec.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pFec)

            pPart = New SqlParameter("@par", Partis)
            pPart.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPart)

            cmd.ExecuteNonQuery()

        End If

    End Sub
End Class