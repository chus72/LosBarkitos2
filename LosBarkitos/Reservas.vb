'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports System.Data.SqlTypes
'Imports System.Drawing.Printing
'Imports Chus.BarControls

Public Class Reservas
    Public TipoReserva As Integer

    Private Sub Reservas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LlenarDG(TipoReserva)
        Me.Height = DG.Height
        DG.Height = Me.Height * 0.9
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        lblOk.Left = DG.Left + DG.Width * 1.21

    End Sub

    Private Sub LlenarDG(ByVal TipoReserva As Integer)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Reservas.numero as [    Número Reserva   ], TipoBarca.Tipo as [   T i p o  ], "
        cmd.CommandText = cmd.CommandText & " convert(char(8),Reservas.hora, 108) as [    Hora reserva  ], "
        cmd.CommandText = cmd.CommandText & " convert(char(8), Reservas.HE, 108) as [   Hora Estimada   ] from  TipoBarca inner join "
        cmd.CommandText = cmd.CommandText & " Reservas ON Reservas.TB = TipoBarca.codigo "
        If TipoReserva <> 0 Then
            cmd.CommandText = cmd.CommandText & " where TipoBarca.codigo = " & TipoReserva
        End If
        cmd.CommandText = cmd.CommandText & " and Reservas.Hecho= 'F'"
        cmd.CommandText = cmd.CommandText & " order by Reservas.numero"

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "reservas")

        DG.DataSource = ds
        DG.DataMember = "reservas"
        DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Sub

    Private Sub lblOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblOk.Click
        Me.Close()
    End Sub

    Private Sub DG_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DG.CellContentClick
        Dim NumReserva As Integer
        Dim celda As DataGridViewCell


        celda = DG.Item(0, DG.CurrentRow.Index)
        'Solo hay una fila y esta en blanco
        If DG.RowCount = 1 Then Return
        NumReserva = CInt(celda.Value)
        MarcarReservaHecha(NumReserva)
        LlenarDG(TipoReserva)
    End Sub

    Private Sub MarcarReservaHecha(ByVal NumReserva As Integer)
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "update Reservas set Hecho='T' where Reservas.Numero= " & NumReserva
        cmd.ExecuteNonQuery()
    End Sub
End Class