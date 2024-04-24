﻿#define IncludeSetVariable
#define PreVersion
#define HasFormsObject
#define AddedGeneratedGame1
#define ListsHaveAssociateWithFactoryBool
#define GumGueHasGetAnimation
#define CsvInheritanceSupport
#define IPositionedSizedObjectInEngine
#define NugetPackageInCsproj
#define SupportsEditMode
#define SupportsShapeCollectionAddToManagerMakeAutomaticallyUpdated
#define ScreensHaveActivityEditMode
#define SupportsNamedSubcollisions
#define TimeManagerHasDelaySeconds
#define GumTextHasIsBold
#define GlueSavedToJson
#define IEntityInFrb
#define SeparateJsonFilesForElements
#define GumSupportsAchxAnimation
#define StartupInGeneratedGame
#define RemoveAutoLocalizationOfVariables
#define GumHasMIsLayoutSuspendedPublic
#define SpriteHasUseAnimationTextureFlip
#define RemoveIsScrollableEntityList
#define HasGetGridLine
#define HasScreenManagerAfterScreenDestroyed
#define ScreenManagerHasPersistentPolygons
#define ShapeManagerCollideAgainstClosest
#define SpriteHasTolerateMissingAnimations
#define AnimationLayerHasName
#define IPlatformer
#define GumDefaults2
#define IStackableInEngine
#define ICollidableHasItemsCollidedAgainst
#define CollisionRelationshipManualPhysics
#define GumSupportsStackSpacing
#define CollisionRelationshipsSupportMoveSoft
#define GeneratedCameraSetupFile
#define ShapeCollectionHasMaxAxisAlignedRectanglesRadiusX
#define AutoNameCollisionListsAsSingle
#define GumHasIgnoredByParentSize
#define GumTextObjectsUpdateTextWith0ChildDepth
#define HasFrameworkElementManager
#define HasGumSkiaElements
#define ITiledTileMetadataInFrb
#define DamageableHasHealth
#define HasGame1GenerateEarly
#define ICollidableHasObjectsCollidedAgainst
#define HasIRepeatPressableInput
#define AllTiledFilesGenerated
#define RemoveRedundantDerivedData
#define GraphicalUiElementProtectedAnimationProperties
#define GraphicalUiElementINotifyPropertyChanged
#define GumTextObjectsHaveTextOverflowProperties
#define TileShapeCollectionAddToLayerSupportsAutomaticallyUpdated
#define ISongInFrb
#define RendererHasExternalEffectManager
#define SpriteHasSetCollisionFromAnimation
#define HasIGumScreenOwner
#define ScreenIsINameable
#define SpriteManagerHasInsertLayer
#define GumUsesSystemTypes
#define GumCommonCodeReferencing
#define GumTextSupportsBbCode
#define DamageDealingToggles
#define HasGum
using Platformer;

using FlatRedBall;
using FlatRedBall.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlatRedBall.Graphics;
using FlatRedBall.Instructions.Reflection;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;
using FlatRedBall.Utilities;

using GlueControl.Dtos;
using GlueControl.Editing;
using GlueControl.Runtime;
using GlueControl.Models;
using GlueControl.Managers;
using Platformer.Performance;

namespace GlueControl
{
    public class InstanceLogic
    {
        #region Objects added at runtime 
        public List<ShapeCollection> ShapeCollectionsAddedAtRuntime = new List<ShapeCollection>();

        public ShapeCollection ShapesAddedAtRuntime = new ShapeCollection();

        public FlatRedBall.Math.PositionedObjectList<Sprite> SpritesAddedAtRuntime = new FlatRedBall.Math.PositionedObjectList<Sprite>();
        public FlatRedBall.Math.PositionedObjectList<Text> TextsAddedAtRuntime = new FlatRedBall.Math.PositionedObjectList<Text>();

        public List<IDestroyable> DestroyablesAddedAtRuntime = new List<IDestroyable>();

        /// <summary>
        /// Dictionary of state categories added at runtime, where the key is the ElementGameType name.
        /// </summary>
        public Dictionary<string, List<StateSaveCategory>> StatesAddedAtRuntime = new Dictionary<string, List<StateSaveCategory>>();

        public Dictionary<string, List<CustomVariable>> CustomVariablesAddedAtRuntime = new Dictionary<string, List<CustomVariable>>();


        public List<IList> ListsAddedAtRuntime = new List<IList>();

#if HasGum
        public List<Gum.Wireframe.GraphicalUiElement> GumObjectsAddedAtRuntime = new List<Gum.Wireframe.GraphicalUiElement>();
        public List<GumCoreShared.FlatRedBall.Embedded.PositionedObjectGueWrapper> GumWrappersAddedAtRuntime = new List<GumCoreShared.FlatRedBall.Embedded.PositionedObjectGueWrapper>();
#endif

        #endregion

        #region Fields/Properties

        static InstanceLogic self;
        public static InstanceLogic Self
        {
            get
            {
                if (self == null)
                {
                    self = new InstanceLogic();
                }
                return self;
            }
        }



