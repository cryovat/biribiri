using BiriBiri.Utils;
using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public abstract class SceneBase : IManaged, IUpdatable, IDrawable
    {
        internal SceneBase()
        {
        }

        internal abstract void ReceiveGame(Game game);

        public virtual void Load()
        {

        }

        public abstract void Setup();

        protected abstract void Populate();

        public abstract void Clear();

        public abstract void Teardown();

        protected virtual void BeforeTeardown()
        {
        }

        public abstract void Update(double delta);

        public virtual void BeforeUpdate()
        {

        }

        public virtual void AfterUpdate()
        {

        }

        public abstract void Draw();

        public virtual void BeforeDraw()
        {

        }

        public virtual void AfterDraw()
        {

        }

        public abstract void DrawOverlays();

        public virtual void BeforeDrawOverlays()
        {

        }

        public virtual void AfterDrawOverlays()
        {

        }
    }

    public abstract class SceneBase<TEntity> : SceneBase where TEntity : EntityBase, IClonable<TEntity>, new()
    {
        private readonly EntityPool<TEntity> _pool = new EntityPool<TEntity>();
    }
}
