using System;
using BiriBiri.Level2.Components;
using BiriBiri.Utils;
using Bridge;
using System.Collections.Generic;
using Bridge.Html5;

namespace BiriBiri.Level2
{
    [FileName("biriBiri.level2.js")]
    public abstract class Scene2D : SceneBase<Entity2D>, IManaged, IUpdatable, IDrawable
    {
        private readonly EntityPool<Entity2D> _pool = new EntityPool<Entity2D>();
        private readonly List<Entity2D> _entities  = new List<Entity2D>();

        private readonly Rectangle _cameraRect  =new Rectangle();

        [FieldProperty]
        public Game Game { get; internal set; }

        [FieldProperty]
        public EntityPresetManager<Entity2D> Presets { get; private set; }

        [FieldProperty]
        public QuadTree<Collidable> Collidables { get; }

        [FieldProperty]
        public QuadTree<DrawableComponentBase2D> Drawables { get; }

        public Scene2D()
        {
            Collidables = new QuadTree<Collidable>();
            Drawables = new QuadTree<DrawableComponentBase2D>();
        }

        public void AddEntity(Entity2D entity)
        {
            entity.Scene = this;

            _entities.Add(entity);

            entity.Setup();
        }

        public void AddEntity(string id, double x, double y)
        {
            var entity = _pool.Get(id);
            entity.Position.X = x;
            entity.Position.Y = y;

            entity.Scene = this;

            _entities.Add(entity);

            entity.Setup();
        }

        internal override void ReceiveGame(Game game)
        {
            Game = game;
            Presets = game.GetPresets2D();

            _pool.PresetManager = Presets;
        }

        public sealed override void Setup()
        {
            Populate();
        }

        public sealed override void Clear()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Teardown();
            }

            _entities.Clear();
        }

        public sealed override void Teardown()
        {
            BeforeTeardown();

            Clear();

            _pool.PresetManager = null;
        }

        public sealed override void Update(double delta)
        {
            BeforeUpdate();

            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Update(delta);
            }

            AfterUpdate();
        }

        public sealed override void Draw()
        {
            BeforeDraw();

            var camera = Game.Camera;

            camera.CopyBoundsTo(_cameraRect);

            Drawables.Query(_cameraRect, h => h.Draw());

            AfterDraw();
        }

        public sealed override void DrawOverlays()
        {
            BeforeDrawOverlays();
            AfterDrawOverlays();
        }
    }
}
