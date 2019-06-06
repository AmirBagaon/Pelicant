using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Pelicant.Models
{
    //public class RobotModel : INotifyPropertyChanged
    //{
    public class RobotModel : IRobotModel
    {
        //delegate string
        //public event PropertyChangedEventHandler PropertyChanged;
        public int width { get; set; }
        public int height { get; set; }
        public string color { get; set; }
        public string x { get; set; }
        public string y { get; set; }

        public RobotModel()
        {
            width = 10;
            height = 10;
            color = "blue";
            x = "10";
            y = "30";
        }

        public JObject getLocation()
        {
            JObject j = new JObject(this.x, this.y);
            return j;
        }

        public string getBattery()
        {
            throw new NotImplementedException();
        }

        public string getMessage()
        {
            throw new NotImplementedException();
        }

        public string getStatus()
        {
            throw new NotImplementedException();
        }
    }
}
