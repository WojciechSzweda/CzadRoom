﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface ILogger
    {
        Task Log(string message);
    }
}
