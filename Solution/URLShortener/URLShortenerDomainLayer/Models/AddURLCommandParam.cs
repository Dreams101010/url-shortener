﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortenerDomainLayer.Models
{
    public class AddURLCommandParam
    {
        public URLDomainModel URLToAdd { get; set; }
    }
}