        /// <summary>
        /// A dictionary of custom elements where the key is the full name of the type that
        /// would exist if the code were generated (such as "ProjectNamespace.Entities.MyEntity")
        /// </summary>
        public Dictionary<string, GlueElement> CustomGlueElements = new Dictionary<string, GlueElement>();


        // this is to prevent multiple objects from having the same name in the same frame:
        static long NewIndex = 0;

        #endregion

        #region Create Instance from Glue

        public OptionallyAttemptedGeneralResponse HandleCreateInstanceCommandFromGlue(Dtos.AddObjectDto dto, int currentAddObjectIndex, PositionedObject forcedParent = null)
        {
            var toReturn = new OptionallyAttemptedGeneralResponse();
            //var glueName = dto.ElementName;
            // this comes in as the game name not glue name
            var elementGameType = dto.ElementNameGame; // CommandReceiver.GlueToGameElementName(glueName);
            var ownerType = this.GetType().Assembly.GetType(elementGameType);
            GlueElement ownerElement = null;
            if (CustomGlueElements.ContainsKey(elementGameType))
            {
                ownerElement = CustomGlueElements[elementGameType];
            }
            object newRuntimeObject = null;

            var addedToEntity =
                (ownerType != null && typeof(PositionedObject).IsAssignableFrom(ownerType))
                ||
                ownerElement != null && ownerElement is EntitySave;

            if (addedToEntity)
            {
                toReturn.DidAttempt = true;
                if (forcedParent != null)
                {
                    if (CommandReceiver.DoTypesMatch(forcedParent, elementGameType))
                    {
                        newRuntimeObject = HandleCreateInstanceCommandFromGlueInner(dto.NamedObjectSave, currentAddObjectIndex, forcedParent);
                    }
                }
                else
                {
                    // need to loop through every object and see if it is an instance of the entity type, and if so, add this object to it
                    for (int i = 0; i < SpriteManager.ManagedPositionedObjects.Count; i++)
                    {
                        var item = SpriteManager.ManagedPositionedObjects[i];
                        if (CommandReceiver.DoTypesMatch(item, elementGameType))
                        {
                            newRuntimeObject = HandleCreateInstanceCommandFromGlueInner(dto.NamedObjectSave, currentAddObjectIndex, item);
                        }
                    }
                }
            }
            else if (forcedParent == null &&
                (ScreenManager.CurrentScreen.GetType().FullName == elementGameType || ownerType?.IsAssignableFrom(ScreenManager.CurrentScreen.GetType()) == true))
            {
                toReturn.DidAttempt = true;

                // it's added to the base screen, so just add it to null
                newRuntimeObject = HandleCreateInstanceCommandFromGlueInner(dto.NamedObjectSave, currentAddObjectIndex, null);
            }
            // did not attempt

            if (toReturn.DidAttempt)
            {
                toReturn.Succeeded = newRuntimeObject != null;
            }
            else
            {
                // assume it will succeed since we can't really know...
                toReturn.Succeeded = true;
            }
            return toReturn;
        }

        private object HandleCreateInstanceCommandFromGlueInner(Models.NamedObjectSave deserialized, int currentAddObjectIndex, PositionedObject owner)
        {
            // The owner is the
            // PositionedObject which
            // owns the newly-created instance
            // from the NamedObjectSave. Note that
            // if the owner is a DynamicEntity, it will
            // automatically remove any attached objects; 
            // however, if it is not, the objects still need
            // to be removed by the Glue control system, so we 
            // are going to add them to the ShapesAddedAtRuntime

            PositionedObject newPositionedObject = null;
            object newObject = null;

