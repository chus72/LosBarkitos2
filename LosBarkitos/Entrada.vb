Public Class Entrada


    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnAceptar_Click(sender, e)
        End If
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If TextBox1.Text <> "qawsedrftg" Then
            End
        Else
            LosBarkitos.Visible = True
            Me.Close()
        End If
    End Sub

    Private Sub Entrada_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LosBarkitos.Visible = False
    End Sub
End Class