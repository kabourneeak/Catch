using System.Collections.Generic;

namespace Catch.Base
{
    public class BasicPath : List<IHexTile>, IPath
    {
        public string Name { get; set; }
    }
}
