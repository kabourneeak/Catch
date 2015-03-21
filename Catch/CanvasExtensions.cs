using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Catch
{
    public static class CanvasExtensions
    {
        public static Matrix3x2 CurrentTransform { get { return RelativeTransforms.Peek(); } }

        private static readonly Stack<Matrix3x2> RelativeTransforms = new Stack<Matrix3x2>( new[] { Matrix3x2.Identity} );

        public static Matrix3x2 ResetTransform(Matrix3x2 baseTransform)
        {
            RelativeTransforms.Clear();
            RelativeTransforms.Push(baseTransform);

            return baseTransform;
        }

        public static Matrix3x2 Push(float x, float y)
        {
            return Push(Matrix3x2.CreateTranslation(x, y));
        }

        public static Matrix3x2 Push(Matrix3x2 relativeTransform)
        {
            var newTransform = Matrix3x2.Add(CurrentTransform, relativeTransform);
            RelativeTransforms.Push(newTransform);

            return newTransform;
        }

        public static Matrix3x2 Pop()
        {
            if (RelativeTransforms.Count > 1)
                return RelativeTransforms.Pop();

            throw new InvalidOperationException("You cannot pop the base Transform.");
        }
    }
}
