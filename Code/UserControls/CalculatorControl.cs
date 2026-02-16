using System.ComponentModel;
using System.Data;

namespace TTV_Calculator.Code.UserControls
{
	public partial class CalculatorControl : UserControl
	{
		private readonly BindingList<GasType> availableColdTankGases = new(Enum.GetValues(typeof(GasType)).Cast<GasType>().ToList());
		private readonly BindingList<GasType> availableHotTankGases = new(Enum.GetValues(typeof(GasType)).Cast<GasType>().ToList());

		private bool coldTankUsePercentagesInsteadOfMoles = false;
		private bool hotTankUsePercentagesInsteadOfMoles = false;
		public CalculatorControl()
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
				{
					e.Value = GasLibrary.Gases[(int)gasType].Name;
				}
			};
			cold_tank_add_gas.Click += (s, e) =>
			{
				if (cold_tank_gas_dropdown.SelectedItem is GasType selectedGasType)
				{
					AddGasRow(selectedGasType, cold_tank_panel, availableColdTankGases, coldTankUsePercentagesInsteadOfMoles);
				}
			};

			// Hot tank dropdown
			hot_tank_gas_dropdown.DataSource = availableHotTankGases;
			hot_tank_gas_dropdown.Format += (s, e) =>
			{
				if (e.ListItem is GasType gasType)
				{
					e.Value = GasLibrary.Gases[(int)gasType].Name;
				}
			};
			hot_tank_add_gas.Click += (s, e) =>
			{
				if (hot_tank_gas_dropdown.SelectedItem is GasType selectedGasType)
				{
					AddGasRow(selectedGasType, hot_tank_panel, availableHotTankGases, hotTankUsePercentagesInsteadOfMoles);
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

            AddGasRow(GasType.Oxygen, cold_tank_panel, availableColdTankGases, coldTankUsePercentagesInsteadOfMoles, 50); // 50% oxygen
            AddGasRow(GasType.Tritium, cold_tank_panel, availableColdTankGases, coldTankUsePercentagesInsteadOfMoles, 50); // 50% tritium

            AddGasRow(GasType.HyperNoblium, hot_tank_panel, availableHotTankGases, hotTankUsePercentagesInsteadOfMoles, 100); // 100% hypernob
        }

		private readonly struct GasRowTag(GasType gasType, NumericUpDown amountInput, Label unitLabel, Button removeButton)
        {
			public readonly GasType GasType = gasType;
			public readonly NumericUpDown AmountInput = amountInput;
			public readonly Label UnitLabel = unitLabel;
			public readonly Button RemoveButton = removeButton;
        }

        private static void AddGasRow(
			GasType gasType,
			FlowLayoutPanel targetPanel,
			BindingList<GasType> availableGases,
			bool usePercentagesInsteadOfMoles,
			decimal initialValue = 0)
		{
			availableGases.Remove(gasType);

			Panel gasRow = new()
			{
				Width = targetPanel.ClientSize.Width - 7,
				Left = 3,
				Height = 40,
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = GasLibrary.Gases[(int)gasType].DisplayColor,
			};

			// Gas row controls
			Label nameLabel = new()
			{
				Text = GasLibrary.Gases[(int)gasType].Name,
				AutoSize = true,
				Location = new Point(0, 0),
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				BackColor = Color.Transparent
			};

			NumericUpDown amountInput = new()
			{
				Minimum = 0,
				Maximum = 100000,
				Width = 60,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
				Location = new Point(0, gasRow.Height - 20),
				UpDownAlign = LeftRightAlignment.Left,
                Value = initialValue,
                DecimalPlaces = 2
			};

			Label unitLabel = new()
			{
				Text = usePercentagesInsteadOfMoles ? "%" : "mols",
				AutoSize = true,
				Location = new Point(amountInput.Right, amountInput.Top),
				Anchor = AnchorStyles.Bottom | AnchorStyles.Left
			};

			Button removeButton = new()
			{
				Text = "Remove",
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				Location = new Point(gasRow.Width - 75, gasRow.Height - 23)
			};

			// Assign tag so we never need to search for anything again
			gasRow.Tag = new GasRowTag(gasType, amountInput, unitLabel, removeButton);

			removeButton.Click += (s, e) =>
			{
				targetPanel.Controls.Remove(gasRow);
				availableGases.Add(gasType);
				gasRow.Dispose();
			};

			// Add controls
			gasRow.Controls.Add(nameLabel);
			gasRow.Controls.Add(amountInput);
			gasRow.Controls.Add(unitLabel);
			gasRow.Controls.Add(removeButton);

			// Add to panel
			targetPanel.Controls.Add(gasRow);
		}

		private static void ResizeGasRows(FlowLayoutPanel panel)
		{
			foreach (Panel gasRow in panel.Controls.OfType<Panel>())
			{
				gasRow.Width = panel.ClientSize.Width - 7;
				if (gasRow.Tag == null)
				{
					continue;
				}
				GasRowTag tag = (GasRowTag)gasRow.Tag;
				tag.RemoveButton.Location = new Point(gasRow.Width - tag.RemoveButton.Width, gasRow.Height - tag.RemoveButton.Height);
			}
		}

		private static void ConvertNumericValues(FlowLayoutPanel panel, float temperature, float targetPressure, bool usePercentagesInsteadOfMoles)
		{
			float maxMoles = targetPressure * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * temperature);

			foreach (Panel gasRow in panel.Controls.OfType<Panel>())
			{
				if (gasRow.Tag == null)
				{
					continue;
				}

				GasRowTag tag = (GasRowTag)gasRow.Tag;
				NumericUpDown amountInput = tag.AmountInput;

				if (usePercentagesInsteadOfMoles)
				{
                    amountInput.Value = maxMoles > 0
                        ? (decimal)((float)amountInput.Value / maxMoles * 100f)
                        : 0m;
                    amountInput.Maximum = 100;
				}
				else
				{
                    amountInput.Maximum = 10000;
                    amountInput.Value = maxMoles > 0
                        ? (decimal)((float)amountInput.Value / 100f * maxMoles)
                        : 0m;
				}
			}
		}

