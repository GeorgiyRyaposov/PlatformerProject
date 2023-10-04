using System;
using Common.ServiceLocator;
using Game.Data;
using StarterAssets;
using UnityEngine;
using Zenject;

namespace Game.Characters.Player
{
    [CreateAssetMenu(fileName = "PlayerCharacterController", menuName = "Services/PlayerCharacterController")]
    public class PlayerCharacterController : ScriptableObject, IInitializableService, ITickable
    {
        [SerializeField] ThirdPersonController thirdPersonControllerPrefab;
        [SerializeField] PlayerFollowCameraController playerFollowCameraControllerPrefab;

        private IDataController dataController;
        private Transform playerTransform; 
        
        public void Initialize()
        {
            dataController = ServicesMediator.DataController;
        }
        public void Dispose()
        {
            ServicesMediator.TickableService.Remove(this);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void SpawnPlayer(Vector3 spawnPoint)
        {
            var thirdPersonController = Instantiate(thirdPersonControllerPrefab, spawnPoint, Quaternion.identity);
            var playerFollowCameraController = Instantiate(playerFollowCameraControllerPrefab);

            playerFollowCameraController.SetFollowTarget(thirdPersonController.CinemachineCameraTarget);

            playerTransform = thirdPersonController.transform;
            
            ServicesMediator.TickableService.Add(this);
        }

        public void Tick()
        {
            if (!playerTransform.hasChanged)
            {
                return;
            }
            
            playerTransform.hasChanged = false;
            dataController.SetPlayerPosition(playerTransform.position);
        }
    }
}