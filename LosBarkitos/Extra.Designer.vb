<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Extra
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
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker
        Me.txtPaxGrupo = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPrecio = New System.Windows.Forms.TextBox
        Me.btbBlanco = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbGrupo = New System.Windows.Forms.ComboBox
        Me.cmbGuia = New System.Windows.Forms.ComboBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnNegro = New System.Windows.Forms.Button
        Me.btnParticular = New System.Windows.Forms.Button
        Me.lblNum = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(12, 12)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(84, 20)
        Me.dtpFecha.TabIndex = 0
        '
        'txtPaxGrupo
        '
        Me.txtPaxGrupo.Location = New System.Drawing.Point(117, 90)
        Me.txtPaxGrupo.Name = "txtPaxGrupo"
        Me.txtPaxGrupo.Size = New System.Drawing.Size(59, 20)
        Me.txtPaxGrupo.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 97)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Num. Pasaj. Grupo:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(72, 142)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Precio:"
        '
        'txtPrecio
        '
        Me.txtPrecio.Location = New System.Drawing.Point(118, 135)
        Me.txtPrecio.Name = "txtPrecio"
        Me.txtPrecio.Size = New System.Drawing.Size(58, 20)
        Me.txtPrecio.TabIndex = 4
        '
        'btbBlanco
        '
        Me.btbBlanco.Location = New System.Drawing.Point(15, 220)
        Me.btbBlanco.Name = "btbBlanco"
        Me.btbBlanco.Size = New System.Drawing.Size(75, 23)
        Me.btbBlanco.TabIndex = 5
        Me.btbBlanco.Text = "Blanco"
        Me.btbBlanco.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(253, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Grupo:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(253, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Guía:"
        '
        'cmbGrupo
        '
        Me.cmbGrupo.FormattingEnabled = True
        Me.cmbGrupo.Location = New System.Drawing.Point(298, 89)
        Me.cmbGrupo.Name = "cmbGrupo"
        Me.cmbGrupo.Size = New System.Drawing.Size(121, 21)
        Me.cmbGrupo.TabIndex = 8
        '
        'cmbGuia
        '
        Me.cmbGuia.FormattingEnabled = True
        Me.cmbGuia.Location = New System.Drawing.Point(298, 116)
        Me.cmbGuia.Name = "cmbGuia"
        Me.cmbGuia.Size = New System.Drawing.Size(121, 21)
        Me.cmbGuia.TabIndex = 9
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(298, 220)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(121, 23)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Contabilizar Grupo"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnNegro
        '
        Me.btnNegro.Location = New System.Drawing.Point(101, 220)
        Me.btnNegro.Name = "btnNegro"
        Me.btnNegro.Size = New System.Drawing.Size(75, 23)
        Me.btnNegro.TabIndex = 11
        Me.btnNegro.Text = "Negro"
        Me.btnNegro.UseVisualStyleBackColor = True
        '
        'btnParticular
        '
        Me.btnParticular.Location = New System.Drawing.Point(61, 268)
        Me.btnParticular.Name = "btnParticular"
        Me.btnParticular.Size = New System.Drawing.Size(75, 23)
        Me.btnParticular.TabIndex = 12
        Me.btnParticular.Text = "Particular"
        Me.btnParticular.UseVisualStyleBackColor = True
        '
        'lblNum
        '
        Me.lblNum.AutoSize = True
        Me.lblNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNum.Location = New System.Drawing.Point(335, 319)
        Me.lblNum.Name = "lblNum"
        Me.lblNum.Size = New System.Drawing.Size(45, 13)
        Me.lblNum.TabIndex = 13
        Me.lblNum.Text = "Label5"
        '
        'Extra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(462, 374)
        Me.Controls.Add(Me.lblNum)
        Me.Controls.Add(Me.btnParticular)
        Me.Controls.Add(Me.btnNegro)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cmbGuia)
        Me.Controls.Add(Me.cmbGrupo)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btbBlanco)
        Me.Controls.Add(Me.txtPrecio)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtPaxGrupo)
        Me.Controls.Add(Me.dtpFecha)
        Me.Name = "Extra"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Extra Marina Ferry"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtPaxGrupo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPrecio As System.Windows.Forms.TextBox
    Friend WithEvents btbBlanco As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbGrupo As System.Windows.Forms.ComboBox
    Friend WithEvents cmbGuia As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnNegro As System.Windows.Forms.Button
    Friend WithEvents btnParticular As System.Windows.Forms.Button
    Friend WithEvents lblNum As System.Windows.Forms.Label
End Class
