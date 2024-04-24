#pragma warning disable
#if ANDROID || IOS || DESKTOP_GL
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
#define SUPPORTS_GLUEVIEW_2
using Color = Microsoft.Xna.Framework.Color;
using System.Linq;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using FlatRedBall;
using System;
using System.Collections.Generic;
using System.Text;
using Platformer.DataTypes;
using FlatRedBall.IO.Csv;
namespace Platformer.Entities
{
    public partial class Player : FlatRedBall.PositionedObject, FlatRedBall.Graphics.IDestroyable, FlatRedBall.Entities.IEntity, FlatRedBall.Math.Geometry.ICollidable, IPlatformer, FlatRedBall.Entities.IDamageable
    {
        // This is made static so that static lazy-loaded content can access it.
        public static string ContentManagerName { get; set; }
        #if DEBUG
        private double mLastTimeCalledActivity=-1;
        #endif
        #if DEBUG
        public static bool HasBeenLoadedWithGlobalContentManager { get; private set; }= false;
        #endif
        static object mLockObject = new object();
        static System.Collections.Generic.List<string> mRegisteredUnloads = new System.Collections.Generic.List<string>();
        static System.Collections.Generic.List<string> LoadedContentManagers = new System.Collections.Generic.List<string>();
        protected static FlatRedBall.Graphics.Animation.AnimationChainList PlatformerAnimations;
        public static System.Collections.Generic.Dictionary<System.String, Platformer.DataTypes.PlatformerValues> PlatformerValuesStatic;
        
        private FlatRedBall.Sprite SpriteInstance;
        private FlatRedBall.Math.Geometry.AxisAlignedRectangle mAxisAlignedRectangleInstance;
        public FlatRedBall.Math.Geometry.AxisAlignedRectangle AxisAlignedRectangleInstance
        {
            get
            {
                return mAxisAlignedRectangleInstance;
            }
            private set
            {
                mAxisAlignedRectangleInstance = value;
            }
        }
        private int mTeamIndex = 0;
        public virtual int TeamIndex
        {
            set
            {
                mTeamIndex = value;
            }
            get
            {
                return mTeamIndex;
            }
        }
        private decimal mMaxHealth = 100m;
        public virtual decimal MaxHealth
        {
            set
            {
                mMaxHealth = value;
            }
            get
            {
                return mMaxHealth;
            }
        }
        public event Action<Platformer.DataTypes.PlatformerValues> BeforeGroundMovementSet;
        public event System.EventHandler AfterGroundMovementSet;
        private Platformer.DataTypes.PlatformerValues mGroundMovement;
        public virtual Platformer.DataTypes.PlatformerValues GroundMovement
        {
            set
            {
                if (BeforeGroundMovementSet != null)
                {
                    BeforeGroundMovementSet(value);
                }
                mGroundMovement = value;
                if (AfterGroundMovementSet != null)
                {
                    AfterGroundMovementSet(this, null);
                }
            }
            get
            {
                return mGroundMovement;
            }
        }
        public event Action<Platformer.DataTypes.PlatformerValues> BeforeAirMovementSet;
        public event System.EventHandler AfterAirMovementSet;
        private Platformer.DataTypes.PlatformerValues mAirMovement;
        public virtual Platformer.DataTypes.PlatformerValues AirMovement
        {
            set
            {
                if (BeforeAirMovementSet != null)
                {
                    BeforeAirMovementSet(value);
                }
                mAirMovement = value;
                if (AfterAirMovementSet != null)
                {
                    AfterAirMovementSet(this, null);
                }
            }
            get
            {
                return mAirMovement;
            }
        }
        public event Action<Platformer.DataTypes.PlatformerValues> BeforeAfterDoubleJumpSet;
        public event System.EventHandler AfterAfterDoubleJumpSet;
        private Platformer.DataTypes.PlatformerValues mAfterDoubleJump;
        public virtual Platformer.DataTypes.PlatformerValues AfterDoubleJump
        {
            set
            {
                if (BeforeAfterDoubleJumpSet != null)
                {
                    BeforeAfterDoubleJumpSet(value);
                }
                mAfterDoubleJump = value;
                if (AfterAfterDoubleJumpSet != null)
                {
                    AfterAfterDoubleJumpSet(this, null);
                }
            }
            get
            {
                return mAfterDoubleJump;
            }
        }
        private FlatRedBall.Math.Geometry.ShapeCollection mGeneratedCollision;
        public FlatRedBall.Math.Geometry.ShapeCollection Collision
        {
            get
            {
                return mGeneratedCollision;
            }
        }
        public HashSet<string> ItemsCollidedAgainst { get; private set;} = new HashSet<string>();
        public HashSet<string> LastFrameItemsCollidedAgainst { get; private set;} = new HashSet<string>();
        public HashSet<object> ObjectsCollidedAgainst { get; private set;} = new HashSet<object>();
        public HashSet<object> LastFrameObjectsCollidedAgainst { get; private set;} = new HashSet<object>();
        #region Platformer Fields
        public bool IsPlatformingEnabled = true;
        /// <summary>
        /// See property for information.
        /// </summary>
        bool mIsOnGround = false;
        bool mCanContinueToApplyJumpToHold = false;
        private float lastNonZeroPlatformerHorizontalMaxSpeed = 0;
        /// <summary>
        /// Whether the character has hit its head on a solid
        /// collision this frame. This typically occurs when the
        /// character is moving up in the air. It is used to prevent
        /// upward velocity from being applied while the player is
        /// holding down the jump button.
        /// </summary>
        bool mHitHead = false;
        /// <summary>
        /// The current slope that the character is standing or walking on in degrees relative
        /// to the direction that the character is facing. In other words, if the charater is
        /// walking uphill to the right (positive slope), if the character turns around the value
        /// will be negative.
        /// </summary>
        float currentSlope = 0;
        bool mHasDoubleJumped = false;
        /// <summary>
        /// Whether the character is in the air and has double-jumped.
        /// This is used to determine which movement variables are active,
        /// effectively preventing multiple double-jumps.
        /// </summary>
        public bool HasDoubleJumped
        {
            get => mHasDoubleJumped;
            set { mHasDoubleJumped = value; DetermineMovementValues(); }
        }
        /// <summary>
        /// The time when the jump button was last pushed. This is used to
        /// determine if upward velocity should be applied while the user
        /// holds the jump button down.
        /// </summary>
        double mTimeJumpPushed = double.NegativeInfinity;
        /// <summary>
        /// See property for information.
        /// </summary>
        DataTypes.PlatformerValues mCurrentMovement;
        /// <summary>
        /// See property for information.
        /// </summary>
        HorizontalDirection mDirectionFacing;
        /// <summary>
        /// See property for information.
        /// </summary>
        MovementType mMovementType;
        /// <summary>
        /// The last time collision checks were performed. Time values uniquely
        /// identify a game frame, so this is used to store whether collisions have
        /// been tested this frame or not. This is used to determine whether collision
        /// variables should be reset or not when a collision method is called, as
        /// multiple collisions (such as vs. solid and vs. cloud) may occur in one frame.
        /// </summary>
        double mLastCollisionTime = -1;
        #endregion
        public Microsoft.Xna.Framework.Vector3 PositionBeforeLastPlatformerCollision;
        #region Platformer Properties
        /// <summary>
        /// The MovementValues which were active when the user last jumped.
        /// These are used to determine the upward velocity to apply while
        /// the user holds the jump button.
        /// </summary>
        public DataTypes.PlatformerValues ValuesJumpedWith { get; set; }
        public bool WasOnGroundLastFrame{ get; private set; }
        /// <summary>
        /// Whether the platformer object is forcefully ignoring cloud collision.
        /// Setting this to true will disable all cloud collision. Note that cloud collision
        /// may still be temporarily ignored due to internal logic, such as when holding down
        /// and pressing jump, but if so that will not be reflected in this property.
        /// </summary>
        public bool ForceIgnoreCloudCollision{ get; set; }
        public FlatRedBall.Input.IInputDevice InputDevice
        {
            get;
            private set;
        }
        /// <summary>
        /// Returns the current time, considering whether a Screen is active. 
        /// This is used to control how long a user can hold the jump button during
        /// a jump to apply upward velocity.
        /// </summary>
        double CurrentTime
        {
            get
            {
                if (FlatRedBall.Screens.ScreenManager.CurrentScreen != null)
                {
                    return FlatRedBall.Screens.ScreenManager.CurrentScreen.PauseAdjustedCurrentTime;
                }
                else
                {
                    return FlatRedBall.TimeManager.CurrentTime;
                }
            }
        }
        /// <summary>
        /// The current movement variables used for horizontal movement and jumping.
        /// These automatically get set according to the default platformer logic and should
        /// not be manually adjusted.
        /// </summary>
        protected DataTypes.PlatformerValues CurrentMovement
        {
            get
            {
                return mCurrentMovement;
            }
        }
        public string CurrentMovementName => CurrentMovement?.Name;
        public float MaxAbsoluteXVelocity => CurrentMovement?.MaxSpeedX ?? 0;
        public float MaxAbsoluteYVelocity => CurrentMovement?.MaxFallSpeed ?? 0;
        /// <summary>
        /// Which direction the character is facing. This can be explicity set in code, but may get overridden by the current InputDevice.
        /// </summary>
        public HorizontalDirection DirectionFacing
        {
            get
            {
                return mDirectionFacing;
            }
            set
            {
                mDirectionFacing = value;
            }
        }
        /// <summary>
        /// The input object which controls whether the jump was pressed.
        /// Common examples include a button or keyboard key.
        /// </summary>
        public FlatRedBall.Input.IPressableInput JumpInput { get; set; }
        /// <summary>
        /// The input object which controls the horizontal movement of the character.
        /// Common examples include a d-pad, analog stick, or keyboard keys.
        /// </summary>
        public FlatRedBall.Input.I1DInput HorizontalInput { get; set; }
        /// <summary>
        /// The input object which controls vertical input such as moving on ladders or falling through cloud collision.
        /// -1 represents full down, 0 is neutral, +1 is full up.
        /// </summary>
        public FlatRedBall.Input.I1DInput VerticalInput { get; set; }
        /// <summary>
        /// The ratio that the horizontal input is being held.
        /// -1 represents full left, 0 is neutral, +1 is full right.
        /// </summary>
        protected virtual float HorizontalRatio
        {
            get
            {
                if (!InputEnabled)
                {
                    return 0;
                }
                else
                {
                    return HorizontalInput.Value;
                }
            }
        }
        /// <summary>
        /// Whether the character is on the ground. This is false
        /// if the character has jumped or walked off of the edge
        /// of a platform.
        /// </summary>
        public bool IsOnGround
        {
            get
            {
                return mIsOnGround;
            }
        }
        /// <summary>
        /// The current movement type. This is set by the default platformer logic and
        /// is used to assign the mCurrentMovement variable.
        /// </summary>
        public MovementType CurrentMovementType
        {
            get
            {
                return mMovementType;
            }
            set
            {
                mMovementType = value;
                UpdateCurrentMovement();
            }
        }
        /// <summary>
        /// Whether input is read to control the movement of the character.
        /// This can be turned off if the player should not be able to control
        /// the character.
        /// </summary>
        public bool InputEnabled { get; set; }
        float groundHorizontalVelocity = 0;
        public float GroundHorizontalVelocity => groundHorizontalVelocity;
        

