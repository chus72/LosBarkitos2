'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports Chus.BarControls
Public Class fEliminarGrupo

    Private Sub fEliminarGrupo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Eliminar GRUPO"
        Dim daGrupos As SqlDataAdapter
        Dim dsDatos As DataSet

        daGrupos = New SqlDataAdapter("select * from Grupos order by Codigo", LosBarkitos.cnt)
        dsDatos = New DataSet
        daGrupos.Fill(dsDatos, "Grupos")
        cmbNombreGrupo.DataSource = dsDatos.Tables("Grupos")
        cmbNombreGrupo.DisplayMember = dsDatos.Tables("Grupos").Columns("Nombre").ToString
        cmbNombreGrupo.SelectedIndex = 0

    End Sub


    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim cmd As SqlCommand

        If (cmbNombreGrupo.Text = "Particulares Adultos" Or _
            cmbNombreGrupo.Text = "Particulares Niños  " Or _
            cmbNombreGrupo.Text = "Sueltos             ") Then

            Me.Close()
            Exit Sub
        End If

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "delete from Grupos where Nombre= '" & CStr(cmbNombreGrupo.Text) & "'"
        cmd.ExecuteNonQuery()
        Me.Close()
    End Sub
End Class