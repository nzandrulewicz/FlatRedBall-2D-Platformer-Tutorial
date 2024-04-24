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
using FlatRedBall.Input;
using FlatRedBall.Managers;
using FlatRedBall.Math;
using FlatRedBall.Screens;
using FlatRedBall.TileCollisions;
using FlatRedBall.Utilities;
using GlueControl.Models;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlueControl.Managers;


namespace GlueControl.Editing
{
    #region Enums

    public enum ElementEditingMode
    {
        EditingScreen,
        EditingEntity
    }

    #endregion

    #region Classes

    class NameableWrapper : INameable, IStaticPositionable
    {
        public string Name { get; set; }

        IStaticPositionable containedAsPositionable;
        object containedObject;
        public object ContainedObject
        {
            get => containedObject;
            set
            {
                containedObject = value;
                containedAsPositionable = value as IStaticPositionable;
            }
        }

        public float X
        {
            get => containedAsPositionable?.X ?? 0;
            set
            {
                if (containedAsPositionable != null)
                {
                    containedAsPositionable.X = value;
                }
            }
        }
        public float Y
        {
            get => containedAsPositionable?.Y ?? 0;
            set
            {
                if (containedAsPositionable != null)
                {
                    containedAsPositionable.Y = value;
                }
            }
        }
        public float Z
        {
            get => containedAsPositionable?.Z ?? 0;
            set
            {
                if (containedAsPositionable != null)
                {
                    containedAsPositionable.Z = value;
                }
            }
        }
    }

    class PropertyChangeArgs
    {
        public INameable Nameable { get; set; }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }

    #endregion

    class EditingManager : IManager
    {
        #region Fields/Properties

        Guides Guides;

        bool showGrid = true;
        public bool ShowGrid
        {
            get => showGrid;
            set
            {
                showGrid = value;
                Guides.Visible = value;
            }
        }

        decimal gridAlpha;
        public decimal GridAlpha
        {
            get => gridAlpha;
            set
            {
                gridAlpha = value;

                var premultValue = (byte)(value * 255);

                Guides.SmallGridLineColor.R = premultValue;
                Guides.SmallGridLineColor.G = premultValue;
                Guides.SmallGridLineColor.B = premultValue;
                Guides.SmallGridLineColor.A = premultValue;

                Guides.RefreshColors();

            }
        }

        public float GuidesGridSpacing
        {
            get => Guides.GridSpacing;
            set => Guides.GridSpacing = value;
        }

        float snapSize = 8;
        public float SnapSize
        {
            get => snapSize;
            set
            {
                snapSize = value;
                foreach (var marker in SelectedMarkers)
                {
                    if (marker is SelectionMarker selectionMarker)
                    {
                        selectionMarker.PositionSnappingSize = snapSize;
                    }
                }
            }
        }

        float polygonPointSnapSize = 1;
        public float PolygonPointSnapSize
        {
            get => polygonPointSnapSize;
            set
            {
                polygonPointSnapSize = value;
                foreach (var marker in SelectedMarkers)
                {
                    if (marker is SelectionMarker selectionMarker)
                    {
                        selectionMarker.PolygonPointSnapSize = polygonPointSnapSize;
                    }
                }
            }
        }

        bool isSnappingEnabled;
        public bool IsSnappingEnabled
        {
            get => isSnappingEnabled;
            set
            {
                isSnappingEnabled = value;
                foreach (var marker in SelectedMarkers)
                {
                    if (marker is SelectionMarker selectionMarker)
                    {
                        selectionMarker.IsSnappingEnabled = isSnappingEnabled;
                    }
                }
            }
        }

        List<ISelectionMarker> SelectedMarkers = new List<ISelectionMarker>();
        SelectionMarker HighlightMarker;
        MeasurementMarker MeasurementMarker;

        List<INameable> itemsOver = new List<INameable>();
        public IEnumerable<INameable> ItemsOver => itemsOver;

        IStaticPositionable itemGrabbed;
        public IStaticPositionable ItemGrabbed => itemGrabbed;

        List<INameable> itemsSelected = new List<INameable>();
        List<INameable> itemsOverLastFrame = new List<INameable>();
        public IEnumerable<INameable> ItemsSelected => itemsSelected;
        INameable ItemSelected => itemsSelected.Count > 0 ? itemsSelected[0] : null;

        List<PropertyChangeArgs> bufferedChangeArgs = new List<PropertyChangeArgs>();
        bool IsBuffering;

        public GlueElement CurrentGlueElement { get; private set; }
        public List<NamedObjectSave> CurrentNamedObjects { get; private set; } = new List<NamedObjectSave>();

        public ElementEditingMode ElementEditingMode { get; set; }

        const float SelectedItemExtraPadding = 2;

