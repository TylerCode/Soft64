from Soft64 import *
from Soft64.MipsR4300 import *
from System import *
from NLog import *

logger = LogManager.GetLogger("TLB Python Script")
tlb = Machine.Current.DeviceCPU.Tlb

logger.Info("Creating fake entry 0")
entry = TLBEntry()
entry.VPN2 = VirtualPageNumber2(2, 0x345)
tlb.AddEntry(0, entry)

logger.Info("Creating fake entry 1")
entry = TLBEntry()
entry.VPN2 = VirtualPageNumber2(2, 0x645)
entry.PfnEven = PageFrameNumber(0x2000);
entry.PfnEven.IsValid = True;
tlb.AddEntry(1, entry)

logger.Info("Creating fake entry 2")
entry = TLBEntry()
entry.VPN2 = VirtualPageNumber2(2, 0x445)
entry.PfnOdd = PageFrameNumber(0x3000);
entry.PfnOdd.IsValid = True;
tlb.AddEntry(2, entry)

logger.Info("Done!")

