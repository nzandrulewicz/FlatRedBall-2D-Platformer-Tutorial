#pragma warning disable
#if ANDROID || IOS || DESKTOP_GL
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
#define SUPPORTS_GLUEVIEW_2
using Color = Microsoft.Xna.Framework.Color;
using System.Linq;
using FlatRedBall;
using System;
using System.Collections.Generic;
using System.Text;
namespace Platformer.Screens
{
    public partial class Level1 : GameScreen, FlatRedBall.Gum.IGumScreenOwner
    {
        #if DEBUG
        public static bool HasBeenLoadedWithGlobalContentManager { get; private set; }= false;
        #endif
        protected static Platformer.GumRuntimes.Level1GumRuntime Level1Gum;
        protected static FlatRedBall.TileGraphics.LayeredTileMap Level1Map;
        
        private Platformer.Entities.Coin Coin1;
        private Platformer.Entities.Coin Coin2;
        private Platformer.Entities.Coin Coin3;
        private Platformer.Entities.Door Door1;
        Platformer.FormsControls.Screens.Level1GumForms Forms;
        Platformer.GumRuntimes.Level1GumRuntime GumScreen;
        public Level1 () 
        	: this ("Level1")
        {
        }
        public Level1 (string contentManagerName) 
        	: base (contentManagerName)
        {
            // skipping instantiation of FlatRedBall.Math.PositionedObjectList<Player> PlayerList in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
            // skipping instantiation of FlatRedBall.Math.PositionedObjectList<Coin> CoinList in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
            // skipping instantiation of FlatRedBall.Math.PositionedObjectList<Door> DoorList in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
        }
        public override void Initialize (bool addToManagers) 
        {
            LoadStaticContent(ContentManagerName);
            Map = Level1Map;
            mSolidCollision = new FlatRedBall.TileCollisions.TileShapeCollection(); mSolidCollision.Name = "SolidCollision";
            mCloudCollision = new FlatRedBall.TileCollisions.TileShapeCollection(); mCloudCollision.Name = "CloudCollision";
            // skipping instantiation of FlatRedBall.Math.PositionedObjectList<Player> PlayerList in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
            // skipping instantiation of Player Player1 in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
            // skipping instantiation of FlatRedBall.Math.PositionedObjectList<Coin> CoinList in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
            Coin1 = new Platformer.Entities.Coin(ContentManagerName, false);
            Coin1.Name = "Coin1";
            Coin1.CreationSource = "Glue";
            Coin2 = new Platformer.Entities.Coin(ContentManagerName, false);
            Coin2.Name = "Coin2";
            Coin2.CreationSource = "Glue";
            Coin3 = new Platformer.Entities.Coin(ContentManagerName, false);
            Coin3.Name = "Coin3";
            Coin3.CreationSource = "Glue";
            // skipping instantiation of FlatRedBall.Math.PositionedObjectList<Door> DoorList in Screens\Level1 (Screen) because it has its InstantiatedByBase set to true
            Door1 = new Platformer.Entities.Door(ContentManagerName, false);
            Door1.Name = "Door1";
            Door1.CreationSource = "Glue";
            GumScreen = Level1Gum;
            Forms = Level1Gum.FormsControl ?? new Platformer.FormsControls.Screens.Level1GumForms(Level1Gum);
            
            
            base.Initialize(addToManagers);
            this.Name = "Level1";
        }
        public override void AddToManagers () 
        {
            mAccumulatedPausedTime = TimeManager.CurrentTime;
            mTimeScreenWasCreated = FlatRedBall.TimeManager.CurrentTime;
            Level1Gum.AddToManagers(global::RenderingLibrary.SystemManagers.Default, null);
            Level1Map.AddToManagers(mLayer);
            InitializeFactoriesAndSorting();
            
            
            Coin1.AddToManagers(mLayer);
            Coin2.AddToManagers(mLayer);
            Coin3.AddToManagers(mLayer);
            Door1.AddToManagers(mLayer);
            base.AddToManagers();
            CustomInitialize();
        }
        public override void Activity (bool firstTimeCalled) 
        {
            if (!IsPaused)
            {
                
                Level1Gum?.AnimateSelf(FlatRedBall.TimeManager.SecondDifference);
                Level1Map?.AnimateSelf();;
            }
            else
            {
            }
            base.Activity(firstTimeCalled);
            if (!IsActivityFinished)
            {
                CustomActivity(firstTimeCalled);
            }
        }
        public override void ActivityEditMode () 
        {
            if (FlatRedBall.Screens.ScreenManager.IsInEditMode)
            {
                CustomActivityEditMode();
                base.ActivityEditMode();
            }
        }
        public override void Destroy () 
        {
            base.Destroy();
            Level1Gum.RemoveFromManagers();
            Level1Gum = null;
            Level1Map.Destroy();
            Level1Map = null;
            
            if (Map != null)
            {
                Map.Destroy();
            }
            if (SolidCollision != null)
            {
                SolidCollision.Visible = false;
            }
            if (CloudCollision != null)
            {
                CloudCollision.Visible = false;
            }
            FlatRedBall.Math.Collision.CollisionManager.Self.Relationships.Clear();
            CustomDestroy();
        }
        public override void PostInitialize () 
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            base.PostInitialize();
            if (Map!= null)
            {
            }
            if (SolidCollision!= null)
            {
            }
            if (CloudCollision!= null)
            {
            }
            if (PlayerList!= null)
            {
                if (Player1!= null)
                {
                    if (Player1.Parent == null)
                    {
                        base.Player1.X = 64f;
                    }
                    else
                    {
                        base.Player1.RelativeX = 64f;
                    }
                    if (Player1.Parent == null)
                    {
                        base.Player1.Y = -384f;
                    }
                    else
                    {
                        base.Player1.RelativeY = -384f;
                    }
                }
            }
            if (CoinList!= null)
            {
                if (!CoinList.Contains(Coin1))
                {
                    CoinList.Add(Coin1);
                }
                if (Coin1.Parent == null)
                {
                    Coin1.X = 192f;
                }
                else
                {
                    Coin1.RelativeX = 192f;
                }
                if (Coin1.Parent == null)
                {
                    Coin1.Y = -320f;
                }
                else
                {
                    Coin1.RelativeY = -320f;
                }
                if (!CoinList.Contains(Coin2))
                {
                    CoinList.Add(Coin2);
                }
                if (Coin2.Parent == null)
                {
                    Coin2.X = 288f;
                }
                else
                {
                    Coin2.RelativeX = 288f;
                }
                if (Coin2.Parent == null)
                {
                    Coin2.Y = -288f;
                }
                else
                {
                    Coin2.RelativeY = -288f;
                }
                if (!CoinList.Contains(Coin3))
                {
                    CoinList.Add(Coin3);
                }
                if (Coin3.Parent == null)
                {
                    Coin3.X = 368f;
                }
                else
                {
                    Coin3.RelativeX = 368f;
                }
                if (Coin3.Parent == null)
                {
                    Coin3.Y = -256f;
                }
                else
                {
                    Coin3.RelativeY = -256f;
                }
            }
            if (DoorList!= null)
            {
                if (!DoorList.Contains(Door1))
                {
                    DoorList.Add(Door1);
                }
                if (Door1.Parent == null)
                {
                    Door1.X = 592f;
                }
                else
                {
                    Door1.RelativeX = 592f;
                }
                if (Door1.Parent == null)
                {
                    Door1.Y = -208f;
                }
                else
                {
                    Door1.RelativeY = -208f;
                }
                Door1.TargetLevel = "Level2";
            }
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public override void AddToManagersBottomUp () 
        {
            base.AddToManagersBottomUp();
        }
        public override void RemoveFromManagers () 
        {
            base.RemoveFromManagers();
            Level1Map.Destroy();
            if (Map != null)
            {
                Map.Destroy();
            }
            if (SolidCollision != null)
            {
                SolidCollision.Visible = false;
            }
            if (CloudCollision != null)
            {
                CloudCollision.Visible = false;
            }
        }
        public override void AssignCustomVariables (bool callOnContainedElements) 
        {
            base.AssignCustomVariables(callOnContainedElements);
            if (callOnContainedElements)
            {
                Coin1.AssignCustomVariables(true);
                Coin2.AssignCustomVariables(true);
                Coin3.AssignCustomVariables(true);
                Door1.AssignCustomVariables(true);
            }
            if (Player1.Parent == null)
            {
                base.Player1.X = 64f;
            }
            else
            {
                base.Player1.RelativeX = 64f;
            }
            if (Player1.Parent == null)
            {
                base.Player1.Y = -384f;
            }
            else
            {
                base.Player1.RelativeY = -384f;
            }
            if (Coin1.Parent == null)
            {
                Coin1.X = 192f;
            }
            else
            {
                Coin1.RelativeX = 192f;
            }
            if (Coin1.Parent == null)
            {
                Coin1.Y = -320f;
            }
            else
            {
                Coin1.RelativeY = -320f;
            }
            if (Coin2.Parent == null)
            {
                Coin2.X = 288f;
            }
            else
            {
                Coin2.RelativeX = 288f;
            }
            if (Coin2.Parent == null)
            {
                Coin2.Y = -288f;
            }
            else
            {
                Coin2.RelativeY = -288f;
            }
            if (Coin3.Parent == null)
            {
                Coin3.X = 368f;
            }
            else
            {
                Coin3.RelativeX = 368f;
            }
            if (Coin3.Parent == null)
            {
                Coin3.Y = -256f;
            }
            else
            {
                Coin3.RelativeY = -256f;
            }
            if (Door1.Parent == null)
            {
                Door1.X = 592f;
            }
            else
            {
                Door1.RelativeX = 592f;
            }
            if (Door1.Parent == null)
            {
                Door1.Y = -208f;
            }
            else
            {
                Door1.RelativeY = -208f;
            }
            Door1.TargetLevel = "Level2";
            
        }
        public override void ConvertToManuallyUpdated () 
        {
            base.ConvertToManuallyUpdated();
        }
        public static new void LoadStaticContent (string contentManagerName) 
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            if (string.IsNullOrEmpty(contentManagerName))
            {
                throw new System.ArgumentException("contentManagerName cannot be empty or null");
            }
            Platformer.Screens.GameScreen.LoadStaticContent(contentManagerName);
            // Set the content manager for Gum
            var contentManagerWrapper = new FlatRedBall.Gum.ContentManagerWrapper();
            contentManagerWrapper.ContentManagerName = contentManagerName;
            RenderingLibrary.Content.LoaderManager.Self.ContentLoader = contentManagerWrapper;
            // Access the GumProject just in case it's async loaded
            var throwaway = GlobalContent.GumProject;
            #if DEBUG
            if (contentManagerName == FlatRedBall.FlatRedBallServices.GlobalContentManager)
            {
                HasBeenLoadedWithGlobalContentManager = true;
            }
            else if (HasBeenLoadedWithGlobalContentManager)
            {
                throw new System.Exception( "Level1 has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
            }
            #endif
            if(Level1Gum == null)
{
 var wasSuspended = Gum.Wireframe.GraphicalUiElement.IsAllLayoutSuspended;
 Gum.Wireframe.GraphicalUiElement.IsAllLayoutSuspended = true;
 Level1Gum = (Platformer.GumRuntimes.Level1GumRuntime)GumRuntime.ElementSaveExtensions.CreateGueForElement(Gum.Managers.ObjectFinder.Self.GetElementSave("Level1Gum"), true);
 Gum.Wireframe.GraphicalUiElement.IsAllLayoutSuspended = wasSuspended;
 if(!wasSuspended) { Level1Gum.UpdateFontRecursive();Level1Gum.UpdateLayout(); }
}
            Level1Map = FlatRedBall.TileGraphics.LayeredTileMap.FromTiledMapSave("content/screens/level1/level1map.tmx", contentManagerName);
            CustomLoadStaticContent(contentManagerName);
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public override void PauseThisScreen () 
        {
            StateInterpolationPlugin.TweenerManager.Self.Pause();
            base.PauseThisScreen();
        }
        public override void UnpauseThisScreen () 
        {
            StateInterpolationPlugin.TweenerManager.Self.Unpause();
            base.UnpauseThisScreen();
        }
        private void InitializeFactoriesAndSorting () 
        {
        }
        [System.Obsolete("Use GetFile instead")]
        public static new object GetStaticMember (string memberName) 
        {
            switch(memberName)
            {
                case  "Level1Gum":
                    return Level1Gum;
                case  "Level1Map":
                    return Level1Map;
            }
            return null;
        }
        public static new object GetFile (string memberName) 
        {
            switch(memberName)
            {
                case  "Level1Gum":
                    return Level1Gum;
                case  "Level1Map":
                    return Level1Map;
            }
            return null;
        }
        object GetMember (string memberName) 
        {
            switch(memberName)
            {
                case  "Level1Gum":
                    return Level1Gum;
                case  "Level1Map":
                    return Level1Map;
            }
            return null;
        }
        public static void Reload (object whatToReload) 
        {
        }
        protected override void RefreshLayoutInternal (object sender, EventArgs e) 
        {
            Level1Gum.UpdateLayout();
            base.RefreshLayoutInternal(sender, e);
        }
        partial void CustomActivityEditMode();
    }
}
