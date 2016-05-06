using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri;
using BiriBiri.Level2;
using BiriBiri.Level2.Components;
using Bridge.Html5;

namespace TestGame.Scenes
{
    public class TitleScene : Scene2D
    {
        protected override void Populate()
        {
            AddEntity("box", 50, 50);
        }
    }
}
