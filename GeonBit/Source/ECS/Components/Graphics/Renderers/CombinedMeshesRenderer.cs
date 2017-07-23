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
// A component that renders a collection of meshes combined together into a static mesh.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.Core.Graphics;
using GeonBit.Core.Graphics.Materials;

namespace GeonBit.ECS.Components.Graphics
{
    /// <summary>
    /// This component combine together several meshes into a single static mesh.
    /// It reduceses draw calls and optimize renderings.
    /// Use this for stuff like building rooms, levels, etc. Anything made of multiple static meshes.
    /// </summary>
    public class CombinedMeshesRenderer : BaseRendererComponent
    {
        /// <summary>
        /// The entity from the core layer used to draw the model.
        /// </summary>
        protected CombinedMeshesEntity _entity;

        /// <summary>
        /// Get the main entity instance of this renderer.
        /// </summary>
        protected override BaseRenderableEntity Entity { get { return _entity; } }

        /// <summary>
        /// Add a model to the combined mesh.
        /// </summary>
        /// <param name="model">Model to add.</param>
        /// <param name="transform">World transformations.</param>
        /// <param name="material">Optional material to use instead of the model default materials.</param>
        public void AddModel(Model model, Matrix transform, MaterialAPI material = null)
        {
            _entity.AddModel(model, transform, material);
            _GameObject.SceneNode.ForceFullUpdate(false);
        }

        /// <summary>
        /// Add a model mesh to the combined mesh.
        /// </summary>
        /// <param name="mesh">Mesh to add.</param>
        /// <param name="transform">World transformations.</param>
        /// <param name="material">Optional material to use instead of the mesh default materials.</param>
        public void AddModelMesh(ModelMesh mesh, Matrix transform, MaterialAPI material = null)
        {
            _entity.AddModelMesh(mesh, transform, material);
            _GameObject.SceneNode.ForceFullUpdate(false);
        }

        /// <summary>
        /// Clear everything from the combined meshes renderer.
        /// </summary>
        public void Clear()
        {
            _entity.Clear();
            _GameObject.SceneNode.ForceFullUpdate(false);
        }

        /// <summary>
        /// Create the Combined Meshes Renderer component.
        /// </summary>
        public CombinedMeshesRenderer()
        {
            _entity = new CombinedMeshesEntity();
        }

        /// <summary>
        /// Clone this component.
        /// </summary>
        /// <returns>Cloned copy of this component.</returns>
        override public BaseComponent Clone()
        {
            throw new Exceptions.InvalidActionException("Cannot clone a combined meshes renderer!");
        }
    }
}
