Imports System.Data.SqlClient
'Imports System.Linq

Public Class AcotadoBarkitos
    Dim FechaListado As DateTime
    Dim DGPrinter As DataGridPrinter

    Private Sub AcotadoBarkitos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFinal.Text = Now
        dtpInicio.Text = Now
        DG.Height = Me.Height * 0.9
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left
        FechaListado = Now
        LlenarDG(CDate(dtpInicio.Text), CDate(dtpFinal.Text))
        DG.Columns.Item(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub

    Public Sub LlenarDG(ByVal FI As DateTime, ByVal FF As DateTime)

        If LosBarkitos.chkNegro.Checked = False Then
            ListadoA(FI, FF)
            'Listado en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            ListadoB(FI, FF)
        End If
    End Sub

    Private Function ListadoA(ByRef FI As DateTime, ByVal FF As DateTime) As DataSet
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim fec, fec1 As SqlParameter
        Dim Dias As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select Viaje.Numero as [       Factura      ], TipoBarca.tipo as [      B a r c a    ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  Viaje.Fecha as [       F e c h a    ], convert(decimal(6,2),Viaje.Precio) as [     P r e c i o    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Viaje ON Viaje.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & "Barcas ON Barcas.Codigo = Viaje.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = Viaje.cod_PV"
        cmd.CommandText = cmd.CommandText & " where convert(char(10),Viaje.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10), @fechF ,111) order by Viaje.Fecha"
        fec = New SqlParameter("@fechI", FI)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        fec1 = New SqlParameter("@fechF", FF)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        DG.DataSource = ds
        DG.DataMember = "Diario"
        DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dias = SumarViajesDia(FI, FF)
        lblNumViajes.Text = "Viajes: " & Dias
        If Dias > 0 Then
            lblTotal.Text = "Total: " & SumarDia(FI, FF) & " €"
        End If
        Return ds
    End Function

    Private Function ListadoB(ByVal FI As DateTime, ByVal FF As DateTime) As DataSet
        Dim cmd As SqlCommand
        Dim ds, dsa As DataSet
        Dim da As SqlDataAdapter
        Dim fec, fec1 As SqlParameter
        Dim Dias As Integer

        dsa = New DataSet
        dsa = ListadoA(FI, FF)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select ViajeN.Numero as [       Factura      ], TipoBarca.tipo as [      B a r c a    ], PuntoVenta.Nombre as [      Punto Venta   ],"
        cmd.CommandText = cmd.CommandText & "  ViajeN.Fecha as [       F e c h a    ], convert(decimal(5,2),ViajeN.Precio) as [     P r e c i o    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " ViajeN ON ViajeN.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & "Barcas ON Barcas.Codigo = ViajeN.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = ViajeN.cod_PV"
        cmd.CommandText = cmd.CommandText & " where convert(char(10),ViajeN.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111) order by ViajeN.fecha"

        fec = New SqlParameter("@fechI", FI)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)

        fec1 = New SqlParameter("@fechF", FF)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Diario")

        ds.Merge(dsa)
        DG.DataSource = ds
        DG.DataMember = "Diario"
        DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dias = SumarViajesDia(FI, FF)
        lblNumViajes.Text = "Viajes: " & Dias
        If Dias > 0 Then
            lblTotal.Text = "Total: " & SumarDia(FI, FF) & " €"
        End If
        Return ds
    End Function

    Private Function SumarViajesDia(ByVal FI As DateTime, ByVal FF As DateTime) As Integer
   

        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarViajesDiaA(FI, FF)
            'Total en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarViajesDiaB(FI, FF)
        End If

    End Function

    Private Function SumarViajesDiaA(ByVal FI As DateTime, ByVal FF As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec1, fec2 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(Viaje.Precio) from viaje where convert(char(10),Viaje.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec1 = New SqlParameter("@fechI", FI)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        fec2 = New SqlParameter("@fechF", FF)
        fec2.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec2)
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function SumarViajesDiaB(ByVal FI As DateTime, ByVal FF As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec1, fec2 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(ViajeN.Precio) from viajeN where convert(char(10),ViajeN.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec1 = New SqlParameter("@fechI", FI)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        fec2 = New SqlParameter("@fechF", FF)
        fec2.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec2)
        Try
            Return SumarViajesDiaA(FI, FF) + cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Function SumarDia(ByVal FI As DateTime, ByVal FF As DateTime) As Double


        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarDiaA(FI, FF)
            'En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarDiaB(FI, FF)
        End If
    End Function

    Private Function SumarDiaA(ByVal FI As DateTime, ByVal FF As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec1, fec2 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(Viaje.Precio) from viaje where convert(char(10),Viaje.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec1 = New SqlParameter("@fechI", FI)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        fec2 = New SqlParameter("@fechF", FF)
        fec2.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec2)
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Function SumarDiaB(ByVal FI As DateTime, ByVal FF As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec1, fec2 As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(ViajeN.Precio) from viajeN  where convert(char(10),ViajeN.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

        fec1 = New SqlParameter("@fechI", FI)
        fec1.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec1)

        fec2 = New SqlParameter("@fechF", FF)
        fec2.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec2)
        Try
            Return SumarDiaA(FI, FF) + cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try

    End Function


    Private Sub lblCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblCerrar.Click
        Me.Close()
    End Sub

    Private Sub dtpInicio_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpInicio.ValueChanged
        LlenarDG(CDate(dtpInicio.Text), dtpFinal.Text)
        FechaListado = CDate(dtpInicio.Text)
    End Sub

    Private Sub dtpFinal_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFinal.ValueChanged
        LlenarDG(CDate(dtpInicio.Text), dtpFinal.Text)
        FechaListado = CDate(dtpInicio.Text)

    End Sub
End Class