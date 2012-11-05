<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fDiario
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DG = New System.Windows.Forms.DataGridView()
        Me.lblFechaHoy = New System.Windows.Forms.Label()
        Me.lblDiarioCerrar = New System.Windows.Forms.Label()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblNumViajes = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblImprimir = New System.Windows.Forms.Label()
        Me.pnlControles = New System.Windows.Forms.Panel()
        Me.rbBarkitos = New System.Windows.Forms.RadioButton()
        Me.rbMarina = New System.Windows.Forms.RadioButton()
        Me.rbTotal = New System.Windows.Forms.RadioButton()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.pnlControles.SuspendLayout()
        Me.SuspendLayout()
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
        Me.DG.ColumnHeadersHeight = 30
        Me.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DG.DefaultCellStyle = DataGridViewCellStyle2
        Me.DG.Location = New System.Drawing.Point(38, 32)
        Me.DG.Name = "DG"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DG.RowHeadersWidth = 75
        Me.DG.Size = New System.Drawing.Size(550, 399)
        Me.DG.TabIndex = 0
        '
        'lblFechaHoy
        '
        Me.lblFechaHoy.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblFechaHoy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFechaHoy.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFechaHoy.Location = New System.Drawing.Point(14, 21)
        Me.lblFechaHoy.Name = "lblFechaHoy"
        Me.lblFechaHoy.Size = New System.Drawing.Size(94, 81)
        Me.lblFechaHoy.TabIndex = 1
        Me.lblFechaHoy.Text = "Hoy"
        Me.lblFechaHoy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDiarioCerrar
        '
        Me.lblDiarioCerrar.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblDiarioCerrar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDiarioCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiarioCerrar.Location = New System.Drawing.Point(14, 268)
        Me.lblDiarioCerrar.Name = "lblDiarioCerrar"
        Me.lblDiarioCerrar.Size = New System.Drawing.Size(94, 81)
        Me.lblDiarioCerrar.TabIndex = 2
        Me.lblDiarioCerrar.Text = "Cerrar"
        Me.lblDiarioCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(11, 31)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(93, 20)
        Me.dtpFecha.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtpFecha)
        Me.GroupBox1.Location = New System.Drawing.Point(129, 21)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(114, 81)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Días Anteriores"
        '
        'lblNumViajes
        '
        Me.lblNumViajes.AutoSize = True
        Me.lblNumViajes.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumViajes.Location = New System.Drawing.Point(14, 187)
        Me.lblNumViajes.Name = "lblNumViajes"
        Me.lblNumViajes.Size = New System.Drawing.Size(35, 26)
        Me.lblNumViajes.TabIndex = 9
        Me.lblNumViajes.Text = "nv"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(14, 152)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(30, 26)
        Me.lblTotal.TabIndex = 10
        Me.lblTotal.Text = "to"
        '
        'lblImprimir
        '
        Me.lblImprimir.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.lblImprimir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblImprimir.Location = New System.Drawing.Point(171, 268)
        Me.lblImprimir.Name = "lblImprimir"
        Me.lblImprimir.Size = New System.Drawing.Size(137, 81)
        Me.lblImprimir.TabIndex = 11
        Me.lblImprimir.Text = "Imprimir"
        Me.lblImprimir.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlControles
        '
        Me.pnlControles.Controls.Add(Me.rbBarkitos)
        Me.pnlControles.Controls.Add(Me.rbMarina)
        Me.pnlControles.Controls.Add(Me.rbTotal)
        Me.pnlControles.Controls.Add(Me.lblImprimir)
        Me.pnlControles.Controls.Add(Me.lblTotal)
        Me.pnlControles.Controls.Add(Me.lblNumViajes)
        Me.pnlControles.Controls.Add(Me.GroupBox1)
        Me.pnlControles.Controls.Add(Me.lblDiarioCerrar)
        Me.pnlControles.Controls.Add(Me.lblFechaHoy)
        Me.pnlControles.Location = New System.Drawing.Point(598, 40)
        Me.pnlControles.Name = "pnlControles"
        Me.pnlControles.Size = New System.Drawing.Size(364, 375)
        Me.pnlControles.TabIndex = 12
        '
        'rbBarkitos
        '
        Me.rbBarkitos.AutoSize = True
        Me.rbBarkitos.Location = New System.Drawing.Point(264, 67)
        Me.rbBarkitos.Name = "rbBarkitos"
        Me.rbBarkitos.Size = New System.Drawing.Size(83, 17)
        Me.rbBarkitos.TabIndex = 14
        Me.rbBarkitos.Text = "Los Barkitos"
        Me.rbBarkitos.UseVisualStyleBackColor = True
        '
        'rbMarina
        '
        Me.rbMarina.AutoSize = True
        Me.rbMarina.Location = New System.Drawing.Point(264, 44)
        Me.rbMarina.Name = "rbMarina"
        Me.rbMarina.Size = New System.Drawing.Size(83, 17)
        Me.rbMarina.TabIndex = 13
        Me.rbMarina.Text = "Marina Ferry"
        Me.rbMarina.UseVisualStyleBackColor = True
        '
        'rbTotal
        '
        Me.rbTotal.AutoSize = True
        Me.rbTotal.Checked = True
        Me.rbTotal.Location = New System.Drawing.Point(264, 21)
        Me.rbTotal.Name = "rbTotal"
        Me.rbTotal.Size = New System.Drawing.Size(49, 17)
        Me.rbTotal.TabIndex = 12
        Me.rbTotal.TabStop = True
        Me.rbTotal.Text = "Total"
        Me.rbTotal.UseVisualStyleBackColor = True
        '
        'PrintDocument1
        '
        '
        'fDiario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(974, 505)
        Me.Controls.Add(Me.pnlControles)
        Me.Controls.Add(Me.DG)
        Me.Name = "fDiario"
        Me.Text = "Diario"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlControles.ResumeLayout(False)
        Me.pnlControles.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DG As System.Windows.Forms.DataGridView
    Friend WithEvents lblFechaHoy As System.Windows.Forms.Label
    Friend WithEvents lblDiarioCerrar As System.Windows.Forms.Label
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblNumViajes As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblImprimir As System.Windows.Forms.Label
    Friend WithEvents pnlControles As System.Windows.Forms.Panel
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents rbBarkitos As System.Windows.Forms.RadioButton
    Friend WithEvents rbMarina As System.Windows.Forms.RadioButton
    Friend WithEvents rbTotal As System.Windows.Forms.RadioButton
End Class
