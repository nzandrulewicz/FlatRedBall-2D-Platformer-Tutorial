using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Screens;
using Platformer.Entities;
using Platformer.Screens;
namespace Platformer.Screens
{
    public partial class GameScreen
    {
        void OnPlayerVsCoinCollidedTunnel (Entities.Player player, Entities.Coin coin) 
        {
            if (this.PlayerVsCoinCollided != null)
            {
                PlayerVsCoinCollided(player, coin);
            }
        }
        void OnPlayerVsDoorCollidedTunnel (Entities.Player player, Entities.Door door) 
        {
            if (this.PlayerVsDoorCollided != null)
            {
                PlayerVsDoorCollided(player, door);
            }
        }
    }
}
