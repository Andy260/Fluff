using UnityEngine;
using UnityEngine.EventSystems;

namespace Sheeplosion.Events
{
    /// <summary>
    /// Defines how the player won
    /// </summary>
    public enum PlayerWinState
    {
        AllSheepDestroyed,
        AllGeneratersDestroyed
    }

    /// <summary>
    /// Defines how the player lost
    /// </summary>
    public enum PlayerLoseState
    {
        NoExplosionsLeft,
        DetonatedNukeSheep
    }

    /// <summary>
    /// Interface to listen for Scene Manager events
    /// </summary>
    public interface ISceneManagerEvents : IEventSystemHandler
    {
        /// <summary>
        /// Called when player has either ran out of explosion triggers
        /// or detonated a nuclear sheep
        /// </summary>
        void OnPlayerFailed(PlayerLoseState a_state);

        /// <summary>
        /// Called when player has either destroyed all generators in the level
        /// or destroyed all sheep
        /// </summary>
        void OnPlayerWon(PlayerWinState a_state);
    }
}
