using Microsoft.Graphics.Canvas;

namespace Catch.Graphics
{
    public class CreateResourcesArgs
    {
        public CreateResourcesArgs(ICanvasResourceCreator resourceCreator, int frameId, bool mandatory)
        {
            FrameId = frameId;
            IsMandatory = mandatory;
            ResourceCreator = resourceCreator;
        }

        public int FrameId { get; private set; }

        public bool IsMandatory { get; private set; }

        public ICanvasResourceCreator ResourceCreator { get; private set; }

        public void SetMandatory()
        {
            IsMandatory = true;
        }
    }
}