            if (deserialized.SourceType == GlueControl.Models.SourceType.Entity)
            {
                newPositionedObject = CreateEntity(deserialized, currentAddObjectIndex);
            }
            else if (deserialized.SourceType == GlueControl.Models.SourceType.FlatRedBallType &&
                deserialized.IsCollisionRelationship())
            {
                newObject = TryCreateCollisionRelationship(deserialized);
            }
            else if (deserialized.SourceType == GlueControl.Models.SourceType.FlatRedBallType)
            {
                switch (deserialized.SourceClassType)
                {
                    case "FlatRedBall.Math.Geometry.AxisAlignedRectangle":
                    case "AxisAlignedRectangle":
                        {
                            var aaRect = new FlatRedBall.Math.Geometry.AxisAlignedRectangle();
                            if (deserialized.AddToManagers)
                            {
                                ShapeManager.AddAxisAlignedRectangle(aaRect);
                                ShapesAddedAtRuntime.Add(aaRect);
                            }
                            if (owner is ICollidable asCollidable && deserialized.IncludeInICollidable)
                            {
                                asCollidable.Collision.Add(aaRect);
                            }
                            newPositionedObject = aaRect;
                        }

                        break;
                    case "FlatRedBall.Math.Geometry.Circle":
                    case "Circle":
                        {
                            var circle = new FlatRedBall.Math.Geometry.Circle();
                            if (deserialized.AddToManagers)
                            {
                                ShapeManager.AddCircle(circle);
                                ShapesAddedAtRuntime.Add(circle);
                            }
                            if (owner is ICollidable asCollidable && deserialized.IncludeInICollidable)
                            {
                                asCollidable.Collision.Add(circle);
                            }
                            newPositionedObject = circle;
                        }
                        break;
                    case "FlatRedBall.Graphics.Layer":
                    case "Layer":
                        {
                            var layer = new Layer();
                            if (deserialized.AddToManagers)
                            {
                                SpriteManager.AddLayer(layer);
                            }
                            newObject = layer;
                        }
                        break;
                    case "FlatRedBall.Math.Geometry.Line":
                    case "Line":
                        {
                            var line = new FlatRedBall.Math.Geometry.Line();
                            if (deserialized.AddToManagers)
                            {
                                ShapeManager.AddLine(line);
                                ShapesAddedAtRuntime.Lines.Add(line);
                            }
                            // eventually lines may be part of ICollidable:
                            //if (owner is ICollidable asCollidable && deserialized.IncludeInICollidable)
                            //{
                            //    asCollidable.Collision.Add(polygon);
                            //}
                            newPositionedObject = line;
                        }
                        break;
                    case "FlatRedBall.Math.Geometry.Polygon":
                    case "Polygon":
                        {
                            var polygon = new FlatRedBall.Math.Geometry.Polygon();
                            if (deserialized.AddToManagers)
                            {
                                ShapeManager.AddPolygon(polygon);
                                ShapesAddedAtRuntime.Add(polygon);
                            }
                            if (owner is ICollidable asCollidable && deserialized.IncludeInICollidable)
                            {
                                asCollidable.Collision.Add(polygon);
                            }
                            newPositionedObject = polygon;
                        }
                        break;
                    case "FlatRedBall.Sprite":
                    case "Sprite":
                        var sprite = new FlatRedBall.Sprite();
                        if (deserialized.AddToManagers)
                        {
                            SpriteManager.AddSprite(sprite);
                            SpritesAddedAtRuntime.Add(sprite);
                        }
                        newPositionedObject = sprite;

                        break;
                    case "Text":
                    case "FlatRedBall.Graphics.Text":
                        var text = new FlatRedBall.Graphics.Text();
                        text.Font = TextManager.DefaultFont;
                        text.SetPixelPerfectScale(Camera.Main);
                        if (deserialized.AddToManagers)
                        {
                            TextManager.AddText(text);
                            TextsAddedAtRuntime.Add(text);
                        }
                        newPositionedObject = text;
                        break;
                    case "FlatRedBall.Math.Geometry.ShapeCollection":
                    case "ShapeCollection":
                        var shapeCollection = new ShapeCollection();
                        ShapeCollectionsAddedAtRuntime.Add(shapeCollection);
                        newObject = shapeCollection;
                        break;
                    case "FlatRedBall.Math.PositionedObjectList<T>":
                        newObject = CreatePositionedObjectList(deserialized);
                        break;
                }

                if (newObject == null)
                {
                    newObject = TryCreateGumObject(deserialized, owner);
                }
            }
            if (newPositionedObject != null)
            {
                newObject = newPositionedObject;

                if (owner != null)
                {
                    newPositionedObject.AttachTo(owner);
                }
            }
            if (newObject != null)
            {
                AssignVariablesOnNewlyCreatedObject(deserialized, newObject);
            }

            return newObject;
        }

        private object TryCreateGumObject(NamedObjectSave deserialized, PositionedObject owner)
        {
#if HasGum
            var type = this.GetType().Assembly.GetType(deserialized.SourceClassType);
            var isGum = type != null && typeof(Gum.Wireframe.GraphicalUiElement).IsAssignableFrom(type);

            if (isGum)
            {
                var oldLayoutSuspended = global::Gum.Wireframe.GraphicalUiElement.IsAllLayoutSuspended;
                global::Gum.Wireframe.GraphicalUiElement.IsAllLayoutSuspended = true;
                var constructor = type.GetConstructor(new Type[] { typeof(bool), typeof(bool) });
                var newGumObjectInstance = 
                    constructor.Invoke(new object[] { true, true }) as Gum.Wireframe.GraphicalUiElement;

                global::Gum.Wireframe.GraphicalUiElement.IsAllLayoutSuspended = oldLayoutSuspended;
                newGumObjectInstance.UpdateFontRecursive();
                newGumObjectInstance.UpdateLayout();

                // eventually support layered, but not for now.....
                newGumObjectInstance.AddToManagers(RenderingLibrary.SystemManagers.Default, null);

                if (owner != null)
                {
                    var wrapperForAttachment = new GumCoreShared.FlatRedBall.Embedded.PositionedObjectGueWrapper(owner, newGumObjectInstance);
                    FlatRedBall.SpriteManager.AddPositionedObject(wrapperForAttachment);
                    wrapperForAttachment.Name = deserialized.InstanceName;
                    //gumAttachmentWrappers.Add(wrapperForAttachment);
                    GumWrappersAddedAtRuntime.Add(wrapperForAttachment);
                }
                GumObjectsAddedAtRuntime.Add(newGumObjectInstance);

                return newGumObjectInstance;
            }
#endif
            return null;
        }

