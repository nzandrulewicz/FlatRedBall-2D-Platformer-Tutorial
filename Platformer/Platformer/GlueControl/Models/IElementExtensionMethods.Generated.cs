#define IncludeSetVariable
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

﻿using FlatRedBall.Content.Instructions;
using GlueControl.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatRedBall.IO;
using GlueControl.Managers;

namespace GlueControl.Models
{
    public interface IFileReferencer
    {
        bool UseGlobalContent
        {
            get;
            set;
        }

        ReferencedFileSave GetReferencedFileSave(string name);


        List<ReferencedFileSave> ReferencedFiles
        {
            get;
        }
    }

    public static class IElementExtensionMethods
    {
        public static void FixAllTypes(this GlueElement element)
        {
            foreach (NamedObjectSave nos in element.NamedObjects)
            {
                nos.FixAllTypes();
            }
            //foreach (StateSave state in element.AllStates)
            //{
            //    state.FixAllTypes(element);
            //}
            //foreach (CustomVariable customVariable in element.CustomVariables)
            //{
            //    customVariable.FixAllTypes();
            //}
            //foreach(var file in element.ReferencedFiles)
            //{
            //    file.FixAllTypes();
            //}
        }

        public static void FixAllTypes(this NamedObjectSave instance)
        {
            //var ati = instance.GetAssetTypeInfo();
            foreach (var instruction in instance.InstructionSaves)
            {
                //if (instruction.Type == null)
                //{
                //    var existingVariableDefinition = ati?.VariableDefinitions.FirstOrDefault(item => item.Name == instruction.Member);

                //    instruction.Type = existingVariableDefinition?.Type;
                //}
                FixAllTypes(instruction);
            }

            //foreach (var property in instance.Properties)
            //{
            //    FixAllTypes(property);
            //}

            foreach (NamedObjectSave contained in instance.ContainedObjects)
            {
                contained.FixAllTypes();
            }
        }

        private static void FixAllTypes(InstructionSave instruction)
        {
            //FixEnumerationType(instruction);

            if (!string.IsNullOrEmpty(instruction.Type) && instruction.Value != null)
            {
                object variableValue = instruction.Value;
                var type = instruction.Type;
                variableValue = FixValue(variableValue, type);
                instruction.Value = variableValue;
            }
        }

        public static object FixValue(object variableValue, string type)
        {
            if (type == "int")
            {
                if (variableValue is long asLong)
                {
                    variableValue = (int)asLong;
                }
            }
            else if (type == "int?")
            {
                if (variableValue is long asLong)
                {
                    variableValue = (int?)asLong;
                }
            }
            else if (type == "float" || type == "Single")
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
            else if (type == "decimal" || type == "Decimal")
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
            else if (type == "float?")
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
            else if (type == "List<Vector2>")
            {
                if (variableValue is Newtonsoft.Json.Linq.JArray jArray)
                {
                    List<Vector2> newList = new List<Vector2>();
                    foreach (string innerValue in jArray)
                    {
                        var split = innerValue.Split(',').Select(item => item.Trim()).ToArray();

                        if (split.Length == 2)
                        {
                            var firstValue = float.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                            var secondValue = float.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);

                            newList.Add(new Vector2(firstValue, secondValue));
                        }
                    }
                    variableValue = newList;
                }
            }

            return variableValue;
        }

        public static ReferencedFileSave GetReferencedFileSaveRecursively(this GlueElement instance, string fileName)
        {
            ReferencedFileSave rfs = GetReferencedFileSave(instance, fileName);

            if (rfs == null && !string.IsNullOrEmpty(instance.BaseObject))
            {
                EntitySave baseEntitySave = ObjectFinder.Self.GetEntitySave(instance.BaseObject);
                if (baseEntitySave != null)
                {
                    rfs = baseEntitySave.GetReferencedFileSaveRecursively(fileName);
                }
            }

            return rfs;
        }

        public static ReferencedFileSave GetReferencedFileSave(IFileReferencer instance, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace("\\", "/");
                foreach (ReferencedFileSave rfs in instance.ReferencedFiles)
                {
                    if (rfs.Name?.ToLowerInvariant() == name?.ToLowerInvariant())
                    {
                        return rfs;
                    }
                }
                // didn't find it, so let's try un-qualified
                if (name.Contains('/') == false)
                {
                    foreach (ReferencedFileSave rfs in instance.ReferencedFiles)
                    {
                        var nameNoExtension = FileManager.RemoveExtension(rfs.Name);
                        if (nameNoExtension.EndsWith('/' + name) || nameNoExtension == name)
                        {
                            return rfs;
                        }
                    }
                }
            }
            return null;
        }

        public static ReferencedFileSave GetReferencedFileSave(IFileReferencer instance, FilePath filePath)
        {
            var name = filePath.FullPath;
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace("\\", "/");
                foreach (ReferencedFileSave rfs in instance.ReferencedFiles)
                {
                    if (rfs.Name?.ToLowerInvariant() == name?.ToLowerInvariant())
                    {
                        return rfs;
                    }
                }
                // didn't find it, so let's try un-qualified
                if (name.Contains('/') == false)
                {
                    foreach (ReferencedFileSave rfs in instance.ReferencedFiles)
                    {
                        var nameNoExtension = FileManager.RemoveExtension(rfs.Name);
                        if (nameNoExtension.EndsWith('/' + name) || nameNoExtension == name)
                        {
                            return rfs;
                        }
                    }
                }
            }
            return null;
        }

        public static CustomVariable GetCustomVariableRecursively(this GlueElement element, string variableName)
        {
            //////////////////////Early Out///////////////////////////////////
            if (string.IsNullOrEmpty(variableName))
            {
                return null;
            }

            ////////////////////End Early Out//////////////////////////
            if (variableName.StartsWith("this."))
            {
                variableName = variableName.Substring("this.".Length);
            }
            CustomVariable foundVariable = element.GetCustomVariable(variableName);

            if (foundVariable != null)
            {
                return foundVariable;
            }
            else
            {
                if (!string.IsNullOrEmpty(element.BaseObject))
                {
                    var baseElement = ObjectFinder.Self.GetElement(element.BaseObject);

                    if (baseElement != null)
                    {
                        foundVariable = GetCustomVariableRecursively(baseElement, variableName);
                    }
                }

                return foundVariable;
            }
        }

        /// <summary>
        /// Returns all named objects contained in this object (both single and objects in lists) as well
        /// as all named objects in base elements.
        /// </summary>
        /// <param name="element">Element named object container.</param>
        /// <returns>All named objects.</returns>
        public static IEnumerable<NamedObjectSave> GetAllNamedObjectsRecurisvely(this GlueElement element)
        {
            if (element != null)
            {
                foreach (NamedObjectSave nos in element.AllNamedObjects)
                {
                    yield return nos;
                }

                var allDerived = ObjectFinder.Self.GetAllBaseElementsRecursively(element);

                foreach (var derived in allDerived)
                {
                    foreach (NamedObjectSave nos in derived.AllNamedObjects)
                    {
                        yield return nos;
                    }
                }
            }
        }
    }
}
