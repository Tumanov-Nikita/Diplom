﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPLOM.Model
{
    public class Compatibility
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Compatibility(string name)
        {
            Name = name;
        }

        public Compatibility()
        {
        }
    }
}