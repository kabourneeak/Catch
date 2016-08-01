using MapWriter.Serialization;

namespace MapWriter.Maps
{
    public class MapOne : MapModel
    {
        public MapOne()
        {
            Rows = 20;
            Columns = 30;

            AddTiles();
            AddPaths();
            AddScript();
        }

        private void AddTiles()
        {
            InitializeTiles("VoidTower");
        }

        private void AddPaths()
        {
            var path1 = new PathModel();
            {
                path1.PathName = nameof(path1);

                path1.PathSteps.Add(new PathStepModel(0, 0));
                path1.PathSteps.Add(new PathStepModel(1, 0));
                path1.PathSteps.Add(new PathStepModel(2, 0));
                path1.PathSteps.Add(new PathStepModel(3, 0));

                Paths.Add(path1);
            }
        }

        private void AddScript()
        {
            EmitScript.Add(new EmitScriptEntryModel
            {
                BeginTime = 120,
                Count = 10,
                DelayTime = 60,
                PathName = "path1",
                AgentTypeName = "BlockMob"
            });
        }
    }
}