        CopyPasteManager CopyPasteManager = new CopyPasteManager();

        public static EditingManager Self { get; private set; }

        bool wasGameActive;

        bool wasPushedInWindow;


        #endregion

        #region Delegates/Events

        public Action<List<PropertyChangeArgs>> PropertyChanged;
        public Action<List<INameable>> ObjectSelected;

        #endregion

        #region Constructor

        public EditingManager()
        {
            Self = this;
            HighlightMarker = new SelectionMarker(null);
            HighlightMarker.BrightColor = Color.LightGreen;
            HighlightMarker.MakePersistent();
            HighlightMarker.Name = nameof(HighlightMarker);
            HighlightMarker.CanMoveItem = false;

            MeasurementMarker = new MeasurementMarker();

            Guides = new Guides();
        }

        #endregion

        #region Markers

        private void UpdateMarkers(bool didChangeItemOver)
        {
            // By buffering, we dont' send every command to the game directly. This is important if a group of objects is selected:
            IsBuffering = true;

            if (didChangeItemOver)
            {
                HighlightMarker.FadingSeed = TimeManager.CurrentTime;
            }

            HighlightMarker.ExtraPaddingInPixels = 4;
            // do we want to show multiple highlights? Probably?
            HighlightMarker.Owner = itemsOver.FirstOrDefault();
            HighlightMarker.Update(!wasGameActive && FlatRedBallServices.Game.IsActive);

            UpdateSelectedMarkers();

            IsBuffering = false;
            if (bufferedChangeArgs.Count > 0)
            {
                PropertyChanged(bufferedChangeArgs.ToList());

                bufferedChangeArgs.Clear();
            }
        }

        private void UpdateSelectedMarkers()
        {
            Vector3 moveVector = Vector3.Zero;
            for (int i = 0; i < itemsSelected.Count; i++)
            {
                var marker = SelectedMarkers[i];
                var item = itemsSelected[i];

                marker.Update(!wasGameActive && FlatRedBallServices.Game.IsActive);

                if (item == itemGrabbed)
                {
                    moveVector = marker.LastUpdateMovement;
                }
            }

            if (moveVector.X != 0 || moveVector.Y != 0)
            {
                foreach (var item in itemsSelected)
                {
                    if (item != itemGrabbed && item is PositionedObject itemAsPositionedObject)
                    {
                        if (itemAsPositionedObject.Parent == null)
                        {
                            itemAsPositionedObject.X += moveVector.X;
                            itemAsPositionedObject.Y += moveVector.Y;
                        }
                        else
                        {
                            itemAsPositionedObject.RelativeX += moveVector.X;
                            itemAsPositionedObject.RelativeY += moveVector.Y;
                        }
                    }
                }
            }
        }

        private void AddAndDestroyMarkersAccordingToItemsSelected()
        {
            var desiredMarkerCount = itemsSelected.Count;

#if MONOGAME_381

            var itemsSelectedHash = itemsSelected.ToHashSet();
#else
            var itemsSelectedHash = itemsSelected;

#endif

            for (int i = SelectedMarkers.Count - 1; i > -1; i--)
            {
                var marker = SelectedMarkers[i];
                var hasSelectedItem = itemsSelectedHash.Contains(marker.Owner);

                if (!hasSelectedItem)
                {
                    marker.Destroy();
                    SelectedMarkers.RemoveAt(i);
                }
            }

            HashSet<INameable> markerItems = new HashSet<INameable>();
            for (int i = 0; i < SelectedMarkers.Count; i++)
            {
                markerItems.Add(SelectedMarkers[i].Owner);
            }

            for (int i = 0; i < itemsSelected.Count; i++)
            {
                var owner = itemsSelected[i];
                var hasMarker = markerItems.Contains(owner);

                if (!hasMarker)
                {
                    var newMarker = CreateNewSelectionMarker(owner);
                    if (SelectedMarkers.Count > 0)
                    {
                        newMarker.FadingSeed = SelectedMarkers[0].FadingSeed;
                    }
                    SelectedMarkers.Add(newMarker);
                    markerItems.Add(owner);

                }
            }
        }

        private ISelectionMarker CreateNewSelectionMarker(INameable owner)
        {
            ISelectionMarker newMarker = null;
            if (owner is FlatRedBall.TileCollisions.TileShapeCollection)
            {
                // Assume with tile shape collections that only one object can be selected, so we can FirstOrDefault it:
                newMarker = new TileShapeCollectionMarker(owner, CurrentNamedObjects.FirstOrDefault());
            }
            else
            {
                var selectionMarker = new SelectionMarker(owner);
                selectionMarker.PositionSnappingSize = SnapSize;
                selectionMarker.PolygonPointSnapSize = PolygonPointSnapSize;
                selectionMarker.IsSnappingEnabled = IsSnappingEnabled;
                newMarker = selectionMarker;
            }
            newMarker.MakePersistent();

            newMarker.Name = "Selection Marker";
            newMarker.CanMoveItem = true;
            newMarker.PropertyChanged += HandleMarkerPropertyChanged;
            return newMarker;
        }

