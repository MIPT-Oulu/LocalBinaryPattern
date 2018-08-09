using System;
using System.Drawing;
using System.Windows.Forms;
using LBP.Components;

namespace LBP
{
    public class ParameterForm
    {
        public static void DefineParam(ref Parameters param)
        {
            // Create different buttons and functions
            var buttonOk = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right
            };
            var buttonCancel = new Button()
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Right
            };
            var form = new Form()
            {
                Text = "Parameters",
                ClientSize = new Size(400, 450),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false,
                AcceptButton = buttonOk,
                CancelButton = buttonCancel
            };
            var neighbourLabel = new Label()
            {
                Text = "Set number of neighbours",
                AutoSize = true
            };
            var neighbourTextBox = new TextBox() { Text = param.Neighbours.ToString() };
            neighbourTextBox.Anchor = neighbourTextBox.Anchor | AnchorStyles.Right;

            var radiusLabel = new Label()
            {
                Text = "Set small radius / LBP radius",
                AutoSize = true
            };
            var radiusTextBox = new TextBox() { Text = param.Radius.ToString() };
            radiusTextBox.Anchor = radiusTextBox.Anchor | AnchorStyles.Right;

            var largeRadiusLabel = new Label()
            {
                Text = "Set large radius",
                AutoSize = true
            };
            var largeRadiusTextBox = new TextBox() { Text = param.LargeRadius.ToString() };
            largeRadiusTextBox.Anchor = largeRadiusTextBox.Anchor | AnchorStyles.Right;

            var filterLabel = new Label()
            {
                Text = "Set filter size (center)",
                AutoSize = true
            };
            var filterTextBox = new TextBox() { Text = param.W_c.ToString() };
            filterTextBox.Anchor = filterTextBox.Anchor | AnchorStyles.Right;

            var filterLargeLabel = new Label()
            {
                Text = "Set filter size (large)",
                AutoSize = true
            };
            var filterLargeTextBox = new TextBox() { Text = param.W_r[0].ToString() };
            filterLargeTextBox.Anchor = filterLargeTextBox.Anchor | AnchorStyles.Right;

            var filterSmallLabel = new Label()
            {
                Text = "Set filter size (small)",
                AutoSize = true
            };
            var filterSmallTextBox = new TextBox() { Text = param.W_r[1].ToString() };
            filterSmallTextBox.Anchor = filterSmallTextBox.Anchor | AnchorStyles.Right;

            var imageLabel = new Label()
            {
                Text = "Loaded image type",
                AutoSize = true
            };
            var imageBox = new ComboBox()
            {
                Location = new Point(10, 340),
                Size = new Size(370, 12),
                DropDownWidth = 200,
                DropDownHeight = 40,
                Items = { ".dat", ".png" },
                Text = ".dat",
                Anchor = neighbourLabel.Anchor | AnchorStyles.Right
            };

            var paddingLabel = new Label()
            {
                Location = new Point(10, 370),
                Size = new Size(300, 12),
                Text = "Padding method",
                AutoSize = true,
                Anchor = neighbourLabel.Anchor | AnchorStyles.Right
            };
            var paddingBox = new ComboBox()
            {
                Location = new Point(10, 390),
                Size = new Size(370, 12),
                DropDownWidth = 200,
                DropDownHeight = 40,
                Items = { "Reflect", "Nearest" },
                Text = "Reflect",
                Anchor = neighbourLabel.Anchor | AnchorStyles.Right
            };

            var save = new RadioButton()
            {
                Location = new Point(200, 90),
                Size = new Size(100, 30),
                Text = "Save images",
                Checked = true
            };

            var scale = new RadioButton()
            {
                Location = new Point(200, 140),
                Size = new Size(100, 30),
                Text = "Scale image values",
                Checked = true
            };

            var meanstd = new RadioButton()
            {
                Location = new Point(200, 190),
                Size = new Size(100, 30),
                Text = "Include mean/std images",
                Checked = false
            };

            // Set size
            neighbourLabel.SetBounds(10, 20, 170, 12);
            neighbourTextBox.SetBounds(10, 40, 170, 20);
            radiusLabel.SetBounds(10, 70, 170, 12);
            radiusTextBox.SetBounds(10, 90, 170, 12);
            largeRadiusLabel.SetBounds(10, 120, 170, 12);
            largeRadiusTextBox.SetBounds(10, 140, 170, 12);
            filterLabel.SetBounds(10, 170, 170, 12);
            filterTextBox.SetBounds(10, 190, 170, 12);
            filterLargeLabel.SetBounds(10, 220, 170, 12);
            filterLargeTextBox.SetBounds(10, 240, 170, 12);
            filterSmallLabel.SetBounds(10, 270, 170, 12);
            filterSmallTextBox.SetBounds(10, 290, 170, 12);
            imageLabel.SetBounds(10, 320, 170, 12);
            buttonOk.SetBounds(230, 420, 75, 25);
            buttonCancel.SetBounds(310, 420, 75, 25);

            // Set all controls to form
            form.Controls.AddRange(new Control[]
            { neighbourLabel, neighbourTextBox, radiusLabel, radiusTextBox, largeRadiusLabel, largeRadiusTextBox, imageLabel, imageBox, save, scale, meanstd,
                filterLabel, filterTextBox, filterLargeLabel, filterLargeTextBox, filterSmallLabel, filterSmallTextBox, paddingLabel, paddingBox, buttonOk, buttonCancel });

            form.ClientSize = new Size(Math.Max(300, neighbourLabel.Right + 10), form.ClientSize.Height);

            // Update parameters
            DialogResult dialogResult = form.ShowDialog();
            if (form.DialogResult == buttonOk.DialogResult)
            {
                param.Neighbours = Convert.ToInt32(neighbourTextBox.Text);
                param.Radius = Convert.ToInt32(radiusTextBox.Text);
                param.LargeRadius = Convert.ToInt32(largeRadiusTextBox.Text);
                param.W_c = Convert.ToInt32(filterTextBox.Text);
                param.W_r[0] = Convert.ToInt32(filterLargeTextBox.Text);
                param.W_r[1] = Convert.ToInt32(filterSmallTextBox.Text);
                param.ImageType = imageBox.Text;
                param.Method = paddingBox.Text;
                param.Save = save.Checked;
                param.Scale = scale.Checked;
                param.Meanstd = meanstd.Checked;
            }
        }
    }
}
