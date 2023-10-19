using UnityEngine;

namespace Game.Characters.Player
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Data/MovementSettings")]
    public class MovementSettings : ScriptableObject
    {
        [Tooltip("Скорость ходьбы в метрах в секунду")]
        public float MoveSpeed = 4.0f;

        [Tooltip("Как быстро игрок поворачивается лицом по направлению движения")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Ускорение и торможение")]
        public float SpeedChangeRate = 10.0f;
        
        [Space(10)]
        [Tooltip("Высота прыжка")]
        public float JumpHeight = 1.2f;

        [Tooltip("Отдельная гравитация для персонажа. По умолчанию -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Задержка перед возможностью прыгнуть снова")]
        public float JumpTimeout = 0.50f;
        
        [Space(10)]
        [Tooltip("Задержка перед возможностью прыгнуть от стены снова")]
        public float WallJumpTimeout = 0.30f;

        [Tooltip("Задержка перед переходом в состояние падения. Может пригодиться при ходьбе по ступенькам")]
        public float FallTimeout = 0.15f;

        [Space(10)] 
        [Tooltip("Наклон поверхности считающийся стеной: 0 - 90 градусов, 1 - 180 градусов (обычный пол)")]
        [Range(0.0f, 1f)]
        public float WallDetection = 0.2f;
        
        [Tooltip("Угол при отталкивании от стены при зажатой кнопке вбок")]
        public float WallJumpSideAngle = 90f;
        [Tooltip("Угол при отталкивании от стены в других случаях")]
        public float WallJumpDefaultAngle = 15f;
        [Tooltip("Как долго игрока поворачивает от стены")]
        public float WallRotationDuration = 1f;
    }
}