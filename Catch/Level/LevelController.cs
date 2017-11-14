using System.Numerics;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;
using CatchLibrary.Serialization.Maps;
using Microsoft.Graphics.Canvas;
using Unity;

namespace Catch.Level
{
    /// <summary>
    /// Controls the execution of a level, executing instructions from a map definition
    /// </summary>
    public class LevelController : IScreenController
    {
        private readonly UpdateController _updateController;
        private readonly FieldController _fieldController;
        private readonly OverlayController _overlayController;

        private readonly GraphicsResourceManager _graphicsResourceManager;
        private readonly UpdateEventArgs _updateEventArgs;
        private readonly UiStateModel _uiState;

        #region Construction

        public LevelController(IConfig config, MapSerializationModel mapSerializationModel)
        {
            /*
             * Bootstrap simulation
             */
            var levelContainer = LevelBootstrapper.CreateContainer(config);

            _graphicsResourceManager = levelContainer.Resolve<GraphicsResourceManager>();

            _uiState = levelContainer.Resolve<UiStateModel>();

            _updateController = levelContainer.Resolve<UpdateController>();
            _overlayController = levelContainer.Resolve<OverlayController>();
            _fieldController = levelContainer.Resolve<FieldController>();

            var map = levelContainer.Resolve<MapModel>();
            var mapLoader = levelContainer.Resolve<MapLoader>();
            mapLoader.InitializeMap(map, mapSerializationModel);
            
            /*
             * Other initialization
             */

            var simulationManager = levelContainer.Resolve<ISimulationManager>();
            var simulationStateModel = levelContainer.Resolve<SimulationStateModel>();
            _updateEventArgs = new UpdateEventArgs(simulationManager, simulationStateModel);
        }

        #endregion

        #region IScreenController Implementation

        public void Initialize(Vector2 size)
        {
            _uiState.WindowSize = size;

            _overlayController.Initialize();
            _fieldController.Initialize();
        }

        public bool AllowPredecessorUpdate() => false;

        public bool AllowPredecessorDraw() => false;

        public bool AllowPredecessorInput() => false;

        public void Update(float deviceTicks)
        {
            _updateController.Update(deviceTicks, _updateEventArgs);
            _fieldController.Update(deviceTicks);
            _overlayController.Update(deviceTicks);
        }

        public void Draw(DrawArgs drawArgs)
        {
            // the FieldController draws the field of play; the map, the agents, all the action
            _fieldController.Draw(drawArgs);

            // the overlay draws second so that it is on top
            _overlayController.Draw(drawArgs);
        }

        #endregion

        #region IViewportController Implementation

        public void PanBy(PanByEventArgs eventArgs)
        {
            _fieldController.PanBy(eventArgs);
        }

        public void ZoomToPoint(ZoomToPointEventArgs eventArgs)
        {
            _fieldController.ZoomToPoint(eventArgs);
        }

        public void Resize(Vector2 size)
        {
            _uiState.WindowSize = size;

            _fieldController.Resize(size);
            _overlayController.Resize(size);
        }

        public void Hover(HoverEventArgs eventArgs)
        {
            _overlayController.Hover(eventArgs);

            if (!eventArgs.Handled)
                _fieldController.Hover(eventArgs);
        }

        public void Touch(TouchEventArgs eventArgs)
        {
            _overlayController.Touch(eventArgs);

            if (!eventArgs.Handled)
                _fieldController.Touch(eventArgs);
        }

        public void KeyPress(KeyPressEventArgs eventArgs)
        {
            _overlayController.KeyPress(eventArgs);

            if (!eventArgs.Handled)
                _fieldController.KeyPress(eventArgs);
        }

        #endregion

        #region IGraphicsResource Implementation

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            _graphicsResourceManager.CreateResources(resourceCreator);
        }

        public void DestroyResources()
        {
            _graphicsResourceManager.DestroyResources();
        }

        #endregion
    }
}
