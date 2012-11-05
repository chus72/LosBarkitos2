Option Strict Off
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Printing
Imports Chus.ChusArrayControles
'Imports Chus.LosBarkitos
Imports System.Data.SqlClient
'Imports Chus.DataGridPrinter
Imports Chus.BarControls
Imports System.IO
Imports System.IO.Ports.SerialPort
'Imports MySQLDriverCS


Public Class LosBarkitos
    Const GENERIC_WRITE As Int32 = &H40000000
    Const OPEN_EXISTING As Int32 = 3
    Public Const cBase As Integer = 1
    Public Const cBarkitos As Integer = 2
    Public Const cOficina As Integer = 3
    Public Const cTPV2 As Integer = 4

    Private m_Barcas As ArrayControles(Of Label)
    Private m_Numeros As ArrayControles(Of Label)

    Private Tiempo(20) As DateTime
    Public BarcasTiempo(20) As Boolean
    Public cInformacion As New ArrayList

    Public cnt As SqlConnection
    Public cnt2 As SqlConnection
    ' Dim DBMySQL As MySQLConnection


    'Numero de barca
    Const NRios As Integer = 13
    Const NSolares As Integer = 6
    Dim cLibreFerry As Drawing.Color = Color.Black
    Dim cFerry As Drawing.Color = Color.Red
    Dim cLibreRio As Drawing.Color = Color.Aquamarine
    Dim cLibreSolar As Drawing.Color = Color.Yellow
    Dim cLibreGold As Drawing.Color = Color.MediumAquamarine
    Dim cAlquilada As Drawing.Color = Color.Green
    Dim cTiempo As Drawing.Color = Color.Red
    Dim cBlanco As Drawing.Color = Color.White
    Dim cNegro As Drawing.Color = Color.Black
    Public TipoBarca As Integer 'Tipo de ticket: 1:bkt, 2:solar, 3:gold, 4:ferry
    Dim DG As DataGridView
    Dim ImagenBarkitos, ImagenBarkitosMarcado, ImagenSolar, ImagenSolarMarcado, ImagenGold, ImagenGoldMarcado As Bitmap
    Dim ImagenFerry, ImagenFerryMarcado As Bitmap
    Private Donde As String
    Public NombreGuia, NombreGrupo As String 'Variables que almacenan el nombre del grupo y guia para incluir en BDD
    Public PaxGrupo As Integer ' Variable que almacena los pax de un grupo


    Private Sub LosBarkitos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If QuePuntoVenta() = cOficina Then
            Dim respuesta As Integer = MessageBox.Show("¿Seguro de cerrar el dia?", "Atención!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
            If respuesta = vbYes Then
                Try
                    InicializarTablasBDD()

                Catch ex As Exception
                    MsgBox("No se  pueden inicializar las tablas", MsgBoxStyle.Exclamation, "FIN")
                End Try
            End If
        End If
        Try
            TraspasarDatosSinConexion()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        'SE IMPRIME EL TOTAL DEL DIA EN PAPEL
        If QuePuntoVenta() = cTPV2 Then
            ListadoPapelEmail(QuePuntoVenta)
        End If

        Seguridad.CopiaLBaLBJBak()
        cnt.Close()
        cnt2.Close()
        ' DBMySQL.Close()


    End Sub

    Private Sub ListadoPapelEmail(ByVal PV As Integer)
        Dim Fic As String = "c:\Total" & Format(Today, "dd/MM/yyyy") & ".txt"
        Fic = Fic.Replace("/", "-")
        Dim cmd As New SqlCommand("SELECT count(numero) as cont1,sum(precio) as pre1 FROM Viaje WHERE convert(char(10),Fecha,103) = convert(char(10), getdate() ,103) ", cnt)
        Dim Lector, Lector2 As SqlDataReader
        Dim Via, Tot As Integer
        Dim Med As Double

        Lector = cmd.ExecuteReader
        Lector.Read()
        Via = CInt(Lector("cont1"))
        Tot = CInt(Lector("pre1"))
        Med = Tot / Via

        Lector.Close()
        cmd.CommandText = "SELECT count(numero) as cont2,sum(precio) as pre2 FROM ViajeN WHERE convert(char(10),Fecha,103) = convert(char(10), getdate() ,103) "
        Lector2 = cmd.ExecuteReader
        Lector2.Read()

        Try
            Via += CInt(Lector2("cont2"))
            Tot += CInt(Lector2("pre2"))
        Catch
        End Try

        Lector2.Close()

        Try
            Using sw As StreamWriter = File.CreateText(Fic)
                sw.WriteLine("Viajes: " & Via.ToString)
                sw.WriteLine("Total: " & Tot.ToString & " €")
                sw.WriteLine("Media: " & Med.ToString & " €")
                sw.WriteLine() : sw.WriteLine()
                sw.Close()

            End Using
        Catch ex As Exception

        End Try
        Dim procID As Integer = Shell("c:\sendemail.exe -f chus@chusito.es -t chusito1972@gmail.com -u BarKiTos -m " & Format(Now, "dd/MM/yyyy") & "  -a  " & Fic & " -s smtp.chusito.es -xu xbj868c -xp Otisuhc0 -s smtp.chusito.es", , True)
        MsgBox("Proceso con id = ", CType(procID.ToString, MsgBoxStyle))
        procID = Shell("PRINT " & Fic, , True)
    End Sub

    Private Sub LosBarkitos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If QuePuntoVenta() = cOficina Then
            'Entrada.Show()
        End If

        'Color del control del panel en losbarkitos
        lblCtrlBkt.BackColor = Color.Magenta
        StatusStrip1.BackColor = Color.Magenta
        ToolStripStatusLabel1.Text = "Carga de imagenes..."
        'cargo las imagen del los pictures de los botones
        CargaImagenesBotones()


        'Color de fondo : magenta
        Me.BackColor = Color.Magenta
        chkNegro.BackColor = Color.Magenta
        ' Pongo los labels de las Barcas en el array m_Barcas
        m_Barcas = New ArrayControles(Of Label)("lblBarcas", Me)
        'Pongo los labels de los numeros en el array
        m_Numeros = New ArrayControles(Of Label)("lblNumero", Me)


        'Me conecto a la BDD LosBarkitos
        'Conexión a la BDD LosBarkitos
        ConexionBDD()


        'Pone las dimensiones correctas de los labels y color=color.Beige
        DimensionarControles()

        'Posiciona los labels en la pantalla
        PosicionarControles()

        'Restriccines para segun qué punto de venta
        Restricciones()

        'Carga los mensajes de informacion
        CargaMensajes()

        If cnt.State = ConnectionState.Open Then
            ActualizarBarcas()
            InicializaTiempo()
        End If

        'Generar eventos en lbls
        GenerarEventos()
        'Inicializo el timer
        Timer1.Interval = 1000
        Timer1.Enabled = True

        'Los precios estan no enabled
        grpPrecio.Enabled = False
        'Aun no se puede sacar un ticket
        lblTicket.Enabled = False
        lblTicketReserva.Enabled = False

        'Color de los pic para los tickets
        picRio.BackColor = cLibreRio
        picSolar.BackColor = cLibreSolar
        picGold.BackColor = cLibreGold

        Donde = CStr("EnPrecio")

        chkNegro.Checked = False

        ToolStripStatusLabel1.Text = CStr(cInformacion(0)) & " o " & CStr(cInformacion(1))
        btnCerrarGrupo.Visible = False

        ColocarPorcentaje()

    End Sub

    Private Sub Restricciones()
        '----------------------------------------------------------------------------------------------------
        'Restricciones para el punto de venta del puerto
        If QuePuntoVenta() = cBarkitos Or QuePuntoVenta() = cTPV2 Then
            'btnReservasCero.Visible = False
            'ListadosToolStripMenuItem.Enabled = False
            BarcasPorLlegarToolStripMenuItem.Visible = False
            DiarioToolStripMenuItem.Visible = False
            AcotadoToolStripMenuItem.Visible = False
            MensualToolStripMenuItem.Visible = False
            mnuGestionGruposGuias.Visible = False
            ResumenDíaToolStripMenuItem.Visible = False
            EstadisticasToolStripMenuItem.Enabled = False
            ExtraToolStripMenuItem.Visible = False
            ExtraToolStripMenuItem1.Visible = False
            GruposToolStripMenuItem1.Visible = False
            GruposToolStripMenuItem2.Visible = False
            GruposToolStripMenuItem3.Visible = False
            CopiaToolStripMenuItem.Visible = False
            CajaBktsToolStripMenuItem.Visible = False
            ListadosTotalesToolStripMenuItem.Visible = False
            FotosToolStripMenuItem.Visible = False
            grpCont.Visible = False
            lblCtrlBkt.Visible = True
            txtPorcentaje.Visible = False
            btnPorcentaje.Visible = False
        End If
        '----------------------------------------------------------------------------------------------------------------------------------
        'Restricciones para la celia
        If QuePuntoVenta() = cBase Then
            lblGastos.Visible = False
            ResumenDíaToolStripMenuItem.Visible = False
            ExtraToolStripMenuItem.Visible = False
            ExtraToolStripMenuItem1.Visible = False
            GruposToolStripMenuItem2.Visible = False
            GruposToolStripMenuItem3.Visible = False
            chkNegro.Visible = False
            chkNegro.Checked = False 'Siempre sera en blanco
            FotosToolStripMenuItem.Visible = False
            'CopiaToolStripMenuItem.Visible = False
            CajaBktsToolStripMenuItem.Visible = False
            ListadosTotalesToolStripMenuItem.Visible = False
            EstadisticasToolStripMenuItem.Visible = False
            ContabilizarGrupoConFacturaToolStripMenuItem.Visible = False
            ResumenDíaToolStripMenuItem.Visible = False
            grpCont.Visible = False
            VerTodosToolStripMenuItem.Visible = False
            txtPorcentaje.Visible = False
            btnPorcentaje.Visible = False
            lblCtrlBkt.Visible = False


        End If
    End Sub

    Private Sub CargaImagenesBotones()
        'If QuePuntoVenta() <> cBarkitos Then 'TPV 
        ' ImagenBarkitos = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\Barkitos.jpg", True)
        ' ImagenBarkitosMarcado = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\BarkitosMarcado.jpg", True)
        ' ImagenSolar = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\Solar.jpg", True)
        ' ImagenSolarMarcado = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\SolarMarcado.jpg", True)
        ' ImagenGold = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\gold.jpg", True)
        ' ImagenGoldMarcado = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\goldmarcado.jpg", True)
        ' ImagenFerry = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\ferry.jpg", True)
        ' ImagenFerryMarcado = New Bitmap("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\ferrymarcado.jpg", True)
        ' Else
        ' ImagenBarkitos = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\Barkitos.jpg", True)
        ' ImagenBarkitosMarcado = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\BarkitosMarcado.jpg", True)
        ' ImagenSolar = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\Solar.jpg", True)
        ' ImagenSolarMarcado = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\SolarMarcado.jpg", True)
        ' ImagenGold = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\gold.jpg", True)
        ' ImagenGoldMarcado = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\goldmarcado.jpg", True)
        ' ImagenFerry = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\ferry.jpg", True)
        'ImagenFerryMarcado = New Bitmap("C:\Documents and Settings\usuari\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\ferrymarcado.jpg", True)
        ' End If

        ImagenBarkitos = New Bitmap("\F_LosBarkitos\Barkitos.jpg", True)
        ImagenBarkitosMarcado = New Bitmap("\F_LosBarkitos\BarkitosMarcado.jpg", True)
        ImagenSolar = New Bitmap("\F_LosBarkitos\Solar.jpg", True)
        ImagenSolarMarcado = New Bitmap("\F_LosBarkitos\SolarMarcado.jpg", True)
        ImagenGold = New Bitmap("\F_LosBarkitos\gold.jpg", True)
        ImagenGoldMarcado = New Bitmap("\F_LosBarkitos\goldmarcado.jpg", True)
        ImagenFerry = New Bitmap("\F_LosBarkitos\ferry.jpg", True)
        ImagenFerryMarcado = New Bitmap("\F_LosBarkitos\ferrymarcado.jpg", True)

    End Sub

    Private Sub ConexionBDD()
        cnt = New SqlConnection
        'cnt.ConnectionString = "Data Source=.\SQLEXPRESS;AttachDbFilename='C:\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2005\Projects\LosBarkitos2\LosBarkitos\Losbarkitos.mdf';Integrated Security=True;Connect Timeout=30; user instance = true"
        'cnt.ConnectionString = "Data Source=.\SQLEXPRESS;AttachDbFilename='\\Servidor\MSSQL\Data\Losbarkitos.mdf';Integrated Security=True;Connect Timeout=30; user instance = true"
        'cnt.ConnectionString = "Data Source=.\SQLEXPRESS;AttachDbFilename='|datadirectory|\Losbarkitos.mdf';Integrated Security=True;Connect Timeout=30; user instance = true"
        'cnt.ConnectionString = "Data Source=SERVIDOR\SQLEXPRESS;initial catalog = losbarkitos ;User ID=sa; password=chusito"
        'ESTA ES LA QUE SIRVE PARA EL ORDENADOR DE CHUSITO
        If QuePuntoVenta() <> cBase Then
            cnt.ConnectionString = "Data Source=CHUSITO\SQLEXPRESS;initial catalog = losbarkitos; Integrated Security=True"
        Else
            cnt.ConnectionString = "Data Source=CHUSITO\SQLEXPRESS;initial catalog = losbarkitos;User ID='SERVIDOR'; Password='Jesus'"
        End If
        'cnt.ConnectionString = "Data Source=SERVIDOR\SQLEXPRESS;initial catalog = losbarkitos; Integrated Security=True"
        'cnt.ConnectionString = "Data Source=192.168.1.16;initial catalog = losbarkitos; Integrated Security=True"

        'Me conecto a la BDD LosbarkitosJ
        ' cntJ = New SqlConnection
        ' cntJ.ConnectionString = "Data source=CHUSITO\SQLEXPRESS; initial catalog= losbarkitosJ; Integrated Security=True"

        'abro la bdd
        Try
            cnt.Open()
        Catch ex As Exception
            MsgBox("No se puede conectar a la BDD principal. Trabajamos sin conexión. Error " & ex.Message, MsgBoxStyle.Exclamation, "SIN CONEXIÓN")
        End Try
        'abro la bdd J
        ' Try
        ' cntJ.Open()
        ' Catch ex As Exception
        ' MsgBox("Problema conexión J")
        ' End Try

        ' Me conecto a la BDD local

        cnt2 = New SqlConnection

        If QuePuntoVenta() = cBase Then 'Celia
            cnt2.ConnectionString = "Data Source=SERVIDOR\SQLEXPRESS; initial catalog = losbarkitoslocal; Integrated Security=true"
        ElseIf QuePuntoVenta() = cBarkitos Then 'TPV
            cnt2.ConnectionString = "Data Source=TPVBARKITOS\SQLEXPRESS; initial catalog = losbarkitoslocal; Integrated Security=true"
        ElseIf QuePuntoVenta() = cOficina Then ' Mio
            cnt2.ConnectionString = "Data Source=CHUSITO\SQLEXPRESS;initial catalog = losbarkitoslocal; Integrated Security=True"
        ElseIf QuePuntoVenta() = cTPV2 Then ' TPV2
            cnt2.ConnectionString = "Data Source=MICRUS\SQLEXPRESS; initial catalog = losbarkitoslocal; Integrated Security=True"
        End If
        Try
            cnt2.Open()
        Catch ex As Exception
            MsgBox("No se puede conectar a la BDD secundariart." & ex.Message)
        End Try

        '   INTENTO CONEXION A LA BASE DE DATOS MYSQL    '
        ' Try
        'DBMySQL = New MySQLConnection(New MySQLConnectionString("192.168.1.16", "LosBarkitosMySQL", "root", "chusito", 3306).AsString)
        'DBMySQL.Open()
        ' Catch ex As Exception
        'MsgBox("No se puede conectar a MySQL. " & ex.Message)
        ' End Try
    End Sub

    Private Sub TraspasarDatosSinConexion()
        Dim cmdLocal As SqlCommand
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim RegLocal As DataRow


        cmdLocal = New SqlCommand
        Try
            cmdLocal.Connection = cnt2

        Catch ex As Exception

        End Try

        'Pone los registros en ds
        cmdLocal.CommandText = "select * from Viaje"
        ds = New DataSet
        da = New SqlDataAdapter
        da.SelectCommand = cmdLocal
        da.Fill(ds, "Registros")

        'Hago el repaso por los registros de local y los pasamos al servidor
        Try
            For Each RegLocal In ds.Tables("Registros").Rows
                Traspaso(RegLocal)
            Next
            'borro la tabla local Viaje
            cmdLocal.CommandText = "delete from Viaje"
            cmdLocal.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("No se puede hacer la copia al servidor de la BDD's", MsgBoxStyle.Exclamation, "NO." & ex.Message & ex.InnerException.ToString)
        End Try
    End Sub

    Private Sub Traspaso(ByVal rl As DataRow)
        Dim cmd As SqlCommand
        Dim pTicket, pPrecio, pA, pN, pPV, pTP, pFec As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "insert into ViajeN (Numero,Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@tic,@pre, @fec ,@adu,@nin,@cpv,@ctb) "
        pTicket = New SqlParameter("@tic", CInt(Rnd() * 2000 + 1))
        pTicket.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pTicket)

        pPrecio = New SqlParameter("@pre", rl("Precio"))
        pPrecio.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPrecio)
        pA = New SqlParameter("@adu", rl("Adultos"))
        pA.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pA)
        pN = New SqlParameter("@nin", rl("Niños"))
        pN.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pN)
        pPV = New SqlParameter("@cpv", rl("cod_PV"))
        pPV.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPV)
        pTP = New SqlParameter("@ctb", rl("cod_Barca"))
        pTP.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pTP)
        pFec = New SqlParameter("@fec", rl("Fecha"))
        cmd.Parameters.Add(pFec)

        ' Try
        cmd.ExecuteNonQuery()
        Beep()
        'Catch

        'End Try
        'Agragamos el viaje en la BDD MySQL
        ' Try
        'AgregarViajeBDDMySQL(CDbl(rl("Precio")), CInt(rl("Adultos")), CInt(rl("Niños")), CInt(rl("cod_PV")), CInt(rl("cod_Barca")), CInt(Rnd() * 2000 + 1))
        ' Catch ex As Exception
        '   'MsgBox("No se traspasan los datos a MYSQL.", MsgBoxStyle.Information, ex.Message)
        ''  End Try
    End Sub

    Private Sub CargaMensajes()
        ' Aquí irán los mensajes de informacion

        cInformacion.Add("Pulsa botón para tipo de barca")
        cInformacion.Add("Pulsa botón salida o llegada de una barca")
        cInformacion.Add("Introduce el importe del ticket y pulsa <ticket> o <reserva>")
        cInformacion.Add("Aquí tienes que introducir el importe del ticket")
        cInformacion.Add("Aquí introduces el número de pasajeros del grupo")

    End Sub

    ''' <summary>
    ''' Inicializa la matriz donde se guarda el tiempo de cada barca
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializaTiempo()
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim i As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Estado, Hora from Barcas"
        lector = cmd.ExecuteReader
        i = 1
        While lector.Read
            If CStr(lector("Estado")) = "A" Then
                Tiempo(i) = CDate(lector("Hora"))
                i = i + 1
            End If
        End While
        lector.Close()

    End Sub

    ''' <summary>
    ''' Dimension de los labels y colocacion del texto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DimensionarControles()
        Dim i As Integer
        '''''''''''''''''''
        ' Las Rios ''''''''
        '''''''''''''''''''
        For i = 0 To NRios - 1
            If (i + 1) = 13 Then
                m_Barcas(i).Text = "Rio 12 + 1"
            Else
                m_Barcas(i).Text = "Rio " & CStr(i + 1)
            End If
            m_Barcas(i).Size = New Size(Me.Width \ 7, 100)
            m_Barcas(i).AutoSize = False
            m_Barcas(i).BorderStyle = BorderStyle.FixedSingle
            m_Barcas(i).BackColor = cLibreRio
        Next
        ''''''''''''''''''''
        'Las solares '''''''
        ''''''''''''''''''''
        For i = NRios To NSolares + NRios - 1
            m_Barcas(i).Text = "Solar " & CStr(i + 1 - NRios)
            m_Barcas(i).Size = New Size(Me.Width \ 7, 100)
            m_Barcas(i).AutoSize = False
            m_Barcas(i).BorderStyle = BorderStyle.FixedSingle
            m_Barcas(i).BackColor = cLibreSolar
        Next
        ''''''''''''''''
        'La Gold '''''''
        ''''''''''''''''
        m_Barcas(19).Text = "Gold"
        m_Barcas(19).Size = New Size(2 * m_Barcas(1).Width, 100)
        m_Barcas(19).AutoSize = False
        m_Barcas(19).BorderStyle = BorderStyle.FixedSingle
        m_Barcas(19).BackColor = cLibreGold

    End Sub

    ''' <summary>
    ''' Pone los labels en la posicion correcta
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PosicionarControles()
        Dim h, a As Integer

        ' Altura y anchura de los labels
        h = m_Barcas(1).Height
        a = m_Barcas(1).Width

        '''''''''''''''''''''''''''''''''''''''''''''''
        ' posicionamiento de los labels para las rios '
        '''''''''''''''''''''''''''''''''''''''''''''''
        For i As Integer = 0 To NRios - 1
            If i < 7 Then
                m_Barcas(i).Top = 0
                m_Barcas(i).Left = i * a
            Else
                m_Barcas(i).Top = h
                m_Barcas(i).Left = (i - 7) * a
            End If

        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''
        'posicionamiento de los labels para las solares '
        '''''''''''''''''''''''''''''''''''''''''''''''''
        For i As Integer = 0 To NSolares - 1
            If i < 2 Then
                m_Barcas(i + NRios).Top = 2 * h
                m_Barcas(i + NRios).Left = i * a
            ElseIf i = 2 Or i = 3 Then
                m_Barcas(i + NRios).Top = 3 * h
                m_Barcas(i + NRios).Left = (i - 2) * a
            ElseIf i = 4 Or i = 5 Then
                m_Barcas(i + NRios).Top = 4 * h
                m_Barcas(i + NRios).Left = (i - 4) * a
            End If
        Next

        '''''''''''''''''''''''''''''''''''''''
        'Posicionamiento del label de la gold '
        '''''''''''''''''''''''''''''''''''''''
        m_Barcas(19).Left = 0
        m_Barcas(19).Top = 5 * h

        'Posicionamiento para lblInformacion

        'Posiciono el boton Cerrar
        btnReservasCero.Width = m_Barcas(19).Width \ 3
        btnReservasCero.Left = 0
        btnReservasCero.Top = m_Barcas(19).Top + m_Barcas(19).Height

        'Posiciono el label descuento
        lblDescuento.Left = btnReservasCero.Left + btnReservasCero.Width
        lblDescuento.Top = btnReservasCero.Top
        'Posiciono el label Gastos para TPV
        lblGastos.Left = lblDescuento.Left + lblDescuento.Width
        lblGastos.Top = lblDescuento.Top

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Posiciono las imagenes para los botones de los tickets '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        picRio.Left = 2 * m_Barcas(0).Width + 50
        picSolar.Left = picRio.Left + picRio.Width
        picGold.Left = picSolar.Left + picSolar.Width
        picFerry.Left = picGold.Left

        ''''''''''''''''''''''''''''''''''
        'Posiciono los labels contadores '
        ''''''''''''''''''''''''''''''''''
        grpCont.Left = 6 * m_Barcas(0).Width + 15
        grpCont.Width = CInt((Me.Width - grpCont.Left) * 0.9)


        ''''''''''''''''''''''''''''''''''''
        'Posición de los labels de tickets '
        ''''''''''''''''''''''''''''''''''''
        'Posiciono el label ticket
        lblTicket.Left = picFerry.Left + CInt(picFerry.Width / 2) - CInt(lblTicket.Width / 2)
        'Posiciono el label ticket-reserva
        lblTicketReserva.Top = lblTicket.Top + lblTicket.Height
        lblTicketReserva.Left = lblTicket.Left
        'Posiciono el boton btnCerrarGrupo
        btnCerrarGrupo.Left = lblTicket.Left
        'Posiciono lblConexion
        lblConexion.Left = lblTicket.Left
        lblConexion.Top = btnCerrarGrupo.Top + btnCerrarGrupo.Height
        lblConexion.Height = btnCerrarGrupo.Height

        'Posiciono el grupo de las teclas
        grpPrecio.Height = CInt((Me.Height - grpPrecio.Top) * 0.8)
        grpPrecio.Left = picRio.Left
        'Posiciono el checkbox chkNegro
        chkNegro.Left = CInt(Me.Width * 0.95)
        chkNegro.Top = CInt(Me.Height * 0.85)
        txtPorcentaje.Left = CInt(chkNegro.Left + chkNegro.Width)
        txtPorcentaje.Top = CInt(chkNegro.Top)
        btnPorcentaje.Top = txtPorcentaje.Top + txtPorcentaje.Height
        btnPorcentaje.Left = txtPorcentaje.Left

        'Posiciono lblCtrlBkt
        lblCtrlBkt.Left = lblTicketReserva.Left
        lblCtrlBkt.Top = lblTicketReserva.Top + lblTicketReserva.Height

    End Sub

    ''' <summary>
    ''' Agrupa los eventos de los labels de las barcas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerarEventos()
        'Eventos para los labels de las barcas
        For Each lbl As Label In m_Barcas
            AddHandler lbl.Click, AddressOf lblBarcas_Click
        Next


        'Eventos para los labels de los numeros
        For Each lbl As Label In m_Numeros
            AddHandler lbl.Click, AddressOf lblNumero_Click
        Next
    End Sub

    Private Sub lblBarcas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lbl As Label
        Dim loc As Integer

        'Recupero el label que ha sido clickado
        lbl = TryCast(sender, Label)
        '' loc2 = CInt(Mid(lbl.Name, 10, 3))
        'Se recupera la posicion del label clickado en el array de labels
        For loc = 0 To m_Barcas.Count - 1
            If lbl.Name = m_Barcas(loc).Name Then
                'Actualiza la tabla controlBarcas
                ActualizarBarcas(loc + 1)
                Exit For
            End If
        Next


    End Sub

    Private Sub lblNumero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lbl As Label

        'recupero el label que ha sido clickado
        lbl = TryCast(sender, Label)
        'Miro donde coloco el numero. Si el ckeck es true lo pongo en label de grupo
        If chkGrupo.Checked And Donde = "EnTicketsGrupo" Then
            lblTicketsGrupo.Text = lblTicketsGrupo.Text & lbl.Text
        Else
            'pongo en precio el numero que ha salido
            If lbl.Name = m_Numeros(0).Name And lblPrecio.Text = "" Then Exit Sub
            lblPrecio.Text = lblPrecio.Text & lbl.Text
        End If

    End Sub

    ''' <summary>
    ''' Actualiza todas las barcas segun BDD
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizarBarcas()
        Dim lbl As Label
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim ind As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Codigo,Nombre, Estado, NV, Tiempo from Barcas"
        lector = cmd.ExecuteReader

        ind = 1
        For Each lbl In m_Barcas
            Dim b As Boolean = lector.Read
            If CStr(lector("Estado")) = "L" Then
                Select Case CInt(lector("Codigo"))
                    Case 1 To 13
                        m_Barcas(ind - 1).BackColor = cLibreRio
                        m_Barcas(ind - 1).ForeColor = cNegro
                        m_Barcas(ind - 1).Text = CStr(lector("Nombre")) & Chr(13) & "--:--:--" & Chr(13) & CStr(lector("NV")) & " viajes"
                        If CStr(lector("Tiempo")) = "No" Then
                            BarcasTiempo(ind - 1) = False
                        ElseIf CStr(lector("Tiempo")) = "Si" Then
                            BarcasTiempo(ind - 1) = False
                        End If
                    Case 14 To 19
                        m_Barcas(ind - 1).BackColor = cLibreSolar
                        m_Barcas(ind - 1).Text = CStr(lector("Nombre")) & Chr(13) & "--:--:--" & Chr(13) & CStr(lector("NV")) & " viajes"
                        m_Barcas(ind - 1).ForeColor = cNegro
                        If CStr(lector("Tiempo")) = "No" Then
                            BarcasTiempo(ind - 1) = False
                        ElseIf CStr(lector("Tiempo")) = "Si" Then
                            BarcasTiempo(ind - 1) = False
                        End If

                    Case Else
                        m_Barcas(ind - 1).BackColor = cLibreGold
                        m_Barcas(ind - 1).ForeColor = cNegro
                        m_Barcas(ind - 1).Text = CStr(lector("Nombre")) & Chr(13) & "--:--:--" & Chr(13) & CStr(lector("NV")) & " Viajes"
                        If CStr(lector("Tiempo")) = "No" Then
                            BarcasTiempo(ind - 1) = False
                        ElseIf CStr(lector("Tiempo")) = "Si" Then
                            BarcasTiempo(ind - 1) = False
                        End If

                End Select
            ElseIf CStr(lector("Estado")) = "A" Then
                m_Barcas(ind - 1).BackColor = cAlquilada
                PonerTextoLbl(m_Barcas(ind - 1), CStr(lector("Nombre")), "--:--:--", CInt(lector("NV")))
                m_Barcas(ind - 1).ForeColor = cBlanco

            End If
            ind = ind + 1
        Next
        lector.Close()
    End Sub

    Private Sub ActualizarBarcas(ByVal loc As Integer)
        Dim est As String
        Dim num As Integer

        num = MirarViajesBarca(loc)
        est = MirarEstadoBarca(loc)
        Select Case est
            'Si la barca esta "libre" Pone el estado en Alquilada (A)
            Case "L"
                'Cambia el estado de la barca y aumenta un viaje
                CambiarEstado(loc, "A", num + 1)

                'Pone en color de Alquilada
                m_Barcas(loc - 1).BackColor = cAlquilada
                m_Barcas(loc - 1).ForeColor = cBlanco
                num = num + 1
                ' m_Barcas(loc - 1).Text = MirarNombreBarca(loc) & Chr(13) & "--:--:--" & Chr(13) & num & " viajes"

                'Si la barca esta "alquilada" Pone el estado en Libre
            Case "A"
                CambiarEstado(loc, "L", num)
                Select Case loc
                    Case 1 To 13
                        m_Barcas(loc - 1).BackColor = cLibreRio
                    Case 14 To 19
                        m_Barcas(loc - 1).BackColor = cLibreSolar
                    Case Else
                        m_Barcas(loc - 1).BackColor = cLibreGold
                End Select
                m_Barcas(loc - 1).ForeColor = cNegro
                ' PonerTextoLbl(m_Barcas(loc - 1), MirarNombreBarca(loc), "--:--:--", MirarViajesBarca(loc))

        End Select
    End Sub

    ' Coge la media de viajes en el dia

    '  Private Function Media() As Double
    ' Dim cmd As SqlCommand
    ' Dim nv, nvn As Integer
    '
    '      cmd = New SqlCommand
    '       cmd.Connection = cnt
    '     cmd.CommandText = "select count(*) from Viaje where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"
    '    nv = CInt(cmd.ExecuteScalar)
    '   cmd.CommandText = "select count(*) from ViajeN where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"
    '  nvn = CInt(cmd.ExecuteScalar)
    ' nv = nv + nvn
    'Return CDbl(lblTotalEuros.Text) / nv
    'End Function

    ''' <summary>
    ''' Inicializa las tablas al cerrar el dia
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InicializarTablasBDD()
        Dim cmd, cmd1 As SqlCommand
        Dim s As String
        Dim n, f As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt
        For n = 1 To NRios + NSolares + 1
            s = "update Barcas set NV=0,Estado='L', Tiempo='No', Reservas = NULL where codigo = " & n
            cmd.CommandText = s
            f = cmd.ExecuteNonQuery
        Next
        cmd1 = New SqlCommand
        cmd1.Connection = cnt
        cmd1.CommandText = "update Contadores set Rio = 0, Solar = 0, Gold = 0, NumeroReservaRio=1,NumeroReservaSolar=1,NumeroReservaGold=1"
        f = cmd1.ExecuteNonQuery

        cmd1.CommandText = "delete from Reservas"
        f = cmd1.ExecuteNonQuery
    End Sub


    Private Sub btnreservascero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReservasCero.Click
        Dim respuesta As Integer = MessageBox.Show("¿Seguro reservas a 0?", "SI/NO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If respuesta = vbYes Then
            Try
                PonerReservasCero()

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub PonerReservasCero()
        Dim cmd As SqlCommand
        cmd = New SqlCommand

        cmd.Connection = cnt

        cmd.CommandText = "update contadores set NumeroReservaRio=1, NumeroReservaSolar=1, NumeroReservaGold=1"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from Reservas"
        cmd.ExecuteNonQuery()
    End Sub

    Private Function MirarEstadoBarca(ByVal loc As Integer) As String
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim est As String
        '---------------------------------------------
        cmd = New SqlCommand
        cmd.Connection = cnt
        'Mira el estado que tiene la barca
        cmd.CommandText = "select Barcas.Estado from Barcas where Barcas.Codigo = " & CStr(loc)

        Try
            lector = cmd.ExecuteReader
            b = lector.Read
            est = CStr(lector("Estado"))
            lector.Close()

            Return est
        Catch ex As Exception
            Return CStr("L")
        End Try

    End Function

    Private Function MirarNombreBarca(ByVal loc As Integer) As String
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim nom As String
        '---------------------------------------------
        cmd = New SqlCommand
        cmd.Connection = cnt
        'Mira el estado que tiene la barca
        cmd.CommandText = "select Barcas.nombre from Barcas where Barcas.Codigo = " & CStr(loc)
        Try
            lector = cmd.ExecuteReader
            b = lector.Read
            nom = CStr(lector("Nombre"))
            lector.Close()
        Catch ex As Exception
            nom = "Sin Conexión"
        End Try


        Return nom
    End Function

    Private Function MirarHoraBarca(ByVal loc As Integer) As DateTime
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim h As DateTime
        '---------------------------------------------
        cmd = New SqlCommand
        cmd.Connection = cnt
        'Mira la hora a la que ha salido la barca
        cmd.CommandText = "select Barcas.Hora from Barcas where Barcas.Codigo = " & CStr(loc)

        Try
            lector = cmd.ExecuteReader
            b = lector.Read
            h = CDate(lector("Hora"))
            lector.Close()

            Return h
        Catch ex As Exception

        End Try

    End Function

    Private Sub CambiarEstado(ByVal loc As Integer, ByVal e As String, ByVal numV As Integer)
        Dim n As Integer
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = cnt
        If e = "L" Then
            cmd.CommandText = "update Barcas set  Estado = '" & CStr(e) & "', NV = " & CStr(numV) & ", Hora = NULL, Tiempo='No' where Barcas.Codigo = " & CStr(loc)
        ElseIf e = "A" Then
            cmd.CommandText = "update Barcas set  Estado = '" & CStr(e) & "', NV = " & CStr(numV) & ", Hora = convert(char(10),getdate(),108), "
            cmd.CommandText = cmd.CommandText & " Reservas=convert(char(10),getdate() + '01:00:00', 108) where(Barcas.Codigo = " & CStr(loc) & ")"
        End If
        Try
            n = cmd.ExecuteNonQuery
        Catch ex As Exception

        End Try


    End Sub

    Private Function MirarViajesBarca(ByVal loc As Integer) As Integer
        Dim cmd As SqlCommand
        Dim lector As SqlDataReader
        Dim b As Boolean
        Dim numV As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Barcas.NV from Barcas where Barcas.Codigo = " & CStr(loc)
        Try
            lector = cmd.ExecuteReader
            b = lector.Read
            numV = CInt(lector("NV"))
            lector.Close()
        Catch ex As Exception

        End Try


        Return numV
    End Function

    ''' <summary>
    ''' Pone el texto en los label de las barcas
    ''' </summary>
    ''' <param name="lbl"></param>
    ''' <param name="Nom"></param>
    ''' <param name="Tie"></param>
    ''' <param name="NV"></param>
    ''' <remarks></remarks>
    Private Sub PonerTextoLbl(ByVal lbl As Label, ByVal Nom As String, ByVal Tie As String, ByVal NV As Integer)
        lbl.Text = Nom & Chr(13) & Tie & Chr(13) & CStr(NV)
        If NV = 1 Then
            lbl.Text = lbl.Text & " Viaje"
        Else
            lbl.Text = lbl.Text & " Viajes"
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim Estado As String
        Dim i As Integer
        Dim hs As Date
        Dim duracion As Double
        Dim tiempo As String

        'Si no hay conexion o la conexion se ha perdido...
        If cnt.State = ConnectionState.Closed Then
            lblConexion.Visible = True
        Else
            lblConexion.Visible = False
        End If

        For i = 1 To 20
            Estado = MirarEstadoBarca(i)
            If Estado = "A" Then
                m_Barcas(i - 1).BackColor = cAlquilada
                m_Barcas(i - 1).ForeColor = cBlanco
                hs = MirarHoraBarca(i)
                duracion = DateDiff(DateInterval.Second, hs, Now) / (24 * 3600)

                'ToolStripStatusLabel1.Text = CStr(DateDiff(DateInterval.Hour, hs, Now))
                tiempo = Format(Date.FromOADate(duracion), "00:mm:ss")
                PonerTextoLbl(m_Barcas(i - 1), MirarNombreBarca(i), tiempo, MirarViajesBarca(i))
                If CDate(tiempo) >= CDate("00:59:59") Then
                    'BarcasTiempo(i) = True
                    'Poner Tiempo de la Barca i en Si
                    PonerTiempo("Si", i)
                End If
                If MirarTiempo(i) = "Si" Then
                    m_Barcas(i - 1).BackColor = cTiempo
                    tiempo = Format(Date.FromOADate(duracion), "01:mm:ss")
                    PonerTextoLbl(m_Barcas(i - 1), MirarNombreBarca(i), tiempo, MirarViajesBarca(i))

                    'Beep()

                End If
                If CDate(tiempo) >= CDate("01:59:59") Then
                    tiempo = "Pasa más de 2 horas"
                    PonerTextoLbl(m_Barcas(i - 1), MirarNombreBarca(i), tiempo, MirarViajesBarca(i))
                    Beep()

                End If
            ElseIf Estado = "L" Then

                'Pone en color de Libre
                If (i >= 1) And (i <= 13) Then
                    m_Barcas(i - 1).BackColor = cLibreRio
                ElseIf (i >= 14) And (i <= 19) Then
                    m_Barcas(i - 1).BackColor = cLibreSolar
                ElseIf i = 20 Then
                    m_Barcas(i - 1).BackColor = cLibreGold
                End If
                m_Barcas(i - 1).ForeColor = cNegro
                PonerTextoLbl(m_Barcas(i - 1), MirarNombreBarca(i), "--:--:--", MirarViajesBarca(i))

            End If
        Next

        'Pone los contadores de los viajes de cada tipo de barca
        PonerLabelsContadores()
        lblTotalEuros.Text = CStr(TotalEuros())

        If QuePuntoVenta() = cBase Then
            lblMedia.Text = CStr(CDbl(lblTotalEuros.Text) / ContarBarcas(cBase))
        ElseIf QuePuntoVenta() = cOficina Then
            lblMedia.Text = CStr(CDbl(lblTotalEuros.Text) / ContarBarcas(cOficina))
        ElseIf QuePuntoVenta() = cBarkitos Or QuePuntoVenta() = cTPV2 Then
            lblMedia.Text = CStr(CDbl(lblTotalEuros.Text) / ContarBarcas(cBarkitos))
        End If
    End Sub

    Private Sub PonerTiempo(ByVal SN As String, ByVal ind As Integer)
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "update Barcas set Tiempo = '" & SN & "' where Codigo = " & ind
        Try
            Dim f As Integer = cmd.ExecuteNonQuery()

        Catch ex As Exception

        End Try
    End Sub

    Private Function MirarTiempo(ByVal ind As Integer) As String
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Tiempo from Barcas where Codigo = " & ind
        Try
            Return CStr(cmd.ExecuteScalar)
        Catch ex As Exception
            Return "No"
        End Try

    End Function

    Private Sub picRio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRio.Click
        grpPrecio.Enabled = True
        lblPrecio.Text = ""
        TipoBarca = 1
        picRio.Image = ImagenBarkitosMarcado
        lblPrecio.BackColor = cLibreRio
        lblPrecio.ForeColor = Color.Black
        picSolar.Image = ImagenSolar
        picGold.Image = ImagenGold
        picFerry.Image = ImagenFerry
        chkGrupo.Checked = False
        chkGrupo.Visible = False
        lblTicketsGrupo.Visible = False
        lblTicket.Enabled = False
        lblTicketReserva.Enabled = False
        ToolStripStatusLabel1.Text = CStr(cInformacion(2))

        ' Podemos sacar una reserva sin sacar el ticket
        lblTicketReserva.Enabled = True
    End Sub

    Private Sub picSolar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picSolar.Click
        grpPrecio.Enabled = True
        lblPrecio.Text = ""
        TipoBarca = 2
        picSolar.Image = ImagenSolarMarcado
        lblPrecio.BackColor = cLibreSolar
        lblPrecio.ForeColor = Color.Black
        picRio.Image = ImagenBarkitos
        picGold.Image = ImagenGold
        picFerry.Image = ImagenFerry
        chkGrupo.Checked = False
        chkGrupo.Visible = False
        lblTicketsGrupo.Visible = False
        lblTicket.Enabled = False
        lblTicketReserva.Enabled = False
        ToolStripStatusLabel1.Text = CStr(cInformacion(2))
        ' Podemos sacar una reserva sin sacar el ticket
        lblTicketReserva.Enabled = True

    End Sub

    Private Sub picGold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picGold.Click
        grpPrecio.Enabled = True
        lblPrecio.Text = ""
        TipoBarca = 3
        picGold.Image = ImagenGoldMarcado
        lblPrecio.BackColor = cLibreGold
        lblPrecio.ForeColor = Color.Black
        picRio.Image = ImagenBarkitos
        picSolar.Image = ImagenSolar
        picFerry.Image = ImagenFerry
        chkGrupo.Checked = False
        chkGrupo.Visible = False
        lblTicketsGrupo.Visible = False
        lblTicket.Enabled = False
        lblTicketReserva.Enabled = False
        ToolStripStatusLabel1.Text = CStr(cInformacion(2))
        ' Podemos sacar una reserva sin sacar el ticket
        lblTicketReserva.Enabled = True

    End Sub

    Private Sub picFerry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picFerry.Click
        TipoBarca = 4
        grpPrecio.Enabled = True
        lblPrecio.Text = ""
        picFerry.Image = ImagenFerryMarcado
        lblPrecio.BackColor = cFerry
        lblPrecio.ForeColor = Color.White
        chkGrupo.Visible = True
        lblPrecio.BackColor = Color.Blue
        picSolar.Image = ImagenSolar
        picRio.Image = ImagenBarkitos
        picGold.Image = ImagenGold
        lblTicket.Enabled = False
        lblTicketReserva.Enabled = False
        ToolStripStatusLabel1.Text = CStr(cInformacion(2))

    End Sub

    Private Sub lblAtras_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAtras.Click
        If Donde = "EnPrecio" Then
            If lblPrecio.Text = "" Then Exit Sub
            lblPrecio.Text = Mid$(lblPrecio.Text, 1, lblPrecio.Text.Length - 1)
            If lblPrecio.Text = "" Then
                lblTicket.Enabled = False
            End If
        Else
            If lblTicketsGrupo.Text = "" Then Exit Sub
            lblTicketsGrupo.Text = Mid$(lblTicketsGrupo.Text, 1, lblTicketsGrupo.Text.Length - 1)
        End If
    End Sub

    Private Sub lblPrecio_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblPrecio.TextChanged
        If lblPrecio.Text <> "" Then
            lblTicket.Enabled = True
            If TipoBarca <> 4 Then lblTicketReserva.Enabled = True
        End If
    End Sub

    Public Sub lblTicket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblTicket.Click
        Dim NumeroFactura As Integer
        Dim PVenta As Integer
        Dim l As String 'Guarda el logo a imprimir

        PVenta = QuePuntoVenta()

        'Solo Provisional ''''''''''''''''''''''''''''
        'If PVenta = cOficina Then PVenta = cBarkitos ''''
        ''''''''''''''''''''''''''''''''''''''''''''''

        Beep()
        grpPrecio.Enabled = False

        If lblPrecio.Text = "" Then lblPrecio.Text = "0"
        lblPrecio.ForeColor = Color.Black
        If txtAdultos.Text = "" Then txtAdultos.Text = "0"
        If txtNiños.Text = "" Then txtNiños.Text = "0"


        'Si TipoBarca=4 es para el Ferry
        Select Case TipoBarca

            Case 1
                lblTotalEuros.Text = CStr(CInt(lblTotalEuros.Text) + CInt(lblPrecio.Text))
                Try
                    NumeroFactura = AgregarViajeBDD(CDbl(lblPrecio.Text), CInt(txtAdultos.Text), CInt(txtNiños.Text), PVenta, TipoBarca)
                    'l = "\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2010\Projects\LosBarkitos2\LosBarkitos\LogoBarkitos.jpg"
                    l = "\F_LosBarkitos\LogoBarkitos.jpg"
                Catch ex As Exception
                    NumeroFactura = AgregarViajeBDDDesconectado(CDbl(lblPrecio.Text), CInt(txtAdultos.Text), CInt(txtNiños.Text), PVenta, TipoBarca)
                    l = "\F_LosBarkitos\LogoBarkitos.jpg"
                End Try
                'Imprime el ticket
                ImprimirTicket(l, NumeroFactura, CDbl(lblPrecio.Text), TipoBarca)
                'Imprime el comprobante
                'ImprimirComprobante(CDbl(lblPrecio.Text))
                'ImprimirOferta(0)
                ' AbrirCaja()

            Case 2
                lblTotalEuros.Text = CStr(CInt(lblTotalEuros.Text) + CInt(lblPrecio.Text))
                Try
                    NumeroFactura = AgregarViajeBDD(CDbl(lblPrecio.Text), CInt(txtAdultos.Text), CInt(txtNiños.Text), PVenta, TipoBarca)
                    'l = "\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2010\Projects\LosBarkitos2\LosBarkitos\LogoSolar.jpg"
                    l = "\F_LosBarkitos\LogoSolar.jpg"
                Catch ex As Exception
                    NumeroFactura = AgregarViajeBDDDesconectado(CDbl(lblPrecio.Text), CInt(txtAdultos.Text), CInt(txtNiños.Text), PVenta, TipoBarca)
                    l = "\F_LosBarkitos\LogoSolar.jpg"
                End Try
                ImprimirTicket(l, NumeroFactura, CDbl(lblPrecio.Text), TipoBarca)
                'ImprimirComprobante(CDbl(lblPrecio.Text))
                'ImprimirOferta(0)
                'AbrirCaja()

            Case 3
                lblTotalEuros.Text = CStr(CInt(lblTotalEuros.Text) + CInt(lblPrecio.Text))
                Try
                    NumeroFactura = AgregarViajeBDD(CDbl(lblPrecio.Text), CInt(txtAdultos.Text), CInt(txtNiños.Text), PVenta, TipoBarca)
                    'l = "\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\gold.jpg"
                    l = "\F_LosBarkitos\Gold.jpg"
                Catch ex As Exception
                    NumeroFactura = AgregarViajeBDDDesconectado(CDbl(lblPrecio.Text), CInt(txtAdultos.Text), CInt(txtNiños.Text), PVenta, TipoBarca)
                    l = "\F_LosBarkitos\Gold.jpg"

                End Try
                ImprimirTicket(l, NumeroFactura, CDbl(lblPrecio.Text), TipoBarca)
                'ImprimirComprobante(CDbl(lblPrecio.Text))
                'ImprimirOferta(0)
                ' AbrirCaja()

            Case 4 ' Ticket para el ferry
                If lblTicketsGrupo.Text = "" Then lblTicketsGrupo.Text = "1"
                NumeroFactura = AgregarViajeBDD(CDbl(lblPrecio.Text), CInt(lblTicketsGrupo.Text), PVenta)
                For nt As Integer = NumeroFactura - CInt(lblTicketsGrupo.Text) + 1 To NumeroFactura
                    'ImprimirTicket("\\" & My.Computer.Name & "\C\Documents and Settings\" & Mid$(My.User.Name, My.Computer.Name.Length + 2) & "\Mis documentos\Visual Studio 2008\Projects\LosBarkitos2\LosBarkitos\LogoFerry.jpg", _
                    '              nt, CDbl(lblPrecio.Text), TipoBarca)
                    ImprimirTicket("\F_LosBarkitos\LogoFerry.jpg", nt, CDbl(lblPrecio.Text), TipoBarca)
                    'Suma el numero de tickets para luego añadir a la BDD el grupo correspondiente
                    If btnCerrarGrupo.Visible = True Then
                        PaxGrupo = PaxGrupo + 1
                    End If

                Next
        End Select


        'Si el ticket no es del ferry,  borrar el precio y el label ticket
        If TipoBarca <> 4 Then
            lblPrecio.Text = ""
            lblTicket.Enabled = False
            lblTicketReserva.Enabled = False
            lblPrecio.BackColor = Color.Black
        Else
            lblTicketsGrupo.Text = "1"
        End If
        ToolStripStatusLabel1.Text = CStr(cInformacion(0))

    End Sub

    ''' <summary>
    ''' Nos dice que punto de venta es el que ha vendido el ticket
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuePuntoVenta() As Integer
        Select Case Mid$(My.User.Name, My.Computer.Name.Length + 2)
            Case "usuari", "Xus" ' TPV viejo
                Return cBarkitos
            Case "Propietario" ' TPV2
                Return cTPV2
            Case "Server", "Jesus"
                Return cBase
            Case "JESUS"
                Return cOficina
        End Select

    End Function


    '  Private Sub AgregarTicketBDDMySQL(ByVal Precio As Double, ByVal Num As Integer, ByVal PV As Integer, ByVal Part As Integer)
    ' Dim cmdMySQL As MySQLCommand
    ' Dim pMySQLTicket, pMySQLPrecio, pMySQLPV, pMySQLTP As MySQLParameter

    '   cmdMySQL = New MySQLCommand
    '    cmdMySQL.Connection = DBMySQL

    '   cmdMySQL.CommandText = "insert into Tickets(Numero,Precio ,Fecha,cod_PV,Part) values (@tic,@pre,curdate(),@cpv,@par) "

    '    pMySQLTicket = New MySQLParameter("@tic", Num)
    '    pMySQLTicket.Direction = ParameterDirection.Input
    '     cmdMySQL.Parameters.Add(pMySQLTicket)

    '    pMySQLPrecio = New MySQLParameter("@pre", Precio)
    '     pMySQLPrecio.Direction = ParameterDirection.Input
    '     cmdMySQL.Parameters.Add(pMySQLPrecio)

    '     pMySQLPV = New MySQLParameter("@cpv", PV)
    '     pMySQLPV.Direction = ParameterDirection.Input
    '      cmdMySQL.Parameters.Add(pMySQLPV)

    '    pMySQLTP = New MySQLParameter("@par", Part)
    '    pMySQLTP.Direction = ParameterDirection.Input
    '     cmdMySQL.Parameters.Add(pMySQLTP)

    '    Try
    '        cmdMySQL.ExecuteNonQuery()

    '    Catch ex As Exception
    '        MessageBox.Show("No se puede hacer la inserción en MySQL.", ex.Message.ToString)
    '     End Try
    '   End Sub




    ' Private Sub AgregarViajeBDDMySQL(ByVal Precio As Double, ByVal A As Integer, ByVal N As Integer, ByVal PV As Integer, ByVal TB As Integer, ByVal NumeroTicket As Integer)
    'Dim cmdMySQL As MySQLCommand
    ' Dim pMySQLTicket, pMySQLPrecio, pMySQLA, pMySQLN, pMySQLPV, pMySQLTP As MySQLParameter

    '   cmdMySQL = New MySQLCommand
    '   cmdMySQL.Connection = DBMySQL

    '    cmdMySQL.CommandText = "insert into Viaje(Numero,Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@tic,@pre, curdate(),@adu,@nin,@cpv,@ctb) "

    '   pMySQLTicket = New MySQLParameter("@tic", NumeroTicket)
    '   pMySQLTicket.Direction = ParameterDirection.Input
    '   cmdMySQL.Parameters.Add(pMySQLTicket)

    '  pMySQLPrecio = New MySQLParameter("@pre", Precio)
    '  pMySQLPrecio.Direction = ParameterDirection.Input
    '  cmdMySQL.Parameters.Add(pMySQLPrecio)

    '    pMySQLA = New MySQLParameter("@adu", A)
    '   pMySQLA.Direction = ParameterDirection.Input
    '  cmdMySQL.Parameters.Add(pMySQLA)
    '
    '   pMySQLN = New MySQLParameter("@nin", N)
    '  pMySQLN.Direction = ParameterDirection.Input
    ' cmdMySQL.Parameters.Add(pMySQLN)
    '
    '   pMySQLPV = New MySQLParameter("@cpv", PV)
    '  pMySQLPV.Direction = ParameterDirection.Input
    ' cmdMySQL.Parameters.Add(pMySQLPV)

    '    pMySQLTP = New MySQLParameter("@ctb", TB)
    '   pMySQLTP.Direction = ParameterDirection.Input
    '  cmdMySQL.Parameters.Add(pMySQLTP)
    ' Try
    '    cmdMySQL.ExecuteNonQuery()

    '    Catch ex As Exception
    '       MessageBox.Show("No se puede hacer la inserción en MySQL.", ex.Message.ToString)
    '  End Try

    'End Sub

    ''' <summary>
    ''' Nos añade los tickets necesarios en la BDD del ferry
    ''' </summary>
    ''' <param name="Precio"></param>
    ''' <param name="Num"></param>
    ''' <remarks></remarks>
    Private Function AgregarViajeBDD(ByVal Precio As Double, ByVal Num As Integer, ByVal PV As Integer) As Integer
        Dim i As Integer
        Dim NumeroTicket As Integer

        'Si se saca el ticket desde MarinaFerry siempre sera en blanco
        If chkNegro.Visible = False Then
            chkNegro.Checked = False
        End If
        'Oficial
        If chkNegro.Checked = False Then
            NumeroTicket = TicketFerrySiguiente()
            For i = 1 To Num
                InsertarViajeFerry(NumeroTicket, Precio, PV)
                NumeroTicket += 1
            Next
            GuardarTicketFerrySiguiente(NumeroTicket)
            Return NumeroTicket - 1
            Beep()
            ' Negro
        ElseIf chkNegro.Checked = True Then
            If Num = 1 Then
                Randomize()
                NumeroTicket = CInt(Rnd() * TicketFerrySiguiente() - 1)
            ElseIf Num > 1 Then
                NumeroTicket = CInt(TicketFerrySiguiente() - Num - 1)
            End If
            For i = 1 To Num
                InsertarViajeFerry(NumeroTicket, Precio, PV)
                NumeroTicket += 1
            Next
            Return NumeroTicket
            Beep()
        End If

    End Function

    ''' <summary>
    ''' Nos añade un viaje en la BDD para un barkito
    ''' </summary>
    ''' <param name="Precio"></param>
    ''' <param name="A"></param>
    ''' <param name="N"></param>
    ''' <param name="PV"></param>
    ''' <param name="TB"></param>
    ''' <remarks></remarks>
    Private Function AgregarViajeBDD(ByVal Precio As Double, ByVal A As Integer, ByVal N As Integer, ByVal PV As Integer, ByVal TB As Integer) As Integer
        Dim cmd As SqlCommand
        Dim pTicket, pPrecio, pA, pN, pPV, pTP As SqlParameter
        Dim NumeroTicket As Integer = TicketSiguiente()
        Dim NN As Integer

        If cnt.State = ConnectionState.Broken Then
            AgregarViajeBDDDesconectado(Precio, A, N, PV, TB)
            Exit Function
        End If

        'miro a ver si es en blanco o negro
        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select NumNegro from Contadores"
        NN = CInt(cmd.ExecuteScalar)

        'Porcentaje de blancos o negros
        Randomize()
        If CInt(Int((NN * Rnd()) + 1)) <= 5 Then
            chkNegro.Checked = True
        Else
            chkNegro.Checked = False
        End If

        '------------------------------------------
        'las pruebas siempre se realizan en negro
        If QuePuntoVenta() = cOficina Then
            chkNegro.Checked = True
        End If

        'Si se saca el ticket desde MarinaFerry AHORA siempre sera en BLANCO
        If chkNegro.Visible = False Then
            chkNegro.Checked = False
        End If

        If chkNegro.Checked = False Then

            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "insert into Viaje (Numero,Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@tic,@pre, getdate(),@adu,@nin,@cpv,@ctb) "
            pTicket = New SqlParameter("@tic", NumeroTicket)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Precio)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pA = New SqlParameter("@adu", A)
            pA.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pA)
            pN = New SqlParameter("@nin", N)
            pN.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pN)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pTP = New SqlParameter("@ctb", TB)
            pTP.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTP)
            cmd.ExecuteNonQuery()
            GuardarTicketSiguiente(NumeroTicket + 1)
            ActualizarContadoresBarcas(TB)
            PonerContadoresBarcasLabels(TB)
            Beep()
            '    lblMedia.Text = CStr(Media())

            'Tiquet de barkitos en negro
        ElseIf chkNegro.Checked = True Then
            'Randomize()
            'NumeroTicket = CInt(10000 * Rnd() + 1)
            NumeroTicket = NumeroTicket - 1 'El numero ticket es el ultimo blanco que ha salido
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "insert into ViajeN (Numero,Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@tic,@pre, getdate(),@adu,@nin,@cpv,@ctb) "
            pTicket = New SqlParameter("@tic", NumeroTicket)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Precio)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)

            pA = New SqlParameter("@adu", A)
            pA.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pA)

            pN = New SqlParameter("@nin", N)
            pN.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pN)

            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)

            pTP = New SqlParameter("@ctb", TB)
            pTP.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTP)

            cmd.ExecuteNonQuery()

            'Hago la insercion en la BDD MySQL


            'GuardarTicketSiguiente(NumeroTicket + 1)
            ActualizarContadoresBarcas(TB)
            PonerContadoresBarcasLabels(TB)
            Beep()
            'lblMedia.Text = CStr(Media())

        End If
        'Try
        'AgregarViajeBDDMySQL(Precio, A, N, PV, TB, NumeroTicket)

        'Catch ex As Exception

        'End Try

        Return NumeroTicket

    End Function


    Private Function AgregarViajeBDDDesconectado(ByVal Precio As Double, ByVal A As Integer, ByVal N As Integer, ByVal PV As Integer, ByVal TB As Integer) As Int16
        Dim cmd As SqlCommand
        Dim pPrecio, pA, pN, pPV, pTP As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = cnt2
        cmd.CommandText = "insert into Viaje (Precio ,Fecha,Adultos,Niños,cod_PV,cod_Barca) values (@pre, getdate(),@adu,@nin,@cpv,@ctb) "

        pPrecio = New SqlParameter("@pre", Precio)
        pPrecio.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPrecio)
        pA = New SqlParameter("@adu", A)
        pA.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pA)
        pN = New SqlParameter("@nin", N)
        pN.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pN)
        pPV = New SqlParameter("@cpv", PV)
        pPV.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPV)
        pTP = New SqlParameter("@ctb", TB)
        pTP.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pTP)
        cmd.ExecuteNonQuery()
        Beep()
        Return 1

    End Function

    ''' <summary>
    ''' Inserta los viajes del ferry de uno en uno
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub InsertarViajeFerry(ByVal NT As Integer, ByVal Pr As Double, ByVal PV As Integer)
        Dim cmd As SqlCommand
        Dim pTicket, pPrecio, pPV, pPart As SqlParameter
        Dim Partis As Boolean

        If Pr = CDbl(10) Or Pr = CDbl(5) Or Pr = CDbl(3) Then
            Partis = True
        Else
            Partis = False
        End If

        If cnt.State = ConnectionState.Broken Then
            InsertarViajeFerryDesconectado(NT, Pr, PV)
            Exit Sub
        End If

        'Si se saca el ticket desde MarinaFerry siempre sera en blanco
        If chkNegro.Visible = False Then
            chkNegro.Checked = False
        End If


        ' Viaje marinaFerry en oficial
        If chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "insert into Tickets (Numero,Precio ,Fecha, cod_PV,Part) values (@tic,@pre, getdate(),@cpv,@par) "
            pTicket = New SqlParameter("@tic", NT)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Pr)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pPart = New SqlParameter("@par", Partis)
            pPart.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPart)
            cmd.ExecuteNonQuery()
            ' Viaje MF en Negro
        ElseIf chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "insert into TicketsN (Numero,Precio ,Fecha, cod_PV, Part) values (@tic,@pre, getdate(),@cpv,@par) "
            pTicket = New SqlParameter("@tic", NT)
            pTicket.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pTicket)

            pPrecio = New SqlParameter("@pre", Pr)
            pPrecio.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPrecio)
            pPV = New SqlParameter("@cpv", PV)
            pPV.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPV)
            pPart = New SqlParameter("@par", Partis)
            pPart.Direction = ParameterDirection.Input
            cmd.Parameters.Add(pPart)

            cmd.ExecuteNonQuery()


        End If

        ' Insercion del ticket en MySQL
        ' If Partis Then
        'AgregarTicketBDDMySQL(Pr, NT, PV, 1)
        ' Else
        ' AgregarTicketBDDMySQL(Pr, NT, PV, 0)

        ' End If

    End Sub

    Private Sub InsertarViajeFerryDesconectado(ByVal NT As Integer, ByVal Pr As Double, ByVal PV As Integer)
        Dim cmd As SqlCommand
        Dim pPrecio, pPV As SqlParameter

        cmd = New SqlCommand
        cmd.Connection = cnt2
        cmd.CommandText = "insert into Tickets (Precio ,Fecha, cod_PV) values (@pre, getdate(),@cpv) "

        pPrecio = New SqlParameter("@pre", Pr)
        pPrecio.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPrecio)
        pPV = New SqlParameter("@cpv", PV)
        pPV.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPV)
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub ActualizarContadoresBarcas(ByVal tb As Integer)
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = cnt
        Select Case tb
            Case 1
                cmd.CommandText = "update Contadores set Rio = Rio + 1"
            Case 2
                cmd.CommandText = "update Contadores set Solar = Solar + 1"
            Case 3
                cmd.CommandText = "update Contadores set Gold = Gold + 1"
        End Select
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub PonerContadoresBarcasLabels()
        PonerContadoresBarcasLabels(1)
        PonerContadoresBarcasLabels(2)
        PonerContadoresBarcasLabels(3)
    End Sub

    Private Sub PonerLabelsContadores()

        lblRios.Text = CStr(ContarRios())
        lblSolares.Text = CStr(ContarSolares())
        lblGolds.Text = CStr(ContarGolds())
        lblTotal.Text = CStr(CInt(lblRios.Text) + CInt(lblSolares.Text) + CInt(lblGolds.Text))

    End Sub

    Private Function TotalEuros() As Double
        Dim cmd As SqlCommand
        Dim A As Double

        cmd = New SqlCommand
        cmd.Connection = cnt

        If QuePuntoVenta() = cBarkitos Or QuePuntoVenta() = cTPV2 Then
            cmd.CommandText = "select sum(Precio) from Viaje where convert(char(10),getdate(),103) = convert(char(10), Viaje.Fecha,103) and Viaje.Cod_PV='2'"
        ElseIf QuePuntoVenta() = cBase Then
            cmd.CommandText = "select sum(Precio) from Viaje where convert(char(10),getdate(),103) = convert(char(10), Viaje.Fecha,103) and Viaje.Cod_PV='1'"
        ElseIf QuePuntoVenta() = cOficina Then
            cmd.CommandText = "select sum(Precio) from Viaje where convert(char(10),getdate(),103) = convert(char(10), Viaje.Fecha,103) "
        End If

        Try
            A = CDbl(cmd.ExecuteScalar)
        Catch ex As Exception
            A = 0
        End Try

        If QuePuntoVenta() = cBarkitos Or QuePuntoVenta() = cTPV2 Then
            cmd.CommandText = "select sum(Precio) from ViajeN where convert(char(10),getdate(),103) = convert(char(10), ViajeN.Fecha,103) and ViajeN.Cod_PV='2'"
        ElseIf QuePuntoVenta() = cBase Then
            cmd.CommandText = "select sum(Precio) from ViajeN where convert(char(10),getdate(),103) = convert(char(10), ViajeN.Fecha,103) and ViajeN.Cod_PV='1'"
        ElseIf QuePuntoVenta() = cOficina Then
            cmd.CommandText = "select sum(Precio) from ViajeN where convert(char(10),getdate(),103) = convert(char(10), ViajeN.Fecha,103)"
        End If

        Try
            Return A + CDbl(cmd.ExecuteScalar)
        Catch ex As Exception
            Return A
        End Try
    End Function


    Private Function ContarRios() As Integer
        Dim cmd As SqlCommand
        Dim A As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt

        cmd.CommandText = "select count(*) from Viaje where Viaje.Cod_Barca = '1' and convert(char(10),getdate(),103) = convert(char(10), Viaje.Fecha,103)"
        Try
            A = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 0
        End Try
        cmd.CommandText = "select count(*) from ViajeN where ViajeN.Cod_Barca = '1' and convert(char(10),getdate(),103) = convert(char(10), ViajeN.Fecha,103)"
        Try
            Return A + CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 0

        End Try
    End Function

    Private Function ContarSolares() As Integer
        Dim cmd As SqlCommand
        Dim A As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt

        cmd.CommandText = "select count(*) from Viaje where Viaje.Cod_Barca = '2' and convert(char(10),getdate(),103) = convert(char(10), Viaje.Fecha,103)"
        Try
            A = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 0
        End Try
        cmd.CommandText = "select count(*) from ViajeN where ViajeN.Cod_Barca = '2' and convert(char(10),getdate(),103) = convert(char(10), ViajeN.Fecha,103)"
        Try
            Return A + CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Function ContarGolds() As Integer
        Dim cmd As SqlCommand
        Dim A As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt

        cmd.CommandText = "select count(*) from Viaje where Viaje.Cod_Barca = '3' and convert(char(10),getdate(),103) = convert(char(10), Viaje.Fecha,103)"
        Try
            A = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 0
        End Try

        cmd.CommandText = "select count(*) from ViajeN where ViajeN.Cod_Barca = '3' and convert(char(10),getdate(),103) = convert(char(10), ViajeN.Fecha,103)"

        Try
            Return A + CInt(cmd.ExecuteScalar)
        Catch
            Return 0
        End Try
    End Function

    Private Sub PonerContadoresBarcasLabels(ByVal tb As Integer)
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = cnt
        Try
            Select Case tb
                Case 1
                    cmd.CommandText = "select Rio from Contadores"
                    lblRios.Text = CStr(CInt(cmd.ExecuteScalar))
                Case 2
                    cmd.CommandText = "select Solar from Contadores"
                    lblSolares.Text = CStr(CInt(cmd.ExecuteScalar))
                Case 3
                    cmd.CommandText = "Select Gold from Contadores"
                    lblGolds.Text = CStr(CInt(cmd.ExecuteScalar))
            End Select
            lblTotal.Text = CStr(CInt(lblRios.Text) + CInt(lblSolares.Text) + CInt(lblGolds.Text))

        Catch ex As Exception
            lblTotal.Text = "SC"
        End Try


    End Sub

    ''' <summary>
    ''' Devuelve el numero de ticket para el ferry
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TicketFerrySiguiente() As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Numero_Ticket_Ferry from Contadores"
        Try
            Return CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 999
        End Try
    End Function

    ''' <summary>
    ''' Devuelve el Numero de ticket para un barkito
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TicketSiguiente() As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Numero_Ticket from Contadores"
        Try
            Return CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return 999
        End Try
    End Function

    ''' <summary>
    ''' Guarda el numero de ticket del ferry
    ''' </summary>
    ''' <param name="t"></param>
    ''' <remarks></remarks>
    Public Sub GuardarTicketFerrySiguiente(ByVal t As Integer)
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "update Contadores set Numero_Ticket_Ferry = " & CStr(t)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' Guarda el numero de Ticket de los barkitos
    ''' </summary>
    ''' <param name="t"></param>
    ''' <remarks></remarks>
    Private Sub GuardarTicketSiguiente(ByVal t As Integer)
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "update Contadores set Numero_Ticket = " & CStr(t)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub lblMasA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMasA.Click
        If txtAdultos.Text = "" Then
            txtAdultos.Text = "1"
        Else
            txtAdultos.Text = CStr(CInt(txtAdultos.Text) + 1)
        End If
    End Sub

    Private Sub lblMenosA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMenosA.Click
        If txtAdultos.Text = "0" Then
            txtAdultos.Text = "0"
        Else
            txtAdultos.Text = CStr(CInt(txtAdultos.Text) - 1)
        End If

    End Sub

    Private Sub lblMenosN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMenosN.Click
        If txtNiños.Text = "0" Then
            txtNiños.Text = "0"
        Else
            txtNiños.Text = CStr(CInt(txtNiños.Text) - 1)
        End If

    End Sub

    Private Sub lblMasN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMasN.Click
        If txtNiños.Text = "" Then
            txtNiños.Text = "1"
        Else
            txtNiños.Text = CStr(CInt(txtNiños.Text) + 1)
        End If

    End Sub

    Private Sub lblTotal_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTotal.MouseEnter
        lblTotalEuros.Visible = True
    End Sub

    Private Sub lblTotal_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTotal.MouseLeave
        lblTotalEuros.Visible = False
    End Sub

    Private Function TotEuros() As Integer
        Dim cmd As SqlCommand

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select sum(Precio) from Viaje where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"

        Try
            Return CInt(cmd.ExecuteScalar)
        Catch ex As Exception
        End Try
    End Function

    Public Function HayDiasMesFerry(ByVal f As Integer) As Boolean
        Dim cmd As SqlCommand
        Dim c As Integer
        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select count(*) from Tickets where datepart(mm,Tickets.Fecha) =  " & CStr(f)
        Try
            c = CInt(cmd.ExecuteScalar)
            If c = 0 Then
                Return False
            Else : Return True
            End If
        Catch ex As Exception

        End Try


    End Function

    Public Function HayDiasMesBkts(ByVal f As Integer) As Boolean
        Dim cmd As SqlCommand
        Dim c As Integer

        If chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "select count(*) from Viaje where datepart(mm,viaje.Fecha) =  " & CStr(f)
            Try
                c = CInt(cmd.ExecuteScalar)
                If c = 0 Then
                    Return False
                Else : Return True
                End If
            Catch ex As Exception

            End Try

            ' Hay viajes en negro
        ElseIf chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "select count(*) from ViajeN where datepart(mm,viajeN.Fecha) =  " & CStr(f)
            Try
                c = CInt(cmd.ExecuteScalar)
                If c = 0 Then
                    Return False
                Else : Return True
                End If
            Catch ex As Exception

            End Try

        End If
    End Function

    Private Function HayViajes() As Boolean
        Dim cmd As SqlCommand
        Dim c As Integer

        If chkNegro.Checked = False Then
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "select count(*) from Viaje where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"
            Try
                c = CInt(cmd.ExecuteScalar)
            Catch ex As Exception

            End Try

            If c = 0 Then
                Return False
            Else : Return True
            End If
            'En negro
        ElseIf chkNegro.Checked = True Then
            cmd = New SqlCommand
            cmd.Connection = cnt
            cmd.CommandText = "select count(*) from ViajeN where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"
            Try
                c = CInt(cmd.ExecuteScalar)
                If c = 0 Then
                    Return False
                Else : Return True
                End If
            Catch ex As Exception

            End Try


        End If
    End Function

    Private Sub chkGrupo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGrupo.CheckedChanged
        If chkGrupo.Checked Then
            FGrupo.Show()
            lblTicketsGrupo.Visible = True
            Donde = "EnTicketsGrupo"
        Else
            lblTicketsGrupo.Text = "1"
            lblTicketsGrupo.Visible = False
            Donde = "EnPrecio"
        End If
    End Sub

    Private Sub lblPunto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPunto.Click
        If lblPrecio.Text.Contains(",") Then Exit Sub
        lblPrecio.Text = lblPrecio.Text & ","
    End Sub

    Private Sub lblPrecio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPrecio.Click
        Donde = "EnPrecio"
        ToolStripStatusLabel1.Text = cInformacion(3).ToString
    End Sub

    Private Sub lblTicketsGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblTicketsGrupo.Click
        Donde = "EnTicketsGrupo"
        ToolStripStatusLabel1.Text = cInformacion(4).ToString
    End Sub

    Private Sub LosBarkitosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LosBarkitosToolStripMenuItem.Click
        fDiario.Show()
    End Sub

    Private Sub LosBarkitosToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LosBarkitosToolStripMenuItem1.Click
        fMensual.Show()
    End Sub

    Private Sub MarinaFerryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarinaFerryToolStripMenuItem.Click
        fDiarioFerry.Show()
    End Sub

    Private Sub MarinaFerryToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarinaFerryToolStripMenuItem1.Click
        fMensualFerry.Show()
    End Sub

    Public Sub ImprimirTicket(ByVal Logo As String, ByVal NumFact As Integer, ByVal Precio As Double, ByVal Tipo As Integer)
        Dim PVenta As Integer
        Dim tic As Ticket
        Dim Imagen As Bitmap
        tic = New Ticket
        Dim txtPVenta As String = ""

        PVenta = QuePuntoVenta()
        Select Case PVenta
            Case cBase
                txtPVenta = "Oficina"
            Case cBarkitos
                txtPVenta = "LosBarkitos"
            Case cTPV2
                txtPVenta = "LosBarkitos"
            Case cOficina
                txtPVenta = "Portátil"
        End Select


        Imagen = New Bitmap(Logo, True)
        tic.HeaderImage = Imagen
        ' Si es un ticket en negro, ponemos un punto al final de EMPURIABRAVA
        If chkNegro.Checked = True Then
            tic.AddHeaderLine("E M P U R I A B R A V A.")
        Else
            tic.AddHeaderLine("E M P U R I A B R A V A")
        End If
        tic.AddHeaderLine("C/ Carmançó, 1")
        tic.AddHeaderLine("17487 Castelló d'Empúries")
        tic.AddHeaderLine("Tel. 972.45.25.79")
        tic.AddHeaderLine("Fax. 972.45.63.24")
        'Añado el N.I.F. segun sea:
        Select Case TipoBarca
            Case 1, 2, 3 : tic.AddHeaderLine("N.I.F.: B-17825137")
            Case 4 : tic.AddHeaderLine("N.I.F.: B-17496761")
        End Select
        tic.AddHeaderLine("")
        'Quito la hora y el dia por si de cas...
        tic.AddHeaderLine(txtPVenta & " - " & CStr(Now.Date))
        If lblGrupoGuia.Text <> "" Then tic.AddHeaderLine(lblGrupoGuia.Text)
        tic.AddSubHeaderLine("Núm. Factura: " & CStr(NumFact))

        Select Case TipoBarca
            Case 1 : tic.AddItem("1", " Barkito", CStr(Precio))
            Case 2 : tic.AddItem("1", " Solar", CStr(Precio))
            Case 3 : tic.AddItem("1", " Gold", CStr(Precio))
            Case 4 : tic.AddItem("1", " Ferry", CStr(Precio))
        End Select

        'Si el ticket es para el ferry...
        If Tipo = 4 Then
            tic.AddTotal("BASE", CStr(Format(Precio / 1.21, "Standard")))
            tic.AddTotal("IVA - 10 %", CStr(Format(Precio - Precio / 1.21, "Standard")))
        Else
            tic.AddTotal("BASE", CStr(Format(Precio / 1.21, "Standard")))
            tic.AddTotal("IVA - 21%", CStr(Format(Precio - Precio / 1.21, "Standard")))
        End If
        tic.AddTotal("TOTAL", CStr(Precio))
        tic.AddFooterLine("Esperamos verles pronto de nuevo")
        tic.AddFooterLine("Gracias por su visita")
        tic.AddFooterLine("losbarkitos.com - marinaferry.es")
        tic.AddFooterLine("--------------------------------")
        tic.AddFooterLine("")
        CorteImpresora()
        tic.AddFooterLine("LosBarKiTos ShOp")
        tic.AddFooterLine("----------------------")
        tic.AddFooterLine("Presentando este ticket")
        tic.AddFooterLine("tiene un DESCUENTO DEL")
        tic.AddFooterLine("10 % en la compra total de")
        tic.AddFooterLine("LosBarKiTos ShOp")
        tic.AddFooterLine("----------------------------------")



        If PVenta = 1 Then
            tic.PrintTicket("SAMSUNG SRP-350")
        ElseIf PVenta = 2 Then
            tic.PrintTicket("SRP350 Partial Cut")
        ElseIf PVenta = cTPV2 Then
            tic.PrintTicket("SRP350 Partial")
        ElseIf PVenta = cOficina Then
            tic.PrintTicket("EPSON Stylus Photo R300 Series")
            ' tic.PrintTicket("EPSON Stylus Photo R300 Series")
        End If
        'AbrirCajon()
    End Sub

    Public Sub ImprimirComprobante(ByVal Precio As Double)
        Dim PVenta As Integer
        Dim tic As Ticket
        tic = New Ticket

        PVenta = QuePuntoVenta()
        tic.AddHeaderLine("=======================")
        tic.AddHeaderLine("||COMPROBANTE BARKITO||")
        tic.AddHeaderLine("||Precio : " & CStr(Precio) & " € ||")
        tic.AddHeaderLine("=======================")
        'Quito la hora y el dia por si de cas...
        'tic.AddHeaderLine(txtPVenta & " " & CStr(Now))
        tic.AddFooterLine("")

        If PVenta = 1 Then
            tic.PrintTicket("SAMSUNG SRP-350")
        ElseIf PVenta = 2 Then
            tic.PrintTicket("SRP350 Partial Cut")
        ElseIf PVenta = 4 Then
            tic.PrintTicket("SRP350 Partial")
        ElseIf PVenta = 3 Then
            tic.PrintTicket("SRP350 Partial Cut")
            'tic.PrintTicket("EPSON Stylus Photo R300 Series")
        End If
    End Sub


    'Si tipo es '0' la oferta viene despues de un ticket y  se exige ticket antiguo
    'Si tipo es '1' la oferta viene del lbldescuento y no se exige ticket antiguo
    Public Sub ImprimirOferta(ByVal tipo As Integer)
        Dim PVenta As Integer
        Dim tic As Ticket
        tic = New Ticket
        Dim txtPVenta As String

        PVenta = QuePuntoVenta()
        Select Case PVenta
            Case cBase
                txtPVenta = "Oficina"
            Case cBarkitos Or cTPV2
                txtPVenta = "LosBarkitos"
            Case Else
                txtPVenta = "Portátil"
        End Select


        ' Si es un ticket en negro, ponemos un punto al final de EMPURIABRAVA
        If chkNegro.Checked = True Then
            tic.AddHeaderLine("O F E R T A / D I S C O U N T .")
        Else
            tic.AddHeaderLine("O F E R T A / D I S C O U N T")
        End If
        tic.AddHeaderLine("*************************")
        tic.AddHeaderLine("*   Sólo de 10h a 14h   *")
        tic.AddHeaderLine("*   Solament 10h a 14h  *")
        tic.AddHeaderLine("*   Only 10h to 14h     *")
        tic.AddHeaderLine("*   Seulement 10h à 14h *")
        tic.AddHeaderLine("*   Nur 10h bis 14h     *")
        tic.AddHeaderLine("*************************")
        tic.AddHeaderLine("")

        tic.AddItem("-", " Descuento", " - 5 €")

        'Si el ticket es para el ferry...
        If tipo = 0 Then
            tic.AddHeaderLine("Imprescindible ticket de compra")
            tic.AddHeaderLine("Essential ticket purchase")
            tic.AddHeaderLine("Essential l'achat du billet")
            tic.AddHeaderLine("Essential-Ticket kaufen")
            tic.AddHeaderLine("______________________________")
        End If
        tic.AddFooterLine("Gracias por su visita")
        tic.AddFooterLine("losbarkitos.com - marinaferry.es")
        tic.AddFooterLine("--------------------------------")
        tic.AddFooterLine("")
        tic.AddFooterLine("")

        If PVenta = 1 Then
            tic.PrintTicket("SAMSUNG SRP-350")
        ElseIf PVenta = 2 Then
            tic.PrintTicket("SRP350 Partial Cut")
        ElseIf PVenta = 4 Then
            tic.PrintTicket("SRP350 Partial")
        ElseIf PVenta = 3 Then
            tic.PrintTicket("EPSON Stylus Photo R300 Series")
        End If

    End Sub

    Private Sub BarcasPorLlegarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BarcasPorLlegarToolStripMenuItem.Click
        fBarcasPorLlegar.Orden = "Hora"
        fBarcasPorLlegar.lblOk.Enabled = False
        fBarcasPorLlegar.Show()
        ToolStripStatusLabel1.Text = CStr(cInformacion(0))

    End Sub

    Private Sub lblTicketReserva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblTicketReserva.Click
        fBarcasPorLlegar.Orden = "Reservas"
        fBarcasPorLlegar.lblOk.Enabled = True
        fBarcasPorLlegar.Show()
        lblTicketReserva.Enabled = False
    End Sub

    Private Sub RioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RioToolStripMenuItem.Click
        Reservas.TipoReserva = 1
        Reservas.Show()
    End Sub

    Private Sub SolarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SolarToolStripMenuItem.Click
        Reservas.TipoReserva = 2
        Reservas.Show()

    End Sub

    Private Sub GoldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoldToolStripMenuItem.Click
        Reservas.TipoReserva = 3
        Reservas.Show()
    End Sub

    Private Sub TodasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TodasToolStripMenuItem.Click
        Reservas.TipoReserva = 0
        Reservas.Show()
    End Sub

    Private Sub btnCerrarGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrarGrupo.Click
        Dim CodGuia As Integer
        Dim CodGrupo As Integer
        Dim cmd As SqlCommand
        Dim com As Double

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select Codigo from Grupos where Nombre = '" & NombreGrupo & "'"
        CodGrupo = CInt(cmd.ExecuteScalar)

        cmd.CommandText = "select Codigo from Guias where Nombre = '" & NombreGuia & "'"
        CodGuia = CInt(cmd.ExecuteScalar)
        InsertarGrupo(CodGrupo, CodGuia, PaxGrupo)
        If CodGrupo = 1 Then
            com = 1.5 * PaxGrupo
        Else
            com = 2 * PaxGrupo
        End If
        If QuePuntoVenta() = cBase Then
            MessageBox.Show("Numero de pax de " & lblGrupoGuia.Text & " : " & PaxGrupo, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, False)
        ElseIf QuePuntoVenta() = cOficina Then
            MessageBox.Show("Numero de pax de " & lblGrupoGuia.Text & " : " & PaxGrupo, "Comisión: " & com, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, False)
        End If
        btnCerrarGrupo.Visible = False
        lblGrupoGuia.Text = ""
        PaxGrupo = 0
    End Sub

    ''' <summary>
    ''' Inserta el grupo en la base de datos
    ''' </summary>
    ''' <param name="CodGrupo"></param>
    ''' <param name="CodGuia"></param>
    ''' <param name="Pax"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub InsertarGrupo(ByVal CodGrupo As Integer, ByVal CodGuia As Integer, ByVal Pax As Integer)

        Dim cmd As SqlCommand
        Dim pCodigo, pNombre, pPax, pFec As SqlParameter
        Dim Fec As DateTime

        Fec = Now.Date

        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "insert into Viajes_Grupo (Cod_Grupo, Cod_Guia ,Fecha, Pax) values (@cod,@nom, @fec, @pax) "
        pCodigo = New SqlParameter("@cod", CodGrupo)
        pCodigo.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pCodigo)

        pNombre = New SqlParameter("@nom", CodGuia)
        pNombre.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pNombre)

        pFec = New SqlParameter("@fec", Fec)
        pFec.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pFec)

        pPax = New SqlParameter("@pax", Pax)
        pPax.Direction = ParameterDirection.Input
        cmd.Parameters.Add(pPax)

        cmd.ExecuteNonQuery()
    End Sub

    Private Function ContarBarcas(ByVal donde As Integer) As Integer
        Dim cmd As SqlCommand
        Dim aux As Integer

        cmd = New SqlCommand
        cmd.Connection = cnt

        Select Case donde
            Case cBarkitos, cBase
                cmd.CommandText = "select count(*) from Viaje where convert(char(10),Fecha,103) = convert(char(10),getdate(),103) and Cod_PV = '" & CStr(donde) & "'"
            Case cOficina
                cmd.CommandText = "select count(*) from Viaje where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"
        End Select

        Try
            aux = CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            aux = 0
        End Try

        Select Case donde
            Case cBarkitos, cBase
                cmd.CommandText = "select count(*) from ViajeN where convert(char(10),Fecha,103) = convert(char(10),getdate(),103) and Cod_PV = '" & CStr(donde) & "'"
            Case cOficina
                cmd.CommandText = "select count(*) from ViajeN where convert(char(10),Fecha,103) = convert(char(10),getdate(),103)"
        End Select

        Try
            Return aux + CInt(cmd.ExecuteScalar)
        Catch ex As Exception
            Return aux
        End Try
    End Function

    'Private Sub chkNegro_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

    'e.SuppressKeyPress = False
    'Select Case e.KeyCode
    'Case Keys.D1, Keys.NumPad1
    '    lblBarcas_Click(lblBarcas_000, e)
    'Case Keys.D2, Keys.NumPad2
    '    lblBarcas_Click(lblBarcas_001, e)
    'Case Keys.D3, Keys.NumPad3
    '    lblBarcas_Click(lblBarcas_002, e)
    'Case Keys.D4, Keys.NumPad4
    '    lblBarcas_Click(lblBarcas_003, e)
    'Case Keys.D5, Keys.NumPad5
    '    lblBarcas_Click(lblBarcas_004, e)
    'Case Keys.D6, Keys.NumPad6
    '   lblBarcas_Click(lblBarcas_005, e)
    'Case Keys.D7, Keys.NumPad7
    '    lblBarcas_Click(lblBarcas_006, e)
    'Case Keys.D8, Keys.NumPad8
    '    lblBarcas_Click(lblBarcas_007, e)
    'Case Keys.D9, Keys.NumPad9
    '    lblBarcas_Click(lblBarcas_008, e)
    'Case Keys.D0, Keys.NumLock
    '    lblBarcas_Click(lblBarcas_009, e)
    'Case Keys.OemOpenBrackets, Keys.Divide
    '    lblBarcas_Click(lblBarcas_010, e)
    'Case Keys.OemCloseBrackets, Keys.Multiply
    '    lblBarcas_Click(lblBarcas_011, e)
    'Case Keys.Back, Keys.Subtract
    '    lblBarcas_Click(lblBarcas_012, e)
    'Case Keys.Insert
    '   lblBarcas_Click(lblBarcas_013, e)
    'Case Keys.Home
    ''   lblBarcas_Click(lblBarcas_014, e)
    'Case Keys.Up
    '  lblBarcas_Click(lblBarcas_015, e)
    ' Case Keys.Delete
    '    lblBarcas_Click(lblBarcas_016, e)
    ' Case Keys.End, Keys.NumPad2
    '      lblBarcas_Click(lblBarcas_017, e)
    '   Case Keys.Down, Keys.NumPad1
    '        lblBarcas_Click(lblBarcas_018, e)
    '     Case Keys.OemPipe, Keys.Decimal
    '          lblBarcas_Click(lblBarcas_019, e)
    '   End Select
    'End Sub

    Private Sub AcercaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcercaToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub

    Private Sub GruposToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GruposToolStripMenuItem1.Click
        fEstadisticaGrupo.Show()
    End Sub

    Private Sub GuíasToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuíasToolStripMenuItem1.Click
        fEstadisticaGuia.Show()
    End Sub

    Private Sub AñadirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AñadirToolStripMenuItem.Click
        fAñadirGrupo.Show()
    End Sub

    Private Sub EliminarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarToolStripMenuItem.Click
        fEliminarGrupo.Show()
    End Sub

    Private Sub AñadirToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AñadirToolStripMenuItem1.Click
        fAñadirGuia.Show()
    End Sub

    Private Sub EliminarToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarToolStripMenuItem1.Click
        fEliminarGuia.Show()
    End Sub

    Private Sub ResumenDíaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResumenDíaToolStripMenuItem.Click
        Resumen.Show()
    End Sub

    Private Sub ContabilizarGrupoConFacturaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContabilizarGrupoConFacturaToolStripMenuItem.Click
        GrupoConFactura.Show()
    End Sub

    Private Sub VerTodosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerTodosToolStripMenuItem.Click
        VerGrupos.Show()
    End Sub

    Private Sub GruposToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GruposToolStripMenuItem2.Click
        GruposMes.Show()
    End Sub

    Private Sub GruposToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GruposToolStripMenuItem3.Click
        GruposDia.Show()
    End Sub

    Private Sub LosBarkitosToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LosBarkitosToolStripMenuItem2.Click
        AcotadoBarkitos.Show()
    End Sub

    Private Sub ExtraToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraToolStripMenuItem.Click
        Extra.Show()
    End Sub

    Private Sub ExtraToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraToolStripMenuItem1.Click
        ExtraP.Show()
    End Sub

    Private Sub FotosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FotosToolStripMenuItem.Click
        Fotos.Show()
    End Sub

    Private Sub lblDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDescuento.Click
        ImprimirOferta(1)
    End Sub

    Private Sub CopiaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopiaToolStripMenuItem.Click
        Seguridad.Show()
    End Sub

    Private Sub MarinaFerryToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarinaFerryToolStripMenuItem2.Click
        AcotadoMarinaFerry.Show()
    End Sub

    Private Sub lblGastos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGastos.Click
        frmGastos.Show()
    End Sub

    Private Sub CambioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CambioToolStripMenuItem.Click
        frmCambio.Show()
    End Sub

    Private Sub AcotadoToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcotadoToolStripMenuItem1.Click
        Totales.Show()
    End Sub

    Private Sub ImpresiónRápidaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImpresiónRápidaToolStripMenuItem.Click
        dlgBkt.Show()
    End Sub

    Private Sub lblConexion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblConexion.Click
        ConexionBDD()
        If cnt.State = ConnectionState.Open Then
            CargaImagenesBotones()
            TraspasarDatosSinConexion()
        End If

    End Sub

    Private Sub lblCtrlBkt_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblCtrlBkt.MouseEnter
        grpCont.Visible = True
        lblTotalEuros.Visible = True
    End Sub

    Private Sub lblCtrlBkt_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblCtrlBkt.MouseLeave
        grpCont.Visible = False
        lblTotalEuros.Visible = False
    End Sub

    Private Sub ColocarPorcentaje()
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "select NumNegro from  Contadores"
        txtPorcentaje.Text = CStr(CInt(cmd.ExecuteScalar))
    End Sub

    Private Sub btnPorcentaje_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPorcentaje.Click
        Dim cmd As SqlCommand
        cmd = New SqlCommand
        cmd.Connection = cnt
        cmd.CommandText = "update Contadores set NumNegro = " & txtPorcentaje.Text
        Try
            cmd.ExecuteNonQuery()
            Beep()
        Catch
            MsgBox("No se ha podido hacer el cambio")
        End Try
    End Sub

    Private Sub CorteImpresora()
        Dim int As Integer = FreeFile()
        FileOpen(1, "c:\cajon", OpenMode.Output)
        PrintLine(1, Chr(27) & Chr(109) & Chr(27) & Chr(112) & Chr(1) & Chr(60) & Chr(240))
        FileClose(1)
        Shell("print /d:USB001 c:\cajon", vbNormalFocus)
    End Sub

    Private Sub AbrirCaja()
        Dim i As Integer = FreeFile()
        FileOpen(1, "c:\cajon", OpenMode.Output)
        PrintLine(1, Chr(27) & "p" & Chr(0) & Chr(25) & Chr(250))
        FileClose(1)
        Shell("print /d:lpt1 c:\cajon", vbNormalFocus)
    End Sub

    Private Sub AbrirCajaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AbrirCajaToolStripMenuItem.Click
        AbrirCaja()
    End Sub


End Class
