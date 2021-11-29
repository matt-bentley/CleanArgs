﻿using CleanArgs.Marshalers.Abstract;
using System.Collections.Generic;

namespace CleanArgs.Marshalers
{
    public class BooleanArgumentMarshaler : ArgumentMarshaler<bool>
    {
        public override bool Parse(List<string> values)
        {
            return true;
        }
    }
}