using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

public class DiagramController
{
    public PlotModel plotModel { get; set; }
    public string Title { get; set; }
    public int MaxValue = 100;
    public int ValuesCount = 60;
    private List<LineSeries> SeriesList;

    public DiagramController()
    {
        SeriesList = new List<LineSeries>();
    }

    public void Init()
    {
        plotModel = new PlotModel
        {
            Title = Title,
            TitleFontSize = 12,
            TitleFontWeight = 1,
            TextColor = OxyColors.White,
            PlotAreaBackground = OxyColor.FromRgb(30, 37, 51),
            PlotAreaBorderThickness = new OxyThickness(1),
            //PlotAreaBorderColor = OxyColors.White,
            PlotAreaBorderColor = OxyColor.FromRgb(10, 25, 33),
            IsLegendVisible = true,
            TitleHorizontalAlignment = 0
        };

        //AddLegend();
        AddAxes();
    }

    public PlotModel GetPlotModel()
    {
        return plotModel;
    }

    private void AddLegend()
    {
        plotModel.Legends.Add(new Legend
        {
            LegendPosition = LegendPosition.BottomCenter,
            LegendPlacement = LegendPlacement.Outside,
            LegendOrientation = LegendOrientation.Horizontal,
            LegendMargin = 1,
            //LegendBackground = OxyColor.FromArgb(200, 255, 255, 255),
            LegendTextColor = OxyColors.White,
            //LegendBorder = OxyColors.Black,
            //LegendBorderThickness = 1
            LegendFontSize = 10
        });
    }

    private void AddAxes()
    {
        plotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Minimum = 0,
            Maximum = MaxValue,
            //Title = "%",
            AxislineColor = OxyColors.White,
            TicklineColor = OxyColor.FromRgb(10, 25, 33),
            TextColor = OxyColors.White,
            IsAxisVisible = false,
        });
        plotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            //Title = "Time",
            IsAxisVisible = false,
            //AxislineColor = OxyColors.White,
            //TextColor = OxyColors.White
        });
    }

    public void AddSeries(string title, OxyColor color)
    {
        LineSeries series = new AreaSeries
        {
            Title = title,
            Color = color,
            TrackerFormatString = "{0}: {Y}",
            StrokeThickness = 1,
            Fill = OxyColor.FromAColor(50, color)
        };

        for (int i = 0; i < ValuesCount; i++)
        {
            series.Points.Add(new DataPoint(i, 0));
        }

        plotModel.Series.Add(series);
        SeriesList.Add(series);
    }

    public void AddValue(int index, double value)
    {
        LineSeries series = SeriesList[index];

        if (series.Points.Count >= ValuesCount)
        {
            series.Points.RemoveAt(0);
            for (int i = 0; i < series.Points.Count; i++)
            {
                series.Points[i] = new DataPoint(i, series.Points[i].Y);
            }
        }

        series.Points.Add(new DataPoint(series.Points.Count, value));
    }
}
