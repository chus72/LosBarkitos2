'Imports Microsoft.VisualBasic
'Imports System
'Imports System.Windows.Forms
'Imports System.Drawing
'Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient

Public Class GruposMes
    Dim MesListado As String
    Dim DGPrinter As DataGridPrinter
    Dim Mes(12) As String

    Private Sub GruposMes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean

        Dim m As DateTime
        DG.Height = Me.Height * 0.9


        Mes(0) = "Enero"
        Mes(1) = "Febrero"
        Mes(2) = "Marzo"
        Mes(3) = "Abril"
        Mes(4) = "Mayo"
        Mes(5) = "Junio"
        Mes(6) = "Julio"
        Mes(7) = "Agosto"
        Mes(8) = "Septiembre"
        Mes(9) = "Octubre"
        Mes(10) = "Noviembre"
        Mes(11) = "Diciembre"
        m = Now
        For i As Integer = 0 To 11
            cmbMes.Items.Add(Mes(i))
        Next
        cmbMes.Text = Mes(m.Month - 1)
        cmbAno.Text = CStr(m.Year)
        pnlControles.Left = CInt((DG.Left + DG.Width + Me.Width) / 2) - CInt(pnlControles.Width / 2)
        DG.Width = Me.Width * 0.65
        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left

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

        LlenarDG(cmbMes.SelectedIndex + 1, cmbAno.Text)

        DG.Columns.Item(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub LlenarDG(ByVal F As Integer, ByVal Ano As String)
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim nom As SqlParameter


        ' En oficial
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select  convert(char(10),Viajes_Grupo.Fecha,103) as [       F e c h a    ],Grupos.Nombre as [    G r u p o   ], Guias.Nombre as [    G u í a   ],Pax as [Pasajeros], Viajes_Grupo.Pax * Grupos.Bruto as [Bruto],Grupos.Comision * Viajes_Grupo.Pax as [Comision], Viajes_Grupo.Pax * Grupos.Bruto - Viajes_Grupo.Pax * Grupos.Comision as [N e t o] "
        cmd.CommandText = cmd.CommandText & " from Viajes_Grupo inner join Grupos on Viajes_Grupo.cod_Grupo = Grupos.codigo inner join Guias on Viajes_Grupo.cod_Guia = Guias.codigo "
        cmd.CommandText = cmd.CommandText & " where datepart(mm, Viajes_Grupo.Fecha)= " & CStr(F) & " and "
        cmd.CommandText = cmd.CommandText & " Grupos.nombre = @nom "
        cmd.CommandText = cmd.CommandText & " and datepart(yyyy, Viajes_Grupo.Fecha)= " & CStr(Ano)
        cmd.CommandText = cmd.CommandText & " order by viajes_grupo.fecha"

        nom = New SqlParameter("@nom", cmbGrupo.Text)
        nom.Direction = ParameterDirection.Input
        cmd.Parameters.Add(nom)

        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Mes")

        DG.DataSource = ds
        DG.DataMember = "Mes"

        DG.Columns.Item(4).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(5).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(6).DefaultCellStyle.Format = "###.##"
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        lblTotal.Text = CStr(SumarViajesMes(cmbMes.SelectedIndex + 1, cmbAno.Text))


    End Sub

    Private Sub cmbMes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMes.SelectedIndexChanged
        If CStr(cmbAno.Text) = "" Then
            cmbAno.Text = CStr(Now.Year)
        End If
        LlenarDG(cmbMes.SelectedIndex + 1, cmbAno.Text)
    End Sub

    Private Sub lblDiarioCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDiarioCerrar.Click
        Me.Close()
    End Sub

    Private Sub lblImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblImprimir.Click
        If SetupImprimir Then
            PrintDocument1.Print()
        End If

    End Sub
    Private Function SetupImprimir() As Boolean
        Dim MyPrintDialog As New PrintDialog()
        Dim Titulo As String
        Dim m As Integer

        m = cmbMes.SelectedIndex

        MyPrintDialog.AllowCurrentPage = False
        MyPrintDialog.AllowPrintToFile = False
        MyPrintDialog.AllowSelection = False
        MyPrintDialog.AllowSomePages = False
        MyPrintDialog.PrintToFile = False
        MyPrintDialog.ShowHelp = False
        MyPrintDialog.ShowNetwork = False

        If MyPrintDialog.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Return False
        End If

        Titulo = "Listado mes: " & Mes(m)

        PrintDocument1.DocumentName = "Customers Report"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(10, 10, 10, 10)


        'Envio el documento a imprimir
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, True, False, 0, 0, SumarViajesMes(cmbMes.SelectedIndex + 1, CStr(cmbAno.Text)), SumarMes(cmbMes.SelectedIndex + 1, CStr(cmbAno.Text)))

        Return True
    End Function

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim more As Boolean = DGPrinter.DrawDataGridView(e.Graphics)
        If more Then
            e.HasMorePages = True
        End If
    End Sub

    Private Sub btnPrintPreview_Click(ByVal sender As Object, ByVal e As EventArgs)

        If SetupImprimir() Then

            Dim MyPrintPreviewDialog As PrintPreviewDialog = New PrintPreviewDialog()
            MyPrintPreviewDialog.Document = PrintDocument1
            MyPrintPreviewDialog.ShowDialog()
        End If
    End Sub

    Private Function SumarViajesMes(ByVal f As Integer, ByVal A As String) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter
        Dim nom As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        ' En oficial
        cmd.CommandText = "select count(*) from Viajes_Grupo inner join grupos on Viajes_Grupo.cod_Grupo = Grupos.codigo "
        cmd.CommandText = cmd.CommandText & " where datepart(mm, Viajes_Grupo.Fecha)= " & CStr(f) & " and datepart(yyyy,Viajes_Grupo.Fecha) = " & CStr(A) & "and "
        cmd.CommandText = cmd.CommandText & " Grupos.nombre = @nom"

        nom = New SqlParameter("@nom", cmbGrupo.Text)
        nom.Direction = ParameterDirection.Input
        cmd.Parameters.Add(nom)


        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Return cmd.ExecuteScalar

    End Function
    Private Function SumarMes(ByVal f As Integer, ByVal A As String) As Double
        Dim cmd As SqlCommand
        Dim fec As SqlParameter


        If LosBarkitos.HayDiasMesFerry(f) Then
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            ' En oficial
            If LosBarkitos.chkNegro.Checked = False Then
                cmd.CommandText = "select sum(Tickets.Precio) from Tickets, Viajes_grupo "
                cmd.CommandText = cmd.CommandText & " where datepart(mm, Tickets.Fecha) =  " & CStr(f) & " and datepart(yyyy,Viajes_Grupo.Fecha) = " & CStr(A)
                ' En negro
            ElseIf LosBarkitos.chkNegro.Checked = True Then
                cmd.CommandText = "select sum(TicketsN.Precio) from TicketsN, Viajes_grupo "
                cmd.CommandText = cmd.CommandText & " where datepart(mm, TicketsN.Fecha) =  " & CStr(f) & " and datepart(yyyy,Viajes_Grupo.Fecha) = " & CStr(A)
            End If
            fec = New SqlParameter("@fech", f)
            fec.Direction = ParameterDirection.Input
            cmd.Parameters.Add(fec)
            Return cmd.ExecuteScalar
        Else
            Return 0
        End If
    End Function

    Private Sub cmbAno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAno.SelectedIndexChanged
        LlenarDG(cmbMes.SelectedIndex + 1, CStr(cmbAno.Text))
    End Sub

    Private Sub cmbGrupo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGrupo.SelectedIndexChanged
        LlenarDG(cmbMes.SelectedIndex + 1, CStr(cmbAno.Text))
    End Sub
End Class