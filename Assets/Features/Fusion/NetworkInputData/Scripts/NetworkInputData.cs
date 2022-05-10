using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace FusionNetworkTest.General
{
    public struct NetworkInputData : INetworkInput
    {
        public const byte MOUSEBUTTON1 = 0x01;
        public const byte MOUSEBUTTON2 = 0x02;
        public Vector3 direction;
        public byte buttons;
    }
}
