﻿using System;
using System.IO;
using Moq;
using Soft64;
using Soft64.Debugging;
using Soft64.MipsR4300;
using Soft64.PI;
using Xunit;
using Soft64.MipsR4300.Debugging;

namespace Soft64.TestUnits
{
    public sealed class PJ64BootTests
    {
        [Fact]
        public void NtscPif6101()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.NTSC, CICKeyType.CIC_X101));
        }

        [Fact]
        public void NtscPif6102()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.NTSC, CICKeyType.CIC_X102));
        }

        [Fact]
        public void NtscPif6103()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.NTSC, CICKeyType.CIC_X103));
        }

        [Fact]
        public void NtscPif6105()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.NTSC, CICKeyType.CIC_X105));
        }

        [Fact]
        public void NtscPif6106()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.NTSC, CICKeyType.CIC_X106));
        }

        [Fact]
        public void PalPif6101()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.PAL, CICKeyType.CIC_X101));
        }

        [Fact]
        public void PalPif6102()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.PAL, CICKeyType.CIC_X102));
        }

        [Fact]
        public void PalPif6103()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.PAL, CICKeyType.CIC_X103));
        }

        [Fact]
        public void PalPif6105()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.PAL, CICKeyType.CIC_X105));
        }

        [Fact]
        public void PalPif6106()
        {
            TestBootSequence(Common.MockUpCartridge(RegionType.PAL, CICKeyType.CIC_X106));
        }

        private void TestBootSequence(Cartridge cart)
        {
            Machine machine = Common.MachineFactory.Create(BootMode.HLE_IPL_OLD, cart, true);

            ExecutionState state = machine.DeviceCPU.State;

            /* CP0 Testing */
            Assert.Equal(0x0000001FUL, state.CP0Regs.Random);
            Assert.Equal(0x00005000UL, state.CP0Regs.Count);
            Assert.Equal(0x0000005CUL, state.CP0Regs.Cause);
            Assert.Equal(0x007FFFF0UL, state.CP0Regs.Context);
            Assert.Equal(0xFFFFFFFFUL, state.CP0Regs.EPC);
            Assert.Equal(0xFFFFFFFFUL, state.CP0Regs.BadVAddr);
            Assert.Equal(0xFFFFFFFFUL, state.CP0Regs.ErrorEPC);
            Assert.Equal(0x0006E463UL, state.CP0Regs.Config);
            Assert.Equal(0x34000000UL, state.CP0Regs.Status);

            /* PIF Testing */
            AssertPIFCodes(
                Cartridge.Current.RomImage.Region,
                Cartridge.Current.RomImage.BootRomInformation.CIC,
                state);

            machine.Run();
            machine.Stop();
            machine.Dispose();
        }

        private void AssertPIFCodes(RegionType region, CICKeyType cic, ExecutionState state)
        {
            Assert.Equal(0x00000000A4000040L, state.PC);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[0]);
            Assert.Equal(0xFFFFFFFFA4001F0CUL, state.GPRRegs[6]);
            Assert.Equal(0xFFFFFFFFA4001F08UL, state.GPRRegs[7]);
            Assert.Equal(0x00000000000000C0UL, state.GPRRegs[8]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[9]);
            Assert.Equal(0x0000000000000040UL, state.GPRRegs[10]);
            Assert.Equal(0xFFFFFFFFA4000040UL, state.GPRRegs[11]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[16]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[17]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[18]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[19]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[21]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[26]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[27]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[28]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[28]);
            Assert.Equal(0xFFFFFFFFA4001FF0UL, state.GPRRegs[29]);
            Assert.Equal(0x0000000000000000UL, state.GPRRegs[30]);

            switch (Cartridge.Current.RomImage.Region)
            {
                case RegionType.MPAL:
                case RegionType.PAL:
                    {
                        switch (Cartridge.Current.RomImage.BootRomInformation.CIC)
                        {
                            default:
                            case CICKeyType.Unknown:
                            case CICKeyType.CIC_X102:
                                {
                                    Assert.Equal(0xFFFFFFFFC0F1D859UL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000002DE108EAUL, state.GPRRegs[14]);
                                    Assert.Equal(0x0000000000000000UL, state.GPRRegs[24]);

                                    break;
                                }
                            case CICKeyType.CIC_X101:
                                {
                                    break;
                                }
                            case CICKeyType.CIC_X103:
                                {
                                    Assert.Equal(0xFFFFFFFFD4646273UL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000001AF99984UL, state.GPRRegs[14]);
                                    Assert.Equal(0x0000000000000000UL, state.GPRRegs[24]);
                                    break;
                                }
                            case CICKeyType.CIC_X105:
                                {
                                    Assert.Equal(0xFFFFFFFFDECAAAD1UL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000000CF85C13UL, state.GPRRegs[14]);
                                    Assert.Equal(0x0000000000000002UL, state.GPRRegs[24]);
                                    AssertEqualMemoryReadU32(0xA4001004, 0xBDA807FC);
                                    break;
                                }
                            case CICKeyType.CIC_X106:
                                {
                                    Assert.Equal(0xFFFFFFFFB04DC903UL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000001AF99984UL, state.GPRRegs[14]);
                                    Assert.Equal(0x0000000000000002UL, state.GPRRegs[24]);
                                    break;
                                }
                        }

                        Assert.Equal(0x0000000000000000UL, state.GPRRegs[20]);
                        Assert.Equal(0x0000000000000006UL, state.GPRRegs[23]);
                        Assert.Equal(0xFFFFFFFFA4001554UL, state.GPRRegs[31]);
                        break;
                    }

                default:
                case RegionType.Unknown:
                case RegionType.NTSC:
                    {
                        switch (Cartridge.Current.RomImage.BootRomInformation.CIC)
                        {
                            default:
                            case CICKeyType.Unknown:
                            case CICKeyType.CIC_X102:
                                {
                                    Assert.Equal(0xFFFFFFFFC95973D5UL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000002449A366UL, state.GPRRegs[14]);
                                    break;
                                }
                            case CICKeyType.CIC_X101:
                                {
                                    Assert.Equal(0x000000000000003FUL, state.GPRRegs[22]);
                                    break;
                                }
                            case CICKeyType.CIC_X103:
                                {
                                    Assert.Equal(0xFFFFFFFF95315A28UL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000005BACA1DFUL, state.GPRRegs[14]);
                                    break;
                                }
                            case CICKeyType.CIC_X105:
                                {
                                    AssertEqualMemoryReadU32(0xA4001004, 0x8DA807FC);
                                    Assert.Equal(0x000000005493FB9AUL, state.GPRRegs[5]);
                                    Assert.Equal(0xFFFFFFFFC2C20384UL, state.GPRRegs[14]);
                                    break;
                                }
                            case CICKeyType.CIC_X106:
                                {
                                    Assert.Equal(0xFFFFFFFFE067221FUL, state.GPRRegs[5]);
                                    Assert.Equal(0x000000005CD2B70FUL, state.GPRRegs[14]);
                                    break;
                                }
                        }

                        Assert.Equal(0x0000000000000001UL, state.GPRRegs[20]);
                        Assert.Equal(0x0000000000000000UL, state.GPRRegs[23]);
                        Assert.Equal(0x0000000000000003UL, state.GPRRegs[24]);
                        Assert.Equal(0xFFFFFFFFA4001550UL, state.GPRRegs[31]);
                        break;
                    }
            }

            switch (cic)
            {
                case CICKeyType.CIC_X101:
                    {
                        Assert.Equal(0x000000000000003FUL, state.GPRRegs[22]);
                        break;
                    }
                case CICKeyType.Unknown:
                default:
                case CICKeyType.CIC_X102:
                    {
                        Assert.Equal(0x0000000000000001UL, state.GPRRegs[1]);
                        Assert.Equal(0x000000000EBDA536UL, state.GPRRegs[2]);
                        Assert.Equal(0x000000000EBDA536UL, state.GPRRegs[3]);
                        Assert.Equal(0x000000000000A536UL, state.GPRRegs[4]);
                        Assert.Equal(0xFFFFFFFFED10D0B3UL, state.GPRRegs[12]);
                        Assert.Equal(0x000000001402A4CCUL, state.GPRRegs[13]);
                        Assert.Equal(0x000000003103E121UL, state.GPRRegs[15]);
                        Assert.Equal(0x000000000000003FUL, state.GPRRegs[22]);
                        Assert.Equal(0xFFFFFFFF9DEBB54FUL, state.GPRRegs[25]);
                        break;
                    }
                case CICKeyType.CIC_X103:
                    {
                        Assert.Equal(0x0000000000000001UL, state.GPRRegs[1]);
                        Assert.Equal(0x0000000049A5EE96UL, state.GPRRegs[2]);
                        Assert.Equal(0x0000000049A5EE96UL, state.GPRRegs[3]);
                        Assert.Equal(0x000000000000EE96UL, state.GPRRegs[4]);
                        Assert.Equal(0xFFFFFFFFCE9DFBF7UL, state.GPRRegs[12]);
                        Assert.Equal(0xFFFFFFFFCE9DFBF7UL, state.GPRRegs[13]);
                        Assert.Equal(0x0000000018B63D28UL, state.GPRRegs[15]);
                        Assert.Equal(0x0000000000000078UL, state.GPRRegs[22]);
                        Assert.Equal(0xFFFFFFFF825B21C9UL, state.GPRRegs[25]);
                        break;
                    }
                case CICKeyType.CIC_X105:
                    {
                        AssertEqualMemoryReadU32(0xA4001000, 0x3C0DBFC0);
                        AssertEqualMemoryReadU32(0xA4001008, 0x25AD07C0);
                        AssertEqualMemoryReadU32(0xA400100C, 0x31080080);
                        AssertEqualMemoryReadU32(0xA4001010, 0x5500FFFC);
                        AssertEqualMemoryReadU32(0xA4001014, 0x3C0DBFC0);
                        AssertEqualMemoryReadU32(0xA4001018, 0x8DA80024);
                        AssertEqualMemoryReadU32(0xA400101C, 0x3C0BB000);
                        Assert.Equal(0x0000000000000000UL, state.GPRRegs[1]);
                        Assert.Equal(0xFFFFFFFFF58B0FBFUL, state.GPRRegs[2]);
                        Assert.Equal(0xFFFFFFFFF58B0FBFUL, state.GPRRegs[3]);
                        Assert.Equal(0x0000000000000FBFUL, state.GPRRegs[4]);
                        Assert.Equal(0xFFFFFFFF9651F81EUL, state.GPRRegs[12]);
                        Assert.Equal(0x000000002D42AAC5UL, state.GPRRegs[13]);
                        Assert.Equal(0x0000000056584D60UL, state.GPRRegs[15]);
                        Assert.Equal(0x0000000000000091UL, state.GPRRegs[22]);
                        Assert.Equal(0xFFFFFFFFCDCE565FUL, state.GPRRegs[25]);
                        break;
                    }
                case CICKeyType.CIC_X106:
                    {
                        Assert.Equal(0x0000000000000000UL, state.GPRRegs[1]);
                        Assert.Equal(0xFFFFFFFFA95930A4UL, state.GPRRegs[2]);
                        Assert.Equal(0xFFFFFFFFA95930A4UL, state.GPRRegs[3]);
                        Assert.Equal(0x00000000000030A4UL, state.GPRRegs[4]);
                        Assert.Equal(0xFFFFFFFFBCB59510UL, state.GPRRegs[12]);
                        Assert.Equal(0xFFFFFFFFBCB59510UL, state.GPRRegs[13]);
                        Assert.Equal(0x000000007A3C07F4UL, state.GPRRegs[15]);
                        Assert.Equal(0x0000000000000085UL, state.GPRRegs[22]);
                        Assert.Equal(0x00000000465E3F72UL, state.GPRRegs[25]);
                        break;
                    }
            }
        }

        private void AssertEqualMemoryReadU32(Int64 offset, UInt32 expected)
        {
            BinaryReader reader = new BinaryReader(Machine.Current.DeviceCPU.VirtualMemoryStream);
            reader.BaseStream.Position = offset;
            Assert.Equal(expected, reader.ReadUInt32());
        }
    }
}