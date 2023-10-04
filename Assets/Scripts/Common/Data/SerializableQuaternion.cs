using UnityEngine;

namespace Common.Data
{
    [System.Serializable]
    public struct SerializableQuaternion
    {
        /// <summary>
        /// x component
        /// </summary>
        public float X;
     
        /// <summary>
        /// y component
        /// </summary>
        public float Y;
     
        /// <summary>
        /// z component
        /// </summary>
        public float Z;
     
        /// <summary>
        /// w component
        /// </summary>
        public float W;
     
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        /// <param name="rZ"></param>
        /// <param name="rW"></param>
        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            X = rX;
            Y = rY;
            Z = rZ;
            W = rW;
        }
     
        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}, {W}]";
        }
     
        /// <summary>
        /// Automatic conversion from SerializableQuaternion to Quaternion
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.X, rValue.Y, rValue.Z, rValue.W);
        }
     
        /// <summary>
        /// Automatic conversion from Quaternion to SerializableQuaternion
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }
}