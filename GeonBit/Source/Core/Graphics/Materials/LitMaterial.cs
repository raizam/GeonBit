﻿#region LICENSE
//-----------------------------------------------------------------------------
// For the purpose of making video games, educational projects or gamification,
// GeonBit is distributed under the MIT license and is totally free to use.
// To use this source code or GeonBit as a whole for other purposes, please seek 
// permission from the library author, Ronen Ness.
// 
// Copyright (c) 2017 Ronen Ness [ronenness@gmail.com].
// Do not remove this license notice.
//-----------------------------------------------------------------------------
#endregion
#region File Description
//-----------------------------------------------------------------------------
// A test material that uses MonoGame default effect with default lightings.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace GeonBit.Core.Graphics.Materials
{
    /// <summary>
    /// A material that support ambient + several point / directional lights.
    /// </summary>
    public class LitMaterial : MaterialAPI
    {
        // the effect instance of this material.
        Effect _effect;

        /// <summary>
        /// Get the effect instance.
        /// </summary>
        public override Effect Effect { get { return _effect; } }

        /// <summary>
        /// If true, will use the currently set lights manager in `Graphics.GraphicsManager.LightsManager` and call ApplyLights() with the lights from manager.
        /// </summary>
        protected override bool UseDefaultLightsManager { get { return true; } }

        // effect parameters
        EffectParameterCollection _effectParams;

        // How many lights we can support at the same time. based on effect definition.
        static readonly int MaxLightsCount = 7;

        // cache of lights we applied
        Lights.LightSource[] _lastLights = new Lights.LightSource[MaxLightsCount];

        /// <summary>
        /// Return if this material support dynamic lighting.
        /// </summary>
        override public bool LightingEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Create the lit material from an empty effect.
        /// </summary>
        public LitMaterial() : this(ResourcesManager.Instance.GetEffect(EffectsPath + "LitEffect"))
        {
            _effectParams = _effect.Parameters;
        }

        /// <summary>
        /// Create the material from another material instance.
        /// </summary>
        /// <param name="other">Other material to clone.</param>
        public LitMaterial(LitMaterial other)
        {
            _effect = other._effect;
            _effectParams = _effect.Parameters;
            MaterialAPI asBase = this;
            other.CloneBasics(ref asBase);
        }

        /// <summary>
        /// Create the lit material.
        /// </summary>
        /// <param name="fromEffect">Effect to create material from.</param>
        public LitMaterial(Effect fromEffect)
        {
            _effect = fromEffect;
            _effectParams = _effect.Parameters;
            SetDefaults();
        }

        /// <summary>
        /// Create the lit material.
        /// </summary>
        /// <param name="fromEffect">Effect to create material from.</param>
        /// <param name="copyEffectProperties">If true, will copy initial properties from effect.</param>
        public LitMaterial(BasicEffect fromEffect, bool copyEffectProperties = true)
        {
            // store effect and set default properties
            _effect = fromEffect;
            _effectParams = _effect.Parameters;
            SetDefaults();

            // copy properties from effect itself
            if (copyEffectProperties)
            {
                // set effect defaults
                Texture = fromEffect.Texture;
                TextureEnabled = fromEffect.TextureEnabled;
                Alpha = fromEffect.Alpha;
                AmbientLight = new Color(fromEffect.AmbientLightColor.X, fromEffect.AmbientLightColor.Y, fromEffect.AmbientLightColor.Z);
                DiffuseColor = new Color(fromEffect.DiffuseColor.X, fromEffect.DiffuseColor.Y, fromEffect.DiffuseColor.Z);
                SpecularColor = new Color(fromEffect.SpecularColor.X, fromEffect.SpecularColor.Y, fromEffect.SpecularColor.Z);
                SpecularPower = fromEffect.SpecularPower;
            }
        }

        /// <summary>
        /// Apply this material.
        /// </summary>
        override protected void MaterialSpecificApply(bool wasLastMaterial)
        {
            // set world matrix
            _effectParams["WorldViewProjection"].SetValue(World * ViewProjection);

            // if it was last material used, stop here - no need for the following settings
            if (wasLastMaterial) { return; }

            // set all effect params
            if (IsDirty(MaterialDirtyFlags.TextureParams))
            {
                _effectParams["MainTexture"].SetValue(Texture);
            }
            if (IsDirty(MaterialDirtyFlags.Alpha))
            {
                _effectParams["Alpha"].SetValue(Alpha);
            }
            if (IsDirty(MaterialDirtyFlags.MaterialColors))
            {
                _effectParams["DiffuseColor"].SetValue(DiffuseColor.ToVector3());
            }
            if (IsDirty(MaterialDirtyFlags.LightSources))
            {
                _effectParams["AmbientColor"].SetValue(AmbientLight.ToVector3());
            }
        }

        /// <summary>
        /// Update material view matrix.
        /// </summary>
        /// <param name="view">New view to set.</param>
        override protected void UpdateView(ref Matrix view)
        {
        }

        /// <summary>
        /// Update material projection matrix.
        /// </summary>
        /// <param name="projection">New projection to set.</param>
        override protected void UpdateProjection(ref Matrix projection)
        {
        }

        /// <summary>
        /// Apply light sources on this material.
        /// </summary>
        /// <param name="lights">Array of light sources to apply.</param>
        /// <param name="worldMatrix">World transforms of the rendering object.</param>
        /// <param name="boundingSphere">Bounding sphere (after world transformation applied) of the rendering object.</param>
        override protected void ApplyLights(Lights.LightSource[] lights, ref Matrix worldMatrix, ref BoundingSphere boundingSphere)
        {
            // iterate on lights and apply only the changed ones
            int lightsCount = Math.Min(MaxLightsCount, lights.Length);
            for (int i = 0; i < lightsCount; ++i)
            {
                // only if light changed
                if (_lastLights[i] != lights[i])
                {
                    // get light data as floats buffer
                    float[] data = lights[i].GetFloatBuffer();

                    // set light's data
                    _effectParams["LightSorce"].SetValue(data);

                    // store light in cache so we won't copy it next time if it haven't changed
                    _lastLights[i] = lights[i];
                }
            }
        }

        /// <summary>
        /// Clone this material.
        /// </summary>
        /// <returns>Copy of this material.</returns>
        public override MaterialAPI Clone()
        {
            return new LitMaterial(this);
        }
    }
}