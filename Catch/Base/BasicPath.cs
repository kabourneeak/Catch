using System.Collections.Generic;
using Catch.Models;

namespace Catch.Base
{
    public class BasicPath : List<IHexTile>, IPath
    {
        public string Name { get; set; }
    }
}
