using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;


namespace ScreenRecord
{
    class ResizeNode : Control
    {
        private Point _lastPos;

        private bool _triangular = true;
        [Category("Appearance")]
        [Description("Determines if the control should appear as a triangle or square")]
        [DefaultValue(true)]
        public bool Triangular
        {
            get { return _triangular; }
            set
            {
                _triangular = value;
                SetRegion();
            }
        }

        public ResizeNode()
        {
            this.Size = new Size(10, 10);
            base.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            base.Cursor = Cursors.SizeNWSE;
            base.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
        }

        private void SetRegion()
        {
            if (_triangular)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddPolygon(new[]
                    {
                        new Point(0, this.Height),
                        (Point)this.Size,
                        new Point(this.Width, 0)
                    });

                    this.Region = new Region(path);
                }
            }
            else
            {
                this.Region = null;
            }
        }

        /// <summary>
        /// Positions the node at the bottom right corner
        /// Docking or anchoring logic is not needed as the anchor property is used for this
        /// </summary>
        private void PositionNode()
        {
            if (this.Parent != null)
            {
                // Use client size so that the control appears within the borders of certain controls
                // Ex: a form or a panel with a border
                int x = this.Parent.ClientSize.Width - this.Width - this.Padding.Right;
                int y = this.Parent.ClientSize.Height - this.Height - this.Padding.Bottom;
                this.Location = new Point(x, y);
            }
        }

        #region Overrides
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _lastPos = e.Location;
        }

        protected override void OnResize(EventArgs e)
        {
            SetRegion();
            PositionNode();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                this.Parent.Width += e.Location.X - _lastPos.X;
                this.Parent.Height += e.Location.Y - _lastPos.Y;
            }
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            PositionNode();
        }
        #endregion
    }
}
