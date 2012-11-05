<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AcotadoMarinaFerry
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.pnlControles = New System.Windows.Forms.Panel
        Me.lblNumViajes = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpFinal = New System.Windows.Forms.DateTimePicker
        Me.dtpInicio = New System.Windows.Forms.DateTimePicker
        Me.lblImprimir = New System.Windows.Forms.Label
        Me.lblCerrar = New System.Windows.Forms.Label
        Me.lblTotalMes = New System.Windows.Forms.Label
        Me.lblViajesMes = New System.Windows.Forms.Label
        Me.DG = New System.Windows.Forms.DataGridView
        Me.pnlControles.SuspendLayout()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlControles
        '
        Me.pnlControles.Controls.Add(Me.lblNumViajes)
        Me.pnlControles.Controls.Add(Me.lblTotal)
        Me.pnlControles.Controls.Add(Me.Label2)
        Me.pnlControles.Controls.Add(Me.Label1)
        Me.pnlControles.Controls.Add(Me.dtpFinal)
        Me.pnlControles.Controls.Add(Me.dtpInicio)
        Me.pnlControles.Controls.Add(Me.lblImprimir)
        Me.pnlControles.Controls.Add(Me.lblCerrar)
        Me.pnlControles.Controls.Add(Me.lblTotalMes)
        Me.pnlControles.Controls.Add(Me.lblViajesMes)
        Me.pnlControles.Location = New System.Drawing.Point(604, 33)
        Me.pnlControles.Name = "pnlControles"
        Me.pnlControles.Size = New System.Drawing.Size(329, 295)
        Me.pnlControles.TabIndex = 9
        '
        'lblNumViajes
        '
        Me.lblNumViajes.AutoSize = True
        Me.lblNumViajes.Location = New System.Drawing.Point(46, 152)
        Me.lblNumViajes.Name = "lblNumViajes"
        Me.lblNumViajes.Size = New System.Drawing.Size(39, 13)
        Me.lblNumViajes.TabIndex = 12
        Me.lblNumViajes.Text = "Label3"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(185, 150)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(39, 13)
        Me.lblTotal.TabIndex = 11
        Me.lblTotal.Text = "Label3"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(79, 98)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Fecha final:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(79, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Fecha Inicial:"
        '
        'dtpFinal
        '
        Me.dtpFinal.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFinal.Location = New System.Drawing.Point(177, 98)
        Me.dtpFinal.Name = "dtpFinal"
        Me.dtpFinal.Size = New System.Drawing.Size(101, 20)
        Me.dtpFinal.TabIndex = 8
        '
        'dtpInicio
        '
        Me.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpInicio.Location = New System.Drawing.Point(177, 41)
        Me.dtpInicio.Name = "dtpInicio"
        Me.dtpInicio.Size = New System.Drawing.Size(101, 20)
        Me.dtpInicio.TabIndex = 7
        '
        'lblImprimir
        '
        Me.lblImprimir.BackColor = System.Drawing.Color.White
        Me.lblImprimir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblImprimir.Location = New System.Drawing.Point(172, 187)
        Me.lblImprimir.Name = "lblImprimir"
        Me.lblImprimir.Size = New System.Drawing.Size(106, 72)
        Me.lblImprimir.TabIndex = 6
        Me.lblImprimir.Text = "Imprimir"
        Me.lblImprimir.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCerrar
        '
        Me.lblCerrar.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblCerrar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCerrar.ForeColor = System.Drawing.Color.Black
        Me.lblCerrar.Location = New System.Drawing.Point(50, 188)
        Me.lblCerrar.Name = "lblCerrar"
        Me.lblCerrar.Size = New System.Drawing.Size(88, 71)
        Me.lblCerrar.TabIndex = 5
        Me.lblCerrar.Text = "Cerrar"
        Me.lblCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTotalMes
        '
        Me.lblTotalMes.AutoSize = True
        Me.lblTotalMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalMes.Location = New System.Drawing.Point(167, 141)
        Me.lblTotalMes.Name = "lblTotalMes"
        Me.lblTotalMes.Size = New System.Drawing.Size(12, 26)
        Me.lblTotalMes.TabIndex = 4
        Me.lblTotalMes.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblViajesMes
        '
        Me.lblViajesMes.AutoSize = True
        Me.lblViajesMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblViajesMes.Location = New System.Drawing.Point(1, 141)
        Me.lblViajesMes.Name = "lblViajesMes"
        Me.lblViajesMes.Size = New System.Drawing.Size(12, 26)
        Me.lblViajesMes.TabIndex = 3
        Me.lblViajesMes.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'DG
        '
        Me.DG.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DG.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DG.DefaultCellStyle = DataGridViewCellStyle2
        Me.DG.Location = New System.Drawing.Point(12, 33)
        Me.DG.Name = "DG"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DG.Size = New System.Drawing.Size(550, 482)
        Me.DG.TabIndex = 8
        '
        'AcotadoMarinaFerry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 562)
        Me.Controls.Add(Me.pnlControles)
        Me.Controls.Add(Me.DG)
        Me.Name = "AcotadoMarinaFerry"
        Me.Text = "AcotadoMarinaFerry"
        Me.pnlControles.ResumeLayout(False)
        Me.pnlControles.PerformLayout()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlControles As System.Windows.Forms.Panel
    Friend WithEvents lblNumViajes As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpFinal As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpInicio As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblImprimir As System.Windows.Forms.Label
    Friend WithEvents lblCerrar As System.Windows.Forms.Label
    Friend WithEvents lblTotalMes As System.Windows.Forms.Label
    Friend WithEvents lblViajesMes As System.Windows.Forms.Label
    Friend WithEvents DG As System.Windows.Forms.DataGridView
End Class