        private void HandleMarkerPropertyChanged(INameable item, string variable, object value)
        {
            var changeArgs = new PropertyChangeArgs
            {
                Nameable = item,
                PropertyName = variable,
                PropertyValue = value
            };

            if (IsBuffering)
            {
                bufferedChangeArgs.Add(changeArgs);
            }
            else
            {
                PropertyChanged(new List<PropertyChangeArgs> { changeArgs });
            }
        }

        ISelectionMarker MarkerFor(INameable item)
        {
            var index = itemsSelected.IndexOf(item);
            if (index >= 0 && index < SelectedMarkers.Count)
            {
                return SelectedMarkers[index];
            }
            return null;
        }

        #endregion

        #region Update/DoXXXXLogic
        public void Update()
        {
#if SupportsEditMode
            var isInEditMode = ScreenManager.IsInEditMode;

            Guides.Visible = isInEditMode && showGrid;


            if (isInEditMode && !ScreenManager.CurrentScreen.IsActivityFinished)
            {
                Guides.UpdateGridLines();

                itemsOverLastFrame.Clear();
                itemsOverLastFrame.AddRange(itemsOver);
                var itemSelectedBefore = ItemSelected;

                var mouse = FlatRedBall.Input.InputManager.Mouse;

                if(!mouse.ButtonDown(Mouse.MouseButtons.LeftButton) &&
                    !mouse.ButtonDown(Mouse.MouseButtons.RightButton) &&
                    !mouse.ButtonDown(Mouse.MouseButtons.MiddleButton))
                {
                    wasPushedInWindow = false;
                }
                else if(mouse.AnyButtonPushed())
                {
                    wasPushedInWindow = mouse.IsInGameWindow();
                }

                // Vic says - not sure how much should be inside the IsActive check
                if (FlatRedBallServices.Game.IsActive && mouse.IsInGameWindow())
                {
                    if (itemGrabbed == null && ItemsSelected.All(item => item is TileShapeCollection == false))
                    {
                        var gameBecameActive = !wasGameActive && FlatRedBallServices.Game.IsActive;

                        SelectionLogic.DoDragSelectLogic(gameBecameActive);
                    }
                    
                    var isCursorUsingMouse = FlatRedBall.Gui.GuiManager.Cursor.DevicesControllingCursor.Contains(mouse);

                    var isOverWindow = false;
                    if(isCursorUsingMouse && FlatRedBall.Gui.GuiManager.Cursor.WindowOver != null)
                    {
                        isOverWindow = true;
                    }

                    if(!isOverWindow)
                    {
                        SelectionLogic.GetItemsOver(itemsSelected, itemsOver, SelectedMarkers, mouse.ButtonDoublePushed(Mouse.MouseButtons.LeftButton), ElementEditingMode);
                    }
                    else
                    {
                        itemsOver.Clear();
                    }
                }
                else
                {
                    if(!mouse.IsInGameWindow())
                    {
                        itemsOver.Clear();
                    }
                    SelectionLogic.DoInactiveWindowLogic();
                }

                var didChangeItemOver = itemsOverLastFrame.Any(item => !itemsOver.Contains(item)) ||
                    itemsOver.Any(item => !itemsOverLastFrame.Contains(item));

                if (FlatRedBallServices.Game.IsActive)
                {
                    if (mouse.IsInGameWindow())
                    {
                        DoGrabLogic();
                    }

                    DoRectangleSelectLogic();

                    DoReleaseLogic();

                    DoHotkeyLogic();

                    if(mouse.IsInGameWindow() || wasPushedInWindow)
                    {
                        CameraLogic.DoCursorCameraControllingLogic(wasPushedInWindow);
                    }

                    DoForwardBackActivity();
                }

                CameraLogic.DoBackgroundLogic();

                UpdateMarkers(didChangeItemOver);

                UpdateMeasurementMarker();
            }
            else
            {
                HighlightMarker.Visible = false;
                if (SelectedMarkers.Count > 0)
                {
                    for (int i = SelectedMarkers.Count - 1; i > -1; i--)
                    {
                        SelectedMarkers[i].Destroy();
                    }
                    SelectedMarkers.Clear();
                }
                itemsSelected.Clear();
                itemsOver.Clear();
                itemGrabbed = null;

            }

            wasGameActive = FlatRedBallServices.Game.IsActive;

#endif
        }

