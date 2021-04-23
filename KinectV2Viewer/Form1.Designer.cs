namespace KinectV2Viewer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ClearPointCloud = new System.Windows.Forms.Button();
            this.buttonAABB = new System.Windows.Forms.Button();
            this.buttonRemovingOutliers = new System.Windows.Forms.Button();
            this.buttonSavePLY = new System.Windows.Forms.Button();
            this.Cut = new System.Windows.Forms.Button();
            this.OrientedBoundingBox = new System.Windows.Forms.Button();
            this.ClusterExtraction = new System.Windows.Forms.Button();
            this.buttonOpenPly = new System.Windows.Forms.Button();
            this.buttonTestPlaneDetection = new System.Windows.Forms.Button();
            this.buttonCapturePointCloud = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBoxReceived = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonGui = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNhapDuLieu = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Home = new System.Windows.Forms.Button();
            this.XAxis = new System.Windows.Forms.Label();
            this.E_Up = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.ServoOn = new System.Windows.Forms.Button();
            this.Y_Down = new System.Windows.Forms.Button();
            this.Record = new System.Windows.Forms.Button();
            this.Z_Down = new System.Windows.Forms.Button();
            this.ServoOff = new System.Windows.Forms.Button();
            this.Z_Up = new System.Windows.Forms.Button();
            this.X_Down = new System.Windows.Forms.Button();
            this.H_Down = new System.Windows.Forms.Button();
            this.H_Up = new System.Windows.Forms.Button();
            this.E_Down = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.Y_Up = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.X_Up = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonAutomatic = new System.Windows.Forms.Button();
            this.buttonInitialPort = new System.Windows.Forms.Button();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelPortStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Xax = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportXStep = new System.Windows.Forms.ToolStripMenuItem();
            this.Yax = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportYStep = new System.Windows.Forms.ToolStripMenuItem();
            this.Zax = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportZStep = new System.Windows.Forms.ToolStripMenuItem();
            this.Eax = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportEStep = new System.Windows.Forms.ToolStripMenuItem();
            this.Hax = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportHStep = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.AutoImage = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBoxDepth = new System.Windows.Forms.PictureBox();
            this.pictureBoxColor = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBoxPointCloud = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.buttonCapture = new System.Windows.Forms.Button();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPointCloud)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(4, 5);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(803, 670);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ClearPointCloud);
            this.tabPage3.Controls.Add(this.buttonAABB);
            this.tabPage3.Controls.Add(this.buttonRemovingOutliers);
            this.tabPage3.Controls.Add(this.buttonSavePLY);
            this.tabPage3.Controls.Add(this.Cut);
            this.tabPage3.Controls.Add(this.OrientedBoundingBox);
            this.tabPage3.Controls.Add(this.ClusterExtraction);
            this.tabPage3.Controls.Add(this.buttonOpenPly);
            this.tabPage3.Controls.Add(this.buttonTestPlaneDetection);
            this.tabPage3.Controls.Add(this.buttonCapturePointCloud);
            this.tabPage3.Controls.Add(this.buttonStop);
            this.tabPage3.Controls.Add(this.buttonStart);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(795, 641);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Image";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ClearPointCloud
            // 
            this.ClearPointCloud.Location = new System.Drawing.Point(399, 479);
            this.ClearPointCloud.Name = "ClearPointCloud";
            this.ClearPointCloud.Size = new System.Drawing.Size(154, 51);
            this.ClearPointCloud.TabIndex = 11;
            this.ClearPointCloud.Text = "ClearPCL";
            this.ClearPointCloud.UseVisualStyleBackColor = true;
            this.ClearPointCloud.Click += new System.EventHandler(this.ClearPointCloud_Click);
            // 
            // buttonAABB
            // 
            this.buttonAABB.Location = new System.Drawing.Point(212, 479);
            this.buttonAABB.Name = "buttonAABB";
            this.buttonAABB.Size = new System.Drawing.Size(156, 52);
            this.buttonAABB.TabIndex = 10;
            this.buttonAABB.Text = "AABB";
            this.buttonAABB.UseVisualStyleBackColor = true;
            this.buttonAABB.Click += new System.EventHandler(this.buttonAABB_Click);
            // 
            // buttonRemovingOutliers
            // 
            this.buttonRemovingOutliers.Location = new System.Drawing.Point(399, 406);
            this.buttonRemovingOutliers.Name = "buttonRemovingOutliers";
            this.buttonRemovingOutliers.Size = new System.Drawing.Size(155, 52);
            this.buttonRemovingOutliers.TabIndex = 9;
            this.buttonRemovingOutliers.Text = "RemovingOutliers";
            this.buttonRemovingOutliers.UseVisualStyleBackColor = true;
            this.buttonRemovingOutliers.Click += new System.EventHandler(this.buttonRemovingOutliers_Click);
            // 
            // buttonSavePLY
            // 
            this.buttonSavePLY.Location = new System.Drawing.Point(211, 406);
            this.buttonSavePLY.Name = "buttonSavePLY";
            this.buttonSavePLY.Size = new System.Drawing.Size(157, 53);
            this.buttonSavePLY.TabIndex = 8;
            this.buttonSavePLY.Text = "Save PLY";
            this.buttonSavePLY.UseVisualStyleBackColor = true;
            this.buttonSavePLY.Click += new System.EventHandler(this.buttonSavePLY_Click);
            // 
            // Cut
            // 
            this.Cut.Location = new System.Drawing.Point(399, 331);
            this.Cut.Name = "Cut";
            this.Cut.Size = new System.Drawing.Size(155, 52);
            this.Cut.TabIndex = 7;
            this.Cut.Text = "Cut";
            this.Cut.UseVisualStyleBackColor = true;
            this.Cut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // OrientedBoundingBox
            // 
            this.OrientedBoundingBox.Location = new System.Drawing.Point(211, 331);
            this.OrientedBoundingBox.Name = "OrientedBoundingBox";
            this.OrientedBoundingBox.Size = new System.Drawing.Size(157, 53);
            this.OrientedBoundingBox.TabIndex = 6;
            this.OrientedBoundingBox.Text = "OrientedBoundingBox";
            this.OrientedBoundingBox.UseVisualStyleBackColor = true;
            this.OrientedBoundingBox.Click += new System.EventHandler(this.OrientedBoundingBox_Click);
            // 
            // ClusterExtraction
            // 
            this.ClusterExtraction.Location = new System.Drawing.Point(399, 256);
            this.ClusterExtraction.Name = "ClusterExtraction";
            this.ClusterExtraction.Size = new System.Drawing.Size(155, 50);
            this.ClusterExtraction.TabIndex = 5;
            this.ClusterExtraction.Text = "ClusterExtraction";
            this.ClusterExtraction.UseVisualStyleBackColor = true;
            this.ClusterExtraction.Click += new System.EventHandler(this.ClusterExtraction_Click);
            // 
            // buttonOpenPly
            // 
            this.buttonOpenPly.Location = new System.Drawing.Point(211, 256);
            this.buttonOpenPly.Name = "buttonOpenPly";
            this.buttonOpenPly.Size = new System.Drawing.Size(157, 51);
            this.buttonOpenPly.TabIndex = 4;
            this.buttonOpenPly.Text = "Open Ply";
            this.buttonOpenPly.UseVisualStyleBackColor = true;
            this.buttonOpenPly.Click += new System.EventHandler(this.buttonOpenPly_Click);
            // 
            // buttonTestPlaneDetection
            // 
            this.buttonTestPlaneDetection.Location = new System.Drawing.Point(399, 175);
            this.buttonTestPlaneDetection.Name = "buttonTestPlaneDetection";
            this.buttonTestPlaneDetection.Size = new System.Drawing.Size(155, 54);
            this.buttonTestPlaneDetection.TabIndex = 3;
            this.buttonTestPlaneDetection.Text = "Test Plane Detection";
            this.buttonTestPlaneDetection.UseVisualStyleBackColor = true;
            this.buttonTestPlaneDetection.Click += new System.EventHandler(this.buttonTestPlaneDetection_Click);
            // 
            // buttonCapturePointCloud
            // 
            this.buttonCapturePointCloud.Location = new System.Drawing.Point(211, 175);
            this.buttonCapturePointCloud.Name = "buttonCapturePointCloud";
            this.buttonCapturePointCloud.Size = new System.Drawing.Size(157, 55);
            this.buttonCapturePointCloud.TabIndex = 2;
            this.buttonCapturePointCloud.Text = "Capture Point Cloud";
            this.buttonCapturePointCloud.UseVisualStyleBackColor = true;
            this.buttonCapturePointCloud.Click += new System.EventHandler(this.buttonCapturePointCloud_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(399, 93);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(155, 60);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(211, 93);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(157, 61);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.menuStrip1);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(795, 641);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Control";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(6, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(783, 601);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBoxReceived);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.buttonClear);
            this.groupBox4.Controls.Add(this.buttonGui);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.textBoxNhapDuLieu);
            this.groupBox4.Location = new System.Drawing.Point(425, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(352, 581);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Code";
            // 
            // listBoxReceived
            // 
            this.listBoxReceived.FormattingEnabled = true;
            this.listBoxReceived.ItemHeight = 16;
            this.listBoxReceived.Location = new System.Drawing.Point(5, 353);
            this.listBoxReceived.Name = "listBoxReceived";
            this.listBoxReceived.Size = new System.Drawing.Size(342, 212);
            this.listBoxReceived.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.GreenYellow;
            this.label10.Location = new System.Drawing.Point(6, 328);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 17);
            this.label10.TabIndex = 5;
            this.label10.Text = "Received:";
            // 
            // buttonClear
            // 
            this.buttonClear.BackColor = System.Drawing.Color.Red;
            this.buttonClear.Location = new System.Drawing.Point(229, 281);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(119, 42);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonGui
            // 
            this.buttonGui.Location = new System.Drawing.Point(6, 281);
            this.buttonGui.Name = "buttonGui";
            this.buttonGui.Size = new System.Drawing.Size(119, 42);
            this.buttonGui.TabIndex = 2;
            this.buttonGui.Text = "Enter";
            this.buttonGui.UseVisualStyleBackColor = true;
            this.buttonGui.Click += new System.EventHandler(this.buttonGui_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.GreenYellow;
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Import:";
            // 
            // textBoxNhapDuLieu
            // 
            this.textBoxNhapDuLieu.Location = new System.Drawing.Point(6, 51);
            this.textBoxNhapDuLieu.Multiline = true;
            this.textBoxNhapDuLieu.Name = "textBoxNhapDuLieu";
            this.textBoxNhapDuLieu.Size = new System.Drawing.Size(342, 227);
            this.textBoxNhapDuLieu.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Home);
            this.groupBox3.Controls.Add(this.XAxis);
            this.groupBox3.Controls.Add(this.E_Up);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.ServoOn);
            this.groupBox3.Controls.Add(this.Y_Down);
            this.groupBox3.Controls.Add(this.Record);
            this.groupBox3.Controls.Add(this.Z_Down);
            this.groupBox3.Controls.Add(this.ServoOff);
            this.groupBox3.Controls.Add(this.Z_Up);
            this.groupBox3.Controls.Add(this.X_Down);
            this.groupBox3.Controls.Add(this.H_Down);
            this.groupBox3.Controls.Add(this.H_Up);
            this.groupBox3.Controls.Add(this.E_Down);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.Y_Up);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.X_Up);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(6, 246);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(414, 349);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "By hand";
            // 
            // Home
            // 
            this.Home.Location = new System.Drawing.Point(211, 213);
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(93, 43);
            this.Home.TabIndex = 82;
            this.Home.Text = "Home";
            this.Home.UseVisualStyleBackColor = true;
            this.Home.Click += new System.EventHandler(this.Home_Click);
            // 
            // XAxis
            // 
            this.XAxis.BackColor = System.Drawing.Color.Silver;
            this.XAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxis.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.XAxis.Location = new System.Drawing.Point(23, 84);
            this.XAxis.Name = "XAxis";
            this.XAxis.Size = new System.Drawing.Size(50, 50);
            this.XAxis.TabIndex = 64;
            this.XAxis.Text = "X";
            this.XAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // E_Up
            // 
            this.E_Up.Location = new System.Drawing.Point(258, 25);
            this.E_Up.Name = "E_Up";
            this.E_Up.Size = new System.Drawing.Size(50, 50);
            this.E_Up.TabIndex = 75;
            this.E_Up.Text = "Up";
            this.E_Up.UseVisualStyleBackColor = true;
            this.E_Up.Click += new System.EventHandler(this.E_Up_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Silver;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label9.Location = new System.Drawing.Point(334, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 50);
            this.label9.TabIndex = 76;
            this.label9.Text = "H";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServoOn
            // 
            this.ServoOn.Location = new System.Drawing.Point(104, 213);
            this.ServoOn.Name = "ServoOn";
            this.ServoOn.Size = new System.Drawing.Size(93, 43);
            this.ServoOn.TabIndex = 79;
            this.ServoOn.Text = "Servo kep";
            this.ServoOn.UseVisualStyleBackColor = true;
            this.ServoOn.Click += new System.EventHandler(this.ServoOn_Click);
            // 
            // Y_Down
            // 
            this.Y_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Y_Down.Location = new System.Drawing.Point(102, 145);
            this.Y_Down.Name = "Y_Down";
            this.Y_Down.Size = new System.Drawing.Size(50, 50);
            this.Y_Down.TabIndex = 68;
            this.Y_Down.Text = "Down";
            this.Y_Down.UseVisualStyleBackColor = true;
            this.Y_Down.Click += new System.EventHandler(this.Y_Down_Click);
            // 
            // Record
            // 
            this.Record.Location = new System.Drawing.Point(211, 275);
            this.Record.Name = "Record";
            this.Record.Size = new System.Drawing.Size(93, 43);
            this.Record.TabIndex = 81;
            this.Record.Text = "Record";
            this.Record.UseVisualStyleBackColor = true;
            this.Record.Click += new System.EventHandler(this.Record_Click);
            // 
            // Z_Down
            // 
            this.Z_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Z_Down.Location = new System.Drawing.Point(181, 145);
            this.Z_Down.Name = "Z_Down";
            this.Z_Down.Size = new System.Drawing.Size(50, 50);
            this.Z_Down.TabIndex = 71;
            this.Z_Down.Text = "Down";
            this.Z_Down.UseVisualStyleBackColor = true;
            this.Z_Down.Click += new System.EventHandler(this.Z_Down_Click);
            // 
            // ServoOff
            // 
            this.ServoOff.Location = new System.Drawing.Point(104, 275);
            this.ServoOff.Name = "ServoOff";
            this.ServoOff.Size = new System.Drawing.Size(93, 43);
            this.ServoOff.TabIndex = 80;
            this.ServoOff.Text = "Servo nha";
            this.ServoOff.UseVisualStyleBackColor = true;
            this.ServoOff.Click += new System.EventHandler(this.ServoOff_Click);
            // 
            // Z_Up
            // 
            this.Z_Up.Location = new System.Drawing.Point(181, 25);
            this.Z_Up.Name = "Z_Up";
            this.Z_Up.Size = new System.Drawing.Size(50, 50);
            this.Z_Up.TabIndex = 72;
            this.Z_Up.Text = "Up";
            this.Z_Up.UseVisualStyleBackColor = true;
            this.Z_Up.Click += new System.EventHandler(this.Z_Up_Click);
            // 
            // X_Down
            // 
            this.X_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.X_Down.Location = new System.Drawing.Point(23, 144);
            this.X_Down.Name = "X_Down";
            this.X_Down.Size = new System.Drawing.Size(50, 50);
            this.X_Down.TabIndex = 66;
            this.X_Down.Text = "Down";
            this.X_Down.UseVisualStyleBackColor = true;
            this.X_Down.Click += new System.EventHandler(this.X_Down_Click);
            // 
            // H_Down
            // 
            this.H_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.H_Down.Location = new System.Drawing.Point(334, 145);
            this.H_Down.Name = "H_Down";
            this.H_Down.Size = new System.Drawing.Size(50, 50);
            this.H_Down.TabIndex = 77;
            this.H_Down.Text = "Down";
            this.H_Down.UseVisualStyleBackColor = true;
            this.H_Down.Click += new System.EventHandler(this.H_Down_Click);
            // 
            // H_Up
            // 
            this.H_Up.Location = new System.Drawing.Point(334, 25);
            this.H_Up.Name = "H_Up";
            this.H_Up.Size = new System.Drawing.Size(50, 50);
            this.H_Up.TabIndex = 78;
            this.H_Up.Text = "Up";
            this.H_Up.UseVisualStyleBackColor = true;
            this.H_Up.Click += new System.EventHandler(this.H_Up_Click);
            // 
            // E_Down
            // 
            this.E_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.E_Down.Location = new System.Drawing.Point(258, 145);
            this.E_Down.Name = "E_Down";
            this.E_Down.Size = new System.Drawing.Size(50, 50);
            this.E_Down.TabIndex = 74;
            this.E_Down.Text = "Down";
            this.E_Down.UseVisualStyleBackColor = true;
            this.E_Down.Click += new System.EventHandler(this.E_Down_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Silver;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(258, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 50);
            this.label8.TabIndex = 73;
            this.label8.Text = "E";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Y_Up
            // 
            this.Y_Up.Location = new System.Drawing.Point(102, 25);
            this.Y_Up.Name = "Y_Up";
            this.Y_Up.Size = new System.Drawing.Size(50, 50);
            this.Y_Up.TabIndex = 69;
            this.Y_Up.Text = "Up";
            this.Y_Up.UseVisualStyleBackColor = true;
            this.Y_Up.Click += new System.EventHandler(this.Y_Up_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Silver;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(181, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 50);
            this.label7.TabIndex = 70;
            this.label7.Text = "Z";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_Up
            // 
            this.X_Up.Location = new System.Drawing.Point(23, 24);
            this.X_Up.Name = "X_Up";
            this.X_Up.Size = new System.Drawing.Size(50, 50);
            this.X_Up.TabIndex = 65;
            this.X_Up.Text = "Up";
            this.X_Up.UseVisualStyleBackColor = true;
            this.X_Up.Click += new System.EventHandler(this.X_Up_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Silver;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(102, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 50);
            this.label6.TabIndex = 67;
            this.label6.Text = "Y";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonAutomatic);
            this.groupBox2.Controls.Add(this.buttonInitialPort);
            this.groupBox2.Controls.Add(this.comboBoxBaudRate);
            this.groupBox2.Controls.Add(this.comboBoxPort);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.labelPortStatus);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 221);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Setup";
            // 
            // buttonAutomatic
            // 
            this.buttonAutomatic.Location = new System.Drawing.Point(247, 162);
            this.buttonAutomatic.Name = "buttonAutomatic";
            this.buttonAutomatic.Size = new System.Drawing.Size(152, 45);
            this.buttonAutomatic.TabIndex = 4;
            this.buttonAutomatic.Text = "Automatic";
            this.buttonAutomatic.UseVisualStyleBackColor = true;
            this.buttonAutomatic.Click += new System.EventHandler(this.buttonAutomatic_Click);
            // 
            // buttonInitialPort
            // 
            this.buttonInitialPort.Location = new System.Drawing.Point(9, 162);
            this.buttonInitialPort.Name = "buttonInitialPort";
            this.buttonInitialPort.Size = new System.Drawing.Size(179, 46);
            this.buttonInitialPort.TabIndex = 3;
            this.buttonInitialPort.Text = "Connect / Disconnect Serial";
            this.buttonInitialPort.UseVisualStyleBackColor = true;
            this.buttonInitialPort.Click += new System.EventHandler(this.buttonInitialPort_Click);
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Location = new System.Drawing.Point(292, 64);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(107, 24);
            this.comboBoxBaudRate.TabIndex = 2;
            this.comboBoxBaudRate.Text = "115200";
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(81, 64);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(107, 24);
            this.comboBoxPort.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(207, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Baud Rate:";
            // 
            // labelPortStatus
            // 
            this.labelPortStatus.AutoSize = true;
            this.labelPortStatus.BackColor = System.Drawing.Color.Red;
            this.labelPortStatus.Location = new System.Drawing.Point(83, 119);
            this.labelPortStatus.Name = "labelPortStatus";
            this.labelPortStatus.Size = new System.Drawing.Size(102, 17);
            this.labelPortStatus.TabIndex = 0;
            this.labelPortStatus.Text = "Not Connected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Connect 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Status1:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM Port";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(789, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.chooseStepToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // chooseStepToolStripMenuItem
            // 
            this.chooseStepToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Xax,
            this.Yax,
            this.Zax,
            this.Eax,
            this.Hax});
            this.chooseStepToolStripMenuItem.Name = "chooseStepToolStripMenuItem";
            this.chooseStepToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.chooseStepToolStripMenuItem.Text = "Choose Step";
            // 
            // Xax
            // 
            this.Xax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportXStep});
            this.Xax.Name = "Xax";
            this.Xax.Size = new System.Drawing.Size(103, 26);
            this.Xax.Text = "X";
            // 
            // ImportXStep
            // 
            this.ImportXStep.Name = "ImportXStep";
            this.ImportXStep.Size = new System.Drawing.Size(108, 26);
            this.ImportXStep.Text = "10";
            // 
            // Yax
            // 
            this.Yax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportYStep});
            this.Yax.Name = "Yax";
            this.Yax.Size = new System.Drawing.Size(103, 26);
            this.Yax.Text = "Y";
            // 
            // ImportYStep
            // 
            this.ImportYStep.Name = "ImportYStep";
            this.ImportYStep.Size = new System.Drawing.Size(108, 26);
            this.ImportYStep.Text = "10";
            // 
            // Zax
            // 
            this.Zax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportZStep});
            this.Zax.Name = "Zax";
            this.Zax.Size = new System.Drawing.Size(103, 26);
            this.Zax.Text = "Z";
            // 
            // ImportZStep
            // 
            this.ImportZStep.Name = "ImportZStep";
            this.ImportZStep.Size = new System.Drawing.Size(108, 26);
            this.ImportZStep.Text = "10";
            // 
            // Eax
            // 
            this.Eax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportEStep});
            this.Eax.Name = "Eax";
            this.Eax.Size = new System.Drawing.Size(103, 26);
            this.Eax.Text = "E";
            // 
            // ImportEStep
            // 
            this.ImportEStep.Name = "ImportEStep";
            this.ImportEStep.Size = new System.Drawing.Size(108, 26);
            this.ImportEStep.Text = "10";
            // 
            // Hax
            // 
            this.Hax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportHStep});
            this.Hax.Name = "Hax";
            this.Hax.Size = new System.Drawing.Size(103, 26);
            this.Hax.Text = "H";
            // 
            // ImportHStep
            // 
            this.ImportHStep.Name = "ImportHStep";
            this.ImportHStep.Size = new System.Drawing.Size(108, 26);
            this.ImportHStep.Text = "10";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.AutoImage);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(795, 641);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "ImageAuto";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // AutoImage
            // 
            this.AutoImage.Location = new System.Drawing.Point(212, 223);
            this.AutoImage.Name = "AutoImage";
            this.AutoImage.Size = new System.Drawing.Size(378, 167);
            this.AutoImage.TabIndex = 0;
            this.AutoImage.Text = "Find Object";
            this.AutoImage.UseVisualStyleBackColor = true;
            this.AutoImage.Click += new System.EventHandler(this.AutoImage_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(809, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(640, 670);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonCapture);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Controls.Add(this.pictureBoxDepth);
            this.tabPage1.Controls.Add(this.pictureBoxColor);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(632, 641);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "RGBD";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(419, 313);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "Depth";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(24, 313);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Color";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(18, 364);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(594, 116);
            this.listBox1.TabIndex = 2;
            // 
            // pictureBoxDepth
            // 
            this.pictureBoxDepth.BackColor = System.Drawing.Color.Aqua;
            this.pictureBoxDepth.Location = new System.Drawing.Point(323, 15);
            this.pictureBoxDepth.Name = "pictureBoxDepth";
            this.pictureBoxDepth.Size = new System.Drawing.Size(290, 284);
            this.pictureBoxDepth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDepth.TabIndex = 1;
            this.pictureBoxDepth.TabStop = false;
            // 
            // pictureBoxColor
            // 
            this.pictureBoxColor.BackColor = System.Drawing.Color.PaleTurquoise;
            this.pictureBoxColor.Location = new System.Drawing.Point(24, 15);
            this.pictureBoxColor.Name = "pictureBoxColor";
            this.pictureBoxColor.Size = new System.Drawing.Size(293, 284);
            this.pictureBoxColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxColor.TabIndex = 0;
            this.pictureBoxColor.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBoxPointCloud);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(632, 641);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "PointCloud";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPointCloud
            // 
            this.pictureBoxPointCloud.BackColor = System.Drawing.Color.LightGray;
            this.pictureBoxPointCloud.Location = new System.Drawing.Point(18, 17);
            this.pictureBoxPointCloud.Name = "pictureBoxPointCloud";
            this.pictureBoxPointCloud.Size = new System.Drawing.Size(593, 465);
            this.pictureBoxPointCloud.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPointCloud.TabIndex = 0;
            this.pictureBoxPointCloud.TabStop = false;
            // 
            // buttonCapture
            // 
            this.buttonCapture.Location = new System.Drawing.Point(159, 309);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(111, 31);
            this.buttonCapture.TabIndex = 5;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1461, 687);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabControl2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPointCloud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button buttonOpenPly;
        private System.Windows.Forms.Button buttonTestPlaneDetection;
        private System.Windows.Forms.Button buttonCapturePointCloud;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBoxPointCloud;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox pictureBoxDepth;
        private System.Windows.Forms.PictureBox pictureBoxColor;
        private System.Windows.Forms.Button ClusterExtraction;
        private System.Windows.Forms.Button OrientedBoundingBox;
        private System.Windows.Forms.Button Cut;
        private System.Windows.Forms.Button buttonSavePLY;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonRemovingOutliers;
        private System.Windows.Forms.Button buttonAABB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox listBoxReceived;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonGui;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNhapDuLieu;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Home;
        private System.Windows.Forms.Label XAxis;
        private System.Windows.Forms.Button E_Up;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button ServoOn;
        private System.Windows.Forms.Button Y_Down;
        private System.Windows.Forms.Button Record;
        private System.Windows.Forms.Button Z_Down;
        private System.Windows.Forms.Button ServoOff;
        private System.Windows.Forms.Button Z_Up;
        private System.Windows.Forms.Button X_Down;
        private System.Windows.Forms.Button H_Down;
        private System.Windows.Forms.Button H_Up;
        private System.Windows.Forms.Button E_Down;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button Y_Up;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button X_Up;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonInitialPort;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelPortStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Xax;
        private System.Windows.Forms.ToolStripMenuItem ImportXStep;
        private System.Windows.Forms.ToolStripMenuItem Yax;
        private System.Windows.Forms.ToolStripMenuItem ImportYStep;
        private System.Windows.Forms.ToolStripMenuItem Zax;
        private System.Windows.Forms.ToolStripMenuItem ImportZStep;
        private System.Windows.Forms.ToolStripMenuItem Eax;
        private System.Windows.Forms.ToolStripMenuItem ImportEStep;
        private System.Windows.Forms.ToolStripMenuItem Hax;
        private System.Windows.Forms.ToolStripMenuItem ImportHStep;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button ClearPointCloud;
        private System.Windows.Forms.Button buttonAutomatic;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button AutoImage;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonCapture;
    }
}