        private Object CreatePositionedObjectList(Models.NamedObjectSave namedObject)
        {
            var sourceClassGenericType = namedObject.SourceClassGenericType;

            var gameTypeName =
                CommandReceiver.GlueToGameElementName(sourceClassGenericType);

            var type = this.GetType().Assembly.GetType(gameTypeName);

            object newList = null;

            if (type == null)
            {
                // see if it's contained in the list of dynamic entities

                var isDynamicEntity = CustomGlueElements.ContainsKey(gameTypeName);
                if (isDynamicEntity)
                {
                    var list = new PositionedObjectList<DynamicEntity>();
                    ListsAddedAtRuntime.Add(list);
                    newList = list;
                }
                else
                {
                    var list = new PositionedObjectList<PositionedObject>();
                    ListsAddedAtRuntime.Add(list);
                    newList = list;
                }
            }
            else
            {
                var poList = typeof(PositionedObjectList<>).MakeGenericType(type);
                var list = poList.GetConstructor(new Type[0]).Invoke(new object[0]) as IList;
                ListsAddedAtRuntime.Add(list);
                newList = list;
            }
            return newList;
        }

        private object TryCreateCollisionRelationship(Models.NamedObjectSave deserialized)
        {
            var type = Editing.VariableAssignmentLogic.GetDesiredRelationshipType(deserialized, out object firstObject, out object secondObject);
            if (type == null)
            {
                return null;
            }
            else
            {
                object toReturn = null;
                var constructor = type.GetConstructors().FirstOrDefault();
                if (constructor != null)
                {
                    List<object> parameters = new List<object>();
                    if (firstObject != null)
                    {
                        parameters.Add(firstObject);
                    }
                    if (secondObject != null)
                    {
                        parameters.Add(secondObject);
                    }
                    var collisionRelationship =
                        constructor.Invoke(parameters.ToArray()) as FlatRedBall.Math.Collision.CollisionRelationship;
                    collisionRelationship.Partitions = FlatRedBall.Math.Collision.CollisionManager.Self.Partitions;
                    toReturn = collisionRelationship;
                    FlatRedBall.Math.Collision.CollisionManager.Self.Relationships.Add(collisionRelationship);
                }
                return toReturn;
            }
        }


        public PositionedObject CreateEntity(Models.NamedObjectSave deserialized, int currentAddObjectIndex)
        {
            var entityNameGlue = deserialized.SourceClassType;
            var newEntity = CreateEntity(CommandReceiver.GlueToGameElementName(entityNameGlue), currentAddObjectIndex);

            return newEntity;
        }

        public void ApplyEditorCommandsToNewEntity(PositionedObject newEntity, int currentAddObjectIndex = -1)
        {
            currentAddObjectIndex = currentAddObjectIndex > 0
                ? currentAddObjectIndex
                : CommandReceiver.GlobalGlueToGameCommands.Count;
            for (int i = 0; i < currentAddObjectIndex; i++)
            {
                var dto = CommandReceiver.GlobalGlueToGameCommands[i];
                if (dto is Dtos.AddObjectDto addObjectDtoRerun)
                {
                    HandleCreateInstanceCommandFromGlue(addObjectDtoRerun, currentAddObjectIndex, newEntity);
                }
                else if (dto is Dtos.GlueVariableSetData glueVariableSetDataRerun)
                {
                    GlueControl.Editing.VariableAssignmentLogic.SetVariable(glueVariableSetDataRerun, newEntity);
                }
                else if (dto is RemoveObjectDto removeObjectDtoRerun)
                {
                    HandleDeleteInstanceCommandFromGlue(removeObjectDtoRerun, newEntity);
                }
            }
        }

