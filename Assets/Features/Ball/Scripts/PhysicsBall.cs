using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace FusionNetworkTest.Player
{
    public class PhysicsBall : NetworkBehaviour
    {
        [Networked] private TickTimer life { get; set; }

        public void Init(Vector3 forward)
        {
            life = TickTimer.CreateFromSeconds(Runner, 5.0f);
            if (!TryGetComponent(out Rigidbody rb))
                return;
            rb.velocity = forward;
        }

        public override void FixedUpdateNetwork()
        {
            if (life.Expired(Runner))
            {
                Runner.Despawn(Object);
                return;
            }
        }
    }
}