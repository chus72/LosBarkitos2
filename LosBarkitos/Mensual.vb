Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports Chus.ChusArrayControles
Imports Chus.LosBarkitos
Imports System.Data.SqlClient


Public Class fMensual
    Dim MesListado As String
    Dim DGPrinter As DataGridPrinter
    Dim Mes(12) As String
    Dim Ano(6) As String
    Dim m As DateTime


    Private Sub Mensual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DG.Height = Me.Height

        Ano(0) = "2008"
        Ano(1) = "2009"
        Ano(2) = "2010"
        Ano(3) = "2011"
        Ano(4) = "2012"
        Ano(5) = "2013"
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
        Label1.Text = m.Year
        'Añado los años que quiero que salgan
        cmbAno.Items.Add(Ano(0))
        cmbAno.Items.Add(Ano(1))
        cmbAno.Items.Add(Ano(2))
        cmbAno.Items.Add(Ano(3))
        cmbAno.Items.Add(Ano(4))
        cmbAno.Items.Add(Ano(5))
        cmbMes.Text = Mes(m.Month - 1)
        cmbAno.Text = m.Year
        pnlControles.Left = CInt((DG.Left + DG.Width + Me.Width) / 2) - CInt(pnlControles.Width / 2)
        DG.Width = Me.Width * 0.65
        DG.Height = Me.Height * 0.8

        DG.Left = 1
        pnlControles.Left = DG.Width + DG.Left

        LlenarDG(cmbMes.SelectedIndex + 1)
        DG.Columns.Item(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub

    Private Sub LlenarDG(ByVal F As Integer)

        If LosBarkitos.chkNegro.Checked = False Then
            ListadoA(F)

            'Listado en negro del los barkitos
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            ListadoB(F)

        End If
    End Sub

    Private Function ListadoA(ByVal F As Integer) As DataSet
        Dim cmd As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select  convert(char(10),Viaje.Fecha,103) as [       F e c h a    ], count(*) as [Viajes], round(sum(Viaje.Precio)/1.21,2) as [        B a s e     ], "
        cmd.CommandText = cmd.CommandText & " convert(decimal(5,2),round((sum(viaje.Precio)-sum(Viaje.Precio)/1.21),2)) as [       I . V . A    ], "
        cmd.CommandText = cmd.CommandText & " convert(decimal(7,2),round(sum(Viaje.Precio),2)) as [       T o t a l    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Viaje ON Viaje.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & " Barcas ON Barcas.Codigo = Viaje.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = Viaje.cod_PV"
        cmd.CommandText = cmd.CommandText & " where datepart(mm, Viaje.Fecha) =  " & CStr(F) & " and datepart(yyyy,Viaje.Fecha) = " & cmbAno.Text
        cmd.CommandText = cmd.CommandText & " group by convert(char(10),Viaje.Fecha,103)"
        cmd.CommandText = cmd.CommandText & " order by convert(char(10),Viaje.Fecha,103)"
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Mes")

        DG.DataSource = ds
        DG.DataMember = "Mes"


        lblTotalMes.Text = "Total: " & SumarMes(F) & " €"
        lblViajesMes.Text = "Viajes: " & SumarViajesMes(F)
        DG.Columns.Item(1).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(2).DefaultCellStyle.Format = "###.##"
        DG.Columns.Item(3).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(4).DefaultCellStyle.Format = "####.##"

        DG.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Return ds

    End Function


    Private Function ListadoB(ByVal F As Integer)
        Dim cmd As SqlCommand
        Dim ds, dsa As DataSet
        Dim da As SqlDataAdapter
 
        'RecogerDatosA(F, V, P)

        dsa = ListadoA(F)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select  convert(char(10),ViajeN.Fecha,103) as [       F e c h a    ],count(*) as [Viajes], round(sum(ViajeN.Precio)/1.21,2) as [        B a s e     ], "
        cmd.CommandText = cmd.CommandText & " convert(decimal(5,2),round((sum(viajeN.Precio)-sum(ViajeN.Precio)/1.21),2)) as [       I . V . A    ], "
        cmd.CommandText = cmd.CommandText & " convert(decimal(7,2),round(sum(ViajeN.Precio),2)) as [       T o t a l    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " ViajeN ON ViajeN.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & " Barcas ON Barcas.Codigo = ViajeN.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = ViajeN.cod_PV"
        cmd.CommandText = cmd.CommandText & " where datepart(mm, ViajeN.Fecha) =  " & CStr(F) & " and datepart(yyyy,ViajeN.Fecha) = " & cmbAno.Text
        cmd.CommandText = cmd.CommandText & " group by convert(char(10),ViajeN.Fecha,103)"
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmd
        da.Fill(ds, "Mes")

        'ds.Merge(dsa)
        DG.DataSource = ds
        DG.DataMember = "Mes"

        lblTotalMes.Text = "Total: " & SumarMes(F) & " €"
        lblViajesMes.Text = "Viajes: " & SumarViajesMes(F)

        'Sumamos el valor del dataset A en el datagrid
        DG.Columns.Item(1).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(2).DefaultCellStyle.Format = "###.##"
        DG.Columns.Item(3).DefaultCellStyle.Format = "####.##"
        DG.Columns.Item(4).DefaultCellStyle.Format = "####.##"

        DG.Columns.Item(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DG.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Return ds

    End Function

    Private Sub RecogerDatosA(ByVal F As Integer, ByRef V As Integer, ByRef P As Double)
        Dim cmd As New SqlCommand
        Dim lector As SqlDataReader

        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select  convert(char(10),Viaje.Fecha,103) as [       F e c h a    ], count(*) as [Viajes], round(sum(Viaje.Precio)/1.21,2) as [        B a s e     ], "
        cmd.CommandText = cmd.CommandText & " convert(decimal(5,2),round((sum(viaje.Precio)-sum(Viaje.Precio)/1.21),2)) as [       I . V . A    ], "
        cmd.CommandText = cmd.CommandText & " convert(decimal(7,2),round(sum(Viaje.Precio),2)) as [       T o t a l    ] from  TipoBarca INNER JOIN"
        cmd.CommandText = cmd.CommandText & " Viaje ON Viaje.cod_Barca = TipoBarca.codigo INNER JOIN "
        cmd.CommandText = cmd.CommandText & " Barcas ON Barcas.Codigo = Viaje.cod_Barca INNER JOIN PuntoVenta ON PuntoVenta.Codigo = Viaje.cod_PV"
        cmd.CommandText = cmd.CommandText & " where datepart(mm, Viaje.Fecha) =  " & CStr(F) & " and datepart(yyyy,Viaje.Fecha) = " & cmbAno.Text
        cmd.CommandText = cmd.CommandText & " group by convert(char(10),Viaje.Fecha,103)"
        cmd.CommandText = cmd.CommandText & " order by convert(char(10),Viaje.Fecha,103)"

        lector = cmd.ExecuteReader

        Dim ind As Integer = 1
        Try
            Do While lector.Read
               
            Loop
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Private Function SumarViajesMes(ByVal f As Integer) As Integer

        If LosBarkitos.chkNegro.Checked = False Then
            Return SumarViajesMesA(f)
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            Return SumarViajesMesB(f)
        End If

    End Function

    Private Function SumarViajesMesA(ByVal f As Integer) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(Viaje.Precio) from viaje "
        cmd.CommandText = cmd.CommandText & " where datepart(mm, Viaje.Fecha) =  " & CStr(f) & " and datepart(yyyy,Viaje.Fecha) = " & cmbAno.Text
        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function SumarViajesMesB(ByVal F As Integer) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter
        Dim SumA As Integer

        SumA = SumarViajesMesA(F)
        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select count(ViajeN.Precio) from viajeN "
        cmd.CommandText = cmd.CommandText & " where datepart(mm, ViajeN.Fecha) =  " & CStr(F) & " and datepart(yyyy,ViajeN.Fecha) = " & cmbAno.Text
        fec = New SqlParameter("@fech", F)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar + SumA

        Catch ex As Exception
            Return SumA
        End Try

    End Function

    Private Function SumarMes(ByVal f As Integer) As Double

        If LosBarkitos.chkNegro.Checked = False Then

            If LosBarkitos.HayDiasMesBkts(f) Then
                Return SumarMesA(f)
            Else
                Return 0
            End If

            'En negro
        ElseIf LosBarkitos.chkNegro.Checked = True Then
            If LosBarkitos.HayDiasMesBkts(f) Then
                Return SumarMesB(f)
            Else
                Return SumarMesA(f)
            End If

        End If
    End Function

    Private Function SumarMesA(ByRef f As Integer) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(Viaje.Precio) from viaje "
        cmd.CommandText = cmd.CommandText & " where datepart(mm, Viaje.Fecha) =  " & CStr(f) & " and datepart(yyyy,Viaje.Fecha) = " & cmbAno.Text
        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar
        Catch
            Return 0
        End Try
    End Function

    Private Function SumarMesB(ByVal f As Integer) As Integer
        Dim cmd As SqlCommand
        Dim fec As SqlParameter
        Dim SumA As Integer

        SumA = SumarMesA(f)

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(ViajeN.Precio) from viajeN "
        cmd.CommandText = cmd.CommandText & " where datepart(mm, ViajeN.Fecha) =  " & CStr(f) & " and datepart(yyyy,ViajeN.Fecha) = " & cmbAno.Text
        fec = New SqlParameter("@fech", f)
        fec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(fec)
        Try
            Return cmd.ExecuteScalar + SumA
        Catch
            Return SumA
        End Try
    End Function


    Private Sub lblCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblCerrar.Click
        Me.Close()
    End Sub

    Private Sub cmbMes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMes.SelectedIndexChanged
        If cmbAno.Text = "" Then cmbAno.Text = m.Year

        LlenarDG(cmbMes.SelectedIndex + 1)
    End Sub

    Private Sub lblImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblImprimir.Click
        If SetupImprimir() Then
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
        If LosBarkitos.chkNegro.Checked = True Then Titulo = Titulo & " (N)"

        PrintDocument1.DocumentName = "Customers Report"
        PrintDocument1.PrinterSettings = MyPrintDialog.PrinterSettings
        PrintDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        PrintDocument1.DefaultPageSettings.Margins = New Printing.Margins(10, 10, 10, 10)


        'Envio el documento a imprimir
        DGPrinter = New DataGridPrinter(DG, PrintDocument1, True, True, Titulo, New Font("Tahoma", 24, FontStyle.Bold, GraphicsUnit.Point), _
            Color.Black, True, True, False, 0, 0, SumarViajesMes(cmbMes.SelectedIndex + 1), SumarMes(cmbMes.SelectedIndex + 1))

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


    Private Sub cmbAno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAno.SelectedIndexChanged
        LlenarDG(cmbMes.SelectedIndex + 1)
        Label1.Text = cmbAno.Text

    End Sub

End Class