using Cinemachine;
using UnityEngine;

namespace Game.Characters.Player
{
    public class PlayerFollowCameraController : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineVirtualCamera virtualCamera;

        public void SetFollowTarget(GameObject cinemachineCameraTarget)
        {
            virtualCamera.Follow = cinemachineCameraTarget.transform;
        }
    }
}