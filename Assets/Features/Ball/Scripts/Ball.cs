using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace FusionNetworkTest.Player
{
    public class Ball : NetworkBehaviour
    {
        private Transform _transform;
        [Networked] private TickTimer life { get; set; }

    

        public override void FixedUpdateNetwork()
        {
            if (life.Expired(Runner))
            {
                Runner.Despawn(Object);
                return;
            }

            _transform.position += _transform.forward * (5 * Runner.DeltaTime);
        }

        public override void Spawned()
        {
            TryGetComponent(out _transform);
        }

        public void Init()
        {
            life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        }
    }
}

