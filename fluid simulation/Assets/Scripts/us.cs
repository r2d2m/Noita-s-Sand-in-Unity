using System;
using System.Collections.Generic;
using UnityEngine;
    public class us
    {
        /// <summary>
        ///  Checks if couple of float values is in a list of couples
        /// </summary>
        public static bool listContains(List<(float,float)> list, (float,float) couple,float precision = 0.01f)
        {
            foreach ((float, float) listElement in list)
            {
                if ((Math.Abs(couple.Item1 - listElement.Item1) < precision) && Math.Abs(couple.Item2 - listElement.Item2) < precision)
                { return true; }
            }
            return false;
        }
    }
