﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Services
{
    public interface IMailService
    {
        void Send(string subjectt, string messagee);
    }
}
