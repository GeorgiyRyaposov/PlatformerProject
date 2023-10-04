using System;
using UnityEngine;

namespace Game.Input
{
    public class InputEventsHolder
    {
        public Vector2 Move;
        public Vector2 Look;
        public bool Jump;
        public bool Sprint;
        public bool Interact;

        public event Action Paused = () => { };

        public void OnPauseClick()
        {
            Paused();
        }
    }
}