            /// <summary>
            /// Stores the value that the entity must fall down to before cloud collision is re-enabled.
            /// If this value is null, then the player should perform normal cloud collision.
            /// When the entity falls through a
            /// cloud (by pressing down direction + jump), then this value is set to a non-null value. 
            /// </summary>
            private float? cloudCollisionFallThroughY = null;

            public float? TopOfLadderY { get; set; }


        #endregion
        
        /// <summary>
        /// Action for when the character executes a jump.
        /// </summary>
        public System.Action JumpAction;

        /// <summary>
        /// Action for when the character lands from a jump.
        /// </summary>
        public System.Action LandedAction;


        public HashSet<string> GroundCollidedAgainst { get; private set;} = new HashSet<string>();
        Platformer.Entities.PlatformerAnimationController PlatformerAnimationController;
        public string EditModeType { get; set; } = "Platformer.Entities.Player";
        public System.Collections.Generic.Dictionary<FlatRedBall.Entities.IDamageArea, double> DamageAreaLastDamage { get; set; } = new System.Collections.Generic.Dictionary<FlatRedBall.Entities.IDamageArea, double>();
        public Action<decimal, FlatRedBall.Entities.IDamageArea> ReactToDamageReceived { get; set; }
        public Func<decimal, FlatRedBall.Entities.IDamageArea, decimal> ModifyDamageReceived { get; set; }
        public decimal CurrentHealth { get; set; }
        public Action<decimal, FlatRedBall.Entities.IDamageArea> Died { get; set; }
        public bool IsDamageReceivingEnabled { get; set; } = true;
        public double LastDamageTime { get; set; } = -999;
        public double InvulnerabilityTimeAfterDamage { get; set; } = 0;
        protected FlatRedBall.Graphics.Layer LayerProvidedByContainer = null;
        public Player () 
        	: this(FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName, true)
        {
        }
        public Player (string contentManagerName) 
        	: this(contentManagerName, true)
        {
        }
        public Player (string contentManagerName, bool addToManagers) 
        	: base()
        {
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);
        }
        protected virtual void InitializeEntity (bool addToManagers) 
        {
            LoadStaticContent(ContentManagerName);
            SpriteInstance = new FlatRedBall.Sprite();
            SpriteInstance.Name = "SpriteInstance";
            SpriteInstance.CreationSource = "Glue";
            mAxisAlignedRectangleInstance = new FlatRedBall.Math.Geometry.AxisAlignedRectangle();
            mAxisAlignedRectangleInstance.Name = "AxisAlignedRectangleInstance";
            mAxisAlignedRectangleInstance.CreationSource = "Glue";
            
            // this provides default controls for the platformer using either keyboad or 360. Can be overridden in custom code:
            this.InitializeInput();

            // needed to figure out corner collisions
            KeepTrackOfReal = true;

            BeforeGroundMovementSet += (newValue) => 
            {
                if(mGroundMovement != null && mGroundMovement == ValuesJumpedWith && IsOnGround)
                {
                    ValuesJumpedWith = newValue;
                }
            };

            BeforeAirMovementSet += (newValue) => 
            {
                if(mAirMovement != null && mAirMovement == ValuesJumpedWith)
                {
                    ValuesJumpedWith = newValue;
                }
            };

            BeforeAfterDoubleJumpSet += (newValue) =>  
            {
                if(mAfterDoubleJump != null && mAfterDoubleJump == ValuesJumpedWith)
                {
                    ValuesJumpedWith = newValue;
                }
            };
            
            AfterGroundMovementSet += (not, used) => UpdateCurrentMovement();
            AfterAirMovementSet += (not, used) => UpdateCurrentMovement();
            AfterAfterDoubleJumpSet += (not, used) => UpdateCurrentMovement();

            PlatformerAnimationController = new Platformer.Entities.PlatformerAnimationController(this);
            PlatformerAnimationController.AnimatedObject = SpriteInstance;
            {
                var configuration = new PlatformerAnimationConfiguration
                {
                    AnimationName="CharacterIdle",
                    HasLeftAndRight=true,
                    MinXVelocityAbsolute=null,
                    MaxXVelocityAbsolute=null ,
                    MinYVelocity=null ,
                    MaxYVelocity=null ,
                    MinHorizontalInputAbsolute=null ,
                    MaxHorizontalInputAbsolute=null ,
                    AbsoluteXVelocityAnimationSpeedMultiplier=null ,
                    AbsoluteYVelocityAnimationSpeedMultiplier=null ,
                    MaxSpeedXRatioMultiplier=null ,
                    MaxSpeedYRatioMultiplier=null ,
                    OnGroundRequirement=null ,
                    MovementName=null ,
                    AnimationSpeedAssignment=global::Platformer.Entities.AnimationSpeedAssignment.ForceTo1
                };
                PlatformerAnimationController.AddLayer(configuration);
            }
            {
                var configuration = new PlatformerAnimationConfiguration
                {
                    AnimationName="CharacterWalk",
                    HasLeftAndRight=true,
                    MinXVelocityAbsolute=1f,
                    MaxXVelocityAbsolute=null ,
                    MinYVelocity=null ,
                    MaxYVelocity=null ,
                    MinHorizontalInputAbsolute=null ,
                    MaxHorizontalInputAbsolute=null ,
                    AbsoluteXVelocityAnimationSpeedMultiplier=null ,
                    AbsoluteYVelocityAnimationSpeedMultiplier=null ,
                    MaxSpeedXRatioMultiplier=null ,
                    MaxSpeedYRatioMultiplier=null ,
                    OnGroundRequirement=null ,
                    MovementName=null ,
                    AnimationSpeedAssignment=global::Platformer.Entities.AnimationSpeedAssignment.ForceTo1
                };
                PlatformerAnimationController.AddLayer(configuration);
            }
            {
                var configuration = new PlatformerAnimationConfiguration
                {
                    AnimationName="CharacterFall",
                    HasLeftAndRight=true,
                    MinXVelocityAbsolute=null,
                    MaxXVelocityAbsolute=null ,
                    MinYVelocity=null ,
                    MaxYVelocity=null ,
                    MinHorizontalInputAbsolute=null ,
                    MaxHorizontalInputAbsolute=null ,
                    AbsoluteXVelocityAnimationSpeedMultiplier=null ,
                    AbsoluteYVelocityAnimationSpeedMultiplier=null ,
                    MaxSpeedXRatioMultiplier=null ,
                    MaxSpeedYRatioMultiplier=null ,
                    OnGroundRequirement=false ,
                    MovementName=null ,
                    AnimationSpeedAssignment=global::Platformer.Entities.AnimationSpeedAssignment.ForceTo1
                };
                PlatformerAnimationController.AddLayer(configuration);
            }
            {
                var configuration = new PlatformerAnimationConfiguration
                {
                    AnimationName="CharacterJump",
                    HasLeftAndRight=true,
                    MinXVelocityAbsolute=null,
                    MaxXVelocityAbsolute=null ,
                    MinYVelocity=0f ,
                    MaxYVelocity=null ,
                    MinHorizontalInputAbsolute=null ,
                    MaxHorizontalInputAbsolute=null ,
                    AbsoluteXVelocityAnimationSpeedMultiplier=null ,
                    AbsoluteYVelocityAnimationSpeedMultiplier=null ,
                    MaxSpeedXRatioMultiplier=null ,
                    MaxSpeedYRatioMultiplier=null ,
                    OnGroundRequirement=false ,
                    MovementName=null ,
                    AnimationSpeedAssignment=global::Platformer.Entities.AnimationSpeedAssignment.ForceTo1
                };
                PlatformerAnimationController.AddLayer(configuration);
            }
            
            PostInitialize();
            if (addToManagers)
            {
                AddToManagers(null);
            }
        }
        public virtual void ReAddToManagers (FlatRedBall.Graphics.Layer layerToAddTo) 
        {
            LayerProvidedByContainer = layerToAddTo;
            FlatRedBall.SpriteManager.AddPositionedObject(this);
            FlatRedBall.SpriteManager.AddToLayer(SpriteInstance, LayerProvidedByContainer);
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(mAxisAlignedRectangleInstance, LayerProvidedByContainer);
        }
        public virtual void AddToManagers (FlatRedBall.Graphics.Layer layerToAddTo) 
        {
            LayerProvidedByContainer = layerToAddTo;
            FlatRedBall.SpriteManager.AddPositionedObject(this);
            FlatRedBall.SpriteManager.AddToLayer(SpriteInstance, LayerProvidedByContainer);
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(mAxisAlignedRectangleInstance, LayerProvidedByContainer);
            AddToManagersBottomUp(layerToAddTo);
            CurrentMovementType = MovementType.Ground;
            CustomInitialize();
        }
        public virtual void Activity () 
        {
            #if DEBUG
            if(TimeManager.TimeFactor > 0 && mLastTimeCalledActivity > 0 && mLastTimeCalledActivity == FlatRedBall.TimeManager.CurrentScreenTime)
            {
                throw new System.Exception("Activity was called twice in the same frame. This can cause objects to move 2x as fast.");
            }
            mLastTimeCalledActivity = FlatRedBall.TimeManager.CurrentScreenTime;
            #endif
            
            if (IsPlatformingEnabled)
            {
                ApplyInput();
                DetermineMovementValues();
            }
            PlatformerAnimationController.Activity();
            CustomActivity();
        }
        public virtual void ActivityEditMode () 
        {
            CustomActivityEditMode();
        }
        public virtual void Destroy () 
        {
            FlatRedBall.SpriteManager.RemovePositionedObject(this);
            
            if (SpriteInstance != null)
            {
                FlatRedBall.SpriteManager.RemoveSpriteOneWay(SpriteInstance);
            }
            if (AxisAlignedRectangleInstance != null)
            {
                FlatRedBall.Math.Geometry.ShapeManager.RemoveOneWay(AxisAlignedRectangleInstance);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: true);
            CustomDestroy();
        }
        public virtual void PostInitialize () 
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            if (SpriteInstance.Parent == null)
            {
                SpriteInstance.CopyAbsoluteToRelative();
                SpriteInstance.AttachTo(this, false);
            }
            SpriteInstance.TextureScale = 1f;
            SpriteInstance.AnimationChains = PlatformerAnimations;
            SpriteInstance.CurrentChainName = "CharacterWalkRight";
            if (mAxisAlignedRectangleInstance.Parent == null)
            {
                mAxisAlignedRectangleInstance.CopyAbsoluteToRelative();
                mAxisAlignedRectangleInstance.AttachTo(this, false);
            }
            if (AxisAlignedRectangleInstance.Parent == null)
            {
                AxisAlignedRectangleInstance.Y = 8f;
            }
            else
            {
                AxisAlignedRectangleInstance.RelativeY = 8f;
            }
            AxisAlignedRectangleInstance.Width = 10f;
            AxisAlignedRectangleInstance.Height = 16f;
            mGeneratedCollision = new FlatRedBall.Math.Geometry.ShapeCollection();
            Collision.AxisAlignedRectangles.AddOneWay(mAxisAlignedRectangleInstance);
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public virtual void AddToManagersBottomUp (FlatRedBall.Graphics.Layer layerToAddTo) 
        {
            CurrentHealth = MaxHealth;
            AssignCustomVariables(false);
        }
        public virtual void RemoveFromManagers () 
        {
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
            if (SpriteInstance != null)
            {
                FlatRedBall.SpriteManager.RemoveSpriteOneWay(SpriteInstance);
            }
            if (AxisAlignedRectangleInstance != null)
            {
                FlatRedBall.Math.Geometry.ShapeManager.RemoveOneWay(AxisAlignedRectangleInstance);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
        }
        public virtual void AssignCustomVariables (bool callOnContainedElements) 
        {
            if (callOnContainedElements)
            {
            }
            SpriteInstance.TextureScale = 1f;
            SpriteInstance.AnimationChains = PlatformerAnimations;
            SpriteInstance.CurrentChainName = "CharacterWalkRight";
            if (AxisAlignedRectangleInstance.Parent == null)
            {
                AxisAlignedRectangleInstance.Y = 8f;
            }
            else
            {
                AxisAlignedRectangleInstance.RelativeY = 8f;
            }
            AxisAlignedRectangleInstance.Width = 10f;
            AxisAlignedRectangleInstance.Height = 16f;
            TeamIndex = 0;
            MaxHealth = 100m;
            GroundMovement = Entities.Player.PlatformerValuesStatic["Ground"];
            AirMovement = Entities.Player.PlatformerValuesStatic["Air"];
        }
        public virtual void ConvertToManuallyUpdated () 
        {
            this.ForceUpdateDependenciesDeep();
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(SpriteInstance);
        }
        public static void LoadStaticContent (string contentManagerName) 
        {
            if (LoadedContentManagers.Contains(contentManagerName))
            {
                return;
            }
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            if (string.IsNullOrEmpty(contentManagerName))
            {
                throw new System.ArgumentException("contentManagerName cannot be empty or null");
            }
            ContentManagerName = contentManagerName;
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
                throw new System.Exception( "Player has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
            }
            #endif
            bool registerUnload = false;
            if (LoadedContentManagers.Contains(contentManagerName) == false)
            {
                LoadedContentManagers.Add(contentManagerName);
                lock (mLockObject)
                {
                    if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
                    {
                        FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("PlayerStaticUnload", UnloadStaticContent);
                        mRegisteredUnloads.Add(ContentManagerName);
                    }
                }
                if (!FlatRedBall.FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/player/platformeranimations.achx", ContentManagerName))
                {
                    registerUnload = true;
                }
                PlatformerAnimations = FlatRedBall.FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/player/platformeranimations.achx", ContentManagerName);
                if (PlatformerValuesStatic == null)
                {
                    {
                        // We put the { and } to limit the scope of oldDelimiter
                        char oldDelimiter = FlatRedBall.IO.Csv.CsvFileManager.Delimiter;
                        FlatRedBall.IO.Csv.CsvFileManager.Delimiter = ',';
                        System.Collections.Generic.Dictionary<System.String, Platformer.DataTypes.PlatformerValues> temporaryCsvObject = new System.Collections.Generic.Dictionary<System.String, Platformer.DataTypes.PlatformerValues>();
                        FlatRedBall.IO.Csv.CsvFileManager.CsvDeserializeDictionary<System.String, Platformer.DataTypes.PlatformerValues>("content/entities/player/platformervaluesstatic.csv", temporaryCsvObject, FlatRedBall.IO.Csv.DuplicateDictionaryEntryBehavior.Replace);
                        FlatRedBall.IO.Csv.CsvFileManager.Delimiter = oldDelimiter;
                        PlatformerValuesStatic = temporaryCsvObject;
                    }
                }
            }
            if (registerUnload && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
            {
                lock (mLockObject)
                {
                    if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
                    {
                        FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("PlayerStaticUnload", UnloadStaticContent);
                        mRegisteredUnloads.Add(ContentManagerName);
                    }
                }
            }
            CustomLoadStaticContent(contentManagerName);
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public static void UnloadStaticContent () 
        {
            if (LoadedContentManagers.Count != 0)
            {
                LoadedContentManagers.RemoveAt(0);
                mRegisteredUnloads.RemoveAt(0);
            }
            if (LoadedContentManagers.Count == 0)
            {
                if (PlatformerAnimations != null)
                {
                    PlatformerAnimations= null;
                }
                if (PlatformerValuesStatic != null)
                {
                    PlatformerValuesStatic= null;
                }
            }
        }
        [System.Obsolete("Use GetFile instead")]
        public static object GetStaticMember (string memberName) 
        {
            switch(memberName)
            {
                case  "PlatformerAnimations":
                    return PlatformerAnimations;
                case  "PlatformerValuesStatic":
                    return PlatformerValuesStatic;
            }
            return null;
        }
        public static object GetFile (string memberName) 
        {
            switch(memberName)
            {
                case  "PlatformerAnimations":
                    return PlatformerAnimations;
                case  "PlatformerValuesStatic":
                    return PlatformerValuesStatic;
            }
            return null;
        }
        object GetMember (string memberName) 
        {
            switch(memberName)
            {
                case  "PlatformerAnimations":
                    return PlatformerAnimations;
            }
            return null;
        }
        public static void Reload (object whatToReload) 
        {
            if (whatToReload == PlatformerAnimations)
            {
                {
                    var cm = FlatRedBall.FlatRedBallServices.GetContentManagerByName("Global");
                    cm.UnloadAsset(PlatformerAnimations);
                    PlatformerAnimations = FlatRedBall.FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>("content/entities/player/platformeranimations.achx");
                }
            }
            if (whatToReload == PlatformerValuesStatic)
            {
                FlatRedBall.IO.Csv.CsvFileManager.UpdateDictionaryValuesFromCsv(PlatformerValuesStatic, "content/entities/player/platformervaluesstatic.csv");
            }
        }
        protected bool mIsPaused;
        public override void Pause (FlatRedBall.Instructions.InstructionList instructions) 
        {
            base.Pause(instructions);
            mIsPaused = true;
        }
        public virtual void SetToIgnorePausing () 
        {
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(this);
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(SpriteInstance);
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(AxisAlignedRectangleInstance);
        }
        

        private void UpdateCurrentMovement()
        {
            if(IsPlatformingEnabled == false)
            {
                return;
            }

            if(mCurrentMovement ?.MaxSpeedX > 0)
            {
                lastNonZeroPlatformerHorizontalMaxSpeed = mCurrentMovement.MaxSpeedX;
            }

            switch (mMovementType)
            {
                case MovementType.Ground:
                    mCurrentMovement = GroundMovement;
                    break;
                case MovementType.Air:
                    mCurrentMovement = AirMovement;
                    break;
                case MovementType.AfterDoubleJump:

                    // The user could have double-jumped into a set of movement values that no longer support double jump.
                    // For example, double jump is supported in water, but once the user moves out of water, AfterDoubleJump
                    // might be set to null
                    if(AfterDoubleJump == null)
                    {
                        mCurrentMovement = AirMovement;
                    }
                    else
                    {
                        mCurrentMovement = AfterDoubleJump;
                    }

                    break;
            }

            if(CurrentMovement != null)
            {
                if(CurrentMovement.CanClimb)
                {
                    this.YAcceleration = 0;
                }
                else
                {
                    this.YAcceleration = -CurrentMovement.Gravity;
                }

                if(!CurrentMovement.UsesAcceleration)
                {
                    this.XAcceleration = 0;
                }
            }
        }


        #region Platformer Methods


        partial void CustomInitializePlatformerInput();


        public void InitializePlatformerInput(FlatRedBall.Input.IInputDevice inputDevice)
        {
            this.JumpInput = inputDevice.DefaultPrimaryActionInput;
            this.HorizontalInput = inputDevice.DefaultHorizontalInput;
            this.VerticalInput = inputDevice.DefaultVerticalInput;
            this.InputDevice = inputDevice;

            InputEnabled = true;
            CustomInitializePlatformerInput();
        }


        /// <summary>
        /// Reads all input and applies the read-in values to control
        /// velocity and character state.
        /// </summary>
        private void ApplyInput()
        {
#if DEBUG
            if(InputDevice == null)
            {
                throw new NullReferenceException("The InputDevice must be set before activity is performed on this entity. This can be set in Glue or manually in code");
            }
#endif

            ApplyHorizontalInput();

            ApplyClimbingInput();

            ApplyJumpInput();
        }

        /// <summary>
        /// Applies the horizontal input to control horizontal movement and state.
        /// </summary>
        private void ApplyHorizontalInput()
        {
            float horizontalRatio = HorizontalRatio;

#if DEBUG
            // Vic asks - TopDown doesn't crash here. Should platformer?
            if(CurrentMovement == null)
            {
                throw new InvalidOperationException("You must set CurrentMovement variable (can be done in Glue)");
            }

#endif
            if(horizontalRatio > 0)
            {
                mDirectionFacing = HorizontalDirection.Right;
            }
            else if(horizontalRatio < 0)
            {
                mDirectionFacing = HorizontalDirection.Left;
            }

            var maxSpeed = CurrentMovement.MaxSpeedX;

            var walkingUphill = (currentSlope > 0 && currentSlope < 90);

            if (CurrentMovement.UphillStopSpeedSlope != CurrentMovement.UphillFullSpeedSlope &&
                currentSlope >= (float)CurrentMovement.UphillFullSpeedSlope &&
                // make sure actually walking uphill:
                walkingUphill)
            {
                if ( currentSlope >= (float)CurrentMovement.UphillStopSpeedSlope)
                {
                    maxSpeed *= 0;
                }
                else
                {
                    var interpolationValue =
                        1 - (currentSlope - (float)CurrentMovement.UphillFullSpeedSlope) /
                        (float)(CurrentMovement.UphillStopSpeedSlope - CurrentMovement.UphillFullSpeedSlope);

                    maxSpeed *= interpolationValue;
                }
            }

            if ((this.CurrentMovement.AccelerationTimeX <= 0 && this.CurrentMovement.IsUsingCustomDeceleration == false)|| this.CurrentMovement.UsesAcceleration == false)
            {
                this.XVelocity = groundHorizontalVelocity + horizontalRatio * maxSpeed;
            }
            else
            {
                var desiredSpeed = groundHorizontalVelocity + horizontalRatio * maxSpeed;

                const float epsilon = .001f;

                var isSpeedingUp = 
                    (Math.Abs(XVelocity - groundHorizontalVelocity) < epsilon && Math.Abs(desiredSpeed - groundHorizontalVelocity) > epsilon) ||
                    ((desiredSpeed > 0 && XVelocity < desiredSpeed && XVelocity > 0) ||
                    (desiredSpeed < 0 && XVelocity > desiredSpeed && XVelocity < 0));
                
                var absoluteValueVelocityDifference = System.Math.Abs(desiredSpeed - XVelocity);
                
                float accelerationMagnitude = 0;
                
                if(isSpeedingUp && CurrentMovement.AccelerationTimeX != 0)
                {
                    accelerationMagnitude = maxSpeed / CurrentMovement.AccelerationTimeX;
                }
                else
                {
                     
                    if(System.Math.Abs(XVelocity) > this.CurrentMovement.MaxSpeedX && this.CurrentMovement.IsUsingCustomDeceleration)
                    {
                        accelerationMagnitude = this.CurrentMovement.CustomDecelerationValue;
                    }
                    // if slowing down and max speed is 0, use the last max speed
                    else if(maxSpeed == 0)
                    {
                        accelerationMagnitude = lastNonZeroPlatformerHorizontalMaxSpeed / CurrentMovement.DecelerationTimeX;
                    }
                    else
                    {
                        accelerationMagnitude = maxSpeed / CurrentMovement.DecelerationTimeX;
                    }
                }
                
                var perFrameVelocityChange = accelerationMagnitude * TimeManager.SecondDifference;
                
                if(perFrameVelocityChange > absoluteValueVelocityDifference)
                {
                    // make sure we don't overshoot:
                    accelerationMagnitude = absoluteValueVelocityDifference * (1 / TimeManager.SecondDifference);
                }
                
                this.XAcceleration = accelerationMagnitude * System.Math.Sign(desiredSpeed - XVelocity);
            }
            groundHorizontalVelocity = 0;
        }

        private void ApplyClimbingInput()
        {
            if(CurrentMovement.CanClimb)
            {
                var verticalInputValue = VerticalInput?.Value ?? 0;
                this.YVelocity = verticalInputValue * CurrentMovement.MaxClimbingSpeed;

                if(this.Y > TopOfLadderY)
                {
                    this.Y = TopOfLadderY.Value;
                    if(this.YVelocity > 0)
                    {
                        this.YVelocity = 0;
                    }
                }

            }

        }


        /// <summary>
        /// Applies the jump input to control vertical velocity and state.
        /// </summary>
        private void ApplyJumpInput()
        {
			bool jumpPushed = JumpInput.WasJustPressed && InputEnabled;
			bool jumpDown = JumpInput.IsDown && InputEnabled;

            if(jumpPushed && mIsOnGround && VerticalInput?.Value < -.5 && CurrentMovement.CanFallThroughCloudPlatforms && CurrentMovement.CloudFallThroughDistance > 0)
            {
                // try falling through the ground
                cloudCollisionFallThroughY = this.Y - CurrentMovement.CloudFallThroughDistance;
            }
            // Test for jumping up
            else if (jumpPushed && // Did the player push the jump button
                CurrentMovement.JumpVelocity > 0 &&
                (
                    mIsOnGround || 
                    CurrentMovement.CanClimb ||
                    AfterDoubleJump == null || 
				    (AfterDoubleJump != null && mHasDoubleJumped == false) ||
				    (AfterDoubleJump != null && AfterDoubleJump.JumpVelocity > 0)

				)
                
            )
            {
                cloudCollisionFallThroughY = null;

                mTimeJumpPushed = CurrentTime;
                this.YVelocity = CurrentMovement.JumpVelocity;
                ValuesJumpedWith = CurrentMovement;
                mIsOnGround = false;

                mCanContinueToApplyJumpToHold = true;

                if (JumpAction != null)
                {
                    JumpAction();
                }

                if (CurrentMovementType == MovementType.Air)
                {
                    // November 26, 2021
                    // We used to check if AfterDoubleJump 
                    // is null, and if so, throw an exception.
                    // In the past if the player wanted to jump
                    // in the air, a double jump was needed. But
                    // Now we want to support wall jumping, which means
                    // that the user can jump off walls and not have a double
                    // jump. Removing this error.
                    //if(AfterDoubleJump == null)
                    //{
                    //    throw new InvalidOperationException("The player is attempting to perform a double-jump, " +
                    //        "but the AfterDoubleJump variable is not set. If you are using glue, select this entity and change the After Double Jump variable.");
                    //}
                    mHasDoubleJumped = true ;
                }
                if(CurrentMovementType == MovementType.Ground && CurrentMovement.CanClimb)
                {
                    // the user jumped off a vine. Force the user into air mode:
                    CurrentMovementType = MovementType.Air;
                }
            }


            double secondsSincePush = CurrentTime - mTimeJumpPushed;

            // This needs to be done before checking if the user can continue to apply jump to hold
            if (ValuesJumpedWith != null && ValuesJumpedWith.JumpApplyByButtonHold &&
				(!JumpInput.IsDown || mHitHead)
                )
            {
                mCanContinueToApplyJumpToHold = false;
            }

            if (ValuesJumpedWith != null && 
                mCanContinueToApplyJumpToHold &&
                secondsSincePush < ValuesJumpedWith.JumpApplyLength &&
				(ValuesJumpedWith.JumpApplyByButtonHold == true && JumpInput.IsDown)
                )
            {
                this.YVelocity = ValuesJumpedWith.JumpVelocity;
            }
            else
            {
                mCanContinueToApplyJumpToHold = false;
            }

            this.YVelocity = System.Math.Max(-CurrentMovement.MaxFallSpeed, this.YVelocity);
        }


        /// <summary>
        /// Assigns the current movement values based off of whether the user is on ground and has double-jumped or not.
        /// This is called automatically, but it can be overridden in derived classes to perform custom assignment of 
        /// movement types.
        /// </summary>
        protected virtual void DetermineMovementValues()
        {
            if (mIsOnGround)
            {
                mHasDoubleJumped = false;
                if (CurrentMovementType == MovementType.Air ||
                    CurrentMovementType == MovementType.AfterDoubleJump)
                {
                    CurrentMovementType = MovementType.Ground;
                }
            }
            else
            {
                if (CurrentMovementType == MovementType.Ground && !CurrentMovement.CanClimb)
                {
                    CurrentMovementType = MovementType.Air;
                }

            }

            if (CurrentMovementType == MovementType.Air && mHasDoubleJumped)
            {
                CurrentMovementType = MovementType.AfterDoubleJump;
            }
            // User may have set HasDoubleJumped to false
            else if (CurrentMovementType == MovementType.AfterDoubleJump && !mHasDoubleJumped)
            {
                CurrentMovementType = MovementType.Air;
            }

        }

        #endregion

        

        /// <summary>
        /// Performs a standard solid collision against an ICollidable.
        /// </summary>
        public bool CollideAgainst(FlatRedBall.Math.Geometry.ICollidable collidable, bool isCloudCollision = false)
        {
            return CollideAgainst(collidable.Collision, isCloudCollision);
        }

        /// <summary>
        /// Performs a standard solid collision against a ShapeCollection.
        /// </summary>
        /// <param name="shapeCollection"></param>
        public bool CollideAgainst(FlatRedBall.Math.Geometry.ShapeCollection shapeCollection)
        {
            return CollideAgainst(shapeCollection, false);
        }

        public bool CollideAgainst(FlatRedBall.Math.Geometry.AxisAlignedRectangle rectangle, bool isCloudCollision = false)
        {
            return CollideAgainst(() =>
            {
                var collided = rectangle.CollideAgainstBounce(this.Collision, 1, 0, 0);
                return (collided, rectangle);
            }, isCloudCollision);
        }

        /// <summary>
        /// Performs a solid or cloud collision against a ShapeCollection.
        /// </summary>
        /// <param name="shapeCollection">The ShapeCollection to collide against.</param>
        /// <param name="isCloudCollision">Whether to perform solid or cloud collisions.</param>
        public bool CollideAgainst(FlatRedBall.Math.Geometry.ShapeCollection shapeCollection, bool isCloudCollision)
        {
            return CollideAgainst(() =>
            {
                var collided = shapeCollection.CollideAgainstBounce(this.Collision, 1, 0, 0);
                PositionedObject lastCollided = null;
                if (shapeCollection.LastCollisionAxisAlignedRectangles.Count > 0) lastCollided = shapeCollection.LastCollisionAxisAlignedRectangles[0];
                if (shapeCollection.LastCollisionCircles.Count > 0) lastCollided = shapeCollection.LastCollisionCircles[0];
                if (shapeCollection.LastCollisionPolygons.Count > 0) lastCollided = shapeCollection.LastCollisionPolygons[0];
                // do we care about other shapes?
                return (collided, lastCollided);
            }, isCloudCollision);
        }

        /// <summary>
        /// Executes the collisionFunction to determine if a collision occurred, and if so, reacts
        /// to the collision by modifying the state of the object and raising appropriate events.
        /// This is useful for situations where custom collisions are needed, but then the standard
        /// behavior is desired if a collision occurs.
        /// </summary>
        /// <param name="collisionFunction">The collision function to execute.</param>
        /// <param name="isCloudCollision">Whether to perform cloud collision (only check when moving down)</param>
        public bool CollideAgainst(System.Func<(bool, PositionedObject)> collisionFunction, bool isCloudCollision, string objectName = null)
        {
            Microsoft.Xna.Framework.Vector3 positionBeforeCollision = this.Position;
            Microsoft.Xna.Framework.Vector3 velocityBeforeCollision = this.Velocity;

            float lastY = this.Y;

            bool isFirstCollisionOfTheFrame = FlatRedBall.TimeManager.CurrentTime != mLastCollisionTime;

            if (isFirstCollisionOfTheFrame)
            {
                // This was set here, but if it is, custom collision (called on non-active collision relationships) won't update 
                // this value and have it be set. Instead, we reset this after applying it
                //groundHorizontalVelocity = 0;
                WasOnGroundLastFrame = mIsOnGround;
                mLastCollisionTime = FlatRedBall.TimeManager.CurrentTime;
                PositionBeforeLastPlatformerCollision = this.Position;
                mIsOnGround = false;
                mHitHead = false;
            }

            if(cloudCollisionFallThroughY != null && this.Y < cloudCollisionFallThroughY)
            {
                cloudCollisionFallThroughY = null;
            }

            bool canCheckCollision = true;

            if(isCloudCollision)
            {
                // need to be moving down...
                // Update September 10, 2021
                // But what if we're standing 
                // on a platform that is moving
                // upward? This moving platform may
                // be a cloud collision, and we should
                // still perform collision. Therefore, we 
                // should probably do cloud collision if...
                // * We are on the ground OR moving downward 
                //    --and--
                // * Not ignoring fallthrough
                //canCheckCollision = velocityBeforeCollision.Y < 0 &&
                canCheckCollision = (velocityBeforeCollision.Y < 0 || WasOnGroundLastFrame) &&
                    // and not ignoring fallthrough
                    cloudCollisionFallThroughY == null;

                if(canCheckCollision)
                {
                    if(WasOnGroundLastFrame == false &&  VerticalInput?.Value < -.5 && CurrentMovement.CanFallThroughCloudPlatforms)
                    {
                        // User is in the air, holding 'down', and the current movement allows the user to fall through clouds
                        canCheckCollision = false;
                    }
                }

                if(ForceIgnoreCloudCollision)
                {
                    canCheckCollision = false;
                }
            }

            bool toReturn = false;

            if (canCheckCollision)
            {

                (bool didCollide, PositionedObject objectCollidedAgainst) = collisionFunction();
                if (didCollide)
                {
                    toReturn = true;

                    // make sure that we've been moved up, and that we're falling
                    bool shouldApplyCollision = true;
                    if (isCloudCollision)
                    {
                        var yReposition = this.Y - positionBeforeCollision.Y;
                        if (yReposition <= 0)
                        {
                            shouldApplyCollision = false;
                        }
                        else
                        {
                            // rewind 1 frame and see if the gaps are bigger than the reposition
                            var thisYDistanceMovedThisFrame = (TimeManager.SecondDifference * velocityBeforeCollision.Y);
                            var otherYDistanceMovedThisFrame = 0f;
                            if (objectCollidedAgainst != null)
                            {
                                otherYDistanceMovedThisFrame = (TimeManager.SecondDifference * objectCollidedAgainst.TopParent.YVelocity);
                            }

                            shouldApplyCollision = yReposition < (-thisYDistanceMovedThisFrame + otherYDistanceMovedThisFrame);
                        }
                    }

                    if (shouldApplyCollision)
                    {

                        if (this.Y > lastY)
                        {
                            if (!WasOnGroundLastFrame && LandedAction != null)
                            {
                                LandedAction();
                            }
                            mIsOnGround = true;
        
                    if(!string.IsNullOrEmpty(objectCollidedAgainst?.Name ?? objectName))
                    {
                        GroundCollidedAgainst.Add(objectCollidedAgainst?.Name ?? objectName);
                    }

        

                            groundHorizontalVelocity = objectCollidedAgainst?.TopParent.XVelocity ?? 0;
                        }
                        if (this.Y < lastY)
                        {
                            mHitHead = true;
                        }
                        if(isCloudCollision)
                        {
                            this.X = positionBeforeCollision.X;
                            this.Velocity.X = velocityBeforeCollision.X;
                        }
                    }
                    else
                    {
                        Position = positionBeforeCollision;
                        Velocity = velocityBeforeCollision;
                    }
                }
            }


            // If a platformer object has multiple parts, one collision may move one set of shapes, but not the other
            // shapes. Need to force that update
            if(toReturn)
            {
                this.ForceUpdateDependenciesDeep();
            }

            return toReturn;
        }

        


        (bool, PositionedObject) DoCollisionWithShapeCollectionConsideringCornerLanding(FlatRedBall.TileCollisions.TileShapeCollection shapeCollection, bool isCloudCollision)
        {
            var positionBefore = this.Position;
            var velocityBefore = this.Velocity;

            var didCollideInternal = shapeCollection.CollideAgainstSolid(this);
            if(isCloudCollision == false && didCollideInternal && velocityBefore.Y < 0)
            {
                var repositionDirection = this.Position - positionBefore;
                if(repositionDirection.X != 0)
                {
                    var positionAfter = this.Position;
                    var velocityAfter = this.Velocity;

                    // undo the last frame to figure out where the Y was for the player last frame:

                    var didRedoWithUpward = false;
                    var didTryCollision = true;

                    var didAnyRedoWithoutUpward = false;

                    for(int i = 0; i < shapeCollection.LastCollisionAxisAlignedRectangles.Count; i++)
                    {
                        var rectangle = shapeCollection.LastCollisionAxisAlignedRectangles[i];

                        var canRepositionHorizontally =
                            (rectangle.RepositionDirections & FlatRedBall.Math.Geometry.RepositionDirections.Left) == FlatRedBall.Math.Geometry.RepositionDirections.Left ||
                            (rectangle.RepositionDirections & FlatRedBall.Math.Geometry.RepositionDirections.Right) == FlatRedBall.Math.Geometry.RepositionDirections.Right;

                        if ((rectangle.RepositionDirections & FlatRedBall.Math.Geometry.RepositionDirections.Up) == FlatRedBall.Math.Geometry.RepositionDirections.Up &&
                            canRepositionHorizontally)
                        {
                            // Player was repositioned horizontally by a shape which can also reposition upward.
                            // Did they get pushed off a ledge?

                            // to test this, move the player back to original position, force an upward collision, and see if the resulting upward
                            // collision is <= the position before.
                            this.Position = positionBefore;
                            this.Velocity = velocityBefore;
                            this.ForceUpdateDependenciesDeep();

                            var oldReposition = rectangle.RepositionDirections;

                            rectangle.RepositionDirections = FlatRedBall.Math.Geometry.RepositionDirections.Up;

                            this.Collision.CollideAgainstBounce(rectangle, 0, 1, 0);
                            didTryCollision = true;
                            rectangle.RepositionDirections = oldReposition;

                            if(this.Y <= LastPosition.Y)
                            {
                                didRedoWithUpward = true;
                                //break;
                            }
                            else
                            {
                                didAnyRedoWithoutUpward = true;
                            }

                        }
                        else if(rectangle.RepositionDirections != FlatRedBall.Math.Geometry.RepositionDirections.None)
                        {
                            didAnyRedoWithoutUpward = true;
                            break;
                        }
                    }


                    if(didAnyRedoWithoutUpward)
                    {
                        this.Position = positionAfter;
                        this.Velocity = velocityAfter;
                    }

                    else if(!didRedoWithUpward && didTryCollision)
                    {
                        this.Position = positionAfter;
                        this.Velocity = velocityAfter;
                        this.ForceUpdateDependenciesDeep();
                    }
                }
            }
            return (didCollideInternal, null);
        }

        public bool CollideAgainst(FlatRedBall.TileCollisions.TileShapeCollection shapeCollection, bool isCloudCollision = false)
        {
            var positionBefore = this.Position;
            var velocityBefore = this.Velocity;


            var collided = CollideAgainst(() => DoCollisionWithShapeCollectionConsideringCornerLanding(shapeCollection, isCloudCollision), isCloudCollision, shapeCollection.Name);


            if(collided)
            {
                currentSlope = 0;
            }

            var wasMovedHorizontally = this.X != positionBefore.X;

            var wasSlowedByPolygons = wasMovedHorizontally && shapeCollection.LastCollisionPolygons.Count != 0;

            if(wasSlowedByPolygons)
            {
                var repositionVector = new Microsoft.Xna.Framework.Vector2(0, 1);
                foreach(var rect in this.Collision.AxisAlignedRectangles)
                {
                    if(rect.LastMoveCollisionReposition.X != 0)
                    {
                        repositionVector = rect.LastMoveCollisionReposition;
                        break;
                    }
                }
                var shouldPreserve = DetermineIfHorizontalVelocityShouldBePreserved(velocityBefore.X, shapeCollection, repositionVector);

                if(shouldPreserve)
                {
                    // This was an attempt to fix snagging...
                    // The problem is that when a rectangle collides
                    // against a polygon, the point on the polygon may
                    // be the one that defines the reposition direction.
                    // This means a rectangle could be sitting on a slope
                    // but still have a perfectly vertical reposition. I tried
                    // to fix this by only getting reposition vectors from the polygon
                    // but that caused the platformer to hop in place in some situations.

                    //float maxYMap = float.NegativeInfinity;
                    //for (int i = 0; i < shapeCollection.LastCollisionPolygons.Count; i++)
                    //{
                    //    var polygon = shapeCollection.LastCollisionPolygons[i];
                    //    for (int j = 0; j < polygon.Points.Count; j++)
                    //    {
                    //        maxYMap = Math.Max(maxYMap, polygon.AbsolutePointPosition(j).Y);
                    //    }
                    //}
                    //for(int i = 0; i < shapeCollection.LastCollisionAxisAlignedRectangles.Count; i++)
                    //{
                    //    var rectangle = shapeCollection.LastCollisionAxisAlignedRectangles[i];
                    //    maxYMap = Math.Max(maxYMap, rectangle.Y + rectangle.ScaleY);
                    //}


                    //float maxCollisionOffset = 0;
                    //foreach(var rectangle in this.Collision.AxisAlignedRectangles)
                    //{
                    //    maxCollisionOffset = -rectangle.RelativeY + rectangle.ScaleY;
                    //}

                    //float maxYAfterReposition = maxCollisionOffset + maxYMap;

                    // keep the velocity and the position:
                    var xDifference = positionBefore.X - this.Position.X;

                    var tangent = new Microsoft.Xna.Framework.Vector2(repositionVector.Y, -repositionVector.X);

                    currentSlope = Microsoft.Xna.Framework.MathHelper.ToDegrees( (float) System.Math.Atan2(tangent.Y, tangent.X));

                    if(DirectionFacing == HorizontalDirection.Left)
                    {
                        currentSlope *= -1;
                    }

                    var multiplier = xDifference / tangent.X;

                    this.Velocity.X = velocityBefore.X;
                    this.Position.X = positionBefore.X;
                    this.Position.Y += multiplier * tangent.Y;
                    //this.Position.Y = Math.Min(this.Position.Y, maxYAfterReposition);
                    this.ForceUpdateDependenciesDeep();
                }
            }
            return collided;
        }

        
        private bool DetermineIfHorizontalVelocityShouldBePreserved(float oldHorizontalVelocity, FlatRedBall.TileCollisions.TileShapeCollection shapeCollection, 
            Microsoft.Xna.Framework.Vector2 repositionVector)
        {
            const float maxSlope = 80; // degrees
            var maxSlopeInRadians = Microsoft.Xna.Framework.MathHelper.ToRadians(maxSlope);
            // The reposition is the normal of the slope, so it's the X
            // That is, on a slope like this:
            // \
            //  \
            //   \
            //    \
            //     \
            // If the slope ^^ is nearly 90, then the X will be nearly 1. To get that, we will do the sin of the slope

            var maxRepositionDirectionX = System.Math.Sin(maxSlopeInRadians);

            bool collidedWithSlopeGreaterThanMax = repositionVector.Y <= 0;

            if(collidedWithSlopeGreaterThanMax == false)
            {
                if(repositionVector.X != 0 || repositionVector.Y != 0)
                {
                    var normalized = Microsoft.Xna.Framework.Vector2.Normalize(repositionVector);

                    if(normalized.X > maxRepositionDirectionX || normalized.X < -maxRepositionDirectionX)
                    {
                        collidedWithSlopeGreaterThanMax = true;
                    }

                }
            }
            var shouldBePreserved = collidedWithSlopeGreaterThanMax == false;

            return shouldBePreserved;
        }


        public bool CollideAgainst(FlatRedBall.TileCollisions.TileShapeCollection shapeCollection, FlatRedBall.Math.Geometry.AxisAlignedRectangle thisCollision, bool isCloudCollision = false)
        {
            return CollideAgainst(() =>
            {
                var didCollide = shapeCollection.CollideAgainstSolid(thisCollision);
                return (didCollide, null);
            }, isCloudCollision, shapeCollection.Name);
        }


        
        /// <summary>
        /// Sets the HorizontalInput and JumpInput instances to either the keyboard or 
        /// Xbox360GamePad index 0. This can be overridden by base classes to default
        /// to different input devices.
        /// </summary>
        protected virtual void InitializeInput()
        {
        
                if (FlatRedBall.Input.InputManager.Xbox360GamePads[0].IsConnected)
                {
                    InitializePlatformerInput(FlatRedBall.Input.InputManager.Xbox360GamePads[0]);
                }
                else
                {
                    InitializePlatformerInput(FlatRedBall.Input.InputManager.Keyboard);
                }
    
        
        }

        public virtual void MoveToLayer (FlatRedBall.Graphics.Layer layerToMoveTo) 
        {
            var layerToRemoveFrom = LayerProvidedByContainer;
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(SpriteInstance);
            }
            if (layerToMoveTo != null || !SpriteManager.AutomaticallyUpdatedSprites.Contains(SpriteInstance))
            {
                FlatRedBall.SpriteManager.AddToLayer(SpriteInstance, layerToMoveTo);
            }
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(AxisAlignedRectangleInstance);
            }
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(AxisAlignedRectangleInstance, layerToMoveTo);
            LayerProvidedByContainer = layerToMoveTo;
        }
        partial void CustomActivityEditMode();
    }
}
