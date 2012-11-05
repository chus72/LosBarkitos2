'Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class ModificacionTicketBkt
    Dim modificacion As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        If modificacion Then
            Dim cmd As SqlCommand
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            If LosBarkitos.chkNegro.Checked = False Then
                cmd.CommandText = "update Viaje set Precio= " & CInt(Me.txtPrecio.Text) & " , Adultos= " & CInt(Me.txtAdultos.Text)
                cmd.CommandText = cmd.CommandText & " , Niños = " & CInt(Me.txtNiños.Text) & " , Cod_Barca= " & CInt(Me.cmbTipoBarca.SelectedIndex + 1)
                cmd.CommandText = cmd.CommandText & " where Viaje.numero = " & CInt(Me.txtNumFact.Text)
                cmd.ExecuteNonQuery()
            Else
                cmd.CommandText = "update ViajeN set Precio= " & CInt(Me.txtPrecio.Text.Trim) & " , Adultos= " & CInt(Me.txtAdultos.Text.Trim)
                cmd.CommandText = cmd.CommandText & " , Niños = " & CInt(Me.txtNiños.Text.Trim) & " , Cod_Barca= " & CInt(Me.cmbTipoBarca.SelectedIndex + 1)
                cmd.CommandText = cmd.CommandText & " where ViajeN.numero = " & CInt(Me.txtNumFact.Text.Trim)
                cmd.ExecuteNonQuery()
            End If
            fDiario.LlenarDG(CDate(fDiario.dtpFecha.Text))
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub txtPrecio_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrecio.TextChanged
        modificacion = True
    End Sub

    Private Sub ModificacionTicketBkt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        modificacion = False

        If LosBarkitos.QuePuntoVenta() = LosBarkitos.cOficina Then
            btnEliminar.Visible = True
        Else
            btnEliminar.Visible = False
        End If

        'Me.btnImprimir_click(sender, e)
    End Sub

    Private Sub txtBarca_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        modificacion = True
    End Sub

    Private Sub txtNiños_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNiños.TextChanged
        modificacion = True
    End Sub

    Private Sub txtAdultos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAdultos.TextChanged
        modificacion = True
    End Sub

    Private Sub cmbTipoBarca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoBarca.SelectedIndexChanged
        modificacion = True
    End Sub

    Private Sub btnImprimir_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        LosBarkitos.TipoBarca = 1
        LosBarkitos.ImprimirTicket("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\LogoBarkitos.jpg", _
                       CInt(Me.txtNumFact.Text), CDbl(txtPrecio.Text), CInt(Me.cmbTipoBarca.SelectedIndex + 1))

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = "delete from viajeN where Numero= '" & CInt(Me.txtNumFact.Text.Trim) & "' and   convert(char(10),ViajeN.Fecha,103) = '" & Mid(Me.txtFecha.Text.Trim, 1, 10) & "'  and convert(char(8),ViajeN.Fecha,108) = '" & Mid(Me.txtFecha.Text.Trim, 12, 8) & "'"
        cmd.ExecuteNonQuery()
        Me.Close()
    End Sub
End Class
