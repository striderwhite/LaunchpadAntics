namespace MIDI_App
{
  partial class FormMain
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.buttonToggleWalkthru = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.buttonReset = new System.Windows.Forms.Button();
      this.labelStatus = new System.Windows.Forms.Label();
      this.dataGridViewConsole = new System.Windows.Forms.DataGridView();
      this.ColumnMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConsole)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonToggleWalkthru
      // 
      this.buttonToggleWalkthru.Location = new System.Drawing.Point(6, 19);
      this.buttonToggleWalkthru.Name = "buttonToggleWalkthru";
      this.buttonToggleWalkthru.Size = new System.Drawing.Size(75, 23);
      this.buttonToggleWalkthru.TabIndex = 0;
      this.buttonToggleWalkthru.Text = "Map Buttons";
      this.buttonToggleWalkthru.UseVisualStyleBackColor = true;
      this.buttonToggleWalkthru.Click += new System.EventHandler(this.buttonToggleWalkthru_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.dataGridViewConsole);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(381, 290);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Console Messages";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.button2);
      this.groupBox2.Controls.Add(this.button1);
      this.groupBox2.Controls.Add(this.buttonReset);
      this.groupBox2.Controls.Add(this.buttonToggleWalkthru);
      this.groupBox2.Location = new System.Drawing.Point(12, 308);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(375, 54);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Button mapping";
      // 
      // buttonReset
      // 
      this.buttonReset.Location = new System.Drawing.Point(87, 19);
      this.buttonReset.Name = "buttonReset";
      this.buttonReset.Size = new System.Drawing.Size(75, 23);
      this.buttonReset.TabIndex = 1;
      this.buttonReset.Text = "Reset";
      this.buttonReset.UseVisualStyleBackColor = true;
      // 
      // labelStatus
      // 
      this.labelStatus.AutoSize = true;
      this.labelStatus.Location = new System.Drawing.Point(12, 369);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new System.Drawing.Size(124, 13);
      this.labelStatus.TabIndex = 6;
      this.labelStatus.Text = "Waiting for Launchpad...";
      // 
      // dataGridViewConsole
      // 
      this.dataGridViewConsole.AllowUserToAddRows = false;
      this.dataGridViewConsole.AllowUserToDeleteRows = false;
      this.dataGridViewConsole.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridViewConsole.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.dataGridViewConsole.BackgroundColor = System.Drawing.SystemColors.Control;
      this.dataGridViewConsole.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
      this.dataGridViewConsole.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewConsole.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnMessage,
            this.ColumnTime});
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.InfoText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridViewConsole.DefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewConsole.GridColor = System.Drawing.Color.LightCoral;
      this.dataGridViewConsole.Location = new System.Drawing.Point(6, 25);
      this.dataGridViewConsole.Name = "dataGridViewConsole";
      this.dataGridViewConsole.ReadOnly = true;
      this.dataGridViewConsole.Size = new System.Drawing.Size(369, 265);
      this.dataGridViewConsole.TabIndex = 0;
      this.dataGridViewConsole.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewConsole_CellContentClick);
      // 
      // ColumnMessage
      // 
      this.ColumnMessage.HeaderText = "Message";
      this.ColumnMessage.Name = "ColumnMessage";
      this.ColumnMessage.ReadOnly = true;
      // 
      // ColumnTime
      // 
      this.ColumnTime.HeaderText = "Time";
      this.ColumnTime.Name = "ColumnTime";
      this.ColumnTime.ReadOnly = true;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(213, 19);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Save";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(294, 19);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Load";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(403, 393);
      this.Controls.Add(this.labelStatus);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "FormMain";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Launchpad ";
      this.Load += new System.EventHandler(this.FormMain_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConsole)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonToggleWalkthru;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button buttonReset;
    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.DataGridView dataGridViewConsole;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMessage;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
  }
}

