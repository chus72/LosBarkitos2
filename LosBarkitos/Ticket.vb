Imports System
Imports System.Drawing.Image
Imports System.Drawing.Printing
Imports System.Collections

' Namespace encargado de generar el ticket

Namespace BarControls
    Public Class Ticket
        Private headerLines As New ArrayList()
        Private subHeaderLines As New ArrayList()
        Friend items As New ArrayList()
        Private totales As New ArrayList()
        Private footerLines As New ArrayList()
        Private m_headerImage As Image = Nothing

        Private count As Integer = 0

        Private m_maxChar As Integer = 35
        Private m_maxCharDescription As Integer = 20

        Private imageHeight As Integer = 0

        Friend leftMargin As Single = 0
        Private topMargin As Single = 3

        Private m_fontName As String = "Lucida Console"
        Friend m_fontSize As Integer = 9

        Friend printFont As Font = Nothing
        Friend myBrush As New SolidBrush(Color.Black)

        Friend gfx As Graphics = Nothing

        Private line As String = Nothing


        Public Sub New()
        End Sub

        Public Property HeaderImage() As Image
            Get
                Return m_headerImage
            End Get
            Set(ByVal value As Image)
                m_headerImage = value

            End Set
        End Property

        Public Property MaxChar() As Integer
            Get
                Return m_maxChar
            End Get
            Set(ByVal value As Integer)
                If value <> m_maxChar Then
                    m_maxChar = value
                End If
            End Set
        End Property

        Public Property MaxCharDescription() As Integer
            Get
                Return m_maxCharDescription
            End Get
            Set(ByVal value As Integer)
                If value <> m_maxCharDescription Then
                    m_maxCharDescription = value
                End If
            End Set
        End Property

        Public Property FontSize() As Integer
            Get
                Return m_fontSize
            End Get
            Set(ByVal value As Integer)
                If value <> m_fontSize Then
                    m_fontSize = value
                End If
            End Set
        End Property

        Public Property FontName() As String
            Get
                Return m_fontName
            End Get
            Set(ByVal value As String)
                If value <> m_fontName Then
                    m_fontName = value
                End If
            End Set
        End Property

        Public Sub AddHeaderLine(ByVal line As String)
            headerLines.Add(line)
        End Sub

        Public Sub AddSubHeaderLine(ByVal line As String)
            subHeaderLines.Add(line)
        End Sub

        Public Sub AddItem(ByVal cantidad As String, ByVal item As String, ByVal price As String)
            Dim newItem As New OrderItem("?"c)
            items.Add(newItem.GenerateItem(cantidad, item, price))
        End Sub

        Public Sub AddTotal(ByVal name As String, ByVal price As String)
            Dim newTotal As New OrderTotal("?"c)
            totales.Add(newTotal.GenerateTotal(name, price))
        End Sub

        Public Sub AddFooterLine(ByVal line As String)
            footerLines.Add(line)
        End Sub

        Private Function AlignRightText(ByVal lenght As Integer) As String
            Dim espacios As String = ""
            Dim spaces As Integer = m_maxChar - lenght
            For x As Integer = 0 To spaces - 1
                espacios += " "
            Next
            Return espacios
        End Function

        Private Function DottedLine() As String
            Dim dotted As String = ""
            For x As Integer = 0 To m_maxChar - 1
                dotted += "="
            Next
            Return dotted
        End Function

        Public Function PrinterExists(ByVal impresora As String) As Boolean
            For Each strPrinter As String In PrinterSettings.InstalledPrinters
                If impresora = strPrinter Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub PrintTicket(ByVal impresora As String)
            printFont = New Font(m_fontName, m_fontSize, FontStyle.Regular)
            Dim pr As New PrintDocument()
            pr.PrinterSettings.PrinterName = impresora
            AddHandler pr.PrintPage, AddressOf pr_PrintPage
            pr.Print()
        End Sub

        Private Sub pr_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            gfx = e.Graphics
            DrawImage()
            DrawHeader()
            DrawSubHeader()
            DrawItems()
            DrawTotales()
            DrawFooter()
            'Insertado por mi para poner tambien el logotipo abajo
            '  DrawImage()

            If m_headerImage IsNot Nothing Then
                HeaderImage.Dispose()
                m_headerImage.Dispose()
            End If
        End Sub

        Friend Function YPosition() As Single
            Return topMargin + (count * printFont.GetHeight(gfx) + imageHeight)
        End Function

        Private Sub DrawImage()
            If m_headerImage IsNot Nothing Then
                Try
                    gfx.DrawImage(m_headerImage, New Point(CInt(leftMargin), CInt(YPosition())))
                    Dim height As Double = (CDbl(m_headerImage.Height) / 58) * 15
                    imageHeight = CInt(Math.Round(height)) + 3
                Catch generatedExceptionName As Exception
                End Try
            End If
        End Sub

        Private Sub DrawHeader()
            For Each header As String In headerLines
                If header.Length > m_maxChar Then
                    Dim currentChar As Integer = 0
                    Dim headerLenght As Integer = header.Length

                    While headerLenght > m_maxChar
                        line = header.Substring(currentChar, m_maxChar)
                        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                        count += 1
                        currentChar += m_maxChar
                        headerLenght -= m_maxChar
                    End While
                    line = header
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), New StringFormat())
                    count += 1
                Else
                    line = header
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                    count += 1
                End If
            Next
            DrawEspacio()
        End Sub

        Private Sub DrawSubHeader()
            For Each subHeader As String In subHeaderLines
                If subHeader.Length > m_maxChar Then
                    Dim currentChar As Integer = 0
                    Dim subHeaderLenght As Integer = subHeader.Length

                    While subHeaderLenght > m_maxChar
                        line = subHeader
                        gfx.DrawString(line.Substring(currentChar, m_maxChar), printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                        count += 1
                        currentChar += m_maxChar
                        subHeaderLenght -= m_maxChar
                    End While
                    line = subHeader
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), New StringFormat())
                    count += 1
                Else
                    line = subHeader

                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                    count += 1

                    line = DottedLine()

                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                    count += 1
                End If
            Next
            DrawEspacio()
        End Sub

        Friend Sub DrawItems()
            Dim ordIt As New OrderItem("?"c)

            gfx.DrawString("DESCRIPCION IMPORTE", printFont, myBrush, leftMargin, YPosition(), New StringFormat())

            count += 1
            DrawEspacio()

            For Each item As String In items
                line = ordIt.GetItemCantidad(item)

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                line = ordIt.GetItemPrice(item)
                line = AlignRightText(line.Length) + line

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                Dim name As String = ordIt.GetItemName(item)

                leftMargin = 0
                If name.Length > m_maxCharDescription Then
                    Dim currentChar As Integer = 0
                    Dim itemLenght As Integer = name.Length

                    While itemLenght > m_maxCharDescription
                        line = ordIt.GetItemName(item)
                        gfx.DrawString(" " + line.Substring(currentChar, m_maxCharDescription), printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                        count += 1
                        currentChar += m_maxCharDescription
                        itemLenght -= m_maxCharDescription
                    End While

                    line = ordIt.GetItemName(item)
                    gfx.DrawString(" " + line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), New StringFormat())
                    count += 1
                Else
                    gfx.DrawString(" " + ordIt.GetItemName(item), printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                    count += 1
                End If
            Next

            leftMargin = 0
            DrawEspacio()
            line = DottedLine()

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

            count += 1
            DrawEspacio()
        End Sub

        Private Sub DrawTotales()
            Dim ordTot As New OrderTotal("?"c)

            For Each total As String In totales
                line = ordTot.GetTotalCantidad(total)
                line = AlignRightText(line.Length) + line

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())
                leftMargin = 0

                line = " " + ordTot.GetTotalName(total)
                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())
                count += 1
            Next
            leftMargin = 0
            DrawEspacio()
            DrawEspacio()
        End Sub

        Private Sub DrawFooter()
            For Each footer As String In footerLines
                If footer.Length > m_maxChar Then
                    Dim currentChar As Integer = 0
                    Dim footerLenght As Integer = footer.Length

                    While footerLenght > m_maxChar
                        line = footer
                        gfx.DrawString(line.Substring(currentChar, m_maxChar), printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                        count += 1
                        currentChar += m_maxChar
                        footerLenght -= m_maxChar
                    End While
                    line = footer
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), New StringFormat())
                    count += 1
                Else
                    line = footer
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

                    count += 1
                End If
            Next
            leftMargin = 0
            DrawEspacio()
        End Sub

        Private Sub DrawEspacio()
            line = ""

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), New StringFormat())

            count += 1
        End Sub
    End Class

    Public Class OrderItem
        Private delimitador As Char() = New Char() {"?"c}

        Public Sub New(ByVal delimit As Char)
            delimitador = New Char() {delimit}
        End Sub

        Public Function GetItemCantidad(ByVal orderItem As String) As String
            Dim delimitado As String() = orderItem.Split(delimitador)
            Return delimitado(0)
        End Function

        Public Function GetItemName(ByVal orderItem As String) As String
            Dim delimitado As String() = orderItem.Split(delimitador)
            Return delimitado(1)
        End Function

        Public Function GetItemPrice(ByVal orderItem As String) As String
            Dim delimitado As String() = orderItem.Split(delimitador)
            Return delimitado(2)
        End Function

        Public Function GenerateItem(ByVal cantidad As String, ByVal itemName As String, ByVal price As String) As String
            Return cantidad + delimitador(0) + itemName + delimitador(0) + price
        End Function
    End Class

    Public Class OrderTotal
        Private delimitador As Char() = New Char() {"?"c}

        Public Sub New(ByVal delimit As Char)
            delimitador = New Char() {delimit}
        End Sub

        Public Function GetTotalName(ByVal totalItem As String) As String
            Dim delimitado As String() = totalItem.Split(delimitador)
            Return delimitado(0)
        End Function

        Public Function GetTotalCantidad(ByVal totalItem As String) As String
            Dim delimitado As String() = totalItem.Split(delimitador)
            Return delimitado(1)
        End Function

        Public Function GenerateTotal(ByVal totalName As String, ByVal price As String) As String
            Return totalName + delimitador(0) + price
        End Function
    End Class

    Public Class TicketReserva
        Inherits Ticket

        Public Overloads Property FontSize() As Integer
            Get
                Return m_fontSize
            End Get
            Set(ByVal value As Integer)
                If value <> m_fontSize Then
                    m_fontSize = value
                End If
            End Set
        End Property

        Private Sub pr_PrintPage2(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            gfx = e.Graphics


            DrawItems()
            ' DrawTotales()
            'Insertado por mi para poner tambien el logotipo abajo
            '  DrawImage()

            ' If m_headerImage IsNot Nothing Then
            ' HeaderImage.Dispose()
            ' m_headerImage.Dispose()
            ' End If
        End Sub

        Public Overloads Sub PrintTicket(ByVal impresora As String)
            Dim pr As New PrintDocument()
            pr.PrinterSettings.PrinterName = impresora
            AddHandler pr.PrintPage, AddressOf pr_PrintPage2
            pr.Print()
        End Sub

        Private Overloads Sub DrawItems()
            For Each item As String In items
                gfx.DrawString(item, printFont, myBrush, leftMargin, YPosition(), New StringFormat())
            Next
        End Sub


    End Class

End Namespace
