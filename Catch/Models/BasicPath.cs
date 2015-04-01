using System.Collections.Generic;
using Catch.Base;

namespace Catch.Models
{
    public class BasicPath : List<IHexTile>, IPath
    {
        public string Name { get; set; }
    }
}