		private static void UpdateUnitLabels(FlowLayoutPanel panel, bool usePercentagesInsteadOfMoles)
		{
			foreach (Panel gasRow in panel.Controls.OfType<Panel>())
			{
				if (gasRow.Tag == null)
				{
					continue;
				}

				GasRowTag tag = (GasRowTag)gasRow.Tag;
				tag.UnitLabel.Text = usePercentagesInsteadOfMoles ? "%" : "mols";
			}
		}

		private static float[] CollectTankContents(FlowLayoutPanel panel)
		{
			float[] contents = new float[GasLibrary.GasCount];

			foreach (Panel row in panel.Controls.OfType<Panel>())
			{
				if (row.Tag == null)
				{
					continue;
				}

				GasRowTag tag = (GasRowTag)row.Tag;
				float value = (float)tag.AmountInput.Value;
				if (value > 0)
				{
					contents[(int)tag.GasType] = value;
				}
			}

			return contents;
		}

		private void Calculate(object sender, EventArgs e)
		{
			float coldTankTemperature = (float)cold_tank_temperature.Value;
			float hotTankTemperature = (float)hot_tank_temperature.Value;

			float[] coldTankContents = CollectTankContents(cold_tank_panel);
			float[] hotTankContents = CollectTankContents(hot_tank_panel);

			if (coldTankUsePercentagesInsteadOfMoles)
			{
				float maxColdTankMoles = (float)cold_tank_kpa.Value * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * coldTankTemperature);
				for (int i = 0; i < GasLibrary.GasCount; i++)
				{
					coldTankContents[i] = coldTankContents[i] / 100f * maxColdTankMoles;
				}
			}
			if (hotTankUsePercentagesInsteadOfMoles)
			{
				float maxHotTankMoles = (float)hot_tank_kpa.Value * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * hotTankTemperature);
				for (int i = 0; i < GasLibrary.GasCount; i++)
				{
					hotTankContents[i] = hotTankContents[i] / 100f * maxHotTankMoles;
				}
			}

			GasMixtureConstructor coldTank = new(coldTankTemperature, 70f, coldTankContents);
			GasMixtureConstructor hotTank = new(hotTankTemperature, 70f, hotTankContents);

			GasMixture combinedTank = new();
			combinedTank.Merge(coldTank, hotTank);

			string[] reactionsThatOccured = new string[4];
			for (int i = 0; i < 4; i++)
			{
				ReactionType reactionResult = combinedTank.React();
				reactionsThatOccured[i] = GasReaction.TypeToString(reactionResult);
			}

			List<string> results = [];
            if (log_bomb_range.Checked)
            {
                results.Add($"Bomb Range:\n- {float.Round(combinedTank.GetBombSize())}");
            }
            if (log_cold_tank.Checked)
            {
                results.Add($"Cold Tank:\n{coldTank}");
            }
            if (log_hot_tank.Checked)
			{
				results.Add($"Hot Tank:\n{hotTank}");
			}
            if (log_combined_tank.Checked)
            {
                results.Add($"Combined Tank:\n{combinedTank}");
            }
            if (log_reactions.Checked)
            {
                results.Add($"Reactions:\n- {string.Join("\n- ", reactionsThatOccured)}");
            }
            calculation_results.Text = string.Join("\n\n", results);
		}
	}
}
