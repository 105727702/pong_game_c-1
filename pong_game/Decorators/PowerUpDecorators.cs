using System;

namespace PongGame.Decorators
{
    /// <summary>
    /// Speed boost decorator - increases entity speed
    /// </summary>
    public class SpeedBoostDecorator : EntityDecorator
    {
        private readonly float _speedMultiplier;
        private readonly DateTime _startTime;
        private readonly double _duration;

        public SpeedBoostDecorator(IGameEntity entity, float speedMultiplier, double duration) 
            : base(entity)
        {
            _speedMultiplier = speedMultiplier;
            _duration = duration;
            _startTime = DateTime.Now;
        }

        public override float GetSpeed()
        {
            if (IsActive())
                return _wrappedEntity.GetSpeed() * _speedMultiplier;
            return _wrappedEntity.GetSpeed();
        }

        public bool IsActive()
        {
            return (DateTime.Now - _startTime).TotalSeconds < _duration;
        }
    }

    /// <summary>
    /// Speed reduction decorator - decreases entity speed
    /// </summary>
    public class SpeedReductionDecorator : EntityDecorator
    {
        private readonly float _speedMultiplier;
        private readonly DateTime _startTime;
        private readonly double _duration;

        public SpeedReductionDecorator(IGameEntity entity, float speedMultiplier, double duration) 
            : base(entity)
        {
            _speedMultiplier = speedMultiplier;
            _duration = duration;
            _startTime = DateTime.Now;
        }

        public override float GetSpeed()
        {
            if (IsActive())
                return _wrappedEntity.GetSpeed() * _speedMultiplier;
            return _wrappedEntity.GetSpeed();
        }

        public bool IsActive()
        {
            return (DateTime.Now - _startTime).TotalSeconds < _duration;
        }
    }

    /// <summary>
    /// Size boost decorator - increases entity size
    /// </summary>
    public class SizeBoostDecorator : EntityDecorator
    {
        private readonly float _sizeMultiplier;
        private readonly DateTime _startTime;
        private readonly double _duration;

        public SizeBoostDecorator(IGameEntity entity, float sizeMultiplier, double duration) 
            : base(entity)
        {
            _sizeMultiplier = sizeMultiplier;
            _duration = duration;
            _startTime = DateTime.Now;
        }

        public override float GetSize()
        {
            if (IsActive())
                return _wrappedEntity.GetSize() * _sizeMultiplier;
            return _wrappedEntity.GetSize();
        }

        public bool IsActive()
        {
            return (DateTime.Now - _startTime).TotalSeconds < _duration;
        }
    }
}
