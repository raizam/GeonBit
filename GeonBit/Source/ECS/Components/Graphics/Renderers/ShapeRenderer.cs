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
// A component that renders a 3D shape.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeonBit.ECS.Components.Graphics
{
    /// <summary>
    /// Possible shapes we can draw.
    /// </summary>
    public enum ShapeMeshes
    {
        /// <summary>Low-poly sphere shape.</summary>
        SphereLowPoly,

        /// <summary>Sphere shape.</summary>
        Sphere,

        /// <summary>Cube shape.</summary>
        Cube,

        /// <summary>Cylinder shape.</summary>
        Cylinder,

        /// <summary>Cone shape.</summary>
        Cone,

        /// <summary>Plane shape.</summary>
        Plane,
    }

    /// <summary>
    /// This component renders a 3D shape from a collection of predefined meshes.
    /// </summary>
    public class ShapeRenderer : ModelRenderer
    {
        /// <summary>
        /// Path of the shape models folder.
        /// </summary>
        public static readonly string ShapeModelsRoot = "GeonBit.Core/BasicMeshes/";

        /// <summary>
        /// Create the model renderer component.
        /// </summary>
        /// <param name="shape">Shape to draw.</param>
        public ShapeRenderer(ShapeMeshes shape) : base(ShapeModelsRoot + shape.ToString())
        {
        }
    }
}