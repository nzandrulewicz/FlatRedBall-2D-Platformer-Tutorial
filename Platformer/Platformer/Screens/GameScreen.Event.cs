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
        void OnPlayerVsCoinCollided (Entities.Player player, Entities.Coin coin) 
        {
            coin.Destroy();
            score += 100;
            GumScreen.ScoreInstance.Text = score.ToString();
        }
        void OnPlayerVsDoorCollided (Entities.Player player, Entities.Door door) 
        {
            MoveToScreen(door.TargetLevel);
        }
        
    }
}
