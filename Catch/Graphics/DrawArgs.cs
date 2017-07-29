using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Graphics.Canvas;

namespace Catch.Graphics
{
    public class DrawArgs
    {
        private readonly Stack<Matrix3x2> _transforms;

        public DrawArgs(CanvasDrawingSession ds, Matrix3x2 baseTransform, int frameId)
        {
            Ds = ds;
            FrameId = frameId;
            LevelOfDetail = DrawLevelOfDetail.Normal;

            _transforms = new Stack<Matrix3x2>();
            _transforms.Push(baseTransform);

            Ds.Transform = CurrentTransform;
        }

        public int FrameId { get; private set; }

        public DrawLevelOfDetail LevelOfDetail { get; set; }

        public CanvasDrawingSession Ds { get; private set; }

        public Matrix3x2 CurrentTransform { get { return _transforms.Peek(); } }
        
        public Matrix3x2 PushTranslation(float x, float y)
        {
            return Push(Matrix3x2.CreateTranslation(x, y));
        }

        public Matrix3x2 PushTranslation(Vector2 offset)
        {
            return Push(Matrix3x2.CreateTranslation(offset));
        }

        public Matrix3x2 PushRotation(float radians)
        {
            return Push(Matrix3x2.CreateRotation(radians));
        }

        public Matrix3x2 PushRotation(float radians, Vector2 center)
        {
            return Push(Matrix3x2.CreateRotation(radians, center));
        }

        public Matrix3x2 PushScale(float xyScale)
        {
            return PushScale(xyScale, xyScale);
        }

        public Matrix3x2 PushScale(float xScale, float yScale)
        {
            return Push(Matrix3x2.CreateScale(xScale, yScale));
        }

        public Matrix3x2 Push(Matrix3x2 relativeTransform)
        {
            var newTransform = Matrix3x2.Multiply(relativeTransform, CurrentTransform);
            _transforms.Push(newTransform);

            Ds.Transform = CurrentTransform;

            return newTransform;
        }

        public Matrix3x2 Pop()
        {
            if (_transforms.Count > 1)
            {
                var oldTop = _transforms.Pop();

                Ds.Transform = CurrentTransform;

                return oldTop;
            }

            throw new InvalidOperationException("You cannot pop the base Transform.");
        }
    }
}
