using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Wpf;

public class HardwareViewModel
{
    public DiagramController CpuLoadDiagram;
    public DiagramController CpuTempDiagram;
    public DiagramController CpuFanDiagram;
    public DiagramController MemoryLoadDiagram;
    public DiagramController CpuTDPDiagram;
    public DiagramController GpuLoadDiagram;
    public DiagramController GpuTempDiagram;
    public DiagramController GpuFanDiagram;
    public DiagramController GpuMemoryLoadDiagram;
    public DiagramController GpuTDPDiagram;
    public DiagramController Storage1TempDiagram;

    public event PropertyChangedEventHandler PropertyChanged;

    private OxyColor blue = OxyColor.FromArgb(100, 0, 100, 255);
    private OxyColor blueHidden = OxyColor.FromArgb(50, 0, 100, 255);
    private OxyColor cyan = OxyColor.FromArgb(100, 0, 200, 255);
    private OxyColor cyanHidden = OxyColor.FromArgb(50, 0, 200, 255);
    private OxyColor purple = OxyColor.FromArgb(100, 180, 0, 180);
    private OxyColor red = OxyColor.FromArgb(100, 180, 0, 0);
    private OxyColor redHidden = OxyColor.FromArgb(50, 180, 0, 0);

    public HardwareViewModel()
    {
        CpuLoadDiagram = new DiagramController { Title = "Load" };
        CpuLoadDiagram.Init();
        CpuLoadDiagram.AddSeries("Core Max (%)", cyanHidden);
        CpuLoadDiagram.AddSeries("Total (%)", cyan);

        CpuTempDiagram = new DiagramController { Title = "Temperature" };
        CpuTempDiagram.Init();
        CpuTempDiagram.AddSeries("Temperature (Tctl/Tdie)", redHidden);
        CpuTempDiagram.AddSeries("Temperature (Tdie)", red);

        CpuFanDiagram = new DiagramController { Title = "Fan speed", MaxValue = 2000 };
        CpuFanDiagram.Init();
        CpuFanDiagram.AddSeries("RPM", cyan);
        
        MemoryLoadDiagram = new DiagramController { Title = "Load" };
        MemoryLoadDiagram.Init();
        MemoryLoadDiagram.AddSeries("Load", blue);

        CpuTDPDiagram = new DiagramController { Title = "TDP", MaxValue = 100 };
        CpuTDPDiagram.Init();
        CpuTDPDiagram.AddSeries("TDP", cyan);

        GpuLoadDiagram = new DiagramController { Title = "Load" };
        GpuLoadDiagram.Init();
        GpuLoadDiagram.AddSeries("Total (%)", purple);

        GpuTempDiagram = new DiagramController { Title = "Temperature" };
        GpuTempDiagram.Init();
        GpuTempDiagram.AddSeries("Temperature (Hot Spot)", redHidden);
        GpuTempDiagram.AddSeries("Temperature", red);

        GpuFanDiagram = new DiagramController { Title = "Fan speed", MaxValue = 2000 };
        GpuFanDiagram.Init();
        GpuFanDiagram.AddSeries("RPM", purple);

        GpuMemoryLoadDiagram = new DiagramController { Title = "Memory" };
        GpuMemoryLoadDiagram.Init();
        GpuMemoryLoadDiagram.AddSeries("Memory", purple);

        GpuTDPDiagram = new DiagramController { Title = "TDP", MaxValue = 150 };
        GpuTDPDiagram.Init();
        GpuTDPDiagram.AddSeries("TDP", purple);
        //GpuTDPDiagram.AddSeries("TDP (Board)", blue);

        Storage1TempDiagram = new DiagramController { Title = "Temperature" };
        Storage1TempDiagram.Init();
        Storage1TempDiagram.AddSeries("Temperature 2", redHidden);
        Storage1TempDiagram.AddSeries("Temperature 1", red);
    }

