using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TTV_Calculator
{
    public partial class ToxinsCalculator : Form
    {
        /// <summary>
        /// exclusively for the cold tank gas dropdown to avoid adding the same gas twice
        /// </summary>
        private readonly BindingList<GasType> availableColdTankGases = new(Enum.GetValues(typeof(GasType)).Cast<GasType>().ToList());
        private readonly BindingList<GasType> availableHotTankGases = new(Enum.GetValues(typeof(GasType)).Cast<GasType>().ToList());

        private bool coldTankUsePercentagesInsteadOfMoles = false;
        private bool hotTankUsePercentagesInsteadOfMoles = false;
        public ToxinsCalculator()
        {
            InitializeComponent();
            SetupUI();
            GetDefaultValues();
        }

        private void SetupUI()
        {   
            // Cold tank pressure checkbox
            cold_tank_percent_instead_of_moles_checkbox.CheckedChanged += (s, e) =>
            {
                coldTankUsePercentagesInsteadOfMoles = cold_tank_percent_instead_of_moles_checkbox.Checked;
                cold_tank_kpa.Enabled = coldTankUsePercentagesInsteadOfMoles;

                ConvertNumericValues(cold_tank_panel, (float)cold_tank_temperature.Value, (float)cold_tank_kpa.Value, coldTankUsePercentagesInsteadOfMoles);
                UpdateUnitLabels(cold_tank_panel, coldTankUsePercentagesInsteadOfMoles);
            };

            // Hot tank pressure checkbox
            hot_tank_percent_instead_of_moles_checkbox.CheckedChanged += (s, e) =>
            {
                hotTankUsePercentagesInsteadOfMoles = hot_tank_percent_instead_of_moles_checkbox.Checked;
                hot_tank_kpa.Enabled = hotTankUsePercentagesInsteadOfMoles;

                ConvertNumericValues(hot_tank_panel, (float)hot_tank_temperature.Value, (float)hot_tank_kpa.Value, hotTankUsePercentagesInsteadOfMoles);
                UpdateUnitLabels(hot_tank_panel, hotTankUsePercentagesInsteadOfMoles);
            };

            // Cold tank dropdown
            cold_tank_gas_dropdown.DataSource = availableColdTankGases;
            cold_tank_gas_dropdown.Format += (s, e) =>
            {
                if (e.ListItem is GasType gasType)
                    e.Value = GasLibrary.Gases[gasType].Name;
            };
            cold_tank_add_gas.Click += (s, e) =>
            {
                if (cold_tank_gas_dropdown.SelectedItem is GasType selectedGasType)
                {
                    AddGasRow(selectedGasType, cold_tank_panel, availableColdTankGases, coldTankUsePercentagesInsteadOfMoles);
                    availableColdTankGases.Remove(selectedGasType);
                }
            };

            // Hot tank dropdown
            hot_tank_gas_dropdown.DataSource = availableHotTankGases;
            hot_tank_gas_dropdown.Format += (s, e) =>
            {
                if (e.ListItem is GasType gasType)
                    e.Value = GasLibrary.Gases[gasType].Name;
            };
            hot_tank_add_gas.Click += (s, e) =>
            {
                if (hot_tank_gas_dropdown.SelectedItem is GasType selectedGasType)
                {
                    AddGasRow(selectedGasType, hot_tank_panel, availableHotTankGases, hotTankUsePercentagesInsteadOfMoles);
                    availableHotTankGases.Remove(selectedGasType);
                }
            };

            // Hook resizing fix
            cold_tank_panel.SizeChanged += (s, e) => ResizeGasRows(cold_tank_panel);
            hot_tank_panel.SizeChanged += (s, e) => ResizeGasRows(hot_tank_panel);
        }

        private void GetDefaultValues()
        {
            coldTankUsePercentagesInsteadOfMoles = cold_tank_percent_instead_of_moles_checkbox.Checked;
            hotTankUsePercentagesInsteadOfMoles = hot_tank_percent_instead_of_moles_checkbox.Checked;
            cold_tank_kpa.Enabled = coldTankUsePercentagesInsteadOfMoles;
            hot_tank_kpa.Enabled = hotTankUsePercentagesInsteadOfMoles;
        }

        private void AddGasRow(GasType gasType, FlowLayoutPanel targetPanel, BindingList<GasType> availableGases, bool usePercentagesInsteadOfMoles)
        {
            Panel gasRow = new Panel();
            gasRow.Width = targetPanel.ClientSize.Width - 7;
            gasRow.Left = (targetPanel.ClientSize.Width - gasRow.Width) / 2;
            gasRow.Height = 40;
            gasRow.BorderStyle = BorderStyle.FixedSingle;
            gasRow.BackColor = GasLibrary.Gases[gasType].DisplayColor;
            gasRow.Tag = gasType;

            // Gas name label
            Label nameLabel = new Label();
            nameLabel.Text = GasLibrary.Gases[gasType].Name;
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(0, 0);
            nameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            nameLabel.BackColor = Color.Transparent;

            // NumericUpDown for amount
            NumericUpDown amountInput = new NumericUpDown();
            amountInput.Minimum = 0;
            amountInput.Maximum = 100000;
            amountInput.Width = 60;
            amountInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            amountInput.Location = new Point(0, gasRow.Height - amountInput.Height);
            amountInput.UpDownAlign = LeftRightAlignment.Left;
            amountInput.DecimalPlaces = 2;

            // Label for unit (% or mols)
            Label unitLabel = new Label();
            unitLabel.Text = usePercentagesInsteadOfMoles ? "%" : "mols";
            unitLabel.AutoSize = true;
            unitLabel.Location = new Point(amountInput.Right, amountInput.Top);
            unitLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Remove button
            Button removeButton = new Button();
            removeButton.Text = "Remove";
            removeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            removeButton.Location = new Point(gasRow.Width - removeButton.Width, gasRow.Height - removeButton.Height);
            removeButton.Click += (s, e) =>
            {
                targetPanel.Controls.Remove(gasRow);
                availableGases.Add(gasType);
            };

            // Add controls to row
            gasRow.Controls.Add(nameLabel);
            gasRow.Controls.Add(amountInput);
            gasRow.Controls.Add(unitLabel);
            gasRow.Controls.Add(removeButton);

            // Add row to list
            targetPanel.Controls.Add(gasRow);
        }

        private void ResizeGasRows(FlowLayoutPanel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Panel gasRow)
                {
                    gasRow.Width = panel.ClientSize.Width - 7;

                    // Ensure the remove button sticks to the right after resize
                    foreach (Control inner in gasRow.Controls)
                    {
                        if (inner is Button removeButton)
                        {
                            removeButton.Location = new Point(gasRow.Width - removeButton.Width, gasRow.Height - removeButton.Height);
                        }
                    }
                }
            }
        }

        private void ConvertNumericValues(FlowLayoutPanel panel, float temperature, float targetPressure, bool usePercentagesInsteadOfMoles)
        {
            // Calculate the max moles if we’re in percentage mode
            float maxMoles = (float)targetPressure * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * temperature);

            // Walk through each row
            foreach (Panel gasRow in panel.Controls.OfType<Panel>())
            {
                NumericUpDown? amountInput = gasRow.Controls.OfType<NumericUpDown>().FirstOrDefault();
                if (amountInput == null)
                {
                    continue;
                }

                if (usePercentagesInsteadOfMoles)
                {
                    // mols -> %
                    if (maxMoles > 0)
                    {
                        amountInput.Value = (decimal)((float)amountInput.Value / maxMoles * 100f);
                    }
                    else
                    {
                        amountInput.Value = 0;
                    }

                    amountInput.Maximum = 100; // percentages capped
                }
                else
                {
                    // % -> mols
                    amountInput.Maximum = 10000; // back to mol limits
                    if (maxMoles > 0)
                    {
                        amountInput.Value = (decimal)((float)amountInput.Value / 100f * maxMoles);
                    }
                    else
                    {
                        amountInput.Value = 0;
                    }
                }
            }
        }

        private void UpdateUnitLabels(FlowLayoutPanel panel, bool usePercentagesInsteadOfMoles)
        {
            foreach (Panel gasRow in panel.Controls.OfType<Panel>())
            {
                var unitLabel = gasRow.Controls.OfType<Label>()
                    .FirstOrDefault(l => l.Text == "%" || l.Text == "mols");

                if (unitLabel != null)
                {
                    unitLabel.Text = usePercentagesInsteadOfMoles ? "%" : "mols";
                }
            }
        }

        private Dictionary<GasType, float> CollectTankContents(FlowLayoutPanel panel)
        {
            Dictionary<GasType, float> contents = new Dictionary<GasType, float>();
            foreach (GasType gas in Enum.GetValues(typeof(GasType)))
            {
                contents[gas] = 0f;
            }

            foreach (Panel gasRow in panel.Controls.OfType<Panel>())
            {
                if (gasRow.Tag is GasType gasType)
                {
                    var amountInput = gasRow.Controls.OfType<NumericUpDown>().FirstOrDefault();

                    if (amountInput != null && amountInput.Value > 0)
                    {
                        contents[gasType] = (float)amountInput.Value;
                    }
                }
            }

            return contents;
        }

        private void Calculate(object sender, EventArgs e)
        {
            float coldTankTemperature = (float)cold_tank_temperature.Value;
            float hotTankTemperature = (float)hot_tank_temperature.Value;

            Dictionary<GasType, float> coldTankContents = CollectTankContents(cold_tank_panel);
            Dictionary<GasType, float> hotTankContents = CollectTankContents(hot_tank_panel);

            if (coldTankUsePercentagesInsteadOfMoles)
            {
                float maxColdTankMoles = (float)cold_tank_kpa.Value * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * coldTankTemperature);
                foreach (GasType gasType in coldTankContents.Keys)
                {
                    coldTankContents[gasType] = coldTankContents[gasType] / 100f * maxColdTankMoles;
                }
            }
            if (hotTankUsePercentagesInsteadOfMoles)
            {
                float maxHotTankMoles = (float)hot_tank_kpa.Value * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * hotTankTemperature);
                foreach (GasType gasType in hotTankContents.Keys)
                {
                    hotTankContents[gasType] = hotTankContents[gasType] / 100f * maxHotTankMoles;
                }
            }

            GasMixture coldTank = new GasMixture(coldTankTemperature, 70f, coldTankContents);
            GasMixture hotTank = new GasMixture(hotTankTemperature, 70f, hotTankContents);
            GasMixture mergedTank = GasMixture.Merge(coldTank, hotTank);

            string[] reactions = new string[4];
            for (int i = 0; i < 4; i++)
            {
                List<string>? reactionsThatOccured = mergedTank.React();
                reactions[i] = reactionsThatOccured == null ? "Hyper-Noblium Supression" :
                    reactionsThatOccured.Count == 0 ? "None" : string.Join(", ", reactionsThatOccured);
            }

            float bombRange = Math.Max((mergedTank.Pressure - 4053f) / 607.95f, 0f);

            calculation_results.Text = $"Cold Tank:\n" +
                coldTank +
                $"\n\nHot Tank:\n" +
                hotTank + 
                $"\n\nCombined Tank:\n" +
                mergedTank +
                $"\n\nReactions:\n" +
                $"- {string.Join("\n- ", reactions)}\n\n" +
                $"Bomb Range:\n" +
                $"- {bombRange}";
        }
    }
}
