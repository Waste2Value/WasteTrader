﻿using System;
using Starcounter;
using WasteTrader.Api;

namespace WasteTrader
{
    class Program
    {
        static void Main()
        {
            var mainHandlers = new MainHandlers();

            mainHandlers.Register();

        }
    }
}