using System;
using System.Collections.Generic;
using UniRx;

public interface IMachineInfoRepository
{
    public IReadOnlyReactiveProperty<MachineInfo?> getMachineInfo(int index);

    public struct MachineInfo
    {
        public int hallId;
        public string name;
        public int efficiency;
        public string status;
        public string symbol;
        public DateTime technicalExaminationDate;
    }
}

public class MachineInfoRepository : IMachineInfoRepository
{
    private Dictionary<int, IMachineInfoRepository.MachineInfo> localMachineInformations;

    public MachineInfoRepository()
    {
        populateMachineInformation();
    }
    public IReadOnlyReactiveProperty<IMachineInfoRepository.MachineInfo?> getMachineInfo(int index)
    {
        if (localMachineInformations.TryGetValue(index, out IMachineInfoRepository.MachineInfo machineInfo))
        {
            return new ReactiveProperty<IMachineInfoRepository.MachineInfo?>(machineInfo);
        }
        else
        {
            return new ReactiveProperty<IMachineInfoRepository.MachineInfo?>(null);
        }
    }

    private void populateMachineInformation()
    {
        localMachineInformations = new Dictionary<int, IMachineInfoRepository.MachineInfo>();
        localMachineInformations.Add(
            0,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "ASP kolumna robocza",
                efficiency = 100,
                status = "Off",
                symbol = "10000-2",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            1,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Ch³odnica pieca indukcyjnego",
                efficiency = 100,
                status = "Off",
                symbol = "10000-3",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            2,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Ci¹garka ³awowa",
                efficiency = 100,
                status = "Off",
                symbol = "10000-4",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            3,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Hala",
                efficiency = -1,
                status = "Off",
                symbol = "",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            4,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Hydraulika",
                efficiency = 100,
                status = "Off",
                symbol = "10000-1",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            5,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Instron Du¿y",
                efficiency = 100,
                status = "Off",
                symbol = "10000-6",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            6,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Kompresor",
                efficiency = 100,
                status = "Off",
                symbol = "10000-7",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            7,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "M³ot Pneumatyczny",
                efficiency = 100,
                status = "Off",
                symbol = "10000-8",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            8,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Piec Elterma PSK-7",
                efficiency = 100,
                status = "Off",
                symbol = "10000-9",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            9,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Prasa 500T",
                efficiency = 81,
                status = "Working",
                symbol = "10000-16",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            10,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Silnik",
                efficiency = 100,
                status = "Off",
                symbol = "10000-11",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            11,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Sterowanie pieca indukcyjnego",
                efficiency = 100,
                status = "Working",
                symbol = "10000-12",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            12,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Walcarka Skoœna",
                efficiency = 100,
                status = "Off",
                symbol = "10000-14",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
        localMachineInformations.Add(
            13,
            new IMachineInfoRepository.MachineInfo
            {
                hallId = 0,
                name = "Zwick",
                efficiency = 98,
                status = "Working",
                symbol = "10000-15",
                technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
            }
        );
    }
}
