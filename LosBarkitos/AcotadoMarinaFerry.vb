Imports System.Data.SqlClient
'Imports System.Linq

Public Class AcotadoMarinaFerry
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
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub

    Public Sub LlenarDG(ByVal FI As DateTime, ByVal FF As DateTime)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim fec, fec1 As SqlParameter
        Dim Dias As Integer

        ' Dim db As New losbarkitoslinqtosqldatacontext


        'Result = From res In LosBarkitos.cnt _
        'Where()
        If LosBarkitos.chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select Tickets.Numero as [       Factura      ], PuntoVenta.Nombre as [      Punto Venta   ], "
            cmd.CommandText = cmd.CommandText & "  Tickets.Fecha as [       F e c h a    ], convert(decimal(6,2),Tickets.Precio) as [     P r e c i o    ] from Tickets INNER JOIN "
            cmd.CommandText = cmd.CommandText & "  PuntoVenta ON PuntoVenta.Codigo = Tickets.cod_PV "
            cmd.CommandText = cmd.CommandText & " where convert(char(10),Tickets.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10), @fechF ,111) order by Tickets.Fecha"
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
            DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dias = SumarViajesDia(FI, FF)
            lblNumViajes.Text = "Tickets: " & Dias
            If Dias > 0 Then
                lblTotal.Text = "Total: " & SumarDia(FI, FF) & " €"
            End If

            'Listado en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select TicketsN.Numero as [       Factura      ], PuntoVenta.Nombre as [      Punto Venta   ], "
            cmd.CommandText = cmd.CommandText & "  TicketsN.Fecha as [       F e c h a    ], convert(decimal(6,2),TicketsN.Precio) as [     P r e c i o    ] from  TicketsN INNER JOIN "
            cmd.CommandText = cmd.CommandText & "  PuntoVenta ON PuntoVenta.Codigo = TicketsN.cod_PV  "
            cmd.CommandText = cmd.CommandText & " where convert(char(10),TicketsN.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10), @fechF ,111) order by TicketsN.Fecha"

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
            DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dias = SumarViajesDia(FI, FF)
            lblNumViajes.Text = "Tickets: " & Dias
            If Dias > 0 Then
                lblTotal.Text = "Total: " & SumarDia(FI, FF) & " €"
            End If

        End If
    End Sub

    Private Function SumarViajesDia(ByVal FI As DateTime, ByVal FF As DateTime) As Integer
        Dim cmd As SqlCommand
        Dim fec1, fec2 As SqlParameter

        If LosBarkitos.chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select count(Tickets.Precio) from Tickets where convert(char(10),Tickets.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

            fec1 = New SqlParameter("@fechI", FI)
            fec1.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec1)

            fec2 = New SqlParameter("@fechF", FF)
            fec2.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec2)

            Return cmd.ExecuteScalar

            'Total en negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select count(TicketsN.Precio) from TicketsN where convert(char(10),TicketsN.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

            fec1 = New SqlParameter("@fechI", FI)
            fec1.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec1)

            fec2 = New SqlParameter("@fechF", FF)
            fec2.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec2)

            Return cmd.ExecuteScalar

        End If

    End Function

    Private Function SumarDia(ByVal FI As DateTime, ByVal FF As DateTime) As Double
        Dim cmd As SqlCommand
        Dim fec1, fec2 As SqlParameter

        If LosBarkitos.chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select sum(Tickets.Precio) from Tickets where convert(char(10),Tickets.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

            fec1 = New SqlParameter("@fechI", FI)
            fec1.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec1)

            fec2 = New SqlParameter("@fechF", FF)
            fec2.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec2)

            Return cmd.ExecuteScalar

            'En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = "select sum(TicketsN.Precio) from TicketsN  where convert(char(10),TicketsN.Fecha,111) between convert(char(10), @fechI ,111) and convert(char(10) , @fechF , 111)"

            fec1 = New SqlParameter("@fechI", FI)
            fec1.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec1)

            fec2 = New SqlParameter("@fechF", FF)
            fec2.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec2)

            Return cmd.ExecuteScalar

        End If
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