        public PositionedObject CreateEntity(string entityNameGameType, int currentAddObjectIndex = -1, bool isMainEntityInScreen = false)
        {
            var containsKey =
                CustomGlueElements.ContainsKey(entityNameGameType);
            if (!containsKey && !string.IsNullOrWhiteSpace(entityNameGameType) && entityNameGameType.Contains('.') == false)
            {
                // It may not be qualified, which means it is coming from content that doesn't qualify - like Tiled
                entityNameGameType = CustomGlueElements.Keys.FirstOrDefault(item => item.Split('.').Last() == entityNameGameType);
                // Now that we've qualified, try again
                if (!string.IsNullOrWhiteSpace(entityNameGameType))
                {
                    containsKey =
                        CustomGlueElements.ContainsKey(entityNameGameType);
                }
            }

            PositionedObject newEntity = null;

            // This function may be given a qualified name like MyGame.Entities.MyEntity (if from Glue) 
            // or an unqualified name like MyEntity (if from Tiled). If from Tiled, then this code attempts
            // to fully qualify the entity name. This attempt to qualify may make the name null, so we need to
            // check and tolerate null.
            if (string.IsNullOrWhiteSpace(entityNameGameType))
            {
                newEntity = null;
            }
            else if (containsKey)
            {
                var dynamicEntityInstance = new Runtime.DynamicEntity();
                dynamicEntityInstance.EditModeType = entityNameGameType;
                SpriteManager.AddPositionedObject(dynamicEntityInstance);

                DestroyablesAddedAtRuntime.Add(dynamicEntityInstance);

                newEntity = dynamicEntityInstance;

                if (!isMainEntityInScreen)
                {
                    // If it is, then the game screen will run all the commands. No need to do it here and have 2x the commands run.
                    ApplyEditorCommandsToNewEntity(newEntity, currentAddObjectIndex);
                }
            }
            else
            {
                PositionedObject newPositionedObject;
                var factory = FlatRedBall.TileEntities.TileEntityInstantiator.GetFactory(entityNameGameType);
                if (factory != null)
                {
                    newPositionedObject = factory?.CreateNew() as FlatRedBall.PositionedObject;
                }
                else
                {
                    // just instantiate it using reflection?
                    var typeName = entityNameGameType;
                    bool ignoreCase = false;
                    var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
                    System.Reflection.Binder binder = null;

                    string contentManagerName = FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName;

                    if (ScreenManager.IsInEditMode)
                    {
                        var entityType = this.GetType().Assembly.GetType(entityNameGameType);
                        var hasBeenLoadedProperty = entityType?.GetProperty("HasBeenLoadedWithGlobalContentManager");
                        if (hasBeenLoadedProperty != null)
                        {
                            var hasBeenLoaded = (bool)hasBeenLoadedProperty.GetValue(null);
                            if (hasBeenLoaded)
                            {
                                contentManagerName = FlatRedBall.FlatRedBallServices.GlobalContentManager;
                            }
                        }
                    }

                    object[] args = new object[]
                    {
                        contentManagerName, // content manager
                        true // add to managers
                    };

                    newPositionedObject = this.GetType().Assembly.CreateInstance(entityNameGameType, ignoreCase, bindingFlags, binder, args, culture: null, activationAttributes: null)
                         as PositionedObject;
                }
                if (newPositionedObject != null && newPositionedObject is IDestroyable asDestroyable)
                {
                    DestroyablesAddedAtRuntime.Add(asDestroyable);
                }
                newEntity = newPositionedObject;

                if (factory == null)
                {
                    ApplyEditorCommandsToNewEntity(newEntity, currentAddObjectIndex);
                }
            }


            return newEntity;
        }

        private void AssignVariablesOnNewlyCreatedObject(Models.NamedObjectSave deserialized, object newObject)
        {
            if (newObject is FlatRedBall.Utilities.INameable asNameable)
            {
                asNameable.Name = deserialized.InstanceName;
            }
            if (newObject is PositionedObject asPositionedObject)
            {
                if (ScreenManager.IsInEditMode)
                {
                    asPositionedObject.Velocity = Microsoft.Xna.Framework.Vector3.Zero;
                    asPositionedObject.Acceleration = Microsoft.Xna.Framework.Vector3.Zero;
                }
                asPositionedObject.CreationSource = "Glue"; // Glue did make this, so do this so the game can select it
            }

            foreach (var instruction in deserialized.InstructionSaves)
            {
                AssignVariable(newObject, instruction, convertFileNamesToObjects: true, deserialized);
            }
        }

        #endregion

        #region Delete Instance from Glue

        public RemoveObjectDtoResponse HandleDeleteInstanceCommandFromGlue(RemoveObjectDto removeObjectDto, PositionedObject forcedItem = null)
        {
            RemoveObjectDtoResponse response = new RemoveObjectDtoResponse();
            response.DidScreenMatch = false;
            response.WasObjectRemoved = false;

            for (int i = 0; i < removeObjectDto.ObjectNames.Count; i++)
            {
                var objectName = removeObjectDto.ObjectNames[i];
                var elementNameGlue = removeObjectDto.ElementNamesGlue[i];
                HandleDeleteObject(forcedItem, elementNameGlue, objectName, response);
            }



            return response;
        }

