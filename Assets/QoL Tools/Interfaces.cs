using UnityEngine;

namespace MonoWaves.QoL
{
    // Not recomended for multiplayer games
    public interface IPlayerInputs
    {
        [HideInInspector] public Vector2 Inputs { get; set; }

        public void KeyboardReciever();
    }
}