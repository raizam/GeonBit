﻿#region LICENSE
/**
 * For the purpose of making video games only, GeonBit is distributed under the MIT license.
 * to use this source code or GeonBit as a whole for any other purpose, please seek written 
 * permission from the library author.
 * 
 * Copyright (c) 2017 Ronen Ness [ronenness@gmail.com].
 * You may not remove this license notice.
 */
#endregion
#region File Description
//-----------------------------------------------------------------------------
// A simpler renderable model.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GeonBit.Core.Graphics
{

    /// <summary>
    /// A basic renderable model with minimum control over materials and meshes, but with best performance compared to other
    /// model renderer types.
    /// Use this class if you want lots of models of the same type that don't require any special properties.
    /// </summary>
    public class SimpleModelEntity : BaseRenderableEntity
    {
        /// <summary>
        /// Model to render.
        /// </summary>
        public Model Model
        {
            get; protected set;
        }

        /// <summary>
        /// Add bias to distance from camera when sorting by distance from camera.
        /// </summary>
        override public float CameraDistanceBias { get { return _lastRadius * 100f; } }

        // store last rendering radius (based on bounding sphere)
        float _lastRadius = 0f;

        /// <summary>
        /// Blending state of this entity.
        /// </summary>
        public BlendState BlendingState = BlendState.AlphaBlend;

        /// <summary>
        /// Create the model entity from model instance.
        /// </summary>
        /// <param name="model">Model to draw.</param>
        public SimpleModelEntity(Model model)
        {
            Model = model;
        }

        /// <summary>
        /// Create the model entity from asset path.
        /// </summary>
        /// <param name="path">Path of the model to load.</param>
        public SimpleModelEntity(string path) : this(ResourcesManager.Instance.GetModel(path))
        {
        }

        /// <summary>
        /// Draw this model.
        /// </summary>
        /// <param name="worldTransformations">World transformations to apply on this entity (this is what you should use to draw this entity).</param>
        public override void DoEntityDraw(Matrix worldTransformations)
        {
            // reset last radius
            _lastRadius = 0f;
            float scaleLen = worldTransformations.Scale.Length();

            // set blend state
            GraphicsManager.GraphicsDevice.BlendState = BlendingState;

            // iterate model meshes
            foreach (var mesh in Model.Meshes)
            {
                // iterate over mesh effects and apply them (set world matrix etc)
                foreach (var effect in mesh.Effects)
                {
                    Materials.MaterialAPI material = effect.Tag as Materials.MaterialAPI;
                    material.Apply(worldTransformations);
                }

                // update last radius
                _lastRadius = System.Math.Max(_lastRadius, mesh.BoundingSphere.Radius * scaleLen);

                // draw the mesh itself
                mesh.Draw();
            }
        }

        /// <summary>
        /// Prepare material to draw this model.
        /// </summary>
        /// <param name="material">Material to prepare.</param>
        /// <param name="world">World transformations.</param>
        protected void PrepareMaterial(Materials.MaterialAPI material, Matrix world)
        {
            // set world / view / projection matrix of the effect
            material.World = world;
        }

        /// <summary>
        /// Get the bounding sphere of this entity.
        /// </summary>
        /// <param name="parent">Parent node that's currently drawing this entity.</param>
        /// <param name="localTransformations">Local transformations from the direct parent node.</param>
        /// <param name="worldTransformations">World transformations to apply on this entity (this is what you should use to draw this entity).</param>
        /// <returns>Bounding box of the entity.</returns>
        protected override BoundingSphere CalcBoundingSphere(Node parent, Matrix localTransformations, Matrix worldTransformations)
        {
            BoundingSphere modelBoundingSphere = ModelUtils.GetBoundingSphere(Model);
            modelBoundingSphere.Radius *= worldTransformations.Scale.Length();
            modelBoundingSphere.Center = worldTransformations.Translation;
            return modelBoundingSphere;

        }

        /// <summary>
        /// Get the bounding box of this entity.
        /// </summary>
        /// <param name="parent">Parent node that's currently drawing this entity.</param>
        /// <param name="localTransformations">Local transformations from the direct parent node.</param>
        /// <param name="worldTransformations">World transformations to apply on this entity (this is what you should use to draw this entity).</param>
        /// <returns>Bounding box of the entity.</returns>
        protected override BoundingBox CalcBoundingBox(Node parent, Matrix localTransformations, Matrix worldTransformations)
        {
            // get bounding box in local space
            BoundingBox modelBoundingBox = ModelUtils.GetBoundingBox(Model);

            // initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // iterate bounding box corners and transform them
            foreach (Vector3 corner in modelBoundingBox.GetCorners())
            {
                // get curr position and update min / max
                Vector3 currPosition = Vector3.Transform(corner, worldTransformations);
                min = Vector3.Min(min, currPosition);
                max = Vector3.Max(max, currPosition);
            }

            // create and return transformed bounding box
            return new BoundingBox(min, max);
        }
    }
}