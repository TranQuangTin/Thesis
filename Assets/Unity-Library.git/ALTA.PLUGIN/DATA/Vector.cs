using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Alta.Plugin.Data
{
    [Serializable]
    public class Color : Vector4
    {
        public Color(UnityEngine.Vector4 v)
        {
            this.a = v.w* 255f;
            this.x = v.x * 255f;
            this.y = v.y * 255f;
            this.z = v.z * 255f;
        }
        public Color() : base()
        {

        }
        public Color(float x,float y,float z,float a) : base(x, y, z, a)
        {

        }

        public UnityEngine.Vector4 to4()
        {
            return new UnityEngine.Vector4(this.x / 255, this.y / 255, this.z / 255,this.a/255);
        }

        public UnityEngine.Color toColor()
        {
            return new UnityEngine.Color(this.x / 255, this.y / 255, this.z / 255,this.a/255);
        }
    }
    [Serializable]
    public class Vector4 : Vector3
    {
        [XmlAttribute("a")]
        public float a;
        public Vector4() : base()
        {
            this.a = 1;
        }

        public Vector4(UnityEngine.Vector4 v)
        {
            this.a = v.w;
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public Vector4(float x, float y, float z, float a) : base(x, y, z)
        {
            this.a = a;
        }
        
        public override string ToString()
        {
            return string.Format("[{0},{1},{2},{4}]", this.x, this.y, this.z, this.a);
        }

    }

    [Serializable]
    public class Vector3 : Vector2
    {
        [XmlAttribute("z")]
        public float z;
        public Vector3(UnityEngine.Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }
        public Vector3(float x, float y, float z) : base(x, y)
        {
            this.z = z;
        }

        public Vector3() : base()
        {
            this.z = 0;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1},{2}]", this.x, this.y, this.z);
        }
    }
    [Serializable]
    public class Vector2
    {
        [XmlAttribute("x")]
        public float x;
        [XmlAttribute("y")]
        public float y;

        public Vector2()
        {
            this.x = this.y = 0;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;

        }

        public Vector2(UnityEngine.Vector2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }
        public override string ToString()
        {
            return string.Format("[{0},{1}]", this.x, this.y);
        }
    }
}
