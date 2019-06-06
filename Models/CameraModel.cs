using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pelicant.Models
{
    public class CameraModel:ICameraModel
    {
        //private const string cameraIP = "http://upload.wikimedia.org/wikipedia/commons/7/79/Big_Buck_Bunny_small.ogv";
        private string cameraIP = "https://www.youtube.com/embed/QFgI34iKPyU?autoplay=1&start=1";
        //private string cameraIP = "https://youtu.be/QFgI34iKPyU?t=2";

        public CameraModel()
        {
            //this.cameraIP += "?autoplay=1"; //Set video to autoplay
        }
        public string getCameraIP { get { return cameraIP; } }
        public int width { get; set; }
        public int height { get; set; }

        
    }
}