        private void HandleDeleteObject(PositionedObject forcedItem, string elementNameGlue, string objectName, RemoveObjectDtoResponse response)
        {
            var elementGameType = CommandReceiver.GlueToGameElementName(elementNameGlue);

            var ownerType = this.GetType().Assembly.GetType(elementGameType);
            GlueElement ownerElement = null;
            if (CustomGlueElements.ContainsKey(elementGameType))
            {
                ownerElement = CustomGlueElements[elementGameType];
            }


            var removedFromEntity =
                (ownerType != null && typeof(PositionedObject).IsAssignableFrom(ownerType))
                ||
                ownerElement != null && ownerElement is EntitySave;


            if (removedFromEntity)
            {
                if (forcedItem != null)
                {
                    if (CommandReceiver.DoTypesMatch(forcedItem, elementGameType))
                    {
                        var objectToDelete = forcedItem.Children.FindByName(objectName);
                        if (objectToDelete != null)
                        {
                            TryDeleteObject(response, objectToDelete);
                        }
                    }
                }
                else // Vic says - but what if we have entities inside of entities? Skipping this logic may result in those subentities not being set up correctly?
                {

                    foreach (var item in SpriteManager.ManagedPositionedObjects)
                    {
                        if (CommandReceiver.DoTypesMatch(item, elementGameType, ownerType))
                        {
                            // try to remove this object from here...
                            //screen.ApplyVariable(variableNameOnObjectInInstance, variableValue, item);
                            var objectToDelete = item.Children.FindByName(objectName);

                            if (objectToDelete != null)
                            {
                                TryDeleteObject(response, objectToDelete);
                            }
                        }
                    }
                }
            }
            // see VariableAssignmentLogic.SetVariable by `var setOnEntity =` code for info on why we do this check
            else if (forcedItem == null)
            {
                bool matchesCurrentScreen =
                    (ScreenManager.CurrentScreen.GetType().FullName == elementGameType || ownerType?.IsAssignableFrom(ScreenManager.CurrentScreen.GetType()) == true);

                if (matchesCurrentScreen)
                {
                    response.DidScreenMatch = true;
                    var isEditingEntity =
                        ScreenManager.CurrentScreen?.GetType() == typeof(Screens.EntityViewingScreen);
                    var editingMode = isEditingEntity
                        ? GlueControl.Editing.ElementEditingMode.EditingEntity
                        : GlueControl.Editing.ElementEditingMode.EditingScreen;

                    var foundPositionedObject = GlueControl.Editing.SelectionLogic.GetAvailableObjects(editingMode)
                            .FirstOrDefault(item => item.Name == objectName);
                    if (foundPositionedObject != null)
                    {
                        TryDeleteObject(response, foundPositionedObject);
                    }
                    if (!response.WasObjectRemoved)
                    {
                        var element = ObjectFinder.Self.GetElement(elementNameGlue);
                        var nos = element?.NamedObjects.FirstOrDefault(item => item.InstanceName == objectName);

                        if (nos?.IsList == true)
                        {
                            if (nos.DefinedByBase)
                            {
                                // If this is defined by base, there's really nothing to do here. The removal of the base would
                                // result in the object actually getting removed..
                                // Treat this as true so we don't restart the game through FRB, or run any additional logic...
                                response.WasObjectRemoved = true;
                            }
                            else
                            {
                                var list = VariableAssignmentLogic.GetRuntimeInstanceRecursively(ScreenManager.CurrentScreen, objectName) as
                                    System.Collections.IList;

                                if (list != null)
                                {
                                    response.WasObjectRemoved = true;
                                    // this list is gone, so let's remove it from the factories 
                                    var factories = FactoryManager.GetAllFactories();
                                    foreach (var factory in factories)
                                    {
                                        factory.ListsToAddTo.Remove(list);
                                    }
                                }
                            }
                        }
                    }

                    if (!response.WasObjectRemoved)
                    {
                        // see if there is a collision relationship with this name
                        var matchingCollisionRelationship = FlatRedBall.Math.Collision.CollisionManager.Self.Relationships.FirstOrDefault(
                            item => item.Name == objectName);

                        if (matchingCollisionRelationship != null)
                        {
                            FlatRedBall.Math.Collision.CollisionManager.Self.Relationships.Remove(matchingCollisionRelationship);
                            response.WasObjectRemoved = true;
                        }
                    }
                }
            }
        }

        private static void TryDeleteObject(RemoveObjectDtoResponse removeResponse, PositionedObject objectToDelete)
        {
            if (objectToDelete is IDestroyable asDestroyable)
            {
                asDestroyable.Destroy();
                removeResponse.WasObjectRemoved = true;
            }
            else if (objectToDelete is AxisAlignedRectangle rectangle)
            {
                ShapeManager.Remove(rectangle);
                removeResponse.WasObjectRemoved = true;
            }
            else if (objectToDelete is Circle circle)
            {
                ShapeManager.Remove(circle);
                removeResponse.WasObjectRemoved = true;
            }
            else if (objectToDelete is Polygon polygon)
            {
                ShapeManager.Remove(polygon);
                removeResponse.WasObjectRemoved = true;
            }
            else if (objectToDelete is Line line)
            {
                ShapeManager.Remove(line);
                removeResponse.WasObjectRemoved = true;
            }
            else if (objectToDelete is Sprite sprite)
            {
                SpriteManager.RemoveSprite(sprite);
                removeResponse.WasObjectRemoved = true;
            }
            else if (objectToDelete is Text text)
            {
                TextManager.RemoveText(text);
                removeResponse.WasObjectRemoved = true;
            }
#if HasGum
            else if (objectToDelete is GumCoreShared.FlatRedBall.Embedded.PositionedObjectGueWrapper gumWrapper)
            {
                gumWrapper.GumObject.Destroy();
                gumWrapper.RemoveSelfFromListsBelongingTo();
            }
#endif
        }


        #endregion

