struct ProcessCPURAM {
    1: optional double cpuPercentage;
    2: optional double ramPercentage;
}

service MonitorService {
    ProcessCPURAM getProcessInfo();
}
