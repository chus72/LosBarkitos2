'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports Chus.BarControls

Public Class fAñadirGuia

    Private Sub fAñadirGuia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Añadir GUIA"
        If txtNombreGuia.Text = "" Then
            btnAñadir.Enabled = False
        End If

    End Sub

    Private Sub txtNombreGuia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNombreGuia.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Return) Then
            btnAñadir_Click(sender, e)
        End If
    End Sub

    Private Sub txtNombreGrupo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombreGuia.TextChanged
        If txtNombreGuia.Text <> "" Then
            btnAñadir.Enabled = True
        Else
            btnAñadir.Enabled = False
        End If
    End Sub

    Private Sub btnAñadir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAñadir.Click
        Dim cmd As SqlCommand
        Dim Codigo As Integer
        Dim existe As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        'Primero miramos que el grupo no exista

        cmd.CommandText = "select count(*) from Guias where Nombre = '" & CStr(txtNombreGuia.Text) & "'"
        existe = CInt(cmd.ExecuteScalar)
        If existe = 0 Then
            cmd.CommandText = "select count(*) from Guias"
            Codigo = CInt(cmd.ExecuteScalar)
            cmd.CommandText = "insert into Guias (Codigo, Nombre) values (" & CStr(Codigo) & ", '" & CStr(txtNombreGuia.Text) & "')"
            cmd.ExecuteNonQuery()
        Else
            MsgBox("El guia " & CStr(txtNombreGuia.Text) & " ya existe en la BDD", MsgBoxStyle.Exclamation, "Error!!!")
        End If
        Me.Close()
    End Sub

End Class