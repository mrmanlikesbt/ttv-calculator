namespace TTV_Calculator.Code.UserControls
{
    partial class CalculatorControl
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
            hot_tank_options_label = new Label();
            hot_tank_kpa_label = new Label();
            hot_tank_kpa = new NumericUpDown();
            hot_tank_percent_instead_of_moles_checkbox = new CheckBox();
            cold_tank_options_label = new Label();
            cold_tank_kpa_label = new Label();
            cold_tank_kpa = new NumericUpDown();
            cold_tank_percent_instead_of_moles_checkbox = new CheckBox();
            hot_tank_temperature_label = new Label();
            hot_tank_temperature = new NumericUpDown();
            cold_tank_temperature_label = new Label();
            cold_tank_temperature = new NumericUpDown();
            hot_tank_label = new Label();
            cold_tank_label = new Label();
            hot_tank_add_gas = new Button();
            cold_tank_add_gas = new Button();
            hot_tank_gas_dropdown = new ComboBox();
            hot_tank_panel = new FlowLayoutPanel();
            cold_tank_gas_dropdown = new ComboBox();
            calculate_button = new Button();
            cold_tank_panel = new FlowLayoutPanel();
            calculation_results = new RichTextBox();
            calculation_results_label = new Label();
            ((System.ComponentModel.ISupportInitialize)hot_tank_kpa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_kpa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature).BeginInit();
            SuspendLayout();
            // 
            // hot_tank_options_label
            // 
            hot_tank_options_label.AutoSize = true;
            hot_tank_options_label.Location = new Point(194, 110);
            hot_tank_options_label.Name = "hot_tank_options_label";
            hot_tank_options_label.Size = new Size(100, 15);
            hot_tank_options_label.TabIndex = 45;
            hot_tank_options_label.Text = "Hot Tank Options";
            // 
            // hot_tank_kpa_label
            // 
            hot_tank_kpa_label.AutoSize = true;
            hot_tank_kpa_label.Location = new Point(261, 155);
            hot_tank_kpa_label.Name = "hot_tank_kpa_label";
            hot_tank_kpa_label.Size = new Size(54, 15);
            hot_tank_kpa_label.TabIndex = 44;
            hot_tank_kpa_label.Text = "Tank kPa";
            // 
            // hot_tank_kpa
            // 
            hot_tank_kpa.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            hot_tank_kpa.Location = new Point(194, 153);
            hot_tank_kpa.Maximum = new decimal(new int[] { 3546, 0, 0, 0 });
            hot_tank_kpa.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            hot_tank_kpa.Name = "hot_tank_kpa";
            hot_tank_kpa.Size = new Size(61, 23);
            hot_tank_kpa.TabIndex = 43;
            hot_tank_kpa.UpDownAlign = LeftRightAlignment.Left;
            hot_tank_kpa.Value = new decimal(new int[] { 2533, 0, 0, 0 });
            // 
            // hot_tank_percent_instead_of_moles_checkbox
            // 
            hot_tank_percent_instead_of_moles_checkbox.AutoSize = true;
            hot_tank_percent_instead_of_moles_checkbox.Location = new Point(194, 128);
            hot_tank_percent_instead_of_moles_checkbox.Name = "hot_tank_percent_instead_of_moles_checkbox";
            hot_tank_percent_instead_of_moles_checkbox.Size = new Size(180, 19);
            hot_tank_percent_instead_of_moles_checkbox.TabIndex = 42;
            hot_tank_percent_instead_of_moles_checkbox.Text = "Percentages Instead of Moles";
            hot_tank_percent_instead_of_moles_checkbox.UseVisualStyleBackColor = true;
            // 
            // cold_tank_options_label
            // 
            cold_tank_options_label.AutoSize = true;
            cold_tank_options_label.Location = new Point(194, 12);
            cold_tank_options_label.Name = "cold_tank_options_label";
            cold_tank_options_label.Size = new Size(105, 15);
            cold_tank_options_label.TabIndex = 41;
            cold_tank_options_label.Text = "Cold Tank Options";
            // 
            // cold_tank_kpa_label
            // 
            cold_tank_kpa_label.AutoSize = true;
            cold_tank_kpa_label.Location = new Point(261, 57);
            cold_tank_kpa_label.Name = "cold_tank_kpa_label";
            cold_tank_kpa_label.Size = new Size(54, 15);
            cold_tank_kpa_label.TabIndex = 40;
            cold_tank_kpa_label.Text = "Tank kPa";
            // 
            // cold_tank_kpa
            // 
            cold_tank_kpa.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            cold_tank_kpa.Location = new Point(194, 55);
            cold_tank_kpa.Maximum = new decimal(new int[] { 3546, 0, 0, 0 });
            cold_tank_kpa.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            cold_tank_kpa.Name = "cold_tank_kpa";
            cold_tank_kpa.Size = new Size(61, 23);
            cold_tank_kpa.TabIndex = 39;
            cold_tank_kpa.UpDownAlign = LeftRightAlignment.Left;
            cold_tank_kpa.Value = new decimal(new int[] { 2533, 0, 0, 0 });
            // 
            // cold_tank_percent_instead_of_moles_checkbox
            // 
            cold_tank_percent_instead_of_moles_checkbox.AutoSize = true;
            cold_tank_percent_instead_of_moles_checkbox.Location = new Point(194, 30);
            cold_tank_percent_instead_of_moles_checkbox.Name = "cold_tank_percent_instead_of_moles_checkbox";
            cold_tank_percent_instead_of_moles_checkbox.Size = new Size(180, 19);
            cold_tank_percent_instead_of_moles_checkbox.TabIndex = 38;
            cold_tank_percent_instead_of_moles_checkbox.Text = "Percentages Instead of Moles";
            cold_tank_percent_instead_of_moles_checkbox.UseVisualStyleBackColor = true;
            // 
            // hot_tank_temperature_label
            // 
            hot_tank_temperature_label.AutoSize = true;
            hot_tank_temperature_label.Location = new Point(591, 429);
            hot_tank_temperature_label.Name = "hot_tank_temperature_label";
            hot_tank_temperature_label.Size = new Size(74, 15);
            hot_tank_temperature_label.TabIndex = 37;
            hot_tank_temperature_label.Text = "Temperature";
            // 
            // hot_tank_temperature
            // 
            hot_tank_temperature.DecimalPlaces = 2;
            hot_tank_temperature.Increment = new decimal(new int[] { 10000, 0, 0, 0 });
            hot_tank_temperature.Location = new Point(671, 425);
            hot_tank_temperature.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            hot_tank_temperature.Minimum = new decimal(new int[] { 27, 0, 0, 65536 });
            hot_tank_temperature.Name = "hot_tank_temperature";
            hot_tank_temperature.Size = new Size(120, 23);
            hot_tank_temperature.TabIndex = 36;
            hot_tank_temperature.Value = new decimal(new int[] { 27, 0, 0, 65536 });
            // 
            // cold_tank_temperature_label
            // 
            cold_tank_temperature_label.AutoSize = true;
            cold_tank_temperature_label.Location = new Point(385, 429);
            cold_tank_temperature_label.Name = "cold_tank_temperature_label";
            cold_tank_temperature_label.Size = new Size(74, 15);
            cold_tank_temperature_label.TabIndex = 35;
            cold_tank_temperature_label.Text = "Temperature";
            // 
            // cold_tank_temperature
            // 
            cold_tank_temperature.DecimalPlaces = 2;
            cold_tank_temperature.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            cold_tank_temperature.Location = new Point(465, 425);
            cold_tank_temperature.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            cold_tank_temperature.Minimum = new decimal(new int[] { 27, 0, 0, 65536 });
            cold_tank_temperature.Name = "cold_tank_temperature";
            cold_tank_temperature.Size = new Size(120, 23);
            cold_tank_temperature.TabIndex = 34;
            cold_tank_temperature.Value = new decimal(new int[] { 27, 0, 0, 65536 });
            // 
            // hot_tank_label
            // 
            hot_tank_label.AutoSize = true;
            hot_tank_label.Location = new Point(591, 12);
            hot_tank_label.Name = "hot_tank_label";
            hot_tank_label.Size = new Size(55, 15);
            hot_tank_label.TabIndex = 33;
            hot_tank_label.Text = "Hot Tank";
            // 
            // cold_tank_label
            // 
            cold_tank_label.AutoSize = true;
            cold_tank_label.Location = new Point(385, 12);
            cold_tank_label.Name = "cold_tank_label";
            cold_tank_label.Size = new Size(60, 15);
            cold_tank_label.TabIndex = 29;
            cold_tank_label.Text = "Cold Tank";
            // 
            // hot_tank_add_gas
            // 
            hot_tank_add_gas.Location = new Point(716, 398);
            hot_tank_add_gas.Name = "hot_tank_add_gas";
            hot_tank_add_gas.Size = new Size(75, 23);
            hot_tank_add_gas.TabIndex = 32;
            hot_tank_add_gas.Text = "Add Gas";
            hot_tank_add_gas.UseVisualStyleBackColor = true;
            // 
            // cold_tank_add_gas
            // 
            cold_tank_add_gas.Location = new Point(510, 398);
            cold_tank_add_gas.Name = "cold_tank_add_gas";
            cold_tank_add_gas.Size = new Size(75, 23);
            cold_tank_add_gas.TabIndex = 28;
            cold_tank_add_gas.Text = "Add Gas";
            cold_tank_add_gas.UseVisualStyleBackColor = true;
            // 
            // hot_tank_gas_dropdown
            // 
            hot_tank_gas_dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            hot_tank_gas_dropdown.FormattingEnabled = true;
            hot_tank_gas_dropdown.Location = new Point(591, 398);
            hot_tank_gas_dropdown.Name = "hot_tank_gas_dropdown";
            hot_tank_gas_dropdown.Size = new Size(121, 23);
            hot_tank_gas_dropdown.TabIndex = 31;
            // 
            // hot_tank_panel
            // 
            hot_tank_panel.AutoScroll = true;
            hot_tank_panel.BorderStyle = BorderStyle.FixedSingle;
            hot_tank_panel.FlowDirection = FlowDirection.TopDown;
            hot_tank_panel.Location = new Point(591, 30);
            hot_tank_panel.Name = "hot_tank_panel";
            hot_tank_panel.Size = new Size(200, 362);
            hot_tank_panel.TabIndex = 30;
            hot_tank_panel.WrapContents = false;
            // 
            // cold_tank_gas_dropdown
            // 
            cold_tank_gas_dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            cold_tank_gas_dropdown.FormattingEnabled = true;
            cold_tank_gas_dropdown.Location = new Point(385, 398);
            cold_tank_gas_dropdown.Name = "cold_tank_gas_dropdown";
            cold_tank_gas_dropdown.Size = new Size(121, 23);
            cold_tank_gas_dropdown.TabIndex = 27;
            // 
            // calculate_button
            // 
            calculate_button.Location = new Point(12, 425);
            calculate_button.Name = "calculate_button";
            calculate_button.Size = new Size(176, 23);
            calculate_button.TabIndex = 26;
            calculate_button.Text = "Calculate";
            calculate_button.UseVisualStyleBackColor = true;
            calculate_button.Click += Calculate;
            // 
            // cold_tank_panel
            // 
            cold_tank_panel.AutoScroll = true;
            cold_tank_panel.BorderStyle = BorderStyle.FixedSingle;
            cold_tank_panel.FlowDirection = FlowDirection.TopDown;
            cold_tank_panel.Location = new Point(385, 30);
            cold_tank_panel.Name = "cold_tank_panel";
            cold_tank_panel.Size = new Size(200, 362);
            cold_tank_panel.TabIndex = 25;
            cold_tank_panel.WrapContents = false;
            // 
            // calculation_results
            // 
            calculation_results.BorderStyle = BorderStyle.FixedSingle;
            calculation_results.Location = new Point(12, 30);
            calculation_results.Name = "calculation_results";
            calculation_results.ReadOnly = true;
            calculation_results.Size = new Size(176, 389);
            calculation_results.TabIndex = 24;
            calculation_results.Text = "";
            // 
            // calculation_results_label
            // 
            calculation_results_label.AutoSize = true;
            calculation_results_label.Location = new Point(12, 12);
            calculation_results_label.Name = "calculation_results_label";
            calculation_results_label.Size = new Size(107, 15);
            calculation_results_label.TabIndex = 23;
            calculation_results_label.Text = "Calculation Results";
            // 
            // CalculatorControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(hot_tank_options_label);
            Controls.Add(hot_tank_kpa_label);
            Controls.Add(hot_tank_kpa);
            Controls.Add(hot_tank_percent_instead_of_moles_checkbox);
            Controls.Add(cold_tank_options_label);
            Controls.Add(cold_tank_kpa_label);
            Controls.Add(cold_tank_kpa);
            Controls.Add(cold_tank_percent_instead_of_moles_checkbox);
            Controls.Add(hot_tank_temperature_label);
            Controls.Add(hot_tank_temperature);
            Controls.Add(cold_tank_temperature_label);
            Controls.Add(cold_tank_temperature);
            Controls.Add(hot_tank_label);
            Controls.Add(cold_tank_label);
            Controls.Add(hot_tank_add_gas);
            Controls.Add(cold_tank_add_gas);
            Controls.Add(hot_tank_gas_dropdown);
            Controls.Add(hot_tank_panel);
            Controls.Add(cold_tank_gas_dropdown);
            Controls.Add(calculate_button);
            Controls.Add(cold_tank_panel);
            Controls.Add(calculation_results);
            Controls.Add(calculation_results_label);
            Name = "CalculatorControl";
            Size = new Size(809, 463);
            ((System.ComponentModel.ISupportInitialize)hot_tank_kpa).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_kpa).EndInit();
            ((System.ComponentModel.ISupportInitialize)hot_tank_temperature).EndInit();
            ((System.ComponentModel.ISupportInitialize)cold_tank_temperature).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label hot_tank_options_label;
        private Label hot_tank_kpa_label;
        private NumericUpDown hot_tank_kpa;
        private CheckBox hot_tank_percent_instead_of_moles_checkbox;
        private Label cold_tank_options_label;
        private Label cold_tank_kpa_label;
        private NumericUpDown cold_tank_kpa;
        private CheckBox cold_tank_percent_instead_of_moles_checkbox;
        private Label hot_tank_temperature_label;
        private NumericUpDown hot_tank_temperature;
        private Label cold_tank_temperature_label;
        private NumericUpDown cold_tank_temperature;
        private Label hot_tank_label;
        private Label cold_tank_label;
        private Button hot_tank_add_gas;
        private Button cold_tank_add_gas;
        private ComboBox hot_tank_gas_dropdown;
        private FlowLayoutPanel hot_tank_panel;
        private ComboBox cold_tank_gas_dropdown;
        private Button calculate_button;
        private FlowLayoutPanel cold_tank_panel;
        private RichTextBox calculation_results;
        private Label calculation_results_label;
    }
}
