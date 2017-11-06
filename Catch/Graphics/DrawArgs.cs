using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Graphics.Canvas;

namespace Catch.Graphics
{
    public class DrawArgs
    {
        private readonly Stack<Matrix3x2> _transforms;
        private readonly CanvasDrawingSession _ds;
        private bool _transformChanged;

        public DrawArgs(CanvasDrawingSession ds, Matrix3x2 baseTransform, int frameId)
        {
            _ds = ds;
            FrameId = frameId;
            LevelOfDetail = DrawLevelOfDetail.Normal;

            _transforms = new Stack<Matrix3x2>();
            _transforms.Push(baseTransform);
            _transformChanged = true;
        }

        public int FrameId { get; }

        /// <summary>
        /// The layer which should be drawn. Complex Drawables with elements on different layers should
        /// draw only the components of the specified layer
        /// </summary>
        public DrawLayer Layer { get; set; }

        /// <summary>
        /// The Level Of Detail which should be drawn. Complex Drawables which should appear with less 
        /// complexity when zoomed out should draw only the components of the specified LoD.
        /// </summary>
        public DrawLevelOfDetail LevelOfDetail { get; set; }

        public ICanvasResourceCreator ResourceCreator => _ds.Device;

        public CanvasDrawingSession Ds
        {
            get
            {
                if (_transformChanged)
                {
                    // Setting the transform is expensive, so only do it if
                    // it might actually be used
                    _ds.Transform = CurrentTransform;
                    _transformChanged = false;
                }

                return _ds;
            }
        }

        public Matrix3x2 CurrentTransform => _transforms.Peek();

        public void PushTranslation(float x, float y) => Push(Matrix3x2.CreateTranslation(x, y));

        public void PushTranslation(Vector2 offset) => Push(Matrix3x2.CreateTranslation(offset));

        public void PushRotation(float radians) => Push(Matrix3x2.CreateRotation(radians));

        public void PushRotation(float radians, Vector2 center) => Push(Matrix3x2.CreateRotation(radians, center));

        public void PushScale(float xyScale) => PushScale(xyScale, xyScale);

        public void PushScale(float xScale, float yScale) => Push(Matrix3x2.CreateScale(xScale, yScale));

        public void Push(Matrix3x2 relativeTransform)
        {
            var newTransform = Matrix3x2.Multiply(relativeTransform, CurrentTransform);
            _transforms.Push(newTransform);
            _transformChanged = true;
        }

        public void Pop()
        {
            if (_transforms.Count == 0)
                throw new InvalidOperationException("You cannot pop the base Transform.");

            _transforms.Pop();
            _transformChanged = true;
        }
    }
}
