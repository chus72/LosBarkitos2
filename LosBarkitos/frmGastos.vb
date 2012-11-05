Imports System.Data.SqlClient
Public Class frmGastos

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim cmd As SqlCommand
        Dim G As Integer

        cmd = New SqlCommand

        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Gastos from Caja"
        G = CInt(cmd.ExecuteScalar)

        cmd.CommandText = "update caja set Gastos = " & G + CInt(txtGastos.Text)
        cmd.ExecuteNonQuery()

        Me.Close()
    End Sub


    Private Sub btnaceptar_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        e.SuppressKeyPress = False
        Select Case e.KeyCode
            Case Keys.Enter
                btnAceptar_Click(btnAceptar, e)

        End Select
    End Sub
End Class