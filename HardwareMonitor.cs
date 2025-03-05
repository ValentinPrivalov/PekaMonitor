using LibreHardwareMonitor.Hardware;
using System;
using System.Windows.Interop;

public class HardwareMonitor
{
    private Computer computer;
    private HardwareData hardwareData;
    private bool debug = true;

    public HardwareMonitor()
    {
        hardwareData = new HardwareData();
        computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsStorageEnabled = true,
            IsMotherboardEnabled = true,
            IsMemoryEnabled = true,
            IsNetworkEnabled = false,
            IsControllerEnabled = true
        };
        computer.Open();
    }

    public void GatherData()
    {
        foreach (var hardware in computer.Hardware)
        {
            hardware.Update();
            Debug(" ");
            Debug($"--- Hardware ({hardware.HardwareType}): {hardware.Name}");

            switch (hardware.HardwareType)
            {
                case HardwareType.Motherboard:
                    GetMotherBoardData(hardware);
                    break;
                case HardwareType.Cpu:
                    GetCPUData(hardware);
                    break;
                case HardwareType.GpuNvidia:
                    GetGPUData(hardware);
                    break;
                case HardwareType.Memory:
                    GetMemoryData(hardware);
                    break;
                case HardwareType.Storage:
                    GetStorageData(hardware);
                    break;
                case HardwareType.GpuAmd:
                    Debug("Skip...");
                    break;
                default:
                    foreach (var sensor in hardware.Sensors)
                    {
                        string msg = "";
                        msg = $"Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                        Debug(msg);
                    }

                    foreach (var subHardware in hardware.SubHardware)
                    {
                        subHardware.Update();
                        string msg = "";
                        foreach (var sensor in subHardware.Sensors)
                        {
                            msg = $"Sub sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                            Debug(msg);
                        }
                    }
                    break;
            }
        }

        debug = false;
    }

    public HardwareData GetHardwareData()
    {
        return hardwareData;
    }

    private void GetMotherBoardData(IHardware hardware)
    {
        hardwareData.motherboardName = hardware.Name;

        foreach (var sensor in hardware.Sensors)
        {
            string msg = "";
            msg = $"Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
            Debug(msg);
        }

        foreach (var subHardware in hardware.SubHardware)
        {
            subHardware.Update();
            foreach (var sensor in subHardware.Sensors)
            {
                string msg = "";

                switch (sensor.Name)
                {
                    case "CPU Fan":
                        if (sensor.SensorType == SensorType.Fan)
                        {
                            hardwareData.cpuFan = Math.Round(sensor.Value ?? 0, 0);
                            msg = $"Fan: {sensor.Value:F0} RPM";
                        }
                        break;
                    default:
                        //msg = $"Sub Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                        break;
                }

                Debug(msg);
            }
        }
    }

    private void GetCPUData(IHardware hardware)
    {
        hardwareData.cpuName = hardware.Name;

        foreach (var sensor in hardware.Sensors)
        {
            string msg = "";
            switch (sensor.Name)
            {
                case "CPU Total":
                    hardwareData.cpuLoadTotal = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Load Total: {sensor.Value:F0}%";
                    break;
                case "CPU Core Max":
                    hardwareData.cpuLoadCoreMax = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Load Core Max: {sensor.Value:F0}%";
                    break;
                case "Core (Tctl/Tdie)":
                    hardwareData.cpuTemperature1 = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Temperature (Tctl/Tdie): {sensor.Value:F0}°C";
                    break;
                case "CCD1 (Tdie)":
                    hardwareData.cpuTemperature2 = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Temperature (Tdie): {sensor.Value:F0}°C";
                    break;
                case "Package":
                    hardwareData.cpuTDP = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"TDP: {sensor.Value:F0} W";
                    break;
                default:
                    //msg = $"Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                    break;
            }

            Debug(msg);
        }

        foreach (var subHardware in hardware.SubHardware)
        {
            subHardware.Update();
            foreach (var sensor in subHardware.Sensors)
            {
                string msg = "";
                msg = $"Sub Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                Debug(msg);
            }
        }
    }

    private void GetGPUData(IHardware hardware)
    {
        hardwareData.gpuName = hardware.Name;

        foreach (var sensor in hardware.Sensors)
        {
            string msg = "";
            switch (sensor.Name)
            {
                case "GPU Core":
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        hardwareData.gpuTemperature1 = Math.Round(sensor.Value ?? 0, 0);
                        msg = $"Temperature: {sensor.Value:F0}°C";
                    }
                    if (sensor.SensorType == SensorType.Load)
                    {
                        hardwareData.gpuLoad = Math.Round(sensor.Value ?? 0, 0);
                        msg = $"Load: {sensor.Value:F0}%";
                    }
                    break;
                case "GPU Power":
                    hardwareData.gpuTDP = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"TDP: {sensor.Value:F0} W";
                    break;
                case "GPU Board Power":
                    hardwareData.gpuTDPBoard = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"TDP (Board): {sensor.Value:F0} W";
                    break;
                case "GPU Hot Spot":
                    hardwareData.gpuTemperature2 = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Temperature (Hot Spot): {sensor.Value:F0}°C";
                    break;
                case "GPU Memory Total":
                    hardwareData.gpuMemoryTotal = Math.Round(sensor.Value / 1000 ?? 0, 1);
                    msg = $"Memory Total: {sensor.Value / 1000} Gb";
                    break;
                case "GPU Memory Used":
                    hardwareData.gpuMemoryUsed = Math.Round(sensor.Value / 1000 ?? 0, 1);
                    msg = $"Memory Used: {sensor.Value / 1000} Gb";
                    break;
                case "GPU Memory":
                    if (sensor.SensorType == SensorType.Load)
                    {
                        hardwareData.gpuMemoryLoad = Math.Round(sensor.Value ?? 0, 0);
                        msg = $"Memory: {sensor.Value:F0}%";
                    }
                    break;
                case "GPU Fan 1":
                    if (sensor.SensorType == SensorType.Fan)
                    {
                        hardwareData.gpuFan = Math.Round(sensor.Value ?? 0, 0);
                        msg = $"Fan: {sensor.Value:F0} RPM";
                    }
                    break;
                default:
                    //msg = $"Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                    break;
            }

            Debug(msg);
        }
    }

    private void GetMemoryData(IHardware hardware)
    {
        hardwareData.gpuName = hardware.Name;

        foreach (var sensor in hardware.Sensors)
        {
            string msg = "";
            switch (sensor.Name)
            {
                case "Memory":
                    hardwareData.memoryLoad = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Load: {sensor.Value:F0} %";
                    break;
                case "Memory Used":
                    hardwareData.memoryUsed = Math.Round(sensor.Value ?? 0, 1);
                    msg = $"Used: {sensor.Value:F1} Gb";
                    break;
                case "Memory Available":
                    hardwareData.memoryAvailable = Math.Round(sensor.Value ?? 0, 1);
                    msg = $"Available: {sensor.Value:F1} Gb";
                    break;
                default:
                    //msg = $"Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                    break;
            }

            Debug(msg);
        }
    }

    private void GetStorageData(IHardware hardware)
    {
        hardwareData.storage1Name = hardware.Name;

        foreach (var sensor in hardware.Sensors)
        {
            string msg = "";
            switch (sensor.Name)
            {
                case "Temperature":
                    hardwareData.storage1Temperature1 = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Temperature: {sensor.Value:F0}°C";
                    break;
                case "Temperature 2":
                    hardwareData.storage1Temperature2 = Math.Round(sensor.Value ?? 0, 0);
                    msg = $"Temperature 2: {sensor.Value:F0}°C";
                    break;
                case "Used Space":
                    msg = $"Used Space: {sensor.Value:F0}%";
                    break;
                default:
                    //msg = $"Sensor: {sensor.Name} - {sensor.Value} ({sensor.SensorType})";
                    break;
            }

            Debug(msg);
        }
    }

    private void Debug(string msg)
    {
        if (msg != "" && debug)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }

    public void Close()
    {
        computer.Close();
    }
}

public class HardwareData
{
    // motherboard data
    public string motherboardName = "";

    // cpu data
    public string cpuName = "";
    public double cpuLoadTotal = 0;
    public double cpuLoadCoreMax = 0;
    public double cpuTemperature1 = 0;
    public double cpuTemperature2 = 0;
    public double cpuFan = 0;
    public double cpuTDP = 0;

    // gpu data
    public string gpuName = "";
    public double gpuLoad = 0;
    public double gpuMemoryTotal = 0;
    public double gpuMemoryUsed = 0;
    public double gpuMemoryLoad = 0;
    public double gpuTemperature1 = 0;
    public double gpuTemperature2 = 0;
    public double gpuFan = 0;
    public double gpuTDP = 0;
    public double gpuTDPBoard = 0;

    // memory data
    public double memoryLoad = 0;
    public double memoryUsed = 0;
    public double memoryAvailable = 0;

    // storage data
    public string storage1Name = "";
    public double storage1Temperature1 = 0;
    public double storage1Temperature2 = 0;
}