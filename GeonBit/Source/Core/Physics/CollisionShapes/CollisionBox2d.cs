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
// Collision shape for a 2d box.
//
// Author: Ronen Ness.
// Since: 2017.
//-----------------------------------------------------------------------------
#endregion
using Microsoft.Xna.Framework;

namespace GeonBit.Core.Physics.CollisionShapes
{
    /// <summary>
    /// 2d Box collision shape.
    /// </summary>
    public class CollisionBox2D : ICollisionShape
    {
        /// <summary>
        /// Create the collision box2d.
        /// </summary>
        /// <param name="width">Box base width (X axis).</param>
        /// <param name="height">Box base height (Y axis).</param>
        /// <param name="depth">Bow base depth (Z axis).</param>
        public CollisionBox2D(float width = 1f, float height = 1f, float depth = 1f)
        {
            _shape = new BulletSharp.Box2DShape(width / 2f, height / 2f, depth / 2f);
        }
    }
}