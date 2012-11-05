<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fEstadisticaGuia
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.pnlControles = New System.Windows.Forms.Panel
        Me.lblImprimir = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblComision = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbGuia = New System.Windows.Forms.ComboBox
        Me.lblDiarioCerrar = New System.Windows.Forms.Label
        Me.DG = New System.Windows.Forms.DataGridView
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument
        Me.pnlControles.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlControles
        '
        Me.pnlControles.Controls.Add(Me.lblImprimir)
        Me.pnlControles.Controls.Add(Me.lblTotal)
        Me.pnlControles.Controls.Add(Me.lblComision)
        Me.pnlControles.Controls.Add(Me.GroupBox1)
        Me.pnlControles.Controls.Add(Me.lblDiarioCerrar)
        Me.pnlControles.Location = New System.Drawing.Point(580, 50)
        Me.pnlControles.Name = "pnlControles"
        Me.pnlControles.Size = New System.Drawing.Size(364, 376)
        Me.pnlControles.TabIndex = 16
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
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(15, 202)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(0, 26)
        Me.lblTotal.TabIndex = 10
        '
        'lblComision
        '
        Me.lblComision.AutoSize = True
        Me.lblComision.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComision.Location = New System.Drawing.Point(9, 234)
        Me.lblComision.Name = "lblComision"
        Me.lblComision.Size = New System.Drawing.Size(0, 26)
        Me.lblComision.TabIndex = 9
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbGuia)
        Me.GroupBox1.Location = New System.Drawing.Point(171, 21)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(137, 81)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Guías"
        '
        'cmbGuia
        '
        Me.cmbGuia.FormattingEnabled = True
        Me.cmbGuia.Location = New System.Drawing.Point(10, 29)
        Me.cmbGuia.Name = "cmbGuia"
        Me.cmbGuia.Size = New System.Drawing.Size(117, 21)
        Me.cmbGuia.TabIndex = 0
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
        Me.DG.Location = New System.Drawing.Point(12, 12)
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
        Me.DG.Size = New System.Drawing.Size(552, 556)
        Me.DG.TabIndex = 15
        '
        'PrintDocument1
        '
        '
        'fEstadisticaGuia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(980, 632)
        Me.Controls.Add(Me.pnlControles)
        Me.Controls.Add(Me.DG)
        Me.Name = "fEstadisticaGuia"
        Me.Text = "Estadísticas del Guía"
        Me.pnlControles.ResumeLayout(False)
        Me.pnlControles.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlControles As System.Windows.Forms.Panel
    Friend WithEvents lblImprimir As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblComision As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbGuia As System.Windows.Forms.ComboBox
    Friend WithEvents lblDiarioCerrar As System.Windows.Forms.Label
    Friend WithEvents DG As System.Windows.Forms.DataGridView
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
End Class
