namespace QuanLySSCenter
{
    partial class module14
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(module14));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.btnThongke = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDenngay = new System.Windows.Forms.DateTimePicker();
            this.dtpTungay = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnexit = new Guna.UI2.WinForms.Guna2Button();
            this.chartKhachHang = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartKhachHang)).BeginInit();
            this.SuspendLayout();
            // 
            // btnThongke
            // 
            this.btnThongke.BorderRadius = 18;
            this.btnThongke.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThongke.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThongke.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThongke.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThongke.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnThongke.ForeColor = System.Drawing.Color.White;
            this.btnThongke.Image = ((System.Drawing.Image)(resources.GetObject("btnThongke.Image")));
            this.btnThongke.ImageSize = new System.Drawing.Size(17, 17);
            this.btnThongke.Location = new System.Drawing.Point(990, 42);
            this.btnThongke.Name = "btnThongke";
            this.btnThongke.Size = new System.Drawing.Size(166, 45);
            this.btnThongke.TabIndex = 29;
            this.btnThongke.Text = "Thống kê";
            this.btnThongke.Click += new System.EventHandler(this.btnThongke_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Location = new System.Drawing.Point(523, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 31);
            this.label1.TabIndex = 28;
            this.label1.Text = "Đến ngày";
            // 
            // dtpDenngay
            // 
            this.dtpDenngay.Location = new System.Drawing.Point(687, 51);
            this.dtpDenngay.Name = "dtpDenngay";
            this.dtpDenngay.Size = new System.Drawing.Size(228, 22);
            this.dtpDenngay.TabIndex = 27;
            // 
            // dtpTungay
            // 
            this.dtpTungay.Location = new System.Drawing.Point(248, 51);
            this.dtpTungay.Name = "dtpTungay";
            this.dtpTungay.Size = new System.Drawing.Size(227, 22);
            this.dtpTungay.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label2.Location = new System.Drawing.Point(112, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 31);
            this.label2.TabIndex = 25;
            this.label2.Text = "Từ ngày";
            // 
            // btnexit
            // 
            this.btnexit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnexit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnexit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnexit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnexit.FillColor = System.Drawing.Color.Transparent;
            this.btnexit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnexit.ForeColor = System.Drawing.Color.White;
            this.btnexit.Image = ((System.Drawing.Image)(resources.GetObject("btnexit.Image")));
            this.btnexit.ImageSize = new System.Drawing.Size(70, 70);
            this.btnexit.Location = new System.Drawing.Point(12, 30);
            this.btnexit.Name = "btnexit";
            this.btnexit.Size = new System.Drawing.Size(94, 45);
            this.btnexit.TabIndex = 36;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // chartKhachHang
            // 
            chartArea1.Name = "ChartArea1";
            this.chartKhachHang.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartKhachHang.Legends.Add(legend1);
            this.chartKhachHang.Location = new System.Drawing.Point(107, 150);
            this.chartKhachHang.Name = "chartKhachHang";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.CustomProperties = "PieLabelStyle=Outside";
            series1.IsXValueIndexed = true;
            series1.LabelFormat = "\"0.00\'%\'\"";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartKhachHang.Series.Add(series1);
            this.chartKhachHang.Size = new System.Drawing.Size(943, 508);
            this.chartKhachHang.TabIndex = 37;
            this.chartKhachHang.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Tỉ lệ khách hàng ";
            title2.Name = "Title2";
            this.chartKhachHang.Titles.Add(title1);
            this.chartKhachHang.Titles.Add(title2);
            // 
            // module14
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1231, 699);
            this.Controls.Add(this.chartKhachHang);
            this.Controls.Add(this.btnexit);
            this.Controls.Add(this.btnThongke);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpDenngay);
            this.Controls.Add(this.dtpTungay);
            this.Controls.Add(this.label2);
            this.Name = "module14";
            this.Text = "Phân loại khách hàng";
            ((System.ComponentModel.ISupportInitialize)(this.chartKhachHang)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnThongke;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDenngay;
        private System.Windows.Forms.DateTimePicker dtpTungay;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button btnexit;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKhachHang;
    }
}