﻿using Soft64;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64UI
{
    public sealed class MemoryEditorForm : CEFWindowForm
    {

        public MemoryEditorForm() : base(MakeLocalUrl("MemoryEditor"))
        {

        }

        protected override void InitializeComponent()
        {
            Text = "Soft64 UI: Memory Editor";

            base.InitializeComponent();
        }

        protected override void RegisterJSObjects()
        {
            HostBrowser.RegisterJsObject("memory", Machine.Current.N64MemorySafe);

            base.RegisterJSObjects();
        }

        public Byte[] ReadMem(Int32 address, Int32 size)
        {
            Byte[] buffer = new Byte[size];
            Machine.Current.N64MemorySafe.Read(buffer, 0, size);
            return buffer;
        }
    }
}
