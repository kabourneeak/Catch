using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Catch
{
    public static class RectExtensions
    {
        /// <returns>the center point of the Rect</returns>
        public static Point Center(this Rect r)
        {
            return new Point((r.Left + r.Right) / 2, (r.Bottom + r.Top) / 2);
        }
    }
}