        private void UpdateMeasurementMarker()
        {
            var doesMarkerUseRightClick = false;
            foreach (var item in this.SelectedMarkers)
            {
                if (item.UsesRightMouseButton)
                {
                    doesMarkerUseRightClick = true;
                }
            }

            if (!doesMarkerUseRightClick)
            {
                MeasurementMarker.Update();
            }
        }

        bool shouldPrintCurrentNamedObjectInformation = false;
        private void DoGrabLogic()
        {
            var mouse = FlatRedBall.Input.InputManager.Mouse;

            if (shouldPrintCurrentNamedObjectInformation)
            {
                PrintCurrentNamedObjectsInformation();

            }

            var shouldTreatAsPush = false;

            if (mouse.IsInGameWindow())
            {
                var didWindowActivate = !wasGameActive && FlatRedBallServices.Game.IsActive;

                shouldTreatAsPush = mouse.ButtonPushed(Mouse.MouseButtons.LeftButton) ||
                    (mouse.ButtonDown(Mouse.MouseButtons.LeftButton) && didWindowActivate);
            }
            if (shouldTreatAsPush)
            {
                var itemOver = itemsOver.FirstOrDefault();
                itemGrabbed = itemOver as IStaticPositionable;

                var clickedOnSelectedItem = itemsSelected.Contains(itemOver);

                var isCtrlDown = InputManager.Keyboard.IsCtrlDown;
                if (!clickedOnSelectedItem)
                {
                    if (itemOver?.Name == null)
                    {
                        Select((NamedObjectSave)null, addToExistingSelection: isCtrlDown, playBump: true);
                    }
                    else
                    {
                        NamedObjectSave nos = null;
                        if (itemOver?.Name != null)
                        {
                            nos = CurrentGlueElement?.AllNamedObjects.FirstOrDefault(item => item.InstanceName == itemOver.Name);
                        }

                        if (nos != null)
                        {
                            Select(nos, addToExistingSelection: isCtrlDown, playBump: true);
                        }
                        else
                        {
                            if (!isCtrlDown)
                            {
                                CurrentNamedObjects.Clear();
                            }
                            // this shouldn't happen, but for now we tolerate it until the current is sent
                            Select(itemOver?.Name, addToExistingSelection: isCtrlDown, playBump: true);
                        }
                    }
                }
                else if (isCtrlDown)
                {
                    NamedObjectSave nos = null;
                    if (itemOver?.Name != null)
                    {
                        nos = CurrentGlueElement?.AllNamedObjects.FirstOrDefault(item => item.InstanceName == itemOver.Name);
                    }
                    if (nos != null)
                    {
                        RemoveFromSelection(nos);
                    }
                }

                if (itemGrabbed != null)
                {
                    foreach (var item in itemsSelected)
                    {
                        var marker = MarkerFor(item);

                        marker.CanMoveItem = item == itemGrabbed;
                    }

                    if (!clickedOnSelectedItem)
                    {
                        if (isCtrlDown)
                        {
                            ObjectSelected(itemsSelected);
                        }
                        else
                        {
                            ObjectSelected(new List<INameable> { itemGrabbed as INameable });
                        }
                    }
                }
                else if (!clickedOnSelectedItem)
                {
                    ObjectSelected(new List<INameable>());
                }
            }
        }



        private void DoReleaseLogic()
        {
            var mouse = FlatRedBall.Input.InputManager.Mouse;

            ///////Early Out
            if (!mouse.ButtonReleased(Mouse.MouseButtons.LeftButton))
            {
                return;
            }
            //////End Early Out

            if (itemGrabbed != null)
            {
                foreach (var item in itemsSelected)
                {
                    var marker = MarkerFor(item);
                }

            }

            itemGrabbed = null;
        }

        private void DoHotkeyLogic()
        {
            var keyboard = FlatRedBall.Input.InputManager.Keyboard;

            if (keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Delete))
            {
                var handledByMarkers = false;

                foreach (var marker in SelectedMarkers)
                {
                    if (marker.HandleDelete())
                    {
                        handledByMarkers = true;
                        break;
                    }
                }

                if (!handledByMarkers)
                {
                    InstanceLogic.Self.DeleteInstancesByGame(itemsSelected);
                    itemsSelected.Clear();
                    AddAndDestroyMarkersAccordingToItemsSelected();
                }
            }

            DoGoToDefinitionLogic();

            DoNudgeHotkeyLogic();

            CopyPasteManager.DoHotkeyLogic(itemsSelected, CurrentNamedObjects, itemGrabbed);

            CameraLogic.DoHotkeyLogic();

