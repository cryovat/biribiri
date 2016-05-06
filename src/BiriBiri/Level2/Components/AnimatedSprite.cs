using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri.Graphics;
using Bridge;

namespace BiriBiri.Level2.Components
{
    [FileName("biriBiri.level2.js")]
    public class AnimatedSprite : DrawableComponentBase2D
    {
        private AnimationSequence _currentSequence;
        private string _sequenceId;
        private double _timer;
        private bool _initialized;
        private int _currentFrame;

        public string SequenceId
        {
            get { return _sequenceId; }
            set
            {
                if (_sequenceId == value) return;

                _sequenceId = value;

                if (_initialized)
                {
                    StartSequence();
                }
            }
        }

        public bool IsPlaying { get; private set; }

        public override void Setup()
        {
            _initialized = true;

            if (!string.IsNullOrEmpty(_sequenceId))
            {
                _currentSequence = Game.Sequences.Get(SequenceId);
                _timer = 0;
            }

            base.Setup();
        }

        public override void Teardown()
        {
            _initialized = false;
        }

        public override void Update(double delta)
        {
            if (_currentSequence == null) return;

            _timer -= delta;

            if (_timer <= 0)
            {
                if (_currentFrame + 1 < _currentSequence.FrameIndices.Length)
                {
                    _currentFrame++;
                    _timer = _currentSequence.Intervals[_currentFrame];
                }
                else if (_currentSequence.Loop)
                {
                    _currentFrame = 0;
                    _timer = _currentSequence.Intervals[0];
                }
            }
        }

        public override void Draw()
        {
            if (_currentSequence == null) return;

            var frameIndex = _currentSequence.FrameIndices[_currentFrame];

            Game.DrawFrame(_currentSequence.TextureId, Owner.Position, Owner.Scale, frameIndex);
        }

        private void StartSequence()
        {
            _currentSequence = Game.Sequences.Get(_sequenceId);
            _timer = _currentSequence.Intervals[0];

            _currentFrame = 0;
        }

        public override ComponentBase Copy()
        {
            throw new NotImplementedException();
        }

        public override bool IsAt(Rectangle rect)
        {
            return rect.Contains(Owner.Position);
        }

        public override bool IsAt(Vector2 point)
        {
            return false;
        }

        public override bool IsContainedBy(Rectangle rect)
        {
            return rect.Contains(Owner.Position);
        }
    }
}
