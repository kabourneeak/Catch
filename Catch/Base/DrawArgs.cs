using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Graphics.Canvas;

namespace Catch.Base
{
    public class DrawArgs
    {
        private readonly Stack<Matrix3x2> _transforms;

        public DrawArgs(CanvasDrawingSession ds, Matrix3x2 baseTransform)
        {
            Ds = ds;
            _transforms = new Stack<Matrix3x2>();
            _transforms.Push(baseTransform);

            Ds.Transform = CurrentTransform;
        }

        public CanvasDrawingSession Ds { get; private set; }

        public Matrix3x2 CurrentTransform { get { return _transforms.Peek(); } }
        
        public Matrix3x2 PushTranslation(float x, float y)
        {
            return PushTranslation(Matrix3x2.CreateTranslation(x, y));
        }

        public Matrix3x2 PushTranslation(Matrix3x2 relativeTransform)
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
