using Microsoft.Maui.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Model
{
    public class CustomPin : Pin
    {
        public bool isVisited {  get; set; }
        public int id { get; set; }
    }
}
