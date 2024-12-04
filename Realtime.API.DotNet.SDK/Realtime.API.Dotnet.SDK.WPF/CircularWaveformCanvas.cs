using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Realtime.API.Dotnet.SDK.WPF
{
    internal class CircularWaveformCanvas : Canvas
    {
        private float[] audioData;

        public void UpdateAudioData(float[] data)
        {
            audioData = data;
            InvalidateVisual(); // 请求重绘
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (audioData == null || audioData.Length == 0)
                return;

            double width = ActualWidth;
            double height = ActualHeight;
            double centerX = width / 2;
            double centerY = height / 2;
            double radius = Math.Min(centerX, centerY) - 10; // 圆的最大半径，留一点边距

            Pen pen = new Pen(Brushes.Blue, 1);
            Brush brush = Brushes.Black;

            // 绘制背景圆
            dc.DrawEllipse(brush, pen, new Point(centerX, centerY), radius, radius);

            // 绘制波形
            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext context = geometry.Open())
            {
                double angleStep = 360.0 / audioData.Length; // 每个点的角度
                for (int i = 0; i < audioData.Length; i++)
                {
                    // 音频幅度影响半径
                    double amplitude = audioData[i] * radius;
                    double finalRadius = radius + amplitude;

                    // 计算点的坐标（极坐标转直角坐标）
                    double angle = Math.PI * 2 * (i / (double)audioData.Length);
                    double x = centerX + finalRadius * Math.Cos(angle);
                    double y = centerY + finalRadius * Math.Sin(angle);

                    if (i == 0)
                    {
                        context.BeginFigure(new Point(x, y), false, false);
                    }
                    else
                    {
                        context.LineTo(new Point(x, y), true, false);
                    }
                }
                context.Close();
            }

            dc.DrawGeometry(null, new Pen(Brushes.White, 1), geometry);
        }
    }
}
