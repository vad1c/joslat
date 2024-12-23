public class RoundButton : Button
{
    public int BorderRadius { get; set; } = 20; // 默认圆角半径
    public Color DefaultBackColor { get; set; } = Color.Green; // 默认背景颜色
    public Color HoverBackColor { get; set; } = Color.LimeGreen; // 悬停背景颜色
    public Color ClickBackColor { get; set; } = Color.DarkGreen; // 点击背景颜色

    private Color _currentBackColor;

    public RoundButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0; // 去除边框
        _currentBackColor = DefaultBackColor;

        // 初始化事件
        MouseEnter += (s, e) =>
        {
            _currentBackColor = HoverBackColor;
            Invalidate();
        };

        MouseLeave += (s, e) =>
        {
            _currentBackColor = DefaultBackColor;
            Invalidate();
        };

        MouseDown += (s, e) =>
        {
            _currentBackColor = ClickBackColor;
            Invalidate();
        };

        MouseUp += (s, e) =>
        {
            _currentBackColor = HoverBackColor;
            Invalidate();
        };
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        Graphics g = pevent.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 抗锯齿

        Rectangle rect = new Rectangle(0, 0, Width, Height);

        // 绘制圆角矩形背景
        using (var brush = new SolidBrush(_currentBackColor))
        using (var path = CreateRoundedRectanglePath(rect, BorderRadius))
        {
            g.FillPath(brush, path);
        }

        // 绘制文本
        TextRenderer.DrawText(
            g,
            Text,
            Font,
            rect,
            ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
        );
    }

    private System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        int diameter = radius * 2;

        // 圆角路径
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90); // 左上角
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90); // 右上角
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90); // 右下角
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90); // 左下角
        path.CloseFigure();

        return path;
    }
}
