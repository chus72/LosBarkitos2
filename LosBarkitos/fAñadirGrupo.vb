'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports Chus.BarControls

Public Class fAñadirGrupo

    Private Sub fAñadirGrupo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Añadir GRUPO"
        If txtNombreGrupo.Text = "" Then
            btnAñadir.Enabled = False
        End If
    End Sub

    Private Sub txtNombreGrupo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNombreGrupo.KeyPress
        If e.KeyChar = (Convert.ToChar(Keys.Return)) Then
            btnAñadir_Click(sender, e)
        End If
    End Sub

    Private Sub txtNombreGrupo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombreGrupo.TextChanged
        If txtNombreGrupo.Text <> "" Then
            btnAñadir.Enabled = True
        Else
            btnAñadir.Enabled = False
        End If
    End Sub



    Private Sub btnAñadir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAñadir.Click
        Dim cmd As SqlCommand
        Dim Codigo As Integer
        Dim Existe As Integer
        Dim pNombre, pCodigo As SqlParameter


        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "Select count(*) from Grupos where Nombre = @nom"
        pNombre = New SqlParameter("@nom", CStr(txtNombreGrupo.Text))
        pNombre.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pNombre)

        Existe = cmd.ExecuteScalar
        If Existe = 0 Then

            cmd.CommandText = "select count(*) from Grupos"
            Codigo = CInt(cmd.ExecuteScalar) - 2 'Porque las 3 ultimos no son grupos y tienen codigos altos

            cmd.CommandText = "insert into Grupos (Codigo, Nombre) values (@cod, @nom)"
            pCodigo = New SqlParameter("@cod", CStr(Codigo))
            pCodigo.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pCodigo)
            cmd.ExecuteNonQuery()
        Else
            MsgBox("El grupo " & CStr(txtNombreGrupo.Text) & " ya existe en la BDD.", MsgBoxStyle.Exclamation, "Error!!!")
        End If
        Me.Close()
    End Sub
End Class