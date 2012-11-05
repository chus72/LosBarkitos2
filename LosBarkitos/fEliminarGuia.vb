'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports Chus.BarControls
Public Class fEliminarGuia

    Private Sub fEliminarGrupo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Eliminar GUIA"
        Dim daGuias As SqlDataAdapter
        Dim dsDatos As DataSet

        daGuias = New SqlDataAdapter("select * from Guias order by Codigo", LosBarkitos.cnt)
        dsDatos = New DataSet
        daGuias.Fill(dsDatos, "Guias")
        cmbNombreGuia.DataSource = dsDatos.Tables("Guias")
        cmbNombreGuia.DisplayMember = dsDatos.Tables("Guias").Columns("Nombre").ToString
        cmbNombreGuia.SelectedIndex = 0

    End Sub


    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim cmd As SqlCommand

        If cmbNombreGuia.Text = "---                 " Then
            Me.Close()
            Exit Sub
        End If
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "delete from Guias where Nombre= '" & CStr(cmbNombreGuia.Text) & "'"
        cmd.ExecuteNonQuery()
        Me.Close()
    End Sub
End Class