        //private static void SendAndEnqueue(Dtos.AddObjectDto addObjectDto)
        //{
        //    var currentScreen = FlatRedBall.Screens.ScreenManager.CurrentScreen;
        //    if (currentScreen is Screens.EntityViewingScreen entityViewingScreen)
        //    {
        //        addObjectDto.ElementNameGame = entityViewingScreen.CurrentEntity.GetType().FullName;
        //    }
        //    else
        //    {
        //        addObjectDto.ElementNameGame = currentScreen.GetType().FullName;
        //    }

        //    GlueControlManager.Self.SendToGlue(addObjectDto);

        //    CommandReceiver.GlobalGlueToGameCommands.Add(addObjectDto);
        //}

        #region Delete Instance from Game

        public void DeleteInstancesByGame(List<INameable> instances)
        {
            // Vic June 27, 2021
            // this sends a command to Glue to delete the object, but doesn't
            // actually delete it in game until Glue tells the game to get rid
            // of it. Is that okay? it's a little slower, but it works. Maybe at
            // some point in the future I'll find a reason why it needs to be immediate.
            // Update - January 16, 2022
            // This does take a little bit of time, and we can make the game way more responsive
            // by batching.
            var dto = new Dtos.RemoveObjectDto();
            dto.ObjectNames = instances.Select(item => item.Name).ToList();

            GlueControlManager.Self.SendToGlue(dto);
        }

        #endregion

        public void AssignVariable(object newRuntimeInstance, FlatRedBall.Content.Instructions.InstructionSave instruction, bool convertFileNamesToObjects, NamedObjectSave nos = null)
        {
            string variableName = instruction.Member;
            object variableValue = instruction.Value;

            Type stateType = VariableAssignmentLogic.TryGetStateType(instruction.Type);

            var valueAsString = variableValue as string;

            string conversionReport = "";

            if (variableValue is string)
            {
                // only convert this if the instance is 
                variableValue = VariableAssignmentLogic.ConvertStringToType(instruction.Type, valueAsString, stateType != null, out conversionReport, convertFileNamesToObjects);
            }
            else if (stateType != null && variableValue is string && !string.IsNullOrWhiteSpace(valueAsString))
            {
                var fieldInfo = stateType.GetField(valueAsString);

                variableValue = fieldInfo.GetValue(null);
            }
            else if (instruction.Type == "int")
            {
                if (variableValue is long asLong)
                {
                    variableValue = (int)asLong;
                }
            }
            else if (instruction.Type == "int?")
            {
                if (variableValue is long asLong)
                {
                    variableValue = (int?)asLong;
                }
            }
            else if (instruction.Type == "float" || instruction.Type == "Single")
            {
                if (variableValue is int asInt)
                {
                    variableValue = (float)asInt;
                }
                else if (variableValue is double asDouble)
                {
                    variableValue = (float)asDouble;
                }
            }
            else if (instruction.Type == "decimal" || instruction.Type == "Decimal")
            {
                if (variableValue is int asInt)
                {
                    variableValue = (decimal)asInt;
                }
                else if (variableValue is double asDouble)
                {
                    variableValue = (decimal)asDouble;
                }
            }
            else if (instruction.Type == "float?")
            {
                if (variableValue is int asInt)
                {
                    variableValue = (float?)asInt;
                }
                else if (variableValue is double asDouble)
                {
                    variableValue = (float?)asDouble;
                }
            }
            else if (instruction.Type == typeof(FlatRedBall.Graphics.Animation.AnimationChainList).FullName ||
                instruction.Type == typeof(Microsoft.Xna.Framework.Graphics.Texture2D).FullName)
            {
                if (convertFileNamesToObjects && variableValue is string asString && !string.IsNullOrWhiteSpace(asString))
                {
                    variableValue = Editing.VariableAssignmentLogic.ConvertStringToType(instruction.Type, asString, false, out conversionReport);
                }
            }
            else if (instruction.Type == typeof(Microsoft.Xna.Framework.Color).FullName)
            {
                if (variableValue is string asString && !string.IsNullOrWhiteSpace(asString))
                {
                    variableValue = Editing.VariableAssignmentLogic.ConvertStringToType(instruction.Type, asString, false, out conversionReport);
                }
            }
            else if (instruction.Type == typeof(Microsoft.Xna.Framework.Graphics.TextureAddressMode).Name)
            {
                if (variableValue is int asInt)
                {
                    variableValue = (Microsoft.Xna.Framework.Graphics.TextureAddressMode)asInt;
                }
                if (variableValue is long asLong)
                {
                    variableValue = (Microsoft.Xna.Framework.Graphics.TextureAddressMode)asLong;
                }
            }
            else if (
                instruction.Type == typeof(FlatRedBall.Graphics.ColorOperation).Name ||
                instruction.Type == typeof(FlatRedBall.Graphics.ColorOperation).FullName
                )
            {
                if (variableValue is int asInt)
                {
                    variableValue = (FlatRedBall.Graphics.ColorOperation)asInt;
                }
                if (variableValue is long asLong)
                {
                    variableValue = (FlatRedBall.Graphics.ColorOperation)asLong;
                }
            }
            else if (instruction.Type == "List<Vector2>")
            {
                if (variableValue is Newtonsoft.Json.Linq.JArray jArray)
                {
                    var vectorList = new List<Microsoft.Xna.Framework.Vector2>();
                    foreach (var item in jArray)
                    {
                        // does this need to be localized?
                        var noQuotes = item.ToString().Replace("\"", "");
                        var floatBeforeComma = float.Parse(noQuotes.Substring(0, noQuotes.IndexOf(',')));
                        var floatAfterComma = float.Parse(noQuotes.Substring(noQuotes.IndexOf(',') + 1));
                        var vector2 = new Microsoft.Xna.Framework.Vector2();
                        vector2.X = floatBeforeComma;
                        vector2.Y = floatAfterComma;
                        vectorList.Add(vector2);
                    }

                    variableValue = vectorList;

                    var isSettingPointsOnPolygon = nos.SourceClassType == "Polygon" || nos.SourceClassType == "FlatRedBall.Math.Geometry.Polygon";

                    if (!isSettingPointsOnPolygon && nos.SourceType == SourceType.Entity)
                    {
                        var nosEntityType = ObjectFinder.Self.GetEntitySave(nos.SourceClassType);

                        var member = nosEntityType.CustomVariables.Find(item => item.Name == variableName);

                        var foundMember = ObjectFinder.Self.GetRootCustomVariable(member);

                        if (foundMember.Name == "Points")
                        {
                            isSettingPointsOnPolygon = true;
                        }
                    }

                    if (isSettingPointsOnPolygon)
                    {
                        // value is actually a List of FlatRedBall Points, so convert vectorList to a list of Points
                        var pointList = new List<FlatRedBall.Math.Geometry.Point>();
                        foreach (var vector in vectorList)
                        {
                            pointList.Add(new FlatRedBall.Math.Geometry.Point(vector.X, vector.Y));
                        }
                        variableValue = pointList;
                    }
                }
            }

            try
            {
                // special case X and Y if attached
                if ((variableName == "X" || variableName == "Y" || variableName == "RotationZ") && newRuntimeInstance is PositionedObject asPositionedObject && asPositionedObject.Parent != null)
                {
                    if (variableName == "X")
                    {
                        asPositionedObject.RelativeX = (float)variableValue;
                    }
                    else if (variableName == "Y")
                    {
                        asPositionedObject.RelativeY = (float)variableValue;
                    }
                    else if (variableName == "RotationZ")
                    {
                        asPositionedObject.RelativeRotationZ = (float)variableValue;
                    }

                }
                else
                {
                    FlatRedBall.Instructions.Reflection.LateBinder.SetValueStatic(newRuntimeInstance, variableName, variableValue);
                }
            }
            catch (MemberAccessException)
            {
                // for info on why this exception is caught, search for MemberAccessException in this file
            }
            catch (Exception e)
            {
                throw new Exception(conversionReport, e);
            }
        }

