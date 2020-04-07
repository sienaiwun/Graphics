﻿using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

using RenderQueueType = UnityEngine.Rendering.HighDefinition.HDRenderQueue.RenderQueueType;

namespace UnityEditor.Rendering.HighDefinition.ShaderGraph
{
    class HDSystemData : HDTargetData
    {
        [SerializeField]
        SurfaceType m_SurfaceType = SurfaceType.Opaque;
        public SurfaceType surfaceType
        {
            get => m_SurfaceType;
            set => m_SurfaceType = value;
        }

        [SerializeField]
        BlendMode m_BlendMode = BlendMode.Alpha;
        public BlendMode blendMode
        {
            get => m_BlendMode;
            set => m_BlendMode = value;
        }

        [SerializeField]
        RenderQueueType m_RenderingPass = RenderQueueType.Opaque;
        public RenderQueueType renderingPass
        {
            get => m_RenderingPass;
            set => m_RenderingPass = value;
        }

        [SerializeField]
        bool m_ZWrite = true;
        public bool zWrite
        {
            get => m_ZWrite;
            set => m_ZWrite = value;
        }

        [SerializeField]
        TransparentCullMode m_TransparentCullMode = TransparentCullMode.Back;
        public TransparentCullMode transparentCullMode
        {
            get => m_TransparentCullMode;
            set => m_TransparentCullMode = value;
        }

        [SerializeField]
        CompareFunction m_ZTest = CompareFunction.LessEqual;
        public CompareFunction zTest
        {
            get => m_ZTest;
            set => m_ZTest = value;
        }

        [SerializeField]
        int m_SortPriority;
        public int sortPriority
        {
            get => m_SortPriority;
            set => m_SortPriority = value;
        }

        [SerializeField]
        bool m_AlphaTest;
        public bool alphaTest
        {
            get => m_AlphaTest;
            set => m_AlphaTest = value;
        }

        // TODO: HDUnlit doesnt support DoubleSidedMode, presumably because normals are irrelevant
        // TODO: Can we share a DoubleSidedMode property anyway to simplify things?
        [SerializeField]
        bool m_DoubleSided;
        public bool doubleSided
        {
            get => m_DoubleSided;
            set => m_DoubleSided = value;
        }
    }

    static class HDSystemDataExtensions
    {
        public static bool TryChangeRenderingPass(this HDSystemData systemData, HDRenderQueue.RenderQueueType value)
        {
            // Catch invalid rendering pass
            switch (value)
            {
                case HDRenderQueue.RenderQueueType.Overlay:
                case HDRenderQueue.RenderQueueType.Unknown:
                case HDRenderQueue.RenderQueueType.Background:
                    throw new ArgumentException("Unexpected kind of RenderQueue, was " + value);
            };

            // Update for SurfaceType
            switch (systemData.surfaceType)
            {
                case SurfaceType.Opaque:
                    value = HDRenderQueue.GetOpaqueEquivalent(value);
                    break;
                case SurfaceType.Transparent:
                    value = HDRenderQueue.GetTransparentEquivalent(value);
                    break;
                default:
                    throw new ArgumentException("Unknown SurfaceType");
            }

            if (Equals(systemData.renderingPass, value))
                return false;

            systemData.renderingPass = value;
            return true;
        }
    }
}
