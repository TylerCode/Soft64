using Moq;
using NLog;
using NLog.Config;
using NLog.Targets;
using Soft64;
using Soft64.MipsR4300.Debugging;
using Soft64.PI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

public static class Common
{
    public static Cartridge MockUpCartridge(RegionType region, CICKeyType cic)
    {
        var mockedCart = new Mock<Cartridge>();
        var mockedCartRom = new Mock<ICartRom>();
        var mockedBootRom = new Mock<IBootRom>();
        GameSerial serial = new GameSerial(new Byte[] { 0x00, 00, 00, 00, 00, 00, 00, 00 });

        MemoryStream stream = new MemoryStream();

        for (Int32 i = 0; i < (1024 ^ 2); i++)
            stream.WriteByte(0);

        mockedBootRom.Setup<CICKeyType>(x => x.CIC).Returns(cic);
        mockedBootRom.Setup<Int32>(x => x.BootChecksum).Returns(0x00);

        mockedCartRom.Setup<PiBusSpeedConfig>(x => x.BusConfig).Returns(new PiBusSpeedConfig(0x80, 0x37, 0x12, 0x40));
        mockedCartRom.Setup<String>(x => x.Name).Returns("MockedCartridge");
        mockedCartRom.Setup<Int32>(x => x.Clockrate).Returns(60);
        mockedCartRom.Setup<Int64>(x => x.EntryPoint).Returns(0x80000000);
        mockedCartRom.Setup<GameSerial>(x => x.Serial).Returns(serial);
        mockedCartRom.Setup<Int32>(x => x.CRC1).Returns(0x00);
        mockedCartRom.Setup<Int32>(x => x.CRC2).Returns(0x00);
        mockedCartRom.Setup<Boolean>(x => x.IsHeaderOnly).Returns(false);
        mockedCartRom.Setup<IBootRom>(x => x.BootRomInformation).Returns(mockedBootRom.Object);
        mockedCartRom.Setup<RegionType>(x => x.Region).Returns(region);
        mockedCartRom.Setup<Stream>(x => x.RomStream).Returns(stream);

        mockedCart.SetupProperty<Boolean>(c => c.IsOpened, true);
        mockedCart.Setup<Stream>(c => c.PiCartridgeStream).Returns(stream);
        mockedCart.Setup<ICartRom>(c => c.RomImage).Returns(mockedCartRom.Object);

        return mockedCart.Object;
    }

    public static class MachineFactory
    {
        public static Machine Create(BootMode mode = BootMode.HLE_IPL, Cartridge cart = null, Boolean breakAtDebug = false)
        {
            Machine machine = new Machine();

            Task.Run(() =>
            {
                machine.SystemBootMode = mode;

                if (cart != null)
                {
                    machine.CartridgeInsert(cart);
                }

                if (breakAtDebug)
                {
                    MipsDebugger debugger = new MipsDebugger();
                    debugger.Attach();
                    debugger.StartDebugging();
                    debugger.Step();
                }
            });

            if (breakAtDebug)
            {
                /* Wait for the machine to boot and then break execution */
                while (!machine.IsPaused)
                    Thread.Sleep(200);
            }

            return machine;
        }
    }

    public static Logger InitNLog(String testName)
    {
        String logFilePath = $"{Environment.CurrentDirectory}\\logs\\testTrace.txt";

        /* Insert a new line in the log */
        var writer = File.AppendText(logFilePath);
        writer.WriteLine();
        writer.Close();

        var config = new LoggingConfiguration();
        FileTarget target = new FileTarget();
        target.Layout = "${logger} :: [${level}] ${message}";
        target.CreateDirs = true;
        target.FileName = logFilePath;
        target.KeepFileOpen = false;
        target.DeleteOldFileOnStartup = false;
        target.Encoding = Encoding.ASCII;
        var emuLogRule = new LoggingRule("*", LogLevel.Trace, target);
        config.LoggingRules.Add(emuLogRule);
        LogManager.Configuration = config;

        var logger = LogManager.GetLogger(testName);
        logger.Trace("---------- Test Started -------------");
        return logger;
    }
}

public sealed class LogEnabledTestAttribute : BeforeAfterTestAttribute
{
    private static Logger logger;

    public override void Before(MethodInfo methodUnderTest)
    {
        logger = Common.InitNLog(methodUnderTest.Name);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        logger.Trace("---------- Test Ended ---------------");
    }
}