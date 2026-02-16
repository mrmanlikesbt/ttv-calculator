using System.ComponentModel;
using System.Data;

namespace TTV_Calculator.Code.UserControls
{
	public partial class BruteForceControl : UserControl
	{
		private readonly BindingList<GasType> availableColdTankGases = new(Enum.GetValues(typeof(GasType)).Cast<GasType>().ToList());
		private readonly BindingList<GasType> availableHotTankGases = new(Enum.GetValues(typeof(GasType)).Cast<GasType>().ToList());

		private readonly BruteForceEngine _engine = new();

		public BruteForceControl()
		{
			_engine.StatusChanged += newStatus =>
			{
				BeginInvoke(() =>
				{
					status_log.Text += $"[{DateTime.Now.ToString("HH:mm:ss")}]: {newStatus}\n";
				});
			};
			_engine.HighscoreUpdated += (bombSize, coldTank, hotTank) =>
			{
				BeginInvoke(() =>
				{
					highscore.Text = $"Bomb Range: {bombSize}\n\n" +
						"Cold Tank:\n" +
						coldTank +
						"\n\nHot Tank:\n" +
						hotTank;
                });
			};
            _engine.Completed += () =>
			{
				BeginInvoke(() =>
				{
					begin_button.Enabled = true;
					pause_button.Enabled = false;
					unpause_button.Enabled = false;
					end_button.Enabled = false;
				});
			};

			InitializeComponent();
			SetupUI();
		}

        private void SetupUI()
		{
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
					AddGasRow(selectedGasType, cold_tank_panel, availableColdTankGases);
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
					AddGasRow(selectedGasType, hot_tank_panel, availableHotTankGases);
				}
			};

			// Hook resizing fix
			cold_tank_panel.SizeChanged += (s, e) => ResizeGasRows(cold_tank_panel);
			hot_tank_panel.SizeChanged += (s, e) => ResizeGasRows(hot_tank_panel);

			AddGasRow(GasType.Oxygen, cold_tank_panel, availableColdTankGases);
			AddGasRow(GasType.Tritium, cold_tank_panel, availableColdTankGases);

			AddGasRow(GasType.HyperNoblium, hot_tank_panel, availableHotTankGases);
		}

		private readonly struct GasRowTag(GasType gasType, Button removeButton)
        {
			public readonly GasType GasType = gasType;
			public readonly Button RemoveButton = removeButton;
        }

        private static void AddGasRow(
			GasType gasType,
			FlowLayoutPanel targetPanel,
			BindingList<GasType> availableGases)
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

			Button removeButton = new()
			{
				Text = "Remove",
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
				Location = new Point(gasRow.Width - 75, gasRow.Height - 23)
			};

			// Assign tag so we never need to search for anything again
			gasRow.Tag = new GasRowTag(gasType, removeButton);

			removeButton.Click += (s, e) =>
			{
				targetPanel.Controls.Remove(gasRow);
				availableGases.Add(gasType);
				gasRow.Dispose();
			};

			// Add controls
			gasRow.Controls.Add(nameLabel);
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
				tag.RemoveButton.Location = new Point(
					gasRow.Width - tag.RemoveButton.Width,
					gasRow.Height - tag.RemoveButton.Height);
			}
		}

		private static GasType[] CollectTankContents(FlowLayoutPanel panel)
		{
			List<GasType> contents = [];

			foreach (Panel row in panel.Controls.OfType<Panel>())
			{
				if (row.Tag == null)
				{
					continue;
				}

				GasRowTag tag = (GasRowTag)row.Tag;
				contents.Add(tag.GasType);
			}

			return contents.ToArray();
		}

        private void begin_button_Click(object sender, EventArgs e)
        {
            begin_button.Enabled = false;
            pause_button.Enabled = true;
            unpause_button.Enabled = false;
            end_button.Enabled = true;
			status_log.Text = "";

            BruteForceOptions options = new()
            {
                ColdPressureMin = (float)cold_tank_kpa_lower.Value,
                ColdPressureMax = (float)cold_tank_kpa_upper.Value,
                ColdPressureStep = (float)cold_tank_pressure_step_size.Value,
                ColdTemperatureMin = (float)cold_tank_temperature_lower.Value,
                ColdTemperatureMax = (float)cold_tank_temperature_upper.Value,
                ColdTemperatureStep = (float)cold_tank_temperature_step_size.Value,
                ColdGasPercentStep = (float)cold_tank_gas_percent_step_size.Value * 0.01f,
                ColdGasTypes = CollectTankContents(cold_tank_panel),

                HotPressureMin = (float)hot_tank_kpa_lower.Value,
                HotPressureMax = (float)hot_tank_kpa_upper.Value,
                HotPressureStep = (float)hot_tank_pressure_step_size.Value,
                HotTemperatureMin = (float)hot_tank_temperature_lower.Value,
                HotTemperatureMax = (float)hot_tank_temperature_upper.Value,
                HotTemperatureStep = (float)hot_tank_temperature_step_size.Value,
                HotGasPercentStep = (float)hot_tank_gas_percent_step_size.Value * 0.01f,
                HotGasTypes = CollectTankContents(hot_tank_panel),
            };

            _engine.Start(options);
        }

        private void end_button_Click(object sender, EventArgs e)
        {
            begin_button.Enabled = true;
            pause_button.Enabled = false;
            unpause_button.Enabled = false;
            end_button.Enabled = false;
            _engine?.Stop();
        }

        private void pause_button_Click(object sender, EventArgs e)
        {
            pause_button.Enabled = false;
            unpause_button.Enabled = true;
            _engine?.Pause();
        }

        private void unpause_button_Click(object sender, EventArgs e)
        {
            pause_button.Enabled = true;
            unpause_button.Enabled = false;
            _engine?.Resume();
        }
    }
}
