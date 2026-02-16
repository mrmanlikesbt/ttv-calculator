namespace TTV_Calculator.Code.UserControls
{
	partial class BruteForceControl : UserControl
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
			if (disposing)
			{
				_engine?.Dispose();
				if (components != null)
				{
					components.Dispose();
				}
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
            components = new System.ComponentModel.Container();
            hot_tank_options_label = new Label();
            cold_tank_options_label = new Label();
            cold_tank_kpa_label = new Label();
            cold_tank_kpa_lower = new NumericUpDown();
            hot_tank_label = new Label();
            cold_tank_label = new Label();
            hot_tank_add_gas = new Button();
            cold_tank_add_gas = new Button();
            hot_tank_gas_dropdown = new ComboBox();
            hot_tank_panel = new FlowLayoutPanel();
            cold_tank_gas_dropdown = new ComboBox();
            begin_button = new Button();
            cold_tank_panel = new FlowLayoutPanel();
            highscore = new RichTextBox();
            highscore_label = new Label();
            cold_tank_kpa_upper = new NumericUpDown();
            cold_tank_kpa_dash = new Label();
            cold_tank_temperature_dash = new Label();
            cold_tank_temperature_upper = new NumericUpDown();
            cold_tank_temperature_label = new Label();
            cold_tank_temperature_lower = new NumericUpDown();
            cold_tank_pressure_step_size_label = new Label();
            cold_tank_pressure_step_size = new NumericUpDown();
            hot_tank_temperature_dash = new Label();
            hot_tank_temperature_upper = new NumericUpDown();
            hot_tank_temperature_label = new Label();
            hot_tank_temperature_lower = new NumericUpDown();
            hot_tank_kpa_dash = new Label();
            hot_tank_kpa_upper = new NumericUpDown();
            hot_tank_kpa_label = new Label();
            hot_tank_kpa_lower = new NumericUpDown();
            end_button = new Button();
            unpause_button = new Button();
            pause_button = new Button();
            cold_tank_temperature_step_size_label = new Label();
            cold_tank_temperature_step_size = new NumericUpDown();
            hot_tank_temperature_step_size_label = new Label();
            hot_tank_temperature_step_size = new NumericUpDown();
            hot_tank_pressure_step_size_label = new Label();
            hot_tank_pressure_step_size = new NumericUpDown();
            cold_tank_gas_percent_step_size_label = new Label();
            cold_tank_gas_percent_step_size = new NumericUpDown();
            gas_percent_step_size_tooltip = new ToolTip(components);
            hot_tank_gas_percent_step_size = new NumericUpDown();
            hot_tank_gas_percent_step_size_label = new Label();
            hot_tank_percent_label = new Label();
            cold_tank_percent_label = new Label();
            status_log = new RichTextBox();
            status_log_label = new Label();
            ((System.ComponentModel.ISupportInitialize)cold_tank_kpa_lower).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_kpa_upper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature_upper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature_lower).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_pressure_step_size).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature_upper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature_lower).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_kpa_upper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_kpa_lower).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature_step_size).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature_step_size).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_pressure_step_size).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_gas_percent_step_size).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_gas_percent_step_size).BeginInit();
            SuspendLayout();
            // 
            // hot_tank_options_label
            // 
            hot_tank_options_label.AutoSize = true;
            hot_tank_options_label.Location = new Point(197, 185);
            hot_tank_options_label.Name = "hot_tank_options_label";
            hot_tank_options_label.Size = new Size(100, 15);
            hot_tank_options_label.TabIndex = 68;
            hot_tank_options_label.Text = "Hot Tank Options";
            // 
            // cold_tank_options_label
            // 
            cold_tank_options_label.AutoSize = true;
            cold_tank_options_label.Location = new Point(197, 13);
            cold_tank_options_label.Name = "cold_tank_options_label";
            cold_tank_options_label.Size = new Size(105, 15);
            cold_tank_options_label.TabIndex = 64;
            cold_tank_options_label.Text = "Cold Tank Options";
            // 
            // cold_tank_kpa_label
            // 
            cold_tank_kpa_label.AutoSize = true;
            cold_tank_kpa_label.Location = new Point(197, 38);
            cold_tank_kpa_label.Name = "cold_tank_kpa_label";
            cold_tank_kpa_label.Size = new Size(169, 15);
            cold_tank_kpa_label.TabIndex = 63;
            cold_tank_kpa_label.Text = "Pressure Range [lower - upper]";
            // 
            // cold_tank_kpa_lower
            // 
            cold_tank_kpa_lower.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            cold_tank_kpa_lower.Location = new Point(197, 56);
            cold_tank_kpa_lower.Maximum = new decimal(new int[] { 3546, 0, 0, 0 });
            cold_tank_kpa_lower.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            cold_tank_kpa_lower.Name = "cold_tank_kpa_lower";
            cold_tank_kpa_lower.Size = new Size(80, 23);
            cold_tank_kpa_lower.TabIndex = 62;
            cold_tank_kpa_lower.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_kpa_lower.Value = new decimal(new int[] { 2533, 0, 0, 0 });
            // 
            // hot_tank_label
            // 
            hot_tank_label.AutoSize = true;
            hot_tank_label.Location = new Point(759, 13);
            hot_tank_label.Name = "hot_tank_label";
            hot_tank_label.Size = new Size(55, 15);
            hot_tank_label.TabIndex = 56;
            hot_tank_label.Text = "Hot Tank";
            // 
            // cold_tank_label
            // 
            cold_tank_label.AutoSize = true;
            cold_tank_label.Location = new Point(535, 13);
            cold_tank_label.Name = "cold_tank_label";
            cold_tank_label.Size = new Size(60, 15);
            cold_tank_label.TabIndex = 52;
            cold_tank_label.Text = "Cold Tank";
            // 
            // hot_tank_add_gas
            // 
            hot_tank_add_gas.Location = new Point(884, 399);
            hot_tank_add_gas.Name = "hot_tank_add_gas";
            hot_tank_add_gas.Size = new Size(75, 23);
            hot_tank_add_gas.TabIndex = 55;
            hot_tank_add_gas.Text = "Add Gas";
            hot_tank_add_gas.UseVisualStyleBackColor = true;
            // 
            // cold_tank_add_gas
            // 
            cold_tank_add_gas.Location = new Point(660, 399);
            cold_tank_add_gas.Name = "cold_tank_add_gas";
            cold_tank_add_gas.Size = new Size(75, 23);
            cold_tank_add_gas.TabIndex = 51;
            cold_tank_add_gas.Text = "Add Gas";
            cold_tank_add_gas.UseVisualStyleBackColor = true;
            // 
            // hot_tank_gas_dropdown
            // 
            hot_tank_gas_dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            hot_tank_gas_dropdown.FormattingEnabled = true;
            hot_tank_gas_dropdown.Location = new Point(759, 399);
            hot_tank_gas_dropdown.Name = "hot_tank_gas_dropdown";
            hot_tank_gas_dropdown.Size = new Size(121, 23);
            hot_tank_gas_dropdown.TabIndex = 54;
            // 
            // hot_tank_panel
            // 
            hot_tank_panel.AutoScroll = true;
            hot_tank_panel.BorderStyle = BorderStyle.FixedSingle;
            hot_tank_panel.FlowDirection = FlowDirection.TopDown;
            hot_tank_panel.Location = new Point(759, 31);
            hot_tank_panel.Name = "hot_tank_panel";
            hot_tank_panel.Size = new Size(200, 362);
            hot_tank_panel.TabIndex = 53;
            hot_tank_panel.WrapContents = false;
            // 
            // cold_tank_gas_dropdown
            // 
            cold_tank_gas_dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            cold_tank_gas_dropdown.FormattingEnabled = true;
            cold_tank_gas_dropdown.Location = new Point(535, 399);
            cold_tank_gas_dropdown.Name = "cold_tank_gas_dropdown";
            cold_tank_gas_dropdown.Size = new Size(121, 23);
            cold_tank_gas_dropdown.TabIndex = 50;
            // 
            // begin_button
            // 
            begin_button.Location = new Point(15, 515);
            begin_button.Name = "begin_button";
            begin_button.Size = new Size(176, 23);
            begin_button.TabIndex = 49;
            begin_button.Text = "Begin";
            begin_button.UseVisualStyleBackColor = true;
            begin_button.Click += begin_button_Click;
            // 
            // cold_tank_panel
            // 
            cold_tank_panel.AutoScroll = true;
            cold_tank_panel.BorderStyle = BorderStyle.FixedSingle;
            cold_tank_panel.FlowDirection = FlowDirection.TopDown;
            cold_tank_panel.Location = new Point(535, 31);
            cold_tank_panel.Name = "cold_tank_panel";
            cold_tank_panel.Size = new Size(200, 362);
            cold_tank_panel.TabIndex = 48;
            cold_tank_panel.WrapContents = false;
            // 
            // highscore
            // 
            highscore.BorderStyle = BorderStyle.FixedSingle;
            highscore.Location = new Point(15, 31);
            highscore.Name = "highscore";
            highscore.ReadOnly = true;
            highscore.Size = new Size(176, 308);
            highscore.TabIndex = 47;
            highscore.Text = "";
            // 
            // highscore_label
            // 
            highscore_label.AutoSize = true;
            highscore_label.Location = new Point(15, 13);
            highscore_label.Name = "highscore_label";
            highscore_label.Size = new Size(61, 15);
            highscore_label.TabIndex = 46;
            highscore_label.Text = "Highscore";
            // 
            // cold_tank_kpa_upper
            // 
            cold_tank_kpa_upper.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            cold_tank_kpa_upper.Location = new Point(320, 56);
            cold_tank_kpa_upper.Maximum = new decimal(new int[] { 3546, 0, 0, 0 });
            cold_tank_kpa_upper.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            cold_tank_kpa_upper.Name = "cold_tank_kpa_upper";
            cold_tank_kpa_upper.Size = new Size(80, 23);
            cold_tank_kpa_upper.TabIndex = 69;
            cold_tank_kpa_upper.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_kpa_upper.Value = new decimal(new int[] { 2533, 0, 0, 0 });
            // 
            // cold_tank_kpa_dash
            // 
            cold_tank_kpa_dash.AutoSize = true;
            cold_tank_kpa_dash.Location = new Point(283, 58);
            cold_tank_kpa_dash.Name = "cold_tank_kpa_dash";
            cold_tank_kpa_dash.Size = new Size(30, 15);
            cold_tank_kpa_dash.TabIndex = 70;
            cold_tank_kpa_dash.Text = "--->";
            // 
            // cold_tank_temperature_dash
            // 
            cold_tank_temperature_dash.AutoSize = true;
            cold_tank_temperature_dash.Location = new Point(283, 102);
            cold_tank_temperature_dash.Name = "cold_tank_temperature_dash";
            cold_tank_temperature_dash.Size = new Size(30, 15);
            cold_tank_temperature_dash.TabIndex = 74;
            cold_tank_temperature_dash.Text = "--->";
            // 
            // cold_tank_temperature_upper
            // 
            cold_tank_temperature_upper.DecimalPlaces = 2;
            cold_tank_temperature_upper.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            cold_tank_temperature_upper.Location = new Point(320, 100);
            cold_tank_temperature_upper.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            cold_tank_temperature_upper.Minimum = new decimal(new int[] { 27, 0, 0, 65536 });
            cold_tank_temperature_upper.Name = "cold_tank_temperature_upper";
            cold_tank_temperature_upper.Size = new Size(80, 23);
            cold_tank_temperature_upper.TabIndex = 73;
            cold_tank_temperature_upper.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_temperature_upper.Value = new decimal(new int[] { 4315, 0, 0, 131072 });
            // 
            // cold_tank_temperature_label
            // 
            cold_tank_temperature_label.AutoSize = true;
            cold_tank_temperature_label.Location = new Point(197, 82);
            cold_tank_temperature_label.Name = "cold_tank_temperature_label";
            cold_tank_temperature_label.Size = new Size(192, 15);
            cold_tank_temperature_label.TabIndex = 72;
            cold_tank_temperature_label.Text = "Temperature Range [lower - upper]";
            // 
            // cold_tank_temperature_lower
            // 
            cold_tank_temperature_lower.DecimalPlaces = 2;
            cold_tank_temperature_lower.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            cold_tank_temperature_lower.Location = new Point(197, 100);
            cold_tank_temperature_lower.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            cold_tank_temperature_lower.Minimum = new decimal(new int[] { 27, 0, 0, 65536 });
            cold_tank_temperature_lower.Name = "cold_tank_temperature_lower";
            cold_tank_temperature_lower.Size = new Size(80, 23);
            cold_tank_temperature_lower.TabIndex = 71;
            cold_tank_temperature_lower.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_temperature_lower.Value = new decimal(new int[] { 27, 0, 0, 65536 });
            // 
            // cold_tank_pressure_step_size_label
            // 
            cold_tank_pressure_step_size_label.AutoSize = true;
            cold_tank_pressure_step_size_label.Location = new Point(414, 38);
            cold_tank_pressure_step_size_label.Name = "cold_tank_pressure_step_size_label";
            cold_tank_pressure_step_size_label.Size = new Size(53, 15);
            cold_tank_pressure_step_size_label.TabIndex = 76;
            cold_tank_pressure_step_size_label.Text = "Step Size";
            // 
            // cold_tank_pressure_step_size
            // 
            cold_tank_pressure_step_size.DecimalPlaces = 1;
            cold_tank_pressure_step_size.Location = new Point(414, 56);
            cold_tank_pressure_step_size.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            cold_tank_pressure_step_size.Name = "cold_tank_pressure_step_size";
            cold_tank_pressure_step_size.Size = new Size(80, 23);
            cold_tank_pressure_step_size.TabIndex = 75;
            cold_tank_pressure_step_size.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_pressure_step_size.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // hot_tank_temperature_dash
            // 
            hot_tank_temperature_dash.AutoSize = true;
            hot_tank_temperature_dash.Location = new Point(283, 274);
            hot_tank_temperature_dash.Name = "hot_tank_temperature_dash";
            hot_tank_temperature_dash.Size = new Size(30, 15);
            hot_tank_temperature_dash.TabIndex = 85;
            hot_tank_temperature_dash.Text = "--->";
            // 
            // hot_tank_temperature_upper
            // 
            hot_tank_temperature_upper.DecimalPlaces = 2;
            hot_tank_temperature_upper.Increment = new decimal(new int[] { 10000, 0, 0, 0 });
            hot_tank_temperature_upper.Location = new Point(320, 272);
            hot_tank_temperature_upper.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            hot_tank_temperature_upper.Minimum = new decimal(new int[] { 27, 0, 0, 65536 });
            hot_tank_temperature_upper.Name = "hot_tank_temperature_upper";
            hot_tank_temperature_upper.Size = new Size(80, 23);
            hot_tank_temperature_upper.TabIndex = 84;
            hot_tank_temperature_upper.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_temperature_upper.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // hot_tank_temperature_label
            // 
            hot_tank_temperature_label.AutoSize = true;
            hot_tank_temperature_label.Location = new Point(197, 254);
            hot_tank_temperature_label.Name = "hot_tank_temperature_label";
            hot_tank_temperature_label.Size = new Size(192, 15);
            hot_tank_temperature_label.TabIndex = 83;
            hot_tank_temperature_label.Text = "Temperature Range [lower - upper]";
            // 
            // hot_tank_temperature_lower
            // 
            hot_tank_temperature_lower.DecimalPlaces = 2;
            hot_tank_temperature_lower.Increment = new decimal(new int[] { 10000, 0, 0, 0 });
            hot_tank_temperature_lower.Location = new Point(197, 272);
            hot_tank_temperature_lower.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            hot_tank_temperature_lower.Minimum = new decimal(new int[] { 27, 0, 0, 65536 });
            hot_tank_temperature_lower.Name = "hot_tank_temperature_lower";
            hot_tank_temperature_lower.Size = new Size(80, 23);
            hot_tank_temperature_lower.TabIndex = 82;
            hot_tank_temperature_lower.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_temperature_lower.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // hot_tank_kpa_dash
            // 
            hot_tank_kpa_dash.AutoSize = true;
            hot_tank_kpa_dash.Location = new Point(283, 230);
            hot_tank_kpa_dash.Name = "hot_tank_kpa_dash";
            hot_tank_kpa_dash.Size = new Size(30, 15);
            hot_tank_kpa_dash.TabIndex = 81;
            hot_tank_kpa_dash.Text = "--->";
            // 
            // hot_tank_kpa_upper
            // 
            hot_tank_kpa_upper.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            hot_tank_kpa_upper.Location = new Point(320, 228);
            hot_tank_kpa_upper.Maximum = new decimal(new int[] { 3546, 0, 0, 0 });
            hot_tank_kpa_upper.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            hot_tank_kpa_upper.Name = "hot_tank_kpa_upper";
            hot_tank_kpa_upper.Size = new Size(80, 23);
            hot_tank_kpa_upper.TabIndex = 80;
            hot_tank_kpa_upper.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_kpa_upper.Value = new decimal(new int[] { 2533, 0, 0, 0 });
            // 
            // hot_tank_kpa_label
            // 
            hot_tank_kpa_label.AutoSize = true;
            hot_tank_kpa_label.Location = new Point(197, 210);
            hot_tank_kpa_label.Name = "hot_tank_kpa_label";
            hot_tank_kpa_label.Size = new Size(169, 15);
            hot_tank_kpa_label.TabIndex = 79;
            hot_tank_kpa_label.Text = "Pressure Range [lower - upper]";
            // 
            // hot_tank_kpa_lower
            // 
            hot_tank_kpa_lower.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            hot_tank_kpa_lower.Location = new Point(197, 228);
            hot_tank_kpa_lower.Maximum = new decimal(new int[] { 3546, 0, 0, 0 });
            hot_tank_kpa_lower.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            hot_tank_kpa_lower.Name = "hot_tank_kpa_lower";
            hot_tank_kpa_lower.Size = new Size(80, 23);
            hot_tank_kpa_lower.TabIndex = 78;
            hot_tank_kpa_lower.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_kpa_lower.Value = new decimal(new int[] { 2533, 0, 0, 0 });
            // 
            // end_button
            // 
            end_button.Enabled = false;
            end_button.Location = new Point(15, 544);
            end_button.Name = "end_button";
            end_button.Size = new Size(176, 23);
            end_button.TabIndex = 89;
            end_button.Text = "End";
            end_button.UseVisualStyleBackColor = true;
            end_button.Click += end_button_Click;
            // 
            // unpause_button
            // 
            unpause_button.Enabled = false;
            unpause_button.Location = new Point(197, 543);
            unpause_button.Name = "unpause_button";
            unpause_button.Size = new Size(176, 23);
            unpause_button.TabIndex = 91;
            unpause_button.Text = "Unpause";
            unpause_button.UseVisualStyleBackColor = true;
            unpause_button.Click += unpause_button_Click;
            // 
            // pause_button
            // 
            pause_button.Enabled = false;
            pause_button.Location = new Point(197, 514);
            pause_button.Name = "pause_button";
            pause_button.Size = new Size(176, 23);
            pause_button.TabIndex = 90;
            pause_button.Text = "Pause";
            pause_button.UseVisualStyleBackColor = true;
            pause_button.Click += pause_button_Click;
            // 
            // cold_tank_temperature_step_size_label
            // 
            cold_tank_temperature_step_size_label.AutoSize = true;
            cold_tank_temperature_step_size_label.Location = new Point(414, 82);
            cold_tank_temperature_step_size_label.Name = "cold_tank_temperature_step_size_label";
            cold_tank_temperature_step_size_label.Size = new Size(53, 15);
            cold_tank_temperature_step_size_label.TabIndex = 93;
            cold_tank_temperature_step_size_label.Text = "Step Size";
            // 
            // cold_tank_temperature_step_size
            // 
            cold_tank_temperature_step_size.DecimalPlaces = 1;
            cold_tank_temperature_step_size.Location = new Point(414, 100);
            cold_tank_temperature_step_size.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            cold_tank_temperature_step_size.Name = "cold_tank_temperature_step_size";
            cold_tank_temperature_step_size.Size = new Size(80, 23);
            cold_tank_temperature_step_size.TabIndex = 92;
            cold_tank_temperature_step_size.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_temperature_step_size.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // hot_tank_temperature_step_size_label
            // 
            hot_tank_temperature_step_size_label.AutoSize = true;
            hot_tank_temperature_step_size_label.Location = new Point(414, 254);
            hot_tank_temperature_step_size_label.Name = "hot_tank_temperature_step_size_label";
            hot_tank_temperature_step_size_label.Size = new Size(53, 15);
            hot_tank_temperature_step_size_label.TabIndex = 97;
            hot_tank_temperature_step_size_label.Text = "Step Size";
            // 
            // hot_tank_temperature_step_size
            // 
            hot_tank_temperature_step_size.DecimalPlaces = 1;
            hot_tank_temperature_step_size.Location = new Point(414, 272);
            hot_tank_temperature_step_size.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            hot_tank_temperature_step_size.Name = "hot_tank_temperature_step_size";
            hot_tank_temperature_step_size.Size = new Size(80, 23);
            hot_tank_temperature_step_size.TabIndex = 96;
            hot_tank_temperature_step_size.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_temperature_step_size.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // hot_tank_pressure_step_size_label
            // 
            hot_tank_pressure_step_size_label.AutoSize = true;
            hot_tank_pressure_step_size_label.Location = new Point(414, 210);
            hot_tank_pressure_step_size_label.Name = "hot_tank_pressure_step_size_label";
            hot_tank_pressure_step_size_label.Size = new Size(53, 15);
            hot_tank_pressure_step_size_label.TabIndex = 95;
            hot_tank_pressure_step_size_label.Text = "Step Size";
            // 
            // hot_tank_pressure_step_size
            // 
            hot_tank_pressure_step_size.DecimalPlaces = 1;
            hot_tank_pressure_step_size.Location = new Point(414, 228);
            hot_tank_pressure_step_size.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            hot_tank_pressure_step_size.Name = "hot_tank_pressure_step_size";
            hot_tank_pressure_step_size.Size = new Size(80, 23);
            hot_tank_pressure_step_size.TabIndex = 94;
            hot_tank_pressure_step_size.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_pressure_step_size.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cold_tank_gas_percent_step_size_label
            // 
            cold_tank_gas_percent_step_size_label.AutoSize = true;
            cold_tank_gas_percent_step_size_label.Location = new Point(197, 126);
            cold_tank_gas_percent_step_size_label.Name = "cold_tank_gas_percent_step_size_label";
            cold_tank_gas_percent_step_size_label.Size = new Size(118, 15);
            cold_tank_gas_percent_step_size_label.TabIndex = 99;
            cold_tank_gas_percent_step_size_label.Text = "Gas Percent Step Size";
            // 
            // cold_tank_gas_percent_step_size
            // 
            cold_tank_gas_percent_step_size.Location = new Point(197, 144);
            cold_tank_gas_percent_step_size.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            cold_tank_gas_percent_step_size.Name = "cold_tank_gas_percent_step_size";
            cold_tank_gas_percent_step_size.Size = new Size(80, 23);
            cold_tank_gas_percent_step_size.TabIndex = 98;
            gas_percent_step_size_tooltip.SetToolTip(cold_tank_gas_percent_step_size, "For example: (100% Oxygen 0% Tritium), (99% Oxygen 1% Tritium), ..., (0% Oxygen 100% Tritium)");
            cold_tank_gas_percent_step_size.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_gas_percent_step_size.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // hot_tank_gas_percent_step_size
            // 
            hot_tank_gas_percent_step_size.Location = new Point(197, 316);
            hot_tank_gas_percent_step_size.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            hot_tank_gas_percent_step_size.Name = "hot_tank_gas_percent_step_size";
            hot_tank_gas_percent_step_size.Size = new Size(80, 23);
            hot_tank_gas_percent_step_size.TabIndex = 100;
            gas_percent_step_size_tooltip.SetToolTip(hot_tank_gas_percent_step_size, "For example: (100% Oxygen 0% Tritium), (99% Oxygen 1% Tritium), ..., (0% Oxygen 100% Tritium)");
            hot_tank_gas_percent_step_size.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_gas_percent_step_size.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // hot_tank_gas_percent_step_size_label
            // 
            hot_tank_gas_percent_step_size_label.AutoSize = true;
            hot_tank_gas_percent_step_size_label.Location = new Point(197, 298);
            hot_tank_gas_percent_step_size_label.Name = "hot_tank_gas_percent_step_size_label";
            hot_tank_gas_percent_step_size_label.Size = new Size(118, 15);
            hot_tank_gas_percent_step_size_label.TabIndex = 101;
            hot_tank_gas_percent_step_size_label.Text = "Gas Percent Step Size";
            // 
            // hot_tank_percent_label
            // 
            hot_tank_percent_label.AutoSize = true;
            hot_tank_percent_label.Location = new Point(280, 318);
            hot_tank_percent_label.Name = "hot_tank_percent_label";
            hot_tank_percent_label.Size = new Size(17, 15);
            hot_tank_percent_label.TabIndex = 102;
            hot_tank_percent_label.Text = "%";
            // 
            // cold_tank_percent_label
            // 
            cold_tank_percent_label.AutoSize = true;
            cold_tank_percent_label.Location = new Point(280, 146);
            cold_tank_percent_label.Name = "cold_tank_percent_label";
            cold_tank_percent_label.Size = new Size(17, 15);
            cold_tank_percent_label.TabIndex = 103;
            cold_tank_percent_label.Text = "%";
            // 
            // status_log
            // 
            status_log.BorderStyle = BorderStyle.FixedSingle;
            status_log.Location = new Point(15, 360);
            status_log.Name = "status_log";
            status_log.ReadOnly = true;
            status_log.Size = new Size(176, 149);
            status_log.TabIndex = 105;
            status_log.Text = "";
            // 
            // status_log_label
            // 
            status_log_label.AutoSize = true;
            status_log_label.Location = new Point(15, 342);
            status_log_label.Name = "status_log_label";
            status_log_label.Size = new Size(62, 15);
            status_log_label.TabIndex = 104;
            status_log_label.Text = "Status Log";
            // 
            // BruteForceControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(status_log);
            Controls.Add(status_log_label);
            Controls.Add(cold_tank_percent_label);
            Controls.Add(hot_tank_percent_label);
            Controls.Add(hot_tank_gas_percent_step_size_label);
            Controls.Add(hot_tank_gas_percent_step_size);
            Controls.Add(cold_tank_gas_percent_step_size_label);
            Controls.Add(cold_tank_gas_percent_step_size);
            Controls.Add(hot_tank_temperature_step_size_label);
            Controls.Add(hot_tank_temperature_step_size);
            Controls.Add(hot_tank_pressure_step_size_label);
            Controls.Add(hot_tank_pressure_step_size);
            Controls.Add(cold_tank_temperature_step_size_label);
            Controls.Add(cold_tank_temperature_step_size);
            Controls.Add(unpause_button);
            Controls.Add(pause_button);
            Controls.Add(end_button);
            Controls.Add(hot_tank_temperature_dash);
            Controls.Add(hot_tank_temperature_upper);
            Controls.Add(hot_tank_temperature_label);
            Controls.Add(hot_tank_temperature_lower);
            Controls.Add(hot_tank_kpa_dash);
            Controls.Add(hot_tank_kpa_upper);
            Controls.Add(hot_tank_kpa_label);
            Controls.Add(hot_tank_kpa_lower);
            Controls.Add(cold_tank_pressure_step_size_label);
            Controls.Add(cold_tank_pressure_step_size);
            Controls.Add(cold_tank_temperature_dash);
            Controls.Add(cold_tank_temperature_upper);
            Controls.Add(cold_tank_temperature_label);
            Controls.Add(cold_tank_temperature_lower);
            Controls.Add(cold_tank_kpa_dash);
            Controls.Add(cold_tank_kpa_upper);
            Controls.Add(hot_tank_options_label);
            Controls.Add(cold_tank_options_label);
            Controls.Add(cold_tank_kpa_label);
            Controls.Add(cold_tank_kpa_lower);
            Controls.Add(hot_tank_label);
            Controls.Add(cold_tank_label);
            Controls.Add(hot_tank_add_gas);
            Controls.Add(cold_tank_add_gas);
            Controls.Add(hot_tank_gas_dropdown);
            Controls.Add(hot_tank_panel);
            Controls.Add(cold_tank_gas_dropdown);
            Controls.Add(begin_button);
            Controls.Add(cold_tank_panel);
            Controls.Add(highscore);
            Controls.Add(highscore_label);
            Name = "BruteForceControl";
            Size = new Size(979, 577);
            ((System.ComponentModel.ISupportInitialize)cold_tank_kpa_lower).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_kpa_upper).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature_upper).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature_lower).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_pressure_step_size).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature_upper).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature_lower).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_kpa_upper).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_kpa_lower).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature_step_size).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature_step_size).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_pressure_step_size).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_gas_percent_step_size).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_gas_percent_step_size).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label hot_tank_options_label;
		private Label cold_tank_options_label;
		private Label cold_tank_kpa_label;
		private NumericUpDown cold_tank_kpa_lower;
		private Label hot_tank_temperature_label;
		private Label hot_tank_label;
		private Label cold_tank_label;
		private Button hot_tank_add_gas;
		private Button cold_tank_add_gas;
		private ComboBox hot_tank_gas_dropdown;
		private FlowLayoutPanel hot_tank_panel;
		private ComboBox cold_tank_gas_dropdown;
		private Button begin_button;
		private FlowLayoutPanel cold_tank_panel;
		private RichTextBox highscore;
		private Label highscore_label;
		private NumericUpDown cold_tank_kpa_upper;
		private Label cold_tank_kpa_dash;
		private Label cold_tank_temperature_dash;
		private NumericUpDown cold_tank_temperature_upper;
		private Label cold_tank_temperature_label;
		private NumericUpDown cold_tank_temperature_lower;
		private Label cold_tank_pressure_step_size_label;
		private NumericUpDown cold_tank_pressure_step_size;
		private Label hot_tank_temperature_dash;
		private NumericUpDown hot_tank_temperature_upper;
		private NumericUpDown hot_tank_temperature_lower;
		private Label hot_tank_kpa_dash;
		private NumericUpDown hot_tank_kpa_upper;
		private Label hot_tank_kpa_label;
		private NumericUpDown hot_tank_kpa_lower;
		private Button end_button;
		private Button unpause_button;
		private Button pause_button;
		private Label cold_tank_temperature_step_size_label;
		private NumericUpDown cold_tank_temperature_step_size;
		private Label hot_tank_temperature_step_size_label;
		private NumericUpDown hot_tank_temperature_step_size;
		private Label hot_tank_pressure_step_size_label;
		private NumericUpDown hot_tank_pressure_step_size;
		private Label cold_tank_gas_percent_step_size_label;
		private NumericUpDown cold_tank_gas_percent_step_size;
		private ToolTip gas_percent_step_size_tooltip;
		private Label hot_tank_gas_percent_step_size_label;
		private NumericUpDown hot_tank_gas_percent_step_size;
		private Label hot_tank_percent_label;
		private Label cold_tank_percent_label;
        private RichTextBox status_log;
        private Label status_log_label;
    }
}
