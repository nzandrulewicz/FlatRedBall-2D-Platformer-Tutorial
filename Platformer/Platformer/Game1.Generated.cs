using System.Linq;
namespace Platformer
{
    public partial class Game1
    {
        #if DEBUG
        GlueCommunication.GameConnectionManager gameConnectionManager;
        #endif
        #if DEBUG
        GlueControl.GlueControlManager glueControlManager;
        #endif
        RenderingLibrary.Graphics.IRenderable GetRenderable (string name, global::RenderingLibrary.ISystemManagers managers) 
        {
            var asBaseType = Gum.Wireframe.RuntimeObjectCreator.TryHandleAsBaseType(name, managers);
            return asBaseType;
        }
        partial void GeneratedInitializeEarly () 
        {
            FlatRedBall.FlatRedBallServices.AddManager(FlatRedBall.Forms.Managers.FrameworkElementManager.Self);
        }
        partial void GeneratedInitialize () 
        {
            global::GumRuntime.ElementSaveExtensions.CustomCreateGraphicalComponentFunc = GetRenderable;
            global::Gum.Wireframe.GraphicalUiElement.SetPropertyOnRenderable = global::Gum.Wireframe.CustomSetPropertyOnRenderable.SetPropertyOnRenderable;
            global::Gum.Wireframe.GraphicalUiElement.UpdateFontFromProperties = global::Gum.Wireframe.CustomSetPropertyOnRenderable.UpdateToFontValues;
            global::Gum.Wireframe.GraphicalUiElement.ThrowExceptionsForMissingFiles = global::Gum.Wireframe.CustomSetPropertyOnRenderable.ThrowExceptionsForMissingFiles;
            global::Gum.Wireframe.GraphicalUiElement.AddRenderableToManagers = global::Gum.Wireframe.CustomSetPropertyOnRenderable.AddRenderableToManagers;
            global::Gum.Wireframe.GraphicalUiElement.RemoveRenderableFromManagers = global::Gum.Wireframe.CustomSetPropertyOnRenderable.RemoveRenderableFromManagers;
            Platformer.GlobalContent.Initialize();
            System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += (s, e) =>
            {
                // Get just the name of assmebly
                // Aseembly name excluding version and other metadata
                string name = e.Name.Contains(", ") ? e.Name.Substring(0, e.Name.IndexOf(", ")) : e.Name;
            
                if (name == "Newtonsoft.Json")
                {
                    // Load whatever version available
                    return System.Reflection.Assembly.Load(name);
                }
            
                return null;
            };
            
            #if DEBUG
            gameConnectionManager = new GlueCommunication.GameConnectionManager(8978);
            gameConnectionManager.OnPacketReceived += async (packet) =>
            {
                if (packet.Packet.PacketType == "OldDTO" && glueControlManager != null)
                {
                    var returnValue = await glueControlManager?.ProcessMessage(packet.Packet.Payload);
            
                    gameConnectionManager.SendItem(new GlueCommunication.GameConnectionManager.Packet
                    {
                        PacketType = "OldDTO",
                        Payload = returnValue,
                        InResponseTo = packet.Packet.Id
                    });
                }
            };
            this.Exiting += (not, used) => gameConnectionManager.Dispose();
            #endif
            var args = System.Environment.GetCommandLineArgs();
            bool? changeResize = null;
            var resizeArgs = args.FirstOrDefault(item => item.StartsWith("AllowWindowResizing="));
            if (!string.IsNullOrEmpty(resizeArgs))
            {
                var afterEqual = resizeArgs.Split('=')[1];
                changeResize = bool.Parse(afterEqual);
            }
            if (changeResize != null)
            {
                CameraSetup.Data.AllowWindowResizing = changeResize.Value;
            }
            CameraSetup.SetupCamera(FlatRedBall.Camera.Main, graphics);
            #if DEBUG
            glueControlManager = new GlueControl.GlueControlManager(8978)
            {
                GameConnectionManager = gameConnectionManager,
            };
            FlatRedBall.FlatRedBallServices.GraphicsOptions.SizeOrOrientationChanged += (not, used) =>
            {
                if (FlatRedBall.Screens.ScreenManager.IsInEditMode)
                {
                    GlueControl.Editing.CameraLogic.UpdateCameraToZoomLevel(zoomAroundCursorPosition: false);
                }
                GlueControl.Editing.CameraLogic.PushZoomLevelToEditor();
            }
            ;
            FlatRedBall.Screens.ScreenManager.BeforeScreenCustomInitialize += (newScreen) => 
            {
                // for info on why we have this if-check, see this issue: https://github.com/vchelaru/FlatRedBall/issues/1046
                if (newScreen.GetType().Name != "EntityViewingScreen")
                {
                    glueControlManager.ReRunAllGlueToGameCommands();
                }
                // These get nulled out when screens are destroyed so we have to re-assign them
                Factories.CoinFactory.EntitySpawned += (newEntity) =>  GlueControl.InstanceLogic.Self.ApplyEditorCommandsToNewEntity(newEntity);
                Factories.PlayerFactory.EntitySpawned += (newEntity) =>  GlueControl.InstanceLogic.Self.ApplyEditorCommandsToNewEntity(newEntity);
            }
            ;
            #endif
            FlatRedBall.Screens.ScreenManager.AfterScreenDestroyed += (screen) =>
            {
                GlueControl.Editing.EditorVisuals.DestroyContainedObjects();
            }
            ;
            System.Type startScreenType = typeof(Platformer.Screens.Level1);
            var commandLineArgs = System.Environment.GetCommandLineArgs();
            if (commandLineArgs.Length > 0)
            {
                var thisAssembly = this.GetType().Assembly;
                // see if any of these are screens:
                foreach (var item in commandLineArgs)
                {
                    var type = thisAssembly.GetType(item);
                    if (type != null)
                    {
                        startScreenType = type;
                        break;
                    }
                }
            }
            if (startScreenType != null)
            {
                FlatRedBall.Screens.ScreenManager.Start(startScreenType);
            }
            // This value is used for parallax. If the game doesn't change its resolution, this this code should solve parallax with zooming cameras.
            global::FlatRedBall.TileGraphics.MapDrawableBatch.NativeCameraWidth = Platformer.CameraSetup.Data.ResolutionWidth;
            global::FlatRedBall.TileGraphics.MapDrawableBatch.NativeCameraHeight = Platformer.CameraSetup.Data.ResolutionHeight;
        }
        partial void GeneratedUpdate (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
        partial void GeneratedDrawEarly (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
        partial void GeneratedDraw (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
    }
}
