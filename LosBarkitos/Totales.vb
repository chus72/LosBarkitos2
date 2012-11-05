Imports System.Data.SqlClient
Public Class Totales
    Dim ElForm As Bitmap

    Private Sub Totales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        llenardatos(CDate(dtpInicio.Text), CDate(dtpFinal.Text))
        dtpInicio.Text = Now
        dtpFinal.Text = Now
    End Sub

    Private Sub LlenarDatos(ByVal Fi As DateTime, ByVal Ff As DateTime)

        txtPasPar.Text = CStr(PasajerosParticulares(Fi, Ff))
        txtTotPar.Text = CStr(TotalParticulares(Fi, Ff))
        PasajerosGrupos(Fi, Ff)
        txtTotPas.Text = CStr(CInt(txtPasPar.Text) + CInt(txtPasGru.Text))
        txtTotTot.Text = CStr(CInt(txtTotPar.Text) + CInt(txtPasTot.Text) - CInt(txtComGru.Text))
        EstadisticaLosBarkitos(Fi, Ff)

    End Sub

    Private Function PasajerosParticulares(ByVal Fi As DateTime, ByVal Ff As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim pa, paN As SqlParameter
        Dim fec, fec1 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = " select @part=count(*) from tickets where part='true' and "
        cmd.CommandText = cmd.CommandText & " convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111);"
        cmd.CommandText = cmd.CommandText & " select @partN=count(*) from ticketsN where part='true' and "
        cmd.CommandText = cmd.CommandText & " convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec = New SqlParameter("@fechI", fi)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        fec1 = New SqlParameter("@fechF", ff)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        pa = New SqlParameter("@part", Nothing)
        pa.Direction = ParameterDirection.Output
        cmd.Parameters.Add(pa)
        paN = New SqlParameter("@partN", Nothing)
        paN.Direction = ParameterDirection.Output
        cmd.Parameters.Add(paN)

        Try
            Dim P As Integer
            Dim PN As Integer
            cmd.ExecuteNonQuery()
            If IsDBNull(cmd.Parameters("@part").Value) Then
                P = 0
            Else
                P = cmd.Parameters("@part").Value
            End If
            If IsDBNull(cmd.Parameters("@partN").Value) Then
                PN = 0
            Else
                PN = cmd.Parameters("@partN").Value
            End If
            Return P + PN

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Function

    Private Function TotalParticulares(ByVal Fi As DateTime, ByVal Ff As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim tp, tpN As SqlParameter
        Dim fec, fec1 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = " select @tot=sum(precio) from tickets where part='true' and "
        cmd.CommandText = cmd.CommandText & " convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111);"
        cmd.CommandText = cmd.CommandText & " select @totN=sum(precio) from ticketsN where part='true' and "
        cmd.CommandText = cmd.CommandText & " convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec = New SqlParameter("@fechI", Fi)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        fec1 = New SqlParameter("@fechF", Ff)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        tp = New SqlParameter("@tot", Nothing)
        tp.Direction = ParameterDirection.Output
        cmd.Parameters.Add(tp)
        tpN = New SqlParameter("@totN", Nothing)
        tpN.Direction = ParameterDirection.Output
        cmd.Parameters.Add(tpN)

        Try
            Dim P As Integer
            Dim PN As Integer
            cmd.ExecuteNonQuery()
            If IsDBNull(cmd.Parameters("@tot").Value) Then
                P = 0
            Else
                P = cmd.Parameters("@tot").Value
            End If
            If IsDBNull(cmd.Parameters("@totN").Value) Then
                PN = 0
            Else
                PN = cmd.Parameters("@totN").Value
            End If
            Return P + PN

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Private Sub PasajerosGrupos(ByVal Fi As DateTime, ByVal Ff As DateTime)
        Dim cmd As SqlCommand
        Dim np, st, co As SqlParameter
        Dim fec, fec1 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = "select @npas=sum(Viajes_Grupo.Pax), @sumT=sum(Viajes_Grupo.pax * Grupos.Bruto), @com=sum(Viajes_Grupo.pax * Grupos.Comision)  from Viajes_Grupo INNER JOIN Grupos ON  Viajes_Grupo.Cod_grupo = Grupos.codigo and "
        cmd.CommandText = cmd.CommandText & "convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec = New SqlParameter("@fechI", Fi)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        fec1 = New SqlParameter("@fechF", Ff)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        np = New SqlParameter("@npas", Nothing)
        np.Direction = ParameterDirection.Output
        cmd.Parameters.Add(np)
        st = New SqlParameter("@sumT", Nothing)
        st.Direction = ParameterDirection.Output
        cmd.Parameters.Add(st)
        co = New SqlParameter("@com", Nothing)
        co.Direction = ParameterDirection.Output
        cmd.Parameters.Add(co)

        Try
            Dim P As Integer
            Dim PN As Integer
            Dim C As Integer
            cmd.ExecuteNonQuery()
            If IsDBNull(cmd.Parameters("@npas").Value) Then
                P = 0
            Else
                P = cmd.Parameters("@npas").Value
            End If
            If IsDBNull(cmd.Parameters("@sumT").Value) Then
                PN = 0
            Else
                PN = cmd.Parameters("@sumT").Value
            End If
            If IsDBNull(cmd.Parameters("@com").Value) Then
                C = 0
            Else
                C = cmd.Parameters("@com").Value
            End If

            txtPasGru.Text = CStr(P)
            txtPasTot.Text = CStr(PN)
            txtComGru.Text = CStr(C)
        Catch ex As Exception

            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub EstadisticaLosBarkitos(ByVal Fi As DateTime, ByVal Ff As DateTime)
        Dim cmd As SqlCommand
        Dim ba, tot, ban, totn As SqlParameter
        Dim fec, fec1 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = " select @barc = count(*), @total=sum(precio) from Viaje where "
        cmd.CommandText = cmd.CommandText & " convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111);"
        cmd.CommandText = cmd.CommandText & " select @barcn = count(*), @totaln=sum(precio) from Viajen where "
        cmd.CommandText = cmd.CommandText & " convert(char(10), Fecha, 111)  between  convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec = New SqlParameter("@fechI", Fi)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        fec1 = New SqlParameter("@fechF", Ff)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        ba = New SqlParameter("@barc", Nothing)
        ba.Direction = ParameterDirection.Output
        cmd.Parameters.Add(ba)
        tot = New SqlParameter("@total", Nothing)
        tot.Direction = ParameterDirection.Output
        cmd.Parameters.Add(tot)

        ban = New SqlParameter("@barcn", Nothing)
        ban.Direction = ParameterDirection.Output
        cmd.Parameters.Add(ban)
        totn = New SqlParameter("@totaln", Nothing)
        totn.Direction = ParameterDirection.Output
        cmd.Parameters.Add(totn)

        Try
            Dim P, PP As Integer
            Dim PN, PNPN As Integer
            cmd.ExecuteNonQuery()
            If IsDBNull(cmd.Parameters("@barc").Value) Then
                P = 0
            Else
                P = cmd.Parameters("@barc").Value
            End If
            If IsDBNull(cmd.Parameters("@barcn").Value) Then
                PP = 0
            Else
                PP = cmd.Parameters("@barcn").Value
            End If

            If IsDBNull(cmd.Parameters("@total").Value) Then
                PN = 0
            Else
                PN = cmd.Parameters("@total").Value
            End If
            If IsDBNull(cmd.Parameters("@totaln").Value) Then
                PNPN = 0
            Else
                PNPN = cmd.Parameters("@totaln").Value
            End If

            txtBarcos.Text = CStr(P + PP)
            'txtTotalBkts.Text = CStr(PN + PNPN)
            txtTotalBkts.Text = (PN + PNPN).ToString("#.###,##")
            txtMedia.Text = CStr((PN + PNPN) / (P + PP))


        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub dtpInicio_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpInicio.ValueChanged
        If CDate(dtpInicio.Text) > CDate(dtpFinal.Text) Then
            MessageBox.Show("La fecha inicial no puede ser mayor que la final", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
            dtpInicio.Text = dtpFinal.Text
            Exit Sub
        End If

        LlenarDatos(CDate(dtpInicio.Text), CDate(dtpFinal.Text))
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Me.Close()
    End Sub

    Private Sub dtpFinal_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFinal.ValueChanged
        If CDate(dtpInicio.Text) > CDate(dtpFinal.Text) Then
            MessageBox.Show("La fecha inicial no puede ser mayor que la final", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
            dtpFinal.Text = dtpInicio.Text
            Exit Sub
        End If
        LlenarDatos(CDate(dtpInicio.Text), CDate(dtpFinal.Text))
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        CopiarPantalla()
        PrintDocument1.DefaultPageSettings.Landscape = True
        PrintDocument1.Print()
    End Sub
    Private Sub CopiarPantalla()
        Dim Grphs As Graphics = Me.CreateGraphics()
        ElForm = New Bitmap(Me.Size.Width, Me.Size.Height, Grphs)
        Dim memoryGraphics As Graphics = Graphics.FromImage(ElForm)
        memoryGraphics.CopyFromScreen(Me.Location.X, Me.Location.Y, 0, 0, Me.Size)
    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawImage(ElForm, 0, 0)
    End Sub
End Class