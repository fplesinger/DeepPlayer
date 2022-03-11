namespace plugins_deepplayer
{
    partial class SVP_DEEP
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btChannels = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbSO = new System.Windows.Forms.Label();
            this.lbChannelUsage = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chbUM = new System.Windows.Forms.CheckBox();
            this.tbChannelBaseName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ampg = new System.Windows.Forms.TabPage();
            this.lmDesc = new System.Windows.Forms.TextBox();
            this.lbPrefSamp = new System.Windows.Forms.Label();
            this.lbf = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.rbFitBoth = new System.Windows.Forms.RadioButton();
            this.lmgd = new System.Windows.Forms.Label();
            this.rbFitRes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rbFitAll = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.rbFitOrig = new System.Windows.Forms.RadioButton();
            this.lmi = new System.Windows.Forms.Label();
            this.lmgn = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lmo = new System.Windows.Forms.Label();
            this.lmprod = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lmdom = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mtpg = new System.Windows.Forms.TabPage();
            this.tbMTD = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbUseFills = new System.Windows.Forms.RadioButton();
            this.rbCurves = new System.Windows.Forms.RadioButton();
            this.chbDrawWinBorders = new System.Windows.Forms.CheckBox();
            this.chbDrawOver = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbGPU = new System.Windows.Forms.RadioButton();
            this.rbCPU = new System.Windows.Forms.RadioButton();
            this.gbPostProc = new System.Windows.Forms.GroupBox();
            this.chbDeSTD = new System.Windows.Forms.CheckBox();
            this.chbSoftMax = new System.Windows.Forms.CheckBox();
            this.chbDoSigm = new System.Windows.Forms.CheckBox();
            this.rbOutMax = new System.Windows.Forms.RadioButton();
            this.rbOutSeparated = new System.Windows.Forms.RadioButton();
            this.gbPreproc = new System.Windows.Forms.GroupBox();
            this.rbPreprocNorm = new System.Windows.Forms.RadioButton();
            this.rbPreprocSTD = new System.Windows.Forms.RadioButton();
            this.rbPreprocNone = new System.Windows.Forms.RadioButton();
            this.gbBatch = new System.Windows.Forms.GroupBox();
            this.nudWO = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudWS = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.chbUpdateAuto = new System.Windows.Forms.CheckBox();
            this.btPrev = new System.Windows.Forms.Button();
            this.btProcess = new System.Windows.Forms.Button();
            this.pbx = new System.Windows.Forms.PictureBox();
            this.bgw = new System.ComponentModel.BackgroundWorker();
            this.ofd_onnx = new System.Windows.Forms.OpenFileDialog();
            this.bgwPrev = new System.ComponentModel.BackgroundWorker();
            this.modelLoader = new System.ComponentModel.BackgroundWorker();
            this.topPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.ampg.SuspendLayout();
            this.mtpg.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbPostProc.SuspendLayout();
            this.gbPreproc.SuspendLayout();
            this.gbBatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWS)).BeginInit();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx)).BeginInit();
            this.SuspendLayout();
            // 
            // btChannels
            // 
            this.btChannels.AllowDrop = true;
            this.btChannels.Location = new System.Drawing.Point(3, 3);
            this.btChannels.Name = "btChannels";
            this.btChannels.Size = new System.Drawing.Size(313, 60);
            this.btChannels.TabIndex = 15;
            this.btChannels.Text = "(Choose channel)";
            this.btChannels.UseVisualStyleBackColor = true;
            this.btChannels.Click += new System.EventHandler(this.btChannels_Click);
            this.btChannels.DragDrop += new System.Windows.Forms.DragEventHandler(this.btChannels_DragDrop);
            this.btChannels.DragEnter += new System.Windows.Forms.DragEventHandler(this.btChannels_DragEnter);
            // 
            // topPanel
            // 
            this.topPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topPanel.Controls.Add(this.groupBox3);
            this.topPanel.Controls.Add(this.tabControl1);
            this.topPanel.Controls.Add(this.groupBox2);
            this.topPanel.Controls.Add(this.groupBox1);
            this.topPanel.Controls.Add(this.gbPostProc);
            this.topPanel.Controls.Add(this.gbPreproc);
            this.topPanel.Controls.Add(this.gbBatch);
            this.topPanel.Controls.Add(this.btChannels);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1107, 223);
            this.topPanel.TabIndex = 1;
            this.topPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.topPanel_Paint);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbSO);
            this.groupBox3.Controls.Add(this.lbChannelUsage);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.chbUM);
            this.groupBox3.Controls.Add(this.tbChannelBaseName);
            this.groupBox3.Location = new System.Drawing.Point(934, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(166, 213);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output naming and selection";
            // 
            // lbSO
            // 
            this.lbSO.AutoSize = true;
            this.lbSO.Location = new System.Drawing.Point(7, 68);
            this.lbSO.Name = "lbSO";
            this.lbSO.Size = new System.Drawing.Size(87, 13);
            this.lbSO.TabIndex = 35;
            this.lbSO.Text = "Selected outputs";
            // 
            // lbChannelUsage
            // 
            this.lbChannelUsage.Enabled = false;
            this.lbChannelUsage.FormattingEnabled = true;
            this.lbChannelUsage.Items.AddRange(new object[] {
            ""});
            this.lbChannelUsage.Location = new System.Drawing.Point(9, 85);
            this.lbChannelUsage.Name = "lbChannelUsage";
            this.lbChannelUsage.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbChannelUsage.Size = new System.Drawing.Size(151, 121);
            this.lbChannelUsage.TabIndex = 13;
            this.lbChannelUsage.SelectedValueChanged += new System.EventHandler(this.lbChannelUsage_SelectedValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Base name:";
            // 
            // chbUM
            // 
            this.chbUM.AutoSize = true;
            this.chbUM.Enabled = false;
            this.chbUM.Location = new System.Drawing.Point(9, 43);
            this.chbUM.Name = "chbUM";
            this.chbUM.Size = new System.Drawing.Size(92, 17);
            this.chbUM.TabIndex = 10;
            this.chbUM.Text = "Use metadata";
            this.chbUM.UseVisualStyleBackColor = true;
            this.chbUM.CheckedChanged += new System.EventHandler(this.chbUM_CheckedChanged);
            // 
            // tbChannelBaseName
            // 
            this.tbChannelBaseName.Location = new System.Drawing.Point(69, 19);
            this.tbChannelBaseName.Name = "tbChannelBaseName";
            this.tbChannelBaseName.Size = new System.Drawing.Size(41, 20);
            this.tbChannelBaseName.TabIndex = 8;
            this.tbChannelBaseName.Text = "Out";
            this.tbChannelBaseName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbChannelBaseName_KeyUp);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ampg);
            this.tabControl1.Controls.Add(this.mtpg);
            this.tabControl1.Location = new System.Drawing.Point(3, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(504, 155);
            this.tabControl1.TabIndex = 32;
            // 
            // ampg
            // 
            this.ampg.BackColor = System.Drawing.SystemColors.Control;
            this.ampg.Controls.Add(this.lmDesc);
            this.ampg.Controls.Add(this.lbPrefSamp);
            this.ampg.Controls.Add(this.lbf);
            this.ampg.Controls.Add(this.button1);
            this.ampg.Controls.Add(this.rbFitBoth);
            this.ampg.Controls.Add(this.lmgd);
            this.ampg.Controls.Add(this.rbFitRes);
            this.ampg.Controls.Add(this.label1);
            this.ampg.Controls.Add(this.rbFitAll);
            this.ampg.Controls.Add(this.label9);
            this.ampg.Controls.Add(this.rbFitOrig);
            this.ampg.Controls.Add(this.lmi);
            this.ampg.Controls.Add(this.lmgn);
            this.ampg.Controls.Add(this.label4);
            this.ampg.Controls.Add(this.label6);
            this.ampg.Controls.Add(this.lmo);
            this.ampg.Controls.Add(this.lmprod);
            this.ampg.Controls.Add(this.label3);
            this.ampg.Controls.Add(this.label7);
            this.ampg.Controls.Add(this.lmdom);
            this.ampg.Controls.Add(this.label5);
            this.ampg.Location = new System.Drawing.Point(4, 22);
            this.ampg.Name = "ampg";
            this.ampg.Padding = new System.Windows.Forms.Padding(3);
            this.ampg.Size = new System.Drawing.Size(496, 129);
            this.ampg.TabIndex = 0;
            this.ampg.Text = "A model (input/output)";
            // 
            // lmDesc
            // 
            this.lmDesc.Location = new System.Drawing.Point(261, 24);
            this.lmDesc.Multiline = true;
            this.lmDesc.Name = "lmDesc";
            this.lmDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lmDesc.Size = new System.Drawing.Size(229, 32);
            this.lmDesc.TabIndex = 34;
            // 
            // lbPrefSamp
            // 
            this.lbPrefSamp.AutoSize = true;
            this.lbPrefSamp.Location = new System.Drawing.Point(5, 97);
            this.lbPrefSamp.Name = "lbPrefSamp";
            this.lbPrefSamp.Size = new System.Drawing.Size(119, 13);
            this.lbPrefSamp.TabIndex = 32;
            this.lbPrefSamp.Text = "Preferred sampling [Hz]:";
            // 
            // lbf
            // 
            this.lbf.Location = new System.Drawing.Point(10, 5);
            this.lbf.Name = "lbf";
            this.lbf.Size = new System.Drawing.Size(453, 22);
            this.lbf.TabIndex = 31;
            this.lbf.Text = "---";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 30);
            this.button1.TabIndex = 16;
            this.button1.Text = "Load an ONNX model";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rbFitBoth
            // 
            this.rbFitBoth.AutoSize = true;
            this.rbFitBoth.Location = new System.Drawing.Point(376, 47);
            this.rbFitBoth.Name = "rbFitBoth";
            this.rbFitBoth.Size = new System.Drawing.Size(85, 17);
            this.rbFitBoth.TabIndex = 4;
            this.rbFitBoth.Tag = "vizBoth:rbChecked";
            this.rbFitBoth.Text = "Independent";
            this.rbFitBoth.UseVisualStyleBackColor = true;
            this.rbFitBoth.Visible = false;
            this.rbFitBoth.CheckedChanged += new System.EventHandler(this.rbFitBoth_CheckedChanged);
            // 
            // lmgd
            // 
            this.lmgd.AutoSize = true;
            this.lmgd.Location = new System.Drawing.Point(263, 105);
            this.lmgd.Name = "lmgd";
            this.lmgd.Size = new System.Drawing.Size(16, 13);
            this.lmgd.TabIndex = 30;
            this.lmgd.Text = "---";
            // 
            // rbFitRes
            // 
            this.rbFitRes.AutoSize = true;
            this.rbFitRes.Checked = true;
            this.rbFitRes.Location = new System.Drawing.Point(231, 47);
            this.rbFitRes.Name = "rbFitRes";
            this.rbFitRes.Size = new System.Drawing.Size(55, 17);
            this.rbFitRes.TabIndex = 3;
            this.rbFitRes.TabStop = true;
            this.rbFitRes.Tag = "vizRes:rbChecked";
            this.rbFitRes.Text = "Result";
            this.rbFitRes.UseVisualStyleBackColor = true;
            this.rbFitRes.Visible = false;
            this.rbFitRes.CheckedChanged += new System.EventHandler(this.rbFitRes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Input:";
            // 
            // rbFitAll
            // 
            this.rbFitAll.AutoSize = true;
            this.rbFitAll.Location = new System.Drawing.Point(289, 47);
            this.rbFitAll.Name = "rbFitAll";
            this.rbFitAll.Size = new System.Drawing.Size(83, 17);
            this.rbFitAll.TabIndex = 1;
            this.rbFitAll.Tag = "vizTotE:rbChecked";
            this.rbFitAll.Text = "Tot. extrems";
            this.rbFitAll.UseVisualStyleBackColor = true;
            this.rbFitAll.Visible = false;
            this.rbFitAll.CheckedChanged += new System.EventHandler(this.rbFitAll_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(167, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Graph description:";
            // 
            // rbFitOrig
            // 
            this.rbFitOrig.AutoSize = true;
            this.rbFitOrig.Location = new System.Drawing.Point(170, 47);
            this.rbFitOrig.Name = "rbFitOrig";
            this.rbFitOrig.Size = new System.Drawing.Size(60, 17);
            this.rbFitOrig.TabIndex = 0;
            this.rbFitOrig.Tag = "vizOrig:rbChecked";
            this.rbFitOrig.Text = "Original";
            this.rbFitOrig.UseVisualStyleBackColor = true;
            this.rbFitOrig.Visible = false;
            this.rbFitOrig.CheckedChanged += new System.EventHandler(this.rbFitOrig_CheckedChanged);
            // 
            // lmi
            // 
            this.lmi.AutoSize = true;
            this.lmi.Location = new System.Drawing.Point(45, 61);
            this.lmi.Name = "lmi";
            this.lmi.Size = new System.Drawing.Size(16, 13);
            this.lmi.TabIndex = 18;
            this.lmi.Text = "---";
            // 
            // lmgn
            // 
            this.lmgn.AutoSize = true;
            this.lmgn.Location = new System.Drawing.Point(263, 90);
            this.lmgn.Name = "lmgn";
            this.lmgn.Size = new System.Drawing.Size(16, 13);
            this.lmgn.TabIndex = 28;
            this.lmgn.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Output:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(192, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Graph name:";
            // 
            // lmo
            // 
            this.lmo.AutoSize = true;
            this.lmo.Location = new System.Drawing.Point(45, 79);
            this.lmo.Name = "lmo";
            this.lmo.Size = new System.Drawing.Size(16, 13);
            this.lmo.TabIndex = 20;
            this.lmo.Text = "---";
            // 
            // lmprod
            // 
            this.lmprod.AutoSize = true;
            this.lmprod.Location = new System.Drawing.Point(263, 75);
            this.lmprod.Name = "lmprod";
            this.lmprod.Size = new System.Drawing.Size(16, 13);
            this.lmprod.TabIndex = 26;
            this.lmprod.Text = "---";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Description:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(207, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Producer:";
            // 
            // lmdom
            // 
            this.lmdom.AutoSize = true;
            this.lmdom.Location = new System.Drawing.Point(263, 59);
            this.lmdom.Name = "lmdom";
            this.lmdom.Size = new System.Drawing.Size(16, 13);
            this.lmdom.TabIndex = 24;
            this.lmdom.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(214, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Domain:";
            // 
            // mtpg
            // 
            this.mtpg.BackColor = System.Drawing.SystemColors.Control;
            this.mtpg.Controls.Add(this.tbMTD);
            this.mtpg.Location = new System.Drawing.Point(4, 22);
            this.mtpg.Name = "mtpg";
            this.mtpg.Padding = new System.Windows.Forms.Padding(3);
            this.mtpg.Size = new System.Drawing.Size(496, 129);
            this.mtpg.TabIndex = 1;
            this.mtpg.Text = "Model metadata";
            // 
            // tbMTD
            // 
            this.tbMTD.Location = new System.Drawing.Point(7, 7);
            this.tbMTD.Multiline = true;
            this.tbMTD.Name = "tbMTD";
            this.tbMTD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMTD.Size = new System.Drawing.Size(456, 113);
            this.tbMTD.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbUseFills);
            this.groupBox2.Controls.Add(this.rbCurves);
            this.groupBox2.Controls.Add(this.chbDrawWinBorders);
            this.groupBox2.Controls.Add(this.chbDrawOver);
            this.groupBox2.Location = new System.Drawing.Point(631, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 58);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display control";
            // 
            // rbUseFills
            // 
            this.rbUseFills.AutoSize = true;
            this.rbUseFills.Location = new System.Drawing.Point(193, 35);
            this.rbUseFills.Name = "rbUseFills";
            this.rbUseFills.Size = new System.Drawing.Size(61, 17);
            this.rbUseFills.TabIndex = 13;
            this.rbUseFills.Text = "Use fills";
            this.rbUseFills.UseVisualStyleBackColor = true;
            // 
            // rbCurves
            // 
            this.rbCurves.AutoSize = true;
            this.rbCurves.Checked = true;
            this.rbCurves.Location = new System.Drawing.Point(192, 17);
            this.rbCurves.Name = "rbCurves";
            this.rbCurves.Size = new System.Drawing.Size(85, 17);
            this.rbCurves.TabIndex = 12;
            this.rbCurves.TabStop = true;
            this.rbCurves.Text = "Draw curves";
            this.rbCurves.UseVisualStyleBackColor = true;
            this.rbCurves.CheckedChanged += new System.EventHandler(this.rbCurves_CheckedChanged);
            // 
            // chbDrawWinBorders
            // 
            this.chbDrawWinBorders.AutoSize = true;
            this.chbDrawWinBorders.Location = new System.Drawing.Point(44, 18);
            this.chbDrawWinBorders.Name = "chbDrawWinBorders";
            this.chbDrawWinBorders.Size = new System.Drawing.Size(128, 17);
            this.chbDrawWinBorders.TabIndex = 11;
            this.chbDrawWinBorders.Text = "Draw window borders";
            this.chbDrawWinBorders.UseVisualStyleBackColor = true;
            this.chbDrawWinBorders.CheckedChanged += new System.EventHandler(this.chbDrawWinBorders_CheckedChanged);
            // 
            // chbDrawOver
            // 
            this.chbDrawOver.AutoSize = true;
            this.chbDrawOver.Location = new System.Drawing.Point(44, 36);
            this.chbDrawOver.Name = "chbDrawOver";
            this.chbDrawOver.Size = new System.Drawing.Size(116, 17);
            this.chbDrawOver.TabIndex = 10;
            this.chbDrawOver.Text = "Draw outputs inline";
            this.chbDrawOver.UseVisualStyleBackColor = true;
            this.chbDrawOver.CheckedChanged += new System.EventHandler(this.chbDrawOver_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbGPU);
            this.groupBox1.Controls.Add(this.rbCPU);
            this.groupBox1.Location = new System.Drawing.Point(325, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 60);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inference engine";
            // 
            // rbGPU
            // 
            this.rbGPU.AutoSize = true;
            this.rbGPU.Location = new System.Drawing.Point(83, 26);
            this.rbGPU.Name = "rbGPU";
            this.rbGPU.Size = new System.Drawing.Size(109, 17);
            this.rbGPU.TabIndex = 2;
            this.rbGPU.Text = "GPU (CUDA only)";
            this.rbGPU.UseVisualStyleBackColor = true;
            this.rbGPU.CheckedChanged += new System.EventHandler(this.rbGPU_CheckedChanged);
            // 
            // rbCPU
            // 
            this.rbCPU.AutoSize = true;
            this.rbCPU.Checked = true;
            this.rbCPU.Location = new System.Drawing.Point(25, 26);
            this.rbCPU.Name = "rbCPU";
            this.rbCPU.Size = new System.Drawing.Size(47, 17);
            this.rbCPU.TabIndex = 1;
            this.rbCPU.TabStop = true;
            this.rbCPU.Text = "CPU";
            this.rbCPU.UseVisualStyleBackColor = true;
            this.rbCPU.CheckedChanged += new System.EventHandler(this.rbCPU_CheckedChanged);
            // 
            // gbPostProc
            // 
            this.gbPostProc.Controls.Add(this.chbDeSTD);
            this.gbPostProc.Controls.Add(this.chbSoftMax);
            this.gbPostProc.Controls.Add(this.chbDoSigm);
            this.gbPostProc.Controls.Add(this.rbOutMax);
            this.gbPostProc.Controls.Add(this.rbOutSeparated);
            this.gbPostProc.Enabled = false;
            this.gbPostProc.Location = new System.Drawing.Point(704, 63);
            this.gbPostProc.Name = "gbPostProc";
            this.gbPostProc.Size = new System.Drawing.Size(224, 153);
            this.gbPostProc.TabIndex = 20;
            this.gbPostProc.TabStop = false;
            this.gbPostProc.Text = "Post-processing";
            // 
            // chbDeSTD
            // 
            this.chbDeSTD.AutoSize = true;
            this.chbDeSTD.Location = new System.Drawing.Point(35, 81);
            this.chbDeSTD.Name = "chbDeSTD";
            this.chbDeSTD.Size = new System.Drawing.Size(97, 17);
            this.chbDeSTD.TabIndex = 12;
            this.chbDeSTD.Text = "De-standardize";
            this.chbDeSTD.UseVisualStyleBackColor = true;
            this.chbDeSTD.CheckedChanged += new System.EventHandler(this.chbDeSTD_CheckedChanged);
            // 
            // chbSoftMax
            // 
            this.chbSoftMax.AutoSize = true;
            this.chbSoftMax.Checked = true;
            this.chbSoftMax.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSoftMax.Location = new System.Drawing.Point(35, 61);
            this.chbSoftMax.Name = "chbSoftMax";
            this.chbSoftMax.Size = new System.Drawing.Size(91, 17);
            this.chbSoftMax.TabIndex = 11;
            this.chbSoftMax.Text = "Apply softmax";
            this.chbSoftMax.UseVisualStyleBackColor = true;
            this.chbSoftMax.CheckedChanged += new System.EventHandler(this.chbSoftMax_CheckedChanged);
            // 
            // chbDoSigm
            // 
            this.chbDoSigm.AutoSize = true;
            this.chbDoSigm.Location = new System.Drawing.Point(35, 41);
            this.chbDoSigm.Name = "chbDoSigm";
            this.chbDoSigm.Size = new System.Drawing.Size(131, 17);
            this.chbDoSigm.TabIndex = 9;
            this.chbDoSigm.Text = "Apply sigmoid function";
            this.chbDoSigm.UseVisualStyleBackColor = true;
            this.chbDoSigm.CheckedChanged += new System.EventHandler(this.chbDoSigm_CheckedChanged);
            // 
            // rbOutMax
            // 
            this.rbOutMax.AutoSize = true;
            this.rbOutMax.Location = new System.Drawing.Point(15, 112);
            this.rbOutMax.Name = "rbOutMax";
            this.rbOutMax.Size = new System.Drawing.Size(174, 17);
            this.rbOutMax.TabIndex = 1;
            this.rbOutMax.Text = "Only index of the highest output";
            this.rbOutMax.UseVisualStyleBackColor = true;
            this.rbOutMax.CheckedChanged += new System.EventHandler(this.rbOutMax_CheckedChanged);
            // 
            // rbOutSeparated
            // 
            this.rbOutSeparated.AutoSize = true;
            this.rbOutSeparated.Checked = true;
            this.rbOutSeparated.Location = new System.Drawing.Point(15, 21);
            this.rbOutSeparated.Name = "rbOutSeparated";
            this.rbOutSeparated.Size = new System.Drawing.Size(173, 17);
            this.rbOutSeparated.TabIndex = 0;
            this.rbOutSeparated.TabStop = true;
            this.rbOutSeparated.Text = "Each output as a new channel ";
            this.rbOutSeparated.UseVisualStyleBackColor = true;
            this.rbOutSeparated.CheckedChanged += new System.EventHandler(this.rbOutSeparated_CheckedChanged);
            // 
            // gbPreproc
            // 
            this.gbPreproc.Controls.Add(this.rbPreprocNorm);
            this.gbPreproc.Controls.Add(this.rbPreprocSTD);
            this.gbPreproc.Controls.Add(this.rbPreprocNone);
            this.gbPreproc.Enabled = false;
            this.gbPreproc.Location = new System.Drawing.Point(511, 144);
            this.gbPreproc.Name = "gbPreproc";
            this.gbPreproc.Size = new System.Drawing.Size(185, 72);
            this.gbPreproc.TabIndex = 19;
            this.gbPreproc.TabStop = false;
            this.gbPreproc.Text = "Pre-processing";
            // 
            // rbPreprocNorm
            // 
            this.rbPreprocNorm.AutoSize = true;
            this.rbPreprocNorm.Location = new System.Drawing.Point(70, 20);
            this.rbPreprocNorm.Name = "rbPreprocNorm";
            this.rbPreprocNorm.Size = new System.Drawing.Size(104, 17);
            this.rbPreprocNorm.TabIndex = 2;
            this.rbPreprocNorm.Text = "Normalize <-1;1>";
            this.rbPreprocNorm.UseVisualStyleBackColor = true;
            this.rbPreprocNorm.CheckedChanged += new System.EventHandler(this.rbPreprocSTD_CheckedChanged);
            // 
            // rbPreprocSTD
            // 
            this.rbPreprocSTD.AutoSize = true;
            this.rbPreprocSTD.Checked = true;
            this.rbPreprocSTD.Location = new System.Drawing.Point(15, 45);
            this.rbPreprocSTD.Name = "rbPreprocSTD";
            this.rbPreprocSTD.Size = new System.Drawing.Size(126, 17);
            this.rbPreprocSTD.TabIndex = 1;
            this.rbPreprocSTD.TabStop = true;
            this.rbPreprocSTD.Text = "Standardize (Z-score)";
            this.rbPreprocSTD.UseVisualStyleBackColor = true;
            this.rbPreprocSTD.CheckedChanged += new System.EventHandler(this.rbPreprocSTD_CheckedChanged);
            // 
            // rbPreprocNone
            // 
            this.rbPreprocNone.AutoSize = true;
            this.rbPreprocNone.Location = new System.Drawing.Point(15, 20);
            this.rbPreprocNone.Name = "rbPreprocNone";
            this.rbPreprocNone.Size = new System.Drawing.Size(51, 17);
            this.rbPreprocNone.TabIndex = 0;
            this.rbPreprocNone.Text = "None";
            this.rbPreprocNone.UseVisualStyleBackColor = true;
            this.rbPreprocNone.CheckedChanged += new System.EventHandler(this.rbPreprocSTD_CheckedChanged);
            // 
            // gbBatch
            // 
            this.gbBatch.Controls.Add(this.nudWO);
            this.gbBatch.Controls.Add(this.label8);
            this.gbBatch.Controls.Add(this.nudWS);
            this.gbBatch.Controls.Add(this.label2);
            this.gbBatch.Enabled = false;
            this.gbBatch.Location = new System.Drawing.Point(511, 63);
            this.gbBatch.Name = "gbBatch";
            this.gbBatch.Size = new System.Drawing.Size(185, 78);
            this.gbBatch.TabIndex = 18;
            this.gbBatch.TabStop = false;
            this.gbBatch.Text = "Inference window";
            // 
            // nudWO
            // 
            this.nudWO.Location = new System.Drawing.Point(87, 46);
            this.nudWO.Maximum = new decimal(new int[] {
            -1486618624,
            232830643,
            0,
            0});
            this.nudWO.Name = "nudWO";
            this.nudWO.Size = new System.Drawing.Size(87, 20);
            this.nudWO.TabIndex = 5;
            this.nudWO.ValueChanged += new System.EventHandler(this.nudOverLap_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Overlap";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // nudWS
            // 
            this.nudWS.Location = new System.Drawing.Point(87, 19);
            this.nudWS.Maximum = new decimal(new int[] {
            -1486618624,
            232830643,
            0,
            0});
            this.nudWS.Name = "nudWS";
            this.nudWS.Size = new System.Drawing.Size(87, 20);
            this.nudWS.TabIndex = 3;
            this.nudWS.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWS.ValueChanged += new System.EventHandler(this.nudWS_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size [samples]";
            // 
            // bottomPanel
            // 
            this.bottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomPanel.Controls.Add(this.chbUpdateAuto);
            this.bottomPanel.Controls.Add(this.btPrev);
            this.bottomPanel.Controls.Add(this.btProcess);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 633);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1107, 37);
            this.bottomPanel.TabIndex = 3;
            // 
            // chbUpdateAuto
            // 
            this.chbUpdateAuto.AutoSize = true;
            this.chbUpdateAuto.Checked = true;
            this.chbUpdateAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbUpdateAuto.Location = new System.Drawing.Point(326, 10);
            this.chbUpdateAuto.Name = "chbUpdateAuto";
            this.chbUpdateAuto.Size = new System.Drawing.Size(128, 17);
            this.chbUpdateAuto.TabIndex = 11;
            this.chbUpdateAuto.Text = "Preview automatically";
            this.chbUpdateAuto.UseVisualStyleBackColor = true;
            // 
            // btPrev
            // 
            this.btPrev.Dock = System.Windows.Forms.DockStyle.Left;
            this.btPrev.Location = new System.Drawing.Point(160, 0);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(160, 35);
            this.btPrev.TabIndex = 21;
            this.btPrev.Text = "Gen preview";
            this.btPrev.UseVisualStyleBackColor = true;
            this.btPrev.Click += new System.EventHandler(this.button2_Click);
            // 
            // btProcess
            // 
            this.btProcess.Dock = System.Windows.Forms.DockStyle.Left;
            this.btProcess.Location = new System.Drawing.Point(0, 0);
            this.btProcess.Name = "btProcess";
            this.btProcess.Size = new System.Drawing.Size(160, 35);
            this.btProcess.TabIndex = 20;
            this.btProcess.Text = "Compute";
            this.btProcess.UseVisualStyleBackColor = true;
            this.btProcess.Click += new System.EventHandler(this.btProcess_Click);
            // 
            // pbx
            // 
            this.pbx.BackColor = System.Drawing.Color.White;
            this.pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbx.Location = new System.Drawing.Point(0, 223);
            this.pbx.Name = "pbx";
            this.pbx.Size = new System.Drawing.Size(1107, 410);
            this.pbx.TabIndex = 4;
            this.pbx.TabStop = false;
            this.pbx.Paint += new System.Windows.Forms.PaintEventHandler(this.pbx_Paint);
            this.pbx.ParentChanged += new System.EventHandler(this.pbx_ParentChanged);
            // 
            // bgw
            // 
            this.bgw.WorkerReportsProgress = true;
            this.bgw.WorkerSupportsCancellation = true;
            this.bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_DoWork);
            this.bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_ProgressChanged);
            this.bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_RunWorkerCompleted);
            // 
            // ofd_onnx
            // 
            this.ofd_onnx.Filter = "ONNX model|*.onnx";
            // 
            // bgwPrev
            // 
            this.bgwPrev.WorkerReportsProgress = true;
            this.bgwPrev.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwPrev_DoWork);
            this.bgwPrev.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwPrev_ProgressChanged);
            this.bgwPrev.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwPrev_RunWorkerCompleted);
            // 
            // modelLoader
            // 
            this.modelLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.modelLoader_DoWork);
            this.modelLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.modelLoader_RunWorkerCompleted);
            // 
            // SVP_DEEP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbx);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "SVP_DEEP";
            this.Size = new System.Drawing.Size(1107, 670);
            this.Load += new System.EventHandler(this.SVP_plugin_v3_Load);
            this.Resize += new System.EventHandler(this.SVP_DEEP_Resize);
            this.topPanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ampg.ResumeLayout(false);
            this.ampg.PerformLayout();
            this.mtpg.ResumeLayout(false);
            this.mtpg.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbPostProc.ResumeLayout(false);
            this.gbPostProc.PerformLayout();
            this.gbPreproc.ResumeLayout(false);
            this.gbPreproc.PerformLayout();
            this.gbBatch.ResumeLayout(false);
            this.gbBatch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWS)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btChannels;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button btProcess;
        private System.Windows.Forms.PictureBox pbx;
        private System.ComponentModel.BackgroundWorker bgw;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog ofd_onnx;
        private System.Windows.Forms.Label lmgd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lmgn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lmprod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lmdom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lmo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lmi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbf;
        private System.Windows.Forms.GroupBox gbBatch;
        private System.Windows.Forms.GroupBox gbPreproc;
        private System.Windows.Forms.RadioButton rbPreprocNorm;
        private System.Windows.Forms.RadioButton rbPreprocSTD;
        private System.Windows.Forms.RadioButton rbPreprocNone;
        private System.Windows.Forms.NumericUpDown nudWO;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudWS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbPostProc;
        private System.Windows.Forms.RadioButton rbOutMax;
        private System.Windows.Forms.RadioButton rbOutSeparated;
        private System.Windows.Forms.TextBox tbChannelBaseName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbGPU;
        private System.Windows.Forms.RadioButton rbCPU;
        private System.Windows.Forms.Button btPrev;
        private System.ComponentModel.BackgroundWorker bgwPrev;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbFitBoth;
        private System.Windows.Forms.RadioButton rbFitRes;
        private System.Windows.Forms.RadioButton rbFitAll;
        private System.Windows.Forms.RadioButton rbFitOrig;
        private System.Windows.Forms.CheckBox chbDoSigm;
        private System.Windows.Forms.CheckBox chbDrawOver;
        private System.ComponentModel.BackgroundWorker modelLoader;
        private System.Windows.Forms.CheckBox chbUpdateAuto;
        private System.Windows.Forms.CheckBox chbDrawWinBorders;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ampg;
        private System.Windows.Forms.TabPage mtpg;
        private System.Windows.Forms.TextBox tbMTD;
        private System.Windows.Forms.CheckBox chbUM;
        private System.Windows.Forms.CheckBox chbSoftMax;
        private System.Windows.Forms.Label lbPrefSamp;
        private System.Windows.Forms.TextBox lmDesc;
        private System.Windows.Forms.RadioButton rbUseFills;
        private System.Windows.Forms.RadioButton rbCurves;
        private System.Windows.Forms.CheckBox chbDeSTD;
        private System.Windows.Forms.ListBox lbChannelUsage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbSO;
    }
}
