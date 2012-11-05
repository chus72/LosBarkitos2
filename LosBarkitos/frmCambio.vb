Imports System.Data.SqlClient
Public Class frmCambio

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "update caja set cambio = " & CInt(txtCambio.Text)
        cmd.ExecuteScalar()
        Me.Close()

    End Sub
End Class