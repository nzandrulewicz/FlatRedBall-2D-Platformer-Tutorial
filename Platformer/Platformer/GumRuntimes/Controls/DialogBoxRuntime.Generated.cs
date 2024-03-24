using System.Linq;
namespace Platformer.GumRuntimes.Controls
{
    public partial class DialogBoxRuntime : Platformer.GumRuntimes.ContainerRuntime, Platformer.GumRuntimes.IDialogBoxBehavior
    {
        #region State Enums
        public enum VariableState
        {
            Default
        }
        #endregion
        #region State Fields
        VariableState mCurrentVariableState;
        #endregion
        #region State Properties
        public VariableState CurrentVariableState
        {
            get
            {
                return mCurrentVariableState;
            }
            set
            {
                mCurrentVariableState = value;
                switch(mCurrentVariableState)
                {
                    case  VariableState.Default:
                        NineSliceInstance.CurrentColorCategoryState = Platformer.GumRuntimes.NineSliceRuntime.ColorCategory.Primary;
                        NineSliceInstance.CurrentStyleCategoryState = Platformer.GumRuntimes.NineSliceRuntime.StyleCategory.Panel;
                        TextInstance.CurrentColorCategoryState = Platformer.GumRuntimes.TextRuntime.ColorCategory.White;
                        TextInstance.CurrentStyleCategoryState = Platformer.GumRuntimes.TextRuntime.StyleCategory.Normal;
                        ContinueIndicatorInstance.CurrentIconCategoryState = Platformer.GumRuntimes.Elements.IconRuntime.IconCategory.Arrow2;
                        ExposeChildrenEvents = false;
                        Height = 128f;
                        Width = 256f;
                        TextInstance.Height = 0f;
                        TextInstance.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
                        TextInstance.Text = "This is a dialog box. This text will be displayed one character at a time. Typically a dialog box is added to a Screen such as the GameScreen, but it defaults to being invisible.";
                        TextInstance.Width = -16f;
                        TextInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                        TextInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center;
                        TextInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle;
                        TextInstance.Y = 8f;
                        ContinueIndicatorInstance.Height = 24f;
                        ContinueIndicatorInstance.Rotation = -90f;
                        ContinueIndicatorInstance.Width = 24f;
                        ContinueIndicatorInstance.X = -8f;
                        ContinueIndicatorInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Right;
                        ContinueIndicatorInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromLarge;
                        ContinueIndicatorInstance.Y = -8f;
                        ContinueIndicatorInstance.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                        ContinueIndicatorInstance.YUnits = Gum.Converters.GeneralUnitType.PixelsFromLarge;
                        break;
                }
            }
        }
        #endregion
        #region State Interpolation
        public void InterpolateBetween (VariableState firstState, VariableState secondState, float interpolationValue) 
        {
            #if DEBUG
            if (float.IsNaN(interpolationValue))
            {
                throw new System.Exception("interpolationValue cannot be NaN");
            }
            #endif
            bool setContinueIndicatorInstanceHeightFirstValue = false;
            bool setContinueIndicatorInstanceHeightSecondValue = false;
            float ContinueIndicatorInstanceHeightFirstValue= 0;
            float ContinueIndicatorInstanceHeightSecondValue= 0;
            bool setContinueIndicatorInstanceCurrentIconCategoryStateFirstValue = false;
            bool setContinueIndicatorInstanceCurrentIconCategoryStateSecondValue = false;
            Platformer.GumRuntimes.Elements.IconRuntime.IconCategory ContinueIndicatorInstanceCurrentIconCategoryStateFirstValue= Platformer.GumRuntimes.Elements.IconRuntime.IconCategory.None;
            Platformer.GumRuntimes.Elements.IconRuntime.IconCategory ContinueIndicatorInstanceCurrentIconCategoryStateSecondValue= Platformer.GumRuntimes.Elements.IconRuntime.IconCategory.None;
            bool setContinueIndicatorInstanceRotationFirstValue = false;
            bool setContinueIndicatorInstanceRotationSecondValue = false;
            float ContinueIndicatorInstanceRotationFirstValue= 0;
            float ContinueIndicatorInstanceRotationSecondValue= 0;
            bool setContinueIndicatorInstanceWidthFirstValue = false;
            bool setContinueIndicatorInstanceWidthSecondValue = false;
            float ContinueIndicatorInstanceWidthFirstValue= 0;
            float ContinueIndicatorInstanceWidthSecondValue= 0;
            bool setContinueIndicatorInstanceXFirstValue = false;
            bool setContinueIndicatorInstanceXSecondValue = false;
            float ContinueIndicatorInstanceXFirstValue= 0;
            float ContinueIndicatorInstanceXSecondValue= 0;
            bool setContinueIndicatorInstanceYFirstValue = false;
            bool setContinueIndicatorInstanceYSecondValue = false;
            float ContinueIndicatorInstanceYFirstValue= 0;
            float ContinueIndicatorInstanceYSecondValue= 0;
            bool setHeightFirstValue = false;
            bool setHeightSecondValue = false;
            float HeightFirstValue= 0;
            float HeightSecondValue= 0;
            bool setNineSliceInstanceCurrentColorCategoryStateFirstValue = false;
            bool setNineSliceInstanceCurrentColorCategoryStateSecondValue = false;
            Platformer.GumRuntimes.NineSliceRuntime.ColorCategory NineSliceInstanceCurrentColorCategoryStateFirstValue= Platformer.GumRuntimes.NineSliceRuntime.ColorCategory.Black;
            Platformer.GumRuntimes.NineSliceRuntime.ColorCategory NineSliceInstanceCurrentColorCategoryStateSecondValue= Platformer.GumRuntimes.NineSliceRuntime.ColorCategory.Black;
            bool setNineSliceInstanceCurrentStyleCategoryStateFirstValue = false;
            bool setNineSliceInstanceCurrentStyleCategoryStateSecondValue = false;
            Platformer.GumRuntimes.NineSliceRuntime.StyleCategory NineSliceInstanceCurrentStyleCategoryStateFirstValue= Platformer.GumRuntimes.NineSliceRuntime.StyleCategory.Solid;
            Platformer.GumRuntimes.NineSliceRuntime.StyleCategory NineSliceInstanceCurrentStyleCategoryStateSecondValue= Platformer.GumRuntimes.NineSliceRuntime.StyleCategory.Solid;
            bool setTextInstanceCurrentColorCategoryStateFirstValue = false;
            bool setTextInstanceCurrentColorCategoryStateSecondValue = false;
            Platformer.GumRuntimes.TextRuntime.ColorCategory TextInstanceCurrentColorCategoryStateFirstValue= Platformer.GumRuntimes.TextRuntime.ColorCategory.Black;
            Platformer.GumRuntimes.TextRuntime.ColorCategory TextInstanceCurrentColorCategoryStateSecondValue= Platformer.GumRuntimes.TextRuntime.ColorCategory.Black;
            bool setTextInstanceHeightFirstValue = false;
            bool setTextInstanceHeightSecondValue = false;
            float TextInstanceHeightFirstValue= 0;
            float TextInstanceHeightSecondValue= 0;
            bool setTextInstanceCurrentStyleCategoryStateFirstValue = false;
            bool setTextInstanceCurrentStyleCategoryStateSecondValue = false;
            Platformer.GumRuntimes.TextRuntime.StyleCategory TextInstanceCurrentStyleCategoryStateFirstValue= Platformer.GumRuntimes.TextRuntime.StyleCategory.Tiny;
            Platformer.GumRuntimes.TextRuntime.StyleCategory TextInstanceCurrentStyleCategoryStateSecondValue= Platformer.GumRuntimes.TextRuntime.StyleCategory.Tiny;
            bool setTextInstanceWidthFirstValue = false;
            bool setTextInstanceWidthSecondValue = false;
            float TextInstanceWidthFirstValue= 0;
            float TextInstanceWidthSecondValue= 0;
            bool setTextInstanceYFirstValue = false;
            bool setTextInstanceYSecondValue = false;
            float TextInstanceYFirstValue= 0;
            float TextInstanceYSecondValue= 0;
            bool setWidthFirstValue = false;
            bool setWidthSecondValue = false;
            float WidthFirstValue= 0;
            float WidthSecondValue= 0;
            switch(firstState)
            {
                case  VariableState.Default:
                    setContinueIndicatorInstanceHeightFirstValue = true;
                    ContinueIndicatorInstanceHeightFirstValue = 24f;
                    setContinueIndicatorInstanceCurrentIconCategoryStateFirstValue = true;
                    ContinueIndicatorInstanceCurrentIconCategoryStateFirstValue = Platformer.GumRuntimes.Elements.IconRuntime.IconCategory.Arrow2;
                    setContinueIndicatorInstanceRotationFirstValue = true;
                    ContinueIndicatorInstanceRotationFirstValue = -90f;
                    setContinueIndicatorInstanceWidthFirstValue = true;
                    ContinueIndicatorInstanceWidthFirstValue = 24f;
                    setContinueIndicatorInstanceXFirstValue = true;
                    ContinueIndicatorInstanceXFirstValue = -8f;
                    if (interpolationValue < 1)
                    {
                        this.ContinueIndicatorInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Right;
                    }
                    if (interpolationValue < 1)
                    {
                        this.ContinueIndicatorInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromLarge;
                    }
                    setContinueIndicatorInstanceYFirstValue = true;
                    ContinueIndicatorInstanceYFirstValue = -8f;
                    if (interpolationValue < 1)
                    {
                        this.ContinueIndicatorInstance.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                    }
                    if (interpolationValue < 1)
                    {
                        this.ContinueIndicatorInstance.YUnits = Gum.Converters.GeneralUnitType.PixelsFromLarge;
                    }
                    if (interpolationValue < 1)
                    {
                        this.ExposeChildrenEvents = false;
                    }
                    setHeightFirstValue = true;
                    HeightFirstValue = 128f;
                    setNineSliceInstanceCurrentColorCategoryStateFirstValue = true;
                    NineSliceInstanceCurrentColorCategoryStateFirstValue = Platformer.GumRuntimes.NineSliceRuntime.ColorCategory.Primary;
                    setNineSliceInstanceCurrentStyleCategoryStateFirstValue = true;
                    NineSliceInstanceCurrentStyleCategoryStateFirstValue = Platformer.GumRuntimes.NineSliceRuntime.StyleCategory.Panel;
                    setTextInstanceCurrentColorCategoryStateFirstValue = true;
                    TextInstanceCurrentColorCategoryStateFirstValue = Platformer.GumRuntimes.TextRuntime.ColorCategory.White;
                    setTextInstanceHeightFirstValue = true;
                    TextInstanceHeightFirstValue = 0f;
                    if (interpolationValue < 1)
                    {
                        this.TextInstance.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
                    }
                    setTextInstanceCurrentStyleCategoryStateFirstValue = true;
                    TextInstanceCurrentStyleCategoryStateFirstValue = Platformer.GumRuntimes.TextRuntime.StyleCategory.Normal;
                    if (interpolationValue < 1)
                    {
                        this.TextInstance.Text = "This is a dialog box. This text will be displayed one character at a time. Typically a dialog box is added to a Screen such as the GameScreen, but it defaults to being invisible.";
                    }
                    setTextInstanceWidthFirstValue = true;
                    TextInstanceWidthFirstValue = -16f;
                    if (interpolationValue < 1)
                    {
                        this.TextInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                    }
                    if (interpolationValue < 1)
                    {
                        this.TextInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center;
                    }
                    if (interpolationValue < 1)
                    {
                        this.TextInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle;
                    }
                    setTextInstanceYFirstValue = true;
                    TextInstanceYFirstValue = 8f;
                    setWidthFirstValue = true;
                    WidthFirstValue = 256f;
                    break;
            }
            switch(secondState)
            {
                case  VariableState.Default:
                    setContinueIndicatorInstanceHeightSecondValue = true;
                    ContinueIndicatorInstanceHeightSecondValue = 24f;
                    setContinueIndicatorInstanceCurrentIconCategoryStateSecondValue = true;
                    ContinueIndicatorInstanceCurrentIconCategoryStateSecondValue = Platformer.GumRuntimes.Elements.IconRuntime.IconCategory.Arrow2;
                    setContinueIndicatorInstanceRotationSecondValue = true;
                    ContinueIndicatorInstanceRotationSecondValue = -90f;
                    setContinueIndicatorInstanceWidthSecondValue = true;
                    ContinueIndicatorInstanceWidthSecondValue = 24f;
                    setContinueIndicatorInstanceXSecondValue = true;
                    ContinueIndicatorInstanceXSecondValue = -8f;
                    if (interpolationValue >= 1)
                    {
                        this.ContinueIndicatorInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Right;
                    }
                    if (interpolationValue >= 1)
                    {
                        this.ContinueIndicatorInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromLarge;
                    }
                    setContinueIndicatorInstanceYSecondValue = true;
                    ContinueIndicatorInstanceYSecondValue = -8f;
                    if (interpolationValue >= 1)
                    {
                        this.ContinueIndicatorInstance.YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Top;
                    }
                    if (interpolationValue >= 1)
                    {
                        this.ContinueIndicatorInstance.YUnits = Gum.Converters.GeneralUnitType.PixelsFromLarge;
                    }
                    if (interpolationValue >= 1)
                    {
                        this.ExposeChildrenEvents = false;
                    }
                    setHeightSecondValue = true;
                    HeightSecondValue = 128f;
                    setNineSliceInstanceCurrentColorCategoryStateSecondValue = true;
                    NineSliceInstanceCurrentColorCategoryStateSecondValue = Platformer.GumRuntimes.NineSliceRuntime.ColorCategory.Primary;
                    setNineSliceInstanceCurrentStyleCategoryStateSecondValue = true;
                    NineSliceInstanceCurrentStyleCategoryStateSecondValue = Platformer.GumRuntimes.NineSliceRuntime.StyleCategory.Panel;
                    setTextInstanceCurrentColorCategoryStateSecondValue = true;
                    TextInstanceCurrentColorCategoryStateSecondValue = Platformer.GumRuntimes.TextRuntime.ColorCategory.White;
                    setTextInstanceHeightSecondValue = true;
                    TextInstanceHeightSecondValue = 0f;
                    if (interpolationValue >= 1)
                    {
                        this.TextInstance.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
                    }
                    setTextInstanceCurrentStyleCategoryStateSecondValue = true;
                    TextInstanceCurrentStyleCategoryStateSecondValue = Platformer.GumRuntimes.TextRuntime.StyleCategory.Normal;
                    if (interpolationValue >= 1)
                    {
                        this.TextInstance.Text = "This is a dialog box. This text will be displayed one character at a time. Typically a dialog box is added to a Screen such as the GameScreen, but it defaults to being invisible.";
                    }
                    setTextInstanceWidthSecondValue = true;
                    TextInstanceWidthSecondValue = -16f;
                    if (interpolationValue >= 1)
                    {
                        this.TextInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                    }
                    if (interpolationValue >= 1)
                    {
                        this.TextInstance.XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center;
                    }
                    if (interpolationValue >= 1)
                    {
                        this.TextInstance.XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle;
                    }
                    setTextInstanceYSecondValue = true;
                    TextInstanceYSecondValue = 8f;
                    setWidthSecondValue = true;
                    WidthSecondValue = 256f;
                    break;
            }
            var wasSuppressed = mIsLayoutSuspended;
            var shouldSuspend = wasSuppressed == false;
            var suspendRecursively = true;
            if (shouldSuspend)
            {
                SuspendLayout(suspendRecursively);
            }
            if (setContinueIndicatorInstanceHeightFirstValue && setContinueIndicatorInstanceHeightSecondValue)
            {
                ContinueIndicatorInstance.Height = ContinueIndicatorInstanceHeightFirstValue * (1 - interpolationValue) + ContinueIndicatorInstanceHeightSecondValue * interpolationValue;
            }
            if (setContinueIndicatorInstanceCurrentIconCategoryStateFirstValue && setContinueIndicatorInstanceCurrentIconCategoryStateSecondValue)
            {
                ContinueIndicatorInstance.InterpolateBetween(ContinueIndicatorInstanceCurrentIconCategoryStateFirstValue, ContinueIndicatorInstanceCurrentIconCategoryStateSecondValue, interpolationValue);
            }
            if (setContinueIndicatorInstanceRotationFirstValue && setContinueIndicatorInstanceRotationSecondValue)
            {
                ContinueIndicatorInstance.Rotation = ContinueIndicatorInstanceRotationFirstValue * (1 - interpolationValue) + ContinueIndicatorInstanceRotationSecondValue * interpolationValue;
            }
            if (setContinueIndicatorInstanceWidthFirstValue && setContinueIndicatorInstanceWidthSecondValue)
            {
                ContinueIndicatorInstance.Width = ContinueIndicatorInstanceWidthFirstValue * (1 - interpolationValue) + ContinueIndicatorInstanceWidthSecondValue * interpolationValue;
            }
            if (setContinueIndicatorInstanceXFirstValue && setContinueIndicatorInstanceXSecondValue)
            {
                ContinueIndicatorInstance.X = ContinueIndicatorInstanceXFirstValue * (1 - interpolationValue) + ContinueIndicatorInstanceXSecondValue * interpolationValue;
            }
            if (setContinueIndicatorInstanceYFirstValue && setContinueIndicatorInstanceYSecondValue)
            {
                ContinueIndicatorInstance.Y = ContinueIndicatorInstanceYFirstValue * (1 - interpolationValue) + ContinueIndicatorInstanceYSecondValue * interpolationValue;
            }
            if (setHeightFirstValue && setHeightSecondValue)
            {
                Height = HeightFirstValue * (1 - interpolationValue) + HeightSecondValue * interpolationValue;
            }
            if (setNineSliceInstanceCurrentColorCategoryStateFirstValue && setNineSliceInstanceCurrentColorCategoryStateSecondValue)
            {
                NineSliceInstance.InterpolateBetween(NineSliceInstanceCurrentColorCategoryStateFirstValue, NineSliceInstanceCurrentColorCategoryStateSecondValue, interpolationValue);
            }
            if (setNineSliceInstanceCurrentStyleCategoryStateFirstValue && setNineSliceInstanceCurrentStyleCategoryStateSecondValue)
            {
                NineSliceInstance.InterpolateBetween(NineSliceInstanceCurrentStyleCategoryStateFirstValue, NineSliceInstanceCurrentStyleCategoryStateSecondValue, interpolationValue);
            }
            if (setTextInstanceCurrentColorCategoryStateFirstValue && setTextInstanceCurrentColorCategoryStateSecondValue)
            {
                TextInstance.InterpolateBetween(TextInstanceCurrentColorCategoryStateFirstValue, TextInstanceCurrentColorCategoryStateSecondValue, interpolationValue);
            }
            if (setTextInstanceHeightFirstValue && setTextInstanceHeightSecondValue)
            {
                TextInstance.Height = TextInstanceHeightFirstValue * (1 - interpolationValue) + TextInstanceHeightSecondValue * interpolationValue;
            }
            if (setTextInstanceCurrentStyleCategoryStateFirstValue && setTextInstanceCurrentStyleCategoryStateSecondValue)
            {
                TextInstance.InterpolateBetween(TextInstanceCurrentStyleCategoryStateFirstValue, TextInstanceCurrentStyleCategoryStateSecondValue, interpolationValue);
            }
            if (setTextInstanceWidthFirstValue && setTextInstanceWidthSecondValue)
            {
                TextInstance.Width = TextInstanceWidthFirstValue * (1 - interpolationValue) + TextInstanceWidthSecondValue * interpolationValue;
            }
            if (setTextInstanceYFirstValue && setTextInstanceYSecondValue)
            {
                TextInstance.Y = TextInstanceYFirstValue * (1 - interpolationValue) + TextInstanceYSecondValue * interpolationValue;
            }
            if (setWidthFirstValue && setWidthSecondValue)
            {
                Width = WidthFirstValue * (1 - interpolationValue) + WidthSecondValue * interpolationValue;
            }
            if (interpolationValue < 1)
            {
                mCurrentVariableState = firstState;
            }
            else
            {
                mCurrentVariableState = secondState;
            }
            if (shouldSuspend)
            {
                ResumeLayout(suspendRecursively);
            }
        }
        #endregion
        #region State Interpolate To
        public FlatRedBall.Glue.StateInterpolation.Tweener InterpolateTo (Platformer.GumRuntimes.Controls.DialogBoxRuntime.VariableState fromState,Platformer.GumRuntimes.Controls.DialogBoxRuntime.VariableState toState, double secondsToTake, FlatRedBall.Glue.StateInterpolation.InterpolationType interpolationType, FlatRedBall.Glue.StateInterpolation.Easing easing, object owner = null) 
        {
            FlatRedBall.Glue.StateInterpolation.Tweener tweener = new FlatRedBall.Glue.StateInterpolation.Tweener(from:0, to:1, duration:(float)secondsToTake, type:interpolationType, easing:easing );
            if (owner == null)
            {
                tweener.Owner = this;
            }
            else
            {
                tweener.Owner = owner;
            }
            tweener.PositionChanged = newPosition => this.InterpolateBetween(fromState, toState, newPosition);
            tweener.Start();
            StateInterpolationPlugin.TweenerManager.Self.Add(tweener);
            return tweener;
        }
        public FlatRedBall.Glue.StateInterpolation.Tweener InterpolateTo (VariableState toState, double secondsToTake, FlatRedBall.Glue.StateInterpolation.InterpolationType interpolationType, FlatRedBall.Glue.StateInterpolation.Easing easing, object owner = null ) 
        {
            Gum.DataTypes.Variables.StateSave current = GetCurrentValuesOnState(toState);
            Gum.DataTypes.Variables.StateSave toAsStateSave = this.ElementSave.States.First(item => item.Name == toState.ToString());
            FlatRedBall.Glue.StateInterpolation.Tweener tweener = new FlatRedBall.Glue.StateInterpolation.Tweener(from: 0, to: 1, duration: (float)secondsToTake, type: interpolationType, easing: easing);
            if (owner == null)
            {
                tweener.Owner = this;
            }
            else
            {
                tweener.Owner = owner;
            }
            tweener.PositionChanged = newPosition => this.InterpolateBetween(current, toAsStateSave, newPosition);
            tweener.Ended += ()=> this.CurrentVariableState = toState;
            tweener.Start();
            StateInterpolationPlugin.TweenerManager.Self.Add(tweener);
            return tweener;
        }
        public FlatRedBall.Glue.StateInterpolation.Tweener InterpolateToRelative (VariableState toState, double secondsToTake, FlatRedBall.Glue.StateInterpolation.InterpolationType interpolationType, FlatRedBall.Glue.StateInterpolation.Easing easing, object owner = null ) 
        {
            Gum.DataTypes.Variables.StateSave current = GetCurrentValuesOnState(toState);
            Gum.DataTypes.Variables.StateSave toAsStateSave = AddToCurrentValuesWithState(toState);
            FlatRedBall.Glue.StateInterpolation.Tweener tweener = new FlatRedBall.Glue.StateInterpolation.Tweener(from: 0, to: 1, duration: (float)secondsToTake, type: interpolationType, easing: easing);
            if (owner == null)
            {
                tweener.Owner = this;
            }
            else
            {
                tweener.Owner = owner;
            }
            tweener.PositionChanged = newPosition => this.InterpolateBetween(current, toAsStateSave, newPosition);
            tweener.Ended += ()=> this.CurrentVariableState = toState;
            tweener.Start();
            StateInterpolationPlugin.TweenerManager.Self.Add(tweener);
            return tweener;
        }
        #endregion
        #region State Animations
        #endregion
        public override void StopAnimations () 
        {
            base.StopAnimations();
            ContinueIndicatorInstance.StopAnimations();
        }
        public override FlatRedBall.Gum.Animation.GumAnimation GetAnimation (string animationName) 
        {
            return base.GetAnimation(animationName);
        }
        #region Get Current Values on State
        private Gum.DataTypes.Variables.StateSave GetCurrentValuesOnState (VariableState state) 
        {
            Gum.DataTypes.Variables.StateSave newState = new Gum.DataTypes.Variables.StateSave();
            switch(state)
            {
                case  VariableState.Default:
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ExposeChildrenEvents",
                        Type = "bool",
                        Value = ExposeChildrenEvents
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "Height",
                        Type = "float",
                        Value = Height
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "Width",
                        Type = "float",
                        Value = Width
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "NineSliceInstance.ColorCategoryState",
                        Type = "ColorCategory",
                        Value = NineSliceInstance.CurrentColorCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "NineSliceInstance.StyleCategoryState",
                        Type = "StyleCategory",
                        Value = NineSliceInstance.CurrentStyleCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.ColorCategoryState",
                        Type = "ColorCategory",
                        Value = TextInstance.CurrentColorCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Height",
                        Type = "float",
                        Value = TextInstance.Height
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Height Units",
                        Type = "DimensionUnitType",
                        Value = TextInstance.HeightUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.StyleCategoryState",
                        Type = "StyleCategory",
                        Value = TextInstance.CurrentStyleCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Text",
                        Type = "string",
                        Value = TextInstance.Text
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Width",
                        Type = "float",
                        Value = TextInstance.Width
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Width Units",
                        Type = "DimensionUnitType",
                        Value = TextInstance.WidthUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.X Origin",
                        Type = "HorizontalAlignment",
                        Value = TextInstance.XOrigin
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.X Units",
                        Type = "PositionUnitType",
                        Value = TextInstance.XUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Y",
                        Type = "float",
                        Value = TextInstance.Y
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Height",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Height
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.IconCategoryState",
                        Type = "IconCategory",
                        Value = ContinueIndicatorInstance.CurrentIconCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Rotation",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Rotation
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Width",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Width
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.X",
                        Type = "float",
                        Value = ContinueIndicatorInstance.X
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.X Origin",
                        Type = "HorizontalAlignment",
                        Value = ContinueIndicatorInstance.XOrigin
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.X Units",
                        Type = "PositionUnitType",
                        Value = ContinueIndicatorInstance.XUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Y",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Y
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Y Origin",
                        Type = "VerticalAlignment",
                        Value = ContinueIndicatorInstance.YOrigin
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Y Units",
                        Type = "PositionUnitType",
                        Value = ContinueIndicatorInstance.YUnits
                    }
                    );
                    break;
            }
            return newState;
        }
        private Gum.DataTypes.Variables.StateSave AddToCurrentValuesWithState (VariableState state) 
        {
            Gum.DataTypes.Variables.StateSave newState = new Gum.DataTypes.Variables.StateSave();
            switch(state)
            {
                case  VariableState.Default:
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ExposeChildrenEvents",
                        Type = "bool",
                        Value = ExposeChildrenEvents
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "Height",
                        Type = "float",
                        Value = Height + 128f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "Width",
                        Type = "float",
                        Value = Width + 256f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "NineSliceInstance.ColorCategoryState",
                        Type = "ColorCategory",
                        Value = NineSliceInstance.CurrentColorCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "NineSliceInstance.StyleCategoryState",
                        Type = "StyleCategory",
                        Value = NineSliceInstance.CurrentStyleCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.ColorCategoryState",
                        Type = "ColorCategory",
                        Value = TextInstance.CurrentColorCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Height",
                        Type = "float",
                        Value = TextInstance.Height + 0f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Height Units",
                        Type = "DimensionUnitType",
                        Value = TextInstance.HeightUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.StyleCategoryState",
                        Type = "StyleCategory",
                        Value = TextInstance.CurrentStyleCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Text",
                        Type = "string",
                        Value = TextInstance.Text
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Width",
                        Type = "float",
                        Value = TextInstance.Width + -16f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Width Units",
                        Type = "DimensionUnitType",
                        Value = TextInstance.WidthUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.X Origin",
                        Type = "HorizontalAlignment",
                        Value = TextInstance.XOrigin
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.X Units",
                        Type = "PositionUnitType",
                        Value = TextInstance.XUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "TextInstance.Y",
                        Type = "float",
                        Value = TextInstance.Y + 8f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Height",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Height + 24f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.IconCategoryState",
                        Type = "IconCategory",
                        Value = ContinueIndicatorInstance.CurrentIconCategoryState
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Rotation",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Rotation + -90f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Width",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Width + 24f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.X",
                        Type = "float",
                        Value = ContinueIndicatorInstance.X + -8f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.X Origin",
                        Type = "HorizontalAlignment",
                        Value = ContinueIndicatorInstance.XOrigin
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.X Units",
                        Type = "PositionUnitType",
                        Value = ContinueIndicatorInstance.XUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Y",
                        Type = "float",
                        Value = ContinueIndicatorInstance.Y + -8f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Y Origin",
                        Type = "VerticalAlignment",
                        Value = ContinueIndicatorInstance.YOrigin
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ContinueIndicatorInstance.Y Units",
                        Type = "PositionUnitType",
                        Value = ContinueIndicatorInstance.YUnits
                    }
                    );
                    break;
            }
            return newState;
        }
        #endregion
        public override void ApplyState (Gum.DataTypes.Variables.StateSave state) 
        {
            bool matches = this.ElementSave.AllStates.Contains(state);
            if (matches)
            {
                var category = this.ElementSave.Categories.FirstOrDefault(item => item.States.Contains(state));
                if (category == null)
                {
                    if (state.Name == "Default") this.mCurrentVariableState = VariableState.Default;
                }
            }
            base.ApplyState(state);
        }
        private bool tryCreateFormsObject;
        public Platformer.GumRuntimes.NineSliceRuntime NineSliceInstance { get; set; }
        public Platformer.GumRuntimes.TextRuntime TextInstance { get; set; }
        public Platformer.GumRuntimes.Elements.IconRuntime ContinueIndicatorInstance { get; set; }
        public DialogBoxRuntime () 
        	: this(true, true)
        {
        }
        public DialogBoxRuntime (bool fullInstantiation = true, bool tryCreateFormsObject = true) 
        	: base(false, tryCreateFormsObject)
        {
            this.tryCreateFormsObject = tryCreateFormsObject;
            this.ExposeChildrenEvents = false;
            if (fullInstantiation)
            {
                Gum.DataTypes.ElementSave elementSave = Gum.Managers.ObjectFinder.Self.GumProjectSave.Components.First(item => item.Name == "Controls/DialogBox");
                this.ElementSave = elementSave;
                string oldDirectory = FlatRedBall.IO.FileManager.RelativeDirectory;
                FlatRedBall.IO.FileManager.RelativeDirectory = FlatRedBall.IO.FileManager.GetDirectory(Gum.Managers.ObjectFinder.Self.GumProjectSave.FullFileName);
                GumRuntime.ElementSaveExtensions.SetGraphicalUiElement(elementSave, this, RenderingLibrary.SystemManagers.Default);
                FlatRedBall.IO.FileManager.RelativeDirectory = oldDirectory;
            }
        }
        public override void SetInitialState () 
        {
            var wasSuppressed = this.IsLayoutSuspended;
            if(!wasSuppressed) this.SuspendLayout();
            base.SetInitialState();
            this.CurrentVariableState = VariableState.Default;
            if(!wasSuppressed) this.ResumeLayout();
            CallCustomInitialize();
        }
        public override void CreateChildrenRecursively (Gum.DataTypes.ElementSave elementSave, RenderingLibrary.ISystemManagers systemManagers) 
        {
            base.CreateChildrenRecursively(elementSave, systemManagers);
            this.AssignInternalReferences();
        }
        private void AssignInternalReferences () 
        {
            NineSliceInstance = this.GetGraphicalUiElementByName("NineSliceInstance") as Platformer.GumRuntimes.NineSliceRuntime;
            TextInstance = this.GetGraphicalUiElementByName("TextInstance") as Platformer.GumRuntimes.TextRuntime;
            ContinueIndicatorInstance = this.GetGraphicalUiElementByName("ContinueIndicatorInstance") as Platformer.GumRuntimes.Elements.IconRuntime;
            if (tryCreateFormsObject)
            {
                FormsControlAsObject = new FlatRedBall.Forms.Controls.Games.DialogBox(this);
            }
        }
        private void CallCustomInitialize () 
        {
            CustomInitialize();
        }
        partial void CustomInitialize();
        public FlatRedBall.Forms.Controls.Games.DialogBox FormsControl {get => (FlatRedBall.Forms.Controls.Games.DialogBox) FormsControlAsObject;}
    }
}
