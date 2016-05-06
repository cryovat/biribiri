// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Bridge;

namespace BiriBiri
{
    public class Rectangle : IEquatable<Rectangle>
    {
        #region Public Fields

        [FieldProperty]
        public double X { get; set; }

        [FieldProperty]
        public double Y { get; set; }

        [FieldProperty]
        public double Width { get; set; }

        [FieldProperty]
        public double Height { get; set; }

        #endregion

        #region Public Properties

        public static Rectangle Empty
        {
            get { return new Rectangle(); }
        }

        public double Left
        {
            get { return X; }
        }

        public double Right
        {
            get { return (X + Width); }
        }

        public double Top
        {
            get { return Y; }
        }

        public double Bottom
        {
            get { return (Y + Height); }
        }

        public bool IsEmpty
        {
            get
            {
                return ((((Width == 0) && (Height == 0)) && (X == 0)) && (Y == 0));
            }
        }

        #endregion

        #region Constructors

        public Rectangle()
        {
        }

        #endregion

        #region Public Methods

        public bool Contains(double x, double y)
        {
            return (((X <= x && (x < (X + Width))) && (Y <= y)) && (y < (Y + Height)));
        }

        public bool Contains(Vector2 value)
        {
            return ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) && (value.Y < (Y + Height)));
        }

        public bool Contains(Rectangle value)
        {
            return ((((X <= value.X) && ((value.X + value.Width) <= (X + Width))) && (Y <= value.Y)) && ((value.Y + value.Height) <= (Y + Height)));
        }

        public override bool Equals(object obj)
        {
            return (obj is Rectangle) && Equals((Rectangle)obj);
        }

        public bool Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override int GetHashCode()
        {
            return ((int)X ^ (int)Y ^ (int)Width ^ (int)Height);
        }

        public void Inflate(double horizontalAmount, double verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        public bool Intersects(Rectangle value)
        {
            return value.Left < Right &&
                   Left < value.Right &&
                   value.Top < Bottom &&
                   Top < value.Bottom;
        }

        public void Intersects(ref Rectangle value, out bool result)
        {
            result = value.Left < Right &&
                     Left < value.Right &&
                     value.Top < Bottom &&
                     Top < value.Bottom;
        }

        public void Intersect(Rectangle value1, Rectangle value2)
        {
            if (value1.Intersects(value2))
            {
                double right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                double left_side = Math.Max(value1.X, value2.X);
                double top_side = Math.Max(value1.Y, value2.Y);
                double bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);

                X = left_side;
                Y = top_side;
                Width = right_side - left_side;
                Height = bottom_side - top_side;
            }
            else
            {
                X = 0;
                Y = 0;
                Width = 0;
                Height = 0;
            }
        }

        public void Offset(double offsetX, double offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        public void Offset(Vector2 amount)
        {
            X += (double)amount.X;
            Y += (double)amount.Y;
        }

        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        }

        public void Union(Rectangle value1, Rectangle value2)
        {
            double x = Math.Min(value1.X, value2.X);
            double y = Math.Min(value1.Y, value2.Y);

            X = x;
            Y = y;
            Width = Math.Max(value1.Right, value2.Right) - x;
            Height = Math.Max(value1.Bottom, value2.Bottom) - y;
        }

        #endregion
    }
}