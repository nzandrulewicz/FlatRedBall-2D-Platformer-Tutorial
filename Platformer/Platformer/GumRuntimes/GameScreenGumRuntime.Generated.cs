using System.Linq;
namespace Platformer.GumRuntimes
{
    public partial class GameScreenGumRuntime : Gum.Wireframe.GraphicalUiElement
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
                        ScoreInstance.HorizontalAlignment = RenderingLibrary.Graphics.HorizontalAlignment.Right;
                        ScoreInstance.Text = "00000";
                        ScoreInstance.Width = 52f;
                        ScoreInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
                        ScoreInstance.X = 32f;
                        ScoreInstance.Y = 32f;
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
            bool setScoreInstanceWidthFirstValue = false;
            bool setScoreInstanceWidthSecondValue = false;
            float ScoreInstanceWidthFirstValue= 0;
            float ScoreInstanceWidthSecondValue= 0;
            bool setScoreInstanceXFirstValue = false;
            bool setScoreInstanceXSecondValue = false;
            float ScoreInstanceXFirstValue= 0;
            float ScoreInstanceXSecondValue= 0;
            bool setScoreInstanceYFirstValue = false;
            bool setScoreInstanceYSecondValue = false;
            float ScoreInstanceYFirstValue= 0;
            float ScoreInstanceYSecondValue= 0;
            switch(firstState)
            {
                case  VariableState.Default:
                    if (interpolationValue < 1)
                    {
                        this.ScoreInstance.HorizontalAlignment = RenderingLibrary.Graphics.HorizontalAlignment.Right;
                    }
                    if (interpolationValue < 1)
                    {
                        this.ScoreInstance.Text = "00000";
                    }
                    setScoreInstanceWidthFirstValue = true;
                    ScoreInstanceWidthFirstValue = 52f;
                    if (interpolationValue < 1)
                    {
                        this.ScoreInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
                    }
                    setScoreInstanceXFirstValue = true;
                    ScoreInstanceXFirstValue = 32f;
                    setScoreInstanceYFirstValue = true;
                    ScoreInstanceYFirstValue = 32f;
                    break;
            }
            switch(secondState)
            {
                case  VariableState.Default:
                    if (interpolationValue >= 1)
                    {
                        this.ScoreInstance.HorizontalAlignment = RenderingLibrary.Graphics.HorizontalAlignment.Right;
                    }
                    if (interpolationValue >= 1)
                    {
                        this.ScoreInstance.Text = "00000";
                    }
                    setScoreInstanceWidthSecondValue = true;
                    ScoreInstanceWidthSecondValue = 52f;
                    if (interpolationValue >= 1)
                    {
                        this.ScoreInstance.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
                    }
                    setScoreInstanceXSecondValue = true;
                    ScoreInstanceXSecondValue = 32f;
                    setScoreInstanceYSecondValue = true;
                    ScoreInstanceYSecondValue = 32f;
                    break;
            }
            var wasSuppressed = mIsLayoutSuspended;
            var shouldSuspend = wasSuppressed == false;
            var suspendRecursively = true;
            if (shouldSuspend)
            {
                SuspendLayout(suspendRecursively);
            }
            if (setScoreInstanceWidthFirstValue && setScoreInstanceWidthSecondValue)
            {
                ScoreInstance.Width = ScoreInstanceWidthFirstValue * (1 - interpolationValue) + ScoreInstanceWidthSecondValue * interpolationValue;
            }
            if (setScoreInstanceXFirstValue && setScoreInstanceXSecondValue)
            {
                ScoreInstance.X = ScoreInstanceXFirstValue * (1 - interpolationValue) + ScoreInstanceXSecondValue * interpolationValue;
            }
            if (setScoreInstanceYFirstValue && setScoreInstanceYSecondValue)
            {
                ScoreInstance.Y = ScoreInstanceYFirstValue * (1 - interpolationValue) + ScoreInstanceYSecondValue * interpolationValue;
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
        public FlatRedBall.Glue.StateInterpolation.Tweener InterpolateTo (Platformer.GumRuntimes.GameScreenGumRuntime.VariableState fromState,Platformer.GumRuntimes.GameScreenGumRuntime.VariableState toState, double secondsToTake, FlatRedBall.Glue.StateInterpolation.InterpolationType interpolationType, FlatRedBall.Glue.StateInterpolation.Easing easing, object owner = null) 
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
                        Name = "ScoreInstance.HorizontalAlignment",
                        Type = "HorizontalAlignment",
                        Value = ScoreInstance.HorizontalAlignment
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Text",
                        Type = "string",
                        Value = ScoreInstance.Text
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Width",
                        Type = "float",
                        Value = ScoreInstance.Width
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Width Units",
                        Type = "DimensionUnitType",
                        Value = ScoreInstance.WidthUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.X",
                        Type = "float",
                        Value = ScoreInstance.X
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Y",
                        Type = "float",
                        Value = ScoreInstance.Y
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
                        Name = "ScoreInstance.HorizontalAlignment",
                        Type = "HorizontalAlignment",
                        Value = ScoreInstance.HorizontalAlignment
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Text",
                        Type = "string",
                        Value = ScoreInstance.Text
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Width",
                        Type = "float",
                        Value = ScoreInstance.Width + 52f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Width Units",
                        Type = "DimensionUnitType",
                        Value = ScoreInstance.WidthUnits
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.X",
                        Type = "float",
                        Value = ScoreInstance.X + 32f
                    }
                    );
                    newState.Variables.Add(new Gum.DataTypes.Variables.VariableSave()
                    {
                        SetsValue = true,
                        Name = "ScoreInstance.Y",
                        Type = "float",
                        Value = ScoreInstance.Y + 32f
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
        public Platformer.GumRuntimes.TextRuntime ScoreInstance { get; set; }
        public GameScreenGumRuntime () 
        	: this(true, true)
        {
        }
        public GameScreenGumRuntime (bool fullInstantiation = true, bool tryCreateFormsObject = true) 
        {
            this.tryCreateFormsObject = tryCreateFormsObject;
            if (fullInstantiation)
            {
                Gum.DataTypes.ElementSave elementSave = Gum.Managers.ObjectFinder.Self.GumProjectSave.Screens.First(item => item.Name == "GameScreenGum");
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
            ScoreInstance = this.GetGraphicalUiElementByName("ScoreInstance") as Platformer.GumRuntimes.TextRuntime;
            if (tryCreateFormsObject)
            {
                FormsControlAsObject = new Platformer.FormsControls.Screens.GameScreenGumForms(this);
            }
        }
        private void CallCustomInitialize () 
        {
            CustomInitialize();
        }
        partial void CustomInitialize();
        public Platformer.FormsControls.Screens.GameScreenGumForms FormsControl {get => (Platformer.FormsControls.Screens.GameScreenGumForms) FormsControlAsObject;}
    }
}
