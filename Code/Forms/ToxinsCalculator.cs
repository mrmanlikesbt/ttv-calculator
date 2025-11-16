using System.ComponentModel;
using System.Text.RegularExpressions;
using TTV_Calculator.Code.UserControls;

namespace TTV_Calculator
{
    public partial class ToxinsCalculator : Form
    {
        private TabControl mainTabControl;
        private Size tabControlChrome; // width/height extra (tab header + border)
        public ToxinsCalculator()
        {
            InitializeComponent();

            // Create TabControl
            mainTabControl = new TabControl()
            {
                Name = "mainTabControl",
                Location = new Point(0, 0),
                Size = this.ClientSize,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            AddTabPage("Calculator", new CalculatorControl());

            Controls.Add(mainTabControl);
            mainTabControl.BringToFront();

            // Compute tabControl chrome (tab header + borders) so we can size it correctly later.
            // chrome = total size - client area size
            tabControlChrome = new Size(
                SystemInformation.FrameBorderSize.Width * 4 + mainTabControl.Size.Width - mainTabControl.ClientSize.Width,
                SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height + mainTabControl.Size.Height - mainTabControl.ClientSize.Height
            );

            // Hook selection change so we resize when user switches tabs
            mainTabControl.SelectedIndexChanged += (s, e) => ResizeFormToSelectedTabContent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
            ResizeFormToSelectedTabContent();
        }

        private void AddTabPage(string name, UserControl newControl)
        {
            string sanitizedName = Regex.Replace(name, @"[^a-zA-Z0-9]", "");
            TabPage newPage = new TabPage(name)
            {
                Name = $"tab{sanitizedName}", 
                Padding = new Padding(0),
            };

            mainTabControl.TabPages.Add(newPage);

            newControl.Size = newControl.PreferredSize;
            newControl.Location = new Point(0, 0);
            newControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            newPage.Controls.Add(newControl);
        }

        private void ResizeFormToSelectedTabContent()
        {
            if (mainTabControl == null)
            {
                return;
            }

            // Find the currently selected TabPage and ensure it has content
            TabPage? page = mainTabControl.SelectedTab;
            if (page == null || page.Controls.Count == 0)
            {
                return;
            }

            // Prefer a UserControl if present; otherwise fall back to the first child control
            Control content = page.Controls.OfType<UserControl>().FirstOrDefault() ?? page.Controls[0];

            // Position & size the content in the tab page
            Size desiredContentSize = content.PreferredSize;
            if (desiredContentSize.Width <= 0 || desiredContentSize.Height <= 0)
            {
                desiredContentSize = content.Size;
            }

            content.Location = new Point(0, 0);
            content.Size = desiredContentSize;

            Size desiredTabSize = new Size(
                desiredContentSize.Width + tabControlChrome.Width,
                desiredContentSize.Height + tabControlChrome.Height
            );

            mainTabControl.Size = desiredTabSize;
            this.ClientSize = mainTabControl.Size;
        }
    }
}