        public void DestroyDynamicallyAddedInstances()
        {
            for (int i = ShapesAddedAtRuntime.AxisAlignedRectangles.Count - 1; i > -1; i--)
            {
                ShapeManager.Remove(ShapesAddedAtRuntime.AxisAlignedRectangles[i]);
            }

            for (int i = ShapesAddedAtRuntime.Circles.Count - 1; i > -1; i--)
            {
                ShapeManager.Remove(ShapesAddedAtRuntime.Circles[i]);
            }

            for (int i = ShapesAddedAtRuntime.Polygons.Count - 1; i > -1; i--)
            {
                ShapeManager.Remove(ShapesAddedAtRuntime.Polygons[i]);
            }


            for (int i = SpritesAddedAtRuntime.Count - 1; i > -1; i--)
            {
                SpriteManager.RemoveSprite(SpritesAddedAtRuntime[i]);
            }

            for (int i = TextsAddedAtRuntime.Count - 1; i > -1; i--)
            {
                TextManager.RemoveText(TextsAddedAtRuntime[i]);
            }

            for (int i = DestroyablesAddedAtRuntime.Count - 1; i > -1; i--)
            {
                DestroyablesAddedAtRuntime[i].Destroy();
            }

            foreach (var list in ListsAddedAtRuntime)
            {
                for (int i = list.Count - 1; i > -1; i--)
                {
                    var positionedObject = list[i] as PositionedObject;
                    positionedObject.RemoveSelfFromListsBelongingTo();
                }
            }

#if HasGum

            for (int i = GumObjectsAddedAtRuntime.Count - 1; i > -1; i--)
            {
                GumObjectsAddedAtRuntime[i].Destroy();
            }
            for(int i = GumWrappersAddedAtRuntime.Count - 1; i > -1; i--)
            {
                GumWrappersAddedAtRuntime[i].RemoveSelfFromListsBelongingTo();
            }
            GumObjectsAddedAtRuntime.Clear();
            GumWrappersAddedAtRuntime.Clear();
#endif

            ShapesAddedAtRuntime.Clear();
            SpritesAddedAtRuntime.Clear();
            DestroyablesAddedAtRuntime.Clear();
            ListsAddedAtRuntime.Clear();
            TextsAddedAtRuntime.Clear();
        }
    }
}
