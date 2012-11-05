<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ModificacionTicketBkt
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNumFact = New System.Windows.Forms.TextBox()
        Me.lblfecha = New System.Windows.Forms.Label()
        Me.txtFecha = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPuntoVenta = New System.Windows.Forms.TextBox()
        Me.lblBarca = New System.Windows.Forms.Label()
        Me.txtPrecio = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtAdultos = New System.Windows.Forms.TextBox()
        Me.txtNiños = New System.Windows.Forms.TextBox()
        Me.cmbTipoBarca = New System.Windows.Forms.ComboBox()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(196, 277)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancelar"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Aceptar"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(56, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Factura:"
        '
        'txtNumFact
        '
        Me.txtNumFact.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtNumFact.Location = New System.Drawing.Point(112, 12)
        Me.txtNumFact.Name = "txtNumFact"
        Me.txtNumFact.ReadOnly = True
        Me.txtNumFact.Size = New System.Drawing.Size(69, 20)
        Me.txtNumFact.TabIndex = 2
        '
        'lblfecha
        '
        Me.lblfecha.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblfecha.AutoSize = True
        Me.lblfecha.Location = New System.Drawing.Point(109, 55)
        Me.lblfecha.Name = "lblfecha"
        Me.lblfecha.Size = New System.Drawing.Size(40, 13)
        Me.lblfecha.TabIndex = 3
        Me.lblfecha.Text = "Fecha:"
        '
        'txtFecha
        '
        Me.txtFecha.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtFecha.Location = New System.Drawing.Point(180, 48)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.ReadOnly = True
        Me.txtFecha.Size = New System.Drawing.Size(69, 20)
        Me.txtFecha.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(109, 131)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Adultos:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(109, 168)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Niños:"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(109, 206)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Punto Venta:"
        '
        'txtPuntoVenta
        '
        Me.txtPuntoVenta.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtPuntoVenta.Location = New System.Drawing.Point(180, 199)
        Me.txtPuntoVenta.Name = "txtPuntoVenta"
        Me.txtPuntoVenta.ReadOnly = True
        Me.txtPuntoVenta.Size = New System.Drawing.Size(100, 20)
        Me.txtPuntoVenta.TabIndex = 10
        '
        'lblBarca
        '
        Me.lblBarca.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblBarca.AutoSize = True
        Me.lblBarca.Location = New System.Drawing.Point(109, 244)
        Me.lblBarca.Name = "lblBarca"
        Me.lblBarca.Size = New System.Drawing.Size(38, 13)
        Me.lblBarca.TabIndex = 11
        Me.lblBarca.Text = "Barca:"
        '
        'txtPrecio
        '
        Me.txtPrecio.Location = New System.Drawing.Point(180, 86)
        Me.txtPrecio.Name = "txtPrecio"
        Me.txtPrecio.Size = New System.Drawing.Size(69, 20)
        Me.txtPrecio.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(109, 93)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Precio:"
        '
        'txtAdultos
        '
        Me.txtAdultos.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtAdultos.Location = New System.Drawing.Point(180, 124)
        Me.txtAdultos.Name = "txtAdultos"
        Me.txtAdultos.Size = New System.Drawing.Size(32, 20)
        Me.txtAdultos.TabIndex = 6
        '
        'txtNiños
        '
        Me.txtNiños.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtNiños.Location = New System.Drawing.Point(180, 161)
        Me.txtNiños.Name = "txtNiños"
        Me.txtNiños.Size = New System.Drawing.Size(32, 20)
        Me.txtNiños.TabIndex = 8
        '
        'cmbTipoBarca
        '
        Me.cmbTipoBarca.FormattingEnabled = True
        Me.cmbTipoBarca.Items.AddRange(New Object() {"Rio", "Solar", "Gold"})
        Me.cmbTipoBarca.Location = New System.Drawing.Point(180, 236)
        Me.cmbTipoBarca.Name = "cmbTipoBarca"
        Me.cmbTipoBarca.Size = New System.Drawing.Size(100, 21)
        Me.cmbTipoBarca.TabIndex = 15
        '
        'btnImprimir
        '
        Me.btnImprimir.Location = New System.Drawing.Point(12, 280)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(75, 23)
        Me.btnImprimir.TabIndex = 16
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Location = New System.Drawing.Point(94, 280)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(75, 23)
        Me.btnEliminar.TabIndex = 17
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'ModificacionTicketBkt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(354, 318)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.cmbTipoBarca)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtPrecio)
        Me.Controls.Add(Me.lblBarca)
        Me.Controls.Add(Me.txtPuntoVenta)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtNiños)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtAdultos)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtFecha)
        Me.Controls.Add(Me.lblfecha)
        Me.Controls.Add(Me.txtNumFact)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ModificacionTicketBkt"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Modificar Ticket de Barkitos"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNumFact As System.Windows.Forms.TextBox
    Friend WithEvents lblfecha As System.Windows.Forms.Label
    Friend WithEvents txtFecha As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPuntoVenta As System.Windows.Forms.TextBox
    Friend WithEvents lblBarca As System.Windows.Forms.Label
    Friend WithEvents txtPrecio As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtAdultos As System.Windows.Forms.TextBox
    Friend WithEvents txtNiños As System.Windows.Forms.TextBox
    Friend WithEvents cmbTipoBarca As System.Windows.Forms.ComboBox
    Friend WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents btnEliminar As System.Windows.Forms.Button

End Class