    public void UpdateValues(HardwareData hardwareData)
    {
        CpuLoadDiagram.GetPlotModel().Title = $"Load - {hardwareData.cpuLoadTotal}% (Core Max - {hardwareData.cpuLoadCoreMax}%)";
        CpuLoadDiagram.AddValue(0, hardwareData.cpuLoadCoreMax);
        CpuLoadDiagram.AddValue(1, hardwareData.cpuLoadTotal);
        CpuLoadDiagram.GetPlotModel().InvalidatePlot(true);

        CpuTempDiagram.GetPlotModel().Title = $"Temperature - {hardwareData.cpuTemperature2}°C (Reserve - {hardwareData.cpuTemperature1}°C)";
        CpuTempDiagram.AddValue(0, hardwareData.cpuTemperature1);
        CpuTempDiagram.AddValue(1, hardwareData.cpuTemperature2);
        CpuTempDiagram.GetPlotModel().InvalidatePlot(true);

        CpuFanDiagram.GetPlotModel().Title = $"Fan speed - {hardwareData.cpuFan} RPM";
        CpuFanDiagram.AddValue(0, hardwareData.cpuFan);
        CpuFanDiagram.GetPlotModel().InvalidatePlot(true);

        MemoryLoadDiagram.GetPlotModel().Title = $"RAM - {hardwareData.memoryLoad}% ({hardwareData.memoryUsed}/{hardwareData.memoryUsed + hardwareData.memoryAvailable} GB)";
        MemoryLoadDiagram.AddValue(0, hardwareData.memoryLoad);
        MemoryLoadDiagram.GetPlotModel().InvalidatePlot(true);

        CpuTDPDiagram.GetPlotModel().Title = $"TDP - {hardwareData.cpuTDP} W";
        CpuTDPDiagram.AddValue(0, hardwareData.cpuTDP);
        CpuTDPDiagram.GetPlotModel().InvalidatePlot(true);

        GpuLoadDiagram.GetPlotModel().Title = $"Load - {hardwareData.gpuLoad}%";
        GpuLoadDiagram.AddValue(0, hardwareData.gpuLoad);
        GpuLoadDiagram.GetPlotModel().InvalidatePlot(true);

        GpuTempDiagram.GetPlotModel().Title = $"Temperature - {hardwareData.gpuTemperature1}°C (Hot Spot - {hardwareData.gpuTemperature2}°C)";
        GpuTempDiagram.AddValue(0, hardwareData.gpuTemperature2);
        GpuTempDiagram.AddValue(1, hardwareData.gpuTemperature1);
        GpuTempDiagram.GetPlotModel().InvalidatePlot(true);

        GpuFanDiagram.GetPlotModel().Title = $"Fan speed - {hardwareData.gpuFan} RPM";
        GpuFanDiagram.AddValue(0, hardwareData.gpuFan);
        GpuFanDiagram.GetPlotModel().InvalidatePlot(true);

        GpuMemoryLoadDiagram.GetPlotModel().Title = $"Memory - {hardwareData.gpuMemoryLoad}% ({hardwareData.gpuMemoryUsed}/{hardwareData.gpuMemoryTotal} GB)";
        GpuMemoryLoadDiagram.AddValue(0, hardwareData.gpuMemoryLoad);
        GpuMemoryLoadDiagram.GetPlotModel().InvalidatePlot(true);

        GpuTDPDiagram.GetPlotModel().Title = $"TDP - {hardwareData.gpuTDP} W";
        GpuTDPDiagram.AddValue(0, hardwareData.gpuTDP);
        //GpuTDPDiagram.AddValue(1, hardwareData.gpuTDPBoard);
        GpuTDPDiagram.GetPlotModel().InvalidatePlot(true);

        Storage1TempDiagram.GetPlotModel().Title = $"Temperature - {hardwareData.storage1Temperature1}°C ({hardwareData.storage1Temperature2}°C)";
        Storage1TempDiagram.AddValue(0, hardwareData.storage1Temperature2);
        Storage1TempDiagram.AddValue(1, hardwareData.storage1Temperature1);
        Storage1TempDiagram.GetPlotModel().InvalidatePlot(true);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
