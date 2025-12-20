// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GodotExtensions.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 4:16:39 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace Godot;

using nGratis.AI.Odin.Engine;

public static class GodotExtensions
{
    extension(Node node)
    {
        public T FindAncestor<T>() where T : Node
        {
            var parentNode = node.GetParent();

            while (parentNode is not null)
            {
                if (parentNode is T targetNode)
                {
                    return targetNode;
                }

                parentNode = parentNode.GetParent();
            }

            throw new OdinException(
                "Ancestor node with target type is not found!",
                ("Target Type", typeof(T).FullName));
        }
    }
}