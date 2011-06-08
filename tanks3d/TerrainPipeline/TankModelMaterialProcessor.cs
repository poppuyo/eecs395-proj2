#region File Description
//-----------------------------------------------------------------------------
// EnvironmentMappedMaterialProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.IO;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
#endregion

namespace TerrainPipeline
{
    /// <summary>
    /// Custom content pipeline processor derives from the built-in
    /// MaterialProcessor. This changes the material to use our custom
    /// environment mapping effect, and also builds the environment map
    /// texture in a special way.
    /// </summary>
    [ContentProcessor]
    public class TankModelMaterialProcessor : MaterialProcessor
    {
        /// <summary>
        /// Converts a material.
        /// </summary>
        public override MaterialContent Process(MaterialContent input,
                                                ContentProcessorContext context)
        {
            // Create a new effect material.
            EffectMaterialContent customMaterial = new EffectMaterialContent();

            // Point the new material at our custom effect file.
            string effectFile = Path.GetFullPath("tankshader.fx");

            customMaterial.Effect = new ExternalReference<EffectContent>(effectFile);

            // Copy texture data across from the original material.
            BasicMaterialContent basicMaterial = (BasicMaterialContent)input;

            if (basicMaterial.Texture != null)
            {
                customMaterial.Textures.Add("Texture", basicMaterial.Texture);
                customMaterial.OpaqueData.Add("TextureEnabled", true);
            }

            // Chain to the base material processor.
            return base.Process(customMaterial, context);
        }
    }
}
