Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports Chus
Imports Chus.ChusArrayControles
Imports Chus.LosBarkitos
Imports System.Data.SqlClient
Imports System.Drawing.Printing

Public Class Resumen

    Dim Fecha As String

    Private Sub Resumen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        dtpFecha.Value = Now

    End Sub

    Private Sub PonerDatos()

        Dim cmd As SqlCommand
        Dim temp1, temp2 As Integer

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        'Cogemos la fecha del cuadro 
        'Fecha = Convert.ToString(Format(CDate(dtpFecha.Value), "short date"))
        Fecha = Convert.ToString(Format(CDate(dtpFecha.Value), "short date"))

        ' COLOCAMOS EL PRIMER TAB PARA LOS PARTICULARES

        lblAdultos.Text = CStr(CStr(TAdultos(Fecha)) & " => " & CStr(SumarParticularesAdultos(Fecha)))
        lblNiños.Text = CStr(CStr(TNiños(Fecha)) & " => " & CStr(SumarParticularesNiños(Fecha)))


        'Total Pax
        lblTotalPax.Text = CStr(TAdultos(Fecha) + TNiños(Fecha))

        lblTotalParticulares.Text = CStr(SumarParticularesAdultos(Fecha) + SumarParticularesNiños(Fecha)) & " €"

        '-------------------------------------------------------------------------------------------------
        'SEGUNDO TAB PARA LOS GRUPOS sin la comision

        lblSueltos.Text = CStr(PaxSueltos(Fecha))

        lblInserso.Text = CStr(PaxInserso(Fecha))

        lblTotalGrupos.Text = CStr(CInt(lblSueltos.Text) + CInt(lblInserso.Text))

        lblTotalGruposEuros.Text = CStr(SumarSueltos(Fecha) + Inserso(Fecha))
        '-----------------------------------------------------------------------------------------------
        'TERCER TAB PARA LOS GRUPOS DE ISRAEL

        'Grupos de Saul

        lblSaul.Text = CStr(PaxSaul(Fecha))
        lblAquatravel.Text = CStr(PaxAquatravel(Fecha))
        lblOtrosIsrael.Text = CStr(PaxIsrael(Fecha))

        lblTotalIsrael.Text = CStr(CInt(lblSaul.Text) + CInt(lblAquatravel.Text) + CInt(lblOtrosIsrael.Text))

        lblTotalIsraelEuros.Text = CStr(SumarSaul(Fecha) + SumarAquatravel(Fecha) + SumarIsrael(Fecha))


        '================================================================================================
        'LOS BARKITOS
        'Rios:
        cmd.CommandText = "select count(*) from Viaje where convert(char(10), Fecha, 103) = '" & Fecha & "' "
        cmd.CommandText = cmd.CommandText & " and cod_Barca='1'"
        Try
            temp1 = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            MsgBox(ex.Message)
            temp1 = 0
        End Try
        cmd.CommandText = "select count(*) from ViajeN where convert(char(10), Fecha, 103) = '" & Fecha & "' "
        cmd.CommandText = cmd.CommandText & " and cod_Barca='1'"
        Try
            temp2 = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            temp2 = 0
        End Try
        lblRios.Text = CStr(temp1 + temp2)

        'Solares:
        cmd.CommandText = "select count(*) from Viaje where convert(char(10), Fecha, 103) = '" & Fecha & "' "
        cmd.CommandText = cmd.CommandText & " and cod_Barca='2'"
        Try
            temp1 = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            temp1 = 0
        End Try
        cmd.CommandText = "select count(*) from ViajeN where convert(char(10), Fecha, 103) = '" & Fecha & "' "
        cmd.CommandText = cmd.CommandText & " and cod_Barca='2'"
        Try
            temp2 = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            temp2 = 0
        End Try
        lblSolares.Text = CStr(temp1 + temp2)
        'Gold:
        cmd.CommandText = "select count(*) from Viaje where convert(char(10), Fecha, 103) = '" & Fecha & "' "
        cmd.CommandText = cmd.CommandText & " and cod_Barca='3'"
        Try
            temp1 = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            temp1 = 0
        End Try
        cmd.CommandText = "select count(*) from ViajeN where convert(char(10), Fecha, 103) = '" & Fecha & "' "
        cmd.CommandText = cmd.CommandText & " and cod_Barca='3'"
        Try
            temp2 = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            temp2 = 0
        End Try
        lblGold.Text = CStr(temp1 + temp2)

        'TotalBarcas:
        lblTotalBarcas.Text = CStr(CInt(lblGold.Text) + CInt(lblRios.Text) + CInt(lblSolares.Text))

        lblTotalEurosOficina.Text = CStr(TotalEurosOficina(Fecha))
        lblTotalEurosLosBarkitos.Text = CStr(TotalEurosBarkitos(Fecha))
        lblTotalEuros.Text = CStr(CInt(lblTotalEurosOficina.Text) + CInt(lblTotalEurosLosBarkitos.Text))
        'Porcentaje
        lblMedia.Text = (CInt(lblTotalEuros.Text) / CInt(lblTotalBarcas.Text)).ToString("##.##")

        '======================================================================================
        'ESTO VA EL EL TAB DE IMPRESION
        txtTexto.Text = ""
        txtTexto.Text = txtTexto.Text & "MARINAFERRY" & vbCrLf & "---------------------" & vbCrLf
        txtTexto.Text = txtTexto.Text & "Particulares:" & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Adultos    : " & lblAdultos.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Niños      : " & lblNiños.Text & vbCrLf & vbCrLf
        txtTexto.Text = txtTexto.Text & "Grupos:" & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Sueltos:  " & lblSueltos.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Inserso:  " & lblInserso.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "Israel:" & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Saul      : " & lblSaul.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Aquatravel: " & lblAquatravel.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "   Otros     : " & lblOtrosIsrael.Text & vbCrLf

        txtTexto.Text = txtTexto.Text & "___________________________________________" & vbCrLf
        txtTexto.Text = txtTexto.Text & "LOS BARKITOS" & vbCrLf & "-------------------" & vbCrLf
        txtTexto.Text = txtTexto.Text & "Total Barcas: " & lblTotalBarcas.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "        Rios:    " & lblRios.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "        Solares: " & lblSolares.Text & vbCrLf
        txtTexto.Text = txtTexto.Text & "        Gold:    " & lblGold.Text & vbCrLf

        txtTexto.Text = txtTexto.Text & "Total:           " & lblTotalEuros.Text & " €" & vbCrLf

        txtTexto.Text = If(lblMedia.Text = "", txtTexto.Text & "Media por barca: --" & vbCrLf, txtTexto.Text & "Media por barca: " & CDbl(lblMedia.Text).ToString("##.##") & " €" & vbCrLf)
        txtTexto.Text = txtTexto.Text & vbCrLf & vbCrLf & "______________________________________" & vbCrLf
        txtTexto.Text = txtTexto.Text & "TOTAL GENERAL : " & CInt(lblTotalParticulares.Text) + CInt(lblTotalGruposEuros.Text) + CInt(lblTotalIsraelEuros.Text) + CInt(lblTotalEuros.Text) & " €."

        TotalFotos(Fecha)

    End Sub


    Private Sub dtpFecha_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFecha.ValueChanged
        PonerDatos()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        PrintDialog1.Document = PrintDocument1
        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub VistaPrevia(ByVal TipoFuente As String, ByVal TamañoFuente As Byte, _
ByVal TextoImpresion As String, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim Fuente As New Font(TipoFuente, TamañoFuente)
        Dim AreaImpresion_Alto, AreaImpresion_Ancho, MargenIzquierdo, MargenSuperior, MargenCentrado As Integer
        Dim Titulo As String = "RESUMEN DIA " & dtpFecha.Text & vbCrLf & vbCrLf
        Dim FuenteTitulo As New Font("Verdana", 20, FontStyle.Bold)
        Dim SangriaMF As Integer = MargenIzquierdo + 250
        Dim SangriaLB As Integer

        With Me.PrintDocument1.DefaultPageSettings
            'Area Neta de Impresion (se descuenta los margenes)
            AreaImpresion_Alto = .PaperSize.Height - .Margins.Top - .Margins.Bottom
            AreaImpresion_Ancho = .PaperSize.Width - .Margins.Left - .Margins.Right
            MargenIzquierdo = .Margins.Left
            MargenSuperior = .Margins.Top
            MargenCentrado = .PaperSize.Width / 2
            SangriaLB = MargenCentrado + 270

            'Verificar si se ha elegido el modo horizontal
            If .Landscape Then
                Dim NroTemp As Integer
                NroTemp = AreaImpresion_Alto
                AreaImpresion_Alto = AreaImpresion_Ancho
                AreaImpresion_Ancho = NroTemp
            End If

            Dim Formato As New StringFormat(StringFormatFlags.LineLimit)

            Dim Rectangulo As New RectangleF(MargenIzquierdo, MargenSuperior + FuenteTitulo.Height, _
            AreaImpresion_Ancho, AreaImpresion_Alto - FuenteTitulo.Height)
            Dim NroLineasImpresion As Integer = CInt(AreaImpresion_Alto / Fuente.Height)
            Dim NroLineasRelleno, NroLetrasLinea As Integer
            Static CaracterActual As Integer

            'Esto esta añadido por mi y es el TITULO 
            Dim RectanguloTitulo As New RectangleF(MargenIzquierdo, MargenSuperior, _
            AreaImpresion_Ancho, FuenteTitulo.Height)

            Dim FormatoMio As StringFormat
            FormatoMio = New StringFormat
            FormatoMio.Alignment = StringAlignment.Center
            e.Graphics.MeasureString(Titulo, FuenteTitulo, New SizeF(AreaImpresion_Ancho, FuenteTitulo.Height), FormatoMio)
            e.Graphics.DrawString(Titulo, FuenteTitulo, Brushes.Blue, RectanguloTitulo, FormatoMio)
            '-------------------------------------------------------

            Dim FuenteCabecera As New Font("Verdana", 14, FontStyle.Bold)
            Dim RectMF As New RectangleF(MargenIzquierdo, MargenSuperior + 2 * FuenteTitulo.Height, AreaImpresion_Ancho, FuenteCabecera.Height)
            Dim RectLB As New RectangleF(MargenCentrado, MargenSuperior + 2 * FuenteTitulo.Height, AreaImpresion_Ancho, FuenteCabecera.Height)

            'Estos son los títulos de las empresas
            e.Graphics.MeasureString("MARINA FERRY", FuenteCabecera, New SizeF(AreaImpresion_Ancho, FuenteTitulo.Height), Formato, NroLetrasLinea, NroLineasRelleno)
            e.Graphics.DrawString("MARINA FERRY", FuenteCabecera, Brushes.Chocolate, RectMF, Formato)
            e.Graphics.MeasureString("LOS BARKITOS", FuenteCabecera, New SizeF(AreaImpresion_Ancho, FuenteTitulo.Height), Formato, NroLetrasLinea, NroLineasRelleno)
            e.Graphics.DrawString("LOS BARKITOS", FuenteCabecera, Brushes.Chocolate, RectLB, Formato)
            e.Graphics.DrawLine(Pens.Red, RectMF.Left, RectMF.Bottom, RectMF.Right, RectMF.Bottom)

            'Esto son los subtitulos
            Dim FuenteSubTitulo As New Font("Verdana", 12, FontStyle.Underline)
            Dim MargenParticulares As Integer = MargenSuperior + 2 * FuenteTitulo.Height + 2 * FuenteCabecera.Height
            Dim rectPart As New RectangleF(MargenIzquierdo, MargenParticulares, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            e.Graphics.DrawString("Particulares", FuenteSubTitulo, Brushes.IndianRed, rectPart, Formato)
            Dim rectTotalBarcas As New RectangleF(MargenCentrado, MargenSuperior + 2 * FuenteTitulo.Height + 2 * FuenteCabecera.Height, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            e.Graphics.DrawString("Total Barcas", FuenteSubTitulo, Brushes.IndianRed, rectTotalBarcas, Formato)

            ' Mas especifico

            Dim FuenteApartados As New Font("Verdana", 10, FontStyle.Regular)

            Dim MargenAdultos As Integer = MargenParticulares + 2 * FuenteSubTitulo.Height
            Dim rectAdultos As New RectangleF(MargenIzquierdo + 50, MargenAdultos, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim rectRios As New RectangleF(MargenCentrado + 50, MargenAdultos, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Adultos: ", FuenteApartados, Brushes.DarkMagenta, rectAdultos, Formato)
            e.Graphics.DrawString("Total Barcas: ", FuenteApartados, Brushes.DarkMagenta, rectRios, Formato)
            Dim rectVA As New RectangleF(SangriaMF, MargenAdultos, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString((lblAdultos.Text & " €").PadRight(10), FuenteApartados, Brushes.DarkMagenta, rectVA, Formato)
            Dim rectVB As New RectangleF(SangriaLB, MargenAdultos, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString(lblTotalBarcas.Text, FuenteApartados, Brushes.DarkMagenta, rectVB, Formato)


            Dim MargenNiños As Integer = MargenAdultos + 2 * FuenteApartados.Height
            Dim rectNiños As New RectangleF(MargenIzquierdo + 50, MargenNiños, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Niños: ", FuenteApartados, Brushes.DarkMagenta, rectNiños, Formato)
            Dim rectBS As New RectangleF(MargenCentrado + 50, MargenNiños, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Solares: ", FuenteApartados, Brushes.DarkMagenta, rectBS, Formato)
            Dim rectVAAA As New RectangleF(SangriaMF, MargenNiños, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim rectVBBB As New RectangleF(SangriaLB, MargenNiños, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString((lblNiños.Text & " €").PadLeft(10), FuenteApartados, Brushes.DarkMagenta, rectVAAA, Formato)
            e.Graphics.DrawString(lblSolares.Text, FuenteApartados, Brushes.DarkMagenta, rectVBBB, Formato)

            Dim MargenGrupos As Integer = MargenNiños + 2 * FuenteApartados.Height
            Dim rectGrupos As New RectangleF(MargenIzquierdo, MargenGrupos, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            e.Graphics.DrawString("Grupos", FuenteSubTitulo, Brushes.IndianRed, rectGrupos, Formato)
            Dim rectBG As New RectangleF(MargenCentrado + 50, MargenGrupos, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Gold: ", FuenteApartados, Brushes.DarkMagenta, rectBG, Formato)
            Dim rectVBBBB As New RectangleF(SangriaLB, MargenGrupos, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString(lblGold.Text, FuenteApartados, Brushes.DarkMagenta, rectVBBBB, Formato)

            Dim MargenSueltos As Integer = MargenGrupos + 2 * FuenteApartados.Height
            Dim rectSueltos As New RectangleF(MargenIzquierdo + 50, MargenSueltos, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Sueltos: ", FuenteApartados, Brushes.DarkMagenta, rectSueltos, Formato)
            Dim rectTotal As New RectangleF(MargenCentrado, MargenSueltos, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            e.Graphics.DrawString("Media por Barca: ", FuenteApartados, Brushes.IndianRed, rectTotal, Formato)
            Dim rectVAAAAA As New RectangleF(SangriaMF, MargenSueltos, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim rectVBBBBB As New RectangleF(SangriaLB, MargenSueltos, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            Dim Sueltos As Integer = SumarSueltos(Fecha)
            e.Graphics.DrawString((lblSueltos.Text & "=> " & CStr(Sueltos) & " €").ToString.PadLeft(10), FuenteApartados, Brushes.DarkMagenta, rectVAAAAA, Formato)
            e.Graphics.DrawString(lblMedia.Text & " €", FuenteApartados, Brushes.IndianRed, rectVBBBBB, Formato)

            Dim MargenInserso As Integer = MargenSueltos + 2 * FuenteApartados.Height
            Dim rectInserso As New RectangleF(MargenIzquierdo + 50, MargenInserso, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Inserso: ", FuenteApartados, Brushes.DarkMagenta, rectInserso, Formato)
            Dim rectMedia As New RectangleF(MargenCentrado, MargenInserso, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            e.Graphics.DrawString("Total: ", FuenteApartados, Brushes.IndianRed, rectMedia, Formato)
            Dim rectVAAAAAA As New RectangleF(SangriaMF, MargenInserso, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim rectVBBBBBB As New RectangleF(SangriaLB, MargenInserso, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            Dim Inser As Double = Inserso(Fecha)
            e.Graphics.DrawString((lblInserso.Text & "=> " & CStr(Inser) & " €").ToString.PadLeft(10), FuenteApartados, Brushes.DarkMagenta, rectVAAAAAA, Formato)
            e.Graphics.DrawString(lblTotalEuros.Text & " €", FuenteApartados, Brushes.IndianRed, rectVBBBBBB, Formato)


            Dim MargenIsrael As Integer = MargenInserso + 2 * FuenteApartados.Height
            Dim rectIsrael As New RectangleF(MargenIzquierdo, MargenIsrael, AreaImpresion_Ancho, FuenteSubTitulo.Height)
            e.Graphics.DrawString("Israel: ", FuenteSubTitulo, Brushes.IndianRed, rectIsrael, Formato)

            Dim MargenSaul As Integer = MargenIsrael + 2 * FuenteSubTitulo.Height
            Dim rectSaul As New RectangleF(MargenIzquierdo + 50, MargenSaul, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Saul: ", FuenteApartados, Brushes.DarkMagenta, rectSaul, Formato)
            Dim rectVAAAAAAAA As New RectangleF(SangriaMF, MargenSaul, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim TSaul As Double = SumarSaul(Fecha)
            e.Graphics.DrawString((lblSaul.Text & " => " & CStr(TSaul) & " €").ToString.PadLeft(10), FuenteApartados, Brushes.DarkMagenta, rectVAAAAAAAA, Formato)

            Dim MargenAquatravel As Integer = MargenSaul + 2 * FuenteApartados.Height
            Dim rectAquatravel As New RectangleF(MargenIzquierdo + 50, MargenAquatravel, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Aquatravel: ", FuenteApartados, Brushes.DarkMagenta, rectAquatravel, Formato)
            Dim rectVAAAAAAAAA As New RectangleF(SangriaMF, MargenAquatravel, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim tAqua As Double = SumarAquatravel(Fecha)
            e.Graphics.DrawString((lblAquatravel.Text & " => " & CStr(tAqua) & " €").ToString.PadLeft(10), FuenteApartados, Brushes.DarkMagenta, rectVAAAAAAAAA, Formato)

            Dim margenOtros As Integer = MargenAquatravel + 2 * FuenteApartados.Height
            Dim rectOtros As New RectangleF(MargenIzquierdo + 50, margenOtros, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Otros: ", FuenteApartados, Brushes.DarkMagenta, rectOtros, Formato)
            Dim rectVAAAAAAAAAA As New RectangleF(SangriaMF, margenOtros, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim tisrael As Double = SumarIsrael(Fecha)
            e.Graphics.DrawString((lblOtrosIsrael.Text & " => " & CStr(tisrael) & " €").ToString.PadLeft(10), FuenteApartados, Brushes.DarkMagenta, rectVAAAAAAAAAA, Formato)

            Dim margenTotalMF As Integer = margenOtros + 2 * FuenteApartados.Height
            Dim rectTotalMF As New RectangleF(MargenIzquierdo, margenTotalMF, AreaImpresion_Ancho, FuenteApartados.Height)
            e.Graphics.DrawString("Total : ", FuenteSubTitulo, Brushes.IndianRed, rectTotalMF, Formato)
            Dim rectVAAAAAAAAAAA As New RectangleF(SangriaMF, margenTotalMF, AreaImpresion_Ancho, FuenteApartados.Height)
            Dim TMF As Double = CDbl(SumarSueltos(Fecha) + Inserso(Fecha) + SumarSaul(Fecha) + SumarAquatravel(Fecha) + SumarIsrael(Fecha) + CDbl(lblTotalParticulares.Text))
            e.Graphics.DrawString(TMF.ToString("#,###.##") & " €", FuenteApartados, Brushes.DarkMagenta, rectVAAAAAAAAAAA, Formato)

            e.Graphics.DrawLine(Pens.Red, rectTotalMF.Left, rectTotalMF.Bottom + 50, rectTotalMF.Right, rectTotalMF.Bottom + 50)

            Dim margenTOTAL As Integer = rectTotalMF.Bottom + 200
            Dim rectTOTALLL As New RectangleF((.PaperSize.Width - 2 * .Margins.Right) / 2 - ("TOTAL: " & CStr(CInt(lblTotalEuros.Text) + TMF) & " €").Length / 2, margenTOTAL, AreaImpresion_Ancho, FuenteTitulo.Height)
            e.Graphics.DrawString("TOTAL: " & (CInt(lblTotalEuros.Text) + TMF).ToString("#,###.##") & " €", FuenteTitulo, Brushes.Blue, rectTOTALLL, Formato)
            'Dim rectVAAAAAAAAAAAA As New RectangleF(SangriaMF, margenTOTAL, AreaImpresion_Ancho, FuenteTitulo.Height)
            'e.Graphics.DrawString(CStr(CInt(lblTotalEuros.Text) + TMF) & " €", FuenteTitulo, Brushes.Blue, rectVAAAAAAAAAAAA, Formato)

            e.Graphics.DrawRectangle(Pens.Blue, MargenIzquierdo, margenTOTAL, .PaperSize.Width - .Margins.Right - 100, FuenteTitulo.Height)

            'e.Graphics.MeasureString(Mid(TextoImpresion, +1), Fuente, _            'New SizeF(Areampresion_Ancho, AreaImpresion_Alto), Formato, NroLetrasLinea, _

            'NroLineasRelleno)

            'e.Graphics.DrawString(Mid(TextoImpresion, CaracterActual + 1), Fuente, _
            'Brushes.Black, Rectangulo, Formato)

            CaracterActual += NroLetrasLinea


            ' If CaracterActual < TextoImpresion.Length Then
            'If CaracterActual < Titulo.Length Then
            ' e.HasMorePages = True
            ' Else
            ' e.HasMorePages = False
            ' CaracterActual = 0
            ' End If
        End With

    End Sub


    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage


        VistaPrevia("Arial", 16, Me.txtTexto.Text, e)
    End Sub


    Private Function PaxSueltos(ByVal f As String) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(pax) from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo<>'1' and cod_grupo<>'8' and cod_grupo<>'9' and cod_grupo<>'34'"
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Function PaxInserso(ByVal f As String) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(pax) from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='1'"
        Try
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Return 0
        End Try

    End Function


    Private Function PaxSaul(ByVal f As String) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(pax) from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='8'"
        Try
            Return cmd.ExecuteScalar

        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function PaxIsrael(ByVal f As String) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(pax) from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='34'"
        Try
            Return cmd.ExecuteScalar

        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function PaxAquatravel(ByVal f As String) As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        cmd.CommandText = "select sum(pax) from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='9'"
        Try
            Return cmd.ExecuteScalar

        Catch ex As Exception
            Return 0
        End Try
    End Function



    Private Function Inserso(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim tmp As Double
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        tmp = 0
        cmd.CommandText = "select * from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='1'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Pax") * (lector("Bruto") - lector("Comision"))
        Loop
        lector.Close()
        Return Total
    End Function


    Private Function SumarSueltos(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim tmp As Double
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        tmp = 0
        cmd.CommandText = "select * from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo<>'1' and cod_grupo<>'8' and cod_grupo<>'9' and cod_grupo<>'34'"
        lector = cmd.ExecuteReader
        Try
            Do While lector.Read
                Total = Total + lector("Pax") * (lector("Bruto") - lector("Comision"))
            Loop

        Catch ex As Exception
            Total = 0
        End Try
        lector.Close()
        Return Total
    End Function


    Private Function SumarSaul(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim tmp As Double
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        tmp = 0
        cmd.CommandText = "select * from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='8'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Pax") * (lector("Bruto") - lector("Comision"))
        Loop
        lector.Close()
        Return Total
    End Function

    Private Function SumarAquatravel(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim tmp As Double
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        tmp = 0
        cmd.CommandText = "select * from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='9'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Pax") * (lector("Bruto") - lector("Comision"))
        Loop
        lector.Close()
        Return Total
    End Function

    Private Function SumarIsrael(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim tmp As Double
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        tmp = 0
        cmd.CommandText = "select * from viajes_grupo inner join grupos on viajes_grupo.cod_grupo = grupos.codigo where convert(char(10),fecha,103) = '" & f & "' and cod_grupo='34'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Pax") * (lector("Bruto") - lector("Comision"))
        Loop
        lector.Close()
        Return Total
    End Function

    Private Function SumarParticularesAdultos(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        Total = 0
        cmd.CommandText = "select * from Tickets where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio > '5'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Precio")
        Loop
        lector.Close()

        cmd.CommandText = "select * from TicketsN where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio > '5'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Precio")
        Loop
        lector.Close()

        Return Total
    End Function

    Private Function TAdultos(ByVal f As String) As Integer
        Dim cmd As SqlCommand
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        Total = 0
        cmd.CommandText = "select count(*) from Tickets where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio > '5'"
        Total = CInt(cmd.ExecuteScalar)

        cmd.CommandText = "select count(*) from TicketsN where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio > '5'"
        Total = Total + CInt(cmd.ExecuteScalar)

        Return Total
    End Function

    Private Function SumarParticularesNiños(ByVal f As String) As Double
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        Total = 0
        cmd.CommandText = "select * from Tickets where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio <= '5'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Precio")
        Loop
        lector.Close()

        cmd.CommandText = "select * from TicketsN where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio <= '5'"
        lector = cmd.ExecuteReader
        Do While lector.Read
            Total = Total + lector("Precio")
        Loop
        lector.Close()

        Return Total
    End Function

    Private Function TNiños(ByVal f As String) As Integer
        Dim cmd As SqlCommand
        Dim Total As Double

        cmd = New SqlCommand
        cmd.Connection = LosBarkitos.cnt
        Total = 0
        cmd.CommandText = "select count(*) from Tickets where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio <= '5'"
        Total = CInt(cmd.ExecuteScalar)

        cmd.CommandText = "select count(*) from TicketsN where convert(char(10),fecha,103) = '" & f & "' and part='true' and precio <= '5'"
        Total = Total + CInt(cmd.ExecuteScalar)

        Return Total
    End Function

    Private Function TotalEurosOficina(ByVal f As String) As Integer
        Dim cmd As SqlCommand
        Dim Total As Double
        Total = 0
        Try
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt

            cmd.CommandText = "select sum(Precio) from Viaje where convert(char(10),fecha,103) = '" & f & "' and cod_PV = '1'"
            Try
                Total = CInt(cmd.ExecuteScalar)
            Catch
                Total = 0
            End Try
            cmd.CommandText = "select sum(Precio) from ViajeN where convert(char(10),fecha,103) = '" & f & "' and cod_PV='1' "
            Try
                Total = Total + CInt(cmd.ExecuteScalar)
            Catch

            End Try


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
            Return Total
    End Function

    Private Function TotalEurosBarkitos(ByVal f As String) As Integer
        Dim cmd As SqlCommand
        Dim Total As Double

        Total = 0
        cmd = New SqlCommand
        Try

            cmd.Connection = LosBarkitos.cnt

            cmd.CommandText = "select sum(Precio) from Viaje where convert(char(10),fecha,103) = '" & f & "' and cod_PV = '4"
            Total = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Total = 0
        End Try
        Try
            cmd.CommandText = "select sum(Precio) from ViajeN where convert(char(10),fecha,103) = '" & f & "' and cod_PV = '4' "
            Total = Total + CInt(cmd.ExecuteScalar)

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

        Return Total
    End Function

    Private Sub TotalFotos(ByVal f As String)
        Dim cmd As SqlCommand
        Dim sum As String

        Try
            cmd = New SqlCommand
            cmd.Connection = LosBarkitos.cnt
            cmd.CommandText = " select sum(Total) from Fotos where convert(char(10), dia,103) = '" & f & "'"
            sum = CInt(cmd.ExecuteScalar).ToString("####.##")
            lblfotos.Text = "Fotos: " & sum
        Catch ex As Exception
            lblfotos.Text = ""
        End Try
    End Sub


End Class