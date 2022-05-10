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
        private Vector3 _forward;
        [SerializeField] private Ball _prefabBall;
        [SerializeField] private PhysicsBall _prefabPhysicsall;
        [Networked] private TickTimer delay { get; set; }

        [Networked(OnChanged = nameof(OnBallSpawned))]
        public NetworkBool isBallSpawned { get; set; }

        private Transform _transform;
        private Material _material;

        Material material
        {
            get
            {
                if (_material == default)
                    _material = GetComponentInChildren<MeshRenderer>().material;
                return _material;
            }
        }

        private void Awake()
        {
            TryGetComponent(out _networkCharacterControllerPrototype);
            TryGetComponent(out _transform);
        }

        public override void Render()
        {
            material.color = Color.Lerp(material.color, Color.white, Time.deltaTime);
        }

        public static void OnBallSpawned(Changed<Player> changed)
        {
            changed.Behaviour.material.color = Color.green;
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data))
                return;


            CheckMovement(data);
            CheckForSpawn(data);
        }

        void CheckForSpawn(NetworkInputData data)
        {
            CheckForBallSpawn(data);
            CheckForPhysiscBallSpawn(data);
        }

        void CheckForPhysiscBallSpawn(NetworkInputData data)
        {
            if (!delay.ExpiredOrNotRunning(Runner))
                return;
            if ((data.buttons & NetworkInputData.MOUSEBUTTON2) == 0)
                return;
            delay = TickTimer.CreateFromSeconds(Runner, .5f);
            Runner.Spawn(_prefabPhysicsall, _transform.position + _forward, Quaternion.LookRotation(_forward),
                Object.InputAuthority, (runner, netObject) =>
                {
                    if (!netObject.TryGetComponent(out PhysicsBall ball))
                        return;
                    ball.Init(10*_forward);
                });
            isBallSpawned = !isBallSpawned;
        }

        void CheckMovement(NetworkInputData data)
        {
            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;
            _networkCharacterControllerPrototype.Move(data.direction * (5 * Runner.DeltaTime));
        }

        void CheckForBallSpawn(NetworkInputData data)
        {
            if (!delay.ExpiredOrNotRunning(Runner))
                return;
            if ((data.buttons & NetworkInputData.MOUSEBUTTON1) == 0)
                return;
            delay = TickTimer.CreateFromSeconds(Runner, .5f);
            Runner.Spawn(_prefabBall, _transform.position + _forward, Quaternion.LookRotation(_forward),
                Object.InputAuthority, (runner, netObject) =>
                {
                    if (!netObject.TryGetComponent(out Ball ball))
                        return;
                    ball.Init();
                });
            isBallSpawned = !isBallSpawned;
        }
    }
}