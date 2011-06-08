#region File Description
//-----------------------------------------------------------------------------
// EnvironmentMappedModelProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
#endregion

namespace TerrainPipeline
{
    /// <summary>
    /// Custom content pipeline processor derives from the built-in
    /// ModelProcessor, extending it to apply an environment mapping
    /// effect to the model as part of the build process.
    /// </summary>
    [ContentProcessor]
    public class TankModelProcessor : ModelProcessor
    {
        /// <summary>
        /// Use our custom EnvironmentMappedMaterialProcessor
        /// to convert all the materials on this model.
        /// </summary>
        protected override MaterialContent ConvertMaterial(MaterialContent material,
                                                         ContentProcessorContext context)
        {
            OpaqueDataDictionary processorParameters = new OpaqueDataDictionary();
            processorParameters["ColorKeyColor"] = ColorKeyColor;
            processorParameters["ColorKeyEnabled"] = ColorKeyEnabled;
            processorParameters["TextureFormat"] = TextureFormat;
            processorParameters["GenerateMipmaps"] = GenerateMipmaps;
            processorParameters["ResizeTexturesToPowerOfTwo"] =
                ResizeTexturesToPowerOfTwo;

            return context.Convert<MaterialContent, MaterialContent>(material,
                "TankModelMaterialProcessor", processorParameters);

        }
    }
}