            if (keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Z) && keyboard.IsCtrlDown)
            {
                GlueCommands.Self.Undo();
            }
        }

        private void DoForwardBackActivity()
        {
            var mouse = InputManager.Mouse;
            if (mouse.ButtonPushed(Mouse.MouseButtons.XButton1))
            {
                GlueControlManager.Self.SendToGlue(new Dtos.SelectPreviousDto());
            }
            else if (mouse.ButtonPushed(Mouse.MouseButtons.XButton2))
            {
                GlueControlManager.Self.SendToGlue(new Dtos.SelectNextDto());
            }
        }

        private void DoGoToDefinitionLogic()
        {
            var keyboard = InputManager.Keyboard;

            if (keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.F12))
            {
                GlueControlManager.Self.SendToGlue(new Dtos.GoToDefinitionDto());
            }
        }

        private void DoNudgeHotkeyLogic()
        {
            var keyboard = InputManager.Keyboard;

            var isShiftDown = keyboard.IsShiftDown;
            var shiftAmount = isShiftDown ? 8 : 1;
            if (keyboard.IsCtrlDown == false)
            {
                if (keyboard.KeyTyped(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    foreach (var item in itemsSelected)
                    {
                        if (item is PositionedObject asPositionedObject)
                        {
                            if (asPositionedObject.Parent != null)
                            {
                                asPositionedObject.RelativeY += shiftAmount;
                            }
                            else
                            {
                                asPositionedObject.Y += shiftAmount;
                            }
                            PropertyChanged(new List<PropertyChangeArgs>
                            {
                                new PropertyChangeArgs
                                {
                                    Nameable = item,
                                    PropertyName = nameof(asPositionedObject.Y),
                                    PropertyValue = asPositionedObject.Y
                                }
                            });
                        }
                    }
                }
                if (keyboard.KeyTyped(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    foreach (var item in itemsSelected)
                    {
                        if (item is PositionedObject asPositionedObject)
                        {
                            if (asPositionedObject.Parent != null)
                            {
                                asPositionedObject.RelativeY -= shiftAmount;
                            }
                            else
                            {
                                asPositionedObject.Y -= shiftAmount;
                            }
                            PropertyChanged(new List<PropertyChangeArgs>
                            {
                                new PropertyChangeArgs
                                {
                                    Nameable = item,
                                    PropertyName = nameof(asPositionedObject.Y),
                                    PropertyValue = asPositionedObject.Y
                                }
                            });
                        }
                    }
                }
                if (keyboard.KeyTyped(Microsoft.Xna.Framework.Input.Keys.Left))
                {
                    foreach (var item in itemsSelected)
                    {
                        if (item is PositionedObject asPositionedObject)
                        {
                            if (asPositionedObject.Parent != null)
                            {
                                asPositionedObject.RelativeX -= shiftAmount;
                            }
                            else
                            {
                                asPositionedObject.X -= shiftAmount;
                            }

                            var args = new PropertyChangeArgs
                            {
                                Nameable = item,
                                PropertyName = nameof(asPositionedObject.X),
                                PropertyValue = asPositionedObject.X
                            };

                            PropertyChanged(new List<PropertyChangeArgs>
                            {
                              args
                            });
                        }
                    }
                }
                if (keyboard.KeyTyped(Microsoft.Xna.Framework.Input.Keys.Right))
                {
                    foreach (var item in itemsSelected)
                    {
                        if (item is PositionedObject asPositionedObject)
                        {
                            if (asPositionedObject.Parent != null)
                            {
                                asPositionedObject.RelativeX += shiftAmount;
                            }
                            else
                            {
                                asPositionedObject.X += shiftAmount;
                            }
                            PropertyChanged(new List<PropertyChangeArgs>
                            {
                                new PropertyChangeArgs
                                {
                                    Nameable = item,
                                    PropertyName = nameof(asPositionedObject.X),
                                    PropertyValue = asPositionedObject.X
                                }
                            });
                        }
                    }
                }
            }
        }

        public bool IsDraggingRectangle => SelectionLogic.LeftSelect != null;

        private void DoRectangleSelectLogic()
        {
            if (SelectionLogic.PerformedRectangleSelection)
            {

                // let's make a dictionary to make this faster:
                Dictionary<string, NamedObjectSave> namedObjectSaveDictionary = new Dictionary<string, NamedObjectSave>();
                var all = CurrentGlueElement?.AllNamedObjects;
                if (all != null)
                {
                    foreach (var item in all)
                    {
                        namedObjectSaveDictionary.Add(item.InstanceName, item);
                    }
                }

                if (CurrentGlueElement != null)
                {
                    var derivedElements = ObjectFinder.Self.GetAllBaseElementsRecursively(CurrentGlueElement);
                    foreach (var derivedElement in derivedElements)
                    {
                        foreach (var item in derivedElement.AllNamedObjects)
                        {
                            // let the derived have priority, check if it already exists:
                            if (namedObjectSaveDictionary.ContainsKey(item.InstanceName) == false)
                            {
                                namedObjectSaveDictionary.Add(item.InstanceName, item);
                            }
                        }
                    }
                }

                var isFirst = true;

                var cache = SelectionLogic.GetAvailableObjects(ElementEditingMode).ToArray();


                foreach (var itemOver in ItemsOver)
                {
                    NamedObjectSave nos = null;
                    if (itemOver?.Name != null && namedObjectSaveDictionary.ContainsKey(itemOver.Name))
                    {
                        nos = namedObjectSaveDictionary[itemOver.Name];
                    }

                    if (nos != null)
                    {
                        var isEditingLocked =
                            ObjectFinder.Self.GetPropertyValueRecursively<bool>(
                                nos, nameof(nos.IsEditingLocked));
                        if (isEditingLocked == false)
                        {
                            Select(nos, addToExistingSelection: isFirst == false, playBump: false, updateMarkers: false, availablePositionedObjectCache: cache);
                        }
                    }
                    else
                    {
                        // this shouldn't happen, but for now we tolerate it until the current is sent
                        Select(itemOver?.Name, addToExistingSelection: isFirst == false, playBump: false, updateMarkers: false);
                    }


                    isFirst = false;
                }

                AddAndDestroyMarkersAccordingToItemsSelected();


                foreach (var marker in SelectedMarkers)
                {
                    marker.PlayBumpAnimation(SelectedItemExtraPadding, isSynchronized: false);
                }

                UpdateMarkers(didChangeItemOver: true);

                ObjectSelected(ItemsOver.ToList());
            }
        }
        #endregion

        public void UpdateDependencies()
        {

        }

        public void SetCurrentGlueElement(GlueElement glueElement)
        {
            if (glueElement?.AllNamedObjects.Any(item => item == null) == true)
            {
                throw new ArgumentException($"There are null items in the the glueElement being sent over, there shouldn't be!\n{glueElement}");
            }
            var oldGlueElement = CurrentGlueElement;

            CurrentGlueElement = glueElement;

            var oldNames = CurrentNamedObjects.Where(item => item != null).Select(item => item.InstanceName).ToArray();
            CurrentNamedObjects.Clear();
            var newNamedObjects = glueElement?.AllNamedObjects.Where(item => oldNames.Contains(item.FieldName)).ToArray();
            // Note - this will fail if the this is being called as a result of a rename. Therefore, the caller is responsbile
            // for re-selecting the NOS
            CurrentNamedObjects.AddRange(newNamedObjects);
        }

        public void ReplaceNamedObjectSave(NamedObjectSave nos, string glueElementName, string containerName)
        {
            ///////////////////Early Out///////////////////
            if (CurrentGlueElement?.Name != glueElementName)
            {
                return;
            }
            ////////////////End Early Out//////////////////

            var oldNos = CurrentGlueElement.AllNamedObjects.FirstOrDefault(item => item.InstanceName == nos.InstanceName);
            var oldContainer = CurrentGlueElement.AllNamedObjects.FirstOrDefault(item => item.ContainedObjects.Contains(oldNos));

            NamedObjectSave newContainer = null;
            if (!string.IsNullOrEmpty(containerName))
            {
                newContainer = CurrentGlueElement.AllNamedObjects.FirstOrDefault(item => item.InstanceName == containerName);
            }

            // Does index matter? Maybe eventually...
            if (oldContainer != null)
            {
                oldContainer.ContainedObjects.Remove(oldNos);
            }

            if (newContainer != null)
            {
                if (oldNos != null)
                {
                    newContainer.ContainedObjects.Remove(oldNos);
                }
                newContainer.ContainedObjects.Add(nos);
            }
            else
            {
                if (oldNos != null)
                {
                    CurrentGlueElement.NamedObjects.Remove(oldNos);
                }
                CurrentGlueElement.NamedObjects.Add(nos);
            }

            if (CurrentNamedObjects.Contains(oldNos))
            {
                // Index matters here because the order should match the same order of the runtime objects
                var index = CurrentNamedObjects.IndexOf(oldNos);
                CurrentNamedObjects.Remove(oldNos);

                CurrentNamedObjects.Insert(index, nos);
            }
            // Things can change due to a re-load so let's check for names too
            else
            {
                var matchingNameNos = CurrentNamedObjects.FirstOrDefault(item => item.InstanceName == nos.InstanceName);
                if (matchingNameNos != null)
                {
                    var index = CurrentNamedObjects.IndexOf(matchingNameNos);
                    CurrentNamedObjects.Remove(matchingNameNos);

                    CurrentNamedObjects.Insert(index, nos);
                }
            }

            nos.FixAllTypes();
        }


        #region Diagnostics

        private void PrintCurrentNamedObjectsInformation()
        {
            var text = $"{CurrentGlueElement?.AllNamedObjects.Count()} NOSes in current element\n";
            foreach (var nos in CurrentNamedObjects)
            {
                text += nos.InstanceName + "\n";
                foreach (var instruction in nos.InstructionSaves)
                {
                    text += $"  {instruction.Member}={instruction.Value}";
                }
                text += "\n";
            }

            WriteDiagnosticText(text);
        }

        private void PrintAvailableObjects()
        {
            var availableObjects = SelectionLogic.GetAvailableObjects(ElementEditingMode);
            var text = $"Number of available objects: {availableObjects.Count()}\n";
            var sublist = availableObjects.Take(30);
            foreach (var item in sublist)
            {
                if (ItemsSelected.Contains(item))
                {
                    text += "> ";
                }
                text += item.Name + "\n";
            }
            WriteDiagnosticText(text);
        }

        private static void WriteDiagnosticText(string text)
        {
            var position = new Vector3();
            position.X = Camera.Main.AbsoluteLeftXEdge;
            position.Y = Camera.Main.AbsoluteTopYEdge;

            var textInstance = EditorVisuals.Text(text, position);
            textInstance.HorizontalAlignment = FlatRedBall.Graphics.HorizontalAlignment.Left;
            textInstance.VerticalAlignment = FlatRedBall.Graphics.VerticalAlignment.Top;
        }

        #endregion

        #region Selection


        internal void Select(IEnumerable<NamedObjectSave> namedObjects, bool addToExistingSelection = false, bool playBump = true, bool focusCameraOnObject = false)
        {
            var isFirst = true;
            if (namedObjects.Count() == 0)
            {
                Select((NamedObjectSave)null, false, playBump, focusCameraOnObject, false);
            }
            else
            {
                foreach (var item in namedObjects)
                {
                    Select(item, addToExistingSelection || !isFirst, playBump, focusCameraOnObject, false);
                    isFirst = false;
                }
            }

            AddAndDestroyMarkersAccordingToItemsSelected();

            UpdateMarkers(didChangeItemOver: true);
        }

        internal void Select(NamedObjectSave namedObject, bool addToExistingSelection = false, bool playBump = true, bool focusCameraOnObject = false, bool updateMarkers = true, PositionedObject[] availablePositionedObjectCache = null)
        {
            if (addToExistingSelection == false)
            {
                CurrentNamedObjects.Clear();
            }

            bool isSelectable = true;
            if (namedObject != null)
            {
                INameable foundObject = GetObjectByName(namedObject?.InstanceName, availablePositionedObjectCache);
                isSelectable = foundObject != null &&
                    SelectionLogic.IsSelectable(foundObject);
            }

            if (isSelectable)
            {
                if (namedObject != null && !CurrentNamedObjects.Contains(namedObject))
                {
                    CurrentNamedObjects.Add(namedObject);
                }

                Select(namedObject?.InstanceName, addToExistingSelection, playBump, focusCameraOnObject, updateMarkers, availablePositionedObjectCache);
            }
            else
            {
                Select((string)null, addToExistingSelection, playBump, focusCameraOnObject, updateMarkers);
            }
        }

        internal void Select(string objectName, bool addToExistingSelection = false, bool playBump = true, bool focusCameraOnObject = false, bool updateMarkers = true, PositionedObject[] availablePositionedObjectCache = null)
        {
            INameable foundObject = string.IsNullOrEmpty(objectName)
                ? null
                : GetObjectByName(objectName, availablePositionedObjectCache);

            //if (!string.IsNullOrEmpty(objectName))
            //{
            //    foundObject = SelectionLogic.GetAvailableObjects(ElementEditingMode)
            //        ?.FirstOrDefault(item => item.Name == objectName);


            //    if (foundObject == null)
            //    {
            //        var screen = ScreenManager.CurrentScreen;
            //        var instance = screen.GetInstance($"{objectName}", screen);

            //        foundObject = instance as INameable;
            //    }
            //}


            if (!addToExistingSelection)
            {
                itemsSelected.Clear();
            }

            if (itemsSelected.Contains(foundObject) == false)
            {
                if (foundObject != null)
                {
                    itemsSelected.Add(foundObject);
                }

                if (updateMarkers)
                {
                    AddAndDestroyMarkersAccordingToItemsSelected();
                }

                if (playBump)
                {
                    MarkerFor(foundObject)?.PlayBumpAnimation(SelectedItemExtraPadding, isSynchronized: false);
                }

                if (updateMarkers)
                {
                    // do this right away so the handles don't pop out of existance when changing selection
                    UpdateMarkers(didChangeItemOver: true);
                }

            }

            if (focusCameraOnObject && foundObject is PositionedObject positionedObject)
            {
                Camera.Main.X = positionedObject.X;
                Camera.Main.Y = positionedObject.Y;
            }
        }


        public INameable GetObjectByName(string objectName, PositionedObject[] availablePositionedObjectCache = null)
        {
            INameable foundObject = null;
            object foundObjectAsObject = null;
            if (!string.IsNullOrEmpty(objectName))
            {
                if(availablePositionedObjectCache != null)
                {
                    for(int i = 0; i < availablePositionedObjectCache.Length; i++)
                    {
                        if (availablePositionedObjectCache[i].Name ==  objectName)
                        {
                            foundObject = availablePositionedObjectCache[i];
                            break;
                        }
                    }
                }
                else
                {
                    var allAvailableObjects = SelectionLogic.GetAvailableObjects(ElementEditingMode);
                    foundObject = allAvailableObjects?.FirstOrDefault(item => item.Name == objectName);
                }
                if (foundObject == null)
                {
                    var screen = ScreenManager.CurrentScreen;
                    var instance = screen.GetInstance($"{objectName}", screen);

                    foundObject = instance as INameable;
                }



                if (foundObject == null && ScreenManager.CurrentScreen is Screens.EntityViewingScreen entityViewingScreen)
                {
                    try
                    {
                        foundObjectAsObject = FlatRedBall.Instructions.Reflection.LateBinder.GetValueStatic(
                            entityViewingScreen.CurrentEntity, objectName);

                        if (foundObjectAsObject != null)
                        {
                            foundObject = foundObjectAsObject as INameable ?? new NameableWrapper { Name = objectName, ContainedObject = foundObjectAsObject };
                        }
                    }
                    catch
                    {

                    }
                }
            }

            if (foundObject == null)
            {
                var message = $"Tried to get object by name {objectName} but couldn't find anything";
                // This object may not exist. Should we tell Glue? I guess...
                if (foundObjectAsObject != null)
                {
                    message += "\n...but was able to find an object of type {}, but it isn't an INameable so it can't be used in the editor.";
                }
                Managers.GlueCommands.Self.PrintOutput(message);
            }

            return foundObject;
        }

        void RemoveFromSelection(NamedObjectSave namedObject)
        {
            CurrentNamedObjects.Remove(namedObject);

            var foundObject = SelectionLogic.GetAvailableObjects(ElementEditingMode)
                ?.FirstOrDefault(item => item.Name == namedObject.InstanceName);

            if (foundObject != null)
            {
                itemsSelected.Remove(foundObject);

                AddAndDestroyMarkersAccordingToItemsSelected();

            }
        }

        public void RefreshSelectionAfterScreenLoad(bool playBump)
        {
            var names = itemsSelected.Select(item => item.Name).ToArray();

            itemsSelected.Clear();

            if (names.Length > 0)
            {
                var allNamedObjectSaves = CurrentGlueElement?.AllNamedObjects.ToArray();
                if (allNamedObjectSaves != null)
                {
                    foreach (var name in names)
                    {
                        var matchingNos = allNamedObjectSaves.FirstOrDefault(item => item.InstanceName == name);
                        Select(matchingNos, addToExistingSelection: true, playBump);
                    }
                }
            }
        }

        public void RaiseObjectSelected()
        {
            if (ObjectSelected != null && ItemsSelected.Count() > 0)
            {
                ObjectSelected(ItemsSelected.ToList());
            }
        }

        #endregion

        #region Variable Assignment (suppression)

        static HashSet<string> VariableAssignmentsToIgnore = new HashSet<string>
        {
            nameof(NamedObjectSave.IsEditingLocked), // this gets copied over by the NOS
            "PartitioningAutomaticManual" // We can't support this in realtime because it's done by codegen and would be hard to change...
        };

        public bool GetIfShouldSuppressVariableAssignment(string variableName, INameable targetInstance)
        {
            var isAnythingGrabbed = itemGrabbed != null;

            ISelectionMarker markerToAsk = null;

            if (isAnythingGrabbed && ItemsSelected.Contains(targetInstance))
            {
                var index = itemsSelected.IndexOf(targetInstance);

                if (index > -1 && index < SelectedMarkers.Count)
                {
                    markerToAsk = SelectedMarkers[index];
                }
            }

            var shouldSuppress = markerToAsk?.ShouldSuppress(variableName) == true;

            if (!shouldSuppress)
            {
                if (VariableAssignmentsToIgnore.Contains(variableName))
                {
                    shouldSuppress = true;
                }
            }

            return shouldSuppress;
        }

        #endregion
    }

}