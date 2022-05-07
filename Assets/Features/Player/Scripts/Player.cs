using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionNetworkTest.General;
using UnityEngine;

namespace FusionNetworkTest.Player
{
    public class Player : NetworkBehaviour
    {
        private NetworkCharacterControllerPrototype _networkCharacterControllerPrototype;

        private void Awake()
        {
            TryGetComponent(out _networkCharacterControllerPrototype);
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data))
                return;
            _networkCharacterControllerPrototype.Move(data.direction * (5 * Runner.DeltaTime));
        }
    }
}