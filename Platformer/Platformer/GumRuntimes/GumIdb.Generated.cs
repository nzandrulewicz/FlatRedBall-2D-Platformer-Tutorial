namespace FlatRedBall.Gum
{
    public  class GumIdbExtensions
    {
        public static void RegisterTypes () 
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Circle", typeof(Platformer.GumRuntimes.CircleRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("ColoredRectangle", typeof(Platformer.GumRuntimes.ColoredRectangleRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Container", typeof(Platformer.GumRuntimes.ContainerRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Container<T>", typeof(Platformer.GumRuntimes.ContainerRuntime<>));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("NineSlice", typeof(Platformer.GumRuntimes.NineSliceRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Polygon", typeof(Platformer.GumRuntimes.PolygonRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Rectangle", typeof(Platformer.GumRuntimes.RectangleRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Sprite", typeof(Platformer.GumRuntimes.SpriteRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Text", typeof(Platformer.GumRuntimes.TextRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonClose", typeof(Platformer.GumRuntimes.Controls.ButtonCloseRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonConfirm", typeof(Platformer.GumRuntimes.Controls.ButtonConfirmRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonDeny", typeof(Platformer.GumRuntimes.Controls.ButtonDenyRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonIcon", typeof(Platformer.GumRuntimes.Controls.ButtonIconRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonStandard", typeof(Platformer.GumRuntimes.Controls.ButtonStandardRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonStandardIcon", typeof(Platformer.GumRuntimes.Controls.ButtonStandardIconRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ButtonTab", typeof(Platformer.GumRuntimes.Controls.ButtonTabRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/CheckBox", typeof(Platformer.GumRuntimes.Controls.CheckBoxRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ComboBox", typeof(Platformer.GumRuntimes.Controls.ComboBoxRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/DialogBox", typeof(Platformer.GumRuntimes.Controls.DialogBoxRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/Keyboard", typeof(Platformer.GumRuntimes.Controls.KeyboardRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/KeyboardKey", typeof(Platformer.GumRuntimes.Controls.KeyboardKeyRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ListBox", typeof(Platformer.GumRuntimes.Controls.ListBoxRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ListBoxItem", typeof(Platformer.GumRuntimes.Controls.ListBoxItemRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/PasswordBox", typeof(Platformer.GumRuntimes.Controls.PasswordBoxRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/PlayerJoinView", typeof(Platformer.GumRuntimes.Controls.PlayerJoinViewRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/PlayerJoinViewItem", typeof(Platformer.GumRuntimes.Controls.PlayerJoinViewItemRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/RadioButton", typeof(Platformer.GumRuntimes.Controls.RadioButtonRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ScrollBar", typeof(Platformer.GumRuntimes.Controls.ScrollBarRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/ScrollViewer", typeof(Platformer.GumRuntimes.Controls.ScrollViewerRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/SettingsView", typeof(Platformer.GumRuntimes.Controls.SettingsViewRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/Slider", typeof(Platformer.GumRuntimes.Controls.SliderRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/TextBox", typeof(Platformer.GumRuntimes.Controls.TextBoxRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/Toast", typeof(Platformer.GumRuntimes.Controls.ToastRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/TreeView", typeof(Platformer.GumRuntimes.Controls.TreeViewRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/TreeViewItem", typeof(Platformer.GumRuntimes.Controls.TreeViewItemRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/TreeViewToggle", typeof(Platformer.GumRuntimes.Controls.TreeViewToggleRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Controls/UserControl", typeof(Platformer.GumRuntimes.Controls.UserControlRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/CautionLines", typeof(Platformer.GumRuntimes.Elements.CautionLinesRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/Divider", typeof(Platformer.GumRuntimes.Elements.DividerRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/DividerHorizontal", typeof(Platformer.GumRuntimes.Elements.DividerHorizontalRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/DividerVertical", typeof(Platformer.GumRuntimes.Elements.DividerVerticalRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/Icon", typeof(Platformer.GumRuntimes.Elements.IconRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/Label", typeof(Platformer.GumRuntimes.Elements.LabelRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/PercentBar", typeof(Platformer.GumRuntimes.Elements.PercentBarRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/PercentBarIcon", typeof(Platformer.GumRuntimes.Elements.PercentBarIconRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Elements/VerticalLines", typeof(Platformer.GumRuntimes.Elements.VerticalLinesRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("GameScreenGum", typeof(Platformer.GumRuntimes.GameScreenGumRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Level1Gum", typeof(Platformer.GumRuntimes.Level1GumRuntime));
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Level2Gum", typeof(Platformer.GumRuntimes.Level2GumRuntime));
            
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Button)] = typeof(Platformer.GumRuntimes.Controls.ButtonStandardRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.CheckBox)] = typeof(Platformer.GumRuntimes.Controls.CheckBoxRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.ComboBox)] = typeof(Platformer.GumRuntimes.Controls.ComboBoxRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Games.DialogBox)] = typeof(Platformer.GumRuntimes.Controls.DialogBoxRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Games.OnScreenKeyboard)] = typeof(Platformer.GumRuntimes.Controls.KeyboardRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.ListBox)] = typeof(Platformer.GumRuntimes.Controls.ListBoxRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.ListBoxItem)] = typeof(Platformer.GumRuntimes.Controls.ListBoxItemRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.PasswordBox)] = typeof(Platformer.GumRuntimes.Controls.PasswordBoxRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Games.PlayerJoinView)] = typeof(Platformer.GumRuntimes.Controls.PlayerJoinViewRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Games.PlayerJoinViewItem)] = typeof(Platformer.GumRuntimes.Controls.PlayerJoinViewItemRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.RadioButton)] = typeof(Platformer.GumRuntimes.Controls.RadioButtonRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.ScrollBar)] = typeof(Platformer.GumRuntimes.Controls.ScrollBarRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.ScrollViewer)] = typeof(Platformer.GumRuntimes.Controls.ScrollViewerRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Games.SettingsView)] = typeof(Platformer.GumRuntimes.Controls.SettingsViewRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Slider)] = typeof(Platformer.GumRuntimes.Controls.SliderRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.TextBox)] = typeof(Platformer.GumRuntimes.Controls.TextBoxRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Popups.Toast)] = typeof(Platformer.GumRuntimes.Controls.ToastRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.TreeView)] = typeof(Platformer.GumRuntimes.Controls.TreeViewRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.TreeViewItem)] = typeof(Platformer.GumRuntimes.Controls.TreeViewItemRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.ToggleButton)] = typeof(Platformer.GumRuntimes.Controls.TreeViewToggleRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.UserControl)] = typeof(Platformer.GumRuntimes.Controls.UserControlRuntime);
            FlatRedBall.Forms.Controls.FrameworkElement.DefaultFormsComponents[typeof(FlatRedBall.Forms.Controls.Label)] = typeof(Platformer.GumRuntimes.Elements.LabelRuntime);
        }
    }
}
