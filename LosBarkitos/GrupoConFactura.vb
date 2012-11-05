
'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports System.Drawing.Printing

Public Class GrupoConFactura

    Private Sub GrupoConFactura_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim dsDatos As DataSet
        Dim daGuias As SqlDataAdapter

        'El relleno de los dos combobox se pueden hacer de dos formas:
        '1:
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Codigo,Nombre from Grupos order by Nombre "
        lector = cmd.ExecuteReader
        b = lector.Read()
        Do While b = True
            cmbGrupo.Items.Add(lector("Nombre"))
            b = lector.Read()
        Loop
        lector.Close()

        '       cmd.CommandText = "select codigo, Nombre from Guias order by Codigo"
        '       lector = cmd.ExecuteReader
        '       b = lector.Read
        '       Do While b = True
        ' cmbGuia.Items.Add(lector("Nombre"))
        ' b = lector.Read
        ' Loop
        ' lector.Close()
        '2:

        daGuias = New SqlDataAdapter("select codigo, Nombre from Guias order by Nombre", LosBarkitos.cnt)
        dsDatos = New DataSet
        daGuias.Fill(dsDatos, "Guias")
        cmbGuia.DataSource = dsDatos.Tables("Guias")
        cmbGuia.DisplayMember = dsDatos.Tables("Guias").Columns("Nombre").ToString
        cmbGuia.SelectedIndex = 0

    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim Pax As Integer

        Try
            Pax = CInt(txtPax.Text)
        Catch ex As Exception
            MsgBox("Error al introducir el número de pasajeros", MsgBoxStyle.Exclamation, "¡ERROR!")
            Exit Sub
        End Try

        IntroducirViaje(cmbGrupo.Text, cmbGuia.Text, Pax)
        Me.Close()

    End Sub

    Private Sub IntroducirViaje(ByVal Grupo As String, ByVal Guia As String, ByVal Pax As Integer)
        Dim cmd As SqlCommand
        Dim numGrupo, numGuia As Integer
        Dim pNG, pNGu, pPax As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt

        cmd.CommandText = "select codigo from Grupos where nombre = '" & cmbGrupo.Text & "'"
        numGrupo = cmd.ExecuteScalar
        cmd.CommandText = "select codigo from Guias where nombre = '" & cmbGuia.Text & "'"
        numGuia = cmd.ExecuteScalar

        cmd.CommandText = "insert into Viajes_Grupo(cod_Grupo, cod_Guia, Fecha, Pax) values (@gru, @gui, getdate(),@pax)"
        pNG = New SqlParameter("@gru", numGrupo)
        pNG.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pNG)

        pNGu = New SqlParameter("@gui", numGuia)
        pNGu.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pNGu)

        pPax = New SqlParameter("@pax", CInt(txtPax.Text))
        pPax.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPax)
        Try
            cmd.ExecuteNonQuery()
            MsgBox("Introducido", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox("No se ha podido introducir el grupo", MsgBoxStyle.Information, ex.InnerException)
        End Try

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class