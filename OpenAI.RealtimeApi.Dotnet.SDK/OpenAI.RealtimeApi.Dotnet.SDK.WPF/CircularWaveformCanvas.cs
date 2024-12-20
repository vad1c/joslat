using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OpenAI.RealtimeApi.Dotnet.SDK.WPF
{
    internal class CircularWaveformCanvas : Canvas
    {
        private float[] audioData;

        public void UpdateAudioData(float[] data)
        {
            audioData = data;
            InvalidateVisual(); 
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
            double radius = Math.Min(centerX, centerY) - 10; 

            Pen pen = new Pen(Brushes.Blue, 1);
            Brush brush = Brushes.Black;

            dc.DrawEllipse(brush, pen, new Point(centerX, centerY), radius, radius);

            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext context = geometry.Open())
            {
                double angleStep = 360.0 / audioData.Length; 
                for (int i = 0; i < audioData.Length; i++)
                {
                    double amplitude = audioData[i] * radius;
                    double finalRadius = radius + amplitude;

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
