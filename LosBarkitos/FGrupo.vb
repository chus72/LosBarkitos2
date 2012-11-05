Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports Chus.ChusArrayControles
Imports Chus.LosBarkitos
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Public Class FGrupo

    Private Sub FGrupo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim dsDatos As DataSet
        Dim daGuias As SqlDataAdapter

        'El relleno de los dos combobox se pueden hacer de dos formas:
        '1:
        cmd = New SqlCommand
        Try
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select Codigo,Nombre from Grupos  order by Nombre "
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
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click


        LosBarkitos.lblGrupoGuia.Text = cmbGrupo.Text & "   " & cmbGuia.Text
        LosBarkitos.lblTicketsGrupo.Text = txtPax.Text
        LosBarkitos.NombreGrupo = cmbGrupo.Text
        LosBarkitos.NombreGuia = cmbGuia.Text
        '  LosBarkitos.PaxGrupo = CInt(txtPax.Text)
        LosBarkitos.btnCerrarGrupo.Visible = True

        LosBarkitos.lblPrecio.Text = ColocarPrecioGrupo(cmbGrupo.Text)

        Me.Close()
    End Sub

    Private Function ColocarPrecioGrupo(ByVal Grupo As String) As String
        Dim cmd As SqlCommand
        cmd = New SqlCommand

        Try
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select Bruto from Grupos where nombre = '" & Grupo & "'"
            Return String.Format("{0:##.##}", cmd.ExecuteScalar)
        Catch ex As Exception
            MsgBox("Error de BDD", MsgBoxStyle.Exclamation, ex.Message)
            Return "0"
        End Try

    End Function

End Class