Imports System.Data.SqlClient


Public Class Seguridad

    Public cntJ As SqlConnection

    Friend Sub CopiaLBaLBJBak()
        'Copia de seguridad de LosBarkitosJ
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "BACKUP DATABASE [LosBarkitos] TO  DISK = N'C:\Archivos de programa\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\Backup\LosBarkitosJ.bak' WITH NOFORMAT, INIT,  NAME = N'LosBarkitosJ-Completa Base de datos Copia de seguridad', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
        Try
            cmd.ExecuteNonQuery()
            MessageBox.Show("Copia de la BDD finalizada", "COPIADO", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            Beep() : Beep()
        Catch
            MessageBox.Show("No se puede copiar la BDD's")
        End Try
        cmd.CommandText = "BACKUP DATABASE [LosBarkitos] TO  DISK = N'd:\LosBarkitosJ.bak' WITH NOFORMAT, INIT,  NAME = N'LosBarkitosJ-Completa Base de datos Copia de seguridad', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
        Try
            cmd.ExecuteNonQuery()
            Beep() : Beep()
        Catch ex As Exception
            MessageBox.Show("Error en la grabación de la BDD en d:\LosBarkitosJ.bak")
        End Try

    End Sub

    Private Sub CopiaPenDrive()
        'Copia de seguridad de LosBarkitosJ
        Dim cmd As SqlCommand
        Dim backup As Boolean = False
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "BACKUP DATABASE [LosBarkitos] TO  DISK = N'J:\LosBarkitos.bak' WITH NOFORMAT, INIT,  NAME = N'LosBarkitosJ-Completa Base de datos Copia de seguridad', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
        Try
            cmd.ExecuteNonQuery()
            MessageBox.Show("Copia de la BDD finalizada", "COPIADO", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            Beep() : Beep()
            backup = True

        Catch
            MessageBox.Show("No se puede copiar la BDD's")
        End Try

    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

        If txtCodigo.Text = "8082003" Then
            CopiaLBaLBJBak()
        ElseIf txtCodigo.Text = "chusinho" Then
            CopiaPenDrive()
        End If
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub txtCodigo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCodigo.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnAceptar_Click(sender, e)
        End If
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub


End Class